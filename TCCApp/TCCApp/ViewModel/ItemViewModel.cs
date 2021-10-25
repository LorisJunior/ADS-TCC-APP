using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using TCCApp.Model;
using TCCApp.Services;
using TCCApp.View;
using TCCApp.ViewModel;
using Xamarin.Forms;

[assembly:Dependency(typeof(ItemViewModel))]
namespace TCCApp.ViewModel
{
    public class ItemViewModel : BaseViewModel
    {
        SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        public IDisposable Subscription { get; set; }
        public ObservableCollection<Item> Items { get; private set; }
        public bool IsDeleting { get; set; }

        private double deleteButtonOpacity;

        public double DeleteButtonOpacity
        {
            get { return deleteButtonOpacity; }
            set => Set(ref deleteButtonOpacity, value);
        }

        private SelectionMode itemSelectionMode;
        public SelectionMode ItemSelectionMode
        {
            get { return itemSelectionMode; }
            set => Set(ref itemSelectionMode, value);
        }

        public ItemViewModel()
        {
            ItemSelectionMode = SelectionMode.None;
            DeleteButtonOpacity = 0.3;
            IsDeleting = false;
            Items = new ObservableCollection<Item>();
        }
        public async void InitSubscription()
        {
            await semaphoreSlim.WaitAsync();

            var observable = DatabaseService
                .firebase
                .Child("Item")
                .Child(App.user.Key).AsObservable<Item>();

            Subscription = observable
            .Where(f => !string.IsNullOrEmpty(f.Key))
            .Subscribe(f =>
            {
                if (!IsDeleting)
                {
                    var item = new Item
                    {
                        Key = f.Key,
                        Tipo = f.Object.Tipo,
                        Quantidade = f.Object.Quantidade,
                        Descricao = f.Object.Descricao,
                        Cor = Color.FromHsla(f.Object.Hue, 0.73, 0.85, 1),
                        ImageUrl = ImageSource.FromStream(() => new MemoryStream(f.Object.ByteImage))
                    };
                    Items.Add(item);
                }
            });

            semaphoreSlim.Release();
        }
       
        public ICommand DeleteItem => new Command(async s =>
        {
            await semaphoreSlim.WaitAsync();
            IsDeleting = true;
            CollectionView collectionView = s as CollectionView;
            var items = collectionView.SelectedItems;

            if (items != null)
            {
                foreach (Item item in items)
                {
                    Items.Remove(item);
                    await DatabaseService.DeleteItem(item.Key);
                }
                
            }
            DeleteButtonOpacity = 0.3;
            ItemSelectionMode = SelectionMode.None;
            IsDeleting = false;
            semaphoreSlim.Release();
        });
        public ICommand GoToAddItemPage => new Command(async() =>
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddItemPage());
        });
        public ICommand SelectMultiple => new Command(async () =>
        {
            if (Items.Count > 0)
            {
                //Chatlist em modo de delete
                IsDeleting = true;
                DeleteButtonOpacity = 1;
                ItemSelectionMode = SelectionMode.Multiple;
                await Application.Current.MainPage.DisplayAlert("Modo delete ativado", "Selecione os itens que deseja deletar", "ok");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Você não itens", "Você não tem itens para deletar", "ok");
            }
        });
    }
}

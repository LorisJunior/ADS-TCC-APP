using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using TCCApp.Model;
using TCCApp.Services;
using TCCApp.ViewModel;
using Xamarin.Forms;

[assembly:Dependency(typeof(AddItemViewModel))]
namespace TCCApp.ViewModel
{
    public class AddItemViewModel : BaseViewModel
    {
        private ImageSource itemImage;
        public ImageSource ItemImage
        {
            get { return itemImage; }
            set => Set(ref itemImage, value);
        }

        private double itemMargin;
        public double ItemMargin
        {
            get { return itemMargin; }
            set => Set(ref itemMargin, value);
        }

        private string nome;
        public string Nome
        {
            get { return nome; }
            set => Set(ref nome, value);
        }

        private double quantidade;
        public double Quantidade
        {
            get { return quantidade; }
            set => Set(ref quantidade, value);
        }

        private string descricao;
        public string Descricao
        {
            get { return descricao; }
            set => Set(ref descricao, value);
        }

        private Color iconColor;
        public Color IconColor
        {
            get { return iconColor; }
            set => Set(ref iconColor, value);
        }

        private double hue;
        public double Hue
        {
            get { return hue; }
            set
            {
                Set(ref hue, value);
                UpdateColor();
            }
        }

        ItemViewModel itemViewModel;
        public AddItemViewModel()
        {
            itemViewModel = DependencyService.Get<ItemViewModel>();
            ItemImage = "shorts.png";
            ItemMargin = 15;
            Hue = 0;
        }
        public ICommand CreateItem => new Command(async() =>
        {
            Item item = new Item
            {
                Nome = nome,
                Quantidade = quantidade,
                Descricao = descricao,
                Cor = iconColor,
                ImageUrl = itemImage
            };

            itemViewModel.Items.Add(item);

            await Application.Current.MainPage.DisplayAlert("Sucesso!", "O item foi criado com sucesso", "ok");

            await Application.Current.MainPage.Navigation.PopAsync();
        });
        public ICommand SelectIcon => new Command(s =>
        {
            try
            {
                CollectionView collectionView = s as CollectionView;
                Icone icone = (Icone)collectionView.SelectedItem;

                if (icone != null)
                {
                    //TODO
                    //Serve para adicionaro ao banco de dados => icone.ByteIcon
                    ItemImage = icone.IconImage;
                    ItemMargin = 15;
                }

            }
            catch (Exception)
            {
            }
        });
        public ICommand GetItemImage => new Command(async() =>
        {
            try
            {
                var media = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions 
                {
                    PhotoSize = PhotoSize.Medium,
                    CompressionQuality = 90
                });
                var stream = media.GetStream();

                ItemImage = ImageSource.FromStream(() => new MemoryStream(ImageService.ConvertToByte(stream)));
                ItemMargin = 0;
            }
            catch (Exception)
            {
            }
        });
        public void UpdateColor()
        {
            IconColor = Color.FromHsla(hue, 0.73, 0.85, 1);
        }
    }
}

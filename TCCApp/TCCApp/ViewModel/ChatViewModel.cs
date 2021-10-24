using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using TCCApp.Model;
using TCCApp.Services;
using TCCApp.ViewModel;
using Xamarin.Forms;

[assembly:Dependency(typeof(ChatViewModel))]
namespace TCCApp.ViewModel
{
    public class ChatViewModel : BaseViewModel
    {
        public ObservableCollection<Message> Messages { get; private set; }
        public string Author { get; set; }
        public string GroupKey { get; set; }
        public IDisposable Subscription { get; set; }

        SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        int messagesFromOtherUsers = 0;

        int subscriptionMessages = 0;

        private string content;

        public ChatViewModel()
        {
            Messages = new ObservableCollection<Message>();
        }
        public async void InitMessages()
        {
            await semaphoreSlim.WaitAsync();

            try
            {
                var msgs = await DatabaseService.GetMessages(GroupKey);

                if (msgs != null)
                {
                    foreach (var msg in msgs)
                    {
                        if (!string.IsNullOrEmpty(msg.Content))
                        {
                            Messages.Add(msg);

                            if (msg.UserKey != App.user.Key)
                            {
                                messagesFromOtherUsers++;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            semaphoreSlim.Release();
        }
        public async void InitSubscription()
        {
            await semaphoreSlim.WaitAsync();

            var observable = DatabaseService.firebase.Child("Chat").Child(GroupKey).AsObservable<InboundMessage>();

            Subscription = observable
            .Where(f => !string.IsNullOrEmpty(f.Key)
            && f.Object?.UserKey != App.user.Key
            && !string.IsNullOrEmpty(f.Object.Content)
            && f.Object.Content != "")
            .Subscribe(f =>
            {
                var message = new InboundMessage
                {
                    Author = f.Object.Author,
                    Content = f.Object.Content,
                    UserKey = f.Object.UserKey
                };
                if (CanAddMessage())
                {
                    Messages.Add(message);
                }
            });

            semaphoreSlim.Release();
        }
        private bool CanAddMessage()
        {
            //ESSE MÉTODO É UTILIZADO PARA EVITAR MENSAGENS DUPLICADAS NA LISTA MESSAGES
            if (subscriptionMessages < messagesFromOtherUsers)
            {
                subscriptionMessages++;
                return false;
            }
            else
            {
                return true;
            }
        }
        public string Content
        {
            get { return content; }
            set => Set(ref content, value);
        }
        public ICommand Send => new Command(async () =>
        {
            var message = new OutboundMessage()
            {
                Author = Author,
                Content = this.content,
                UserKey = App.user.Key
            };

            if (!string.IsNullOrEmpty(message.Content))
            {
                Messages.Add(message);

                await DatabaseService.AddMessage(message, GroupKey);
            }
            

            Content = string.Empty;
        });
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TCCApp.Model;
using TCCApp.ViewModel;
using Xamarin.Forms;

[assembly:Dependency(typeof(ChatListViewModel))]
namespace TCCApp.ViewModel
{
    public class ChatListViewModel : BaseViewModel
    {
        public ObservableCollection<ChatList> Chats { get; set; }

        public ChatListViewModel()
        {
            Chats = new ObservableCollection<ChatList>
            {
                new ChatList
                {
                    Author = "Lucia",
                    GroupKey = "Conversa1"
                },
                new ChatList
                {
                    Author = "Junior",
                    GroupKey = "Conversa2"
                },
                new ChatList
                {
                    Author = "Ronaldo",
                    GroupKey = "Conversa3"
                },
            };
        }
    }
}

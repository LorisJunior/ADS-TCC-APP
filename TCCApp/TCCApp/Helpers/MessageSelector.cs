using TCCApp.Model;
using TCCApp.ViewModel;
using Xamarin.Forms;

namespace TCCApp.Helpers
{
    public class MessageSelector : DataTemplateSelector
    {
        public DataTemplate SimpleTextSelector { get; set; }
        public DataTemplate InboundTextSelector { get; set; }

        
        ChatViewModel chatViewModel;

        public MessageSelector()
        {
            chatViewModel = DependencyService.Get<ChatViewModel>();
        }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            //if (((Message)item).Author == chatViewModel.Author)
            if (((Message)item).UserKey == App.user.Key)
            {
                return SimpleTextSelector;
            }
            else
            {
                return InboundTextSelector;
            }
            //Todo - escolher entre os dois
            /*var list = (CollectionView)container;

            if (item is OutboundMessage)
            {
                return SimpleTextSelector;
            }
            else
            {
                return InboundTextSelector;
            }*/
        }
    }
}

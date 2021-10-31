using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace TCCApp.Helpers
{
    static class ObservableCollectionHelper
    {
        public static void SafeClear<T>(this ObservableCollection<T> observableCollection)
        {
            if (observableCollection != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    observableCollection.Clear();
                });
            }
        }
    }
}

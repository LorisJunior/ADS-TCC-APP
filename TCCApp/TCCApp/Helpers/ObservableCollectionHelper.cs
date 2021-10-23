using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace TCCApp.Helpers
{
    static class ObservableCollectionHelper
    {
        public static void SafeClear<T>(this ObservableCollection<T> observableCollection)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                observableCollection.Clear();
            });
        }

        public static void TSafeClear<T>(this ICollection<T> Collection)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Collection.Clear();
            });
        }
    }
}

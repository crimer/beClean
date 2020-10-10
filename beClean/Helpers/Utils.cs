using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;

namespace beClean.Helpers
{
    public static class Utils
    {
        public static ObservableCollection<T> Convert<T>(IEnumerable original)
        {
            return new ObservableCollection<T>(original.Cast<T>());
        }
    }
}

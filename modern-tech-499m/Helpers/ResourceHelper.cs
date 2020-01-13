using System.Windows;

namespace modern_tech_499m.Helpers
{
    public static class ResourceHelper
    {
        public static T GetAsResource<T>(this string resourceName)
        {
            return (T)Application.Current.FindResource(resourceName);
        }
    }
}

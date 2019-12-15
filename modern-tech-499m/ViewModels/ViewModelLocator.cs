namespace modern_tech_499m.ViewModels
{
    /// <summary>
    /// Locates viewmodels from IoC for use un binding in XAML files
    /// </summary>
    public class ViewModelLocator
    {
        #region Pubilic properties

        /// <summary>
        /// A singleton instance of the locator
        /// </summary>
        public static ViewModelLocator Instance => new ViewModelLocator();

        #endregion

        /// <summary>
        /// The application viewmodel
        /// </summary>
        public static ApplicationViewModel ApplicationViewModel => BootStrapper.Resolve<ApplicationViewModel>();
    }
}

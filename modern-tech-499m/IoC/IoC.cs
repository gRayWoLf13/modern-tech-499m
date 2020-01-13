using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_tech_499m
{
    /// <summary>
    /// A class with a shortcuts to commonly used IoC items
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// A shortcut to access the <see cref="IUIManager"/>
        /// </summary>
        public static IUIManager UI = BootStrapper.Resolve<IUIManager>();
    }
}

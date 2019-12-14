using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_tech_499m
{
    /// <summary>
    /// Styles of page animations
    /// </summary>
    public enum PageAnimation
    {
        /// <summary>
        /// No animation
        /// </summary>
        None = 0,
        /// <summary>
        /// The page slides in from the right
        /// </summary>
        SlideAndFadeInFromRight = 1,
        /// <summary>
        /// The page slides out to the left
        /// </summary>
        SlideAndFadeOutToLeft = 2
    }
}

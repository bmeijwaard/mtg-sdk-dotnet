﻿// <copyright file="MtgApiController.cs">
//     Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Jason Regnier</author>
namespace MtgApiManager.Lib.Core
{
    /// <summary>
    /// Managers MTG API related functionality. 
    /// </summary>
    internal static class MtgApiController
    {
        /// <summary>
        /// Gets or sets the headers with previous, last, next, first links (when appropriate).
        /// </summary>
        public static string Link
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rate limit for a given user.
        /// </summary>
        public static int RatelimitLimit
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of requests left before the rate limit is exceeded.
        /// </summary>
        public static int RatelimitRemaining
        {
            get;
            set;
        }
    }
}
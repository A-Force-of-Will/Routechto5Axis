// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingletonBehaviour.cs" company="">
//   Copyright (c) $date$ 
// </copyright>
// <summary>
//  If this project is helpful please take a short survey at ->
//  http://ux.mastercam.com/Surveys/APISDKSupport 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace RoutechToFiveAxis.Services
{
    /// <summary> The singleton behaviour. </summary>
    ///
    /// <typeparam name="T"> Type of T. </typeparam>
    public abstract class SingletonBehaviour<T>
    {
        /// <summary>
        /// Backing field for the T Instance property
        /// </summary>
        private static T instance;

        /// <summary>
        /// Gets the Instance of type T
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance != null && !instance.Equals(null))
                {
                    return instance;
                }

                instance = Activator.CreateInstance<T>();

                return instance;
            }
        }
    }
}
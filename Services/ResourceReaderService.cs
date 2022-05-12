// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceReaderService.cs" company="">
//   Copyright (c) $date$ 
// </copyright>
// <summary>
//  If this project is helpful please take a short survey at ->
//  http://ux.mastercam.com/Surveys/APISDKSupport 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using RoutechToFiveAxis.Models;

using NETHookResources = RoutechToFiveAxis.Properties.Resources;

namespace RoutechToFiveAxis.Services
{
    /// <summary> The resource reader service. </summary>
    public class ResourceReaderService : SingletonBehaviour<ResourceReaderService>
    {
        /// <summary> Gets the resource string from our resources. </summary>
        ///
        /// <param name="name"> The resource name. </param>
        ///
        /// <returns> The value of the resource. </returns>
        public static Result<string> GetString(string name)
        {
            try
            {
                return Result.Ok(NETHookResources.ResourceManager.GetString(name));
            }
            catch
            {
                return Result.Fail<string>($"Missing resource {name} ");
            }
        }
    }
}
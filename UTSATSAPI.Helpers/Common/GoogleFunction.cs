using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace UTSATSAPI.Helpers.Common
{
    

    public static class GoogleFunction
    {
        #region Variables
   
        #endregion

        #region Constructors

        #endregion

        #region Public Methods

        
        public static void DownloadGoogleDriveFile(string driveUrl,string filePath)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                client.DownloadFile(new Uri(driveUrl), filePath);
            }
        }
        #endregion

    }
}

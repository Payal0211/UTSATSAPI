using Amazon.Auth.AccessControlPolicy;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;

namespace UTSATSAPI.Controllers
{
    [Route("BackUp/", Name = "BackUp")]
    [ApiController]
    public class BackUpController : ControllerBase
    {

        #region Admin Paths


        private readonly string AdminDotNetSourcePath = "C:\\inetpub\\wwwroot\\TCAdmin_NewUTS";
        private readonly string AdminDotNetDestinationPath = "E:\\AdminUTSLiveBackup\\DotNet\\";
        private readonly string AdminReactSourcePath = "C:\\TC_NewAdmin_React";
        private readonly string AdminReactDestinationPath = "E:\\AdminUTSLiveBackup\\React\\";
        #endregion

        #region Client portal Paths
        private readonly string ClientDotNetSourcePath = "C:\\inetpub\\wwwroot\\UTSClient_Live";
        private readonly string ClientDotNetDestinationPath = "E:\\ClientLiveBackup\\DotNet\\";
        private readonly string ClientReactSourcePath = "C:\\UTS_Client_Live_React";
        private readonly string ClientReactDestinationPath = "E:\\ClientLiveBackup\\React\\";
        #endregion     

        [HttpGet("AdminLive")]
        public async Task<IActionResult> BackupFiles()
        {
            try
            {
                int filesCopied = 0;
                int directoriesCopied = 0;

                //Admin backup
                string dateFolder = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");

                string AdminfullDestinationPath = Path.Combine(AdminDotNetDestinationPath, dateFolder);

                if (!Directory.Exists(AdminfullDestinationPath))
                {
                    Directory.CreateDirectory(AdminfullDestinationPath);
                }

                CopyFilesRecursively(new DirectoryInfo(AdminDotNetSourcePath), new DirectoryInfo(AdminfullDestinationPath), ref filesCopied, ref directoriesCopied);

                BackupResponse responseDotNet = new BackupResponse
                {
                    Success = true,
                    Message = "Files backed up successfully.",
                    SourcePath = AdminDotNetSourcePath,
                    DestinationPath = AdminfullDestinationPath,
                    BackupTime = DateTime.Now,
                    FilesCopied = filesCopied,
                    DirectoriesCopied = directoriesCopied
                };

                //React backup
                dateFolder = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");

                string ReactfullDestinationPath = Path.Combine(AdminReactDestinationPath, dateFolder);

                if (!Directory.Exists(ReactfullDestinationPath))
                {
                    Directory.CreateDirectory(ReactfullDestinationPath);
                }

                CopyFilesRecursively(new DirectoryInfo(AdminReactSourcePath), new DirectoryInfo(ReactfullDestinationPath), ref filesCopied, ref directoriesCopied);

                BackupResponse responseReact = new BackupResponse
                {
                    Success = true,
                    Message = "Files backed up successfully.",
                    SourcePath = AdminReactSourcePath,
                    DestinationPath = ReactfullDestinationPath,
                    BackupTime = DateTime.Now,
                    FilesCopied = filesCopied,
                    DirectoriesCopied = directoriesCopied
                };

                List<BackupResponse> list = new List<BackupResponse>();
                list.Add(responseDotNet);
                list.Add(responseReact);

                return Ok(list);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("ClientPortal")]
        public async Task<IActionResult> BackupFiles_Client()
        {
            try
            {
                int filesCopied = 0;
                int directoriesCopied = 0;

                //Client backup
                string dateFolder = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                string ClientfullDestinationPath = Path.Combine(ClientDotNetDestinationPath, dateFolder);

                if (!Directory.Exists(ClientfullDestinationPath))
                {
                    Directory.CreateDirectory(ClientfullDestinationPath);
                }

                CopyFilesRecursively(new DirectoryInfo(ClientDotNetSourcePath), new DirectoryInfo(ClientfullDestinationPath), ref filesCopied, ref directoriesCopied);

                BackupResponse responseDotNet = new BackupResponse
                {
                    Success = true,
                    Message = "Files backed up successfully.",
                    SourcePath = ClientDotNetSourcePath,
                    DestinationPath = ClientfullDestinationPath,
                    BackupTime = DateTime.Now,
                    FilesCopied = filesCopied,
                    DirectoriesCopied = directoriesCopied
                };

                //React backup
                dateFolder = DateTime.Now.ToString("dd-MM-yyyy HH_mm_ss");
                string ReactfullDestinationPath = Path.Combine(ClientReactDestinationPath, dateFolder);

                if (!Directory.Exists(ReactfullDestinationPath))
                {
                    Directory.CreateDirectory(ReactfullDestinationPath);
                }

                CopyFilesRecursively(new DirectoryInfo(ClientReactSourcePath), new DirectoryInfo(ReactfullDestinationPath), ref filesCopied, ref directoriesCopied);

                BackupResponse responseReact = new BackupResponse
                {
                    Success = true,
                    Message = "Files backed up successfully.",
                    SourcePath = ClientReactSourcePath,
                    DestinationPath = ReactfullDestinationPath,
                    BackupTime = DateTime.Now,
                    FilesCopied = filesCopied,
                    DirectoriesCopied = directoriesCopied
                };

                List<BackupResponse> list = new List<BackupResponse>();
                list.Add(responseDotNet);
                list.Add(responseReact);

                return Ok(list);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        private void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target, ref int filesCopied, ref int directoriesCopied)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                DirectoryInfo targetSubDir = target.CreateSubdirectory(dir.Name);
                directoriesCopied++;
                CopyFilesRecursively(dir, targetSubDir, ref filesCopied, ref directoriesCopied);
            }

            foreach (FileInfo file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);
                filesCopied++;
            }
        }
    }

    public class BackupResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public DateTime BackupTime { get; set; }
        public int FilesCopied { get; set; }
        public int DirectoriesCopied { get; set; }
    }
}

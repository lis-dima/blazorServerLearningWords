using lewBlazorServer.Data.Entities;
using lewBlazorServer.Data.Interfaces;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Transcription;
using System.Data.SqlTypes;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace lewBlazorServer.Services.Helpers
{
    public class Storage
    {
        public static bool HasAudio(EntityType type, int id)
        {
            return File.Exists(GetAudioPath(type, id, false));
        }

        public static void DeleteAudio(string path)
        {
            if(File.Exists(path))
                File.Delete(path);
        }

        public static string GetAudioPath(EntityType type, int id, bool createFolder)
        {
            //var fileName = AppropriateFileName(text);
            var path = Path.Combine(new string[] {
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "storages",
                "global",
                type.ToString(),
                id + ".wav"
            });
            if (createFolder)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            return path;
        }

        public static string GetUserDailyFolder(string userId, bool createFolder)
        {
            var path = Path.Combine(new string[] {
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "storages",
                "daily",
                userId
            });
            if (createFolder && !File.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            return path;
        }

        public static string GetUserDailyFolder_MergedFolder(string userId)
        {
            var path = Path.Combine(new string[] {
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "storages",
                "daily",
                userId,
                "merged"
            });
            DirectoryInfo di = Directory.CreateDirectory(path);
            return path;
        }

        public static string TodayIndex()
        {
            var dateY = DateTime.Now.ToString("yyyy");
            int dateYFormated = Int32.Parse(dateY) - 2023;
            return (dateYFormated + DateTime.Now.ToString("MMdd")).TrimStart('0');
        }

        public static void PrepareUserFolders(string userId, bool clearUserDir)
        {
            var userFolderPath = Path.Combine(new string[] {
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "storages",
                "daily",
                userId
            });
            if (clearUserDir && Directory.Exists(userFolderPath))
            {
                Directory.Delete(userFolderPath, true);
            }
            var folderPath = Path.Combine(new string[] {
                userFolderPath,
                "merged"
            });
            Directory.CreateDirectory(folderPath);

            folderPath = Path.Combine(new string[] {
                userFolderPath,
                "archive"
            });
            Directory.CreateDirectory(folderPath);
        }

        public static string GetUserArchivepath(string userId)
        {
            return Path.Combine(
                lewBlazorServer.Services.Helpers.Storage.GetUserDailyFolder(userId, false),
                "audio for " + TodayIndex() + ".zip"
            );
        }

        public static string LocalPath2Web(string path)
        {
            int wwwrootIndex = path.IndexOf("wwwroot");
            if (wwwrootIndex < 0)
            {
                return "LocalPath2Web-error";
            }
            string relativePath = path.Substring(wwwrootIndex + "wwwroot".Length);
            string webPath = relativePath.Replace(Path.DirectorySeparatorChar, '/');
            if (!webPath.StartsWith("/"))
            {
                webPath = "/" + webPath;
            }
            return webPath;
        }
    }
}
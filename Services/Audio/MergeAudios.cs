using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.IO.Compression;

namespace lewBlazorServer.Services.MergeAudios
{
    public class MergeAudios
    {
        public static void Merge(string[] merge, string outputPath) // outputPath .zip
        {
            //ChangeSeparator();
            //return;
            var separator = Path.Combine(new string[] {
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "sounds",
                "beforeStartSound.wav",
            });
            List<AudioFileReader> afrs = new List<AudioFileReader>();
            afrs.Add(new AudioFileReader(separator));
            foreach (var f in merge)
            {
                afrs.Add(new AudioFileReader(f));
            }
            var playList = new ConcatenatingSampleProvider(afrs.ToArray());
            WaveFileWriter.CreateWaveFile16(outputPath, playList);
        }

        public static void CreateAudioArchiveFromFolder(string audiosFolderPath, string folder2SaveArchive) //folder2SaveArchive must contain .zip
        {
            ZipFile.CreateFromDirectory(audiosFolderPath, folder2SaveArchive);
        }

        public static void CreateAudioArchiveFromFilesPaths(string[] audiosFolderPaths, string folder2SaveArchive) //folder2SaveArchive must contain .zip
        {
            using (ZipArchive archive = ZipFile.Open(folder2SaveArchive, ZipArchiveMode.Update))
            {
                foreach (var path in audiosFolderPaths)
                {
                    string fileName = Path.GetFileName(path);
                    archive.CreateEntryFromFile(path, fileName);
                }
            }
        }

        static void ChangeSeparator()
        {
            var separator = Path.Combine(new string[] {
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "sounds",
                "sep-3sec.wav",
            });
            var separatorNew = Path.Combine(new string[] {
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "sounds",
                "NEW_sep-3sec.wav",
            });
            using (var reader = new WaveFileReader(separator))
            {
                var newFormat = new WaveFormat(16000, 16, 1);
                using (var conversionStream = new WaveFormatConversionStream(newFormat, reader))
                {
                    WaveFileWriter.CreateWaveFile(separatorNew, conversionStream);
                }
            }
        }
    }
}

using lewBlazorServer.Data.Entities;
using lewBlazorServer.Services.Audio;
using Microsoft.CodeAnalysis.Options;

namespace lewBlazorServer.Services.Helpers
{
    public class EnumConverter
    {
        public static string TtsLang(Language language)
        {
            var res = "";
            switch (language)
            {
                case Language.En:
                    res = "en-GB";
                    break;
                case Language.Ru:
                    res = "ru-RU";
                    break;
                case Language.Ua:
                    res = "uk-UA";
                    break;
            }
            return res;
        }

        public static TtsSpeaker TtsSpeaker(Language language)
        {
            var res = Audio.TtsSpeaker.en_GB_OliverNeural;
            switch (language)
            {
                case Language.Ru:
                    res = Audio.TtsSpeaker.ru_RU_DmitryNeural; 
                    break;
            }
            return res;
        }


        public static string TtsSpeaker(TtsSpeaker speaker)
        {
            var res = "";
            switch (speaker)
            {
                case Audio.TtsSpeaker.en_GB_OliverNeural:
                    res = "en-GB-OliverNeural";
                    break;
                case Audio.TtsSpeaker.ru_RU_DmitryNeural:
                    res = "ru-RU-DmitryNeural";
                    break;
            }
            return res;
        }

        public static Language LangFromStr(string langStr)
        {
            return (Language)Enum.Parse(typeof(Language), langStr);
        }
    }
}

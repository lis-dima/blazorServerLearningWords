using lewBlazorServer.Data.Dto;
using lewBlazorServer.Services.Helpers;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;


namespace lewBlazorServer.Services.Audio
{
    public static class CreateAudioTTS
    {
        public async static Task<bool> Text2Speach(string lang, string text, string pathWhere2Save, string speaker, int recursiveCD)
        {
            --recursiveCD;
            if (recursiveCD == 0) return false;
            var speechConfig = SpeechConfig.FromSubscription(Secret.TTS_APP_KEY, Secret.TTS_REGION);
            var audioConfig = AudioConfig.FromWavFileOutput(pathWhere2Save);
            //speechConfig.SpeechSynthesisVoiceName = speaker;

            var ssml = @$"<speak 
                            xmlns='http://www.w3.org/2001/10/synthesis' 
                            xmlns:mstts='http://www.w3.org/2001/mstts' 
                            xmlns:emo='http://www.w3.org/2009/10/emotionml' 
                            version='1.0' 
                            xml:lang='{lang}'
                        >
                            <voice name='{speaker}'>
                                <prosody rate='-20%' pitch='0%'>{text}</prosody>
                            </voice>
                        </speak>";

            speechConfig.SetProperty(PropertyId.SpeechServiceResponse_RequestSentenceBoundary, "true");

            using (var speechSynthesizer = new SpeechSynthesizer(speechConfig, audioConfig))
            {
                var speechSynthesisResult = await speechSynthesizer.SpeakSsmlAsync(ssml);
                if (!OutputSpeechSynthesisResult(speechSynthesisResult, text))
                    await Text2Speach(lang, text, pathWhere2Save, speaker, recursiveCD);
            }
            return true;
        }

        static bool OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, string text)
        {
            bool res = false;
            switch (speechSynthesisResult.Reason)
            {
                case ResultReason.SynthesizingAudioCompleted:
                    Console.WriteLine($"TTS done={text}");
                    res = true;
                    break;
                case ResultReason.Canceled:
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(speechSynthesisResult);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]  TEXT:{text}");
                    }
                    break;
                default:
                    break;
            }
            return res;
        }

        public static async Task<bool> CreateAudio(CreateAudioDto createAudioDto)
        {
            string path = Storage.GetAudioPath(createAudioDto.Lang, createAudioDto.Type, createAudioDto.TextId, true);
            return await Text2Speach(createAudioDto.Lang.ToString(), createAudioDto.Text, path, EnumConverter.TtsSpeaker(createAudioDto.Speaker), 5);
        }
    }

    // should use EnumConverter.TtsSpeaker(Speaker speaker)
    public enum TtsSpeaker
    {
        en_GB_OliverNeural, // en-GB-OliverNeural
        ru_RU_DmitryNeural  // ru-RU-DmitryNeural
    }
}

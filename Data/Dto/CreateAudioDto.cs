using lewBlazorServer.Data.Entities;
using lewBlazorServer.Data.Interfaces;
using lewBlazorServer.Services.Helpers;
using lewBlazorServer.Services.Audio;

namespace lewBlazorServer.Data.Dto
{
    public class CreateAudioDto
    {
        public int TextId { get; set; }
        public string Text { get; set; }
        public Language Lang { get; set; }
        public TtsSpeaker Speaker { get; set; }
        public EntityType Type { get; set; }
        public CreateAudioDto() { }
        public CreateAudioDto(Word word)
        {
            Type = EntityType.Word;
            Text = word.Value;
            TextId = word.Id;
            Lang = word.Language;
            Speaker = TtsSpeaker.en_GB_OliverNeural; //becouse Word can be only EN
        }

        public CreateAudioDto(Example example)
        {
            Type = EntityType.Example;
            Text = example.Value;
            TextId = example.Id;
            Lang = example.Language;
            Speaker = EnumConverter.TtsSpeaker(Lang);
        }

        public CreateAudioDto(Description description) {
            Type = EntityType.Description;
            Text = description.Value;
            TextId = description.Id;
            Lang = description.Language;
            Speaker = EnumConverter.TtsSpeaker(Lang);
        }

        public CreateAudioDto(Translation translation)
        {
            Type = EntityType.Translation;
            Text = translation.Value;
            TextId = translation.Id;
            Lang = translation.Language;
            Speaker = EnumConverter.TtsSpeaker(Lang);
        }

        public CreateAudioDto(IRecordEntity data)
        {
            Type = data.Type;
            Text = data.Value;
            TextId = data.Id;
            Lang = data.Language;
            Speaker = EnumConverter.TtsSpeaker(Lang);
        }
    }
}

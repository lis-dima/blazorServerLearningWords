using lewBlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace lewBlazorServer.Data.Dto
{
    public class CreateWordDto
    {
        public string Word { get; set; } = "Sun";
        public string Transcription { get; set; } = "sʌn";
        public Language Language { get; set; } = Language.En;

        public List<Translation> Translations { get; set; } = new List<Translation>();
        public List<Description> Descriptions { get; set; } = new List<Description>();
        public List<Example> Examples { get; set; } = new List<Example>();

    }
}

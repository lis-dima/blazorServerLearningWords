namespace lewBlazorServer.Data.Entities
{
    public class Word
    {
        public int Id { get; set; }
        public Language Language { get; set; } = Language.En;
        public string Value { get; set; }
        public string Transcription { get; set; }
        public List<Description> Descriptions { get; set; }
        public List<Example> Examples { get; set; }
        public List<Translation> Translations { get; set; }
    }


    public enum Language
    {
        En,
        Ua,
        Ru
    }
}

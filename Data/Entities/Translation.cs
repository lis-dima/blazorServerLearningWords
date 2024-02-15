using lewBlazorServer.Data.Interfaces;
using System.Text.Json.Serialization;

namespace lewBlazorServer.Data.Entities
{
    public class Translation : IRecordEntity
    {
        public int Id { get; set; }
        public Language Language { get; set; } = Language.En;

        public string Value { get; set; }
        public int WordId { get; set; }
        [JsonIgnore]
        public Word Word { get; set; }
        public EntityType Type { get; set; } = EntityType.Translation;
    }
}

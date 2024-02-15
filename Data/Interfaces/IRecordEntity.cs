using lewBlazorServer.Data.Entities;

namespace lewBlazorServer.Data.Interfaces
{
    public interface IRecordEntity
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int WordId { get; set; }
        public EntityType Type { get; set; }
        public Language Language { get; set; }
    }


    public enum EntityType
    {
        Word,
        Translation,
        Description,
        Example
    }
}

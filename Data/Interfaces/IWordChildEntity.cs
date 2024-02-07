using lewBlazorServer.Data.Entities;

namespace lewBlazorServer.Data.Interfaces
{
    public interface IWordChildEntity
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int WordId { get; set; }
        public EntityType WordChildType { get; set; }
    }


    public enum EntityType
    {
        Word,
        Translation,
        Description,
        Example
    }
}

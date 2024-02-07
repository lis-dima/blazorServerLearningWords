namespace lewBlazorServer.Data.Entities
{
    public class OnStudy
    {
        public int Id { get; set; }
        public string UserId { get; set; } = String.Empty;
        public int WordId { get; set; }
        public DateTime ShowAfter { get; set; } = DateTime.Now;
        public int Lvl { get; set; } = 1;
        public Word Word { get; set; }

        public OnStudy()
        {
            
        }
        public OnStudy(string userId, int wordId)
        {
            UserId=userId;
            WordId=wordId;
        }
    }
}

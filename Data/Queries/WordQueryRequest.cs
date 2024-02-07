using lewBlazorServer.Data.Entities;

namespace lewBlazorServer.Data.Queries
{
    public class WordQueryRequest
    {
        public bool ShouldIncludeExamples;
        public List<Language> TranslationLanguages = new List<Language>();
    }
}

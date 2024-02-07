using lewBlazorServer.Data.Entities;

namespace lewBlazorServer.Services.WordService
{
    public interface IWordService
    {
        Task<Response<Word>> CreateWord(string word, string transcription, Language language);
        Task<Response<Example>> CreateExample4Word(string value, Language language, int wordId);
        Task<Response<Description>> CreateDescription4Word(string value, Language language, int wordId);
        Task<Response<Translation>> CreateTranslation4Word(string value, Language language, int wordId);
        Task<Response<Word>> GetWord(int wordId, Language translationLang);
        Task<Response<Word>> GetWord(string word, Language translationLang);
        Task<Response<Example>> UpdateWordExample(int id, string value);
        Task<Response<Description>> UpdateWordDescription(int id, string value);
        Task<Response<List<Word>>> LastAddedWords(int page, int perPage);
    }
}

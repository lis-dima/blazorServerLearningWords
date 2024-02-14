using Azure;
using lewBlazorServer.Data;
using lewBlazorServer.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lewBlazorServer.Services.WordService
{
    public class WordService : IWordService
    {
        private readonly ApplicationDbContext context;

        public WordService(ApplicationDbContext context)
        {
            this.context=context;
        }

        public async Task<Response<Word>> CreateWord(string word, string transcription, Language language)
        {
            var resp = new Response<Word>();
            var Word = new Word() { Value = word, Transcription = transcription, Language = language };
            context.Words.Add(Word);
            await context.SaveChangesAsync();
            resp.Data = Word;
            return resp;
        }
        public async Task<Response<Example>> CreateExample4Word(string value, Language language, int wordId)
        {
            var resp = new Response<Example>();
            var example = new Example() { Value = value, Language = language, WordId = wordId };
            context.Add(example);
            await context.SaveChangesAsync();
            resp.Data = example;
            return resp;
        }

        public async Task<Response<Description>> CreateDescription4Word(string value, Language language, int wordId)
        {
            var resp = new Response<Description>();
            var desc = new Description() { Value = value, Language = language, WordId = wordId };
            context.Add(desc);
            await context.SaveChangesAsync();
            resp.Data = desc;
            return resp;
        }

        public async Task<Response<Translation>> CreateTranslation4Word(string value, Language language, int wordId)
        {
            var resp = new Response<Translation>();
            var trans = new Translation() { Value = value, Language = language, WordId = wordId };
            context.Add(trans);
            await context.SaveChangesAsync();
            resp.Data = trans;
            return resp;
        }

        public async Task<Response<Word>> GetWord(int wordId, Language translationLang)
        {
            var resp = new Response<Word>();
            var w = await context
                .Words
                .Include(w => w.Translations.Where(t => t.Language == translationLang))
                .Include(w => w.Descriptions)
                .Include(w => w.Examples)
                .FirstOrDefaultAsync(w => w.Id == wordId);
            if (w == null)
                resp.Errors.Add(new Error("no such word"));
            resp.Data = w;
            return resp;
        }

        public async Task<Response<Word>> GetWord(string word, Language translationLang)
        {
            var resp = new Response<Word>();
            var w = await context
                .Words
                .Include(w => w.Translations.Where(t => t.Language == translationLang))
                .Include(w => w.Descriptions)
                .Include(w => w.Examples)
                .FirstOrDefaultAsync(w => w.Value == word);
            if (w == null)
                resp.Errors.Add(new Error("no such word"));
            resp.Data = w;
            return resp;
        }

        public async Task<Response<Example>> UpdateWordExample(int id, string value)
        {
            var resp = new Response<Example>();
            var example = await context.Examples.FirstOrDefaultAsync(t => t.Id == id);
            if (example == null)
            {
                resp.Errors.Add(new("no such example with id: " + id));
                return resp;
            }
            example.Value = value;
            await context.SaveChangesAsync();
            resp.Data = example;
            return resp;
        }


        public async Task<Response<Description>> UpdateWordDescription(int id, string value)
        {
            var resp = new Response<Description>();
            var desc = await context.Descriptions.FirstOrDefaultAsync(t => t.Id == id);
            if (desc == null)
            {
                resp.Errors.Add(new("no such example with id: " + id));
                return resp;
            }
            desc.Value = value;
            await context.SaveChangesAsync();
            resp.Data = desc;
            return resp;
        }

        public async Task<Response<Translation>> UpdateWordTranslation(int id, string value)
        {
            var resp = new Response<Translation>();
            var desc = await context.Translations.FirstOrDefaultAsync(t => t.Id == id);
            if (desc == null)
            {
                resp.Errors.Add(new("no such translation with id: " + id));
                return resp;
            }
            desc.Value = value;
            await context.SaveChangesAsync();
            resp.Data = desc;
            return resp;
        }

        public async Task<Response<List<Word>>> LastAddedWords(int page, int perPage)
        {
            int skipCount = (page - 1) * perPage;

            var words = await context.Words
                                 .OrderByDescending(w => w.Id)
                                 .Skip(skipCount)
                                 .Take(perPage)
                                 .ToListAsync();
            var resp = new Response<List<Word>>();
            resp.Data = words;
            return resp;
        }

        public async Task<Response<bool>> DeleteExample(int id)
        {
            var respond = new Response<bool>();
            var entity = await context.Examples.FindAsync(id);
            if (entity != null)
            {
                respond.Data = true;
                context.Examples.Remove(entity);
                await context.SaveChangesAsync();
            }
            return respond;
        }

        public async Task<Response<bool>> DeleteTranslation(int id)
        {
            var respond = new Response<bool>();
            var entity = await context.Translations.FindAsync(id);
            if (entity != null)
            {
                respond.Data = true;
                context.Translations.Remove(entity);
                await context.SaveChangesAsync();
            }
            return respond;
        }

        public async Task<Response<bool>> DeleteDescription(int id)
        {
            var respond = new Response<bool>();
            var entity = await context.Descriptions.FindAsync(id);
            if (entity != null)
            {
                respond.Data = true;
                context.Descriptions.Remove(entity);
                await context.SaveChangesAsync();
            }
            return respond;
        }

        public async Task<Response<bool>> DeleteWord(int id)
        {
            var respond = new Response<bool>();
            var entity = await context.Words.FindAsync(id);
            if (entity != null)
            {
                respond.Data = true;
                context.Words.Remove(entity);
                await context.SaveChangesAsync();
            }
            return respond;
        }
    }
}

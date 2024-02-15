using Azure;
using Humanizer;
using lewBlazorServer.Data.Dto;
using lewBlazorServer.Data.Entities;
using lewBlazorServer.Data.Interfaces;
using lewBlazorServer.Migrations;
using lewBlazorServer.Services;
using lewBlazorServer.Services.Audio;
using lewBlazorServer.Services.Helpers;
using lewBlazorServer.Services.WordService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;


namespace lewBlazorServer.Controllers
{
    public class WordController
    {
        public event Action<Word> OnAfterWordChanged;
        public event Action<IRecordEntity> OnSetOnEditWordChildEntity;
        private readonly IWordService wordService;

        public WordController(IWordService wordService)
        {
            this.wordService = wordService;
        }

        public async Task<Services.Response<Word>> CreateWord(CreateWordDto data)
        {
            var resp = new Services.Response<Word>();
            var result = await wordService.CreateWord(data.Word, data.Transcription, data.Language);
            resp.Errors = result.Errors;
            resp.Data = result.Data;

            if (!resp.Ok) return resp;

            await CreateAudioTTS.CreateAudio(new(result.Data)); //creation of audio


            foreach (var example in data.Examples)
            {
                await CreateWordExample(example.Value, example.Language, resp.Data.Id);
            }

            foreach (var description in data.Descriptions)
            {
                await CreateWordDescription(description.Value, description.Language, resp.Data.Id);
            }

            foreach (var translation in data.Translations)
            {
                await CreateWordTranslation(translation.Value, translation.Language, resp.Data.Id);
            }
            return resp;
        }

        public async Task<Services.Response<Translation>> CreateWordTranslation(string value, Language language, int wordId)
        {
            var response = new Services.Response<Translation>();
            var result = await wordService.CreateTranslation4Word(value, language, wordId);
            response.Data = result.Data;
            response.Errors = result.Errors;
            if (response.Ok)
            {
                await CreateAudioTTS.CreateAudio(new(response.Data)); //creation of audio
            }
            return response;
        }

        public async Task<Services.Response<Description>> CreateWordDescription(string value, Language language, int wordId)
        {
            var response = new Services.Response<Description>();
            var result = await wordService.CreateDescription4Word(value, language, wordId);
            response.Data = result.Data;
            response.Errors = result.Errors;
            if (response.Ok)
            {
                await CreateAudioTTS.CreateAudio(new(response.Data)); //creation of audio
            }
            return response;
        }

        public async Task<Services.Response<Example>> CreateWordExample(string value, Language language, int wordId)
        {
            var response = new Services.Response<Example>();
            var result = await wordService.CreateExample4Word(value, language, wordId);
            response.Data = result.Data;
            response.Errors = result.Errors;
            if (response.Ok)
            {
                await CreateAudioTTS.CreateAudio(new(response.Data)); //creation of audio
            }
            return response;
        }

        public async Task<Services.Response<Word>> GetWord(string value, Language translationLang = Language.En)
        {
            return await wordService.GetWord(value, translationLang);
        }

        public async Task<Services.Response<Word>> GetWord(int wordId, Language translationLang = Language.En)
        {
            return await wordService.GetWord(wordId, translationLang);
        }

        public async Task<List<Word>> LastAddedWords(int page, int perPage)
        {
            return (await wordService.LastAddedWords(page, perPage)).Data;
        }

        public void SetOnEdit(IRecordEntity wordChildEntity)
        {
            OnSetOnEditWordChildEntity?.Invoke(wordChildEntity);
        }

        public async Task UpdateWordChildEntity(IRecordEntity editedChildEntity)
        {
            if (editedChildEntity.Type == EntityType.Example)
            {
                var resp = await wordService.UpdateWordExample(editedChildEntity.Id, editedChildEntity.Value);
                if (resp.Ok)
                {
                    OnAfterWordChanged?.Invoke((await GetWord(editedChildEntity.WordId)).Data);
                }
            }
            else if (editedChildEntity.Type == EntityType.Description)
            {
                var resp = await wordService.UpdateWordDescription(editedChildEntity.Id, editedChildEntity.Value);
                if (resp.Ok)
                {
                    OnAfterWordChanged?.Invoke((await GetWord(editedChildEntity.WordId)).Data);
                }
            }
            else if (editedChildEntity.Type == EntityType.Translation)
            {
                var resp = await wordService.UpdateWordTranslation(editedChildEntity.Id, editedChildEntity.Value);
                if (resp.Ok)
                {
                    OnAfterWordChanged?.Invoke((await GetWord(editedChildEntity.WordId)).Data);
                }
            }
        }

        public async Task UpdateOrCreateChildEntity(IRecordEntity editedChildEntity)
        {
            List<Error> errors = new List<Error>();
            if (editedChildEntity.Type == EntityType.Translation)
            {
                errors = (await wordService.UpdateWordTranslation(editedChildEntity.Id, editedChildEntity.Value)).Errors;
            }
            else if (editedChildEntity.Type == EntityType.Description)
            {
                errors = (await wordService.UpdateWordDescription(editedChildEntity.Id, editedChildEntity.Value)).Errors;
            }
            else if (editedChildEntity.Type == EntityType.Example)
            {
                errors = (await wordService.UpdateWordExample(editedChildEntity.Id, editedChildEntity.Value)).Errors;
            }

            if (errors.Count == 0)
            {
                await CreateAudioTTS.CreateAudio(new(editedChildEntity));
            }
            else if (errors.Count == 1 && errors[0].Code == ErrorCode.NotFound)
            {
                if (editedChildEntity.Type == EntityType.Translation)
                    await CreateWordTranslation(editedChildEntity.Value, editedChildEntity.Language, editedChildEntity.WordId);
                else if (editedChildEntity.Type == EntityType.Description)
                    await CreateWordDescription(editedChildEntity.Value, editedChildEntity.Language, editedChildEntity.WordId);
                else if (editedChildEntity.Type == EntityType.Example)
                    await CreateWordExample(editedChildEntity.Value, editedChildEntity.Language, editedChildEntity.WordId);
            }
        }
    
        public async Task<bool> DeleteChildEntity(int entityId, EntityType type)
        {
            var successful = false ;
            if (type == EntityType.Translation)
            {
                var r = await wordService.DeleteTranslation(entityId);
                successful = r.Data;
            }
            else if (type == EntityType.Example)
            {
                var r = await wordService.DeleteExample(entityId);
                successful = r.Data;
            }
            else if (type == EntityType.Description)
            {
                var r = await wordService.DeleteDescription(entityId);
                successful = r.Data;
            }
            return successful;
        }

        public async Task DeleteWord(int id)
        {
            await wordService.DeleteWord(id);
        }


    }
}

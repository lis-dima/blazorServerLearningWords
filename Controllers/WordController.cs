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
        public event Action<IWordChildEntity> OnSetOnEditWordChildEntity;
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

        private async Task<Services.Response<Translation>> CreateWordTranslation(string value, Language language, int wordId)
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

        private async Task<Services.Response<Description>> CreateWordDescription(string value, Language language, int wordId)
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

        private async Task<Services.Response<Example>> CreateWordExample(string value, Language language, int wordId)
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
            return ( await wordService.LastAddedWords(page, perPage)).Data;
        }

        public void SetOnEdit(IWordChildEntity wordChildEntity)
        {
            OnSetOnEditWordChildEntity?.Invoke(wordChildEntity);
        }

        public async Task UpdateWordChildEntity(IWordChildEntity editedChildEntity)
        {
            if (editedChildEntity.WordChildType == EntityType.Example)
            {
                var resp = await wordService.UpdateWordExample(editedChildEntity.Id, editedChildEntity.Value);
                if (resp.Ok)
                {
                    OnAfterWordChanged?.Invoke((await GetWord(editedChildEntity.WordId)).Data);
                }
            }
            else if (editedChildEntity.WordChildType == EntityType.Description)
            {
                var resp = await wordService.UpdateWordDescription(editedChildEntity.Id, editedChildEntity.Value);
                if (resp.Ok)
                {
                    OnAfterWordChanged?.Invoke((await GetWord(editedChildEntity.WordId)).Data);
                }
            }
        }
    }
}

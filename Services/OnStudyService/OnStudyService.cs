using lewBlazorServer.Data;
using lewBlazorServer.Data.Entities;
using lewBlazorServer.Data.Queries;
using Microsoft.EntityFrameworkCore;

namespace lewBlazorServer.Services.OnStudyService
{
    public class OnStudyService
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public OnStudyService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context=context;
            this.httpContextAccessor=httpContextAccessor;
        }

        public async Task<Response<OnStudy>> SetWordOnStudy(string userId, int wordId)
        {
            var resp = new Response<OnStudy>();
            if ((await GetWordOnStudy(userId, wordId)).Ok)
            {
                resp.Errors.Add(new( ErrorCode.AlreadyExist, $"The word with id ({wordId}) is already in on study"));
            }
            else
            {
                OnStudy onStudy = new OnStudy(userId, wordId);
                context.Add(onStudy);
                await context.SaveChangesAsync();
                resp.Data = onStudy;
            }
            return resp;
        }

        public async Task<Response<List<OnStudy>>> GetWordsOnStudy(bool only4Today, string userId, int page, int perPage, WordQueryRequest wqr)
        {
            var resp = new Response<List<OnStudy>>();
            int skipCount = (page - 1) * perPage;

            var query = context.OnStudies.AsQueryable();
            query = query.Where(d => d.UserId == userId);

            if (only4Today)
            {
                var today = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                query = query.Where(d => d.ShowAfter <= today);
            }

            if (wqr.ShouldIncludeExamples)
            {
                query = query.Include(d => d.Word)
                    .ThenInclude(w => w.Examples);
            }


            if (wqr.TranslationLanguages?.Count > 0)
            {
                query = query.Include(d => d.Word)
                    .ThenInclude(w => w.Translations.Where(t =>
                        wqr.TranslationLanguages.Contains(t.Language)
                    ));
            }

            resp.Data = await query.OrderByDescending(d => d.Id)
                                  .Include(d => d.Word)
                                  .Skip(skipCount)
                                  .Take(perPage)
                                  .ToListAsync();
            return resp;
        }

        public async Task<Response<OnStudy>> GetWordOnStudy(string userId, int wordId)
        {
            var resp = new Response<OnStudy>();
            resp.Data = context.OnStudies.Where(d => d.UserId == userId && d.WordId == wordId).FirstOrDefault();
            if (resp.Data == null)
            {
                resp.Errors.Add(new( ErrorCode.NotFound, "No such word on study"));
            }
            return resp;
        }

        public async Task<Response<bool>> IsWordOnStudy(int wordId)
        {
            var resp = new Response<bool>();
            var userId = httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            resp.Data = context.OnStudies.Where(d => d.UserId == userId && d.WordId == wordId).FirstOrDefault() != null;
            return resp;
        }


        public async Task<Response<bool>> RemoveWordOnStudy(string userId, int wordId)
        {
            var resp = new Response<bool>();
            var onStudyResp = await GetWordOnStudy(userId, wordId);
            if (!onStudyResp.Ok)
            {
                resp.Errors.Add(new(ErrorCode.NotFound, "No such word on study"));
            }
            else
            {
                context.OnStudies.Remove(onStudyResp.Data);
                await context.SaveChangesAsync();
                resp.Data = true;
            }
            return resp;
        }

        public async Task IncreaseOnStudyLvl(OnStudy onStudy)
        {
            onStudy.ShowAfter = DateTime.Today.AddDays(GetIntervalOffset(onStudy.Lvl));
            ++onStudy.Lvl;
            await context.SaveChangesAsync();
        }

        public async Task DecreaseOnStudyLvl(OnStudy onStudy)
        {
            onStudy.ShowAfter = DateTime.Today.AddDays(3);
            --onStudy.Lvl;
            await context.SaveChangesAsync();
        }

        int GetIntervalOffset(int currentLvl)
        {
            return currentLvl switch
            {
                1 => 3,
                2 => 7,
                3 => 14,
                4 => 30,
                5 => 75,
                6 => 183,
                _ => 365
            };
        }
    }
}

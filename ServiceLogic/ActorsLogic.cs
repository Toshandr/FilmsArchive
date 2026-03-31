using FilmsArchive.Models;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Cache;

namespace MovieAPI.Services
{
    public class ActorService
    {
        private readonly postgresContext _context;
        private readonly RedisCache? _cache;

        public ActorService(postgresContext context, RedisCache? cache = null)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<List<ActorDto>> GetAllActors()
        {
            // Проверяем Redis
            if (_cache != null)
            {
                var cached = await _cache.GetAsync<List<ActorDto>>("actors:all");
                if (cached != null)
                {
                    return cached;
                }
            }

            // Берём из БД
            var actors = await _context.Actors
                .Select(a => new ActorDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    BirthYear = a.Birthyear,
                    MoviesCount = a.Movieactors.Count
                })
                .ToListAsync();

            // Сохраняем в Redis
            if (_cache != null)
            {
                await _cache.SetAsync("actors:all", actors, TimeSpan.FromMinutes(10));
            }
            
            return actors;
        }

        public async Task<ActorDto> CreateActor(CreateActorDto createDto)
        {
            var actor = new Actor
            {
                Name = createDto.Name,
                Birthyear = createDto.BirthYear
            };
            
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            // Очищаем кеш. Без очистки при добавлении нового актера, список актеров в кеше будет устаревшим
            if (_cache != null)
            {
                await _cache.DeleteAsync("actors:all");
            }
            
            return new ActorDto
            {
                Id = actor.Id,
                Name = actor.Name,
                BirthYear = actor.Birthyear,
                MoviesCount = 0
            };
        }
    }
}
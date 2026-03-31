using FilmsArchive.Models;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Cache;

namespace MovieAPI.Services
{
    public class MovieService
    {
        private readonly postgresContext _context;
        private readonly RedisCache? _cache;

        public MovieService(postgresContext context, RedisCache? cache = null)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<List<MovieDto>> GetAllMovies()
        {
            // Проверяем Redis
            if (_cache != null)
            {
                var cached = await _cache.GetAsync<List<MovieDto>>("movies:all");
                if (cached != null) 
                {
                    return cached;
                }
            }

            // Берём из БД
            var movies = await _context.Movies
                .Include(m => m.Movieactors)
                .ThenInclude(ma => ma.Actor)
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    Genre = m.Genre,
                    Actors = m.Movieactors.Select(ma => new ActorInMovieDto
                    {
                        Id = ma.Actor.Id,
                        Name = ma.Actor.Name,
                        Role = ma.Role
                    }).ToList()
                })
                .ToListAsync();

            // Сохраняем в Redis
            if (_cache != null)
            {
                await _cache.SetAsync("movies:all", movies, TimeSpan.FromMinutes(10));
            }
            
            return movies;
        }

        public async Task<MovieDto> CreateMovie(CreateMovieDto createDto)
        {
            var movie = new Movie
            {
                Title = createDto.Title,
                Year = createDto.Year,
                Genre = createDto.Genre,
                Description = createDto.Description
            };
            
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            
            foreach (var actorDto in createDto.Actors)
            {
                _context.Movieactors.Add(new Movieactor
                {
                    Movieid = movie.Id,
                    Actorid = actorDto.ActorId,
                    Role = actorDto.Role
                });
            }
            
            await _context.SaveChangesAsync();
            
            await _context.Entry(movie)
                .Collection(m => m.Movieactors)
                .Query()
                .Include(ma => ma.Actor)
                .LoadAsync();

            // Очищаем кеш
            if (_cache != null) //без очистки при добавлении нового фильма, список фильмов в кеше будет устаревшим. Также так как при добавлении фильма может измениться количество фильмов у актера, то и список актеров в кеше будет устаревшим
            {
                await _cache.DeleteAsync("movies:all");
                await _cache.DeleteAsync("actors:all");
            }
            
            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                Actors = movie.Movieactors.Select(ma => new ActorInMovieDto
                {
                    Id = ma.Actor.Id,
                    Name = ma.Actor.Name,
                    Role = ma.Role
                }).ToList()
            };
        }
    }
}
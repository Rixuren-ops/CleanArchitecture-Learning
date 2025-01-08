
using CleanArchitecture.Data;
using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;

StreamerDbContext dbContext = new StreamerDbContext();

//await AddNewRecords();
//QueryStreaming();
//await QueryFilter();
//await QueryMethods();
//await QueryLinq();
//await TrackingAndNotTracking();
//await AddNewStreamerWithVideo();
//await AddNewStreamerWithVideoId();
//await AddNewActorWithVIdeo();
//await AddNewDirectorWithVideo();
await MultipleEntitiesQuery();

Console.WriteLine("Oprima un boton para terminar el programa");
Console.ReadKey();

async Task MultipleEntitiesQuery()
{
    //var VideoWithActors = await dbContext!.Videos!.Include(v => v.Actors).FirstOrDefaultAsync(q => q.Id == 1);
    //var actor = await dbContext!.Actors!.Select(a => a.Name).ToListAsync();
    var VideoWithDirector = await dbContext!.Videos!
                                  .Where(x => x.Director != null)
                                  .Include(v => v.Director)
                                  .Select(x =>
                                           new
                                           {
                                               MyDirector = $"{x.Director.Name} {x.Director.LastName}",
                                               Movie = x.Name
                                           })
                                  .ToListAsync();

    foreach (var item in VideoWithDirector)
    {
        Console.WriteLine($"Director: {item.MyDirector}, Movie: {item.Movie}");
    }
}

async Task AddNewDirectorWithVideo()
{
    var Director = new Director()
    {
        Name = "lorenzo",
        LastName = "Basteri",
        VideoId = 1
    };

    await dbContext!.AddAsync(Director);
    await dbContext!.SaveChangesAsync();

}
async Task AddNewActorWithVIdeo()
{
    var actor = new Actor
    {
        Name = "Brad",
        LastName = "Pitt"
    };

    await dbContext!.AddAsync(actor);
    await dbContext!.SaveChangesAsync();

    var VideoActor = new VideoActor
    {
        ActorId = actor.Id,
        VideoId = 1
    };

    await dbContext!.AddAsync(VideoActor);
    await dbContext!.SaveChangesAsync();
}

async Task AddNewRecords()
{

    Streamer streamer = new Streamer
    {
        Name = "Netflix",
        Url = "https://www.netflix.com"
    };

    Streamer streameDisney = new Streamer
    {
        Name = "Disney",
        Url = "https://www.Disney.com"
    };
    //dbContext!.Streamers!.Add(streamer);
    //await dbContext!.SaveChangesAsync();

    dbContext!.Streamers!.Add(streameDisney);
    await dbContext!.SaveChangesAsync();

    var movies = new List<Video>
{
    new Video {Name = "La Cenicienta", Streamer = streameDisney},
    new Video {Name = "101 Dalmatas", Streamer = streameDisney},
    new Video {Name = "El Jorobado de Notredame", Streamer = streameDisney},
    new Video {Name = "Star Wars", Streamer = streameDisney}
};

    await dbContext.AddRangeAsync(movies);
    await dbContext.SaveChangesAsync();
}

void QueryStreaming()
{
    var streamers = dbContext!.Streamers!.ToList();

    foreach (var streamer in streamers)
    {
        Console.WriteLine($"Streamer: {streamer.Name}");

    }
}

async Task QueryMethods()
{
    var streamer = dbContext!.Streamers!;
    var firstAsync = await streamer.Where(s => s.Name.Contains("a")).FirstAsync();
    var FirstOrDefaultAsync = await streamer.Where(s => s.Name.Contains("a")).FirstOrDefaultAsync();
    var FirstOrDefaultAsync_v2 = await streamer.FirstOrDefaultAsync(s => s.Name.Contains("a"));
    var singleAsync = await streamer.Where(s => s.Id == 1).SingleAsync(); ///debe de haber solo un registro
    var SingleOrDefaultAsync = await streamer.Where(s => s.Id == 1).SingleOrDefaultAsync(); ///debe de haber solo un registro

    var resultado = await streamer.FindAsync(1);

}
async Task QueryFilter()
{
    Console.WriteLine("Ingrese una compañia de streaming");
    var streamerName = Console.ReadLine();
    var streamers = await dbContext!.Streamers!.Where(s => s.Name.Equals(streamerName)).ToListAsync();

    foreach (var streamer in streamers)
    {
        Console.WriteLine($"Streamer: {streamer.Name}");
    }

    var streamerPartialResults = await dbContext!.Streamers!.Where(s => EF.Functions.Like(s.Name, $"%{streamerName}%")).ToListAsync();

    foreach (var streamer in streamerPartialResults)
    {
        Console.WriteLine($"Streamer: {streamer.Name}");
    }

}

async Task QueryLinq()
{
    Console.WriteLine("Ingrese una compañia de streaming");
    var streamerName = Console.ReadLine();
    var streamers = await (from i in dbContext!.Streamers
                           where EF.Functions.Like(i.Name, $"{streamerName}")
                           select i).ToListAsync();

    foreach (var streamer in streamers)
        Console.WriteLine($"Streamer: {streamer.Name}");
}

async Task AddNewStreamerWithVideo()
{
    var view = new Streamer
    {
        Name = "HBO",
        Url = "https://www.hbo.com"
    };

    var HungerGames = new Video
    {
        Name = "Hunger Games",
        Streamer = view
    };

    await dbContext!.AddAsync(HungerGames);
    await dbContext!.SaveChangesAsync();
}

async Task AddNewStreamerWithVideoId()
{

    var BatmanForever = new Video
    {
        Name = "Hunger Games",
        StreamerId = 1002
    };

    await dbContext!.AddAsync(BatmanForever);
    await dbContext!.SaveChangesAsync();
}

async Task TrackingAndNotTracking()
{
    var streamerWithTracking = await dbContext!.Streamers!.FirstOrDefaultAsync(x => x.Id == 1);
    var streamerWithOutTracking = await dbContext!.Streamers!.AsNoTracking().FirstOrDefaultAsync(x => x.Id == 2);

    streamerWithTracking!.Name = "Netflix Super";
    streamerWithOutTracking!.Name = "Amazon Super";

    await dbContext.SaveChangesAsync();
}

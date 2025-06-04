using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public enum WatchType
{
    WatchList,
    Watch
}

public class Film
{
    public string? Title { get; set; }
    public int Year { get; set; }
    public string? Director { get; set; }
    public Film(string title, int year, string director)
    {
        Title = title;
        Year = year;
        Director = director;
    }
}

public class FilmLog
{
    private List<Film> _films { get; set; }
    private Dictionary<Film, WatchType> _watch { get; set; }
    private Dictionary<Film, float> _rate { get; set; }
    private Dictionary<Film, string> _review { get; set; }
    private Dictionary<Film, bool> _star { get; set; }
    public FilmLog()
    {
        _films = new List<Film>();
        _watch = new Dictionary<Film, WatchType>();
        _rate = new Dictionary<Film, float>();
        _review = new Dictionary<Film, string>();
        _star = new Dictionary<Film, bool>();
    }
    public void AddFilm(string title, int year, string director)
    {
        Film film = new Film(title, year, director);
        _films.Add(film);
        _watch[film] = WatchType.WatchList;
        _rate[film] = 0;
        _review[film] = " ";
        _star[film] = false;
    }
    public void AddReview(Film film, string review) => _review[film] = review;
    public void AddRate(Film film, float rate) => _rate[film] = rate;
    public void AddWatch(Film film) => _watch[film] = WatchType.Watch;
    public void RemoveWatch(Film film) => _watch[film] = WatchType.WatchList;
    public void AddStar(Film film) => _star[film] = true;
    public void RemoveStar(Film film) => _star[film] = false;
    public bool HasStar(Film film) => _star.TryGetValue(film, out bool value) && value;
    public bool HasWatch(Film film) => _watch.TryGetValue(film, out WatchType wt) && wt != WatchType.WatchList;
    public Film SeekFilm(string title)
    {
        foreach (Film film in _films)
        {
            if (film.Title == title)
            {
                return film;
            }
        }
        throw new KeyNotFoundException($"Film with title '{title}' was not found.");
    }
    public void RemoveFilm(Film film)
    {
        _films.Remove(film);
        _review.Remove(film);
        _rate.Remove(film);
        _watch.Remove(film);
        _star.Remove(film);
    }
    public void UpdateFilm(Film film, string newTitle, int newYear, string newDirector)
    {
        film.Title = newTitle;
        film.Year = newYear;
        film.Director = newDirector;
    }
    public void FullDisplay()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("----------------------------------------------------------------------------------------------------------------------------------");
        sb.AppendLine($"{"Title",-30} {"Year",6} {"Director",-25} {"Watch",-10} {"Rate",6} {"Star",6} {"Review"}");
        sb.AppendLine("----------------------------------------------------------------------------------------------------------------------------------");

        foreach (Film film in _films)
        {
            string title = film.Title ?? "";
            string director = film.Director ?? "";
            string watch = _watch.TryGetValue(film, out var wt) ? wt.ToString() : "";
            string rate = _rate.TryGetValue(film, out var r) ? r.ToString("0.0") : "";
            string review = _review.TryGetValue(film, out var rev) ? rev : "";
            string star = _star.TryGetValue(film, out var st) && st ? "★" : "";

            sb.AppendLine($"{title,-30} {film.Year,6} {director,-25} {watch,-10} {rate,6} {star,6} {review}");
        }

        sb.AppendLine("----------------------------------------------------------------------------------------------------------------------------------");
        Console.WriteLine(sb.ToString());
    }
    public void FilmDisplay()
    {
        if (_films.Count > 0) Console.WriteLine("Summary:");
        foreach (Film film in _films)
        {
            string title = film.Title ?? "";
            string director = film.Director ?? "";
            Console.Write($"{title} ({film.Year}) dir, {director}.\n");
        }
    }
    public void LoadFromTxt()
    {
        if (!File.Exists("log.txt"))
        {
            return;
        }
        using (FileStream fs = new FileStream("log.txt", FileMode.Open, FileAccess.Read))
        using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split('|');

                string title = parts[0];
                if (!int.TryParse(parts[1], out int year)) continue;
                string director = parts[2];

                var film = new Film(title, year, director);
                _films.Add(film);

                if (parts.Length > 3 && Enum.TryParse(parts[3], out WatchType wt))
                    _watch[film] = wt;

                if (parts.Length > 4 && float.TryParse(parts[4], out float rating))
                    _rate[film] = rating;

                if (parts.Length > 5 && !string.IsNullOrWhiteSpace(parts[5]))
                    _review[film] = parts[5];

                if (parts.Length > 6 && bool.TryParse(parts[6], out bool star))
                    _star[film] = star;
            }
        }
    }
    public void SaveToTxt()
    {
        using (FileStream fs = new FileStream("log.txt", FileMode.OpenOrCreate, FileAccess.Write))
        using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
        {
            foreach (Film film in _films)
            {
                writer.Write($"{film.Title}|{film.Year}|{film.Director}");
                writer.Write($"|{_watch[film]}|{_rate[film]}|{_review[film]}|{_star[film]}");
                writer.WriteLine();
            }
        }
    }
    // ✅ JSON Serialization
    public void SerializeToJson()
    {
        var entries = new List<FilmEntry>();
        foreach (var film in _films)
        {
            entries.Add(new FilmEntry
            {
                Title = film.Title,
                Year = film.Year,
                Director = film.Director,
                Watch = _watch.TryGetValue(film, out var wt) ? wt : WatchType.WatchList,
                Rate = _rate.TryGetValue(film, out var r) ? r : 0f,
                Review = _review.TryGetValue(film, out var rv) ? rv : "",
                Star = _star.TryGetValue(film, out var s) && s
            });
        }

        var json = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("films.json", json);
    }

    // ✅ JSON Deserialization
    public void LoadFromJson()
    {
        if (!File.Exists("films.json")) return;

        string json = File.ReadAllText("films.json");
        var entries = JsonSerializer.Deserialize<List<FilmEntry>>(json);

        if (entries == null) return;

        _films.Clear();
        _watch.Clear();
        _rate.Clear();
        _review.Clear();
        _star.Clear();

        foreach (var entry in entries)
        {
            var film = new Film(entry.Title ?? "", entry.Year, entry.Director ?? "");
            _films.Add(film);
            _watch[film] = entry.Watch;
            _rate[film] = entry.Rate;
            _review[film] = entry.Review;
            _star[film] = entry.Star;
        }
    }
}

public class Display
{
    public Display()
    {
        clr();
        rite("Welcom ^_^\nEnter your name: ");
        if (Console.ReadLine()?.ToString().ToLower() == "luana")
        {
            clr();
            rite("Hai sayangku cintaku duniaku segala-galaku");
        }
        else
        {
            clr();
            rite("Welcome to your filmlog");
        }
    }
    private void rite(string msg) => Console.Write(msg);
    private void clr() => Console.Clear();
    public int MenuPrompt()
    {
        while (true)
        {
            string? input = Console.ReadLine();
            if (input != null && int.TryParse(input, out int result)) return result;
            rite("Invalid Input. Please try again. ");
        }
    }
    public void Menu()
    {
        rite("MyFilmLog\n\n 1. Add Film\n 2. Edit Film\n 3. Remove Film\n 4. Add Misc\n 5. View Films\n 6. Exit\n\n");
        rite("Insert an option: ");
    }
    public void MiscMenu()
    {
        rite(" 1. Add Review\n 2. Add Rate\n 3. Add/Remove Watch\n 4. Add/Remove Star\n 5. Back\n\n");
        rite("Insert an option: ");
    }
    public void WatchMenu()
    {
        rite("Watchtype = 1. Wathclist 2. Watched");
        rite("Insert an option: ");
    }
    public void Wait()
    {
        rite("\n\nPress any key to continue...");
        Console.ReadLine();
    }
}

public class FilmEntry
{
    public string? Title { get; set; }
    public int Year { get; set; }
    public string? Director { get; set; }
    public WatchType Watch { get; set; }
    public float Rate { get; set; }
    public string Review { get; set; } = "";
    public bool Star { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        FilmLog MyFilm = new FilmLog();
        MyFilm.LoadFromTxt();
        Display display = new Display();
        display.Wait();
        Film film;
        int menu;
        string? title;
        int year;
        string? director;
        while (true)
        {
            Console.Clear();
            MyFilm.FilmDisplay();
            Console.WriteLine();
            display.Menu();
            menu = display.MenuPrompt();
            Console.Clear();
            switch (menu)
            {
                case 1:
                    Console.WriteLine("New Film\n");
                    Console.Write("Title: ");
                    title = Console.ReadLine();
                    Console.Write("Year: ");
                    int.TryParse(Console.ReadLine(), out year);
                    Console.Write("Director: ");
                    director = Console.ReadLine();
                    MyFilm.AddFilm(title, year, director);
                    break;
                case 2:
                    Console.WriteLine("Update Film\n");
                    Console.Write("Old title: ");
                    film = MyFilm.SeekFilm(Console.ReadLine());
                    Console.Write("New title: ");
                    title = Console.ReadLine();
                    Console.Write("New year: ");
                    int.TryParse(Console.ReadLine(), out year);
                    Console.Write("New director: ");
                    director = Console.ReadLine();
                    MyFilm.UpdateFilm(film, title, year, director);
                    break;
                case 3:
                    Console.WriteLine("Remove Film\n");
                    Console.Write("Title: ");
                    film = MyFilm.SeekFilm(Console.ReadLine());
                    MyFilm.RemoveFilm(film);
                    break;
                case 4:
                    display.MiscMenu();
                    menu = display.MenuPrompt();
                    switch (menu)
                    {
                        case 1:
                            Console.WriteLine("Add Review\n");
                            Console.Write("Title: ");
                            film = MyFilm.SeekFilm(Console.ReadLine() ?? throw new Exception("No title entered."));
                            Console.Write("Review: ");
                            MyFilm.AddReview(film, Console.ReadLine() ?? "");
                            break;

                        case 2:
                            Console.WriteLine("Add Rate\n");
                            Console.Write("Title: ");
                            film = MyFilm.SeekFilm(Console.ReadLine() ?? throw new Exception("No title entered."));
                            Console.Write("Rate (0.0 - 10.0): ");
                            if (float.TryParse(Console.ReadLine(), out float rate)) MyFilm.AddRate(film, rate);
                            else Console.WriteLine("Invalid rate.");
                            break;
                        case 3:
                            Console.WriteLine("Toogle Watch\n");
                            Console.Write("Title: ");
                            film = MyFilm.SeekFilm(Console.ReadLine() ?? throw new Exception("No title entered."));
                            if (MyFilm.HasWatch(film)) MyFilm.RemoveWatch(film);
                            else MyFilm.AddWatch(film);
                            break;
                        case 4:
                            Console.WriteLine("Toggle Star\n");
                            Console.Write("Title: ");
                            film = MyFilm.SeekFilm(Console.ReadLine() ?? throw new Exception("No title entered."));
                            if (MyFilm.HasStar(film)) MyFilm.RemoveStar(film);
                            else MyFilm.AddStar(film);
                            break;
                        case 5:
                            Console.WriteLine("Returning to main menu...");
                            break;

                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                    break;
                case 5:
                    Console.Clear();
                    MyFilm.FullDisplay();
                    display.Wait();
                    break;
                case 6:
                    Console.Write("Thank You :)");
                    MyFilm.SaveToTxt();
                    Environment.Exit(0);
                    break;
                case 7:
                    MyFilm.SerializeToJson();
                    break;
                case 8:
                    MyFilm.LoadFromJson();
                    break;
                default:
                    Console.Write("Invalid");
                    break;
            }
        }
    }
}
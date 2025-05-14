using System;
using static System.Console;
using System.Collections.Generic;
using System.Linq;

public class Film {
    public int Id;
    public string Title;
    public string Director;
    public int Year;
    public Film(int id, string title, string director, int year){
        var setter = new FilmSetter();

        Id = id;
        Title = setter.SetFilm(title, "Title");
        Director = setter.SetFilm(director, "Director");
        Year = setter.SetFilm(year, "Year");
    }
    public void Display() {
        WriteLine($"{Id}. {Title} ({Year}), dir. {Director}");
    }
}
public class ListFilm{
    public List<Film> films = new List<Film>();
    public void AddFilm(int id) {
        WriteLine("Please input the title, director, and year...");
        string title = ReadLine().ToString();
        string director = ReadLine().ToString();
        int year = int.Parse(ReadLine());
        films.Add(new Film(id, title, director, year));
    }
    public void RemoveFilm() {
        WriteLine("Please input the corresponding Id...");
        int id = int.Parse(ReadLine());
        films.RemoveAll(r => r.Id == id);
    }
    public void ViewAll() {
        foreach (var film in films) {
            film.Display();
        }
    }
}

public class FilmSetter {
    public string SetFilm(string? data, string askedData)
    {
        if (string.IsNullOrWhiteSpace(data))
            data = FilmRequester.RequestFilm(askedData);
        return data;
    }
    //OverLoading
    public int SetFilm(int data, string askedData){
        if (data == 0)
            data = int.Parse(FilmRequester.RequestFilm(askedData));
        return data;
    }
}

public class FilmRequester {
    public static string RequestFilm(string askedData){
        WriteLine($"Please input {askedData} again:");
        var input = ReadLine();
        if (string.IsNullOrWhiteSpace(input)) {
            return RequestFilm(askedData);
        }
        return input;
    }
}

/*public class FilmAsker {
    public static void AskFilm () {
        WriteLine("How many films do you have?");
       int  n = int.Parse(ReadLine())
        for (int i=0; i<n; i++) {
            WriteLine($"Input film {0} with format like this: The Batman, Robert, 2019");
            films = ReadLine().Split(',');
            foreach (film in films) {
                var 
            }
        }
    }
}*/

class Program {
    static void Main() {
        string TextMenu = "MyFilm List\n  1. Add Film\n  2. Remove Film\n  3. View MyFilm\n  4. Exit";
        ListFilm MyFilmList = new ListFilm();
        int Id = 1;
        bool exit = false;
        while (!exit) {
            WriteLine(TextMenu);
            int UserInput = int.Parse(ReadLine());
            switch (UserInput){
                case 1:
                MyFilmList.AddFilm(Id);
                Id++;
                    break;
                case 2:
                MyFilmList.RemoveFilm();
                    break;
                case 3:
                MyFilmList.ViewAll();
                    break;
                case 4:
                    exit = true;
                    break;
            }
        }
    }
}
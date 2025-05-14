using System;

namespace FilmMVC.Models {
    // error di sini di bagian Home/Create
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
    }

    public class FilmSetter {
        public string SetFilm(string? data, string askedData)
        {
            if (string.IsNullOrWhiteSpace(data))
                data = FilmRequester.RequestFilm(askedData);
            return data;
        }

        public int SetFilm(int data, string askedData){
            if (data == 0)
                data = int.Parse(FilmRequester.RequestFilm(askedData));
            return data;
        }
    }

    public class FilmRequester {
        public static string RequestFilm(string askedData){
            Console.WriteLine($"Please input {askedData} again:");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return RequestFilm(askedData);
            return input;
        }
    }
    public class ErrorViewModel{
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

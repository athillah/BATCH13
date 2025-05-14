class Program {
    static void Main(string[] args) {
        var p = new Person("John", "Doe");
        Console.WriteLine(p.Name);
    }
}

class Person {
    public Person (string name, string surname) {
        Name = name;
        Surname = surname;
    }
    public string Name;
    public string Surname;
}
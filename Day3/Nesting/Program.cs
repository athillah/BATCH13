using System;
using static System.Console;

[Flags]
public enum Department {
    Engineering = 5, Production = 4, Maintainance = 4,
    Finance = 5, Marketing = 5, Procurement = 4,
}

[Flags]
public enum Level {
    Manager = 8, Director = 16,
    Specialist = 4, Supervisor = 6,
    Technician = 2, Administrator = 1,
}

interface IPerson {
    string Name { get; set; }
}

interface IEmployee : IPerson {
    int Id { get; set; }
    Department Department { get; set; }
    Level Level { get; set; }
    interface IPayRoll {
        double GetSalary();
    }
}

public class Engineer : IEmployee {
    public string Name { get; set; }
    public int Id { get; set; }
    public Department Department { get; set; }
    public Level Level { get; set; }
    private double _salary { get; set; }
    public Engineer (string name, int id, Department department, Level level) {
        Name = name; Id = id; Department = department; Level = level;
    }
    public class Salary : IEmployee.IPayRoll {
        private readonly Engineer _engineer;
        public Salary(Engineer engineer) {
            _engineer = engineer;
            _engineer._salary = GetSalary();
        }
        public double GetSalary() {
            const int baseSalary = 100;
            double salary = ((baseSalary+(double)_engineer.Department)*(double)_engineer.Level);
            WriteLine($"{salary:C}");
            return salary;
        }
    }
}

public class Intern : Engineer {
    public Intern(string name, int id, Department department) : base(name, id, department, Level.Administrator) {
        WriteLine($"{Id}. {Name}, {Department} {Level}");
    }
    public class InternModifier {
        public double SalaryModifier (double salary) {
            Random rng = new Random();
            int modifier = rng.Next(3);
            salary = salary / (modifier + 2);
            return salary;
        }
    }
}

class Program
{
    static void Main()
    {
        List<IEmployee> Engineers = new List<IEmployee>() {
            new Engineer("Asep", 1, Department.Procurement, Level.Director),
            new Engineer("Bobi", 2, Department.Maintainance, Level.Specialist),
            new Engineer("Carli", 3, Department.Finance, Level.Supervisor),
            new Engineer("Denis", 4, Department.Marketing, Level.Administrator),
            new Engineer("Eva", 5, Department.Engineering, Level.Technician),
            new Engineer("Farah", 6, Department.Production, Level.Manager),
            new Engineer("Gunawan", 7, Department.Finance, Level.Director)
        };

        foreach (Engineer engineer in Engineers) {
            Engineer.Salary salaryProcessor = new Engineer.Salary(engineer);
            Console.WriteLine($"{engineer.Id}. {engineer.Name}, {engineer.Department} {engineer.Level}");
            salaryProcessor.GetSalary();
        }

        Intern Ahmad = new Intern("Herman", 8, Department.Marketing);
        Engineer.Salary SalaryProcessor = new Engineer.Salary(Ahmad);
        double baseSalary = SalaryProcessor.GetSalary();

        Intern.InternModifier mod = new Intern.InternModifier();
        double adjustedSalary = mod.SalaryModifier(baseSalary);
        WriteLine($"Modified Intern Salary: {adjustedSalary:C}");
    }
}
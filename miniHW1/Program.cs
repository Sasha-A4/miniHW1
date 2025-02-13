using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;




public interface IAlive
{
    int Food { get;  }
}

public interface IInventory
{
    int Number { get; }
}

public abstract class Animal : IAlive, IInventory
{
    public int Food { get; protected set; }
    public int Number { get; private set; }

    public Animal(int number, int food)
    {
        Number = number;
        Food = food;
    }
}

public abstract class Herbo : Animal
{
    public int Kindness { get; protected set; }

    public Herbo(int number, int food, int kindness) : base(number, food)
    {
        Kindness = kindness;
    }
}


public abstract class Predator : Animal
{
    public Predator(int number, int food) : base(number, food) { }
}


public class Monkey : Herbo
{
    public Monkey(int number, int food, int kindness) : base(number, food, kindness) { }
}

public class Rabbit : Herbo
{
    public Rabbit(int number, int food, int kindness) : base(number, food, kindness) { }
}

public class Tiger : Predator
{
    public Tiger(int number, int food) : base(number, food) { }
}

public class Wolf : Predator
{
    public Wolf(int number, int food) : base(number, food) { }
}



public class Thing : IInventory
{
    public int Number { get; private set; }
    public Thing(int number) { Number = number; }
}

public class Table : Thing
{
    public Table(int number) : base(number) { }
}

public class Computer : Thing
{
    public Computer(int number) : base(number) { }
}



public class Vet
{
    public bool Check(Animal animal)
    {
        return new Random().Next(2) == 1;
    }
}


public class Zoo
{
    private readonly Vet _vet;
    private readonly List<Animal> _animals = new List<Animal>();
    private readonly List<IInventory> _inventory = new List<IInventory>();


    public Zoo(Vet vet)
    {
        _vet = vet;
    }

    public void AddAnimal(Animal animal)
    {
        if (_vet.Check(animal))
        {
            _animals.Add(animal);
            _inventory.Add(animal);
            Console.WriteLine($"Животное {animal.GetType().Name} прошло проверку здоровья и принято в зоопарк.");
        }
        else
        {
            Console.WriteLine($"Животное {animal.GetType().Name} не прошло проверку здоровья.");
        }
    }

    public void AddThing(IInventory item)
    {
        _inventory.Add(item);
    }

    public void ReportFood()
    {
        int allFood = _animals.Sum(a => a.Food);
        Console.WriteLine($"Общее количество потребляемой еды: {allFood} кг в сутки.");
    }

    public void ContactZoo()
    {
        var contactAnimals = _animals.OfType<Herbo>().Where(h => h.Kindness > 5);
        Console.WriteLine("Животные для контактного зоопарка:");
        foreach (var animal in contactAnimals)
        {
            Console.WriteLine(animal.GetType().Name);
        }
    }

    public void ListInventory()
    {
        Console.WriteLine("Инвентаризационные вещи и животные ");
        foreach (var item in _inventory)
        {
            Console.WriteLine($"{item.GetType().Name} - Инвентарный номер {item.Number}");
        }
    }
}


class Program
{
    static void Main()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<Vet>()
            .AddSingleton<Zoo>()
            .BuildServiceProvider();


        var zoo = serviceProvider.GetService<Zoo>();

        zoo.AddAnimal(new Monkey(1, 3, 7));
        zoo.AddAnimal(new Rabbit(2, 2, 4));
        zoo.AddAnimal(new Tiger(3, 5));
        zoo.AddAnimal(new Wolf(4, 6));

        zoo.AddThing(new Table(101));
        zoo.AddThing(new Computer(102));

        zoo.ReportFood();
        zoo.ContactZoo();
        zoo.ListInventory();

    }
}






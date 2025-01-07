using System;

#region Задание №1 - Учебный класс ClassRoom и ученики
// Базовый класс ученик
class Pupil
{
    public virtual void Study()
    {
        Console.WriteLine("Ученик учится.");
    }

    public virtual void Read()
    {
        Console.WriteLine("Ученик читает.");
    }

    public virtual void Write()
    {
        Console.WriteLine("Ученик пишет.");
    }

    public virtual void Relax()
    {
        Console.WriteLine("Ученик отдыхает.");
    }
}

// Отличный ученик
class ExcelentPupil : Pupil
{
    public override void Study()
    {
        Console.WriteLine("Отличный ученик учится отлично.");
    }

    public override void Read()
    {
        Console.WriteLine("Отличный ученик читает быстро.");
    }

    public override void Write()
    {
        Console.WriteLine("Отличный ученик пишет без ошибок.");
    }

    public override void Relax()
    {
        Console.WriteLine("Отличный ученик мало отдыхает.");
    }
}

// Хороший ученик
class GoodPupil : Pupil
{
    public override void Study()
    {
        Console.WriteLine("Хороший ученик учится хорошо.");
    }

    public override void Read()
    {
        Console.WriteLine("Хороший ученик читает уверенно.");
    }

    public override void Write()
    {
        Console.WriteLine("Хороший ученик пишет с небольшими ошибками.");
    }

    public override void Relax()
    {
        Console.WriteLine("Хороший ученик отдыхает в меру.");
    }
}

// Плохой ученик
class BadPupil : Pupil
{
    public override void Study()
    {
        Console.WriteLine("Плохой ученик учится плохо.");
    }

    public override void Read()
    {
        Console.WriteLine("Плохой ученик читает медленно.");
    }

    public override void Write()
    {
        Console.WriteLine("Плохой ученик пишет с ошибками.");
    }

    public override void Relax()
    {
        Console.WriteLine("Плохой ученик много отдыхает.");
    }
}

// Класс для управления учебным классом
class ClassRoom
{
    private Pupil[] pupils = new Pupil[4];

    public ClassRoom(params Pupil[] pupils)
    {
        for (int i = 0; i < pupils.Length && i < 4; i++)
        {
            this.pupils[i] = pupils[i];
        }
    }

    public void ShowPupilsInfo()
    {
        foreach (Pupil pupil in pupils)
        {
            if (pupil != null)
            {
                pupil.Study();
                pupil.Read();
                pupil.Write();
                pupil.Relax();
                Console.WriteLine();
            }
        }
    }
}
#endregion

#region Задание №2 - Транспортные средства
// Базовый класс Транспорт
class Vehicle
{
    public double Price { get; set; }
    public double Speed { get; set; }
    public int Year { get; set; }

    public Vehicle(double price, double speed, int year)
    {
        Price = price;
        Speed = speed;
        Year = year;
    }

    public virtual void ShowInfo()
    {
        Console.WriteLine($"Цена: {Price}, Скорость: {Speed}, Год выпуска: {Year}");
    }
}


class Plane : Vehicle
{
    public double Height { get; set; }
    public int Passengers { get; set; }

    public Plane(double price, double speed, int year, double height, int passengers)
        : base(price, speed, year)
    {
        Height = height;
        Passengers = passengers;
    }

    public override void ShowInfo()
    {
        base.ShowInfo();
        Console.WriteLine($"Высота: {Height}, Пассажиры: {Passengers}");
    }
}

class Car : Vehicle
{
    public Car(double price, double speed, int year)
        : base(price, speed, year)
    {
    }

    public override void ShowInfo()
    {
        base.ShowInfo();
        Console.WriteLine("Это автомобиль.");
    }
}

class Ship : Vehicle
{
    public int Passengers { get; set; }
    public string Port { get; set; }

    public Ship(double price, double speed, int year, int passengers, string port)
        : base(price, speed, year)
    {
        Passengers = passengers;
        Port = port;
    }

    public override void ShowInfo()
    {
        base.ShowInfo();
        Console.WriteLine($"Пассажиры: {Passengers}, Порт приписки: {Port}");
    }
}
#endregion

#region Задание №3 - Документы
// Базовый класс DocumentWorker
class DocumentWorker
{
    public virtual void OpenDocument()
    {
        Console.WriteLine("Документ открыт.");
    }

    public virtual void EditDocument()
    {
        Console.WriteLine("Редактирование документа доступно в версии Pro.");
    }

    public virtual void SaveDocument()
    {
        Console.WriteLine("Сохранение документа доступно в версии Pro.");
    }
}

// Pro версия
class ProDocumentWorker : DocumentWorker
{
    public override void EditDocument()
    {
        Console.WriteLine("Документ отредактирован.");
    }

    public override void SaveDocument()
    {
        Console.WriteLine("Документ сохранен в старом формате, сохранение в остальных форматах доступно в версии Expert.");
    }
}

// Expert версия
class ExpertDocumentWorker : ProDocumentWorker
{
    public override void SaveDocument()
    {
        Console.WriteLine("Документ сохранен в новом формате.");
    }
}
#endregion

class Program
{
    static void Main()
    {
        #region Задание №1 - Демонстрация ClassRoom
        Console.WriteLine("Задание №1 - ClassRoom:");
        Pupil Vanya = new ExcelentPupil();
        Pupil Dima = new GoodPupil();
        Pupil Ilya = new BadPupil();
        Pupil Anton = new GoodPupil();
        ClassRoom classRoom = new ClassRoom(Vanya, Dima, Ilya);
        classRoom.ShowPupilsInfo();
        #endregion

        #region Задание №2 - Демонстрация транспорта
        Console.WriteLine("Задание №2 - Транспортные средства:");
        Vehicle plane = new Plane(1000000, 900, 2018, 10000, 180);
        Vehicle car = new Car(30000, 200, 2020);
        Vehicle ship = new Ship(500000, 80, 2015, 500, "Одесса");

        plane.ShowInfo();
        Console.WriteLine();
        car.ShowInfo();
        Console.WriteLine();
        ship.ShowInfo();
        Console.WriteLine();
        #endregion

        #region Задание №3 - Демонстрация DocumentWorker
        Console.WriteLine("Задание №3 - DocumentWorker:");

        Console.WriteLine("Введите ключ доступа (pro/exp):");
        string key = Console.ReadLine();
        DocumentWorker worker;

        if (key == "pro")
        {
            worker = new ProDocumentWorker();
        }
        else if (key == "exp")
        {
            worker = new ExpertDocumentWorker();
        }
        else
        {
            worker = new DocumentWorker();
        }

        worker.OpenDocument();
        worker.EditDocument();
        worker.SaveDocument();
        #endregion
    }
}

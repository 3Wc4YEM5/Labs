using System;
using System.Collections;
using System.Collections.Generic;

public class MyMatrix
{
    private int[,] matrix;
    private int rows;
    private int cols;

    public MyMatrix(int rows, int cols, int minValue, int maxValue)
    {
        this.rows = rows;
        this.cols = cols;
        matrix = new int[rows, cols];
        Random rand = new Random();

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                matrix[i, j] = rand.Next(minValue, maxValue);
    }

    public int this[int row, int col]
    {
        get => matrix[row, col];
        set => matrix[row, col] = value;
    }

    public static MyMatrix operator +(MyMatrix a, MyMatrix b)
    {
        MyMatrix result = new MyMatrix(a.rows, a.cols, 0, 1);
        for (int i = 0; i < a.rows; i++)
            for (int j = 0; j < a.cols; j++)
                result[i, j] = a[i, j] + b[i, j];
        return result;
    }

    public static MyMatrix operator -(MyMatrix a, MyMatrix b)
    {
        MyMatrix result = new MyMatrix(a.rows, a.cols, 0, 1);
        for (int i = 0; i < a.rows; i++)
            for (int j = 0; j < a.cols; j++)
                result[i, j] = a[i, j] - b[i, j];
        return result;
    }

    public static MyMatrix operator *(MyMatrix a, MyMatrix b)
    {
        MyMatrix result = new MyMatrix(a.rows, b.cols, 0, 1);
        for (int i = 0; i < a.rows; i++)
            for (int j = 0; j < b.cols; j++)
                for (int k = 0; k < a.cols; k++)
                    result[i, j] += a[i, k] * b[k, j];
        return result;
    }

    public static MyMatrix operator *(MyMatrix a, int scalar)
    {
        MyMatrix result = new MyMatrix(a.rows, a.cols, 0, 1);
        for (int i = 0; i < a.rows; i++)
            for (int j = 0; j < a.cols; j++)
                result[i, j] = a[i, j] * scalar;
        return result;
    }

    public static MyMatrix operator /(MyMatrix a, int scalar)
    {
        MyMatrix result = new MyMatrix(a.rows, a.cols, 0, 1);
        for (int i = 0; i < a.rows; i++)
            for (int j = 0; j < a.cols; j++)
                result[i, j] = a[i, j] / scalar;
        return result;
    }
}

public class Car
{
    public string Name { get; set; }
    public int ProductionYear { get; set; }
    public int MaxSpeed { get; set; }
}

public class CarComparer : IComparer<Car>
{
    public enum SortCriteria { Name, ProductionYear, MaxSpeed }

    private SortCriteria criteria;

    public CarComparer(SortCriteria criteria)
    {
        this.criteria = criteria;
    }

    public int Compare(Car x, Car y)
    {
        return criteria switch
        {
            SortCriteria.Name => string.Compare(x.Name, y.Name),
            SortCriteria.ProductionYear => x.ProductionYear.CompareTo(y.ProductionYear),
            SortCriteria.MaxSpeed => x.MaxSpeed.CompareTo(y.MaxSpeed),
            _ => 0
        };
    }
}

public class CarCatalog : IEnumerable<Car>
{
    private List<Car> cars = new List<Car>();

    public void AddCar(Car car)
    {
        cars.Add(car);
    }

    public IEnumerator<Car> GetEnumerator() => cars.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<Car> GetCars()
    {
        for (int i = 0; i < cars.Count; i++)
        {
            yield return cars[i];
        }
    }

    // Обратный проход с последнего до первого элемента
    public IEnumerable<Car> GetCarsReversed()
    {
        for (int i = cars.Count - 1; i >= 0; i--)
        {
            yield return cars[i];
        }
    }
    public IEnumerable<Car> FilterByYear(int year)
    {
        foreach (var car in cars)
        {
            if (car.ProductionYear == year)
                yield return car;
        }
    }
    public IEnumerable<Car> FilterByMaxSpeed(int speed)
    {
        foreach (var car in cars)
        {
            if (car.MaxSpeed >= speed)
                yield return car;
        }
    }
}

class Program
{
    static void Main()
    {
        // Задание 1
        Console.WriteLine("Введите количество строк матрицы:");
        int rows = int.Parse(Console.ReadLine());

        Console.WriteLine("Введите количество столбцов матрицы:");
        int cols = int.Parse(Console.ReadLine());

        Console.WriteLine("Введите минимальное значение:");
        int minValue = int.Parse(Console.ReadLine());
        Console.WriteLine("Введите максимальное значение:");
        int maxValue = int.Parse(Console.ReadLine());

        var matrix = new MyMatrix(rows, cols, minValue, maxValue);
        Console.WriteLine("Сгенерированная матрица:");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(matrix[i, j] + " ");
            }
            Console.WriteLine();
        }

        // Задание 2
        var cars = new Car[]
        {
            new Car { Name = "Toyota", ProductionYear = 2020, MaxSpeed = 180 },
            new Car { Name = "Honda", ProductionYear = 2018, MaxSpeed = 200 },
            new Car { Name = "Ford", ProductionYear = 2022, MaxSpeed = 220 }
        };

        Array.Sort(cars, new CarComparer(CarComparer.SortCriteria.Name));
        Console.WriteLine("\nСортировка по имени:");
        foreach (var car in cars)
            Console.WriteLine(car.Name);

        Array.Sort(cars, new CarComparer(CarComparer.SortCriteria.ProductionYear));
        Console.WriteLine("\nСортировка по году выпуска:");
        foreach (var car in cars)
            Console.WriteLine(car.ProductionYear);

        Array.Sort(cars, new CarComparer(CarComparer.SortCriteria.MaxSpeed));
        Console.WriteLine("\nСортировка по максимальной скорости:");
        foreach (var car in cars)
            Console.WriteLine(car.MaxSpeed);

        // Задание 3
        var catalog = new CarCatalog();
        catalog.AddCar(new Car { Name = "Toyota", ProductionYear = 2020, MaxSpeed = 180 });
        catalog.AddCar(new Car { Name = "Honda", ProductionYear = 2018, MaxSpeed = 200 });
        catalog.AddCar(new Car { Name = "Ford", ProductionYear = 2022, MaxSpeed = 220 });

        Console.WriteLine("\nВсе автомобили:");
        foreach (var car in catalog)
            Console.WriteLine(car.Name);

        Console.WriteLine("\nАвтомобили 2020 года:");
        foreach (var car in catalog.FilterByYear(2020))
            Console.WriteLine(car.Name);

        Console.WriteLine("\nАвтомобили с максимальной скоростью >= 200:");
        foreach (var car in catalog.FilterByMaxSpeed(200))
            Console.WriteLine(car.Name);
    }
}
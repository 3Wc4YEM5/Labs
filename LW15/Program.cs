using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json; 




// Интерфейс наблюдателя
public interface IFileWatcherObserver
{
    void OnFileAdded(string filePath);
    void OnFileRemoved(string filePath);
    void OnFileChanged(string filePath);
    void OnDirectoryAdded(string directoryPath);
    void OnDirectoryRemoved(string directoryPath);
    void OnDirectoryChanged(string directoryPath);
}

// Класс наблюдаемого объекта
public class FileSystemWatcherEmulator
{
    private readonly string _directoryPath;
    private readonly HashSet<string> _knownFiles;
    private readonly Dictionary<string, DateTime> _fileModificationTimes;
    private readonly HashSet<string> _knownDirectories;
    private readonly List<IFileWatcherObserver> _observers;
    private readonly Timer _timer;
    private readonly int _pollingInterval;

    public FileSystemWatcherEmulator(string directoryPath, int pollingInterval = 1000)
    {
        if (!Directory.Exists(directoryPath))
            throw new ArgumentException($"Directory {directoryPath} does not exist.");

        _directoryPath = directoryPath;
        _knownFiles = new HashSet<string>(Directory.GetFiles(_directoryPath));
        _fileModificationTimes = _knownFiles.ToDictionary(f => f, f => File.GetLastWriteTime(f));
        _knownDirectories = new HashSet<string>(Directory.GetDirectories(_directoryPath));
        _observers = new List<IFileWatcherObserver>();
        _pollingInterval = pollingInterval;
        _timer = new Timer(CheckForChanges, null, 0, _pollingInterval);
    }

    // Подписка на изменения
    public void Attach(IFileWatcherObserver observer)
    {
        _observers.Add(observer);
    }

    // Отписка от изменений
    public void Detach(IFileWatcherObserver observer)
    {
        _observers.Remove(observer);
    }

    private void CheckForChanges(object state)
    {
        var currentFiles = new HashSet<string>(Directory.GetFiles(_directoryPath));
        var currentFileModificationTimes = currentFiles.ToDictionary(f => f, f => File.GetLastWriteTime(f));
        var currentDirectories = new HashSet<string>(Directory.GetDirectories(_directoryPath));

        // Проверка файлов

        // Найти добавленные файлы
        var addedFiles = currentFiles.Except(_knownFiles).ToList();
        foreach (var file in addedFiles)
        {
            NotifyFileAdded(file);
        }

        // Найти удалённые файлы
        var removedFiles = _knownFiles.Except(currentFiles).ToList();
        foreach (var file in removedFiles)
        {
            NotifyFileRemoved(file);
        }

        // Найти изменённые файлы
        var modifiedFiles = currentFiles.Where(f => _knownFiles.Contains(f) && _fileModificationTimes[f] != currentFileModificationTimes[f]).ToList();
        foreach (var file in modifiedFiles)
        {
            NotifyFileChanged(file);
        }

        // Проверка директорий

        // Найти добавленные директории
        var addedDirectories = currentDirectories.Except(_knownDirectories).ToList();
        foreach (var directory in addedDirectories)
        {
            NotifyDirectoryAdded(directory);
        }

        // Найти удалённые директории
        var removedDirectories = _knownDirectories.Except(currentDirectories).ToList();
        foreach (var directory in removedDirectories)
        {
            NotifyDirectoryRemoved(directory);
        }

        // Найти изменённые директории (проверяем время последнего изменения)
        var modifiedDirectories = currentDirectories.Where(d => _knownDirectories.Contains(d) && Directory.GetLastWriteTime(d) != Directory.GetLastWriteTime(d)).ToList();
        foreach (var directory in modifiedDirectories)
        {
            NotifyDirectoryChanged(directory);
        }

        // Обновить известное состояние
        _knownFiles.Clear();
        _fileModificationTimes.Clear();
        foreach (var file in currentFiles)
        {
            _knownFiles.Add(file);
            _fileModificationTimes[file] = currentFileModificationTimes[file];
        }

        _knownDirectories.Clear();
        foreach (var directory in currentDirectories)
        {
            _knownDirectories.Add(directory);
        }
    }

    private void NotifyFileAdded(string filePath)
    {
        foreach (var observer in _observers)
        {
            observer.OnFileAdded(filePath);
        }
    }

    private void NotifyFileRemoved(string filePath)
    {
        foreach (var observer in _observers)
        {
            observer.OnFileRemoved(filePath);
        }
    }

    private void NotifyFileChanged(string filePath)
    {
        foreach (var observer in _observers)
        {
            observer.OnFileChanged(filePath);
        }
    }

    private void NotifyDirectoryAdded(string directoryPath)
    {
        foreach (var observer in _observers)
        {
            observer.OnDirectoryAdded(directoryPath);
        }
    }

    private void NotifyDirectoryRemoved(string directoryPath)
    {
        foreach (var observer in _observers)
        {
            observer.OnDirectoryRemoved(directoryPath);
        }
    }

    private void NotifyDirectoryChanged(string directoryPath)
    {
        foreach (var observer in _observers)
        {
            observer.OnDirectoryChanged(directoryPath);
        }
    }

    public void Stop()
    {
        _timer.Dispose();
    }
}

// Интерфейс репозитория
public interface ILoggerRepository
{
    void WriteLog(string message);
}

// Репозиторий для текстового файла
public class TextFileLoggerRepository : ILoggerRepository
{
    private readonly string _filePath;

    public TextFileLoggerRepository(string filePath)
    {
        _filePath = filePath;
    }

    public void WriteLog(string message)
    {
        File.AppendAllText(_filePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
    }
}

// Репозиторий для JSON-файла
public class JsonFileLoggerRepository : ILoggerRepository
{
    private readonly string _filePath;
    private readonly List<string> _logEntries;

    public JsonFileLoggerRepository(string filePath)
    {
        _filePath = filePath;

        // Если файл существует, загружаем существующие логи, иначе создаем новый список
        if (File.Exists(_filePath))
        {
            var existingContent = File.ReadAllText(_filePath);
            _logEntries = JsonConvert.DeserializeObject<List<string>>(existingContent) ?? new List<string>();
        }
        else
        {
            _logEntries = new List<string>();
        }
    }

    public void WriteLog(string message)
    {
        _logEntries.Add($"{DateTime.Now}: {message}");
        File.WriteAllText(_filePath, JsonConvert.SerializeObject(_logEntries, Formatting.Indented));
    }
}

// Основной класс MyLogger
public class MyLogger
{
    private readonly ILoggerRepository _repository;

    public MyLogger(ILoggerRepository repository)
    {
        _repository = repository;
    }

    public void Log(string message)
    {
        _repository.WriteLog(message);
    }
}


// Пример класса-наблюдателя
public class FileWatcherObserver : IFileWatcherObserver
{
    public void OnFileAdded(string filePath)
    {
        Console.WriteLine($"File added: {filePath}");
    }

    public void OnFileRemoved(string filePath)
    {
        Console.WriteLine($"File removed: {filePath}");
    }

    public void OnFileChanged(string filePath)
    {
        Console.WriteLine($"File changed: {filePath}");
    }

    public void OnDirectoryAdded(string directoryPath)
    {
        Console.WriteLine($"Directory added: {directoryPath}");
    }

    public void OnDirectoryRemoved(string directoryPath)
    {
        Console.WriteLine($"Directory removed: {directoryPath}");
    }

    public void OnDirectoryChanged(string directoryPath)
    {
        Console.WriteLine($"Directory changed: {directoryPath}");
    }
}


// Класс SingleRandomizer реализует паттерн "Одиночка"
public class SingleRandomizer
{
    private static SingleRandomizer _instance;
    private static readonly object _syncRoot = new object();
    private readonly Random _random;

    
    private SingleRandomizer()
    {
        _random = new Random();
    }


    public static SingleRandomizer Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new SingleRandomizer();
                    }
                }
            }
            return _instance;
        }
    }

    // Метод для получения следующего случайного числа
    public int Next(int minValue, int maxValue)
    {
        lock (_syncRoot) 
        {
            return _random.Next(minValue, maxValue);
        }
    }



}

// Тестирование
public class Program
{
    public static void Main()
    {
        var directoryPath = "./test_directory"; 
        Directory.CreateDirectory(directoryPath); 

        var watcher = new FileSystemWatcherEmulator(directoryPath);
        var observer = new FileWatcherObserver();
        watcher.Attach(observer);

        Console.WriteLine("File watcher started. Press Enter to exit.");
        Console.ReadLine();

        watcher.Stop();








                // Репозиторий для текстового файла
        var textLogger = new MyLogger(new TextFileLoggerRepository("log.txt"));
        textLogger.Log("This is a log message to a text file.");

        // Репозиторий для JSON-файла
        var jsonLogger = new MyLogger(new JsonFileLoggerRepository("log.json"));
        jsonLogger.Log("This is a log message to a JSON file.");

        Console.WriteLine("Logs have been written to the respective targets.");












        Thread thread1 = new Thread(() => PrintRandomNumbers("Thread 1"));
        Thread thread2 = new Thread(() => PrintRandomNumbers("Thread 2"));

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();
    }
    private static void PrintRandomNumbers(string threadName)
    {
        for (int i = 0; i < 5; i++)
        {
            int randomValue = SingleRandomizer.Instance.Next(1, 100);
            Console.WriteLine($"{threadName}: {randomValue}");
        }
    }
}


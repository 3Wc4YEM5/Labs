using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CSharpProject
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Ticker> Tickers => Set<Ticker>();
        public DbSet<Price> Prices => Set<Price>();
        public DbSet<TodaysCondition> TodaysConditions => Set<TodaysCondition>();
        public ApplicationContext() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=helloapp.db");
        }
    }

    public class Price
    {
        public int Id { get; set; }
        public int? tickerId { get; set; }
        public double price { get; set; }
        public string? date { get; set; }
        public string? ticker { get; set; }
    }
    public class Ticker
    {
        public int Id { get; set; }
        public string? ticker { get; set; }
    }
    public class TodaysCondition
    {
        public int Id { get; set; }
        public int? tickerId { get; set; }
        public double? state { get; set; }
        public string? ticker { get; set; }
    }

    class Lab09
    {
        static readonly Mutex mutex = new();

        static async Task Main()
        {
            Ticker obj = new Ticker();

            List<string> tickers = [];

            using (StreamReader reader = new("ticker.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {//будет выполнять итерации до тех пор, пока ReadLine() не вернет null. Внутри цикла считываем каждую строку и добавляем ее в список tickers
                    tickers.Add(line);

                }

            }

            List<Task> tasks = [];
            int i = 1;
            foreach (string ticker in tickers)
            {

                tasks.Add(GetDataForTicker(ticker, i));
                System.Threading.Thread.Sleep(500);
                i++;
            }

            await Task.WhenAll(tasks);

            Console.WriteLine("Price считаны в базу");
            Console.WriteLine("Считаны в базу данных тикеры");

            Console.WriteLine("Выберите ticker");
            string? tickrr;
            tickrr = Console.ReadLine();

            using (ApplicationContext db = new ApplicationContext())
            {
                var prid = db.Tickers.FirstOrDefault(t => t.ticker == tickrr);
                var aaaa = db.TodaysConditions.FirstOrDefault(t => t.Id == prid.Id);
                Console.WriteLine(aaaa.state);
            }
        }

        static async Task GetDataForTicker(string ticker, int i)
        {
            try
            {
                using HttpClient client = new();

                string url = $"https://api.marketdata.app/v1/stocks/candles/D/{ticker}/?from=2023-11-16&to=2024-11-15&token=UmF5LW9nUDJoam9NdURSMFM0WXJpMm5JcDI1RWlsRWJUb1drRi03QzVOcz0";

                HttpResponseMessage response = await client.GetAsync(url); string responseContent = await response.Content.ReadAsStringAsync();
                dynamic responceObject = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
                double averagePrice = 0;
                double previousaveragePrice = 0;
                if (responceObject != null && responceObject.t != null && responceObject.h != null && responceObject.l != null)
                {
                    List<long> timestamps = responceObject?.t?.ToObject<List<long>>() ?? new List<long>();
                    List<double> highs = responceObject?.h?.ToObject<List<double>>() ?? new List<double>();
                    List<double> lows = responceObject?.l?.ToObject<List<double>>() ?? new List<double>();
                    if (timestamps.Count >= 2)
                    {
                        long tm = timestamps[timestamps.Count - 1];
                        DateTime startingDate = new DateTime(1970, 1, 1);
                        DateTime newDateTime = startingDate.AddSeconds(tm);
                        string dat = newDateTime.ToString();

                        averagePrice += (highs[timestamps.Count - 1] + lows[timestamps.Count - 1]) / 2;
                        Console.WriteLine($"{ticker}:{dat}, {averagePrice}");

                        previousaveragePrice = averagePrice - (highs[timestamps.Count - 2] + lows[timestamps.Count - 2]) / 2;
                        using (ApplicationContext db = new ApplicationContext())
                        {
                            var newTicker = new Ticker { ticker = ticker };
                            db.Tickers.Add(newTicker);
                            db.SaveChanges();

                            var TodayCondition = new TodaysCondition
                            {
                                tickerId = i,
                                state = previousaveragePrice,
                                ticker = ticker

                            };
                            db.TodaysConditions.Add(TodayCondition);
                            db.SaveChanges();

                            var newPrice = new Price
                            {
                                tickerId = i,
                                price = averagePrice,
                                date = dat,
                                ticker = ticker
                            };

                            db.Prices.Add(newPrice);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        using (ApplicationContext db = new ApplicationContext())
                        {
                            var newTicker = new Ticker { ticker = ticker };

                            db.Tickers.Add(newTicker);
                            db.SaveChanges();
                            var TodayCondition = new TodaysCondition
                            {
                                tickerId = i,
                                state = 0,
                                ticker = ticker

                            };
                            db.TodaysConditions.Add(TodayCondition);
                            db.SaveChanges();

                            var newPrice = new Price
                            {
                                tickerId = i,
                                price = 0,
                                date = "0",
                                ticker = ticker
                            };
                            db.Prices.Add(newPrice);
                            db.SaveChanges();
                        }
                        Console.WriteLine($"{ticker}:0, {averagePrice}");
                    }
                }
                else
                {
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        var newTicker = new Ticker { ticker = ticker };
                        db.Tickers.Add(newTicker);
                        db.SaveChanges();


                        var TodayCondition = new TodaysCondition
                        {
                            tickerId = i,
                            state = 0,
                            ticker = ticker

                        };
                        db.TodaysConditions.Add(TodayCondition);
                        db.SaveChanges();

                        var newPrice = new Price
                        {
                            tickerId = i,
                            price = 0,
                            date = "0",
                            ticker = ticker
                        };

                        db.Prices.Add(newPrice);
                        db.SaveChanges();
                    }
                    Console.WriteLine($"{ticker}:0, {averagePrice}");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {ticker}: {ex.Message}");
            }
        }




    }

}
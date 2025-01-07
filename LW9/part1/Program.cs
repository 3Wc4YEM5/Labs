using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

class lab091
{
    static readonly Mutex mutex = new Mutex();
    static async Task Main()
    {


        List<string> tickers = new List<string>();

        using (StreamReader reader = new StreamReader("ticker.txt"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {//будет выполнять итерации до тех пор, пока ReadLine() не вернет null. Внутри цикла считываем каждую строку и добавляем ее в список tickers
                tickers.Add(line);
            }
        }


        List<Task> tasks = new List<Task>();
        foreach (string ticker in tickers)
        {

            tasks.Add(GetDataForTicker(ticker));
            System.Threading.Thread.Sleep(600);

        }
        await Task.WhenAll(tasks);


    }

    static async Task GetDataForTicker(string ticker)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {

                string url = $"https://api.marketdata.app/v1/stocks/candles/D/{ticker}/?from=2023-11-02&to=2024-11-01&token=RFU4aDItQlRLRjFuNDd5OVlMWGs4UGh6eXY5bldMWDRvS0xxOHctcmNLOD0";

                HttpResponseMessage response = await client.GetAsync(url);

                string responseContent = await response.Content.ReadAsStringAsync();
                dynamic responceObject = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
                double averagePrice = 0;
                if (responceObject != null && responceObject.t != null && responceObject.h != null && responceObject.l != null)
                {
                    List<long> timestamps = responceObject?.t?.ToObject<List<long>>() ?? new List<long>();
                    List<double> highs = responceObject?.h?.ToObject<List<double>>() ?? new List<double>();
                    List<double> lows = responceObject?.l?.ToObject<List<double>>() ?? new List<double>();

                    for (int i = 0; i < timestamps.Count; i++)
                    {
                        averagePrice += (highs[i] + lows[i]) / 2;
                    }
                    averagePrice /= timestamps.Count;
                    if (averagePrice == 0)
                    {
                        Console.WriteLine($"Ошибка при обработке {ticker}: Отсутствуют данные.");
                        await WriteToFile(ticker, averagePrice);
                    }
                    else
                    {
                        await WriteToFile(ticker, averagePrice);
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке {ticker}: {ex.Message}");
            }

        }
    }




    private static async Task WriteToFile(string ticker, double averagePrice)
    {
        mutex.WaitOne();
        try
        {
            using (StreamWriter writer = new StreamWriter("Average.txt", true))
            {
                await writer.WriteAsync($"{ticker}:{averagePrice} \n");
                Console.WriteLine($"{ticker}: {averagePrice}");
            }
        }
        finally
        {
            mutex.ReleaseMutex(); // Освобождаем мьютекс после завершения операции
        }
    }
}
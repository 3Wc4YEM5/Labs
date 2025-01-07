using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Globalization;


namespace network_application
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();
        private const string apiKey = "c0bca192b2e8c99548078dfb57a5150c";
        private List<Grad> cities;
        private ListBox cityListBox;
        private Label weatherLabel;
        private Button getWeatherButton;
        public Form1()
        {
            InitializeComponent();

            cities = LoadCitiesFromFile("city.txt");

            cityListBox = new ListBox();
            weatherLabel = new Label();
            getWeatherButton = new Button();

            // Настройка элементов
            cityListBox.Location = new Point(230, 150);
            cityListBox.Size = new Size(200, 200);
            cityListBox.DataSource = cities.Select(c => c.Name).ToList();

            weatherLabel.Location = new Point(440, 210);
            weatherLabel.Size = new Size(300, 500);

            getWeatherButton.Location = new Point(430, 150);
            getWeatherButton.Size = new Size(150, 30);
            getWeatherButton.Text = "Узнать погоду";
            getWeatherButton.Click += GetWeatherButton_Click;

            Controls.Add(cityListBox);
            Controls.Add(weatherLabel);
            Controls.Add(getWeatherButton);
        }

        private List<Grad> LoadCitiesFromFile(string filename)
        {
            List<Grad> cities = new List<Grad>();
            foreach (string line in File.ReadAllLines(filename))
            {
                string[] parts = line.Replace(", ", "\t").Replace(".", ",").Split('\t');
                if (parts.Length == 3)
                {
                    Grad city = new Grad(
                        parts[0].Trim(),
                        Convert.ToDouble(parts[1]),
                        Convert.ToDouble(parts[2])
                    );
                    cities.Add(city);
                }
            }
            return cities;
        }

        private async void GetWeatherButton_Click(object sender, EventArgs e)
        {
            string selectedCityName = cityListBox.SelectedItem?.ToString();
            if (selectedCityName != null)
            {
                Grad selectedCity = cities.FirstOrDefault(c => c.Name == selectedCityName);
                if (selectedCity != null)
                {

                    Weather? weather = await GetWeatherAsync(selectedCity.Lat, selectedCity.Lon);
                    if (weather != null)
                    {

                        weatherLabel.Text = weather.Value.ToString();
                    }
                    else
                    {
                        weatherLabel.Text = "Не удалось получить данные о погоде.";
                    }
                }
                else
                {
                    weatherLabel.Text = "Город не найден.";
                }
            }
            else
            {
                weatherLabel.Text = "Выберите город.";
            }
        }

        private static async Task<Weather?> GetWeatherAsync(double lat, double lon)
        {
            try
            {
                var response = await client.GetStringAsync($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&units=metric&appid={apiKey}");
                dynamic json = JsonConvert.DeserializeObject(response);

                if (json.sys.country != null)
                {
                    return new Weather
                    {
                        Country = json.sys.country,
                        Name = json.name,
                        Temp = json.main.temp,
                        Description = json.weather[0].description
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении данных о погоде: {ex.Message}");
                return null;
            }
        }
    }
    public struct Weather
    {
        public string Country { get; set; }
        public string Name { get; set; }
        public float Temp { get; set; }
        public string Description { get; set; }

        public Weather(string country, string name, float temp, string description)
        {
            Country = country;
            Name = name;
            Temp = temp;
            Description = description;
        }

        public override string ToString()
        {
            return $"Страна: {Country}\nНазвание города: {Name}\nТемпература воздуха: {Temp}°C\nОписание погоды: {Description}\n";
        }
    }

    public class Grad
    {
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }

        public Grad(string name, double lat, double lon)
        {
            Name = name;
            Lat = lat;
            Lon = lon;
        }
    }
}
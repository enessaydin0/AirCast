using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AirCast
{
    public partial class grafikEkrani : Form
    {
        public string Sehir { get; set; }

        private const string ApiKey = "70cd583b8c2ea46fcfe5ca55532224d8";
        private const string ApiUrl = "https://api.openweathermap.org/data/2.5/forecast?q={0}&units=metric&cnt=7&appid=" + ApiKey;
        public grafikEkrani()
        {
            InitializeComponent();
        }

        private async void grafikEkrani_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Sehir))
            {
                MessageBox.Show("Lütfen geçerli bir şehir girin!");
                this.Close();
            }

            // OpenWeather API'den verileri çek
            var temperatures = await GetWeatherData(Sehir);
            if (temperatures == null)
            {
                MessageBox.Show("Veri alınamadı. Lütfen tekrar deneyin.");
                return;
            }

            string[] days = { "Pzt", "Sal", "Çar", "Per", "Cum", "Cmt", "Paz" };

            // PictureBox üzerine grafiği çiz
            DrawWeatherGraph(temperatures, days);
        }

        private async Task<int[]> GetWeatherData(string city)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(string.Format(ApiUrl, city));
                    var weatherData = JsonConvert.DeserializeObject<WeatherResponse>(response);

                    // 7 günün sıcaklıklarını al
                    int[] temperatures = new int[7];
                    for (int i = 0; i < 7; i++)
                    {
                        temperatures[i] = (int)weatherData.list[i].main.temp;
                    }

                    return temperatures;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        private void DrawWeatherGraph(int[] temperatures, string[] days)
        {
            // PictureBox'ın boyutuna uygun bir Bitmap oluştur
            Bitmap bitmap = new Bitmap(WeatherPictureBox.Width, WeatherPictureBox.Height);

            // Grafik çizecek nesneleri oluştur
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // Arka planı beyaza boya
                g.Clear(Color.White);

                // Kalem ve fırça tanımları
                Pen axisPen = new Pen(Color.Black, 2);  // Eksen çizgisi
                Pen graphPen = new Pen(Color.Blue, 2);  // Grafik çizgisi
                Brush textBrush = new SolidBrush(Color.Black);  // Yazılar
                Brush pointBrush = new SolidBrush(Color.Red);   // Noktalar

                // Grafik çizim alanı
                int margin = 50; // Kenar boşluğu
                int graphWidth = bitmap.Width - margin * 2;
                int graphHeight = bitmap.Height - margin * 2;

                // Y ekseninin min ve max değerini hesapla
                int minTemp = temperatures.Min();
                int maxTemp = temperatures.Max();

                // Y ekseni için uygun aralık hesapla
                int rangeStart = minTemp - 5; // 5 derece daha düşükten başlasın
                int rangeEnd = maxTemp + 5;  // 5 derece daha yüksek olmalı

                // 5 derece aralıklarla Y eksenini çiz
                float yStep = (float)graphHeight / (rangeEnd - rangeStart);
                for (int i = rangeStart; i <= rangeEnd; i += 5)
                {
                    float y = bitmap.Height - margin - (i - rangeStart) * yStep;
                    g.DrawString($"{i}°C", new Font("Arial", 8), textBrush, margin - 35, y - 5);
                }

                // X eksenini çiz
                g.DrawLine(axisPen, margin, bitmap.Height - margin, bitmap.Width - margin, bitmap.Height - margin);

                // Y eksenini çiz
                g.DrawLine(axisPen, margin, bitmap.Height - margin, margin, margin);

                // X ekseni için her günün aralığını belirle
                float xStep = (float)graphWidth / (days.Length - 1); // Günler arası mesafe

                // Gün isimlerini yaz
                for (int i = 0; i < days.Length; i++)
                {
                    float x = margin + i * xStep;
                    g.DrawString(days[i], new Font("Arial", 10), textBrush, x - 10, bitmap.Height - margin + 10);
                }

                // Sıcaklık grafiğini çiz
                for (int i = 0; i < temperatures.Length - 1; i++)
                {
                    float x1 = margin + i * xStep;
                    float y1 = bitmap.Height - margin - (temperatures[i] - rangeStart) * yStep;
                    float x2 = margin + (i + 1) * xStep;
                    float y2 = bitmap.Height - margin - (temperatures[i + 1] - rangeStart) * yStep;

                    // Çizgiyi çiz
                    g.DrawLine(graphPen, x1, y1, x2, y2);

                    // Noktaları işaretle
                    g.FillEllipse(pointBrush, x1 - 3, y1 - 3, 6, 6);
                }
            }

            // PictureBox'a grafiği ata
            WeatherPictureBox.Image = bitmap;
        }
    }
    public class WeatherResponse
    {
        public List<WeatherList> list { get; set; }
    }

    public class WeatherList
    {
        public Main main { get; set; }
    }

    public class Main
    {
        public float temp { get; set; }
    }
}


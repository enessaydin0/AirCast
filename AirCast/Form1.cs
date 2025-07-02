using System;
using System.Globalization;
using System.Net.Http;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows.Forms;
using Azure.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;


namespace AirCast;
public partial class Form1 : Form
{
    private static readonly string apiKey = "API ANAHTARINIZI BURAYA YAPIÞTIRIN";
    private string baseUrl = "http://api.openweathermap.org/data/2.5/weather?";
    public string SecilenSehir { get; set; }

    private System.Windows.Forms.Timer timer;
    public Form1()
    {
        InitializeComponent();

        timer = new System.Windows.Forms.Timer();
        timer.Interval = 1000;
        timer.Tick += Timer_Tick;
        timer.Start();

        TarihGuncelle(null, null);
    }
    private void TarihGuncelle(object sender, EventArgs e)
    {

        button2.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }
    private async void Form1_Load(object sender, EventArgs e)
    {
        anagun_label.Location = new Point(120, 83);
        sehir_label.Location = new Point(100, 10);
        birincigun_label.Location = new Point(26, 15);

        textBox1.Text = "Þehir yazýnýz...";
        textBox1.ForeColor = Color.Gray;

        string[] allDays = new string[] { "Pazar", "Pazartesi", "Salý", "Çarþamba", "Perþembe", "Cuma", "Cumartesi" };

        CultureInfo culture = new CultureInfo("tr-TR");

        // Bugünün tarihini al
        DateTime today = DateTime.Now;

        string todayDay = today.ToString("dddd", culture);

        int todayIndex = Array.IndexOf(allDays, todayDay);

        if (todayIndex == -1)
        {
            MessageBox.Show("Bugünün günü belirlenemedi.");
            return;
        }

        // Gerçek hayattaki günlere göre yeniden yaziyor. örneðin: bu gün perþembeyse diðer günler cuma dan baþlayarak çarþambaya kadar sýralanýyor.
        anagun_label.Text = allDays[todayIndex]; // Bugün
        birincigun_label.Text = allDays[(todayIndex + 1) % 7];
        ikincigun_label.Text = allDays[(todayIndex + 2) % 7];
        ucuncugun_label.Text = allDays[(todayIndex + 3) % 7];
        dorduncugun_label.Text = allDays[(todayIndex + 4) % 7];
        besincigun_label.Text = allDays[(todayIndex + 5) % 7];
        altincigun_label.Text = allDays[(todayIndex + 6) % 7];
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        string currentTime = DateTime.Now.ToString("HH:mm:ss"); // Saat, dakika ve saniye formatýnda

        saat_button.Text = currentTime;
    }

    private void gunkartlariGroupbox_Enter(object sender, EventArgs e)
    {

    }
    private void AutoCompleteSetup()
    {

    }

    private void groupBox2_Enter(object sender, EventArgs e)
    {
    }

    private void cmbCitySuggestions_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {

    }

    private void textBox1_Click(object sender, EventArgs e)
    {

    }

    private void anaderece_button_Click(object sender, EventArgs e)
    {

    }


    private async void sehironayla_button_Click(object sender, EventArgs e)
    {
        string cityName = textBox1.Text;  // Kullanýcýnýn girdiði þehir adý
        SecilenSehir = cityName;

        if (string.IsNullOrEmpty(cityName))
        {
            MessageBox.Show("Lütfen bir þehir adý girin.");
            return;
        }

        try
        {
            // Mevcut hava durumu verilerini çekiyoruz
            var weatherData = await GetWeatherAsync(cityName);

            // Ana sýcaklýk bilgileri
            anaderece_button.Text = weatherData.NormalTemperature.ToString("0") + "°C";
            maxderece_button.Text = "Max: " + weatherData.MaxTemperature.ToString("0") + "°C";
            minderece_button.Text = "Min: " + weatherData.MinTemperature.ToString("0") + "°C";

            // Rüzgar ve nem bilgileri
            ruzgar_label.Text = " " + weatherData.WindSpeed.ToString("0") + " m/s";
            nem_label.Text = " " + weatherData.Humidity.ToString() + "%";

            // Þehir adýný düzgün formatta yazdýrýyoruz
            sehir_label.Text = cityName;

            // Hava durumu ikonunu güncelliyoruz
            UpdateWeatherIcon(weatherData.WeatherMain);

            // 6 günlük hava tahmini verilerini çekiyoruz
            List<Forecast> forecastList = await WeatherForecast.GetWeatherForecast(cityName);

            if (forecastList != null && forecastList.Count > 0)
            {
                // Verileri konsola yazdýr
                for (int i = 0; i < forecastList.Count; i++)
                {
                    Console.WriteLine($"Gün {i + 1}: {forecastList[i].Temp}°C");
                }

                // UI güncellemesi için Invoke kontrolü
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        birincigunderece_button.Text = forecastList[0].Temp.ToString("0") + "°C";
                        ikincigunderece_button.Text = forecastList[1].Temp.ToString("0") + "°C";
                        ucuncugunderece_button.Text = forecastList[2].Temp.ToString("0") + "°C";
                        dorduncugunderece_button.Text = forecastList[3].Temp.ToString("0") + "°C";
                        besincigunderece_button.Text = forecastList[4].Temp.ToString("0") + "°C";
                        altincigunderece_button.Text = forecastList[5].Temp.ToString("0") + "°C";
                    }));
                }
                else
                {
                    // Eðer InvokeRequired deðilse doðrudan atama yap
                    birincigunderece_button.Text = forecastList[0].Temp.ToString("0") + "°C";
                    ikincigunderece_button.Text = forecastList[1].Temp.ToString("0") + "°C";
                    ucuncugunderece_button.Text = forecastList[2].Temp.ToString("0") + "°C";
                    dorduncugunderece_button.Text = forecastList[3].Temp.ToString("0") + "°C";
                    besincigunderece_button.Text = forecastList[4].Temp.ToString("0") + "°C";
                    altincigunderece_button.Text = forecastList[5].Temp.ToString("0") + "°C";
                }
            }
            else
            {
                Console.WriteLine("Veri alýnamadý!");
            }

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            cityName = textInfo.ToTitleCase(cityName.ToLower());

            sehir_label.Text = cityName;

            UpdateWeatherIcon(weatherData.WeatherMain);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Hata: " + ex.Message);
        }
    }

    public class Forecast
    {
        public double Temp { get; set; }  // JSON'daki "temp" deðeri için
    }

    public class DailyForecast
    {
        public float Temperature { get; set; }
        public float MaxTemperature { get; set; }
        public float MinTemperature { get; set; }
        public float WindSpeed { get; set; }
        public int Humidity { get; set; }
        public string WeatherMain { get; set; }
    }
    private void ChangeGroupBoxBackgroundColor(Color color)
    {
        foreach (Control control in this.Controls)
        {
            if (control is GroupBox)
            {
                control.BackColor = color;
            }
        }
    }


    private void UpdateWeatherIcon(string weatherCondition)
    {
        Random rand = new Random();

        switch (weatherCondition.ToLower())
        {
            case "clear":
                // Güneþli
                anahavadurumuicon_pcbox.Image = Properties.Resources.sunny;
                birincigunhavadurumu_pcbox.Image = Properties.Resources.sunny;
                ikincigunhavadurumu_pcbox.Image = Properties.Resources.sunny;
                ucuncigunhavadurumu_pcbox.Image = Properties.Resources.sunny;
                dorduncigunhavadurumu_pcbox.Image = Properties.Resources.sunny;
                besincigunhavadurumu_pcbox.Image = Properties.Resources.sunny;
                altincigunhavadurumu_pcbox.Image = Properties.Resources.sunny;
               
                this.BackgroundImage = Properties.Resources.sunnyday;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                havadurumuicon_label.Text = "Güneþli";
                // Güneþli havaya uygun uyarýlar
                string[] sunnyMessages = { "Bugün dýþarýda güneþin tadýný çýkar!", "Havanýn keyfini çýkar, güneþ seni bekliyor.", "Sýcak bir gün seni bekliyor, dýþarýda bol bol hareket et!", "Güneþli hava, gün boyu enerjik kalman için ideal.", "Sýcak hava geldi, bol su içmeyi unutma."};
                otomatikuyari_label.Text = sunnyMessages[rand.Next(sunnyMessages.Length)];
                break;
            case "rain":
                // Yaðmurlu
                anahavadurumuicon_pcbox.Image = Properties.Resources.rain;
                birincigunhavadurumu_pcbox.Image = Properties.Resources.rain;
                ikincigunhavadurumu_pcbox.Image = Properties.Resources.rain;
                ucuncigunhavadurumu_pcbox.Image = Properties.Resources.rain;
                dorduncigunhavadurumu_pcbox.Image = Properties.Resources.rain;
                besincigunhavadurumu_pcbox.Image = Properties.Resources.rain;
                altincigunhavadurumu_pcbox.Image = Properties.Resources.rain;

                this.BackgroundImage = Properties.Resources.rainday2;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                havadurumuicon_label.Text = "Yaðmurlu";
                // Yaðmurlu havaya uygun uyarýlar
                string[] rainyMessages = { "Yaðmurlu havada dýþarýya çýkmadan önce þemsiye al.", "Yaðmur baþlamak üzere, þemsiye almayý unutma!", "Yaðmur yaðýyor, biraz rahatlamak için içeri girebilirsin.", "Yaðmurlu hava, yolda kayganlýk oluþturabilir, dikkatli sürüþ yap.", "Yaðmurla birlikte rüzgar da var, þemsiyeni saðlam tut!", "Yaðmur, yollarý kaygan hale getirebilir, dikkat et." };
                otomatikuyari_label.Text = rainyMessages[rand.Next(rainyMessages.Length)];
                break;
            case "clouds":
                // Bulutlu
                anahavadurumuicon_pcbox.Image = Properties.Resources.cloudy;
                birincigunhavadurumu_pcbox.Image = Properties.Resources.cloudy;
                ikincigunhavadurumu_pcbox.Image = Properties.Resources.cloudy;
                ucuncigunhavadurumu_pcbox.Image = Properties.Resources.cloudy;
                dorduncigunhavadurumu_pcbox.Image = Properties.Resources.cloudy;
                besincigunhavadurumu_pcbox.Image = Properties.Resources.cloudy;
                altincigunhavadurumu_pcbox.Image = Properties.Resources.cloudy;

                this.BackgroundImage = Properties.Resources.cloudyday;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                havadurumuicon_label.Text = "Bulutlu";
                // Bulutlu havaya uygun uyarýlar
                string[] cloudyMessages = { "Bulutlar var, ama yaðmur yok gibi görünüyor.", "Hava bulutlu, dýþarýsý biraz serin olabilir.", "Bulutlu bir gün, dýþarýda biraz hava alabilirsin.", "Bulutlu hava, sýcaklýk deðiþken olabilir, hazýrlýklý ol." };
                otomatikuyari_label.Text = cloudyMessages[rand.Next(cloudyMessages.Length)];
                break;
            case "snow":
                // Kar
                anahavadurumuicon_pcbox.Image = Properties.Resources.snow;
                birincigunhavadurumu_pcbox.Image = Properties.Resources.snow;
                ikincigunhavadurumu_pcbox.Image = Properties.Resources.snow;
                ucuncigunhavadurumu_pcbox.Image = Properties.Resources.snow;
                dorduncigunhavadurumu_pcbox.Image = Properties.Resources.snow;
                besincigunhavadurumu_pcbox.Image = Properties.Resources.snow;
                altincigunhavadurumu_pcbox.Image = Properties.Resources.snow;

                this.BackgroundImage = Properties.Resources.snowyday;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                havadurumuicon_label.Text = "Kar Yaðýþý";
                // Kar yaðýþý uyarýlarý
                string[] snowMessages = { "Kar yaðýyor, yollar kaygan olabilir, dikkat et!", "Kar yaðýþý baþladý, kýþ keyfini çýkarmak için dýþarýda vakit geçirebilirsin.", "Kar yaðýþý baþlamak üzere, dýþarý çýkarken kalýn giyin.", "Bugün kar yaðýþlarý var, dýþarýda uzun süre kalmaktan kaçýn." };
                otomatikuyari_label.Text = snowMessages[rand.Next(snowMessages.Length)];
                break;
            case "thunderstorm":
                // Fýrtýna
                anahavadurumuicon_pcbox.Image = Properties.Resources.thunderstorm;
                birincigunhavadurumu_pcbox.Image = Properties.Resources.thunderstorm;
                ikincigunhavadurumu_pcbox.Image = Properties.Resources.thunderstorm;
                ucuncigunhavadurumu_pcbox.Image = Properties.Resources.thunderstorm;
                dorduncigunhavadurumu_pcbox.Image = Properties.Resources.thunderstorm;
                besincigunhavadurumu_pcbox.Image = Properties.Resources.thunderstorm;
                altincigunhavadurumu_pcbox.Image = Properties.Resources.thunderstorm;

                this.BackgroundImage = Properties.Resources.thunderstorm2;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                havadurumuicon_label.Text = "Fýrtýna";
                // Fýrtýna uyarýlarý
                string[] thunderstormMessages = { "Fýrtýna geliyor, dýþarý çýkarken dikkatli ol.", "Fýrtýna baþlamak üzere, dýþarýda rüzgar ve yaðmur çok þiddetli olabilir.", "Fýrtýna uyarýsý var, þemsiye ve kalýn giysiler almayý unutma.", "Fýrtýna geliyor, elektrik kesintileri olabilir." };
                otomatikuyari_label.Text = thunderstormMessages[rand.Next(thunderstormMessages.Length)];
                break;
            case "drizzle":
                // Hafif Yaðmur
                anahavadurumuicon_pcbox.Image = Properties.Resources.drizzle;
                birincigunhavadurumu_pcbox.Image = Properties.Resources.drizzle;
                ikincigunhavadurumu_pcbox.Image = Properties.Resources.drizzle;
                ucuncigunhavadurumu_pcbox.Image = Properties.Resources.drizzle;
                dorduncigunhavadurumu_pcbox.Image = Properties.Resources.drizzle;
                besincigunhavadurumu_pcbox.Image = Properties.Resources.drizzle;
                altincigunhavadurumu_pcbox.Image = Properties.Resources.drizzle;

                this.BackgroundImage = Properties.Resources.rainday2;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                havadurumuicon_label.Text = "Hafif Yaðmur";
                // Hafif yaðmur uyarýlarý
                string[] drizzleMessages = { "Hafif yaðmur baþlýyor, þemsiye alman iyi olabilir.", "Yaðmur hafif, ama seni ýslatabilir.", "Dýþarýda biraz yaðmur var, ama rahatsýz etmeyecek kadar hafif.", "Yaðmur hafif, ancak ýslanabilirsin, þemsiye almanda fayda var." };
                otomatikuyari_label.Text = drizzleMessages[rand.Next(drizzleMessages.Length)];
                break;
            case "mist":
            case "haze":
            case "fog":
                // Sisli
                anahavadurumuicon_pcbox.Image = Properties.Resources.mist;
                birincigunhavadurumu_pcbox.Image = Properties.Resources.mist;
                ikincigunhavadurumu_pcbox.Image = Properties.Resources.mist;
                ucuncigunhavadurumu_pcbox.Image = Properties.Resources.mist;
                dorduncigunhavadurumu_pcbox.Image = Properties.Resources.mist;
                besincigunhavadurumu_pcbox.Image = Properties.Resources.mist;
                altincigunhavadurumu_pcbox.Image = Properties.Resources.mist;

                this.BackgroundImage = Properties.Resources.foggyday;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                havadurumuicon_label.Text = "Sisli";
                // Sisli hava uyarýlarý
                string[] fogMessages = { "Sisli hava, görüþ mesafesini zorlaþtýrabilir, dikkat et.", "Sisli hava nedeniyle araç kullanýrken hýzýný azalt ve dikkatli git.", "Sisli hava, özellikle trafiði zorlaþtýrabilir, dikkatli ol." };
                otomatikuyari_label.Text = fogMessages[rand.Next(fogMessages.Length)];
                break;
            case "partly cloudy":
                // Parçalý Bulutlu
                anahavadurumuicon_pcbox.Image = Properties.Resources.partly_cloudy;
                birincigunhavadurumu_pcbox.Image = Properties.Resources.partly_cloudy;
                ikincigunhavadurumu_pcbox.Image = Properties.Resources.partly_cloudy;
                ucuncigunhavadurumu_pcbox.Image = Properties.Resources.partly_cloudy;
                dorduncigunhavadurumu_pcbox.Image = Properties.Resources.partly_cloudy;
                besincigunhavadurumu_pcbox.Image = Properties.Resources.partly_cloudy;
                altincigunhavadurumu_pcbox.Image = Properties.Resources.partly_cloudy;

                this.BackgroundImage = Properties.Resources.cloudyday;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                havadurumuicon_label.Text = "Parçalý Bulutlu";
                // Parçalý bulutlu hava uyarýlarý
                string[] partlyCloudyMessages = { "Parçalý bulutlu bir gün, dýþarýda rahat bir hava var.", "Parçalý bulutlu hava, dýþarýda vakit geçirmek için harika bir fýrsat!", "Bulutlar yavaþça geçiyor, havada güzel bir esinti var, dýþarýda rahat bir gün!" };
                otomatikuyari_label.Text = partlyCloudyMessages[rand.Next(partlyCloudyMessages.Length)];
                break;
            default:
                // Bilinmeyen durum
                anahavadurumuicon_pcbox.Image = Properties.Resources.cloud;
                birincigunhavadurumu_pcbox.Image = Properties.Resources.cloudy;
                ikincigunhavadurumu_pcbox.Image = Properties.Resources.cloudy;
                ucuncigunhavadurumu_pcbox.Image = Properties.Resources.cloudy;
                dorduncigunhavadurumu_pcbox.Image = Properties.Resources.cloudy;
                besincigunhavadurumu_pcbox.Image = Properties.Resources.cloudy;
                altincigunhavadurumu_pcbox.Image = Properties.Resources.cloudy;

                this.BackgroundImage = Properties.Resources.cloudyday;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                havadurumuicon_label.Text = "Bilinmiyor";
                otomatikuyari_label.Text = "Bilinmeyen hava durumu.";
                break;
        }
    }

    private void FormBackgroundColor(Color color)
    {
        this.BackColor = color;
    }

    public class WeatherForecast
    {
        private static readonly string apiKey = "70cd583b8c2ea46fcfe5ca55532224d8"; // OpenWeather API anahtarýnýzý buraya ekleyin.
        private static readonly string baseUrl = "https://api.openweathermap.org/data/2.5/forecast";

        public static async Task<List<Forecast>> GetWeatherForecast(string cityName)
        {
            string url = $"{baseUrl}?q={cityName}&cnt=6&units=metric&appid={apiKey}"; // 6 gün hava durumu verilerini alýyoruz

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(url);
                    var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);

                    List<Forecast> forecastList = new List<Forecast>();

                    // API'den gelen her bir gün için sýcaklýklarý alýyoruz
                    for (int i = 0; i < 6; i++)
                    {
                        var temp = jsonResponse.list[i].main.temp;
                        forecastList.Add(new Forecast { Temp = temp });
                    }

                    return forecastList;
                }
                catch (HttpRequestException httpEx)
                {
                    MessageBox.Show("Veri alýnýrken bir hata oluþtu: " + httpEx.Message);
                    return null;
                }
            }
        }
    }

    public class HavaDurumuVerisi
    {
        public List<Forecast> List { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
    }

    public class Weather
    {
        public string main { get; set; }
        public string description { get; set; }
    }



    private Image GetWeatherIcon(string weatherCondition)
    {
        switch (weatherCondition.ToLower())
        {
            case "clear":
                return Properties.Resources.sunny;
            case "rain":
                return Properties.Resources.rain;
            case "clouds":
                return Properties.Resources.cloudy;
            case "snow":
                return Properties.Resources.snow;
            case "thunderstorm":
                return Properties.Resources.thunderstorm;
            case "drizzle":
                return Properties.Resources.drizzle;
            case "mist":
                return Properties.Resources.mist;
            case "haze":
                return Properties.Resources.mist;
            case "fog":
                return Properties.Resources.mist;
            case "partly cloudy":
                return Properties.Resources.partly_cloudy;
            default:
                return Properties.Resources.cloud; // Varsayýlan ikon
        }
    }


    private async Task<(double NormalTemperature, double MaxTemperature, double MinTemperature, double WindSpeed, int Humidity, string WeatherMain)> GetWeatherAsync(string city)
    {
        using (HttpClient client = new HttpClient())
        {
            // API'ye istek gönderiyoruz
            string url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={apiKey}";

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic weatherData = JsonConvert.DeserializeObject(jsonResponse);

                // Sýcaklýk, rüzgar hýzý, nem ve hava durumu bilgilerini alýyoruz
                double temperature = weatherData.main.temp;
                double maxTemp = weatherData.main.temp_max;
                double minTemp = weatherData.main.temp_min;
                double windSpeed = weatherData.wind.speed;  // Rüzgar hýzý (m/s)
                int humidity = weatherData.main.humidity;  // Nem oraný (%)
                string weatherMain = weatherData.weather[0].main;  // Hava durumu türü (Güneþli, Yaðmurlu vs.)


                // Verileri tuple olarak döndürüyoruz
                return (temperature, maxTemp, minTemp, windSpeed, humidity, weatherMain);
            }
            else
            {
                throw new Exception("Hava durumu bilgisi alýnamadý.");
            }
        }
    }

    public async Task<List<(DateTime Date, double Temperature, string WeatherMain)>> GetForecastAsync(string city)
    {
        using (HttpClient client = new HttpClient())
        {
            // OpenWeatherMap API'yi kullanýyoruz
            string url = $"http://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&appid={apiKey}";

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic forecastData = JsonConvert.DeserializeObject(jsonResponse);

                // 6 günlük tahmin verilerini saklayacaðýmýz liste
                List<(DateTime Date, double Temperature, string WeatherMain)> forecastList = new List<(DateTime, double, string)>();

                foreach (var item in forecastData.list)
                {
                    // Tarih, sýcaklýk ve hava durumu ana bilgisini alýyoruz
                    DateTime date = DateTime.Parse(item.dt_txt.ToString());
                    double temperature = item.main.temp;
                    string weatherMain = item.weather[0].main;

                    forecastList.Add((date, temperature, weatherMain));
                }

                return forecastList;
            }
            else
            {
                throw new Exception("Hava durumu tahmini alýnamadý.");
            }
        }
    }


    private void havadurumuicon_label_Click(object sender, EventArgs e)
    {

    }

    private void textBox1_Enter(object sender, EventArgs e)
    {
        if (textBox1.Text == "Þehir yazýnýz...")
        {
            textBox1.Text = "";
            textBox1.ForeColor = Color.Black;
        }
    }
    public class ForecastDay
    {
        public string Date { get; set; }
        public float Temperature { get; set; }
        public string WeatherCondition { get; set; }
    }
    public List<ForecastDay> Get6DayForecast(string city)
    {
        string apiKey = "70cd583b8c2ea46fcfe5ca55532224d8";
        string apiUrl = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&cnt=6&appid={apiKey}";
        List<ForecastDay> forecastList = new List<ForecastDay>();

        using (HttpClient client = new HttpClient())
        {
            var response = client.GetAsync(apiUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                dynamic weatherData = JsonConvert.DeserializeObject(data);

                foreach (var item in weatherData.list)
                {
                    ForecastDay day = new ForecastDay
                    {
                        Date = Convert.ToDateTime(item.dt_txt).ToString("dddd"),
                        Temperature = (float)item.main.temp,
                        WeatherCondition = (string)item.weather[0].main
                    };
                    forecastList.Add(day);
                }
            }
        }
        return forecastList;
    }


    private void textBox1_Leave(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(textBox1.Text))
        {
            textBox1.Text = "Þehir yazýnýz...";
            textBox1.ForeColor = Color.Gray;
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        var grafikForm = new grafikEkrani();
        grafikForm.Sehir = SecilenSehir; // Þehir bilgisini aktar
        grafikForm.ShowDialog();

    }

    private void gunes_ve_ay_Click(object sender, EventArgs e)
    {

    }

    private void button2_Click(object sender, EventArgs e)
    {

    }

    private void button3_Click(object sender, EventArgs e)
    {

    }

    private void ruzgarPusulasi_button_Click(object sender, EventArgs e)
    {
        RuzgarPusulasi ruzgarPusulaform = new RuzgarPusulasi();
        ruzgarPusulaform.Show();
    }
}

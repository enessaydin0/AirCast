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
    private static readonly string apiKey = "API ANAHTARINIZI BURAYA YAPI�TIRIN";
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

        textBox1.Text = "�ehir yaz�n�z...";
        textBox1.ForeColor = Color.Gray;

        string[] allDays = new string[] { "Pazar", "Pazartesi", "Sal�", "�ar�amba", "Per�embe", "Cuma", "Cumartesi" };

        CultureInfo culture = new CultureInfo("tr-TR");

        // Bug�n�n tarihini al
        DateTime today = DateTime.Now;

        string todayDay = today.ToString("dddd", culture);

        int todayIndex = Array.IndexOf(allDays, todayDay);

        if (todayIndex == -1)
        {
            MessageBox.Show("Bug�n�n g�n� belirlenemedi.");
            return;
        }

        // Ger�ek hayattaki g�nlere g�re yeniden yaziyor. �rne�in: bu g�n per�embeyse di�er g�nler cuma dan ba�layarak �ar�ambaya kadar s�ralan�yor.
        anagun_label.Text = allDays[todayIndex]; // Bug�n
        birincigun_label.Text = allDays[(todayIndex + 1) % 7];
        ikincigun_label.Text = allDays[(todayIndex + 2) % 7];
        ucuncugun_label.Text = allDays[(todayIndex + 3) % 7];
        dorduncugun_label.Text = allDays[(todayIndex + 4) % 7];
        besincigun_label.Text = allDays[(todayIndex + 5) % 7];
        altincigun_label.Text = allDays[(todayIndex + 6) % 7];
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        string currentTime = DateTime.Now.ToString("HH:mm:ss"); // Saat, dakika ve saniye format�nda

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
        string cityName = textBox1.Text;  // Kullan�c�n�n girdi�i �ehir ad�
        SecilenSehir = cityName;

        if (string.IsNullOrEmpty(cityName))
        {
            MessageBox.Show("L�tfen bir �ehir ad� girin.");
            return;
        }

        try
        {
            // Mevcut hava durumu verilerini �ekiyoruz
            var weatherData = await GetWeatherAsync(cityName);

            // Ana s�cakl�k bilgileri
            anaderece_button.Text = weatherData.NormalTemperature.ToString("0") + "�C";
            maxderece_button.Text = "Max: " + weatherData.MaxTemperature.ToString("0") + "�C";
            minderece_button.Text = "Min: " + weatherData.MinTemperature.ToString("0") + "�C";

            // R�zgar ve nem bilgileri
            ruzgar_label.Text = " " + weatherData.WindSpeed.ToString("0") + " m/s";
            nem_label.Text = " " + weatherData.Humidity.ToString() + "%";

            // �ehir ad�n� d�zg�n formatta yazd�r�yoruz
            sehir_label.Text = cityName;

            // Hava durumu ikonunu g�ncelliyoruz
            UpdateWeatherIcon(weatherData.WeatherMain);

            // 6 g�nl�k hava tahmini verilerini �ekiyoruz
            List<Forecast> forecastList = await WeatherForecast.GetWeatherForecast(cityName);

            if (forecastList != null && forecastList.Count > 0)
            {
                // Verileri konsola yazd�r
                for (int i = 0; i < forecastList.Count; i++)
                {
                    Console.WriteLine($"G�n {i + 1}: {forecastList[i].Temp}�C");
                }

                // UI g�ncellemesi i�in Invoke kontrol�
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        birincigunderece_button.Text = forecastList[0].Temp.ToString("0") + "�C";
                        ikincigunderece_button.Text = forecastList[1].Temp.ToString("0") + "�C";
                        ucuncugunderece_button.Text = forecastList[2].Temp.ToString("0") + "�C";
                        dorduncugunderece_button.Text = forecastList[3].Temp.ToString("0") + "�C";
                        besincigunderece_button.Text = forecastList[4].Temp.ToString("0") + "�C";
                        altincigunderece_button.Text = forecastList[5].Temp.ToString("0") + "�C";
                    }));
                }
                else
                {
                    // E�er InvokeRequired de�ilse do�rudan atama yap
                    birincigunderece_button.Text = forecastList[0].Temp.ToString("0") + "�C";
                    ikincigunderece_button.Text = forecastList[1].Temp.ToString("0") + "�C";
                    ucuncugunderece_button.Text = forecastList[2].Temp.ToString("0") + "�C";
                    dorduncugunderece_button.Text = forecastList[3].Temp.ToString("0") + "�C";
                    besincigunderece_button.Text = forecastList[4].Temp.ToString("0") + "�C";
                    altincigunderece_button.Text = forecastList[5].Temp.ToString("0") + "�C";
                }
            }
            else
            {
                Console.WriteLine("Veri al�namad�!");
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
        public double Temp { get; set; }  // JSON'daki "temp" de�eri i�in
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
                // G�ne�li
                anahavadurumuicon_pcbox.Image = Properties.Resources.sunny;
                birincigunhavadurumu_pcbox.Image = Properties.Resources.sunny;
                ikincigunhavadurumu_pcbox.Image = Properties.Resources.sunny;
                ucuncigunhavadurumu_pcbox.Image = Properties.Resources.sunny;
                dorduncigunhavadurumu_pcbox.Image = Properties.Resources.sunny;
                besincigunhavadurumu_pcbox.Image = Properties.Resources.sunny;
                altincigunhavadurumu_pcbox.Image = Properties.Resources.sunny;
               
                this.BackgroundImage = Properties.Resources.sunnyday;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                havadurumuicon_label.Text = "G�ne�li";
                // G�ne�li havaya uygun uyar�lar
                string[] sunnyMessages = { "Bug�n d��ar�da g�ne�in tad�n� ��kar!", "Havan�n keyfini ��kar, g�ne� seni bekliyor.", "S�cak bir g�n seni bekliyor, d��ar�da bol bol hareket et!", "G�ne�li hava, g�n boyu enerjik kalman i�in ideal.", "S�cak hava geldi, bol su i�meyi unutma."};
                otomatikuyari_label.Text = sunnyMessages[rand.Next(sunnyMessages.Length)];
                break;
            case "rain":
                // Ya�murlu
                anahavadurumuicon_pcbox.Image = Properties.Resources.rain;
                birincigunhavadurumu_pcbox.Image = Properties.Resources.rain;
                ikincigunhavadurumu_pcbox.Image = Properties.Resources.rain;
                ucuncigunhavadurumu_pcbox.Image = Properties.Resources.rain;
                dorduncigunhavadurumu_pcbox.Image = Properties.Resources.rain;
                besincigunhavadurumu_pcbox.Image = Properties.Resources.rain;
                altincigunhavadurumu_pcbox.Image = Properties.Resources.rain;

                this.BackgroundImage = Properties.Resources.rainday2;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                havadurumuicon_label.Text = "Ya�murlu";
                // Ya�murlu havaya uygun uyar�lar
                string[] rainyMessages = { "Ya�murlu havada d��ar�ya ��kmadan �nce �emsiye al.", "Ya�mur ba�lamak �zere, �emsiye almay� unutma!", "Ya�mur ya��yor, biraz rahatlamak i�in i�eri girebilirsin.", "Ya�murlu hava, yolda kayganl�k olu�turabilir, dikkatli s�r�� yap.", "Ya�murla birlikte r�zgar da var, �emsiyeni sa�lam tut!", "Ya�mur, yollar� kaygan hale getirebilir, dikkat et." };
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
                // Bulutlu havaya uygun uyar�lar
                string[] cloudyMessages = { "Bulutlar var, ama ya�mur yok gibi g�r�n�yor.", "Hava bulutlu, d��ar�s� biraz serin olabilir.", "Bulutlu bir g�n, d��ar�da biraz hava alabilirsin.", "Bulutlu hava, s�cakl�k de�i�ken olabilir, haz�rl�kl� ol." };
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

                havadurumuicon_label.Text = "Kar Ya����";
                // Kar ya���� uyar�lar�
                string[] snowMessages = { "Kar ya��yor, yollar kaygan olabilir, dikkat et!", "Kar ya���� ba�lad�, k�� keyfini ��karmak i�in d��ar�da vakit ge�irebilirsin.", "Kar ya���� ba�lamak �zere, d��ar� ��karken kal�n giyin.", "Bug�n kar ya���lar� var, d��ar�da uzun s�re kalmaktan ka��n." };
                otomatikuyari_label.Text = snowMessages[rand.Next(snowMessages.Length)];
                break;
            case "thunderstorm":
                // F�rt�na
                anahavadurumuicon_pcbox.Image = Properties.Resources.thunderstorm;
                birincigunhavadurumu_pcbox.Image = Properties.Resources.thunderstorm;
                ikincigunhavadurumu_pcbox.Image = Properties.Resources.thunderstorm;
                ucuncigunhavadurumu_pcbox.Image = Properties.Resources.thunderstorm;
                dorduncigunhavadurumu_pcbox.Image = Properties.Resources.thunderstorm;
                besincigunhavadurumu_pcbox.Image = Properties.Resources.thunderstorm;
                altincigunhavadurumu_pcbox.Image = Properties.Resources.thunderstorm;

                this.BackgroundImage = Properties.Resources.thunderstorm2;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                havadurumuicon_label.Text = "F�rt�na";
                // F�rt�na uyar�lar�
                string[] thunderstormMessages = { "F�rt�na geliyor, d��ar� ��karken dikkatli ol.", "F�rt�na ba�lamak �zere, d��ar�da r�zgar ve ya�mur �ok �iddetli olabilir.", "F�rt�na uyar�s� var, �emsiye ve kal�n giysiler almay� unutma.", "F�rt�na geliyor, elektrik kesintileri olabilir." };
                otomatikuyari_label.Text = thunderstormMessages[rand.Next(thunderstormMessages.Length)];
                break;
            case "drizzle":
                // Hafif Ya�mur
                anahavadurumuicon_pcbox.Image = Properties.Resources.drizzle;
                birincigunhavadurumu_pcbox.Image = Properties.Resources.drizzle;
                ikincigunhavadurumu_pcbox.Image = Properties.Resources.drizzle;
                ucuncigunhavadurumu_pcbox.Image = Properties.Resources.drizzle;
                dorduncigunhavadurumu_pcbox.Image = Properties.Resources.drizzle;
                besincigunhavadurumu_pcbox.Image = Properties.Resources.drizzle;
                altincigunhavadurumu_pcbox.Image = Properties.Resources.drizzle;

                this.BackgroundImage = Properties.Resources.rainday2;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                havadurumuicon_label.Text = "Hafif Ya�mur";
                // Hafif ya�mur uyar�lar�
                string[] drizzleMessages = { "Hafif ya�mur ba�l�yor, �emsiye alman iyi olabilir.", "Ya�mur hafif, ama seni �slatabilir.", "D��ar�da biraz ya�mur var, ama rahats�z etmeyecek kadar hafif.", "Ya�mur hafif, ancak �slanabilirsin, �emsiye almanda fayda var." };
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
                // Sisli hava uyar�lar�
                string[] fogMessages = { "Sisli hava, g�r�� mesafesini zorla�t�rabilir, dikkat et.", "Sisli hava nedeniyle ara� kullan�rken h�z�n� azalt ve dikkatli git.", "Sisli hava, �zellikle trafi�i zorla�t�rabilir, dikkatli ol." };
                otomatikuyari_label.Text = fogMessages[rand.Next(fogMessages.Length)];
                break;
            case "partly cloudy":
                // Par�al� Bulutlu
                anahavadurumuicon_pcbox.Image = Properties.Resources.partly_cloudy;
                birincigunhavadurumu_pcbox.Image = Properties.Resources.partly_cloudy;
                ikincigunhavadurumu_pcbox.Image = Properties.Resources.partly_cloudy;
                ucuncigunhavadurumu_pcbox.Image = Properties.Resources.partly_cloudy;
                dorduncigunhavadurumu_pcbox.Image = Properties.Resources.partly_cloudy;
                besincigunhavadurumu_pcbox.Image = Properties.Resources.partly_cloudy;
                altincigunhavadurumu_pcbox.Image = Properties.Resources.partly_cloudy;

                this.BackgroundImage = Properties.Resources.cloudyday;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                havadurumuicon_label.Text = "Par�al� Bulutlu";
                // Par�al� bulutlu hava uyar�lar�
                string[] partlyCloudyMessages = { "Par�al� bulutlu bir g�n, d��ar�da rahat bir hava var.", "Par�al� bulutlu hava, d��ar�da vakit ge�irmek i�in harika bir f�rsat!", "Bulutlar yava��a ge�iyor, havada g�zel bir esinti var, d��ar�da rahat bir g�n!" };
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
        private static readonly string apiKey = "70cd583b8c2ea46fcfe5ca55532224d8"; // OpenWeather API anahtar�n�z� buraya ekleyin.
        private static readonly string baseUrl = "https://api.openweathermap.org/data/2.5/forecast";

        public static async Task<List<Forecast>> GetWeatherForecast(string cityName)
        {
            string url = $"{baseUrl}?q={cityName}&cnt=6&units=metric&appid={apiKey}"; // 6 g�n hava durumu verilerini al�yoruz

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(url);
                    var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response);

                    List<Forecast> forecastList = new List<Forecast>();

                    // API'den gelen her bir g�n i�in s�cakl�klar� al�yoruz
                    for (int i = 0; i < 6; i++)
                    {
                        var temp = jsonResponse.list[i].main.temp;
                        forecastList.Add(new Forecast { Temp = temp });
                    }

                    return forecastList;
                }
                catch (HttpRequestException httpEx)
                {
                    MessageBox.Show("Veri al�n�rken bir hata olu�tu: " + httpEx.Message);
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
                return Properties.Resources.cloud; // Varsay�lan ikon
        }
    }


    private async Task<(double NormalTemperature, double MaxTemperature, double MinTemperature, double WindSpeed, int Humidity, string WeatherMain)> GetWeatherAsync(string city)
    {
        using (HttpClient client = new HttpClient())
        {
            // API'ye istek g�nderiyoruz
            string url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={apiKey}";

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic weatherData = JsonConvert.DeserializeObject(jsonResponse);

                // S�cakl�k, r�zgar h�z�, nem ve hava durumu bilgilerini al�yoruz
                double temperature = weatherData.main.temp;
                double maxTemp = weatherData.main.temp_max;
                double minTemp = weatherData.main.temp_min;
                double windSpeed = weatherData.wind.speed;  // R�zgar h�z� (m/s)
                int humidity = weatherData.main.humidity;  // Nem oran� (%)
                string weatherMain = weatherData.weather[0].main;  // Hava durumu t�r� (G�ne�li, Ya�murlu vs.)


                // Verileri tuple olarak d�nd�r�yoruz
                return (temperature, maxTemp, minTemp, windSpeed, humidity, weatherMain);
            }
            else
            {
                throw new Exception("Hava durumu bilgisi al�namad�.");
            }
        }
    }

    public async Task<List<(DateTime Date, double Temperature, string WeatherMain)>> GetForecastAsync(string city)
    {
        using (HttpClient client = new HttpClient())
        {
            // OpenWeatherMap API'yi kullan�yoruz
            string url = $"http://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&appid={apiKey}";

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic forecastData = JsonConvert.DeserializeObject(jsonResponse);

                // 6 g�nl�k tahmin verilerini saklayaca��m�z liste
                List<(DateTime Date, double Temperature, string WeatherMain)> forecastList = new List<(DateTime, double, string)>();

                foreach (var item in forecastData.list)
                {
                    // Tarih, s�cakl�k ve hava durumu ana bilgisini al�yoruz
                    DateTime date = DateTime.Parse(item.dt_txt.ToString());
                    double temperature = item.main.temp;
                    string weatherMain = item.weather[0].main;

                    forecastList.Add((date, temperature, weatherMain));
                }

                return forecastList;
            }
            else
            {
                throw new Exception("Hava durumu tahmini al�namad�.");
            }
        }
    }


    private void havadurumuicon_label_Click(object sender, EventArgs e)
    {

    }

    private void textBox1_Enter(object sender, EventArgs e)
    {
        if (textBox1.Text == "�ehir yaz�n�z...")
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
            textBox1.Text = "�ehir yaz�n�z...";
            textBox1.ForeColor = Color.Gray;
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        var grafikForm = new grafikEkrani();
        grafikForm.Sehir = SecilenSehir; // �ehir bilgisini aktar
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

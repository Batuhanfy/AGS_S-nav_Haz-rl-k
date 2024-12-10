using System.IO;
using System.Text.Json;
using System;
using System.Collections.Generic;

namespace TodoListAGS
{

    public partial class Form1 : Form
    {

        private string jsonDosyaYolu = "timing.json";
        private List<string> tumDersler = new List<string>
{
    "Say�sal Yetenek",
    "S�zel Yetenek",
    "Tarih",
    "Co�rafya",
    "E�itimin Temelleri ve T�rk Milli E�itim Sistemi",
    "Fizik",
    "Kimya",
    "Biyoloji",
    "Yer Bilimi",
    "Astronomi",
    "�evre Bilimi"
};
        private void UpdateLessonDurations(DersVerileri veriler)
        {
            foreach (var ders in veriler.Dersler)
            {
                string dersAdi = ders.Key;
                string toplamSure = ders.Value.ToplamSure;

                switch (dersAdi)
                {
                    case "Say�sal Yetenek":
                        label16.Text = toplamSure;
                        break;

                    case "S�zel Yetenek":
                        label17.Text = toplamSure;
                        break;

                    case "Tarih":
                        label18.Text = toplamSure;
                        break;

                    case "Co�rafya":
                        label19.Text = toplamSure;
                        break;

                    case "E�itimin Temelleri ve T�rk Milli E�itim Sistemi":
                        label20.Text = toplamSure;
                        break;

                    case "Fizik":
                        label21.Text = toplamSure;
                        break;

                    case "Kimya":
                        label22.Text = toplamSure;
                        break;

                    case "Biyoloji":
                        label23.Text = toplamSure;
                        break;

                    case "Yer Bilimi":
                        label24.Text = toplamSure;
                        break;

                    case "Astronomi":
                        label25.Text = toplamSure;
                        break;

                    case "�evre Bilimi":
                        label26.Text = toplamSure;
                        break;
                }
            }
        }

        private DersVerileri DersVerileriOku()
        {
            if (File.Exists(jsonDosyaYolu))
            {

                string json = File.ReadAllText(jsonDosyaYolu);
                return JsonSerializer.Deserialize<DersVerileri>(json);
            }
            else
            {

                DersVerileri yeniVeriler = new DersVerileri();
                DersVerileriYaz(yeniVeriler);
                return yeniVeriler;
            }
        }

        private void EnCokVeEnAzCalisilanDersleriGoster()
        {

            DersVerileri veriler = DersVerileriOku();

            if (veriler == null || veriler.Dersler == null || veriler.Dersler.Count == 0)
            {
                label34.Text = " - ";
                label35.Text = " - ";
                return;
            }

            var dersSureleri = veriler.Dersler.Select(ders => new
            {
                DersAdi = ders.Key,
                Sure = TimeSpan.Parse(ders.Value.ToplamSure)
            }).ToList();

            var enCokCalisilan = dersSureleri.OrderByDescending(d => d.Sure).FirstOrDefault();
            var enAzCalisilan = dersSureleri.OrderBy(d => d.Sure).FirstOrDefault();

            if (enCokCalisilan != null)
                label34.Text = $"{enCokCalisilan.DersAdi} ({enCokCalisilan.Sure})";

            if (enAzCalisilan != null)
                label35.Text = $"{enAzCalisilan.DersAdi} ({enAzCalisilan.Sure})";
        }

        private void DersVerileriYaz(DersVerileri veriler)
        {
            string json = JsonSerializer.Serialize(veriler, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonDosyaYolu, json);
        }

        private Dictionary<string, TimeSpan> dersSureleri = new Dictionary<string, TimeSpan>();

        public Form1()
        {

            InitializeComponent();

            timer2.Interval = 1000;
            label27.Text = "00:00:00";

            timer1.Interval = 1000;
            timer1.Start();

        }
        string oldTitle;
        private DateTime targetDate = new DateTime(2025, 7, 13);

        private void Form1_Load(object sender, EventArgs e)
        {
            oldTitle = this.Text;

            DersVerileri veriler = DersVerileriOku();

            EnCokVeEnAzCalisilanDersleriGoster();
            Son1GunCalisilmayanDersleriListele();
            ToplamCalismaSuresiniGoster();

            if (veriler == null || veriler.Dersler == null)
                return;

            UpdateLessonDurations(veriler);

        }

        private void Son1GunCalisilmayanDersleriListele()
        {

            DersVerileri veriler = DersVerileriOku();

            if (veriler == null)
                veriler = new DersVerileri();

            string dundenItibaren = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            listBox1.Items.Clear();

            foreach (string dersAdi in tumDersler)
            {
                bool calisildiMi = false;

                if (veriler.Dersler.ContainsKey(dersAdi))
                {

                    foreach (var kayit in veriler.Dersler[dersAdi].GunlukKayitlar)
                    {
                        if (DateTime.Parse(kayit.Key) >= DateTime.Now.AddDays(-1) && TimeSpan.Parse(kayit.Value) > TimeSpan.Zero)
                        {
                            calisildiMi = true;
                            break;
                        }
                    }
                }

                if (!calisildiMi)
                {
                    listBox1.Items.Add(dersAdi);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            TimeSpan remainingTime = targetDate - currentDate;

            int monthsRemaining = (targetDate.Year - currentDate.Year) * 12 + targetDate.Month - currentDate.Month;

            DateTime nextMonth = currentDate.AddMonths(monthsRemaining);
            int daysRemaining = (targetDate - nextMonth).Days;

            label15.Text = $"Kalan S�re: {monthsRemaining} ay, {daysRemaining} g�n";
        }

        private void label27_Click(object sender, EventArgs e)
        {

        }
        private int Kronometreseconds = 0;
        private bool isKronometreRunning = false;
        private string[] motivasyonSozleri = new string[]
   {
    "Ba�ar�, cesaretin �d�l�d�r. � Sophocles",
    "Ba�ar�, yap�lmayan i�lerin de�il, yap�lan i�lerin �r�n�d�r. � Thomas Edison",
    "Ba�ar�s�zl�k, ba�ar� yolundaki en b�y�k ��retmendir. � Bill Gates",
    "Gelecek, hayal edenlerin ellerindedir. � Eleanor Roosevelt",
    "Her hayal, b�y�k bir ba�ar�n�n tohumudur. � Mark Twain",
    "Ba�ar�, korkular�n de�il, cesaretin meyvesidir. � Aristoteles",
    "Ba�ar�, risk almay� bilmekten ge�er. � Elon Musk",
    "Ba�ar�ya inan�yorsan�z, onu elde edebilirsiniz. � Oprah Winfrey",
    "D��enler, yeniden aya�a kalkmay� bilenlerdir. � Nelson Mandela",
    "��iniz hayat�n�z�n b�y�k bir k�sm�n� kaplayacak ve tatmin olman�n tek yolu, harika i�ler yapaca��n�za inand���n�z i�i yapmakt�r. � Steve Jobs",
    "G��, fiziksel kapasiteden de�il, iradeli bir kararl�l�ktan gelir. � Mahatma Gandhi",
    "F�rsatlar durup dururken kar��n�za ��kmaz, onlar� siz yarat�rs�n�z. � Chris Grosser",
    "�ansa �ok inan�r�m ve ne kadar �ok �al��t�ysam ona o kadar �ok sahip oldum. � Thomas Jefferson",
    "Ba�ar�ya ��kan asans�r bozuk. Merdivenleri t�rmanmaya ba�lay�n. � Joe Girard",
    "Hayat her ne kadar zor g�r�nse de, yapabilece�imiz ve ba�arabilece�imiz bir �ey mutlaka vard�r. � Stephen Hawking",
    "Yapabilece�in en iyi �eyi yap, sonra yapabildi�in kadar�n� yap, sonunda yapabilece�in �eyi yap. � Theodore Roosevelt",
    "Yava� yava� ilerlemek, hi� ilerlememekten iyidir. � Konf��y�s",
    "Ba�ar� genellikle aramaktan �ok me�gul olanlara gelir. � Henry David Thoreau",
    "Ba�ar� harekete ba�l�d�r. Ba�ar�l� insanlar harekete ge�er. Hata yaparlar ama b�rakmazlar. � Conrad Hilton",
    "Ba�ar�ya giden yol her zaman in�aat halindedir. � Lily Tomlin",
    "Her �eyin m�kemmel olmas�n� beklemeyin. �imdi ba�lay�n. � Mark Victor Hansen",
    "Ba�ar�n�z� belirleyecek �ey, tavr�n�zd�r. � Zig Ziglar",
    "Ba�ar�l� sava���, lazer gibi odaklanm�� s�radan bir insand�r. � Bruce Lee",
    "Bizim i�in ac� gelen zorluklar, �o�unlukla gizli nimetlerdir. � Oscar Wilde",
    "Nerede oldu�un �nemli de�il, elindekilerle ba�la ve yapabilece�ini yap. � Arthur Ashe",
    "B�y�k i�ler yapmak istiyorsan�z, sevdi�iniz i�i yap�n. � Steve Jobs",
    "Ba�ar�, en dibe vurdu�unuzda ne kadar y�kse�e s��rayaca��n�zd�r. � George S. Patton",
    "Ba�laman�n yolu, konu�may� b�rak�p yapmaya ba�lamakt�r. � Walt Disney",
    "B�t�n merdivenleri g�rmek zorunda de�ilsiniz. Yapman�z gereken ilk ad�m� atmakt�r. � Martin Luther King Jr.",
    "Bir �eye ba�lay�p ba�ar�s�z olmaktan daha k�t� tek �ey, hi�bir �eye ba�lamamakt�r. � Seth Godin",
    "Ba�ar�ya ula�man�n s�rr�, ad�m atmaktan korkmamakt�r. � Anonim",
    "Ba�ar� tesad�f de�ildir, s�k� �al��man�n ve �zverinin sonucudur. � Pele",
    "Zorluklara meydan okumak, ba�ar�n�n anahtar�d�r. � Anonim",
    "Hayatta korkulacak �ey yoktur, anla��lacak �eyler vard�r. � Marie Curie",
    "Her �ampiyon, bir zamanlar pes etmeyi reddeden bir yar��mac�yd�. � Sylvester Stallone",
    "Saati izleme; onun yapt���n� yap, devam et. � Sam Levenson",
    "Motivasyon ba�lamak i�in gereklidir, al��kanl�k ise devam ettirir. � Jim Rohn",
    "U�am�yorsan�z ko�un, ko�am�yorsan�z y�r�y�n, y�r�yemiyorsan�z s�r�n�n; ama her durumda ilerleyin. � Martin Luther King Jr.",
    "�lerlemenin anahtar� s�rekli harekettir. � Tony Robbins",
    "En iyi ��k�� yolu, her zaman i�inden ge�mektir. � Robert Frost",
    "Elinizde olanla ba�lay�n, yapabilece�iniz �eyi yap�n. � Theodore Roosevelt",
    "Soru, kimin izin verece�i de�il; kimin durduraca��d�r. � Ayn Rand",
    "�stedi�iniz her �ey, korkunun di�er taraf�nda. � George Addair",
    "En iyi motivasyon, kendi kendini motive etmektir. � Jim Rohn",
    "Ba�ar�n�n tek s�n�r�, zihninizdir. Bedeniniz m�cadeleye haz�rd�r. � Mo Brossette",
    "Ba�lamak i�in m�kemmel olman�za gerek yok, ama m�kemmel olmak i�in ba�lamal�s�n�z. � Zig Ziglar",
    "Yapamayaca��n�z� d���nd���n�z her �eyi yap�n. Ba�ar�s�z olsan�z bile, s�n�rlar�n�z� zorlam�� olursunuz. � Eleanor Roosevelt",
    "�mkans�z olan� yapmak, ba�lamakla ba�lar. � Nelson Mandela",
    "Yapmak zorunda olmad���n�z �eyleri yapt���n�zda, ba�kalar�n�n yapamad��� �eyleri yapars�n�z. � Alexander Graham Bell",
    "Yar�n� in�a etmenin en iyi yolu, bug�nden ba�lamakt�r. � Abraham Lincoln",
    "Zafer, zafer benimdir diyebilenindir. Ba�ar� ise �ba�araca��m� diye ba�layarak sonunda �ba�ard�m� diyenindir. � Mustafa Kemal Atat�rk",
    "Kazanma iste�i ve ba�ar�ya ula�ma arzusu birle�irse ki�isel m�kemmelli�in kap�s�n� a�ar. � Konf��y�s",
    "Hi�bir �eyden vazge�me, ��nk� sadece kaybedenler vazge�er. � Abraham Lincoln",
    "Sessizce s�k� �al���n, b�rak�n ba�ar� sesiniz olsun. � Frank Ocean",
    "Ba�ar� son de�ildir, ba�ar�s�zl�k ise �l�mc�l de�ildir: �nemli olan ilerlemeye cesaret etmektir. � Winston S. Churchill"
   };

        private void timer2_Tick(object sender, EventArgs e)
        {
            Kronometreseconds++;

            int hours = Kronometreseconds / 3600;
            int minutes = (Kronometreseconds % 3600) / 60;
            int secs = Kronometreseconds % 60;

            label27.Text = $"{hours:00}:{minutes:00}:{secs:00}";

            if (checkBox3.Checked)
            {

                if (Kronometreseconds == 1800)
                {
                    ShowReminder("Dilerseniz k���k bir mola verebilirsiniz.", "MOLA HATIRLATMASI");
                }
                else if (Kronometreseconds == 3600)
                {
                    ShowReminder("TOPLAM 60 DAK�KAYI GER�DE BIRAKTINIZ", "MOLA HATIRLATMASI");
                }
                else if (Kronometreseconds == 5400)
                {
                    ShowReminder("1 SAAT 30 DAK�KA S�REY� GER�DE BIRAKTINIZ", "MOLA HATIRLATMASI");
                }
                else if (Kronometreseconds == 7200)
                {
                    ShowReminder("2 SAAT� GER�DE BIRAKTINIZ", "MOLA HATIRLATMASI");
                }

            }

            if (checkBox2.Checked)
            {

                Random random = new Random();
                int index = random.Next(motivasyonSozleri.Length);
                string rastgeleSoz = motivasyonSozleri[index];

                if (Kronometreseconds == 600)
                {
                    ShowReminder(rastgeleSoz, rastgeleSoz);
                }
                else if (Kronometreseconds == 2400)
                {
                    ShowReminder(rastgeleSoz, rastgeleSoz);
                }
                else if (Kronometreseconds == 4800)
                {
                    ShowReminder(rastgeleSoz, rastgeleSoz);
                }
                else if (Kronometreseconds == 7200)
                {
                    ShowReminder(rastgeleSoz, rastgeleSoz);
                }

            }

        }
        private void ShowReminder(string message, string title)
        {

            notifyIcon1.BalloonTipTitle = title;
            notifyIcon1.BalloonTipText = message;
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.ShowBalloonTip(1500);
        }
        private void ToplamCalismaSuresiniGoster()
        {

            DersVerileri veriler = DersVerileriOku();

            if (veriler == null || veriler.Dersler == null || veriler.Dersler.Count == 0)
            {
                label36.Text = "Toplam S�re: 0 g�n 0 saat 0 dakika 0 saniye";
                return;
            }

            TimeSpan toplamSure = TimeSpan.Zero;
            foreach (var ders in veriler.Dersler)
            {
                toplamSure += TimeSpan.Parse(ders.Value.ToplamSure);
            }

            int gun = toplamSure.Days;
            int saat = toplamSure.Hours;
            int dakika = toplamSure.Minutes;
            int saniye = toplamSure.Seconds;

            label36.Text = $" {gun} g�n {saat} saat {dakika} dakika {saniye} saniye �al��t�n�z.";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (isKronometreRunning)
            {
                string secilenDers = comboBox1.SelectedItem.ToString();
                TimeSpan elapsed = TimeSpan.FromSeconds(Kronometreseconds);

                SureEkle(secilenDers, elapsed);

                Kronometreseconds = 0;
                label27.Text = "00:00:00";

                DersVerileri veriler = DersVerileriOku();

                if (veriler == null || veriler.Dersler == null)
                    return;

                UpdateLessonDurations(veriler);

                ToplamCalismaSuresiniGoster();
                EnCokVeEnAzCalisilanDersleriGoster();
                Son1GunCalisilmayanDersleriListele();

                timer2.Stop();
                button1.Text = "Kronometre Ba�lat";
                this.Text = oldTitle;
            }
            else
            {

                if (comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("L�tfen bir ders se�iniz.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string secilenDers = comboBox1.SelectedItem.ToString();

                if (!dersSureleri.ContainsKey(secilenDers))
                {
                    dersSureleri[secilenDers] = TimeSpan.Zero;
                }

                timer2.Start();
                button1.Text = "�al��may� Bitir";

                this.WindowState = FormWindowState.Minimized;
                this.Text = "DERS �ALI�ILIYOR";

                notifyIcon1.BalloonTipTitle = "�Y� �ALI�MALAR  ";
                notifyIcon1.BalloonTipText = "KRONOMETRE �UANDA DERS S�RES�N� KAYDETMEKTED�R.";
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon1.ShowBalloonTip(4000);
            }
            isKronometreRunning = !isKronometreRunning;
        }

        private void SureEkle(string dersAdi, TimeSpan sure)
        {

            DersVerileri veriler = DersVerileriOku();

            TimeSpan toplamSure = TimeSpan.Parse(veriler.ToplamSure);
            veriler.ToplamSure = (toplamSure + sure).ToString();

            if (!veriler.Dersler.ContainsKey(dersAdi))
            {
                veriler.Dersler[dersAdi] = new DersBilgisi();
            }

            DersBilgisi ders = veriler.Dersler[dersAdi];
            TimeSpan dersToplamSure = TimeSpan.Parse(ders.ToplamSure);
            ders.ToplamSure = (dersToplamSure + sure).ToString();

            string bugun = DateTime.Now.ToString("yyyy-MM-dd");
            if (!ders.GunlukKayitlar.ContainsKey(bugun))
            {
                ders.GunlukKayitlar[bugun] = "00:00:00";
            }

            TimeSpan gunlukSure = TimeSpan.Parse(ders.GunlukKayitlar[bugun]);
            ders.GunlukKayitlar[bugun] = (gunlukSure + sure).ToString();

            DersVerileriYaz(veriler);
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string mesaj = "Ders �al��ma S�releri:\n";
            foreach (var item in dersSureleri)
            {
                mesaj += $"{item.Key}: {item.Value}\n";
            }
            MessageBox.Show(mesaj);
        }
    }
}
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
    "Sayýsal Yetenek",
    "Sözel Yetenek",
    "Tarih",
    "Coðrafya",
    "Eðitimin Temelleri ve Türk Milli Eðitim Sistemi",
    "Fizik",
    "Kimya",
    "Biyoloji",
    "Yer Bilimi",
    "Astronomi",
    "Çevre Bilimi"
};
        private void UpdateLessonDurations(DersVerileri veriler)
        {
            foreach (var ders in veriler.Dersler)
            {
                string dersAdi = ders.Key;
                string toplamSure = ders.Value.ToplamSure;

                switch (dersAdi)
                {
                    case "Sayýsal Yetenek":
                        label16.Text = toplamSure;
                        break;

                    case "Sözel Yetenek":
                        label17.Text = toplamSure;
                        break;

                    case "Tarih":
                        label18.Text = toplamSure;
                        break;

                    case "Coðrafya":
                        label19.Text = toplamSure;
                        break;

                    case "Eðitimin Temelleri ve Türk Milli Eðitim Sistemi":
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

                    case "Çevre Bilimi":
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

            label15.Text = $"Kalan Süre: {monthsRemaining} ay, {daysRemaining} gün";
        }

        private void label27_Click(object sender, EventArgs e)
        {

        }
        private int Kronometreseconds = 0;
        private bool isKronometreRunning = false;
        private string[] motivasyonSozleri = new string[]
   {
    "Baþarý, cesaretin ödülüdür. – Sophocles",
    "Baþarý, yapýlmayan iþlerin deðil, yapýlan iþlerin ürünüdür. – Thomas Edison",
    "Baþarýsýzlýk, baþarý yolundaki en büyük öðretmendir. – Bill Gates",
    "Gelecek, hayal edenlerin ellerindedir. – Eleanor Roosevelt",
    "Her hayal, büyük bir baþarýnýn tohumudur. – Mark Twain",
    "Baþarý, korkularýn deðil, cesaretin meyvesidir. – Aristoteles",
    "Baþarý, risk almayý bilmekten geçer. – Elon Musk",
    "Baþarýya inanýyorsanýz, onu elde edebilirsiniz. – Oprah Winfrey",
    "Düþenler, yeniden ayaða kalkmayý bilenlerdir. – Nelson Mandela",
    "Ýþiniz hayatýnýzýn büyük bir kýsmýný kaplayacak ve tatmin olmanýn tek yolu, harika iþler yapacaðýnýza inandýðýnýz iþi yapmaktýr. – Steve Jobs",
    "Güç, fiziksel kapasiteden deðil, iradeli bir kararlýlýktan gelir. – Mahatma Gandhi",
    "Fýrsatlar durup dururken karþýnýza çýkmaz, onlarý siz yaratýrsýnýz. – Chris Grosser",
    "Þansa çok inanýrým ve ne kadar çok çalýþtýysam ona o kadar çok sahip oldum. – Thomas Jefferson",
    "Baþarýya çýkan asansör bozuk. Merdivenleri týrmanmaya baþlayýn. – Joe Girard",
    "Hayat her ne kadar zor görünse de, yapabileceðimiz ve baþarabileceðimiz bir þey mutlaka vardýr. – Stephen Hawking",
    "Yapabileceðin en iyi þeyi yap, sonra yapabildiðin kadarýný yap, sonunda yapabileceðin þeyi yap. – Theodore Roosevelt",
    "Yavaþ yavaþ ilerlemek, hiç ilerlememekten iyidir. – Konfüçyüs",
    "Baþarý genellikle aramaktan çok meþgul olanlara gelir. — Henry David Thoreau",
    "Baþarý harekete baðlýdýr. Baþarýlý insanlar harekete geçer. Hata yaparlar ama býrakmazlar. — Conrad Hilton",
    "Baþarýya giden yol her zaman inþaat halindedir. — Lily Tomlin",
    "Her þeyin mükemmel olmasýný beklemeyin. Þimdi baþlayýn. — Mark Victor Hansen",
    "Baþarýnýzý belirleyecek þey, tavrýnýzdýr. — Zig Ziglar",
    "Baþarýlý savaþçý, lazer gibi odaklanmýþ sýradan bir insandýr. — Bruce Lee",
    "Bizim için acý gelen zorluklar, çoðunlukla gizli nimetlerdir. — Oscar Wilde",
    "Nerede olduðun önemli deðil, elindekilerle baþla ve yapabileceðini yap. — Arthur Ashe",
    "Büyük iþler yapmak istiyorsanýz, sevdiðiniz iþi yapýn. — Steve Jobs",
    "Baþarý, en dibe vurduðunuzda ne kadar yükseðe sýçrayacaðýnýzdýr. — George S. Patton",
    "Baþlamanýn yolu, konuþmayý býrakýp yapmaya baþlamaktýr. – Walt Disney",
    "Bütün merdivenleri görmek zorunda deðilsiniz. Yapmanýz gereken ilk adýmý atmaktýr. – Martin Luther King Jr.",
    "Bir þeye baþlayýp baþarýsýz olmaktan daha kötü tek þey, hiçbir þeye baþlamamaktýr. – Seth Godin",
    "Baþarýya ulaþmanýn sýrrý, adým atmaktan korkmamaktýr. – Anonim",
    "Baþarý tesadüf deðildir, sýký çalýþmanýn ve özverinin sonucudur. – Pele",
    "Zorluklara meydan okumak, baþarýnýn anahtarýdýr. – Anonim",
    "Hayatta korkulacak þey yoktur, anlaþýlacak þeyler vardýr. – Marie Curie",
    "Her þampiyon, bir zamanlar pes etmeyi reddeden bir yarýþmacýydý. – Sylvester Stallone",
    "Saati izleme; onun yaptýðýný yap, devam et. — Sam Levenson",
    "Motivasyon baþlamak için gereklidir, alýþkanlýk ise devam ettirir. — Jim Rohn",
    "Uçamýyorsanýz koþun, koþamýyorsanýz yürüyün, yürüyemiyorsanýz sürünün; ama her durumda ilerleyin. — Martin Luther King Jr.",
    "Ýlerlemenin anahtarý sürekli harekettir. — Tony Robbins",
    "En iyi çýkýþ yolu, her zaman içinden geçmektir. — Robert Frost",
    "Elinizde olanla baþlayýn, yapabileceðiniz þeyi yapýn. — Theodore Roosevelt",
    "Soru, kimin izin vereceði deðil; kimin durduracaðýdýr. — Ayn Rand",
    "Ýstediðiniz her þey, korkunun diðer tarafýnda. — George Addair",
    "En iyi motivasyon, kendi kendini motive etmektir. — Jim Rohn",
    "Baþarýnýn tek sýnýrý, zihninizdir. Bedeniniz mücadeleye hazýrdýr. — Mo Brossette",
    "Baþlamak için mükemmel olmanýza gerek yok, ama mükemmel olmak için baþlamalýsýnýz. — Zig Ziglar",
    "Yapamayacaðýnýzý düþündüðünüz her þeyi yapýn. Baþarýsýz olsanýz bile, sýnýrlarýnýzý zorlamýþ olursunuz. — Eleanor Roosevelt",
    "Ýmkansýz olaný yapmak, baþlamakla baþlar. — Nelson Mandela",
    "Yapmak zorunda olmadýðýnýz þeyleri yaptýðýnýzda, baþkalarýnýn yapamadýðý þeyleri yaparsýnýz. — Alexander Graham Bell",
    "Yarýný inþa etmenin en iyi yolu, bugünden baþlamaktýr. — Abraham Lincoln",
    "Zafer, zafer benimdir diyebilenindir. Baþarý ise “baþaracaðým” diye baþlayarak sonunda “baþardým” diyenindir. — Mustafa Kemal Atatürk",
    "Kazanma isteði ve baþarýya ulaþma arzusu birleþirse kiþisel mükemmelliðin kapýsýný açar. — Konfüçyüs",
    "Hiçbir þeyden vazgeçme, çünkü sadece kaybedenler vazgeçer. — Abraham Lincoln",
    "Sessizce sýký çalýþýn, býrakýn baþarý sesiniz olsun. — Frank Ocean",
    "Baþarý son deðildir, baþarýsýzlýk ise ölümcül deðildir: Önemli olan ilerlemeye cesaret etmektir. — Winston S. Churchill"
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
                    ShowReminder("Dilerseniz küçük bir mola verebilirsiniz.", "MOLA HATIRLATMASI");
                }
                else if (Kronometreseconds == 3600)
                {
                    ShowReminder("TOPLAM 60 DAKÝKAYI GERÝDE BIRAKTINIZ", "MOLA HATIRLATMASI");
                }
                else if (Kronometreseconds == 5400)
                {
                    ShowReminder("1 SAAT 30 DAKÝKA SÜREYÝ GERÝDE BIRAKTINIZ", "MOLA HATIRLATMASI");
                }
                else if (Kronometreseconds == 7200)
                {
                    ShowReminder("2 SAATÝ GERÝDE BIRAKTINIZ", "MOLA HATIRLATMASI");
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
                label36.Text = "Toplam Süre: 0 gün 0 saat 0 dakika 0 saniye";
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

            label36.Text = $" {gun} gün {saat} saat {dakika} dakika {saniye} saniye çalýþtýnýz.";
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
                button1.Text = "Kronometre Baþlat";
                this.Text = oldTitle;
            }
            else
            {

                if (comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Lütfen bir ders seçiniz.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string secilenDers = comboBox1.SelectedItem.ToString();

                if (!dersSureleri.ContainsKey(secilenDers))
                {
                    dersSureleri[secilenDers] = TimeSpan.Zero;
                }

                timer2.Start();
                button1.Text = "Çalýþmayý Bitir";

                this.WindowState = FormWindowState.Minimized;
                this.Text = "DERS ÇALIÞILIYOR";

                notifyIcon1.BalloonTipTitle = "ÝYÝ ÇALIÞMALAR  ";
                notifyIcon1.BalloonTipText = "KRONOMETRE ÞUANDA DERS SÜRESÝNÝ KAYDETMEKTEDÝR.";
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
            string mesaj = "Ders Çalýþma Süreleri:\n";
            foreach (var item in dersSureleri)
            {
                mesaj += $"{item.Key}: {item.Value}\n";
            }
            MessageBox.Show(mesaj);
        }
    }
}
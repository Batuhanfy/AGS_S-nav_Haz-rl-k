using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListAGS
{

    public class DersVerileri
    {
        public string ToplamSure { get; set; } = "00:00:00";
        public Dictionary<string, DersBilgisi> Dersler { get; set; } = new Dictionary<string, DersBilgisi>();
    }

    public class DersBilgisi
    {
        public string ToplamSure { get; set; } = "00:00:00";
        public Dictionary<string, string> GunlukKayitlar { get; set; } = new Dictionary<string, string>();
    }

}

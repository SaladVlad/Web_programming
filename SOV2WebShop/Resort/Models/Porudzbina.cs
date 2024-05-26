namespace Resort.Models
{
    public class Porudzbina
    {
        public Porudzbina(int kod, string adresa, string telefon, string proizvod, string tipPlacanja)
        {
            Kod = kod;
            Adresa = adresa;
            Telefon = telefon;
            Proizvod = proizvod;
            TipPlacanja = tipPlacanja;
        }

        public int Kod { get; set; }
        public string Adresa { get; set; }
        public string Telefon { get; set; }
        public string Proizvod {  get; set; }
        public string TipPlacanja { get; set; }
    }
}
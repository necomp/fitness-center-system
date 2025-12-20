namespace fitcensys.Models
{
    public class AIViewModel
    {
        // Kullanıcı Verileri
        public string FullName { get; set; }
        public int Age { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public string Gender { get; set; }

        // Hedef
        public string SelectedGoal { get; set; }

        // AI Çıktıları
        public string AIResponse { get; set; } // Tavsiye Metni
        public string ImageUrl { get; set; }   // <-- YENİ: Üretilen resmin adresi
        // public string AICharacter { get; set; } <-- BUNU SİLDİK (Goril yok)
    }
}
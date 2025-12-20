namespace fitcensys.Extensions
{
    public static class EnumExtensions
    {
        // "this DayOfWeek day" diyerek DayOfWeek tipine yeni bir metot ekliyoruz.
        public static string ToTurkishName(this DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Monday => "Pazartesi",
                DayOfWeek.Tuesday => "Salı",
                DayOfWeek.Wednesday => "Çarşamba",
                DayOfWeek.Thursday => "Perşembe",
                DayOfWeek.Friday => "Cuma",
                DayOfWeek.Saturday => "Cumartesi",
                DayOfWeek.Sunday => "Pazar",
                _ => "Bilinmiyor"
            };
        }
    }
}
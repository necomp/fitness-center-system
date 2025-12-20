using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using fitcensys.Models;
using System.Text;
using Newtonsoft.Json;

namespace fitcensys.Controllers
{
    [Authorize]
    public class AIController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        // --- DİKKAT: API ÇALIŞMIYORSA BUNU 'true' YAP ---
        // 'true' yaparsan OpenAI'ye bağlanmaz, sahte resim ve yazı gösterir. (Test için)
        private const bool USE_MOCK_DATA = false;

        public AIController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new AIViewModel
            {
                FullName = user.FirstName + " " + user.LastName,
                Height = user.Height,
                Weight = user.Weight,
                Age = user.BirthDate != DateTime.MinValue ? DateTime.Now.Year - user.BirthDate.Year : 25,
                Gender = "Erkek"
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateAdvice(AIViewModel model)
        {
            // 1. ADIM: VERİLERİ KURTAR (Veri kaybını önlemek için şart)
            // Sayfa yenilendiğinde boy/kilo 0 olmasın diye Identity'den tekrar çekiyoruz
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                model.FullName = user.FirstName + " " + user.LastName;
                model.Height = user.Height;
                model.Weight = user.Weight;
                model.Age = user.BirthDate != DateTime.MinValue ? DateTime.Now.Year - user.BirthDate.Year : 25;
                model.Gender = "Erkek";
            }

            var apiKey = _configuration["OpenAI:ApiKey"];
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            try
            {
                // 2. ADIM: LİSTENDEN EN STABİL MODELİ SEÇELİM
                // Preview modeller yerine listendeki en kararlı model olan 'gpt-4o-2024-11-20' deniyoruz.
                var textBody = new
                {
                    model = "gpt-4o-2024-11-20",
                    messages = new[] {
                new { role = "user", content = $"Boy:{model.Height}cm, Kilo:{model.Weight}kg, Hedef:{model.SelectedGoal}. Kısa tavsiye ver." }
            }
                };

                var textResp = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions",
                    new StringContent(JsonConvert.SerializeObject(textBody), Encoding.UTF8, "application/json"));

                var resultText = await textResp.Content.ReadAsStringAsync();

                if (textResp.IsSuccessStatusCode)
                {
                    dynamic jsonText = JsonConvert.DeserializeObject(resultText);
                    model.AIResponse = jsonText.choices[0].message.content;

                    var imgBody = new
                    {
                        model = "dall-e-3", // En yüksek model
                        prompt = $"A high-end, ultra-realistic, cinematic fitness photography of a {model.Age} years old muscular {model.Gender.ToLower()} athlete, achieving the goal of '{model.SelectedGoal}'. " +
                         $"Inside a modern, high-tech gym with neon aesthetic lighting, sharp focus, 8k resolution, professional lighting, shot on 35mm lens.",
                        n = 1,
                        size = "1024x1024", // DALL-E 3 için ideal boyut
                        quality = "hd",      // HD kalite ekledik
                        style = "vivid"      // Renklerin daha canlı olması için
                    };

                    var imgResp = await _httpClient.PostAsync("https://api.openai.com/v1/images/generations",
                        new StringContent(JsonConvert.SerializeObject(imgBody), Encoding.UTF8, "application/json"));

                    if (imgResp.IsSuccessStatusCode)
                    {
                        dynamic jsonImg = JsonConvert.DeserializeObject(await imgResp.Content.ReadAsStringAsync());
                        model.ImageUrl = jsonImg.data[0].url;
                    }
                    else
                    {
                        var imgError = await imgResp.Content.ReadAsStringAsync();
                        model.AIResponse += $"\n\n⚠️ GÖRSEL HATASI: {imgError}";
                    }
                }
                else
                {
                    model.AIResponse = $"❌ API YETKİ SORUNU: {resultText}";
                }
            }
            catch (Exception ex)
            {
                model.AIResponse = $"❌ SİSTEM HATASI: {ex.Message}";
            }

            return View("Index", model);
        }
    }
}
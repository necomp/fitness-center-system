using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace fitcensys.Services
{
    // IEmailSender interface'ini implemente ediyoruz.
    // Şimdilik metodun içi boş, sadece hatayı susturmak için var.
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Burası boş dönüyor, yani mail atılmış gibi davranıyor.
            return Task.CompletedTask;
        }
    }
}
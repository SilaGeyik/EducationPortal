using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace EducationPortal.Web.Hubs
{
    public class NotificationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            // Admin rol/grup kontrolü yapmıyoruz.
            // Çünkü admin panelde zaten bu layout çalışıyor.
            return base.OnConnectedAsync();
        }
    }
}

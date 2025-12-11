using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace EducationPortal.Web.Hubs
{
    public class NotificationHub : Hub
    {
        // Her yeni bağlantı kurulduğunda çalışır
        public override async Task OnConnectedAsync()
        {
            // Sadece bağlanan kullanıcıya mesaj
            await Clients.Caller.SendAsync(
                "ReceiveNotification",
                "Yönetici paneline gerçek zamanlı bağlantı kuruldu."
            );

            await base.OnConnectedAsync();
        }

       
        public async Task SendDashboardInfo(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}


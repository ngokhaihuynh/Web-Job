using Microsoft.AspNet.SignalR;

namespace WebJob
{
    public class ChatHub : Hub
    {
        public void SendMessage(string sender, string receiver, string message)
        {
            Clients.All.addNewMessageToPage(sender, message); // Gửi tin nhắn đến tất cả người dùng
        }
    }
}

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebJob.Models;
using WebJob.Models.EF;

namespace WebJob.Areas.Admin.Controllers
{
    public class MessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Hiển thị lịch sử tin nhắn của một người dùng cụ thể
        // Trang quản lý tin nhắn
        public ActionResult Conversations()
        {
            // Lấy các cuộc hội thoại từ cơ sở dữ liệu, không sử dụng GetUserId() trong LINQ
            var conversations = db.conversations
                .OrderByDescending(c => c.LastMessageTime)
                .ToList(); // Lấy về danh sách hội thoại

            // Lấy thông tin người dùng hiện tại (UserID)
            var currentUserId = User.Identity.GetUserId();

            // Dựng lại danh sách các hội thoại với thông tin về tin nhắn mới
            var conversationViewModels = conversations.Select(c => new ConversationViewModel
            {
                Conversation = c,
                UserName = db.Users.FirstOrDefault(u => u.Id == c.UserID)?.UserName,  // Lấy tên người dùng
                HasNewMessage = c.Messages.Any(m => m.IsRead == false && m.SenderID != currentUserId) // Kiểm tra tin nhắn chưa đọc
            }).ToList();

            return View(conversationViewModels);
        }

        public ActionResult ConversationDetails(int id)
        {
            // Tìm cuộc hội thoại theo ID và bao gồm tin nhắn liên quan
            var conversation = db.conversations
                .Include("Messages") // Sử dụng Include để lấy tin nhắn liên quan
                .FirstOrDefault(c => c.ConversationID == id);

            // Nếu không tìm thấy hội thoại
            if (conversation == null)
            {
                return HttpNotFound();
            }

            // Lấy tên người dùng từ bảng AspNetUsers hoặc bảng của bạn
            var userName = db.Users
                .Where(u => u.Id == conversation.UserID) // Giả sử conversation.UserID chứa UserID của người dùng
                .Select(u => u.UserName) // Hoặc u.FullName nếu bạn muốn lấy tên đầy đủ
                .FirstOrDefault();

            // Cập nhật trạng thái "đã đọc" cho tất cả tin nhắn chưa được đọc
            var messagesToUpdate = conversation.Messages.Where(m => !m.IsRead).ToList();
            foreach (var message in messagesToUpdate)
            {
                message.IsRead = true; // Đánh dấu là đã đọc
            }

            db.SaveChanges(); // Lưu lại các thay đổi vào cơ sở dữ liệu

            // Trả về view với cuộc hội thoại và danh sách tin nhắn đã được cập nhật cùng với tên người dùng
            ViewBag.UserName = userName; // Lưu tên người dùng vào ViewBag

            return View(conversation);
        }



        [HttpPost]
        public ActionResult SendMessage(int conversationId, string content)
        {
            var adminId = User.Identity.GetUserId();

            // Thêm tin nhắn mới
            var message = new Message
            {
                ConversationID = conversationId,
                SenderID = adminId,
                Content = content,
                Timestamp = DateTime.Now,
                IsRead = false
            };

            db.message.Add(message);

            // Cập nhật thời gian tin nhắn cuối cùng trong cuộc hội thoại
            var conversation = db.conversations.FirstOrDefault(c => c.ConversationID == conversationId);
            if (conversation != null)
            {
                conversation.LastMessageTime = DateTime.Now;
            }

            db.SaveChanges();

            // Chỉ trả về một phần HTML cho tin nhắn mới
            return PartialView("_MessagePartial", message);
        }
        public ActionResult GetMessages(int conversationId)
        {
            var conversation = db.conversations
                .Include("Messages")
                .FirstOrDefault(c => c.ConversationID == conversationId);

            if (conversation == null)
            {
                return HttpNotFound();
            }

            var messages = conversation.Messages.OrderBy(m => m.Timestamp).ToList();

            // Trả về một Partial View với tất cả tin nhắn trong cuộc hội thoại
            return PartialView("_SendMessagePartial", messages);
        }


    }
}
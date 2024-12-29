using System;
using System.Linq;
using System.Web.Mvc;
using WebJob.Models.EF;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using WebJob.Models;

namespace WebJob.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       
        [HttpPost]
        public JsonResult SendMessage(string content)
        {
            var userId = User.Identity.GetUserId();

            // Tìm cuộc trò chuyện của người dùng, nếu không có thì tạo mới
            var conversation = db.conversations.FirstOrDefault(c => c.UserID == userId);

            if (conversation == null) // Nếu chưa có cuộc trò chuyện
            {
                conversation = new Conversation
                {
                    UserID = userId,
                    LastMessageTime = DateTime.Now
                };
                db.conversations.Add(conversation);
                db.SaveChanges(); // Lưu để lấy ConversationID
            }

            // Tạo tin nhắn mới
            var message = new Message
            {
                ConversationID = conversation.ConversationID, // Gán ID cuộc trò chuyện
                SenderID = userId, // ID người gửi
                Content = content, // Nội dung tin nhắn
                Timestamp = DateTime.Now, // Thời gian gửi
                IsRead = false // Đánh dấu chưa đọc
            };

            db.message.Add(message); // Thêm tin nhắn vào database

            // Cập nhật thời gian cuối của cuộc trò chuyện
            conversation.LastMessageTime = DateTime.Now;

            db.SaveChanges(); // Lưu thay đổi

            return Json(new { success = true, message = content });
        }


        [HttpGet]
        public JsonResult GetMessages()
        {
            var userId = User.Identity.GetUserId(); // Lấy ID người dùng hiện tại

            // Tìm cuộc trò chuyện của người dùng
            var conversation = db.conversations.FirstOrDefault(c => c.UserID == userId);

            if (conversation != null)
            {
                // Lấy tất cả tin nhắn của cuộc trò chuyện đó
                var messages = db.message
                    .Where(m => m.ConversationID == conversation.ConversationID)
                    .OrderBy(m => m.Timestamp) // Sắp xếp theo thời gian
                    .ToList() // Tải dữ liệu về bộ nhớ
                    .Select(m => new
                    {
                        Sender = m.SenderID == userId ? "Bạn" : "Hệ thống", // Xác định người gửi
                        Content = m.Content,
                        Timestamp = m.Timestamp // Không định dạng tại đây
                    })
                    .ToList();

                return Json(messages, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, messages = new List<object>() }, JsonRequestBehavior.AllowGet);
        }
    }
}

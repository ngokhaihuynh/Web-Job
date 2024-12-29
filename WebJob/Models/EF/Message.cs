using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebJob.Models.EF
{
    [Table("Message")]
    public class Message
    {
        [Key]
        public int MessageID { get; set; }

        [Required]
        public int ConversationID { get; set; }

        [Required]
        public string SenderID { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime Timestamp { get; set; }

        public bool IsRead { get; set; }

        // Liên kết với Conversation
        [ForeignKey("ConversationID")]
        public virtual Conversation Conversation { get; set; }
       
    }
}
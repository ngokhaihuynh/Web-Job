using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebJob.Models.EF
{
    [Table("Conversations")]
    public class Conversation
    {
        [Key]
        public int ConversationID { get; set; }

        [Required]
        public string UserID { get; set; }

        
        public string AdminID { get; set; }

        public DateTime LastMessageTime { get; set; }
        
        public bool IsArchived { get; set; }

        // Liên kết với Messages
        public virtual ICollection<Message> Messages { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebJob.Models.EF;

namespace WebJob.Models
{
    public class ConversationViewModel
    {
        public Conversation Conversation { get; set; }
        public string UserName { get; set; }
        public bool HasNewMessage { get; set; }
    }

}
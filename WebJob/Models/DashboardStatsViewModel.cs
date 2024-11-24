using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebJob.Models
{
    public class DashboardStatsViewModel
    {
       
            public int NewUsers { get; set; }      // Số lượng người dùng mới trong tuần
            public int NewJobs { get; set; }       // Số lượng công việc mới trong tuần
            public int NewContacts { get; set; }   // Số lượng phản hồi mới (chưa đọc)
        

    }
}
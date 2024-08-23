using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebJob.Models
{
    public class SaveJob
    {
        public List<SaveJobItem> Items { get; set; }
        public SaveJob()
        {
            this.Items = new List<SaveJobItem>();
        }

        public void AddSaveJob(SaveJobItem item)
        {
            var checkExits = Items.FirstOrDefault(x => x.SaveJobId == item.SaveJobId);
            if(checkExits != null)
            {
                checkExits.SaveJobName = item.SaveJobName;
                checkExits.SaveJobCate = item.SaveJobCate;
                checkExits.SaveJobImg = item.SaveJobImg;
                checkExits.SaveJobSalaryMin = item.SaveJobSalaryMin;
                checkExits.SaveJobSalaryMax = item.SaveJobSalaryMax;
            }
            else
            {
                Items.Add(item);
            }
        }
        public void Remove(int id) {
            var checkExits = Items.SingleOrDefault(x => x.SaveJobId == id);
            if(checkExits != null)
            {
                Items.Remove(checkExits);
            }
         
        }
        public void ClearSave()
        {
            Items.Clear();
        }

    }

    public class SaveJobItem
    {
        public int SaveJobId { get; set; }
        public string SaveJobName { get; set; }
        public string Alias { get; set; }
        public string SaveJobCate { get; set; }
        public string SaveJobImg { get; set; }
        public int SaveJobSalaryMin { get; set; }
        public int SaveJobSalaryMax { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solar1TesterSorterLableGenerator.Model
{
    public class NewTesterSorterPackingModel
    {
        [Key]
        public Guid NewTesterSorterPackingID { get; set; }

        public string BinNo { get; set; }
        public string Class { get; set; }
        public string Eff { get; set; }
        public string Power{ get; set; }

        public string Lot { get; set; }
        public string Shift { get; set; }
        public string BatchNumber { get; set; }
        public DateTime Date { get; set; }
        public string PDate { get; set; }

        public string PackSerialNumber { get; set; }//DateTime.Now.ToString("yyyyMMdd"); //GetLastSerialNumber
        public bool IsDeleted { get; set; }
        public int SystemUserID { get; set; }
        //public virtual SystemUser SystemUser { get; set; }
    }
}

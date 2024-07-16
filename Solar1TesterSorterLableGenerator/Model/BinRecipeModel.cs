using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solar1TesterSorterLableGenerator.DatabaseHelper
{
    public class BinRecipeModel
    {
        public int BinRecipeModelID { get; set; }
        public string BinNumber { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string IV { get; set; } = string.Empty;
        public string Power { get; set; } = string.Empty;
        public string Eff { get; set; } = string.Empty;
        public string Print { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
    }
}

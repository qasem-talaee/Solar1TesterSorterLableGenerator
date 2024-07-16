using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solar1TesterSorterLableGenerator
{
    public static class SerialNumberHelper
    {
        private static string _path = Path.Combine(Environment.CurrentDirectory, "LastSerialNumber.txt");
        private static List<string> _serialData = new List<string>();

        public static string GetLastSerialNumber()
        {            
            return DateTime.Now.ToString("yyyyMMdd") + Convert.ToInt32(ReadSerialNumber()[1]).ToString("0000");
        }

        private static List<string> ReadSerialNumber() {
            try
            {
                _serialData = File.ReadAllLines(_path).ToList();
                var lastGeneratedDate = Convert.ToDateTime(_serialData[0]);

                if (DateTime.Now.Date != lastGeneratedDate.Date)
                {
                    _serialData[0] = DateTime.Now.ToString("yyyy/MM/dd");
                    _serialData[1] = "1";
                    File.WriteAllLines(_path, _serialData);
                }
                else
                {
                    _serialData[1] = Convert.ToString(Convert.ToInt32(_serialData[1]) + 1);
                    File.WriteAllLines(_path, _serialData);
                }
            }
            catch (Exception ex)
            {
                _serialData[0] = DateTime.Now.ToString("yyyy/MM/dd");
                _serialData[1] = "1";
                File.WriteAllLines(_path, _serialData);
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            return _serialData;
        }



    }
}

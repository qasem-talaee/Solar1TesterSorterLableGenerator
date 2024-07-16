using Solar1TesterSorterLableGenerator.Model;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solar1TesterSorterLableGenerator.Helper
{
    public class FileManagerHelper
    {
        public string JonasFilePath = ConfigurationSettings.AppSettings["JonasFileLocation"];
        public string LogFilePath = Path.Combine(Environment.CurrentDirectory, "Log", DateTime.Now.ToString("yyyy-MM-dd"));
        public string LogFileName = "";

        public FileManagerHelper()
        {
            if (!Directory.Exists(LogFilePath))
            {
                Directory.CreateDirectory(LogFilePath);
            }
            if(!File.Exists(Path.Combine(Environment.CurrentDirectory, "JonasReadedLog")))
            {
                File.Create(Path.Combine(Environment.CurrentDirectory, "JonasReadedLog"));
            }
        }

        public List<DataFromJonasModel> ReadInfoFormFile()
        {
            List<string> files = new List<string>();
            files = Directory.GetFiles(JonasFilePath, "Box*.log").OrderBy(x => x).ToList();
            if (files.Count > 0)
            {
                try
                {
                    LogFileName = Path.GetFileName(files[files.Count - 1]).Replace(".log", "") + DateTime.Now.ToString("yyyy-mm-ss-HH-mm-ss") + ".log";
                    File.Move(files[files.Count - 1], Path.Combine(LogFilePath, LogFileName));
                }
                catch (Exception)
                {
                    LogFileName = "";
                    MessageBox.Show("امکان خواندن اطلاعات وجود ندارد.لطفا مجددا تلاش کنید", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            List<DataFromJonasModel> Result = new List<DataFromJonasModel>();
            int index = 0;
            if (LogFileName != "")
            {
                string file = Path.Combine(LogFilePath, LogFileName);
                string[] lines = File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    if (line.Contains("BOX"))
                    {
                        DataFromJonasModel element = new DataFromJonasModel();
                        List<string> fields = line.Split(';').ToList();
                        for(int i=0; i<fields.Count; i++)
                        {
                            element.ID = index;
                            List<string> subField = fields[i].Split('=').ToList();
                            if (i == 0)
                            {
                                element.BoxNumber = subField[subField.Count - 1].Replace(";", "");
                            }
                            if(i == 2)
                            {
                                element.Code = subField[subField.Count - 1].Remove(subField[subField.Count - 1].IndexOf(" "));
                            }
                        }
                        Result.Add(element);
                        index++;
                    }
                }
            }
            Result = ManagePreviousSystemLogFile(Result);
            LogFileName = "";
            return Result;
        }

        public List<DataFromJonasModel> ManagePreviousSystemLogFile(List<DataFromJonasModel> list)
        {
            string file = Path.Combine(Environment.CurrentDirectory, "JonasReadedLog");
            string[] previousData = File.ReadAllLines(file);
            File.WriteAllText(file, "");
            foreach(string line in previousData)
            {
                string[] items = line.Split('-');
                if(list.Count != 0)
                {
                    list.Add(new DataFromJonasModel()
                    {
                        ID = list[list.Count - 1].ID + 1,
                        BoxNumber = items[1],
                        Code = items[2],
                    });
                }
                else
                {
                    list.Add(new DataFromJonasModel()
                    {
                        ID = 0,
                        BoxNumber = items[1],
                        Code = items[2],
                    });
                }
            }
            foreach (DataFromJonasModel line in list)
            {
                File.AppendAllText(file, line.ID.ToString() + "-" + line.BoxNumber + "-" + line.Code + Environment.NewLine);
            }
            return list;
        }


    }
}

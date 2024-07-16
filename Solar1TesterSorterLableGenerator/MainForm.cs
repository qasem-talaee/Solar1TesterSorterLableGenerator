using Microsoft.EntityFrameworkCore;
using Solar1TesterSorterLableGenerator.DatabaseHelper;
using Solar1TesterSorterLableGenerator.Helper;
using Solar1TesterSorterLableGenerator.Model;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Export;
using static Stimulsoft.Base.StiDataLoaderHelper;

namespace Solar1TesterSorterLableGenerator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        //ScannerHelper scannerHelper = new ScannerHelper();
        public FileManagerHelper FileManagerHelper = new FileManagerHelper();
        ApiHelper apiHelper = new ApiHelper();
        private TesterSorterDbContext _db = new TesterSorterDbContext();
        private string _scanedBinNumber = "";
        private List<NewTesterSorterPackingModel> PackingDataSource = new List<NewTesterSorterPackingModel>();
        private List<DataFromJonasModel> JonasDataSource = new List<DataFromJonasModel>();
        DataFromJonasModel SelectedDataFromJonasTable;
        private string ActionButtonStatus = "Add";
        public NewTesterSorterPackingModel PackModelForUpdate = new NewTesterSorterPackingModel();
        public string ReportFilePath = Path.Combine(Environment.CurrentDirectory, "Report", "Report.mrt");

        private void btn_print_Click(object sender, EventArgs e)
        {
            if(ActionButtonStatus == "Add")
            {
                if (SelectedDataFromJonasTable == null)
                {
                    MessageBox.Show("بین اسکن نشده است", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (drl_shift.Text == "")
                    {
                        MessageBox.Show("شیفت مشخص نیست", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show("آیا می خواهید پرینت کنید؟", "تاییدیه", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (File.Exists(ReportFilePath))
                            {
                                string GeneratedSerialNumber = SerialNumberHelper.GetLastSerialNumber();
                                NewTesterSorterPackingModel PackModel = new NewTesterSorterPackingModel()
                                {
                                    NewTesterSorterPackingID = new Guid(),
                                    BinNo = _db.Bs_BinRecipeModel.FirstOrDefault(x => x.Code == SelectedDataFromJonasTable.Code).BinNumber,
                                    Class = txt_class.Text,
                                    Eff = txt_eff.Text,
                                    Power = txt_power.Text,
                                    Lot = txt_lot.Text,
                                    Shift = drl_shift.Text,
                                    BatchNumber = txt_batch.Text,
                                    Date = DateTime.Now,
                                    PDate = DateTime.Now.ToPersianDateString(),
                                    PackSerialNumber = GeneratedSerialNumber,
                                    IsDeleted = false,
                                    SystemUserID = 0,

                                };

                                StiReport stiReport = new StiReport();
                                //stiReport.PrinterSettings.Collate = true;
                                ReportModel reportInput = new ReportModel()
                                {
                                    SerialNumber = PackModel.PackSerialNumber,
                                    Class = PackModel.Class,
                                    Eff = PackModel.Eff,
                                    Power = PackModel.Power,
                                    Lot = PackModel.Lot,
                                    BatchNumber = PackModel.BatchNumber,
                                    Date = PackModel.Date.ToString("yyyy/M/dd HH:mm:ss"),
                                };
                                stiReport.Load(ReportFilePath);
                                stiReport.RegData("p", reportInput);
                                stiReport.Render();
                                stiReport.Show();

                                bool result = apiHelper.CreatePrintPacking(PackModel);
                                if (!result)
                                {
                                    MessageBox.Show("خطایی رخ داده است", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    GetAllPackDataSource();
                                    ClearForm();
                                    ManageJonasReadedLogFile();
                                    SelectedDataFromJonasTable = null;
                                }
                            }
                            else
                            {
                                MessageBox.Show("فایل گزارش پیدا نشد", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                
            

            }
            if (ActionButtonStatus == "Update")
            {
                if (drl_shift.Text == "" && PackModelForUpdate != null)
                {
                    MessageBox.Show("شیفت مشخص نیست", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("آیا می خواهید آپدیت کنید؟", "تاییدیه", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (File.Exists(ReportFilePath))
                        {
                            PackModelForUpdate.BinNo = PackModelForUpdate.BinNo;
                            PackModelForUpdate.Class = txt_class.Text;
                            PackModelForUpdate.Eff = txt_eff.Text;
                            PackModelForUpdate.Power = txt_power.Text;
                            PackModelForUpdate.Lot = txt_lot.Text;
                            PackModelForUpdate.Shift = drl_shift.Text;
                            PackModelForUpdate.BatchNumber = txt_batch.Text;

                            StiReport stiReport = new StiReport();
                            ReportModel reportInput = new ReportModel()
                            {
                                SerialNumber = PackModelForUpdate.PackSerialNumber,
                                Class = PackModelForUpdate.Class,
                                Eff = PackModelForUpdate.Eff,
                                Power = PackModelForUpdate.Power,
                                Lot = PackModelForUpdate.Lot,
                                BatchNumber = PackModelForUpdate.BatchNumber,
                                Date = PackModelForUpdate.Date.ToString("yyyy/M/dd HH:mm:ss"),
                            };
                            stiReport.Load(ReportFilePath);
                            stiReport.RegData("p", reportInput);
                            stiReport.Render();
                            stiReport.Show();

                            bool result = apiHelper.UpdatePrintPacking(PackModelForUpdate);
                            if (!result)
                            {
                                MessageBox.Show("خطایی رخ داده است", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                GetAllPackDataSource();
                                ClearForm();
                            }
                        }
                        else
                        {
                            MessageBox.Show("فایل گزارش پیدا نشد", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void ManageJonasReadedLogFile()
        {
            JonasDataSource.Remove(JonasDataSource.FirstOrDefault(x => x.ID == SelectedDataFromJonasTable.ID));
            grid_from_jonas.DataSource = null;
            grid_from_jonas.DataSource = JonasDataSource;
            string file = Path.Combine(Environment.CurrentDirectory, "JonasReadedLog");
            File.WriteAllText(file, "");
            foreach (DataFromJonasModel line in JonasDataSource)
            {
                File.AppendAllText(file, line.ID.ToString() + "-" + line.BoxNumber + "-" + line.Code + Environment.NewLine);
            }
        }

        public void GetAllPackDataSource()
        {
            PackingDataSource = apiHelper.GetAllPrintPacking();
            grid_today_work.DataSource = PackingDataSource;
            lbl_count.Text = "Count : " + PackingDataSource.Count.ToString();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            grid_today_work.BestFitColumns(BestFitColumnMode.DisplayedCells);
            grid_today_work.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            grid_from_jonas.BestFitColumns(BestFitColumnMode.DisplayedCells);
            grid_from_jonas.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            lbl_lastBinScan.Text = "";
            lbl_lastExit.Text = "";
            GetAllPackDataSource();
            ReadFromPreviousJonasLogFile();
        }

        private void ReadFromPreviousJonasLogFile()
        {
            string file = Path.Combine(Environment.CurrentDirectory, "JonasReadedLog");
            string[] previousData = File.ReadAllLines(file);
            foreach (string line in previousData)
            {
                string[] items = line.Split('-');
                JonasDataSource.Add(new DataFromJonasModel()
                {
                    ID = Convert.ToInt32(items[0]),
                    BoxNumber = items[1],
                    Code = items[2],
                });
            }
            grid_from_jonas.DataSource = JonasDataSource;
        }

        /*
        private void ScannerHelper_ScanComplated(object sender, string e)
        {
            SetControlText(e);
        }
        */

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //scannerHelper.CloseSerialPort();
            //LoginForm loginForm = new LoginForm();
            //loginForm.Show();
        }

        #region CrossThread Handler
        /*
        public void SetControlText( string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(SetControlText), new object[] { text });
            }
            else
            {
                _scanedBinNumber = text;
                BinRecipeModel binRecipeModel = _db.Bs_BinRecipeModel.FirstOrDefault(x => x.BinNumber == _scanedBinNumber);
                if (binRecipeModel == null)
                {
                    MessageBox.Show("بین در رسپی پیدا نشد.لطفا با واحد پروسس تماس بگیرید", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _scanedBinNumber = "";
                }
                else
                {
                    ActionButtonStatus = "Add";
                    btn_print.Text = "Print";
                    PackModelForUpdate = null;
                    lbl_lastExit.Text = "";
                    txt_class.Text = binRecipeModel.Class;
                    txt_eff.Text = binRecipeModel.Eff;
                    txt_power.Text = binRecipeModel.Power;
                    lbl_lastBinScan.Text = "Bin " + _scanedBinNumber;
                }
            }
        }
        */
        #endregion

        private void btn_recipe_Click(object sender, EventArgs e)
        {
            RecipeForm recipeForm = new RecipeForm();
            recipeForm.ShowDialog();
        }

        public void ClearForm()
        {
            txt_class.Text = "";
            txt_eff.Text = "";
            txt_power.Text = "";
            txt_lot.Text = "";
            _scanedBinNumber = "";
            lbl_lastBinScan.Text = "";
            ActionButtonStatus = "Add";
            btn_print.Text = "Print";
            PackModelForUpdate = null;
            lbl_lastExit.Text = "";
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void grid_today_work_CommandCellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Column != null)
            {
                if (e.Column.GetType() == typeof(GridViewCommandColumn))
                {
                    var btn = (GridViewCommandColumn)e.Column;
                    if (btn.Name == "btn_print")
                    {
                        if (File.Exists(ReportFilePath))
                        {
                            StiReport stiReport = new StiReport();
                            ReportModel reportInput = new ReportModel()
                            {
                                SerialNumber = grid_today_work.SelectedRows[0].Cells[2].Value.ToString(),
                                Class = grid_today_work.SelectedRows[0].Cells[3].Value.ToString(),
                                Eff = grid_today_work.SelectedRows[0].Cells[7].Value.ToString(),
                                Power = grid_today_work.SelectedRows[0].Cells[6].Value.ToString(),
                                Lot = grid_today_work.SelectedRows[0].Cells[4].Value.ToString(),
                                BatchNumber = grid_today_work.SelectedRows[0].Cells[5].Value.ToString(),
                                Date = DateTime.Parse(grid_today_work.SelectedRows[0].Cells[8].Value.ToString()).ToString("yyyy/M/dd HH:mm:ss"),
                            };
                            stiReport.Load(ReportFilePath);
                            stiReport.RegData("p", reportInput);
                            stiReport.Render();
                            stiReport.Show();
                        }
                        else
                        {
                            MessageBox.Show("فایل گزارش پیدا نشد", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    if (btn.Name == "btn_delete")
                    {
                        DialogResult dialogResult = MessageBox.Show("آیا می خواهید حذف کنید؟", "تاییدیه", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            bool result = apiHelper.DeletePrintPacking(grid_today_work.SelectedRows[0].Cells[0].Value.ToString());
                            if (result)
                            {
                                ClearForm();
                                GetAllPackDataSource();
                            }
                            else
                            {
                                MessageBox.Show("خطایی رخ داده است", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        private void grid_today_work_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Column != null)
            {
                try
                {
                    PackModelForUpdate = PackingDataSource.FirstOrDefault(x => x.NewTesterSorterPackingID.ToString() == grid_today_work.SelectedRows[0].Cells[0].Value.ToString());
                    txt_class.Text = PackModelForUpdate.Class;
                    txt_eff.Text = PackModelForUpdate.Eff;
                    txt_power.Text = PackModelForUpdate.Power;
                    txt_lot.Text = PackModelForUpdate.Lot;
                    txt_batch.Text = PackModelForUpdate.BatchNumber;
                    ActionButtonStatus = "Update";
                    btn_print.Text = "Update";
                    lbl_lastBinScan.Text = "";
                    lbl_lastExit.Text = "Exit Time : " + PackModelForUpdate.Date.ToString();
                }
                catch { }
            }
        }

        private void btn_getExel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Csv files (*.csv)|*.csv";
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                if(filePath != "" || filePath != null)
                {
                    string headerText = "Count,Serial Number,Batch Number,Bin Number,Class,Efficiency,Power,Lot,Shift,Date,Jalali Date";
                    File.WriteAllText(filePath, headerText);
                    int index = 1;
                    foreach(NewTesterSorterPackingModel packModel in PackingDataSource)
                    {
                        string text = index.ToString() + ",\"=\"\"" + packModel.PackSerialNumber + "\"\"\"," + packModel.BatchNumber + "," + packModel.BinNo + "," + packModel.Class + "," + packModel.Eff + "," + packModel.Power + "," + packModel.Lot + "," + packModel.Shift + "," + packModel.Date.ToString() + "," + packModel.PDate;
                        File.AppendAllText(filePath, Environment.NewLine + text);
                        index = index + 1;
                    }
                    MessageBox.Show("فایل با موفقیت ذخیره شد", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btn_getInfoFromFile_Click(object sender, EventArgs e)
        {
            JonasDataSource = FileManagerHelper.ReadInfoFormFile();
            grid_from_jonas.DataSource = JonasDataSource;
        }

        private void grid_from_jonas_CommandCellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Column != null)
            {
                if (e.Column.GetType() == typeof(GridViewCommandColumn))
                {
                    var btn = (GridViewCommandColumn)e.Column;
                    if (btn.Name == "btn_select")
                    {
                        SelectedDataFromJonasTable = new DataFromJonasModel()
                        {
                            ID = Convert.ToInt32(grid_from_jonas.SelectedRows[0].Cells[0].Value.ToString()),
                            BoxNumber = grid_from_jonas.SelectedRows[0].Cells[1].Value.ToString(),
                            Code = grid_from_jonas.SelectedRows[0].Cells[2].Value.ToString(),
                        };
                        BinRecipeModel binRecipeModel = _db.Bs_BinRecipeModel.FirstOrDefault(x => x.Code == SelectedDataFromJonasTable.Code);
                        if (binRecipeModel == null)
                        {
                            MessageBox.Show("کلاس در رسپی پیدا نشد.لطفا با واحد پروسس تماس بگیرید", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            _scanedBinNumber = "";
                        }
                        else
                        {
                            ActionButtonStatus = "Add";
                            btn_print.Text = "Print";
                            PackModelForUpdate = null;
                            lbl_lastExit.Text = "";
                            txt_class.Text = binRecipeModel.Class;
                            txt_eff.Text = binRecipeModel.Eff;
                            txt_power.Text = binRecipeModel.Power;
                            lbl_lastBinScan.Text = binRecipeModel.BinNumber;
                        }
                    }
                }
            }
        }


    }
}

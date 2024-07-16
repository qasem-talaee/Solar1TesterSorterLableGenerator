using Microsoft.EntityFrameworkCore;
using Solar1TesterSorterLableGenerator.DatabaseHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Solar1TesterSorterLableGenerator
{
    public partial class RecipeForm : Form
    {
        public RecipeForm()
        {
            InitializeComponent();
        }

        public List<BinRecipeModel> BinDataSourceList { get; set; }
        TesterSorterDbContext _db = new TesterSorterDbContext();
        public string ButtonActionStatus = "Add";
        public int BinIDForUpdate;

        private void RecipeForm_Load(object sender, EventArgs e)
        {
            grid_bin_list.BestFitColumns(BestFitColumnMode.DisplayedCells);
            grid_bin_list.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            ReadListFromDatabase();
        }

        public void ReadListFromDatabase()
        {
            BinDataSourceList = _db.Bs_BinRecipeModel.ToList();
            grid_bin_list.DataSource = BinDataSourceList;
        }

        public void ClearForm()
        {
            txt_binNumber.Text = "";
            txt_code.Text = "";
            txt_iv.Text = "";
            txt_power.Text = "";
            txt_Eff.Text = "";
            txt_print.Text = "";
            txt_class.Text = "";
            btn_action.Text = "Add";
            ButtonActionStatus = "Add";
        }

        private void btn_action_Click(object sender, EventArgs e)
        {
            if(txt_binNumber.Text != "")
            {
                if(ButtonActionStatus == "Add")
                {
                    if(BinDataSourceList.FirstOrDefault(x => x.BinNumber == txt_binNumber.Text) == null)
                    {
                        BinRecipeModel binRecipeModelAdd = new BinRecipeModel()
                        {
                            BinRecipeModelID = 0,
                            BinNumber = txt_binNumber.Text,
                            Code = txt_code.Text,
                            IV = txt_iv.Text,
                            Power = txt_power.Text,
                            Eff = txt_Eff.Text,
                            Print = txt_print.Text,
                            Class = txt_class.Text,
                        };
                        _db.Bs_BinRecipeModel.Add(binRecipeModelAdd);
                        _db.SaveChanges();
                        ClearForm();
                        ReadListFromDatabase();
                    }
                    else
                    {
                        MessageBox.Show("شماره بین تکراری است", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if(ButtonActionStatus == "Update")
                {
                    BinRecipeModel binRecipeModelUpdate = BinDataSourceList.FirstOrDefault(x => x.BinRecipeModelID == BinIDForUpdate);
                    binRecipeModelUpdate.BinNumber = txt_binNumber.Text;
                    binRecipeModelUpdate.Code = txt_code.Text;
                    binRecipeModelUpdate.IV = txt_iv.Text;
                    binRecipeModelUpdate.Power = txt_power.Text;
                    binRecipeModelUpdate.Eff = txt_Eff.Text;
                    binRecipeModelUpdate.Print = txt_print.Text;
                    binRecipeModelUpdate.Class = txt_class.Text;
                    _db.Bs_BinRecipeModel.Update(binRecipeModelUpdate);
                    _db.SaveChanges();
                    ClearForm();
                    ReadListFromDatabase();
                }
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void grid_bin_list_CommandCellClick(object sender, GridViewCellEventArgs e)
        {
            if(e.Column != null)
            {
                if(e.Column.GetType() == typeof(GridViewCommandColumn))
                {
                    var btn = (GridViewCommandColumn)e.Column;
                    if (btn.Name == "btn_remove")
                    {
                        DialogResult dialogResult = MessageBox.Show("آیا می خواهید حذف کنید؟", "تاییدیه", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            BinRecipeModel binRecipeModel = BinDataSourceList.FirstOrDefault(x => x.BinRecipeModelID == Convert.ToInt32(grid_bin_list.SelectedRows[0].Cells[0].Value.ToString()));
                            _db.Bs_BinRecipeModel.Remove(binRecipeModel);
                            _db.SaveChanges();
                            ClearForm();
                            ReadListFromDatabase();
                        }
                    }
                }
            }
        }

        private void grid_bin_list_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Column != null)
            {
                try
                {
                    BinRecipeModel binRecipeModel = BinDataSourceList.FirstOrDefault(x => x.BinRecipeModelID == Convert.ToInt32(grid_bin_list.SelectedRows[0].Cells[0].Value.ToString()));
                    BinIDForUpdate = binRecipeModel.BinRecipeModelID;
                    txt_binNumber.Text = binRecipeModel.BinNumber;
                    txt_code.Text = binRecipeModel.Code;
                    txt_iv.Text = binRecipeModel.IV;
                    txt_power.Text = binRecipeModel.Power;
                    txt_Eff.Text = binRecipeModel.Eff;
                    txt_print.Text = binRecipeModel.Print;
                    txt_class.Text = binRecipeModel.Class;
                    ButtonActionStatus = "Update";
                    btn_action.Text = "Update";
                }
                catch { }
            }
        }


    }
}

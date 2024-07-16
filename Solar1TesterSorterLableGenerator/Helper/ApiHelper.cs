using Solar1TesterSorterLableGenerator.DatabaseHelper;
using Solar1TesterSorterLableGenerator.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solar1TesterSorterLableGenerator.Helper
{
    public class ApiHelper
    {
        private string SaveDataStatus = ConfigurationSettings.AppSettings["SaveDataStatus"];
        private TesterSorterDbContext _db = new TesterSorterDbContext();

        public bool CreatePrintPacking(NewTesterSorterPackingModel model)
        {
            if(SaveDataStatus == "Local")
            {
                try
                {
                    _db.Bs_NewTesterSorterPacking.Add(model);
                    _db.SaveChanges();
                    return true;
                }
                catch(Exception)
                {
                    return false;
                }
            }
            if(SaveDataStatus == "Server")
            {

            }
            return false;
        }

        public bool UpdatePrintPacking(NewTesterSorterPackingModel model)
        {
            if (SaveDataStatus == "Local")
            {
                try
                {
                    _db.Bs_NewTesterSorterPacking.Update(model);
                    _db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            if (SaveDataStatus == "Server")
            {

            }
            return false;
        }

        public bool DeletePrintPacking(string modelID)
        {
            if (SaveDataStatus == "Local")
            {
                try
                {
                    NewTesterSorterPackingModel model = _db.Bs_NewTesterSorterPacking.FirstOrDefault(x => x.NewTesterSorterPackingID.ToString() == modelID);
                    _db.Bs_NewTesterSorterPacking.Remove(model);
                    _db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            if (SaveDataStatus == "Server")
            {

            }
            return false;
        }

        public NewTesterSorterPackingModel GetPrintPacking(string modelID)
        {
            if (SaveDataStatus == "Local")
            {
                try
                {
                    NewTesterSorterPackingModel model = _db.Bs_NewTesterSorterPacking.FirstOrDefault(x => x.NewTesterSorterPackingID.ToString() == modelID);
                    return model;
                }
                catch (Exception)
                {
                    return new NewTesterSorterPackingModel();
                }
            }
            if (SaveDataStatus == "Server")
            {

            }
            return new NewTesterSorterPackingModel();
        }

        public List<NewTesterSorterPackingModel> GetAllPrintPacking()
        {
            List<NewTesterSorterPackingModel> models = new List<NewTesterSorterPackingModel>();

            if (SaveDataStatus == "Local")
            {
                try
                {
                    models = _db.Bs_NewTesterSorterPacking.Where(x => !x.IsDeleted && x.Date.Year == DateTime.Now.Year && x.Date.Month == DateTime.Now.Month && x.Date.Day == DateTime.Now.Day).OrderByDescending(x => x.Date).ToList();
                    return models;
                }
                catch (Exception)
                {
                    return models;
                }
            }
            if (SaveDataStatus == "Server")
            {

            }
            return models;
        }


    }
}

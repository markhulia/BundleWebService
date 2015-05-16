using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Isab.Data;

namespace BundleWebService
{
    /// <summary>
    /// Summary description for BundlesWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class BundlesWS : System.Web.Services.WebService
    {
        private static Random _rnd = new Random(10);
        private static string[] _descriptions = new[] {"Compressor", "Hose", "Battery", "Front door", "Back door", "Antenna", "Fuse", "Radiator", "Transmission", "Tire"};
        private static string[] _items = new[] { "L001", "L002", "L003", "L004", "L005", "L006", "L007", "L008", "L009", "L010" };

        private static List<PickRow> _rows = new List<PickRow>();
        private static object _lockObject = new object();

        static BundlesWS()
        {
            GenerateRows();
        }

        private static void GenerateRows()
        {
            for (int i = 0; i < 5; i++)
            {
                var partIndex = _rnd.Next(0, 9);
                _rows.Add(new PickRow
                {
                    Id = Guid.NewGuid().ToString(),
                    ItemNo = string.Format(_items[partIndex]),
                    Quantity = _rnd.Next(1, 100),
                    Location = string.Format("L{0:0000}", _rnd.Next(1, 100)),
                    Description = _descriptions[partIndex]
                });
            }
        }

        [WebMethod]
        public PickRow GetNextRow()
        {
            lock (_lockObject)
            {
                if (_rows.Count == 0)
                {
                    GenerateRows();

                    // Return null to simulate that there is no more rows, 
                    // on next GetNextRow the user will receive a new row
                    return null;
                }
            }
            return _rows[0];
        }
        
        [WebMethod]
        public void Report(string id, int quantiy)
        {
            if (id == null) throw new ArgumentNullException("id");
            lock (_lockObject)
            {
                var row = _rows.FirstOrDefault(i => i.Id == id);
                if (row == null)
                    throw new InvalidOperationException("Row not found");

                // Remove the row to simulate that it is finished
                _rows.Remove(row);
            }
        }
    }
}

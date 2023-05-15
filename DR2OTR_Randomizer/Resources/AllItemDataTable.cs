using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DR2OTR_Randomizer.Resources
{
    public class AllItemDataTable
    {

        
        public DataTable SetAllItemData()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load($"{Application.StartupPath}\\Resources\\AllItemData.xml");

            var dataTable = new DataTable();

            dataTable.Columns.Add("ItemState", typeof(bool));
            dataTable.Columns.Add("ItemName", typeof(string));
            dataTable.Columns.Add("ItemTag", typeof(string));

            foreach (XmlElement xmlElement in xmlDocument.DocumentElement)
            {
                dataTable.Rows.Add(Convert.ToBoolean(xmlElement.SelectSingleNode("ItemBool").InnerText),
                     xmlElement.SelectSingleNode("ItemName").InnerText,
                     xmlElement.SelectSingleNode("ItemTag").InnerText
                );
            }
            return dataTable;
        }
    }
}

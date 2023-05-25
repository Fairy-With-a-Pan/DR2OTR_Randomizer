using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DR2OTR_Randomizer.Resources
{
    public class AllItemStatData
    {
        /// <summary>
        /// Use this to add all the data for the item stats
        /// </summary>
        public List<object> GetAllItemStatData()
        {
            //Gets the xml file and loads it
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load($"{Application.StartupPath}\\Resources\\ItemStatData.xml");
            List<object> listOffStatLists = new List<object>();

            foreach (XmlElement element in xmlDocument.DocumentElement)
            {
                ///Gose through each element in the xml file get and
                ///the elemnts being the item stats catagory and the
                ///nodes being the item stats with all the data inside
                ///onces its got all the data and gone though all the children
                ///nodes it will and it to the object List and clear the list
                ///for the next catorgry
                List<ItemStatsData> list = new List<ItemStatsData>();
                foreach (XmlNode node in element.ChildNodes)
                {
                    list.Add(new ItemStatsData()
                    {
                        StatState = Convert.ToBoolean(node.ChildNodes[0].InnerText),
                        StatName = node.ChildNodes[1].InnerText,
                        StatDescription = node.ChildNodes[2].InnerText,
                        StatMin = Convert.ToInt32(node.ChildNodes[3].InnerText),
                        StatMax = Convert.ToInt32(node.ChildNodes[4].InnerText),
                        StatInGameName = node.ChildNodes[5].InnerText
                    }   );
                }
                listOffStatLists.Add(list);
            }
            return listOffStatLists;
        }       
    }
}
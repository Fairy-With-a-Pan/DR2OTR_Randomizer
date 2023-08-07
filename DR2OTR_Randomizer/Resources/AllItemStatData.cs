using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DR2OTR_Randomizer.Resources
{
    public class AllItemStatData
    {
        public string path = $"{Application.StartupPath}\\Resources\\ItemStatData.xml";

        /// <summary>
        /// Use this to add all the data for the item stats
        /// </summary>
        public List<object> GetAllItemStatData()
        {
            //Gets the xml file and loads it
            XmlDocument xmlDocument = new();
            xmlDocument.Load($"{path}");
            List<object> listOffStatLists = new();

            foreach (XmlElement element in xmlDocument.DocumentElement)
            {
                ///Gets each item stat category by Getting the first 
                ///element witch is the StatsCategory and pass each
                ///of the child elements and there nodes in to a list
                ///and adding that to the 
                List<ItemStatsData> list = new();
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
                    });
                }
                listOffStatLists.Add(list);
            }
            return listOffStatLists;
        }       
    }
}
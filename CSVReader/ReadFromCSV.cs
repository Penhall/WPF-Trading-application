using CommonFrontEnd.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.CSVReader
{
    class ReadFromCSV
    {
        string currentDir = Environment.CurrentDirectory;
        public string[] ReaDFromCSV()
        {

            DirectoryInfo directory = new DirectoryInfo(
                Path.GetFullPath(Path.Combine(currentDir, @"Profile/TouchLineScrips.csv")));
            string[] Lines = File.ReadAllLines(directory.ToString());
            return Lines;
        }

        public void ReadVariables()
        {
            DirectoryInfo directory = new DirectoryInfo(
                Path.GetFullPath(Path.Combine(currentDir, @"Profile/Variables.csv")));
            //string CSVFilePathName = @"E:\TWS_DotNetNewStructure\TWS_DOTNETT\CommonFrontEnd\Variables.csv";
            string[] Lines = File.ReadAllLines(directory.ToString());

            string[] RetrieveUptrendValue = Lines[0].Split(',');
            UtilityLoginDetails.GETInstance.UpTrendColorGlobal = RetrieveUptrendValue[1];

            string[] RetrieveDownTrendValue = Lines[1].Split(',');
            UtilityLoginDetails.GETInstance.DownTrendColorGlobal = RetrieveDownTrendValue[1];

            //string[] RetrieveUpTrendHexa = Lines[2].Split(',');
            //App.UpTrendHexa = (Color)ColorConverter.ConvertFromString(RetrieveUpTrendHexa[1]);

            //string[] RetrieveDownTrendHexa = Lines[3].Split(',');
            //App.DownTrendHexa = (Color)ColorConverter.ConvertFromString(RetrieveDownTrendHexa[1]); 



        }
    }
}

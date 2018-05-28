using CommonFrontEnd.Common;
using CommonFrontEnd.SharedMemories;
using CommonFrontEnd.View.BSEBulletin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.ViewModel.BSEBulletin
{
    class BSEBulletinsBoardVM : BaseViewModel
    {

        #region MyProperties

        static BSEBulletinsBoard mWindow = null;

        private string _LinktodisplayCount;

        public string Linktodisplay
        {
            get { return _LinktodisplayCount; }
            set { _LinktodisplayCount = value; NotifyPropertyChanged(nameof(Linktodisplay)); }
        }

        public static DirectoryInfo BseBulletin = new DirectoryInfo(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"Sub Systems/BSEDATA.ini")));
        public static IniParser BseBulletindData = new IniParser(BseBulletin.ToString());

        private string _Link1;

        public string lblLink1
        {
            get { return _Link1; }
            set { _Link1 = value; NotifyPropertyChanged(nameof(lblLink1)); }
        }

        private string _Link2;

        public string lblLink2
        {
            get { return _Link2; }
            set { _Link2 = value; NotifyPropertyChanged(nameof(lblLink2)); }
        }

        private string _lblLink3;

        public string lblLink3
        {
            get { return _lblLink3; }
            set { _lblLink3 = value; NotifyPropertyChanged(nameof(lblLink3)); }
        }


        private string _textReadData = string.Empty;

        public string textReadData
        {
            get { return _textReadData; }
            set { _textReadData = value; NotifyPropertyChanged(nameof(textReadData)); }
        }


        private RelayCommand _mouseClick;

        public RelayCommand mouseClick
        {
            get
            {
                return _mouseClick ?? (_mouseClick = new RelayCommand(
                   (object e) => ClickFirstLink(e)
                       ));
            }
        }

        private RelayCommand _mouseClickLinkTwo;

        public RelayCommand mouseClickLinkTwo
        {
            get
            {
                return _mouseClickLinkTwo ?? (_mouseClickLinkTwo = new RelayCommand(
                   (object e) => ClickSecondLink(e)
                       ));
            }
        }

        private RelayCommand _mouseClickLinkThree;

        public RelayCommand mouseClickLinkThree
        {
            get
            {
                return _mouseClickLinkThree ?? (_mouseClickLinkThree = new RelayCommand(
                   (object e) => ClickThirdLink()
                       ));
            }
        }



        private RelayCommand _CloseWindowsOnEscape;

        public RelayCommand CloseWindowsOnEscape
        {
            get
            {
                return _CloseWindowsOnEscape ?? (_CloseWindowsOnEscape = new RelayCommand(
                   (object e) => CloseWindowsOnEscape_Click()
                       ));
            }
        }


        private RelayCommand _TwsHelp;
        public RelayCommand TwsHelp
        {
            get
            {
                return _TwsHelp ?? (_TwsHelp = new RelayCommand((object e) =>
                {
                    if (File.Exists(MainWindowVM.DirectoryCHMFile.ToString()))
                        System.Windows.Forms.Help.ShowHelp(null, MainWindowVM.DirectoryCHMFile.ToString());
                }));
            }
        }



        #endregion
        #region constructor
        public BSEBulletinsBoardVM()
        {
            if (ReadDatabyFile())
                ClickThirdLink();

            mWindow = System.Windows.Application.Current.Windows.OfType<BSEBulletinsBoard>().FirstOrDefault();

        }
        #endregion


        #region Methode
        private void CloseWindowsOnEscape_Click()
        {
            mWindow?.Close();
        }
        private bool ReadDatabyFile()
        {
            string[] strArray;
            string LinktodisplayCountString;

            int counter = 0;
            string line;
            bool flag = false;

            try
            {


                string SearchNextCount;
                if (!File.Exists(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"Sub Systems/BSEDATA.ini"))))
                {
                    return false;
                }
                MemoryManager.lines = File.ReadAllLines(Path.GetFullPath(Path.Combine(System.Environment.CurrentDirectory, @"Sub Systems/BSEDATA.ini"))).ToList<string>();

                for (int i = 0; i < MemoryManager.lines.Count; i++)
                {
                    if (MemoryManager.lines[i].StartsWith("Linktodisplay"))
                    {
                        string[] strarr = MemoryManager.lines[i].Split('=');
                        int LinktodisplayCount = Convert.ToInt32(strarr[1]);
                        LinktodisplayCount++;
                        SearchNextCount = "#" + LinktodisplayCount + "#";
                        flag = SEarchInFile(SearchNextCount);
                        if (flag == true)
                        {
                            Global.UtilityLoginDetails.GETInstance.NextBulletinPoint = Convert.ToString(LinktodisplayCount + 1);
                            //First Link
                            AddFirstLink(LinktodisplayCount);
                            //Second Link
                            AddSecondLink(LinktodisplayCount);
                            LinktodisplayCount += 1;
                        }
                        else
                        {
                            Global.UtilityLoginDetails.GETInstance.NextBulletinPoint = "0";
                            //First Link
                            AddFirstLink(1);
                            //Second Link
                            AddSecondLink(2);
                        }
                    }
                }


                FindDefaultBulletin();


            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);

            }
            return true;
        }

        private bool SEarchInFile(string searchNextCount)
        {
            for (int i = 0; i < MemoryManager.lines.Count; i++)
            {
                if (MemoryManager.lines[i].Contains(searchNextCount))
                {
                    int index = i;
                    return true;
                }
            }
            return false;
        }

        private void AddSecondLink(int linktodisplayCount)
        {
            int SecondLinkBulletinNumber = linktodisplayCount + 1;
            string searchSecondLinkPoint = SecondLinkBulletinNumber + "#";
            for (int i = 0; i < MemoryManager.lines.Count; i++)
            {
                if (MemoryManager.lines[i].StartsWith(searchSecondLinkPoint))
                {
                    string[] arr = MemoryManager.lines[i].Split('=');
                    lblLink2 = arr[1];
                    break;
                }
            }
        }

        private void AddFirstLink(int FirstLinkBulletinNumber)
        {
            string searchFirstLinkPoint = FirstLinkBulletinNumber + "#";
            for (int i = 0; i < MemoryManager.lines.Count; i++)
            {
                if (MemoryManager.lines[i].StartsWith(searchFirstLinkPoint))
                {
                    string[] arr = MemoryManager.lines[i].Split('=');
                    lblLink1 = arr[1];
                    break;
                }
            }


        }
        private void ClickFirstLink(object e)
        {
            string s = lblLink1.ToString();
            for (int i = 0; i < MemoryManager.lines.Count; i++)
            {
                if (MemoryManager.lines[i].Contains(s))
                {
                    string[] arr = MemoryManager.lines[i].Split('#', '=');
                    AttachDataToTextBox(arr[0]);
                    break;
                }
            }
        }

        private void ClickSecondLink(object e)
        {
            string s = lblLink2.ToString();
            for (int i = 0; i < MemoryManager.lines.Count; i++)
            {
                if (MemoryManager.lines[i].Contains(s))
                {
                    string[] arr = MemoryManager.lines[i].Split('#', '=');
                    AttachDataToTextBoxSecond(arr[0]);
                    break;
                }
            }
        }


        private void ClickThirdLink()
        {
            textReadData = string.Empty;
            string str = String.Join("\n", MemoryManager.lines);
            string[] arr = str.Split('|');
            textReadData = arr[1];
        }

        private void AttachDataToTextBox(string v)
        {
            textReadData = string.Empty;
            string SearchText = "#" + v + "#";
            string str = String.Join("\n", MemoryManager.lines);
            string[] arr = str.Split(new[] { SearchText }, StringSplitOptions.None);
            string splited = arr[1];
            string[] startwithTilda = splited.Split('~');
            string text = string.Empty;
            for (int i = 0; i < startwithTilda.Count(); i++)
            {
                if (startwithTilda[i].Contains("#"))
                {
                    break;
                }
                else
                {
                    textReadData += startwithTilda[i];
                }
            }


        }
        private void AttachDataToTextBoxSecond(string v)
        {
            textReadData = string.Empty;
            int second = Convert.ToInt16(v);

            string SearchText = "#" + second + "#";
            string str = String.Join("\n", MemoryManager.lines);
            string[] arr = str.Split(new[] { SearchText }, StringSplitOptions.None);
            string splited = arr[1];
            string[] startwithTilda = splited.Split('~');
            string text = string.Empty;
            for (int i = 0; i < startwithTilda.Count(); i++)
            {
                if (startwithTilda[i].Contains("#") || startwithTilda[i].Contains("["))
                {
                    break;
                }
                else
                {
                    textReadData += startwithTilda[i];
                }
            }

            //textReadData = arr[1];
            //for (int i = 0; i < lines.Count; i++)
            //{
            //    int index = lines.IndexOf(SearchText);
            //    if (lines[index].Contains(SearchText))
            //    {
            //        bool result = true;
            //        index = index + 1;
            //        while (result)
            //        {
            //            if (lines[index].StartsWith("~"))
            //            {
            //                textReadData += lines[index];
            //                index = index + 1;
            //            }
            //            else if(!string.IsNullOrWhiteSpace(lines[index]) && !lines[index].StartsWith("~") && !lines[index].EndsWith("~"))
            //            {
            //                textReadData += lines[index];
            //                index = index + 1;
            //            }
            //            else if (lines[index].EndsWith("~"))
            //            {
            //                textReadData += lines[index];
            //                index = index + 1;
            //            }
            //            index++;
            //        }
            //        textReadData = textReadData.Trim();
            //    }
            //}
        }
        private void FindDefaultBulletin()
        {

            //lblLink3 = BseBulletindData.GetSetting("TWSBSEDATA", "LinkName");
            //StringBuilder s = new StringBuilder();
            //string[] strArray = File.ReadAllLines(Convert.ToString(BseBulletin));
            for (int i = 0; i < MemoryManager.lines.Count(); i++)
            {
                if (MemoryManager.lines[i].Contains("LinkName="))
                {
                    string[] arr = MemoryManager.lines[i].Split('=');
                    lblLink3 = arr[1].Trim();
                    break;

                }
            }
        }



        #endregion

    }
}

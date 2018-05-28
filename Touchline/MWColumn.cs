using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CommonFrontEnd.Common.Enumerations;

namespace CommonFrontEnd.ViewModel.Touchline
{
    public class MWColumn
    {
        //Database id PKEY
        private Int32 mintId;
        //ScreenField ID
        private Int32 mintFieldId;
        private Int32 mintLegId;
        // added to handle sequence Number of the Column on screen
        private Int32 mintPosition;
        // col lable
        private string mstrName;
        //Full Formula
        private string mstrFormulaText;
        private long mlngWidth;
        //X pos
        private long mintStartPosition;
        private string mstrAlignment;
        private string mstrHidden;
        // 0.DATA_TYPE_ALPHA 2.DATA_TYPE_DATE 1.DATA_TYPE_NUMERIC
        private int mintType;
        //if not -1 then bc index else 
        private Int32 mintPositionInMessage;
        private string mintSortDirection;
        // if multiple cols sorts
        private Int32 mintSortSequence;
        private Int32 mintRowstate;
        private const int mDefaultWidth = 80;
        private long mlngMinWidth = MDefaultWidth;

        public int MintId
        {
            get
            {
                return mintId;
            }

            set
            {
                mintId = value;
            }
        }

        public int MintFieldId
        {
            get
            {
                return mintFieldId;
            }

            set
            {
                mintFieldId = value;
            }
        }

        public int MintLegId
        {
            get
            {
                return mintLegId;
            }

            set
            {
                mintLegId = value;
            }
        }

        public int MintPosition
        {
            get
            {
                return mintPosition;
            }

            set
            {
                mintPosition = value;
            }
        }

        public string MstrName
        {
            get
            {
                return mstrName;
            }

            set
            {
                mstrName = value;
            }
        }

        public string MstrFormulaText
        {
            get
            {
                return mstrFormulaText;
            }

            set
            {
                mstrFormulaText = value;
            }
        }

        public long MlngWidth
        {
            get
            {
                return mlngWidth;
            }

            set
            {
                mlngWidth = value;
            }
        }

        public long MintStartPosition
        {
            get
            {
                return mintStartPosition;
            }

            set
            {
                mintStartPosition = value;
            }
        }

        public string MstrAlignment
        {
            get
            {
                return mstrAlignment;
            }

            set
            {
                mstrAlignment = value;
            }
        }

        public string MstrHidden
        {
            get
            {
                return mstrHidden;
            }

            set
            {
                mstrHidden = value;
            }
        }

        public int MintType
        {
            get
            {
                return mintType;
            }

            set
            {
                mintType = value;
            }
        }

        public int MintPositionInMessage
        {
            get
            {
                return mintPositionInMessage;
            }

            set
            {
                mintPositionInMessage = value;
            }
        }

        public string MintSortDirection
        {
            get
            {
                return mintSortDirection;
            }

            set
            {
                mintSortDirection = value;
            }
        }

        public int MintSortSequence
        {
            get
            {
                return mintSortSequence;
            }

            set
            {
                mintSortSequence = value;
            }
        }

        public int MintRowstate
        {
            get
            {
                return mintRowstate;
            }

            set
            {
                mintRowstate = value;
            }
        }

        public static int MDefaultWidth
        {
            get
            {
                return mDefaultWidth;
            }
        }

        public long MlngMinWidth
        {
            get
            {
                return mlngMinWidth;
            }

            set
            {
                mlngMinWidth = value;
            }
        }

        public enum MWCOLS_FIELDS
        {
            Id = 0,
            FieldId = 1,
            LegId = 2,
            PositionOnScreen = 3,
            ColumnName = 4,
            FormulaText = 5,
            ColumnWidth = 6,
            Align = 7,
            Hidden = 8,
            Type = 9,
            // 1=int 2=string 3=dbl '4=dateTime =5=dateTime = 6=time
            PositionInMessage = 10,
            //in broadcast
            SortOrder = 11,
            SortSequence = 12,
            Rowstate = 13,
            SIZE = 14
        }

        public MWColumn(string[] pstrCol, ref ArrayList pNameList)
        {
            try
            {
                MintId =Convert.ToInt32(pstrCol[(int)MWColumn.MWCOLS_FIELDS.Id]);
                MintFieldId =Convert.ToInt32((pstrCol[(int)MWColumn.MWCOLS_FIELDS.FieldId]).ToString().Length == 0 ? "0" : pstrCol[(int)MWColumn.MWCOLS_FIELDS.FieldId]);
                MintLegId = Convert.ToInt32((pstrCol[(int)MWColumn.MWCOLS_FIELDS.LegId]).ToString().Length == 0 ? "0" : pstrCol[(int)MWColumn.MWCOLS_FIELDS.LegId]);
                MintPosition = Convert.ToInt32((pstrCol[(int)MWColumn.MWCOLS_FIELDS.PositionOnScreen]).ToString().Length == 0 ? "0" : pstrCol[(int)MWColumn.MWCOLS_FIELDS.PositionOnScreen]);
                if ((pNameList != null) && pNameList.Count > 0)
                {
                    //: If the User has one legged MW, he can give the LegName blank
                    if (pNameList.Count == 1 && string.IsNullOrEmpty(pNameList[MintLegId].ToString().Trim()))
                    {
                        MstrName = pstrCol[(int)MWColumn.MWCOLS_FIELDS.ColumnName];
                       
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(pNameList[MintLegId].ToString().Trim()))
                        {
                            MstrName = pstrCol[(int)MWColumn.MWCOLS_FIELDS.ColumnName];
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(pstrCol[(int)MWColumn.MWCOLS_FIELDS.FormulaText]))
                            {
                                MstrName = pstrCol[(int)MWColumn.MWCOLS_FIELDS.ColumnName];
                            }
                            else
                            {
                                MstrName = Convert.ToString(pNameList[MintLegId]) + "." + pstrCol[(int)MWColumn.MWCOLS_FIELDS.ColumnName];
                            }

                        }
                    }
                }
                else
                {
                    MstrName = pstrCol[(int)MWColumn.MWCOLS_FIELDS.ColumnName];
                }
                MstrFormulaText = pstrCol[(int)MWColumn.MWCOLS_FIELDS.FormulaText];

                MlngWidth =Convert.ToInt64(pstrCol[(int)MWColumn.MWCOLS_FIELDS.ColumnWidth]) == 0 ||Convert.ToInt32((pstrCol[(int)MWColumn.MWCOLS_FIELDS.ColumnWidth]))== 0 ? MDefaultWidth : Convert.ToInt32(pstrCol[(int)MWColumn.MWCOLS_FIELDS.ColumnWidth]);
                MintStartPosition = Convert.ToInt32(MlngWidth) == 0 ? Convert.ToInt16(MintPosition) * Convert.ToInt16(MlngWidth) : Convert.ToInt16(MintPosition) * MDefaultWidth;
                MstrAlignment = pstrCol[(int)MWColumn.MWCOLS_FIELDS.Align];
                MstrHidden = pstrCol[(int)MWColumn.MWCOLS_FIELDS.Hidden];
                MintType = Convert.ToInt32(pstrCol[(int)MWColumn.MWCOLS_FIELDS.Type]);
                //1=int 2=string 3=dbl '4=dateTime =5=dateTime = 6=time
                MintPositionInMessage = Convert.ToInt16(pstrCol[(int)MWColumn.MWCOLS_FIELDS.PositionInMessage]);
                MintSortDirection =pstrCol[(int)MWColumn.MWCOLS_FIELDS.SortOrder] == "" ? "0" : pstrCol[(int)MWColumn.MWCOLS_FIELDS.SortOrder];
                MintSortSequence = Convert.ToInt32(pstrCol[(int)MWColumn.MWCOLS_FIELDS.SortSequence]=="" ? "0" : pstrCol[(int)MWColumn.MWCOLS_FIELDS.SortSequence]);
                MintRowstate = Convert.ToInt32(Convert.ToInt32(pstrCol[(int)MWColumn.MWCOLS_FIELDS.Rowstate]) == 0 ? "0" : pstrCol[(int)MWColumn.MWCOLS_FIELDS.Rowstate]);
                //If mintFieldId = 0 AndAlso mintLegId = 0 Then
                // REM : For local formulas
                // mstrFormulaText = mstrName
                //End If
            }
            catch (Exception ex)
            {
                throw new Exception("Column Data Is Missing ", ex);
            }

        }
    }
}

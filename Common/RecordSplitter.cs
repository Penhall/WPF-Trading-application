using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.Common
{

    /// <summary>
    /// This class helps split the record sent from the server into
    /// records and fields.
    /// </summary>
    public class RecordSplitter
    {

            public static char RECORD_SEPARATOR = '~';
            public static char FIELD_SEPARATOR = '|';
            private string data;
            public string[][] records;

            /// <summary>
            /// This is the constructor to which the data returned from
            /// the server needs to be passed. This will break up the
            /// data into records and fields.
            /// </summary>
            /// <param name="pData">This is the data returned from the server.</param>
            public RecordSplitter(string pData)
            {
                data = pData;
                if (data != null)
                {
                    split();
                }
            }
            /// <summary>
            /// This method splits the data in the input into records and
            /// fields.
            /// </summary>
            private void split()
            {
                char[] lDelimiter;
                string[] lRecords;
                int lCount;
                lDelimiter = new char[1];
                lDelimiter[0] = RECORD_SEPARATOR;
                //First split the records using the RECORD_SEPARATOR
                lRecords = data.Split(lDelimiter);
                lDelimiter[0] = FIELD_SEPARATOR;
                //The length of the lRecords array will be the number of 
                //records in the data.
                //Allocate String Arrays to accomodate all the records.
                int lreclenght = lRecords.Length;
                records = new string[lreclenght][];
                //for (lCount = 0; lCount < lreclenght; lCount++)
                //{
                //    //Split each record using the field separator and
                //    //form the jagged two dimensional array of records
                //    //and fields.
                //    records[lCount] = lRecords[lCount].Split(lDelimiter);
                //}
                Parallel.For(0, lreclenght, (i) =>
                {
                    records[i] = lRecords[i].Split(lDelimiter);
                });
            }
            /// <summary>
            /// This method returns the number of records obtained from
            /// the data passed in the constructor.
            /// </summary>
            /// <returns>The number of records.</returns>
            public int numberOfRecords()
            {
                return records.Length;
            }
            /// <summary>
            /// This method returns the number of fields in the specified
            /// record
            /// </summary>
            /// <param name="pRecordPointer">This is the record from which number 
            /// of fields needs to be returned.</param>
            /// <returns>The Return value will be number of fields if the
            /// Record pointer is a valid record.
            /// Else this value will be -1.</returns>
            public int numberOfFields(int pRecordPointer)
            {
                if (pRecordPointer < 0)
                {
                    return -1;
                }
                else if (pRecordPointer < records.Length)
                {
                    return records[pRecordPointer].Length;
                }
                else
                {
                    return -1;
                }
            }
            /// <summary>
            /// This will return the requested Record.
            /// </summary>
            /// <param name="pRecordNumber"></param>
            /// <returns>The array of strings containing the fields 
            /// in the requested Record. If the requested record does 
            /// not exist then a null value is returned.</returns>
            public string[] getRecord(int pRecordNumber)
            {
                if (pRecordNumber < records.Length && pRecordNumber >= 0)
                {
                    return records[pRecordNumber];
                }
                else
                {
                    return null;
                }
            }
            /// <summary>
            /// This will return the value of the requested field.
            /// </summary>
            /// <param name="pRecordNumber">The Record number from which the field needs to be returned</param>
            /// <param name="pFieldNumber">The Field number in the record that needs to be returned.</param>
            /// <returns>This will be the value of the requested field. If
            /// either the record pointer or the field pointer is
            /// invalid or out of range a null value is returned.</returns>
            public string getField(int pRecordNumber, int pFieldNumber)
            {
                if (pRecordNumber < records.Length)
                {
                    if (pFieldNumber < records[pRecordNumber].Length)
                    {
                        return records[pRecordNumber][pFieldNumber];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            public bool setField(int pRecordNumber, int pFieldNumber, string pFieldValue)
            {
                bool lSet = false;




                if (pRecordNumber < records.Length)
                {
                    if (pFieldNumber > records[pRecordNumber].Length - 1)
                    {
                        Array.Resize(ref records[pRecordNumber], pFieldNumber + 1);
                    }
                    if (pFieldNumber < records[pRecordNumber].Length)
                    {
                        records[pRecordNumber][pFieldNumber] = pFieldValue;
                        lSet = true;
                    }
                }
                return lSet;
            }
            public string[][] getRecords()
            {
                return records;
            }

            public string GetActualData()
            {
                return data;
            }

            /// <summary>
            /// This will set the value of the requested field.
            /// </summary>
            /// <param name="pRecordNumber">The Record number whose value needs to be changed</param>
            /// <param name="strName">The Field name whose record needs to be changed.</param>
            /// <param name="strValue">The Value which has to be replaced for the corresponding Field Name.</param>
            /// <returns>This will be return the success or failure of change to the Internal Array.</returns>
            public bool setFieldByName(int pRecordNumber, string strName, string strValue)
            {
                // records[RecordNumber][FieldNumber]

                // No. of Fields passed
                long lngNoOfRecords;
                // Counter
                int pCount;
                // Index where the value is stored
                int pFieldNumber;
                // Get From Constants
                int pNameRow;

                pNameRow = 2;

                // finding the no of elements in the arrary ie. How many fields are returned
                lngNoOfRecords = records[pRecordNumber].Length;

                //finding the index where the ColumnName matches in the 3rd Row ie.where Field Names are stored
                for (pCount = 0; pCount < lngNoOfRecords; pCount++)
                {
                    // comparing with the name 
                    if (records[pNameRow][pCount] == strName)
                    {
                        pFieldNumber = pCount;
                        return setField(pRecordNumber, pFieldNumber, strValue);
                    }
                }
                return false;
            }

            // last parameter specifies Where Names are to be found ie. Header Names
            public bool setFieldByName(int pRecordNumber, string strName, string strValue, int pNameRow)
            {
                // records[RecordNumber][FieldNumber]

                // No. of Fields passed
                long lngNoOfRecords;
                // Counter
                int pCount;
                // Index where the value is stored
                int pFieldNumber;

                // finding the no of elements in the arrary ie. How many fields are returned
                lngNoOfRecords = records[pRecordNumber].Length;

                //finding the index where the ColumnName matches in the 3rd Row ie.where Field Names are stored
                for (pCount = 0; pCount < lngNoOfRecords; pCount++)
                {
                    // comparing with the name 
                    if (records[pNameRow][pCount] == strName)
                    {
                        pFieldNumber = pCount;
                        return setField(pRecordNumber, pFieldNumber, strValue);
                    }
                }
                return false;
            }

            /// <summary>
            /// This will get the value of the requested field.
            /// </summary>
            /// <param name="pRecordNumber">The Record number whose value needs to be found.</param>
            /// <param name="strName">The Field name whose record needs to be found.</param>
            /// <returns>This will be the value of the requested field. If
            /// either the record pointer or the field pointer is
            /// invalid or out of range a null value is returned.</returns>
            public string getFieldByName(int pRecordNumber, string strName)
            {

                // No. of Fields passed
                long lngNoOfRecords;
                // Counter
                int pCount;
                // Index where the value is stored
                int pFieldNumber;

                // Get From Constants
                int pNameRow;

                pNameRow = 2;

                // finding the no of elements in the arrary ie. How many fields are returned
                lngNoOfRecords = records[pNameRow].Length;

                //finding the index where the ColumnName matches in the 3rd Row ie.where Field Names are stored
                for (pCount = 0; pCount < lngNoOfRecords; pCount++)
                {
                    // comparing with the name 
                    if ((records[pNameRow][pCount]) == strName)
                    {
                        pFieldNumber = pCount;
                        return getField(pRecordNumber, pFieldNumber);
                    }
                }
                return null;
            }

            public string getFieldByName(int pRecordNumber, string strName, int pNameRow)
            {

                // No. of Fields passed
                long lngNoOfRecords;
                // Counter
                int pCount;
                // Index where the value is stored
                int pFieldNumber;

                // finding the no of elements in the arrary ie. How many fields are returned
                lngNoOfRecords = records[pNameRow].Length;

                //finding the index where the ColumnName matches in the 3rd Row ie.where Field Names are stored
                for (pCount = 0; pCount < lngNoOfRecords; pCount++)
                {
                    // comparing with the name 
                    if ((records[pNameRow][pCount]).Trim().ToUpper() == strName.Trim().ToUpper())
                    {
                        pFieldNumber = pCount;
                        return getField(pRecordNumber, pFieldNumber);
                    }
                }
                return null;
            }
        }
    
}

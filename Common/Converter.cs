using CommonFrontEnd.SharedMemories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using static CommonFrontEnd.ControllerModel.SenderControllerModel;

namespace CommonFrontEnd.Common
{
    #region Commented


    //public static class Converter
    // {

    //    /// <summary>
    //    ///  serialize Object to byte array
    //    /// </summary>
    //    /// <typeparam name="T">type param required</typeparam>
    //    /// <param name="obj">data</param>
    //    /// <returns>byte array</returns>
    //     public static byte[] ObjectToByteArray<T>(T obj)
    //     {
    //         try
    //         {
    //             BinaryFormatter bf = new BinaryFormatter();
    //             using (var ms = new MemoryStream())
    //             {
    //                 bf.Serialize(ms, obj);
    //                 return ms.ToArray();
    //             }
    //         }
    //         catch (Exception ex)
    //         {
    //             throw ex;
    //         }
    //     }

    //     /// <summary>
    //     /// deserialize a byte array to an Object
    //     /// </summary>
    //     /// <typeparam name="T">type parameter</typeparam>
    //     /// <param name="arrBytes">byte array</param>
    //     /// <returns>returns object of specified type</returns>
    //     public static T ByteArrayToObject<T>(byte[] arrBytes)
    //     {
    //         try
    //         {
    //             using (var memStream = new MemoryStream())
    //             {
    //                 var binForm = new BinaryFormatter();
    //                 memStream.Write(arrBytes, 0, arrBytes.Length);
    //                 memStream.Seek(0, SeekOrigin.Begin);
    //                 var obj = binForm.Deserialize(memStream);
    //                 return (T)obj;
    //             }
    //         }
    //         catch (Exception ex)
    //         {
    //             throw ex;
    //         }
    //     }
    // }
    #endregion
#if TWS
    public static class Converter
    {

        #region Data Structures Conversion

        /// <summary>
        ///  Serialize object (of Type T) to byte array with specified array length. 
        /// </summary>
        /// <typeparam name="T">Type param</typeparam>
        /// <param name="obj">data</param>
        /// <param name="count">Specify array length. default is 0</param>
        /// <param name="offset">Start index to write the data. default is 0</param>
        /// <returns>Returns byte array. If array length is not specified then length of bytes array after binary serialization will be considered</returns>
        public static byte[] ObjectToByteArray<T>(T obj, int count, int offset)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                byte[] newArray;
                using (var ms = new MemoryStream())
                {
                    bf.Serialize(ms, obj);
                    if (count > 0)
                    {
                        newArray = new byte[count];
                    }
                    else
                    {
                        newArray = new byte[ms.ToArray().Length];
                    }

                    ms.ToArray().CopyTo(newArray, offset);

                    return newArray;
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                throw ex;
            }
        }

        /// <summary>
        /// deserialize a byte array to an Object
        /// </summary>
        /// <typeparam name="T">type parameter</typeparam>
        /// <param name="arrBytes">byte array</param>
        /// <returns>returns object of specified type</returns>
        public static T ByteArrayToObject<T>(byte[] arrBytes)
        {
            try
            {
                using (var memStream = new MemoryStream())
                {
                    var binForm = new BinaryFormatter();
                    memStream.Write(arrBytes, 0, arrBytes.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    var obj = binForm.Deserialize(memStream);
                    return (T)obj;
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                throw ex;
            }
        }

        public static byte[] GetMessageInBytes<T>(T oModel, MessagesMessage msg) where T : class
        {
            //GetXMLFromObject(oModel);
            List<byte> data = new List<byte>();
            byte[] buffer;
            int modelPropertyIndex = 0;

            var msgType = Convert.ToInt64(msg.Number);
            PropertyInfo[] modelProperties = MemoryManager._assemblyTypeMetadata[msgType].Item1;//oModel.GetType().GetProperties();//Consider only request
            for (int i = 0; i < msg.Items.Length; i++)
            {
                switch (msg.Items[i].Source)
                {
                    case "M":
                        modelPropertyIndex = Array.IndexOf(modelProperties, modelProperties.Where(x => x.Name == msg.Items[i].Name).FirstOrDefault());
                        if (modelPropertyIndex >= 0)
                        {
                            switch (msg.Items[i].Type.ToLower())
                            {
                                case "int8_t":
                                    sbyte signed8bit = Convert.ToSByte(Convert.ToInt16(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    byte Signed8bitByte = Convert.ToByte(signed8bit);
                                    data.Add(Signed8bitByte);
                                    break;

                                case "uint8_t":
                                    byte uSigned8bit = Convert.ToByte(Convert.ToInt16(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.Add(uSigned8bit);
                                    break;

                                case "int16_t":
                                    buffer = BitConverter.GetBytes(Convert.ToInt16(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;

                                case "uint16_t":
                                    buffer = BitConverter.GetBytes(Convert.ToUInt16(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;

                                case "int32_t":
                                    buffer = BitConverter.GetBytes(Convert.ToInt32(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;

                                case "uint32_t":
                                    buffer = BitConverter.GetBytes(Convert.ToUInt32(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;

                                case "int64_t":
                                    buffer = BitConverter.GetBytes(Convert.ToInt64(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;

                                case "uint64_t":
                                    buffer = BitConverter.GetBytes(Convert.ToUInt64(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;

                                case "char":
                                    if (msg.Items[i].Name.ToLower().Equals("hour") || msg.Items[i].Name.ToLower().Equals("minute") ||
                                        msg.Items[i].Name.ToLower().Equals("second"))
                                    {
                                        data.Add(BitConverter.GetBytes(Convert.ToInt32(modelProperties[modelPropertyIndex].GetValue(oModel)))[0]);
                                    }
                                    else
                                    {
                                        char[] text = new char[Convert.ToInt32(msg.Items[i].Length as object)];
                                        text = modelProperties[modelPropertyIndex].GetValue(oModel).ToString().ToCharArray();
                                        Array.Resize(ref text, Convert.ToInt32(msg.Items[i].Length as object));
                                        foreach (char c in text)
                                        {
                                            buffer = BitConverter.GetBytes(c);
                                            data.Add(buffer[0]);
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case "D":
                        string defaultValue = msg.Items[i].Value;
                        switch (msg.Items[i].Type.ToLower())//for 
                        {
                            case "int8_t":
                                sbyte signed8bit = Convert.ToSByte(Convert.ToInt16(defaultValue));
                                byte Signed8bitByte = Convert.ToByte(signed8bit);
                                data.Add(Signed8bitByte);
                                break;

                            case "uint8_t":
                                byte uSigned8bit = Convert.ToByte(Convert.ToInt16(defaultValue));
                                data.Add(uSigned8bit);
                                break;

                            case "int16_t":
                                buffer = BitConverter.GetBytes(Convert.ToInt16(defaultValue));
                                data.AddRange(buffer.ToList<byte>());
                                break;

                            case "uint16_t":
                                buffer = BitConverter.GetBytes(Convert.ToUInt16(defaultValue));
                                data.AddRange(buffer.ToList<byte>());
                                break;

                            case "int32_t":
                                buffer = BitConverter.GetBytes(Convert.ToInt32(defaultValue));
                                data.AddRange(buffer.ToList<byte>());
                                break;

                            case "uint32_t":
                                buffer = BitConverter.GetBytes(Convert.ToUInt32(defaultValue));
                                data.AddRange(buffer.ToList<byte>());
                                break;

                            case "int64_t":
                                buffer = BitConverter.GetBytes(Convert.ToInt64(defaultValue));
                                data.AddRange(buffer.ToList<byte>());
                                break;

                            case "uint64_t":
                                buffer = BitConverter.GetBytes(Convert.ToUInt64(defaultValue));
                                data.AddRange(buffer.ToList<byte>());
                                break;

                            case "char":

                                if (msg.Items[i].Name.ToLower().Equals("hour") || msg.Items[i].Name.ToLower().Equals("minute") ||
                                    msg.Items[i].Name.ToLower().Equals("second"))
                                {
                                    data.Add(BitConverter.GetBytes(Convert.ToInt32(defaultValue))[0]);
                                }
                                else
                                {
                                    char[] text = new char[Convert.ToInt32(msg.Items[i].Length as object)];
                                    text = defaultValue.ToCharArray();
                                    Array.Resize(ref text, Convert.ToInt32(msg.Items[i].Length as object));
                                    foreach (char c in text)
                                    {
                                        buffer = BitConverter.GetBytes(c);
                                        data.Add(buffer[0]);
                                    }
                                }
                                break;
                        }
                        break;
                    case "N":
                        switch (msg.Items[i].Type.ToLower())//for 
                        {
                            case "int8_t":
                                sbyte signed8bit = Convert.ToSByte(Convert.ToInt16(Enumerations.SignedInt.OneByte));
                                byte Signed8bitByte = Convert.ToByte(signed8bit);
                                data.Add(Signed8bitByte);
                                break;

                            case "uint8_t":
                                byte uSigned8bit = Convert.ToByte(Convert.ToInt16(Enumerations.USignedInt.OneByte));
                                data.Add(uSigned8bit);
                                break;

                            case "int16_t":
                                buffer = BitConverter.GetBytes(Convert.ToInt16(Enumerations.SignedInt.TwoByte));
                                data.AddRange(buffer.ToList<byte>());
                                break;

                            case "uint16_t":
                                buffer = BitConverter.GetBytes(Convert.ToUInt16(Enumerations.USignedInt.TwoByte));
                                data.AddRange(buffer.ToList<byte>());
                                break;

                            case "int32_t":
                                buffer = BitConverter.GetBytes(Convert.ToInt32(Enumerations.SignedInt.FourByte));
                                data.AddRange(buffer.ToList<byte>());
                                break;

                            case "uint32_t":
                                buffer = BitConverter.GetBytes(Convert.ToUInt32(Enumerations.USignedInt.FourByte));
                                data.AddRange(buffer.ToList<byte>());
                                break;

                            case "int64_t":
                                buffer = BitConverter.GetBytes(Convert.ToInt64(Enumerations.SignedInt.EightByte));
                                data.AddRange(buffer.ToList<byte>());
                                break;

                            case "uint64_t":
                                buffer = BitConverter.GetBytes(Convert.ToUInt64(Enumerations.USignedInt.EightByte));
                                data.AddRange(buffer.ToList<byte>());
                                break;

                            case "char":

                                if (msg.Items[i].Name.ToLower().Equals("hour") || msg.Items[i].Name.ToLower().Equals("minute") ||
                                    msg.Items[i].Name.ToLower().Equals("second"))
                                {
                                    data.Add(BitConverter.GetBytes(Convert.ToInt32(Enumerations.FixedString.FirstPosition))[0]);
                                }
                                else
                                {
                                    char[] text = new char[Convert.ToInt32(msg.Items[i].Length as object)];
                                    text = (Enumerations.FixedString.FirstPosition).ToString().ToCharArray();
                                    Array.Resize(ref text, Convert.ToInt32(msg.Items[i].Length as object));
                                    foreach (char c in text)
                                    {
                                        buffer = BitConverter.GetBytes(c);
                                        data.Add(buffer[0]);
                                    }
                                }
                                break;
                        }
                        break;
                    case "U":
                        modelPropertyIndex = Array.IndexOf(modelProperties, modelProperties.Where(x => x.Name == msg.Items[i].Name).FirstOrDefault());
                        if (modelPropertyIndex >= 0)
                        {
                            switch (msg.Items[i].Type.ToLower())
                            {
                                case "int8_t|clienttype|u|orrq":
                                    var tempclientType = -1;
                                    var dataclienttype = modelProperties[modelPropertyIndex].GetValue(oModel).ToString();
                                    if (Enum.IsDefined(typeof(Enumerations.Order.ClientTypes), modelProperties[modelPropertyIndex].GetValue(oModel).ToString()))
                                    {
                                        tempclientType = (int)Enum.Parse(typeof(Enumerations.Order.ClientTypes), dataclienttype, true);
                                    }
                                    sbyte signed8bit = Convert.ToSByte(Convert.ToInt16(tempclientType));//Convert.ToSByte(Convert.ToInt16(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    byte Signed8bitByte = Convert.ToByte(signed8bit);
                                    data.Add(Signed8bitByte);
                                    break;
                                case "int8_t|buysellindicator|u|orrq":
                                    var tempBuySellFlag = -1;
                                    var dataBuySellFlag = modelProperties[modelPropertyIndex].GetValue(oModel).ToString();
                                    if (Enum.IsDefined(typeof(Enumerations.Order.BuySellFlag), modelProperties[modelPropertyIndex].GetValue(oModel).ToString()))
                                    {
                                        tempBuySellFlag = (int)Enum.Parse(typeof(Enumerations.Order.BuySellFlag), dataBuySellFlag, true);
                                    }
                                    signed8bit = Convert.ToSByte(Convert.ToInt16(tempBuySellFlag));//Convert.ToSByte(Convert.ToInt16(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    Signed8bitByte = Convert.ToByte(signed8bit);
                                    data.Add(Signed8bitByte);
                                    break;
                                case "int8_t|ordertype|u|orrq":
                                    var tempOrderType = -1;
                                    var dataOrderType = modelProperties[modelPropertyIndex].GetValue(oModel).ToString();
                                    if (Enum.IsDefined(typeof(Enumerations.Order.OrderTypes), modelProperties[modelPropertyIndex].GetValue(oModel).ToString()))
                                    {
                                        tempOrderType = (int)Enum.Parse(typeof(Enumerations.Order.OrderTypes), dataOrderType, true);
                                        if (Enumerations.Order.OrderTypes.OCO.ToString() == dataOrderType)
                                        {
                                            tempOrderType = 0;//if OCO order send orderType limit to backend
                                        }
                                    }
                                    signed8bit = Convert.ToSByte(Convert.ToInt16(tempOrderType));//Convert.ToSByte(Convert.ToInt16(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    Signed8bitByte = Convert.ToByte(signed8bit);
                                    data.Add(Signed8bitByte);
                                    break;
                                case "int8_t|orderretentionstatus|u|orrq":
                                    var tempRetType = -1;
                                    var dataRetType = modelProperties[modelPropertyIndex].GetValue(oModel).ToString();
                                    if (Enum.IsDefined(typeof(Enumerations.Order.RetType), modelProperties[modelPropertyIndex].GetValue(oModel).ToString()))
                                    {
                                        tempRetType = (int)Enum.Parse(typeof(Enumerations.Order.RetType), dataRetType, true);
                                    }
                                    signed8bit = Convert.ToSByte(Convert.ToInt16(tempRetType));//Convert.ToSByte(Convert.ToInt16(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    Signed8bitByte = Convert.ToByte(signed8bit);
                                    data.Add(Signed8bitByte);
                                    break;
                                case "int8_t|execinst|u|orrq":
                                    var tempExecInst = -1;
                                    var dataExecInst = modelProperties[modelPropertyIndex].GetValue(oModel).ToString();
                                    if (Enum.IsDefined(typeof(Enumerations.Order.ExecInst), modelProperties[modelPropertyIndex].GetValue(oModel).ToString()))
                                    {
                                        tempExecInst = (int)Enum.Parse(typeof(Enumerations.Order.ExecInst), dataExecInst, true);
                                    }
                                    signed8bit = Convert.ToSByte(Convert.ToInt16(tempExecInst));//Convert.ToSByte(Convert.ToInt16(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    Signed8bitByte = Convert.ToByte(signed8bit);
                                    data.Add(Signed8bitByte);
                                    break;

                                case "uint16_t|exchange|u|orrq":
                                    //Enumerations.Order.Exchanges
                                    var tempExchange = -1;
                                    var dataExchange = modelProperties[modelPropertyIndex].GetValue(oModel).ToString();
                                    if (Enum.IsDefined(typeof(Enumerations.Order.Exchanges), modelProperties[modelPropertyIndex].GetValue(oModel).ToString()))
                                    {
                                        tempExchange = (int)Enum.Parse(typeof(Enumerations.Order.Exchanges), dataExchange, true);
                                    }
                                    buffer = BitConverter.GetBytes(Convert.ToUInt16(tempExchange));//BitConverter.GetBytes(Convert.ToUInt16(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;
                                case "uint16_t|segment|u|orrq":
                                    var tempSegment = -1;
                                    var dataSegment = modelProperties[modelPropertyIndex].GetValue(oModel).ToString();
                                    if (Enum.IsDefined(typeof(Enumerations.Order.ScripSegment), modelProperties[modelPropertyIndex].GetValue(oModel).ToString()))
                                    {
                                        tempSegment = (int)Enum.Parse(typeof(Enumerations.Order.ScripSegment), dataSegment, true);
                                    }
                                    buffer = BitConverter.GetBytes(Convert.ToUInt16(tempSegment));//BitConverter.GetBytes(Convert.ToUInt16(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;
                                case "int32_t|quantity|u|orrq":
                                    buffer = BitConverter.GetBytes(Convert.ToInt32(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;
                                case "int32_t|revealqty|u|orrq":
                                    buffer = BitConverter.GetBytes(Convert.ToInt32(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;
                                case "int32_t|marketsegmentid|u|orrq":
                                    buffer = BitConverter.GetBytes(Convert.ToInt32(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;
                                case "uint32_t|messagetag|u|orrq":
                                    buffer = BitConverter.GetBytes(Convert.ToUInt32(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;

                                case "int64_t|ordernumber|u|orrq":
                                    buffer = BitConverter.GetBytes(Convert.ToInt64(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;
                                case "int64_t|price|u|orrq":
                                    buffer = BitConverter.GetBytes(Convert.ToInt64(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;
                                case "int64_t|triggerprice|u|orrq":
                                    buffer = BitConverter.GetBytes(Convert.ToInt64(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;
                                case "int64_t|protectionpercentage|u|orrq":
                                    string strprotectionpercentage = Convert.ToString(modelProperties[modelPropertyIndex].GetValue(oModel));
                                    decimal decprotectionpercentage = 0.0m;
                                    if (!string.IsNullOrEmpty(strprotectionpercentage))
                                    {
                                        decprotectionpercentage = Convert.ToDecimal(strprotectionpercentage);
                                    }
                                    buffer = BitConverter.GetBytes(Convert.ToInt64(decprotectionpercentage));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;
                                case "int64_t|senderlocationid|u|orrq":
                                    buffer = BitConverter.GetBytes(Convert.ToInt64(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;
                                case "int64_t|scripcode|u|orrq":
                                    buffer = BitConverter.GetBytes(Convert.ToInt64(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                    data.AddRange(buffer.ToList<byte>());
                                    break;
                                //case "uint64_t":
                                //    buffer = BitConverter.GetBytes(Convert.ToUInt64(modelProperties[modelPropertyIndex].GetValue(oModel)));
                                //    data.AddRange(buffer.ToList<byte>());
                                //    break;

                                case "char|orderaction|u|orrq":
                                    var tempOrderAction = "";
                                    var dataOrderAction = modelProperties[modelPropertyIndex].GetValue(oModel).ToString();
                                    if (Enum.IsDefined(typeof(Enumerations.Order.Modes), modelProperties[modelPropertyIndex].GetValue(oModel).ToString()))
                                    {
                                        tempOrderAction = Convert.ToString(Enum.Parse(typeof(Enumerations.Order.Modes), dataOrderAction, true));
                                    }
                                    char[] text = tempOrderAction.ToString().ToCharArray();
                                    foreach (char c in text)
                                    {
                                        buffer = BitConverter.GetBytes(c);
                                        data.Add(buffer[0]);
                                    }

                                    break;
                                case "char|clientid|u|orrq":
                                    text = new char[Convert.ToInt32(msg.Items[i].Length as object)];
                                    text = modelProperties[modelPropertyIndex].GetValue(oModel).ToString().ToCharArray();
                                    Array.Resize(ref text, Convert.ToInt32(msg.Items[i].Length as object));
                                    foreach (char c in text)
                                    {
                                        buffer = BitConverter.GetBytes(c);
                                        data.Add(buffer[0]);
                                    }

                                    break;
                                case "char|participantcode|u|orrq":
                                    text = new char[Convert.ToInt32(msg.Items[i].Length as object)];
                                    text = modelProperties[modelPropertyIndex].GetValue(oModel).ToString().ToCharArray();
                                    Array.Resize(ref text, Convert.ToInt32(msg.Items[i].Length as object));
                                    foreach (char c in text)
                                    {
                                        buffer = BitConverter.GetBytes(c);
                                        data.Add(buffer[0]);
                                    }
                                    break;
                                case "char|freetext3|u|orrq":

                                    text = new char[Convert.ToInt32(msg.Items[i].Length as object)];
                                    text = modelProperties[modelPropertyIndex].GetValue(oModel).ToString().ToCharArray();
                                    Array.Resize(ref text, Convert.ToInt32(msg.Items[i].Length as object));
                                    foreach (char c in text)
                                    {
                                        buffer = BitConverter.GetBytes(c);
                                        data.Add(buffer[0]);
                                    }

                                    break;
                                case "char|filler_c|u|orrq":

                                    text = new char[Convert.ToInt32(msg.Items[i].Length as object)];
                                    text = modelProperties[modelPropertyIndex].GetValue(oModel).ToString().ToCharArray();
                                    Array.Resize(ref text, Convert.ToInt32(msg.Items[i].Length as object));
                                    foreach (char c in text)
                                    {
                                        buffer = BitConverter.GetBytes(c);
                                        data.Add(buffer[0]);
                                    }

                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }


            return data.ToArray<byte>();
        }

        #region Commented
        //public static T GetObjectFromBytes<T>(byte[] reply1, MessagesMessage msg, T oModel) where T : class
        //{
        //    try
        //    {


        //        byte[] reply = reply1;

        //        int modelPropertyIndex = 0;

        //        short SlotNum = BitConverter.ToInt16(reply, 0);

        //        int BodyLen = BitConverter.ToInt32(reply, 4);

        //        short msgtype = BitConverter.ToInt16(reply, 8);

        //        PropertyInfo[] modelProperties = null;

        //        if (SlotNum == 0)//UMS
        //        {
        //            modelProperties = MemoryManager._assemblyTypeMetadata[msgtype].Item3;//oModel.GetType().GetProperties();
        //        }
        //        else//reply
        //        {
        //            modelProperties = MemoryManager._assemblyTypeMetadata[msgtype].Item2;//oModel.GetType().GetProperties();
        //        }



        //        for (int i = 0; i < msg.Items.Length; i++)
        //        {
        //            switch (msg.Items[i].Source)
        //            {
        //                case "M":
        //                    modelPropertyIndex = Array.IndexOf(modelProperties, modelProperties.Where(x => x.Name == msg.Items[i].Name).FirstOrDefault());
        //                    if (modelPropertyIndex >= 0)
        //                    {
        //                        switch (msg.Items[i].Type.ToLower())
        //                        {
        //                            case "int8_t":
        //                                char[] Msg = new char[2];
        //                                sbyte Signed8Bit = 0;
        //                                for (int j = 0; j < 2; j++)
        //                                {
        //                                    byte[] tmp = new byte[2];
        //                                    try
        //                                    {
        //                                        tmp[0] = reply[msg.Items[i].Position];
        //                                        tmp[1] = new byte();
        //                                    }
        //                                    catch (Exception ex)
        //                                    {
        //                                        ExceptionUtility.LogError(ex);
        //                                    }
        //                                    Msg[j] = BitConverter.ToChar(tmp, 0);
        //                                    Signed8Bit = Convert.ToSByte(Msg[j]);
        //                                    break;
        //                                }
        //                                oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed8Bit);
        //                                break;

        //                            case "uint8_t":
        //                                byte uSigned8bit = Convert.ToByte(reply[msg.Items[i].Position].ToString());
        //                                oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned8bit);
        //                                break;

        //                            case "int16_t":
        //                                short Signed16bit = BitConverter.ToInt16(reply, msg.Items[i].Position);
        //                                oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed16bit);
        //                                break;

        //                            case "uint16_t":
        //                                ushort uSigned16bit = BitConverter.ToUInt16(reply, msg.Items[i].Position);
        //                                oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned16bit);
        //                                break;

        //                            case "int32_t":
        //                                int Signed32bit = BitConverter.ToInt32(reply, msg.Items[i].Position);
        //                                oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed32bit);
        //                                break;

        //                            case "uint32_t":
        //                                uint uSigned32bit = BitConverter.ToUInt32(reply, msg.Items[i].Position);
        //                                oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned32bit);
        //                                break;

        //                            case "int64_t":
        //                                long Signed64bit = BitConverter.ToInt64(reply, msg.Items[i].Position);
        //                                oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed64bit);
        //                                break;

        //                            case "uint64_t":
        //                                ulong uSigned64bit = BitConverter.ToUInt64(reply, msg.Items[i].Position);
        //                                oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned64bit);
        //                                break;

        //                            case "char":
        //                                int len = Convert.ToInt16(msg.Items[i].Length);
        //                                int position = msg.Items[i].Position;
        //                                int offset = position;

        //                                if (len == 1)
        //                                {
        //                                    if (msg.Items[i].Name.ToLower().Equals("transactiontype")
        //                                        || msg.Items[i].Name.ToLower().Equals("buysell")
        //                                        || msg.Items[i].Name.ToLower().Equals("trend")
        //                                        || msg.Items[i].Name.ToLower().Equals("ordertype")
        //                                        || msg.Items[i].Name.ToLower().Equals("orderactioncode"))
        //                                    {
        //                                        //lstHeader.Add(item.name, GetStringFromByte(replyData[offset]));
        //                                        oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, GetStringFromByte(reply[offset]));
        //                                    }
        //                                    else if (msg.Items[i].Name.ToLower().Equals("year"))
        //                                    {
        //                                        int year = Convert.ToInt32(reply[offset]) + 1900;
        //                                        //lstHeader.Add(item.name, year.ToString());
        //                                        oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, year.ToString());
        //                                    }
        //                                    else
        //                                    {
        //                                        //lstHeader.Add(item.name, replyData[offset].ToString());
        //                                        oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, reply[offset].ToString());
        //                                    }
        //                                    offset += 1;
        //                                }
        //                                else
        //                                {

        //                                    char[] chrMsg = new char[msg.Items[i].Length];
        //                                    for (int j = 0; j < msg.Items[i].Length; j++)
        //                                    {
        //                                        byte[] tmp = new byte[2];

        //                                        tmp[0] = reply[offset];
        //                                        tmp[1] = new byte();

        //                                        chrMsg[j] = BitConverter.ToChar(tmp, 0);
        //                                        offset += 1;
        //                                    }
        //                                    string charData = GetStringFromCharArray(chrMsg);
        //                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, charData);
        //                                }
        //                                break;

        //                            case "maxchar":
        //                                int positionNew = msg.Items[i].Position;
        //                                int offsetNew = positionNew;
        //                                int length = reply.Length - positionNew;
        //                                char[] chrMsgNew = new char[reply.Length];
        //                                for (int j = 0; j < length; j++)
        //                                {
        //                                    byte[] tmp = new byte[2];

        //                                    tmp[0] = reply[offsetNew];
        //                                    tmp[1] = new byte();

        //                                    chrMsgNew[j] = BitConverter.ToChar(tmp, 0);
        //                                    offsetNew += 1;
        //                                }
        //                                string charDataNew = GetStringFromCharArray(chrMsgNew);
        //                                oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, charDataNew);
        //                                break;
        //                        }
        //                    }

        //                    break;

        //                case "D":
        //                    string defaultValue = msg.Items[i].Value;
        //                    switch (msg.Items[i].Type.ToLower())//for 
        //                    {
        //                        case "int8_t":
        //                            sbyte Signed8Bit = Convert.ToSByte(defaultValue);
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed8Bit);
        //                            break;

        //                        case "uint8_t":
        //                            byte uSigned8bit = Convert.ToByte(defaultValue);
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned8bit);
        //                            break;

        //                        case "int16_t":
        //                            short Signed16bit = Convert.ToInt16(defaultValue);
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed16bit);
        //                            break;

        //                        case "uint16_t":
        //                            ushort uSigned16bit = Convert.ToUInt16(defaultValue);
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned16bit);
        //                            break;

        //                        case "int32_t":
        //                            int Signed32bit = Convert.ToInt32(defaultValue);
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed32bit);
        //                            break;

        //                        case "uint32_t":
        //                            uint uSigned32bit = Convert.ToUInt32(defaultValue);
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned32bit);
        //                            break;

        //                        case "int64_t":
        //                            long Signed64bit = Convert.ToInt64(defaultValue);
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed64bit);
        //                            break;

        //                        case "uint64_t":
        //                            ulong uSigned64bit = Convert.ToUInt64(defaultValue);
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned64bit);
        //                            break;

        //                        case "char":
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, defaultValue);
        //                            break;
        //                    }
        //                    break;
        //                case "N":
        //                    switch (msg.Items[i].Type.ToLower())//for 
        //                    {
        //                        case "int8_t":
        //                            sbyte signed8bit = Convert.ToSByte(Convert.ToInt16(Enumerations.SignedInt.OneByte));
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, signed8bit);
        //                            break;

        //                        case "uint8_t":
        //                            byte uSigned8bit = Convert.ToByte(Convert.ToInt16(Enumerations.USignedInt.OneByte));
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned8bit);
        //                            break;

        //                        case "int16_t":
        //                            short Signed16bit = Convert.ToInt16(Enumerations.SignedInt.TwoByte);
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed16bit);
        //                            break;

        //                        case "uint16_t":
        //                            ushort uSigned16bit = Convert.ToUInt16(Enumerations.USignedInt.TwoByte);
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned16bit);
        //                            break;

        //                        case "int32_t":
        //                            int Signed32bit = Convert.ToInt32(Enumerations.SignedInt.FourByte);
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed32bit);
        //                            break;

        //                        case "uint32_t":
        //                            uint uSigned32bit = Convert.ToUInt32(Enumerations.USignedInt.FourByte);
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned32bit);
        //                            break;

        //                        case "int64_t":
        //                            long Signed64bit = Convert.ToInt64(Enumerations.SignedInt.EightByte);
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed64bit);
        //                            break;

        //                        case "uint64_t":
        //                            ulong uSigned64bit = Convert.ToUInt64(Enumerations.USignedInt.EightByte);
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned64bit);
        //                            break;

        //                        case "char":
        //                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Enumerations.Char.OneByte);
        //                            break;

        //                    }
        //                    break;

        //                default:
        //                    break;
        //            }

        //        }



        //        return oModel;

        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionUtility.LogError(ex);
        //        throw ex;
        //    }
        //}
        #endregion
        public static T GetObjectFromBytes<T>(byte[] reply1, MessagesMessage msg, T oModel, string repeatType) where T : class
        {
            try
            {
                int requestQPosition = 0;
                int replyQPosition = 0;
                int umsQPosition = 0;

                byte[] reply = reply1;

                int modelPropertyIndex = 0;

                int SlotNum = BitConverter.ToInt32(reply, 0);

                uint BodyLen = BitConverter.ToUInt32(reply, 4);

                uint msgtype = BitConverter.ToUInt32(reply, 8);

                PropertyInfo[] modelProperties = null;

                if (repeatType == null)
                {
                    if (SlotNum == 0)//UMS
                    {
                        modelProperties = MemoryManager._assemblyTypeMetadata[msgtype].Item3;//oModel.GetType().GetProperties();
                    }
                    else//reply
                    {
                        modelProperties = MemoryManager._assemblyTypeMetadata[msgtype].Item2;//oModel.GetType().GetProperties();
                    }
                }

                else
                {
                    if (repeatType == "REQUESTQ")
                    {
                        var reqTypeCollection = SharedMemories.MemoryManager._assemblyTypeMetadata[msgtype].Item4;
                        if (reqTypeCollection != null && reqTypeCollection.Count > 0)
                        {
                            modelProperties = reqTypeCollection[msg.Name];
                        }
                    }
                    else if (repeatType == "REPLYQ")
                    {
                        var repTypeCollection = SharedMemories.MemoryManager._assemblyTypeMetadata[msgtype].Item5;
                        if (repTypeCollection != null && repTypeCollection.Count > 0)
                        {

                            modelProperties = repTypeCollection[msg.Name];
                        }
                    }
                    else if (repeatType == "UMSQ")
                    {
                        var umsTypeCollection = SharedMemories.MemoryManager._assemblyTypeMetadata[msgtype].Item6;
                        if (umsTypeCollection != null && umsTypeCollection.Count > 0)
                        {

                            modelProperties = umsTypeCollection[msg.Name];
                        }
                    }

                }

                for (int i = 0; i < msg.Items.Length; i++)
                {
                    switch (msg.Items[i].Source)
                    {
                        case "M":
                            modelPropertyIndex = Array.IndexOf(modelProperties, modelProperties.Where(x => x.Name == msg.Items[i].Name).FirstOrDefault());
                            if (modelPropertyIndex >= 0)
                            {
                                switch (msg.Items[i].Type.ToLower())
                                {
                                    case "int8_t":
                                        char[] Msg = new char[2];
                                        sbyte Signed8Bit = 0;
                                        for (int j = 0; j < 2; j++)
                                        {
                                            byte[] tmp = new byte[2];
                                            try
                                            {
                                                tmp[0] = reply[msg.Items[i].Position];
                                                tmp[1] = new byte();
                                            }
                                            catch (Exception ex)
                                            {
                                                ExceptionUtility.LogError(ex);
                                            }
                                            Msg[j] = BitConverter.ToChar(tmp, 0);
                                            Signed8Bit = Convert.ToSByte(Msg[j]);
                                            break;
                                        }
                                        oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed8Bit);
                                        break;

                                    case "uint8_t":
                                        byte uSigned8bit = Convert.ToByte(reply[msg.Items[i].Position].ToString());
                                        oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned8bit);
                                        break;

                                    case "int16_t":
                                        short Signed16bit = BitConverter.ToInt16(reply, msg.Items[i].Position);
                                        oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed16bit);
                                        break;

                                    case "uint16_t":
                                        ushort uSigned16bit = BitConverter.ToUInt16(reply, msg.Items[i].Position);
                                        oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned16bit);
                                        break;

                                    case "int32_t":
                                        int Signed32bit = BitConverter.ToInt32(reply, msg.Items[i].Position);
                                        oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed32bit);
                                        break;

                                    case "uint32_t":
                                        uint uSigned32bit = BitConverter.ToUInt32(reply, msg.Items[i].Position);
                                        oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned32bit);
                                        break;

                                    case "int64_t":
                                        long Signed64bit = BitConverter.ToInt64(reply, msg.Items[i].Position);
                                        oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed64bit);
                                        break;

                                    case "uint64_t":
                                        ulong uSigned64bit = BitConverter.ToUInt64(reply, msg.Items[i].Position);
                                        oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned64bit);
                                        break;

                                    case "char":
                                        int len = Convert.ToInt16(msg.Items[i].Length);
                                        int position = msg.Items[i].Position;
                                        int offset = position;

                                        if (len == 1)
                                        {
                                            if (msg.Items[i].Name.ToLower().Equals("transactiontype")
                                                || msg.Items[i].Name.ToLower().Equals("buyorsell")
                                                || msg.Items[i].Name.ToLower().Equals("trend")
                                                || msg.Items[i].Name.ToLower().Equals("ordertype")
                                                || msg.Items[i].Name.ToLower().Equals("orderactioncode")
                                                || msg.Items[i].Name.ToLower().Equals("audcode")
                                                || msg.Items[i].Name.ToLower().Equals("type_c")
                                                || msg.Items[i].Name.ToLower().Equals("type"))
                                            {
                                                //lstHeader.Add(item.name, GetStringFromByte(replyData[offset]));
                                                oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, GetStringFromByte(reply[offset]));
                                            }
                                            else if (msg.Items[i].Name.ToLower().Equals("year"))
                                            {
                                                int year = Convert.ToInt32(reply[offset]) + 1900;
                                                //lstHeader.Add(item.name, year.ToString());
                                                oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, year.ToString());
                                            }
                                            else
                                            {
                                                //lstHeader.Add(item.name, replyData[offset].ToString());
                                                oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, reply[offset].ToString());
                                            }
                                            offset += 1;
                                        }
                                        else
                                        {

                                            char[] chrMsg = new char[msg.Items[i].Length];
                                            for (int j = 0; j < msg.Items[i].Length; j++)
                                            {
                                                byte[] tmp = new byte[2];

                                                tmp[0] = reply[offset];
                                                tmp[1] = new byte();

                                                chrMsg[j] = BitConverter.ToChar(tmp, 0);
                                                offset += 1;
                                            }
                                            string charData = GetStringFromCharArray(chrMsg);
                                            oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, charData);
                                        }
                                        break;

                                    case "maxchar":
                                        int positionNew = msg.Items[i].Position;
                                        int offsetNew = positionNew;
                                        int length = reply.Length - positionNew;
                                        char[] chrMsgNew = new char[reply.Length];
                                        for (int j = 0; j < length; j++)
                                        {
                                            byte[] tmp = new byte[2];

                                            tmp[0] = reply[offsetNew];
                                            tmp[1] = new byte();

                                            chrMsgNew[j] = BitConverter.ToChar(tmp, 0);
                                            offsetNew += 1;
                                        }
                                        string charDataNew = GetStringFromCharArray(chrMsgNew);
                                        oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, charDataNew);
                                        break;
                                }
                            }

                            break;

                        case "D":
                            string defaultValue = msg.Items[i].Value;
                            switch (msg.Items[i].Type.ToLower())//for 
                            {
                                case "int8_t":
                                    sbyte Signed8Bit = Convert.ToSByte(defaultValue);
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed8Bit);
                                    break;

                                case "uint8_t":
                                    byte uSigned8bit = Convert.ToByte(defaultValue);
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned8bit);
                                    break;

                                case "int16_t":
                                    short Signed16bit = Convert.ToInt16(defaultValue);
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed16bit);
                                    break;

                                case "uint16_t":
                                    ushort uSigned16bit = Convert.ToUInt16(defaultValue);
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned16bit);
                                    break;

                                case "int32_t":
                                    int Signed32bit = Convert.ToInt32(defaultValue);
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed32bit);
                                    break;

                                case "uint32_t":
                                    uint uSigned32bit = Convert.ToUInt32(defaultValue);
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned32bit);
                                    break;

                                case "int64_t":
                                    long Signed64bit = Convert.ToInt64(defaultValue);
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed64bit);
                                    break;

                                case "uint64_t":
                                    ulong uSigned64bit = Convert.ToUInt64(defaultValue);
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned64bit);
                                    break;

                                case "char":
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, defaultValue);
                                    break;
                            }
                            break;
                        case "N":
                            switch (msg.Items[i].Type.ToLower())//for 
                            {
                                case "int8_t":
                                    sbyte signed8bit = Convert.ToSByte(Convert.ToInt16(Enumerations.SignedInt.OneByte));
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, signed8bit);
                                    break;

                                case "uint8_t":
                                    byte uSigned8bit = Convert.ToByte(Convert.ToInt16(Enumerations.USignedInt.OneByte));
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned8bit);
                                    break;

                                case "int16_t":
                                    short Signed16bit = Convert.ToInt16(Enumerations.SignedInt.TwoByte);
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed16bit);
                                    break;

                                case "uint16_t":
                                    ushort uSigned16bit = Convert.ToUInt16(Enumerations.USignedInt.TwoByte);
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned16bit);
                                    break;

                                case "int32_t":
                                    int Signed32bit = Convert.ToInt32(Enumerations.SignedInt.FourByte);
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed32bit);
                                    break;

                                case "uint32_t":
                                    uint uSigned32bit = Convert.ToUInt32(Enumerations.USignedInt.FourByte);
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned32bit);
                                    break;

                                case "int64_t":
                                    long Signed64bit = Convert.ToInt64(Enumerations.SignedInt.EightByte);
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Signed64bit);
                                    break;

                                case "uint64_t":
                                    ulong uSigned64bit = Convert.ToUInt64(Enumerations.USignedInt.EightByte);
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, uSigned64bit);
                                    break;

                                case "char":
                                    oModel.GetType().GetProperty(msg.Items[i].Name).SetValue(oModel, Enumerations.Char.OneByte);
                                    break;

                            }
                            break;
                        case "REQUESTQ":
                            switch (msg.Items[i].Type.ToLower())//for 
                            {

                                default:
                                    var DependencyPropertyName = msg.Items[i].DependencyProperty;
                                    var length = Convert.ToInt32(oModel.GetType().GetProperty(DependencyPropertyName).GetValue(oModel));
                                    if (length < 0)
                                        break;
                                    var position = (msg.Items[i]).Position;//Convert.ToInt32(oModel.GetType().GetProperty("Position").GetValue(oModel));
                                    var modelName = msg.Items[i].Type;//AdvancedTWS.SharedMemories.ConfigurationMasterMemory.RepeatDict[msgtype].Name;

                                    Type type = SharedMemories.MemoryManager._assemblyConDict[modelName];
                                    object otype = Activator.CreateInstance(type);

                                    for (int index = 0; index < length; index++)
                                    {
                                        otype = GetObjectFromBytes(reply1, ConfigurationMasterMemory.RepeatDict[msgtype], otype, "REQUESTQ");
                                    }


                                    break;
                            }
                            break;
                        case "REPLYQ":
                            switch (msg.Items[i].Type.ToLower())//for 
                            {

                                default:
                                    var DependencyPropertyName = msg.Items[i].DependencyProperty;
                                    var length = Convert.ToInt32(oModel.GetType().GetProperty(DependencyPropertyName).GetValue(oModel));
                                    if (length < 0)
                                        break;

                                    var position = (msg.Items[i]).Position;//Convert.ToInt32(oModel.GetType().GetProperty("Position").GetValue(oModel));
                                    var modelName = msg.Items[i].Type;//AdvancedTWS.SharedMemories.ConfigurationMasterMemory.RepeatDict[msgtype].Name;
                                    var mappedProperty = msg.Items[i].MappedProperty;


                                    Type type = SharedMemories.MemoryManager._assemblyConDict[modelName];

                                    replyQPosition = position;

                                    //var listo = Activator.CreateInstance(List<Model.Broadcast.MarketPictureReplyGrp>);
                                    List<object> oTempList = new List<object>(length);

                                    int localReplyCount = ConfigurationMasterMemory.RepeatDict[msgtype].Items.Count();

                                    for (int index = 0; index < length; index++)
                                    {
                                        for (int localReplyIndex = 0; localReplyIndex < localReplyCount; localReplyIndex++)
                                        {
                                            int localReplyLength = replyQPosition;
                                            ConfigurationMasterMemory.RepeatDict[msgtype].Items[localReplyIndex].Position = localReplyLength;//Convert.ToByte(localReplyLength);
                                            replyQPosition = localReplyLength + ConfigurationMasterMemory.RepeatDict[msgtype].Items[localReplyIndex].Length;
                                        }
                                        object otype = Activator.CreateInstance(type);
                                        otype = GetObjectFromBytes(reply1, ConfigurationMasterMemory.RepeatDict[msgtype], otype, "REPLYQ");
                                        oTempList.Add(otype);
                                    }
                                    oModel.GetType().GetProperty(mappedProperty).SetValue(oModel, oTempList);

                                    break;
                            }
                            break;
                        case "UMSQ":
                            #region Commented


                            //switch (msg.Items[i].Type.ToLower())//for 
                            //{

                            //    default:
                            //        var DependencyPropertyName = msg.Items[i].DependencyProperty;
                            //        var length = Convert.ToInt32(oModel.GetType().GetProperty(DependencyPropertyName).GetValue(oModel));
                            //        var position = (msg.Items[i]).Position;//Convert.ToInt32(oModel.GetType().GetProperty("Position").GetValue(oModel));
                            //        var modelName = msg.Items[i].Type;//AdvancedTWS.SharedMemories.ConfigurationMasterMemory.RepeatDict[msgtype].Name;

                            //        Type type = SharedMemories.MemoryManager._assemblyConDict[modelName];
                            //        object otype = Activator.CreateInstance(type);

                            //        for (int index = 0; index < length; index++)
                            //        {
                            //            otype = GetObjectFromBytes(reply1, ConfigurationMasterMemory.RepeatDict[msgtype], otype, "UMSQ");
                            //        }


                            //        break;
                            //}
                            //break;
                            #endregion
                            switch (msg.Items[i].Type.ToLower())//for 
                            {

                                default:
                                    var DependencyPropertyName = msg.Items[i].DependencyProperty;
                                    var length = Convert.ToInt32(oModel.GetType().GetProperty(DependencyPropertyName).GetValue(oModel));
                                    if (length < 0)
                                        break;
                                    var position = (msg.Items[i]).Position;//Convert.ToInt32(oModel.GetType().GetProperty("Position").GetValue(oModel));
                                    var modelName = msg.Items[i].Type;//AdvancedTWS.SharedMemories.ConfigurationMasterMemory.RepeatDict[msgtype].Name;
                                    var mappedProperty = msg.Items[i].MappedProperty;

                                    Type type = SharedMemories.MemoryManager._assemblyConDict[modelName];

                                    umsQPosition = position;

                                    IList<object> oTempList = new List<object>(length);

                                    int localUMSCount = SharedMemories.ConfigurationMasterMemory.RepeatDict[msgtype].Items.Count();

                                    for (int index = 0; index < length; index++)
                                    {
                                        for (int localUMSIndex = 0; localUMSIndex < localUMSCount; localUMSIndex++)
                                        {
                                            int localUMSLength = umsQPosition;
                                            SharedMemories.ConfigurationMasterMemory.RepeatDict[msgtype].Items[localUMSIndex].Position = localUMSLength;//Convert.ToByte(localReplyLength);
                                            umsQPosition = localUMSLength + SharedMemories.ConfigurationMasterMemory.RepeatDict[msgtype].Items[localUMSIndex].Length;
                                        }
                                        object otype = Activator.CreateInstance(type);
                                        otype = GetObjectFromBytes(reply1, SharedMemories.ConfigurationMasterMemory.RepeatDict[msgtype], otype, "UMSQ");
                                        oTempList.Add(otype);
                                    }
                                    oModel.GetType().GetProperty(mappedProperty).SetValue(oModel, oTempList);
                                    break;
                            }
                            break;
                        default:
                            break;
                    }

                }



                return oModel;

            }
            catch (Exception ex)
            {
                ExceptionUtility.LogError(ex);
                throw ex;
            }
        }
        private static string GetStringFromCharArray(char[] cArray)
        {
            string temp = new string(cArray);
            if (temp.Contains("\0"))
            {
                temp = temp.Remove(temp.IndexOf("\0"));
                temp = temp.Replace("\0", string.Empty);
            }
            temp.Trim();
            return temp;
        }
        private static string GetStringFromByte(byte data)
        {
            List<byte> lst = new List<byte>();
            lst.Add(data);
            lst.Add(new byte());
            char cc;
            cc = BitConverter.ToChar(lst.ToArray<byte>(), 0);
            return cc.ToString();
        }
        #endregion

        #region Temporary Code to Check Performance
        private static sbyte GetSByte(byte[] data, int offset)
        {
            char[] Msg = new char[2];
            sbyte Signed8Bit = 0;
            for (int j = 0; j < 2; j++)
            {
                byte[] tmp = new byte[2];
                try
                {
                    tmp[0] = data[offset];
                    tmp[1] = new byte();
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogError(ex);
                }
                Msg[j] = BitConverter.ToChar(tmp, 0);
                Signed8Bit = Convert.ToSByte(Msg[j]);
                break;
            }
            return Signed8Bit;
        }

        private static string GetString(byte[] data, int length, int offset)
        {
            char[] chrMsg = new char[length];
            for (int j = 0; j < length; j++)
            {
                byte[] tmp = new byte[2];

                tmp[0] = data[offset];
                tmp[1] = new byte();

                chrMsg[j] = BitConverter.ToChar(tmp, 0);
                offset += 1;
            }
            string charData = GetStringFromCharArray(chrMsg);

            return charData;
        }

        #endregion


        #region ETI Date Conversion
        /// <summary>
        /// Returns UTC time from ETI UTC Timestamp(nanoseconds)
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            unixTimeStamp = unixTimeStamp / 1000000;

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var timeSpan = TimeSpan.FromMilliseconds(Convert.ToDouble(unixTimeStamp));
            var localDateTime = epoch.Add(timeSpan);
            return localDateTime;
        }
        #endregion

        #region Serialization
        public static string GetXMLFromObject(object o)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            var Name = "";
            try
            {
                Name = o.GetType().Name;
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, o);
            }
            catch (Exception ex)
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }
            DirectoryInfo LogPath = new DirectoryInfo(
    Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"xmlFiles/")));
            if (!LogPath.Exists)
            {
                LogPath.Create();
            }

            using (StreamWriter writer = new StreamWriter(LogPath.ToString() + Name + System.DateTime.Now.Day + System.DateTime.Now.Month + System.DateTime.Now.Year + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second + System.DateTime.Now.Millisecond + ".xml", true))
            {
                writer.WriteLine(sw.ToString());
                writer.Close();
            }
            return sw.ToString();
        }

        public static Object ObjectToXML(string xml, Type objectType)
        {
            StringReader strReader = null;
            XmlSerializer serializer = null;
            XmlTextReader xmlReader = null;
            Object obj = null;
            try
            {
                strReader = new StringReader(xml);
                serializer = new XmlSerializer(objectType);
                xmlReader = new XmlTextReader(strReader);
                obj = serializer.Deserialize(xmlReader);
            }
            catch (Exception exp)
            {
                //Handle Exception Code
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
                if (strReader != null)
                {
                    strReader.Close();
                }
            }
            return obj;
        }
        #endregion
    }
#endif
}

#region Commented


//foreach (PropertyInfo oProperty in oModel.GetType().GetProperties())
//{

//    foreach (CommonFrontEnd.ControllerModel.SenderControllerModel.MessagesMessageItems item in msg.Items)
//    {
//        string propName = oProperty.Name;
//        string source = item.Source;


//        if (Convert.ToString(propName).Equals(item.name))
//        {
//            switch (source)
//            {
//                case "M":

//                    item.Value = Convert.ToString(oProperty.GetValue(oModel));
//                    switch (item.Type.ToLower())
//                    {
//                        case "int8_t":
//                        case "int16_t":
//                            buffer = BitConverter.GetBytes(Convert.ToInt16(oProperty.GetValue(oModel)));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "uint8_t":
//                        case "uint16_t":
//                            buffer = BitConverter.GetBytes(Convert.ToUInt16(oProperty.GetValue(oModel)));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "int32_t":
//                            buffer = BitConverter.GetBytes( (oProperty.GetValue(oModel)));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "uint32_t":
//                            buffer = BitConverter.GetBytes(Convert.ToUInt32(oProperty.GetValue(oModel)));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "int64_t":
//                            buffer = BitConverter.GetBytes(Convert.ToInt64(oProperty.GetValue(oModel)));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "uint64_t":
//                            buffer = BitConverter.GetBytes(Convert.ToUInt64(oProperty.GetValue(oModel)));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "char":
//                            if (item.name.ToLower().Equals("hour") || item.name.ToLower().Equals("minute") ||
//                                item.name.ToLower().Equals("second"))
//                            {
//                                data.Add(BitConverter.GetBytes( (oProperty.GetValue(oModel)))[0]);
//                            }
//                            else
//                            {
//                                char[] text = new char[ (item.length as object)];
//                                text = oProperty.GetValue(oModel).ToString().ToCharArray();
//                                Array.Resize(ref text,  (item.length as object));
//                                foreach (char c in text)
//                                {
//                                    buffer = BitConverter.GetBytes(c);
//                                    data.Add(buffer[0]);
//                                }
//                            }
//                            break;
//                    }

//                    break;
//                case "D":
//                    string defaultValue = item.Value;
//                    switch (item.Type.ToLower())//for 
//                    {
//                        case "int8_t":
//                        case "int16_t":
//                            buffer = BitConverter.GetBytes(Convert.ToInt16(defaultValue));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "uint8_t":
//                        case "uint16_t":
//                            buffer = BitConverter.GetBytes(Convert.ToUInt16(defaultValue));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "int32_t":
//                            buffer = BitConverter.GetBytes( (defaultValue));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "uint32_t":
//                            buffer = BitConverter.GetBytes(Convert.ToUInt32(defaultValue));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "int64_t":
//                            buffer = BitConverter.GetBytes(Convert.ToInt64(defaultValue));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "uint64_t":
//                            buffer = BitConverter.GetBytes(Convert.ToUInt64(defaultValue));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "char":

//                            if (item.name.ToLower().Equals("hour") || item.name.ToLower().Equals("minute") ||
//                                item.name.ToLower().Equals("second"))
//                            {
//                                data.Add(BitConverter.GetBytes( (defaultValue))[0]);
//                            }
//                            else
//                            {
//                                char[] text = new char[ (item.length as object)];
//                                text = defaultValue.ToCharArray();
//                                Array.Resize(ref text,  (item.length as object));
//                                foreach (char c in text)
//                                {
//                                    buffer = BitConverter.GetBytes(c);
//                                    data.Add(buffer[0]);
//                                }
//                            }
//                            break;
//                    }
//                    break;
//                case "N":
//                    switch (item.Type.ToLower())//for 
//                    {
//                        case "int8_t":
//                        case "int16_t":

//                            buffer = BitConverter.GetBytes(Convert.ToInt16(SignedInt.TwoByte));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "uint8_t":
//                        case "uint16_t":
//                            buffer = BitConverter.GetBytes(Convert.ToUInt16(USignedInt.TwoByte));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "int32_t":
//                            buffer = BitConverter.GetBytes( (SignedInt.FourByte));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "uint32_t":
//                            buffer = BitConverter.GetBytes(Convert.ToUInt32(USignedInt.FourByte));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "int64_t":
//                            buffer = BitConverter.GetBytes(Convert.ToInt64(SignedInt.EightByte));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "uint64_t":
//                            buffer = BitConverter.GetBytes(Convert.ToUInt64(USignedInt.EightByte));
//                            data.AddRange(buffer.ToList<byte>());
//                            break;

//                        case "char":

//                            if (item.name.ToLower().Equals("hour") || item.name.ToLower().Equals("minute") ||
//                                item.name.ToLower().Equals("second"))
//                            {
//                                data.Add(BitConverter.GetBytes( (FixedString.FirstPosition))[0]);
//                            }
//                            else
//                            {
//                                char[] text = new char[ (item.length as object)];
//                                text = (FixedString.FirstPosition).ToString().ToCharArray();
//                                Array.Resize(ref text,  (item.length as object));
//                                foreach (char c in text)
//                                {
//                                    buffer = BitConverter.GetBytes(c);
//                                    data.Add(buffer[0]);
//                                }
//                            }
//                            break;
//                    }
//                    break;
//                default:
//                    break;
//            }
//            break;
//        }

//    }
//}
#endregion
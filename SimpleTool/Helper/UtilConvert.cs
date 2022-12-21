using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleSqlSugar
{
    /// <summary>
    /// 类型转换
    /// </summary>
    public static class UtilConvert
    {


        #region 基本数据类型转换

        public static string IntToString(int i)
        {
            return i.ToString();
        }

        /// <summary>
        /// object数组转Int数组
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static List<int> ObjListToIntList(this object[] ids)
        {
            List<int> list = new List<int>();
            foreach (var id in ids)
            {
                list.Add(id.ObjToInt());
            }
            return list;
        }


        /// <summary>
        /// 字符串转指定类型数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static T[] SplitToArray<T>(string value, char split)
        {
            T[] arr = value.Split(new string[] { split.ToString() }, StringSplitOptions.RemoveEmptyEntries).CastSuper<T>().ToArray();
            return arr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static int ObjToInt(this object thisValue)
        {
            int reval = 0;
            if (thisValue == null) return 0;
            if (thisValue != null && thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return reval;
        }

        public static string[] StringToArray(this string thisValue)
        {
            return thisValue.Split(",");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static int ObjToInt(this object thisValue, int errorValue)
        {
            if (thisValue != null && thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out int reval))
            {
                return reval;
            }
            return errorValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static double ObjToMoney(this object thisValue)
        {
            if (thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out double reval))
            {
                return reval;
            }
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static double ObjToMoney(this object thisValue, double errorValue)
        {
            if (thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out double reval))
            {
                return reval;
            }
            return errorValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static string ObjToString(this object thisValue)
        {
            if (thisValue != null) return thisValue.ToString().Trim();
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static string ObjToString(this object thisValue, string errorValue)
        {
            if (thisValue != null) return thisValue.ToString().Trim();
            return errorValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static decimal ObjToDecimal(this object thisValue)
        {
            if (thisValue != null && thisValue != DBNull.Value && decimal.TryParse(thisValue.ToString(), out decimal reval))
            {
                return reval;
            }
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static decimal ObjToDecimal(this object thisValue, decimal errorValue)
        {
            if (thisValue != null && thisValue != DBNull.Value && decimal.TryParse(thisValue.ToString(), out decimal reval))
            {
                return reval;
            }
            return errorValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static DateTime ObjToDate(this object thisValue)
        {
            DateTime reval = DateTime.MinValue;
            if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out reval))
            {
                reval = Convert.ToDateTime(thisValue);
            }
            return reval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static DateTime ObjToDate(this object thisValue, DateTime errorValue)
        {
            if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out DateTime reval))
            {
                return reval;
            }
            return errorValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static bool ObjToBool(this object thisValue)
        {
            bool reval = false;
            if (thisValue != null && thisValue != DBNull.Value && bool.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return reval;
        }

        #endregion

        #region 强制转换类型
        /// <summary>
        /// 强制转换类型
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> CastSuper<TResult>(this IEnumerable source)
        {
            foreach (object item in source)
            {
                yield return (TResult)Convert.ChangeType(item, typeof(TResult));
            }
        }
        #endregion

        #region 进制

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] strToToHexByte(this string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// 将一个byte数组转换成16进制字符串
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>格式化的16进制字符串</returns>
        public static string ByteArrayToHexString(this byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            }
            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// 十六进制转换到十进制
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static int Hex2Ten(this string hex)
        {
            int ten = 0;
            for (int i = 0, j = hex.Length - 1; i < hex.Length; i++)
            {
                ten += HexChar2Value(hex.Substring(i, 1)) * ((int)Math.Pow(16, j));
                j--;
            }
            return ten;
        }

        public static int HexChar2Value(string hexChar)
        {
            switch (hexChar)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    return Convert.ToInt32(hexChar);
                case "a":
                case "A":
                    return 10;
                case "b":
                case "B":
                    return 11;
                case "c":
                case "C":
                    return 12;
                case "d":
                case "D":
                    return 13;
                case "e":
                case "E":
                    return 14;
                case "f":
                case "F":
                    return 15;
                default:
                    return 0;
            }
        }
        #endregion

        #region 流
        /// <summary> 
        /// 将 Stream 转成 byte[] 
        /// </summary> 
        public static byte[] StreamToBytes(Stream stream)
        {
            //设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Close();
            return bytes;
        }
        #endregion

        #region 时间

        public static int ConvertDateTimeToInt(this DateTime time)
        {
#pragma warning disable CS0618 // 类型或成员已过时
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
#pragma warning restore CS0618 // 类型或成员已过时
            return (int)(time - startTime).TotalSeconds; // 相差秒数

        }


        #endregion


        #region 对字符串进行UrlEncode/UrlDecode

        /// <summary>
        /// 对字符进行UrlEncode编码
        /// string转Encoding格式
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encod">编码格式</param>
        /// <param name="cap">是否输出大写字母</param>
        /// <returns></returns>
        public static string UrlEncode(string text, Encoding encod, bool cap = true)
        {
            if (cap)
            {
                StringBuilder builder = new StringBuilder();
                foreach (char c in text)
                {
                    if (System.Web.HttpUtility.UrlEncode(c.ToString(), encod).Length > 1)
                    {
                        builder.Append(System.Web.HttpUtility.UrlEncode(c.ToString(), encod).ToUpper());
                    }
                    else
                    {
                        builder.Append(c);
                    }
                }
                return builder.ToString();
            }
            else
            {
                string encodString = System.Web.HttpUtility.UrlEncode(text, encod);
                return encodString;
            }
        }

        /// <summary>
        /// 对字符进行UrlDecode解码
        /// Encoding转string格式
        /// </summary>
        /// <param name="encodString"></param>
        /// <param name="encod">编码格式</param>
        /// <returns></returns>
        public static string UrlDecode(string encodString, Encoding encod)
        {
            string text = System.Web.HttpUtility.UrlDecode(encodString, encod);
            return text;
        }
        #endregion
    }
}
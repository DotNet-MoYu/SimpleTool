using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SimpleSqlSugar
{
    /// <summary>
    /// 加密解密
    /// </summary>
    public static class EncryptHelper
    {
        #region DES加密解密
        private const string desKey = "2wsxZSE$";
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DesEncrypt(string value, string key = desKey)
        {

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();//实例化加密类对象
            byte[] arr_key = Encoding.UTF8.GetBytes(key.Substring(0, 8));//定义字节数组用来存放密钥 GetBytes方法用于将字符串中所有字节编码为一个字节序列
                                                                         ////!!!DES加密key位数只能是64位，即8字节
                                                                         ////注意这里用的编码用当前默认编码方式，不确定就用Default
            byte[] arr_str = Encoding.UTF8.GetBytes(value);//定义字节数组存放要加密的字符串
            MemoryStream ms = new MemoryStream();//实例化内存流对象
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(arr_key, arr_key), CryptoStreamMode.Write);//创建加密流对象，参数 内存流/初始化向量IV/加密流模式
            cs.Write(arr_str, 0, arr_str.Length);//需加密字节数组/offset/length，此方法将length个字节从 arr_str 复制到当前流。0是偏移量offset，即从指定index开始复制。
            cs.Close();
            string str_des = Convert.ToBase64String(ms.ToArray());
            return str_des;
        }


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DesDecrypt(this string value, string key = desKey)
        {

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();//实例化加密类对象

            byte[] arr_key = Encoding.UTF8.GetBytes(key.Substring(0, 8));//定义字节数组用来存放密钥 GetBytes方法用于将字符串中所有字节编码为一个字节序列
                                                                         ////!!!DES加密key位数只能是64位，即8字节
                                                                         ////注意这里用的编码用当前默认编码方式，不确定就用Default
                                                                         //解密
            var ms = new MemoryStream();
            byte[] arr_des = Convert.FromBase64String(value);//注意这里仍要将密文作为base64字符串处理获得数组，否则报错
                                                             //byte[] arr_des = Encoding.UTF8.GetBytes(str_des);//不可行，将加密方法中ms的字符数组转为utf-8也不行
            des = new DESCryptoServiceProvider();//解密方法定义加密对象
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(arr_key, arr_key), CryptoStreamMode.Write);//创建加密流对象，参数 内存流/初始化向量IV/加密流模式

            cs = new CryptoStream(ms, des.CreateDecryptor(arr_key, arr_key), CryptoStreamMode.Write);
            cs.Write(arr_des, 0, arr_des.Length);
            cs.FlushFinalBlock();

            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray());//此处与加密前编码一致

        }

        #endregion
        #region Base64加密解密

        /// <summary>
        /// Base64是一種使用64基的位置計數法。它使用2的最大次方來代表僅可列印的ASCII 字元。
        /// 這使它可用來作為電子郵件的傳輸編碼。在Base64中的變數使用字元A-Z、a-z和0-9 ，
        /// 這樣共有62個字元，用來作為開始的64個數字，最後兩個用來作為數字的符號在不同的
        /// 系統中而不同。
        /// Base64加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Encrypt(this string str)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Decrypt(this string str)
        {
            byte[] decbuff = Convert.FromBase64String(str);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }
        #endregion
        #region SHA256加密解密

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string SHA256EncryptString(this string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="StrIn">待加密字符串</param>
        /// <returns>加密数组</returns>
        public static Byte[] SHA256EncryptByte(this string StrIn)
        {
            var sha256 = new SHA256Managed();
            var Asc = new ASCIIEncoding();
            var tmpByte = Asc.GetBytes(StrIn);
            var EncryptBytes = sha256.ComputeHash(tmpByte);
            sha256.Clear();
            return EncryptBytes;
        }
        #endregion
    }
}
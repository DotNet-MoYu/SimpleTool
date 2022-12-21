using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ShinyLot.Common
{
    /// <summary>
    /// 网络帮助
    /// </summary>
    public class NetHelper
    {

        /// <summary>
        /// 获取本地IP地址信息
        /// </summary>
        public static string GetAddressIP()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }

        /// <summary>
        /// 查看指定端口是否打开
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool CheckRemotePort(string url)
        {
            System.Uri urlInfo = new Uri(url, false);

            bool result = false;
            try
            {
                IPAddress ip = IPAddress.Parse(urlInfo.Host);
                IPEndPoint point = new IPEndPoint(ip, urlInfo.Port);
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.Connect(point);
                result = true;
            }
            catch (SocketException ex)
            {
                //10061 Connection is forcefully rejected.
                if (ex.ErrorCode != 10061)
                {
                    return false;
                }
            }
            return result;

        }

        /// <summary>
        /// ping IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool PingIp(string url)
        {
            try
            {
                System.Uri urlInfo = new Uri(url, false);
                Ping pingSender = new Ping();
                PingReply reply = pingSender.Send(urlInfo.Host, 120);//第一个参数为ip地址，第二个参数为ping的时间
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }

                else

                {

                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}
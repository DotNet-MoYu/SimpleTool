using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTool
{
    /// <summary>
    ///坐标系转换帮助类
    /// </summary>
    /// WGS84：谷歌地图 OMS
    /// 火星坐标系：高德地图 腾讯地图
    /// 百度坐标系：百度地图
    /// 7
    /// 在地图学中，一般将坐标分为投影坐标和地理坐标。地理坐标和投影坐标的联系和区别对于一般的地图使用者而言可能并不需要掌握的非常清楚。通俗一点来说，地理坐标是一个球体的坐标，而投影坐标是一个平面的坐标。常用的GPS、百度地图、高德地图等都是采用的地理坐标。
    //确定了地理坐标之后，常见的地图应用分别采用的都是什么坐标系呢？一般来说，国外的一些地图，都是采用WGS84坐标系，比如谷歌地图、OMS地图、Bing地图（非中国区域）等。而国内的地图，处于保密需求，大多采用火星坐标系，在WGS84的基础上作了一些偏移，如高德地图、腾讯地图等。而百度又在火星坐标系的基础上又作了一定偏移，生成了自己的百度坐标系。
    //这些加密算法，一般是无法准确还原的，但是在小范围内的数据，可以通过一定的变换相互转化。具体的C#代码如下
    public class GpsHelper
    {
        const double x_pi = 3.14159265358979324 * 3000.0 / 180.0;
        const double pi = 3.1415926535897932384626; // π
        const double a = 6378245.0;// 长半轴
        const double ee = 0.00669342162296594323; // 扁率

        //火星转百度
        public static GPSPoint Gcj02tobd09(GPSPoint HxCor)
        {
            double z = Math.Sqrt(HxCor.lng * HxCor.lng + HxCor.lat * HxCor.lat) + 0.00002 * Math.Sin(HxCor.lat * x_pi);
            double theta = Math.Atan2(HxCor.lat, HxCor.lng) + 0.000003 * Math.Cos(HxCor.lng * x_pi);
            GPSPoint BaiduCor;
            BaiduCor.lng = z * Math.Cos(theta) + 0.0065;
            BaiduCor.lat = z * Math.Sin(theta) + 0.006;
            return BaiduCor;
        }

        //百度转火星
        public static GPSPoint Bd09togcj02(GPSPoint Bd)
        {
            double x = Bd.lng - 0.0065;
            double y = Bd.lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) - .000003 * Math.Cos(x * x_pi);
            GPSPoint hx;
            hx.lng = z * Math.Cos(theta);
            hx.lat = z * Math.Sin(theta);
            return hx;
        }

        //WGS84转火星
        public static GPSPoint Wgs84togcj02(GPSPoint wgs84)
        {
            if (OutOfChina(wgs84))
            {
                return wgs84;
            }
            double dlat = Transformlat(wgs84.lng - 105.0, wgs84.lat - 35.0);
            double dlng = Transformlng(wgs84.lng - 105.0, wgs84.lat - 35.0);
            double radlat = wgs84.lat / 180.0 * pi;
            double magic = Math.Sin(radlat);
            magic = 1 - ee * magic * magic;
            double sqrtmagic = Math.Sqrt(magic);
            dlat = (dlat * 180.0) / ((a * (1 - ee)) / (magic * sqrtmagic) * pi);
            dlng = (dlng * 180.0) / (a / sqrtmagic * Math.Cos(radlat) * pi);
            GPSPoint hx;
            hx.lat = wgs84.lat + dlat;
            hx.lng = wgs84.lng + dlng;
            return hx;
        }


        //火星转84
        public static GPSPoint Gcj02towgs84(GPSPoint hx)
        {
            if (OutOfChina(hx))
            {
                return hx;
            }
            double dlat = Transformlat(hx.lng - 105.0, hx.lat - 35.0);
            double dlng = Transformlng(hx.lng - 105.0, hx.lat - 35.0);
            double radlat = hx.lat / 180.0 * pi;
            double magic = Math.Sin(radlat);
            magic = 1 - ee * magic * magic;
            double sqrtmagic = Math.Sqrt(magic);
            dlat = (dlat * 180.0) / ((a * (1 - ee)) / (magic * sqrtmagic) * pi);
            dlng = (dlng * 180.0) / (a / sqrtmagic * Math.Cos(radlat) * pi);
            GPSPoint wgs84;
            double mglat = hx.lat + dlat;
            double mglng = hx.lng + dlng;
            wgs84.lat = hx.lat * 2 - mglat;
            wgs84.lng = hx.lng * 2 - mglng;
            return wgs84;
        }

        //百度转84
        public static GPSPoint Bd09towgs84(GPSPoint bd)
        {
            GPSPoint hx = Bd09togcj02(bd);
            GPSPoint wgs84 = Gcj02towgs84(hx);
            return wgs84;
        }

        //84转百度
        public static GPSPoint Wgs84tobd09(GPSPoint wgs84)
        {
            GPSPoint hx = Wgs84togcj02(wgs84);
            GPSPoint bd = Gcj02tobd09(hx);
            return bd;
        }
        /*辅助函数*/
        //判断是否在国内
        private static Boolean OutOfChina(GPSPoint wgs84)
        {
            if (wgs84.lng < 72.004 || wgs84.lng > 137.8347)
            {
                return true;
            }
            if (wgs84.lat < 0.8293 || wgs84.lat > 55.8271)
            {
                return true;
            }
            return false;
        }

        /*辅助函数*/
        //转换lat
        private static double Transformlat(double lng, double lat)
        {
            double ret = -100.0 + 2.0 * lng + 3.0 * lat + 0.2 * lat * lat +
        0.1 * lng * lat + 0.2 * Math.Sqrt(Math.Abs(lng));
            ret += (20.0 * Math.Sin(6.0 * lng * pi) + 20.0 *
                    Math.Sin(2.0 * lng * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(lat * pi) + 40.0 *
                    Math.Sin(lat / 3.0 * pi)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(lat / 12.0 * pi) + 320 *
                    Math.Sin(lat * pi / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        /*辅助函数*/
        //转换lng
        private static double Transformlng(double lng, double lat)
        {
            double ret = 300.0 + lng + 2.0 * lat + 0.1 * lng * lng +
        0.1 * lng * lat + 0.1 * Math.Sqrt(Math.Abs(lng));
            ret += (20.0 * Math.Sin(6.0 * lng * pi) + 20.0 *
                    Math.Sin(2.0 * lng * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(lng * pi) + 40.0 *
                    Math.Sin(lng / 3.0 * pi)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(lng / 12.0 * pi) + 300.0 *
                    Math.Sin(lng / 30.0 * pi)) * 2.0 / 3.0;
            return ret;
        }

        // 按类计算
        public static GPSPoint Transform(GPSPoint corOld, string transType)
        {
            switch (transType)
            {
                case "百度转火星":
                    return GpsHelper.Bd09togcj02(corOld);
                case "百度转84":
                    return GpsHelper.Bd09towgs84(corOld);
                case "火星转84":
                    return GpsHelper.Gcj02towgs84(corOld);
                case "84转火星":
                    return GpsHelper.Wgs84togcj02(corOld);
                case "火星转百度":
                    return GpsHelper.Gcj02tobd09(corOld);
                case "84转百度":
                    return GpsHelper.Wgs84tobd09(corOld);
                default:
                    return corOld;
            }
        }

    }

    /// <summary>
    /// 点位信息
    /// </summary>
    public struct GPSPoint
    {
        public double lng;
        public double lat;
    }

}

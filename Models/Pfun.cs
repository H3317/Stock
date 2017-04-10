using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Stock.Models
{
    //常用公共函数
    static class Pfun
    {
        //字符串转换为时间
        public static DateTime StringtoDatetime(string date,string format)
        {
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo {ShortDatePattern = "yyyy-MM-dd HH:mm:ss"};
            return Convert.ToDateTime(date, dtFormat);
        }
        //s字符串转换为时间
        public static DateTime StringtoDatetime1(string date,string format)
        {
            return DateTime.ParseExact(date, format, System.Globalization.CultureInfo.CurrentCulture);
        }

        //字符串转换为浮点数
        public static double Stringtodouble(string data)
        {
            double result;
            bool success = double.TryParse(data, out result);
            if (success)
                return result;
            else
                return 0;
        }
        //字符串转换为浮点数
        public static double Stringtodouble1(string data)
        {
            return Regex.IsMatch(data??"0", @"^[+-]?\d*[.]?\d*$") ? Convert.ToDouble(data) : 0;
        }
        //字符串转换为整型
        public static int Stringtoint(string data)
        {
            int result;
            bool success = int.TryParse(data, out result);
            if (success)
                return result;
            else
                return 0;
        }
        //字符串转换为整型
        public static int Stringtoint1(string data)
        {
            return Regex.IsMatch(data??"0", @"^[+-]?\d*$") ? Convert.ToInt32(data) : 0;
        }
    }
}

using System;
using System.Linq;
using System.Text;

namespace SimpleServer.util
{
    class StringUtilities
    {
        public static String Capitalize(String str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            if (str.Trim().Length == 0)
            {
                return str;
            }
            String firstLetterUppercase = str.Substring(0, 1).ToUpper();
            String theRest = str.Substring(1);
            return firstLetterUppercase + theRest;
        }

        public static String Reverse(String str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            if (str.Trim().Length == 0)
            {
                return str;
            }
            return ReverseWithoutStringBuilder(str);
        }

        private static string ReverseWithoutStringBuilder(string str)
        {
            string reverseStr = "";
            for (int i = str.Length - 1; i >= 0; i--)
            {
                reverseStr += str[i];
            }

            return reverseStr;
        }

        private static string ReverseWithStringBuilder(string str)
        {
            StringBuilder reverseString = new StringBuilder();
            for (int i = str.Length - 1; i >= 0; i--)
            {
                reverseString.Append(str[i]);
            }

            return reverseString.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSM.Models
{
    public class Constant
    {
        public static string STRING_FIRSTNAME = "[FIRST NAME]";
        public static string STRING_PRODUCTNAME = "[PRODUCT NAME]";
        public static string STRING_DATE = "[DATE],[DAY],[TIME]";
        public static string STRING_DAY = "[DAY]";
        public static string STRING_TIME = "[TIME]";
        public static string STRING_CODE = "[CODE]";
        public static string STRING_POST = "[POST";
        public static string STRING_FEATURE = "[FEATURE";
        public static string STRING_COMPANY = "[COMPANY NAME]";
        public static string STRING_LINK = "[LINK";
        public static List<string> AllConstant = new List<string> { STRING_FIRSTNAME, STRING_PRODUCTNAME };
        public static string replaceMailContent(dealdata data, String content) {
            String result = content;
            while (result.Contains(STRING_FIRSTNAME)) {
                result = result.Replace(STRING_FIRSTNAME, data.contact.FirstName);
            }
            while (result.Contains(STRING_PRODUCTNAME))
            {
                result = result.Replace(STRING_FIRSTNAME, data.productname);
            }
            while (result.Contains(STRING_DATE))
            {

                DealTask meeting = new DealTask();
              
                result = result.Replace(STRING_FIRSTNAME,data.date);

            }
    
            return result;
        }
    }
}
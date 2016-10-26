using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WorkerRole1
{
    public class Constant
    {
        public static string STRING_FIRSTNAME = "[First Name]";
        public static string STRING_PRODUCTNAME = "[PRODUCT NAME]";
        public static string STRING_DATE = "[DATE],[DAY],[TIME]";
        public static string STRING_DAY = "[DAY]";
        public static string STRING_TIME = "[TIME]";
        public static string STRING_CODE = "[CODE]";
        public static string STRING_POST = "[POST]";
        public static string STRING_FEATURE = "[FEATURE]";
        public static string STRING_COMPANY = "[COMPANY NAME]";
        public static string STRING_LINK = "[LINK";
        public static string USER_NAME = "[User name]";
        public static string PASSWORD = "[Password]";
        public static List<string> AllConstant = new List<string> { STRING_FIRSTNAME, STRING_PRODUCTNAME };
        public static string replaceMailContent(DealTask task)
        {
            String result = task.TaskDescription;
            while (result.Contains(STRING_FIRSTNAME))
            {
                result = result.Replace(STRING_FIRSTNAME, task.Deal.contact.FirstName);
            }
            while (result.Contains(STRING_PRODUCTNAME))
            {
                result = result.Replace(STRING_FIRSTNAME, task.Deal.softwareProduct.name);
            }
            while (result.Contains(STRING_DATE))
            {

                DealTask meeting = task.Deal.DealTasks.Where(u=> u.type == 4).FirstOrDefault();

                result = result.Replace(STRING_DATE, meeting.Deadline.ToString());

            }

            if (result.Contains(USER_NAME))
            {
                SSMEntities se = new SSMEntities();
                String replaceall = "";
                foreach (Deal_Item item in task.Deal.Deal_Item) {
                    TrialAccount trial = item.productMarketPlan.TrialAccounts.Where(u => u.contactID == null).FirstOrDefault();
                    replaceall = replaceall + '\n' + "User Name for " + item.productMarketPlan.Name + ": " + trial.UserName + " Password: " + trial.Password;
                    TrialAccount trialupdate = se.TrialAccounts.Find(trial.AccountID);
                    trialupdate.contactID = task.Deal.contact.id;
                    se.SaveChanges();
                }
                result = result.Replace(USER_NAME, replaceall);

            }
        



            return result;
        }
    }

}

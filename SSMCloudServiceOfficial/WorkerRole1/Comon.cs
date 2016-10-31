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
        public static string STRING_FIRSTNAME = "";
        public static string STRING_PRODUCTNAME = "";
        public static string STRING_DATE = "";
        public static string STRING_DAY = "";
        public static string STRING_TIME = "";
        public static string STRING_CODE = "";
        public static string STRING_POST = "";
        public static string STRING_FEATURE = "";
        public static string STRING_COMPANY = "";
        public static string STRING_LINK = "";
        public static string USER_NAME = "";
        public static string PASSWORD = "";
        public Constant() {
            SSMEntities se = new SSMEntities();
            STRING_FIRSTNAME = se.ConfigureSys.Find(3).value;
            STRING_PRODUCTNAME = se.ConfigureSys.Find(4).value;
            STRING_DATE = se.ConfigureSys.Find(5).value;
            STRING_DAY = se.ConfigureSys.Find(7).value;
            STRING_TIME = se.ConfigureSys.Find(8).value;
            STRING_CODE = se.ConfigureSys.Find(9).value;
            STRING_FEATURE = se.ConfigureSys.Find(10).value;
            STRING_COMPANY = se.ConfigureSys.Find(11).value;
            STRING_LINK = se.ConfigureSys.Find(12).value;
            USER_NAME = se.ConfigureSys.Find(13).value;
            PASSWORD = se.ConfigureSys.Find(14).value;
        }
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

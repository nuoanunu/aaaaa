using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSM.Models.TempModel
{
    public class FollowupProgressModel
    {
        [UIHint("Plan_Step")]
        public List<Plan_Step> steps { get; set; }
        public int id { get; set; }

        public int productID { get; set; }
        public String Name { get; set; }
        public String Desription { get; set; }
        public FollowupProgressModel()
        {
            steps = new List<Plan_Step>();
            int count = steps.Count();

            for (int i = count + 1; i < 9; i++)
            {
                Plan_Step step = new Plan_Step();
                step.stepNo = i;
                step.TimeFromLastStep = 0;
                steps.Add(step);
            }
        }
        public FollowupProgressModel(PrePurchase_FollowUp_Plan plan)
        {
            this.Name = plan.name;
            this.id = plan.id;
            this.Desription = plan.Description;

            this.productID = plan.productID;
            steps = new List<Plan_Step>();
            foreach (Plan_Step step in plan.Plan_Step)
            {

                steps.Add(step);

            }

            int count = steps.Count();

            for (int i = count + 1; i < 9; i++)
            {
                Plan_Step step = new Plan_Step();
                step.stepNo = i;
                step.TimeFromLastStep = 0;
                steps.Add(step);
            }
            count = steps.Count();

        }
    }
}
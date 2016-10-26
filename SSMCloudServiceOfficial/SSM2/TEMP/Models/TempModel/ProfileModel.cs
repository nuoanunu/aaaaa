using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSM.Models.TempModel
{
    public class ProfileModel
    {
        public String username { get; set; }
        public String password { get; set; }
        public HttpPostedFileBase avatar { get; set; }
        public String firstname { get; set; }
        public String lastname { get; set; }
        public String fullname { get; set; }
        public String email { get; set; }
        public String city { get; set; }
        public String country { get; set; }
        public DateTime birthday { get; set; }
        public String phone { get; set; }
        public List<ProductRespont> responsibleList { get; set; }
        public ProfileModel(){
            responsibleList = new List<ProductRespont>();
            SSMEntities se = new SSMEntities();
            foreach (softwareProduct software in se.softwareProducts.ToList()) {
                ProductRespont pr = new ProductRespont();
                pr.productName = software.name;
                pr.softwareid = software.id;
                pr.minshare = 0;
                pr.maxshare = 0;
                responsibleList.Add(pr);
            }
            }
    }
    public class ProductRespont {
        public String productName { get; set; }
        public int softwareid { get; set; }
        public bool takerespone { get; set; }
        public float minshare {get;set;}
        public float maxshare { get; set; }
    }
}
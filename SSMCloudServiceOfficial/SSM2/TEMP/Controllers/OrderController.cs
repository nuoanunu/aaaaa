using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SSM.Models;
using SSM.Models.Repository;
using SSM.Models.TempModel;

namespace SSM.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        // GET: Order
        public String getOptionsValue(int optionID)
        {
            try
            {
                productRepository pr = new productRepository(new SSMEntities());
                attributeOption opp = pr.getProductOption(optionID);
                optionInfo oi = new optionInfo();
                oi.price = opp.price;
                oi.des = opp.description;


                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(oi);
            }
            catch (Exception e)
            {

            }

            return "";
        }
        public ActionResult getProductAttribute(int productID) {
            productRepository pr = new productRepository(new SSMEntities());
            ViewData["ProductDetail"] = pr.getById(productID);
            return PartialView("TableOptionDetail");
        }
        public String getAllProduct() {
            try {
                productRepository pr = new productRepository(new SSMEntities());
                List<softwareProduct> lst = pr.getAll();
                List<ProductInfo> lst2 = new List<ProductInfo>();
                foreach (softwareProduct so in lst ) {
                   
                    ProductInfo pi = new ProductInfo();
                    pi.name = so.name;
                    pi.id = so.id;
                    lst2.Add(pi);
                }


                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(lst2);
            }
            catch (Exception e) {

            }
     
            return "";
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OrderDetail(int id)
        {
            return View("orderDetail");
        }
        public ActionResult NewDeal(Deal newdeal, String responsiblesRole ,int planID)
        {
            
            productRepository pr = new productRepository(new SSMEntities());
            ViewData["AllProductDetail"] = pr.getAll();
            return View("CreateDeal");
        }
      
    }
}
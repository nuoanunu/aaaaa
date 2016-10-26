using SSM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSM.Models.Repository
{
    public class productRepository: IDisposable
    {
        SSMEntities context;
        public productRepository(SSMEntities contextt)
        {
            context = contextt;
        }
        public List<softwareProduct> getAll() {
            return context.softwareProducts.ToList();
        }
   
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public attributeOption getProductOption(int id) {
            return context.attributeOptions.Find(id);
        }
        public softwareProduct getById(int id)
        {

            return context.softwareProducts .Find(id);
        }
        public void insertNewOption(attributeOption opt) {
            context.attributeOptions.Add(opt);
            context.SaveChanges();
        }
        public void insertNewAttribute(productAttribute opt)
        {
            context.productAttributes.Add(opt);
            context.SaveChanges();
        }
        public void insertNewMarketPlan( productMarketPlan pro, String[] optionsid) {
            context.productMarketPlans.Add(pro);
            context.SaveChanges();
            System.Diagnostics.Debug.WriteLine("AAAAAAAA " + pro.id); 
            for (int i = 0; i < optionsid.Length; i++) {
                try {
                    int id = int.Parse(optionsid[i]);
                    if (context.attributeOptions.Find(id) != null)
                    {
                        PlanOption o = new PlanOption();
                        o.optionID = id;
                        o.orderItemID = pro.id;
                        context.PlanOptions.Add(o);
                        context.SaveChanges();
                    }
                }
                catch (Exception e) {
                }
            
            }
        }
    }
}
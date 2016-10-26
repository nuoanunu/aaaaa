using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSM.Models.Repository
{
    public class DealRepository : IDisposable
    {
        SSMEntities context;
        public DealRepository(SSMEntities contextt)
        {
            context = contextt;
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
        public Deal getByID(int id) {
            try { return context.Deals.Find(id); }
            catch (Exception e) { }
            return null;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
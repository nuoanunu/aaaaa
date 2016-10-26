using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSM.Models;

namespace SSM.Controllers
{
    public class iController : Controller
    {
        public SSMEntities dbcnt = new SSMEntities();
    }
}
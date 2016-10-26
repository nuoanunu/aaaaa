using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebRole1.Models;
using WebRole1.Models.Storage;

namespace WebRole1.Controllers
{
    public class TicketController : Controller
    {
        // GET: Ticket
        public ActionResult Index()
        {
            SSMEntities se = new SSMEntities();
            AspNetUser thisuer = se.AspNetUsers.Find(User.Identity.GetUserId());
            ViewData["myticket"] = thisuer.Tickets.ToList();
            ViewData["catelist"] = se.TicketCategories.ToList();
            ViewData["mostviewedticket"] = se.Tickets.SqlQuery("select top 10 * from [Ticket] order by ViewCounted").ToList();
            return View();
        }

        public ActionResult NewCommend(String description, int ticketID, HttpPostedFileBase file )
        {
            try {
                SSMEntities se = new SSMEntities();

                if (file != null)
                {
                    TicketFile ticketFile = new TicketFile();

                    Storage storeage = new Storage();
                    ticketFile.Url = storeage.uploadMyfile("ticket" + ticketID, "file" + ticketID, file);
                    ticketFile.FileName = file.FileName;
                    se.TicketFiles.Add(ticketFile);
                    ticketFile.TicketID = ticketID;
                    se.SaveChanges();
                }

                TicketComment commend = new TicketComment();
                commend.Creator = User.Identity.GetUserId();
                commend.Description = description;
                commend.createddate = DateTime.Today;
                commend.point = 1;
                commend.ticketID = ticketID;
                commend.Accepted = false;

                se.TicketComments.Add(commend);
                se.SaveChanges();
                return RedirectToAction("Detail", new { id = ticketID });


            }
            catch (Exception e){ }
            
            
            return RedirectToAction("Index");
        }
        public ActionResult Detail(int id)
        {
            SSMEntities se = new SSMEntities();
            ViewData["ticketdetail"] = se.Tickets.Find(id);
            return View("TicketDetail");
        }
        public ActionResult Newticket(String title, int serverity, int category, String description) {
            Ticket tick = new Ticket();
            tick.Creator = User.Identity.GetUserId();
            tick.CreatedDay = DateTime.Today;
            tick.Description = description;
            tick.TicketCategory = category;
            tick.TicketServerity = serverity;
            tick.TicketName = title;
            tick.TicketStatus = 1;
            tick.ViewCounted = 0;
            SSMEntities se = new SSMEntities();
            se.Tickets.Add(tick);
            se.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
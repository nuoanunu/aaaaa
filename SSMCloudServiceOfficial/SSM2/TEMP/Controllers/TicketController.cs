using Microsoft.AspNet.Identity;
using SSM.Models;
using SSM.Models.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSM.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        // GET: Ticket
        public ActionResult Index()
        {
            SSMEntities se = new SSMEntities();
            AspNetUser thisuer = se.AspNetUsers.Find(User.Identity.GetUserId());
            ViewData["mytickets"] = thisuer.Tickets.ToList();
            return View("MyTickets");
        }
        public ActionResult Detail(int id)
        {
            SSMEntities se = new SSMEntities();
            ViewData["ticketdetail"] = se.Tickets.Find(id);
            return View("TicketDetail");
        }
        public ActionResult NewCommend(String description, int ticketID, HttpPostedFileBase file)
        {
            try
            {
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
            catch (Exception e) { }


            return RedirectToAction("Index");
        }
    }
}
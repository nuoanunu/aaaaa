using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSM.Models;
using SSM.Models.Repository;
using SSM.Models.TempModel;
using SSM.ViewModels;

namespace SSM.Controllers
{
    [Authorize]
    public class MailTemplateController : Controller
    {
        EmailCategoryRepository cateRepo;
        MailTemplateRepository mailRepo;
        
        public MailTemplateController()
        {
            cateRepo = new EmailCategoryRepository(new SSMEntities());
            mailRepo = new MailTemplateRepository(new SSMEntities());
        }

        public ActionResult Index()
        {
            ViewData["templateList"] = mailRepo.getAll();
            return View("MailTemplateList");

           
        }


        public ActionResult Add()
        {
            var viewModel = new CreateEmailTemplateViewModel
            {
                EmailTemplateEntity = new EmailTemplateEntity(),
                EmailCategories = getCategories(),
                EmailCategory = new EmailCategory()
            };
            return View("Add",viewModel);
        }

        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Save(CreateEmailTemplateViewModel viewModel)
        {            
            if (!ModelState.IsValid)
            {
                var newViewModel = new CreateEmailTemplateViewModel
                {
                    EmailTemplateEntity = viewModel.EmailTemplateEntity,
                    EmailCategories = getCategories()
                };
                return View("Add", newViewModel);
            }
            var emailTemplate = viewModel.EmailTemplateEntity;
            if (emailTemplate.id == 0)
            {
                emailTemplate.isActive = true;
                emailTemplate.createdDate = DateTime.Now;
                mailRepo.CreateNewEmailTemplate(emailTemplate);
            }
            else
            {
                mailRepo.EditMailTemplate(emailTemplate);
            }
            return RedirectToAction("Index", "MailTemplate");
        }


        //Not used yet, insert category not using ajax
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult InsertNewCategory(CreateEmailTemplateViewModel viewModel)
        {
            var emailCategory = viewModel.EmailCategory;
            var dbEmailCategory = new EMAIL_Category();
            dbEmailCategory.Name = emailCategory.Name;
            dbEmailCategory.description = emailCategory.description;
            dbEmailCategory.createddate = DateTime.Now;
            cateRepo.CreateNewEmailCategory(dbEmailCategory);
            var newViewModel = new CreateEmailTemplateViewModel
            {
                EmailTemplateEntity = viewModel.EmailTemplateEntity,
                EmailCategories = getCategories(),
                EmailCategory = new EmailCategory()
            };
            return RedirectToAction("Add", "MailTemplate");
        }


        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult InsertNewCategoryUsingAjax(CreateEmailTemplateViewModel viewModel)
        {
            var emailCategory = viewModel.EmailCategory;
            var dbEmailCategory = new EMAIL_Category();
            dbEmailCategory.Name = emailCategory.Name;
            dbEmailCategory.description = emailCategory.description;
            dbEmailCategory.createddate = DateTime.Now;
            
            var dbEmailCategoryAfterInsert = cateRepo.CreateNewEmailCategory(dbEmailCategory);
            return Json(dbEmailCategoryAfterInsert);
        }

        public ActionResult EditTemplate(int id)
        {
            Email_Template dbTemplate = mailRepo.getById(id);

            EmailTemplateEntity template = new EmailTemplateEntity();
            template.Name = dbTemplate.Name;
            template.MailContent = dbTemplate.MailContent;
            template.CateID = dbTemplate.CateID;

            EmailCategory cate = new EmailCategory();
            cate.Name = cateRepo.getById((int)template.CateID).Name;


            var viewModel = new CreateEmailTemplateViewModel
            {
                EmailTemplateEntity = template,
                EmailCategories = getCategories(),
                EmailCategory = cate
            };
            return View("EditTemplate", viewModel);


        }

        public List<EmailCategory> getCategories()
        {
            var dbCategories = cateRepo.getAll();
            EmailCategory category = null;
            List<EmailCategory> cates = new List<EmailCategory>();
            foreach (var cate in dbCategories)
            {
                category = new EmailCategory();
                category.id = cate.id;
                category.Name = cate.Name;
                category.color = cate.color;
                category.createddate = cate.createddate;
                category.creator = cate.creator;
                category.description = cate.description;
                category.lastupdate = category.lastupdate;
                cates.Add(category);
            }
            return cates;
        }
    }
}
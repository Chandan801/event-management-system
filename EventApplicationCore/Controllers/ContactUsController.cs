using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventApplicationCore.Filters;
using EventApplicationCore.Interface;
using EventApplicationCore.Model;
using Microsoft.AspNetCore.Http;
using EventApplicationCore.Library;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventApplicationCore.Controllers
{
    [ValidateUserSession]
    public class ContactUsController : Controller
    {
        ILogin _ILogin;
        IRegistration _IRegistration;
        public ContactUsController(ILogin ILogin, IRegistration IRegistration)
        {
            _ILogin = ILogin;
            _IRegistration = IRegistration;
        }


        // GET: /<controller>/ Userinformation
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ContactUs()
        {
            return View(new ContactUsModel());
        }

        [HttpPost]
        
        public IActionResult ContactUs(ContactUsModel ContactUsModel)
        {
           
                return View(ContactUsModel);
           

            
        }

        
    }
}

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
    public class CustomerController : Controller
    {
        ILogin _ILogin;
        IRegistration _IRegistration;
        public CustomerController(ILogin ILogin, IRegistration IRegistration)
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
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordModel ChangePasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View(ChangePasswordModel);
            }

            var password = EncryptionLibrary.EncryptText(ChangePasswordModel.Password);
            var registrationModel = _IRegistration.Userinformation(Convert.ToInt32(HttpContext.Session.GetString("UserID")));

            if (registrationModel.Password == password)
            {
                var registration = new Registration();
                registration.Password = EncryptionLibrary.EncryptText(ChangePasswordModel.NewPassword);
                registration.ID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                var result = _ILogin.UpdatePassword(registration);

                if (result)
                {
                    TempData["MessageUpdate"] = "Password Updated Successfully";
                    ModelState.Clear();
                    return View(new ChangePasswordModel());
                }
                else
                {
                    return View(ChangePasswordModel);
                }
            }
            else
            {
                TempData["MessageUpdate"] = "Invalid Password";
                return View(ChangePasswordModel);
            }
        }
        [HttpGet]
        public IActionResult ContactUs()
        {
            return View(new ContactUsModel());
        }

        [HttpPost]

        public IActionResult ContactUs(ContactUsModel contactUsModel)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(new ContactUsModel());
                }

                if (contactUsModel != null && contactUsModel.Name != null)
                {
                    var result = 0;
                    var validateChecked = 0;

                    validateChecked = 1;

                    ContactUsModel objContact = new ContactUsModel()
                    {
                        Name = contactUsModel.Name,
                        EmailId = contactUsModel.EmailId,
                        Subject = contactUsModel.Subject,
                        Message = contactUsModel.Message,
                        Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID")),
                        CreatedDate = DateTime.Now
                    };
                    result = _IRegistration.SaveContactUs(objContact);



                    if (validateChecked == 0)
                    {
                        ModelState.AddModelError("", "Please Enter Details!");
                    }



                    if (result > 0)
                    {
                        ModelState.Clear();
                        TempData["MessageUpdate"] = "Thank you for contacting us!";
                        return View(new ContactUsModel());
                    }
                    else
                    {
                        ModelState.AddModelError("", "ERROR While Saving Details!");
                        return View(contactUsModel);
                    }
                }
                else
                {
                    return View(new ContactUsModel());
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}

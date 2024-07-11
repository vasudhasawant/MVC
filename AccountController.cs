using _3_MVCCRUDUSINGCRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3_MVCCRUDUSINGCRUD.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account

        ProductDBContext _db = new ProductDBContext();
     
        [HttpGet]
        public ActionResult ListUsers()
        {

            return View(_db.Users.ToList());
        }





        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Users.Add(user);
                    _db.SaveChanges();


                    ViewBag.Message = "User Created Successfully Login To Use Website";
                }
                else
                {

                    ViewBag.Message = "Please correct input";

                }
            }
            catch
            {
                    ViewBag.Message = "Error in Creating user";
            }

            return View();
        }

        [HttpGet]
        public JsonResult IsEmailExists(string Email)
        {
            bool IsEmailExists = _db.Users.Any(u=>u.Email.Equals(Email));
            return Json(!IsEmailExists,JsonRequestBehavior.AllowGet);
        }
    }
}
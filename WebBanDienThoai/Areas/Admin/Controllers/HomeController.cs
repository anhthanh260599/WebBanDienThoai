using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDienThoai.Context;


namespace WebBanDienThoai.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        WebBanDienThoaiEntities objWebBanDienThoaiEntities = new WebBanDienThoaiEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {
            if (Session["idUser"] != null)
            {
                var lstProduct = objWebBanDienThoaiEntities.Products.ToList();
                return View(lstProduct);
            }
            else
            {
                return View("Login");

            }
        }
    }
}
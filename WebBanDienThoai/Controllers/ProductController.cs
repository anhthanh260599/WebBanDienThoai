using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDienThoai.Context;

namespace WebBanDienThoai.Controllers
{
    public class ProductController : Controller
    {
        WebBanDienThoaiEntities objWebBanDienThoaiEntities = new WebBanDienThoaiEntities();

        // GET: Product
        public ActionResult Detail(int Id)
        {
            var objProduct = objWebBanDienThoaiEntities.Products.Where(n=>n.Id==Id).FirstOrDefault();

            return View(objProduct);
        }


    }
}
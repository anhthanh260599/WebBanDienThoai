using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDienThoai.Context;

namespace WebBanDienThoai.Controllers
{
    public class CategoryController : Controller
    {
        WebBanDienThoaiEntities objWebBanDienThoaiEntities = new WebBanDienThoaiEntities();
        // GET: Category
        public ActionResult Index()
        {
            var lstCategory = objWebBanDienThoaiEntities.Categories.ToList();
            return View(lstCategory);
        }

        public ActionResult ProductCategory(int Id) 
        {
            var lstProduct = objWebBanDienThoaiEntities.Products.Where(n=>n.CategoryId == Id).ToList();
            return View(lstProduct);
        }
    }
}
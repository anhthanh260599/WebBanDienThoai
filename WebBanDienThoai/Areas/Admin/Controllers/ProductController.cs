using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDienThoai.Context;
using static WebBanDienThoai.Common;

namespace WebBanDienThoai.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        WebBanDienThoaiEntities objWebBanDienThoaiEntities = new WebBanDienThoaiEntities();

        public object ListtoDataConverter { get; private set; }


        // GET: Admin/Product
        //Lấy tất cả sản phẩm trong Bảng Product
        //public ActionResult Index(string SearchString)
        //{
        //    //Lấy tất cả sản phẩm trong Bảng Product
        //    var lstProduct = objWebBanDienThoaiEntities.Products.ToList();
        //    return View(lstProduct);
        //}
        public ActionResult Index(string currentFilter,string SearchString,int? page)
        {
           
            var lstProduct = new List<Product>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                // lấy ds sản phẩm theo từ khóa tìm kiếm
                lstProduct = objWebBanDienThoaiEntities.Products.Where(n=>n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                // lấy tất cả sản phẩm trong bảng Product
                lstProduct = objWebBanDienThoaiEntities.Products.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            // Số lượng item của 1 trang = 4
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            // sắp xếp theo id sản phẩm, sản phẩm mới lên đầu tiên
            lstProduct = lstProduct.OrderByDescending(n=>n.Id).ToList();

            return View(lstProduct.ToPagedList(pageNumber,pageSize))    ;
        }

        void LoadData()
        {
            Common objCommon = new Common();
            // Lấy dữ liệu danh mục dưới DB, Parse (từ kiểu int sang kiểu chuỗi) (xem class Common)
            var lstCat = objWebBanDienThoaiEntities.Categories.ToList();
            // Convert sang select list dạng value, text
            ListtoDataTableConverter converter = new ListtoDataTableConverter();

            DataTable dtCategory = converter.ToDataTable(lstCat);
            ViewBag.ListCategory = objCommon.ToSelectList(dtCategory, "Id", "Name");

            // Lấy dữ liệu thương hiệu dưới DB, Parse (từ kiểu int sang kiểu chuỗi) (xem class Common)
            var lstBrand = objWebBanDienThoaiEntities.Brands.ToList();
            DataTable dtBrand = converter.ToDataTable(lstBrand);
            // Convert sang select list dạng value, text
            ViewBag.ListBrand = objCommon.ToSelectList(dtBrand, "Id", "Name");

            //Loại sản phẩm (code cứng)
            List<ProductType> lstProductType = new List<ProductType>();
            ProductType objProductType = new ProductType();

            objProductType.Id = 01;
            objProductType.Name = "Giảm giá sốc";
            lstProductType.Add(objProductType);

            objProductType = new ProductType();
            objProductType.Id = 02;
            objProductType.Name = "Đề xuất";
            lstProductType.Add(objProductType);

            DataTable dtProductType = converter.ToDataTable(lstProductType);
            ViewBag.ProductType = objCommon.ToSelectList(dtProductType, "Id", "Name");
        }

        //Thêm 
        [HttpGet]
        public ActionResult Create()
        {
            this.LoadData();
            return View();
        }

        //Thêm hình ảnh và lưu vào db
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Product objProduct)
        {
            this.LoadData();
            if (ModelState.IsValid) //Kiểm tra ràng buộc dữ liệu
            {
                try
                {

                    if (objProduct.ImageUpload != null) // Kiểm tra hình ảnh và tạo đuôi hình ảnh
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName); // tenhinh
                        string extension = Path.GetExtension(objProduct.ImageUpload.FileName); // đuôi tên hình (png,jpg,...)
                        fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension; // tenhinh.png
                        objProduct.Avatar = fileName;
                        objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));
                    }
                    objWebBanDienThoaiEntities.Products.Add(objProduct);
                    objWebBanDienThoaiEntities.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch
                {
                    return View();
                }
            }
            return View(objProduct);
        }

        //Xem chi tiết
        [HttpGet]
        public ActionResult Details(int id)
        {
            var objProduct = objWebBanDienThoaiEntities.Products.Where(n=>n.Id== id).FirstOrDefault();
            return View(objProduct);
        }

        //Xóa sản phẩm
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objProduct = objWebBanDienThoaiEntities.Products.Where(n => n.Id == id).FirstOrDefault();
           
            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Delete(Product objPro)
        {
            var objProduct = objWebBanDienThoaiEntities.Products.Where(n => n.Id == objPro.Id).FirstOrDefault();

            objWebBanDienThoaiEntities.Products.Remove(objProduct);
            objWebBanDienThoaiEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        //Chỉnh sửa sản phẩm
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objProduct = objWebBanDienThoaiEntities.Products.Where(n => n.Id == id).FirstOrDefault();

            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Edit(Product objProduct)
        {
            if (objProduct.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objProduct.Avatar = fileName;
                objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));
            }
            objWebBanDienThoaiEntities.Entry(objProduct).State = EntityState.Modified;
            objWebBanDienThoaiEntities.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
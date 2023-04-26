using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanDienThoai.Context;
using WebBanDienThoai.Models;

namespace WebBanDienThoai.Controllers
{
    public class PaymentController : Controller
    {
        WebBanDienThoaiEntities objWebBanDienThoaiEntities = new WebBanDienThoaiEntities();

        // GET: Payment
        public ActionResult Index()
        {
            if (Session["idUser"] == null)
            {
                return RedirectToAction("Login","Home");
            }
            else
            {
                // lấy thông tin từ giờ hàng từ biến Session
                var lstCart = (List<CartModel>)Session["cart"];
                // gán chữ liệu cho Order
                Order objOrder = new Order();
                objOrder.Name = "Đơn hàng-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                objOrder.UserId = int.Parse(Session["idUser"].ToString());
                objOrder.CreatedOnUtc= DateTime.Now;
                objOrder.Status = 1;
                objWebBanDienThoaiEntities.Orders.Add(objOrder);
                // lưu thông tin dữ liệu vào bảng Order
                objWebBanDienThoaiEntities.SaveChanges();

                // Lấy OrderId vừa mới tạo để lưu vào bảng OrderDetail
                int intOrderId = objOrder.Id;

                List<OrderDetail> lstOrderDetail = new List<OrderDetail>();

                foreach (var item in lstCart)
                {
                    OrderDetail obj = new OrderDetail();
                    obj.Quantity= item.Quantity;
                    obj.ProductId = item.Product.Id;
                    obj.OrderId = intOrderId;
                    lstOrderDetail.Add(obj);
                }
                objWebBanDienThoaiEntities.OrderDetails.AddRange(lstOrderDetail);
                objWebBanDienThoaiEntities.SaveChanges();

            }
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_BanSach.Models;

namespace Web_BanSach.Controllers
{
    public class DatHangController : Controller
    {
        QL_SACHEntities db = new QL_SACHEntities();
        // GET: DatHang
        public ActionResult Checkout()
        {
            var gio = Session["GioHang"] as List<GioHang>;
            if (gio == null || !gio.Any())
            {
                return RedirectToAction("Index", "Cart");
            }
            ViewBag.TongTien = gio.Sum(s => s.ThanhTien);
            return View(gio);
        }
        [HttpPost]
        public ActionResult DatHang( string diaChi)
        {
            var kh = Session["KH"] as KHACHHANG;
            var gio = Session["GioHang"] as List<GioHang>;
            if (gio == null || !gio.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            // Tạo hóa đơn
            HOADON hd = new HOADON
            {
                NGAYLAP = DateTime.Now,
                MATINHTRANG = 1,
                TONGTIEN = (int?)gio.Sum(s => s.ThanhTien),
                MAKHACHHANG = kh.MAKHACHHANG,
                DIACHI = diaChi,
            };
            db.HOADONs.Add(hd);
            db.SaveChanges();

            // Thêm chi tiết hóa đơn
            foreach (var item in gio)
            {
                CHITIET_HOADON ct = new CHITIET_HOADON
                {
                    MAHOADON = hd.MAHOADON,
                    MASACH = item.MaSach,
                    SOLUONG = item.SoLuong,
                    DONGIA = (int?)item.GiaBan
                };
                db.CHITIET_HOADON.Add(ct);
            }
            db.SaveChanges();

            // Xóa giỏ hàng
            Session["GioHang"] = null;

            return RedirectToAction("Success");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_BanSach.Models;

namespace Web_BanSach.Controllers
{
    public class CartController : Controller
    {
        QL_SACHEntities db = new QL_SACHEntities();
        // GET: Cart
        private List<GioHang> LayGio()
        {
            List<GioHang> gio = Session["GioHang"] as List<GioHang>;
            if (gio == null)
            {
                gio = new List<GioHang>();
                Session["GioHang"] = gio;
            }
            return gio;
        }
        // hiển thị giỏ hàng
        public ActionResult Index()
        {
            List<GioHang> gio = LayGio();
            ViewBag.TongTien = gio.Sum(s => s.ThanhTien);
            return View(gio);
        }

        //thêm sách vào giỏ hàng
        public ActionResult ThemGio(int masach, string url)
        {
            if (Session["TEN"] == null)
            {
                return RedirectToAction("Login", "Acount");
            }
            List<GioHang> gio = LayGio();

            GioHang sp = gio.FirstOrDefault(s => s.MaSach == masach);
            if (sp == null)
            {
                var s = db.BANGSACHes.FirstOrDefault(x => x.MASACH == masach);
                if (s == null) return HttpNotFound();

                GioHang item = new GioHang
                {
                    MaSach = masach,
                    TenSach = s.TENSACH,
                    AnhBia = s.ANHBIA,
                    GiaBan = s.GIABAN ?? 0,
                    SoLuong = 1
                };

                gio.Add(item);
            }
            else
            {
                sp.SoLuong++;
            }

            return Redirect(url);
        }

        // Cập nhật số lượng
        [HttpPost]
        public ActionResult CapNhatGio(int masach, int soluong)
        {
            List<GioHang> gio = LayGio();
            GioHang sp = gio.FirstOrDefault(s => s.MaSach == masach);

            if (sp != null)
            {
                if (soluong > 0)
                    sp.SoLuong = soluong;
                else
                    gio.Remove(sp);
            }

            return RedirectToAction("Index", "Cart");
        }
        // Xóa 1 sản phẩm khỏi giỏ
        public ActionResult XoaGio(int masach)
        {
            List<GioHang> gio = LayGio();
            GioHang sp = gio.FirstOrDefault(s => s.MaSach == masach);

            if (sp != null)
                gio.Remove(sp);

            return RedirectToAction("Index", "Cart");
        }




        /// ////////////////////////////////////////////////
        //xử lý badge
        public int TongSoLuong()
        {
            List<GioHang> gio = Session["GioHang"] as List<GioHang>;
            if (gio == null) return 0;
            return gio.Sum(s => s.SoLuong);
        }
        public ActionResult GetCartCount()
        {
            int count = TongSoLuong();
            return Json(new { count = count }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ThemGioAjax(int masach)
        {
            List<GioHang> gio = LayGio();

            GioHang sp = gio.FirstOrDefault(s => s.MaSach == masach);
            if (sp == null)
            {
                var s = db.BANGSACHes.FirstOrDefault(x => x.MASACH == masach);
                if (s == null) return HttpNotFound();

                GioHang item = new GioHang
                {
                    MaSach = masach,
                    TenSach = s.TENSACH,
                    AnhBia = s.ANHBIA,
                    GiaBan = s.GIABAN ?? 0,
                    SoLuong = 1
                };
                gio.Add(item);
            }
            else
            {
                sp.SoLuong++;
            }

            return Json(new { success = true, count = TongSoLuong() }, JsonRequestBehavior.AllowGet);
        }

        // ===============================



    }
}
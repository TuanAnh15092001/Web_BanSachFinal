using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Web_BanSach.Controllers
{
    public class AcountController : Controller
    {
        QL_SACHEntities ql = new QL_SACHEntities();
        [HttpGet]
        public ActionResult Login(string email, string matkhau)
        {
            ViewBag.Url = !String.IsNullOrEmpty(Request.UrlReferrer.ToString()) ? Request.UrlReferrer.ToString() : "/";
            // Tìm khách hàng theo email + mật khẩu
            var kh = ql.KHACHHANGs
                       .SingleOrDefault(k => k.EMAIL == email && k.MATKHAU == matkhau);

            if (kh != null)
            {
                // Lưu thông tin vào Session
                Session["TEN"] = kh.TENKHACHHANG;     // bạn đang dùng để "Xin chào"
                Session["MaKH"] = kh.MAKHACHHANG;      // THÊM DÒNG NÀY

                return RedirectToAction("Index", "Sach");
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XuLyFormDN(FormCollection f, string duongdan)
        {
            string email = f["email"];
            string pass = f["password"];

            // Kiểm tra khách hàng
            var kh = ql.KHACHHANGs.FirstOrDefault(k => k.EMAIL == email && k.MATKHAU == pass);
            if (kh != null)
            {
                FormsAuthentication.SetAuthCookie(kh.EMAIL, true);
                // Set session KH
                Session["KH"] = kh;
                Session["MA"] = kh.MAKHACHHANG;
                Session["TEN"] = kh.TENKHACHHANG;
                Session["LOAI_TK"] = "KHACHHANG";

                // QUAN TRỌNG: thêm dòng này cho KhachHangController dùng
                Session["MaKH"] = kh.MAKHACHHANG;

                // Redirect về trang trước khi login (duongdan) hoặc default
                if (!string.IsNullOrEmpty(duongdan))
                    return Redirect(duongdan);

                return RedirectToAction("Index", "Sach"); // default
            }

            // Kiểm tra nhân viên
            var nv = ql.NHANVIENs.FirstOrDefault(n => n.EMAIL == email && n.MATKHAU == pass);
            if (nv != null)
            {
                FormsAuthentication.SetAuthCookie(nv.TENNHANVIEN, true);
                Session["TEN"] = nv.TENNHANVIEN;
                Session["LOAI_TK"] = "NHANVIEN";
                Session["MA"] = nv.MANHANVIEN;
                Session["VAITRO"] = nv.MAVAITRO;

                if (nv.MAVAITRO == 1) // admin
                {
                    return RedirectToAction("Index", "Sach", new { area = "Admin" });
                }

                return RedirectAfterLogin(duongdan);
            }

            ViewBag.Loi = "Email hoặc mật khẩu không đúng";
            return View("Login");
        }

        // =======================
        // Hàm xử lý redirect hợp lý
        // =======================
        private ActionResult RedirectAfterLogin(string duongdan)
        {
            // nếu không có đường dẫn cũ thì về Trang chủ
            if (string.IsNullOrEmpty(duongdan))
                return RedirectToAction("Index", "Sach");

            // Nếu URL có chứa Register → về trang chủ thay vì quay lại Register
            if (duongdan.ToLower().Contains("/acount/register"))
                return RedirectToAction("Index", "Sach");

            // còn lại thì quay đúng đường dẫn cũ
            return Redirect(duongdan);
        }


        // GET: Hiển thị form đăng ký
        public ActionResult Register()
        {
            ViewBag.Url = !String.IsNullOrEmpty(Request.UrlReferrer.ToString()) ? Request.UrlReferrer.ToString() : "/";
            return View();
        }

        // POST: Xử lý form đăng ký
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(FormCollection f)
        {
            string ten = f["TENKHACHHANG"];
            string pass = f["MATKHAU"];
            string sdt = f["SDT"];
            string email = f["EMAIL"];
            string diachi = f["DIACHI"];

            // Kiểm tra email đã tồn tại chưa
            var check = ql.KHACHHANGs.FirstOrDefault(k => k.EMAIL == email);
            if (check != null)
            {
                ViewBag.Loi = "Email này đã được sử dụng!";
                return View();
            }

            // Tạo đối tượng khách hàng mới
            KHACHHANG kh = new KHACHHANG();
            kh.TENKHACHHANG = ten;
            kh.EMAIL = email;
            kh.MATKHAU = pass;
            kh.SDT = sdt;
            kh.DIACHI = diachi;

            ql.KHACHHANGs.Add(kh);
            ql.SaveChanges();

            ViewBag.ThongBao = "Đăng ký thành công! Bạn có thể đăng nhập ngay.";
            return RedirectToAction("Login");
        }



        public ActionResult DangXuat()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Sach", new { area = "" });
            //có thể dùng cách dưới
            //return Redirect("/");
        }

        // GET: Acount

        //////////



    }
}

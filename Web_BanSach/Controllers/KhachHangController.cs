using System.Linq;
using System.Web.Mvc;
using Web_BanSach.Models;   // namespace chứa QL_SACHEntities, KHACHHANG, DONHANG...

namespace Web_BanSach.Controllers
{
    public class KhachHangController : Controller
    {
        QL_SACHEntities db = new QL_SACHEntities();

        // Xem thông tin khách hàng
        public ActionResult XemThongTin()
        {
            if (Session["MaKH"] == null)
                return RedirectToAction("Login", "Acount"); 

            int maKH = (int)Session["MaKH"];
            var kh = db.KHACHHANGs.SingleOrDefault(k => k.MAKHACHHANG == maKH);

            if (kh == null) return HttpNotFound();

            return View(kh);  
        }

        // GET: Sửa thông tin
        [HttpGet]
        public ActionResult SuaThongTin()
        {
            if (Session["MaKH"] == null)
                return RedirectToAction("Login", "Acount");

            int maKH = (int)Session["MaKH"];
            var kh = db.KHACHHANGs.SingleOrDefault(k => k.MAKHACHHANG == maKH);

            if (kh == null) return HttpNotFound();

            return View(kh);
        }

        // POST: Sửa thông tin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaThongTin(KHACHHANG model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var kh = db.KHACHHANGs.Find(model.MAKHACHHANG);
            if (kh == null) return HttpNotFound();

            // Cập nhật các thuộc tính được phép sửa
            kh.TENKHACHHANG = model.TENKHACHHANG;
            kh.MATKHAU = model.MATKHAU;       // nếu cho phép đổi mật khẩu
            kh.SDT = model.SDT;
            kh.EMAIL = model.EMAIL;
            kh.DIACHI = model.DIACHI;

            db.SaveChanges();

            return RedirectToAction("XemThongTin");
        }

        // Thông tin đơn hàng của khách hàng đó
        public ActionResult ThongTinDonHang()
        {
            if (Session["MaKH"] == null)
                return RedirectToAction("Login", "Acount");

            int maKH = (int)Session["MaKH"];
            var kh = db.KHACHHANGs.SingleOrDefault(k => k.MAKHACHHANG == maKH);
            var donHangs = db.HOADONs
                             .Where(d => d.MAKHACHHANG == maKH)
                             .OrderByDescending(d => d.NGAYLAP)
                             .ToList();

            return View(donHangs);  
        }
    }
}
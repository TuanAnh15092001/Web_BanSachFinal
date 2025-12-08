using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_BanSach.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        QL_SACHEntities db = new QL_SACHEntities();
        // GET: Admin/KhachHang
        public ActionResult Index()
        {
            List<KHACHHANG> lstKH = db.KHACHHANGs.ToList();
            return View(lstKH);
        }
    }
}
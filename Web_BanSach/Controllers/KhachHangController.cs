using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_BanSach.Controllers
{
    public class KhachHangController : Controller
    {
        QL_SACHEntities db = new QL_SACHEntities();
        // GET: KhachHang
        public ActionResult Index()
        {
            return View();
        }
    }
}
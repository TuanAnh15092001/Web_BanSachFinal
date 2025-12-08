using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_BanSach.Areas.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        QL_SACHEntities db = new QL_SACHEntities();
        // GET: Admin/ThongKe
        public ActionResult Index()
        {
            List<HOADON> listHD = db.HOADONs.ToList();
            return View(listHD);
        }
    }
}
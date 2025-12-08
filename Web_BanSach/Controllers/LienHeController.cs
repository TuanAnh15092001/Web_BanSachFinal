using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_BanSach.Controllers
{
    public class LienHeController : Controller
    {
        // GET: LienHe
        public ActionResult Index()
        {
            ViewBag.TieuDe = "Thông tin liên Hệ của nhóm 5";
            ViewBag.Message = "Web bán sách trực tuyến được thực hiện bởi nhóm 5.";
            List<string> list = new List<string>();
            list.Add("1. Nguyễn Tuấn Anh - 2001207182");
            list.Add("2. La thuận Phát - 2001230634");
            list.Add("3. Nguyễn Hà Trọng Phúc - 2001230686");
            list.Add("4. Đinh Tấn Huy - 2033216429");
            ViewBag.Nhom5 = list;
            return View();
        }
    }
}
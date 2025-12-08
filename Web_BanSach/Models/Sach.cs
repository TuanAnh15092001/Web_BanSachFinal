using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_BanSach.Controllers;

namespace Web_BanSach.Models
{
    public class Sach
    {
        public virtual LOAISACH LOAISACH { get; set; }
        public virtual NHAXUATBAN NHAXUATBAN { get; set; }
    }

    public class Saches : BANGSACH
    {
        public List<LOAISACH> lstloai { get; set; }
        public List<NHAXUATBAN> lstNXB { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_BanSach.Models
{
    public class GioHang
    {
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public string AnhBia { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public decimal ThanhTien => SoLuong * GiaBan;
    }
}
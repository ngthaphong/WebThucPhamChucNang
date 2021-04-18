using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Giohang
    {
        //Tao doi tuong data chua dữ liệu từ model dbSP đã tạo. 
        dbQLSanPhamDataContext data = new dbQLSanPhamDataContext();
        public int iMaSP { set; get; }
        public string sTenSanPham { set; get; }
        public string sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }

        }
        //Khoi tao gio hàng theo MaSP duoc truyen vao voi Soluong mac dinh la 1
        public Giohang(int Masanpham)
        {
            iMaSP = Masanpham;
            SANPHAM sanpham = data.SANPHAMs.Single(n => n.MaSP == iMaSP);
            sTenSanPham = sanpham.TenSanPham;
            sAnhbia = sanpham.Anhbia;
            dDongia = double.Parse(sanpham.Giaban.ToString());
            iSoluong = 1;
        }
    }

}
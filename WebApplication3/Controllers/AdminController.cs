using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;
using System.IO;
using PagedList;

namespace WebApplication3.Controllers
{
    public class AdminController : Controller
    {
       


        dbQLSanPhamDataContext db = new dbQLSanPhamDataContext();
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SanPham(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.SACHes.ToList());
            return View(db.SANPHAMs.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult LOAI(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.SACHes.ToList());
            return View(db.LOAIs.ToList().OrderBy(n => n.MaLoai).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult NSX(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.SACHes.ToList());
            return View(db.NHASANXUATs.ToList().OrderBy(n => n.MaNSX).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult DONHANG(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.SACHes.ToList());
            return View(db.DONDATHANGs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult KHACHHANG(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.SACHes.ToList());
            return View(db.KHACHHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
       




        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến 
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (ad)        
           
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    // ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

        [HttpGet]
        public ActionResult ThemmoiSanpham()
        {
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
            ViewBag.MaLoai = new SelectList(db.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSanpham(SANPHAM sanpham, HttpPostedFileBase fileUpload )
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaLoai = new SelectList(db.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    sanpham.Anhbia = fileName;
                    //Luu vao CSDL
                    db.SANPHAMs.InsertOnSubmit(sanpham);
                    db.SubmitChanges();
                }
                return RedirectToAction("SanPham");
            } 
        }

        //Hiển thị sản phẩm
        public ActionResult Chitietsanpham(int id)
        {
            //Lay ra doi tuong sach theo ma
            SANPHAM sanpham = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.Masanpham = sanpham.MaSP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }
        //Xóa sản phẩm
        [HttpGet]
        public ActionResult Xoasanpham(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            SANPHAM sanpham = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.Masanpham = sanpham.MaSP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }

        [HttpPost, ActionName("Xoasanpham")]
        public ActionResult Xacnhanxoa(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            SANPHAM sanpham = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.Masanpham = sanpham.MaSP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SANPHAMs.DeleteOnSubmit(sanpham);
            db.SubmitChanges();
            return RedirectToAction("SanPham");
        }
        //Chinh sửa sản phẩm

        public ActionResult Suasanpham(int id)
        {
            //Lay ra doi tuong sach theo ma
            SANPHAM sanpham = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.Masanpham = sanpham.MaSP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, sắp xep tang dan trheo ten san pham, chon lay gia tri Ma sp, hien thi thi Tensanpham
            ViewBag.MaLoai = new SelectList(db.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", sanpham.MaLoai);
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX", sanpham.MaNSX);
            return View(sanpham);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasanpham(SANPHAM sanpham, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaLoai = new SelectList(db.LOAIs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    sanpham.Anhbia = fileName;
                    //Luu vao CSDL   
                    UpdateModel(sanpham);
                    db.SubmitChanges();

                }
                return RedirectToAction("Sanpham");
            }
        }


        public ActionResult Chitietdonhang(int id)
        {
            //Lay ra doi tuong sach theo ma
            CHITIETDONTHANG chitietdonhang = db.CHITIETDONTHANGs.SingleOrDefault(n => n.MaDonHang == id);
            ViewBag.Madonhang = chitietdonhang.MaDonHang;
            if (chitietdonhang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chitietdonhang);
        }

    }
}
using Facebook;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;


namespace WebApplication3.Controllers
{
    public class NguoiDungController : Controller
    {

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        dbQLSanPhamDataContext db = new dbQLSanPhamDataContext();

        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        // POST: Hàm Dangky(post) Nhận dữ liệu từ trang Dangky và thực hiện việc tạo mới dữ liệu
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            // Gán các giá tị người dùng nhập liệu cho các biến 
            var hoten = collection.Get("HotenKH");
            var tendn = collection.Get("TenDN");
            var matkhau = collection.Get("Matkhau");
            var matkhaunhaplai = collection.Get("Matkhaunhaplai");
            var diachi = collection.Get("Diachi");
            var email = collection.Get("Email");
            var dienthoai = collection.Get("Dienthoai");
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection.Get("Ngaysinh"));


            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Please fill in your name";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Please enter your username";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Please enter your password";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Please enter your password";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "please email";
            }
             if (String.IsNullOrEmpty(dienthoai))
            {
               
                ViewData["Loi6"] = "Please phone number";
            }
             if (String.IsNullOrEmpty(ngaysinh))
            {
                ViewData["Loi7"] = "Please your birthday";
            }

            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)
                kh.HoTen = hoten;
                kh.Taikhoan = tendn;
                kh.Matkhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachi;
                kh.DienthoaiKH = dienthoai;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
               // kh.Ngaysinh = DateTime.Parse("03/03/1933");
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("DanhNhap");
            }
            return this.Dangky();
        }



        [HttpGet]
        public ActionResult DanhNhap()
        {
            return View();
        }
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });


            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                // Get the user's information, like email, first name, middle name etc
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string HoTen = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var user = new KHACHHANG();
                user.Email = email;
                user.Taikhoan = email;
                user.HoTen = firstname + " " + middlename + " " + lastname;
                var resultInsert = new KhachhangDao().InsertForFacebook(user);
                if (resultInsert > 0)
                {
                   

                        Session["Taikhoan"] = user;

                        return RedirectToAction("Index", "LHTStore");

              
                }
            }
            return Redirect("/");
        }





        [HttpPost]
        public ActionResult DanhNhap(FormCollection collection)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến 
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
           
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Please enter your username";
            }
            else if (String.IsNullOrEmpty(matkhau))
                {
                ViewData["Loi2"] = "Please enter your password";
                }
                else
                {
                    //Gán giá trị cho đối tượng được tạo mới (kh)
                    KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);
                    if (kh != null)
                    {
                       
                          Session["Taikhoan"] = kh;
                          
                          return RedirectToAction("Index", "LHTStore");
                       
                    }
                   else
                        ViewBag.Thongbao = "The username or password is incorrect";
                    }

           

            return View();
        }
 }
}

    
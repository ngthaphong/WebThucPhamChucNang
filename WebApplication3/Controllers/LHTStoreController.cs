using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;
using MVCEmail.Models;
using System.Net;
using System.Net.Mail;
using PagedList;
using PagedList.Mvc;
using System.Threading.Tasks;

namespace WebApplication3.Controllers
{
    public class LHTStoreController : Controller
    {
        // GET: LHTStore
        dbQLSanPhamDataContext data = new dbQLSanPhamDataContext();

        private List<SANPHAM> laySanPhamMoi(int count)
        {
            return data.SANPHAMs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }

        public ActionResult Index(int ? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);

            var SanPhamMoi = laySanPhamMoi(8);
            return View(SanPhamMoi.ToPagedList(pageNum,pageSize));
        }
        public ActionResult Health()
        {
           
            return View();
        }
        public ActionResult Nutrition()
        {

            return View();
        }
        public ActionResult Workout()
        {

            return View();
        }
        public ActionResult Health2()
        {

            return View();
        }
        public ActionResult Health3()
        {

            return View();
        }
        public ActionResult Nutrition2()
        {

            return View();
        }
        public ActionResult Workout2()
        {

            return View();
        }

        public ActionResult Introduce()
        {

            return View();
        }
        public ActionResult Order()
        {

            return View();
        }
        public ActionResult Contact()
        {

            return View();
        }

        public ActionResult Loai()
        {
            var Loai = from cd in data.LOAIs select cd;
            return PartialView(Loai);
        }
        public ActionResult NhaSanxuat()
        {
            var NhaSX = from nsx in data.NHASANXUATs select nsx;
            return PartialView(NhaSX);
        }

        public ActionResult SPTheoLoai(int id ,int? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);

           
           
            var SPtheoLoai = from SPtL in data.SANPHAMs where SPtL.MaLoai==id select SPtL;
            return PartialView(SPtheoLoai.ToPagedList(pageNum, pageSize));
        }
        public ActionResult SPTheoNSX(int id, int? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);

            var SPtheoNSX = from SPtNSX in data.SANPHAMs where SPtNSX.MaNSX == id select SPtNSX;
            return PartialView(SPtheoNSX.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Details(int? id)
        {
            var SP = from s in data.SANPHAMs
                            where s.MaSP == id
                            select s;
            return PartialView(SP.Single());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("hieuhoang1197@gmail.com"));  // replace with valid value 
                message.From = new MailAddress("hieuhoang11977@gmail.com");  // replace with valid value
                message.Subject = "Your email subject";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "hieuhoang11977@gmail.com",  // replace with valid value
                        Password = "dolekieungan"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
        }

        public ActionResult Sent()
        {
            return View();
        }
        




    }
}
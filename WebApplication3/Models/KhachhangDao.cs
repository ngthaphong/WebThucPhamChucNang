using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
namespace WebApplication3.Models
{
    public class KhachhangDao
    {


        public interface ThanhToanTrucTuyen
        {
            string ThanhToanTrucTuyen { get; set; }
        }
        dbQLSanPhamDataContext db = null;
            public KhachhangDao()
            {
                db = new dbQLSanPhamDataContext();
            }
            public long InsertForFacebook(KHACHHANG entity)
            {
                var user = db.KHACHHANGs.SingleOrDefault(x => x.Taikhoan == entity.Taikhoan);
                if (user == null)
                {
                    db.KHACHHANGs.InsertOnSubmit(entity);
                    db.SubmitChanges();
                    return entity.MaKH;
                }
                else
                {
                    return user.MaKH;
                }

            }
        
    }
}
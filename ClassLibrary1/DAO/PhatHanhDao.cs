using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;

namespace Model.DAO
{
    public class PhatHanhDao
    {
        QLVESODbContext db = null;
        public PhatHanhDao()
        {
            db = new QLVESODbContext();
        }
        public IEnumerable<PhatHanh> listAll(int page, int pageSize)
        {
            return db.PhatHanhs.OrderBy(x => x.MaDaiLy).ToPagedList(page, pageSize);
        }
        public string Insert(PhatHanh entity)
        {
            db.PhatHanhs.Add(entity);
            db.SaveChanges();
            return entity.MaDaiLy;
        }
        public PhatHanh GetById(string id)
        {
            return db.PhatHanhs.FirstOrDefault(m => m.MaDaiLy == id);
        }
        public bool Update(PhatHanh pc)
        {
            try
            {
                var tempt = db.PhatHanhs.FirstOrDefault(m=>m.MaDaiLy == pc.MaDaiLy);
                tempt.LoaiVeso = pc.LoaiVeso;
                tempt.HoaHong = pc.HoaHong;
                tempt.MaLoaiVeSo = pc.MaLoaiVeSo;
                tempt.NgayNhan = pc.NgayNhan;
                tempt.SLBan = pc.SLBan;
                tempt.SoLuong = pc.SoLuong;
                tempt.TienThanhToan = pc.TienThanhToan;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Delete(string id)
        {
            try
            {
                var pc = db.PhatHanhs.Find(id);
                db.PhatHanhs.Remove(pc);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public String AutoGetMa()
        {
            var countrow = db.PhieuChis.Count();
            int getcount = countrow + 1;
            string newmahd = "PCH";
            if (getcount < 10) newmahd = newmahd + "000" + getcount.ToString();
            else if (getcount < 100) newmahd = newmahd + "00" + getcount.ToString();
            return newmahd;
        }
    }
}

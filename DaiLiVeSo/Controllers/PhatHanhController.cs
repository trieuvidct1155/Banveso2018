using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EF;

namespace DaiLiVeSo.Controllers
{
    public class PhatHanhController : Controller
    {
        
        // GET: PhatHanh
        public ActionResult Index(int? _Page)
        {
            var dao = new PhatHanhDao();
            int PageNumber = (_Page ?? 1);
            int PageSize = 10;
            return View(dao.listAll(PageNumber, PageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(PhatHanh phatHanh)
        {
            if (ModelState.IsValid)
            {
                var dao = new PhatHanhDao();
                phatHanh.Flag = true;
                decimal slg = TinhToanSLPhatTheoDaiLy(phatHanh.MaDaiLy, phatHanh.MaLoaiVeSo, phatHanh.NgayNhan);
                phatHanh.SoLuong = (int)slg;
                string result = dao.Insert(phatHanh);
                if (result != null)
                {
                    return RedirectToAction("Index", "PhatHanh");
                }
            }
            else
            {
                //return RedirectToAction("Create", "PhatHanh");
                ModelState.AddModelError("", "Thêm loại vé mới không thành công");
            }
            return View("Index");
        }



        [HttpGet]
        public ActionResult Edit(string id)
        {
            var loai = new PhatHanhDao().GetById(id);

            return View(loai);
        }

        [HttpPost]
        public ActionResult Edit(PhatHanh loai)
        {
            if (ModelState.IsValid)
            {
                loai.Flag = true;
                var dao = new PhatHanhDao();
                var result = dao.Update(loai);
                if (result)
                {
                    return RedirectToAction("Index", "LoaiVeSo");
                }
            }
            else
            {
                ModelState.AddModelError("", "Chỉnh sửa giải không thành công");
            }
            return View("Index");
        }

        public ActionResult Delete(string id)
        {
            new PhatHanhDao().Delete(id);
            return RedirectToAction("Index");

        }

        public decimal TinhToanSLPhatTheoDaiLy(string MaDaiLy, string MaLoaiVeSo, System.DateTime NgayNhan)
        {
            var dao =  new QLVESODbContext();
            decimal SLDK = dao.SoLuongDKs.OrderByDescending(m => m.NgayDK).Where(m => m.MaDaiLy == MaDaiLy & System.DateTime.Compare(m.NgayDK, NgayNhan) <= 0).Select(m => (int) m.SoLuongDK1).FirstOrDefault();
            System.DateTime NgayDK = dao.SoLuongDKs.OrderByDescending(m => m.NgayDK).Where(m => m.MaDaiLy == MaDaiLy & System.DateTime.Compare(m.NgayDK, NgayNhan) <= 0).Select(m => m.NgayDK).FirstOrDefault();
            var listTop3 = dao.PhatHanhs.Where(m => m.MaDaiLy == MaDaiLy & System.DateTime.Compare(m.NgayNhan, NgayNhan) <= 0 & m.SLBan != null).OrderByDescending(m => m.NgayNhan).Take(3);
            int count = listTop3.Count();
            if (count == 0)
            {
                return SLDK;
            }
            else
            {
                decimal? sum = 0;
                foreach (var item in listTop3)
                {
                    sum += item.SLBan / item.SoLuong;
                }
                decimal? getReturn = Math.Round((decimal)sum / count * SLDK);
                return getReturn ?? default(decimal);
            }
        }

    }
}
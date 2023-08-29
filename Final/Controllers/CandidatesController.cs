using Final.Models;
using Final.Repositories;
using Final.Repositories.Interfaces;
using Final.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace Final.Controllers
{
   // [Authorize]
    public class CandidatesController : Controller
    {
        private readonly CandidateDbContext db = new CandidateDbContext();
        IGenericRepo<Candidate> repo;
        public CandidatesController()
        {
            this.repo = new GenericRepo<Candidate>(db);
        }
        // GET: Candidates
        [AllowAnonymous]
        public ActionResult Index(int pg = 1)
        {
            var data = this.repo.GetAll("Qualifications").ToPagedList(pg, 5);
            //var data = await db.Candidates.OrderBy(a => a.Id).ToPagedListAsync(pg, 5);
            return View(data);
        }
        public ActionResult Create()
        {

            CandidateViewModel c = new CandidateViewModel();
            c.Qualifications.Add(new Qualification { });
            return View(c);
        }
        [HttpPost]
        public ActionResult Create(CandidateViewModel data, string act = "")
        {
            if (act == "add")
            {
                data.Qualifications.Add(new Qualification { });

                foreach (var item in ModelState.Values)
                {
                    item.Errors.Clear();
                }
            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                data.Qualifications.RemoveAt(index);
                foreach (var item in ModelState.Values)
                {
                    item.Errors.Clear();
                }
            }
            if (act == "insert")
            {
                if (ModelState.IsValid)
                {
                    var b = new Candidate
                    {
                        CandidateName = data.CandidateName,
                        BirthDate = data.BirthDate,
                        AppliedFor = (Models.AppliedFor)data.AppliedFor,
                        ExpectedSalary = data.ExpectedSalary,
                        Conditions = data.Conditions,
                    };
                    string ext = Path.GetExtension(data.Picture.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Server.MapPath("~/Pictures/") + fileName;
                    data.Picture.SaveAs(savePath);
                    b.Picture = fileName;
                    foreach (var l in data.Qualifications)
                    {

                        b.Qualifications.Add(l);
                    }
                    this.repo.Insert(b);
                }
            }
            ViewBag.Act = act;
            return PartialView("_CreatePartial", data);
        }
        public ActionResult Edit(int id)
        {
            var x = this.repo.Get(id, "Qualifications"); ;
            var c = new CandidateEditModel
            {
                Id = x.Id,
                CandidateName = x.CandidateName,
                AppliedFor = (ViewModels.AppliedFor)x.AppliedFor,
                ExpectedSalary = x.ExpectedSalary,
                Conditions = x.Conditions,
                BirthDate = x.BirthDate,
                Qualifications = x.Qualifications.ToList()

            };
            ViewData["CurrentPic"] =x.Picture;
            return View(c);

        }
        [HttpPost]
        public ActionResult Edit(CandidateEditModel data, string act = "")
        {
            if (act == "add")
            {
                data.Qualifications.Add(new Qualification { });
                foreach (var item in ModelState.Values)
                {
                    item.Errors.Clear();
                }
            }

            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                data.Qualifications.RemoveAt(index);
                foreach (var item in ModelState.Values)
                {
                    item.Errors.Clear();
                }
            }

            if (act == "update")
            {
                if (ModelState.IsValid)
                {
                    var c = db.Candidates.First(x => x.Id == data.Id);

                    c.CandidateName = data.CandidateName;
                    c.BirthDate = data.BirthDate;
                    c.AppliedFor = (Models.AppliedFor)data.AppliedFor;
                    c.ExpectedSalary = data.ExpectedSalary;
                    c.Conditions = data.Conditions;
                   
                    
                   
                    if (data.Picture != null)
                    {
                        string ext = Path.GetExtension(data.Picture.FileName);
                        string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                        string savePath = Server.MapPath("~/Pictures/") + fileName;
                        data.Picture.SaveAs(savePath);
                        c.Picture = fileName;
                    }
                    c.Qualifications.Clear();
                    this.repo.ExecuteCommand($"DELETE FROM Qualifications WHERE CandidateId={c.Id}");
                    this.repo.Update(c);
                    
                    foreach (var item in data.Qualifications)
                    {
                        this.repo.ExecuteCommand($@"INSERT INTO Qualifications([Degree],[PassingYear],[Institute],[Result],[CandidateId]) VALUES('{item.Degree}', {item.PassingYear},'{item.Institute}', '{item.Result}', {c.Id})");
                    }
                  
                   
                    return RedirectToAction("Index");
                }

            }
            ViewData["CurrentPic"] = db.Candidates.First(x => x.Id == data.Id).Picture;
            return View(data);

        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            this.repo.ExecuteCommand($"dbo.DeleteCandidate {id}");
            return Json(new { success = true, deleted = id });
        }
    }
}
    

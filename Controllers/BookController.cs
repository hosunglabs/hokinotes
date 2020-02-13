using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;
using hosungnotes.Models;
using hosungnotes.App_Code;
using System.Threading.Tasks;

namespace hosungnotes.Controllers
{
    public class BookController : Controller
    {
        [GWAuthorize]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [GWAuthorize]
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Write(string txtLabel, string txtSubject, string txtComment)
        {
            BoardDispatch boardDispatch = new BoardDispatch();
            int seq = boardDispatch.InsertBoard(txtLabel, txtSubject, txtComment);
            return RedirectToAction("Detail", "Book", new { Seq = seq, txtSearch = "", hidPageSize = 1 });
        }

        [GWAuthorize]
        public ActionResult Detail(int seq, string txtSearch, int hidPageSize)
        {
            ViewBag.SearchOption = new SearchOptionView { TxtSearch = txtSearch,  PageSize = hidPageSize };

            BoardT boardT = GetDetail(seq,1);

            return View(boardT);
        }

        [GWAuthorize]
        [HttpGet]
        public ActionResult Modify(int seq, string txtSearch, int hidPageSize)
        {
            ViewBag.SearchOption = new SearchOptionView { TxtSearch = txtSearch, PageSize = hidPageSize };

            BoardT boardT = GetDetail(seq,0);

            return View(boardT);
        }

        private BoardT GetDetail(int seq, int option)
        {
            BoardDispatch boardDispatch = new BoardDispatch();
            DataSet data = boardDispatch.GetBoardDetail(seq);

            BoardT boardT = new BoardT
            {
                Comment = option == 1 ? data.Tables[0].Rows[0]["Comment"].ToString().Replace("\n", "<br/>") : data.Tables[0].Rows[0]["Comment"].ToString(),
                LabelName = data.Tables[0].Rows[0]["LabelName"].ToString(),
                RegDate = (DateTime)data.Tables[0].Rows[0]["RegDate"],
                Seq = (int)data.Tables[0].Rows[0]["Seq"],
                Subject = data.Tables[0].Rows[0]["Subject"].ToString()
            };

            return boardT;
        }

        [GWAuthorize]
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Modify(int seq, string txtLabel, string txtSubject, string txtComment, string txtSearch, int hidPageSize)
        {
            BoardDispatch boardDispatch = new BoardDispatch();
            int returnVal = boardDispatch.UpdateBoard(seq, txtLabel, txtSubject, txtComment);
            return RedirectToAction("Detail", "Book", new { Seq = seq, txtSearch = txtSearch, hidPageSize = hidPageSize });
        }

        [GWAuthorize]
        public ActionResult Delete(int seq, string txtSearch, int hidPageSize)
        {
            BoardDispatch boardDispatch = new BoardDispatch();
            int retVal = boardDispatch.DeleteBoard(seq);
            return RedirectToAction("Main", "Search",new { txtSearch = txtSearch, hidPageSize = hidPageSize });
        }

        [HttpPost]
        public JsonResult UploadFile()
        {
            bool retVal = false;
            string retMsg = String.Empty;

            if (Request.Files.Count > 0)
            {
                try
                {
                    string fileName = Request.Files[0].FileName;

                    string imgFileName = String.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), fileName.Substring(fileName.LastIndexOf(".")));
                    string path = System.Configuration.ConfigurationManager.AppSettings["UploadFolder"] + imgFileName;

                    Request.Files[0].SaveAs(path);
                    retMsg = imgFileName;
                    retVal = true;
                }
                catch(Exception ex)
                {
                    retMsg = ex.Message;
                }                
            }

            return Json(new { isSuccess = retVal, msg= retMsg });
        }
    }
}
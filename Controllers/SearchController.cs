using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hosungnotes.Models;

using System.Data;
using hosungnotes.App_Code;

namespace hosungnotes.Controllers
{
    public class SearchController : Controller
    {
        const int PAGECOUNT = 10;

        [GWAuthorize]
        public ActionResult Index()
        {
            BoardDispatch boardDispatch = new BoardDispatch();

            //라벨리스트
            DataSet dataLabel = boardDispatch.GetLabelList(6);
            ViewBag.LabelList = dataLabel.Tables[0].Rows;

            return View();
        }

        [GWAuthorize]
        public ActionResult Main(string txtSearch,string hidPageSize)
        {
            string searchValue = txtSearch == null ? "" : txtSearch;

            SearchOptionView searchOptionView = new SearchOptionView();
            searchOptionView.TxtSearch = searchValue;
            ViewBag.SearchOptionView = searchOptionView;


            int pageSize = String.IsNullOrWhiteSpace(hidPageSize) ? 1 : Convert.ToInt32(hidPageSize);
            int startCount = ((pageSize - 1) * PAGECOUNT) + 1;
            int endCount = pageSize * PAGECOUNT;

            BoardDispatch boardDispatch = new BoardDispatch();
            DataSet data = boardDispatch.GetBoardList(searchValue, startCount, endCount);

            List<BoardT> boardResults = new List<BoardT>();

            foreach(DataRow row in data.Tables[0].Rows)
                boardResults.Add(new BoardT
                {
                    Comment = row["Comment"].ToString(),
                    LabelName = row["LabelName"].ToString(),
                    RegDate = (DateTime)row["RegDate"],
                    Seq = (int)row["Seq"],
                    Subject = row["Subject"].ToString()
                });

            //Paging 처리
            int totalCount = Convert.ToInt32(data.Tables[1].Rows[0][0]);
            var pager = new Pager(totalCount, pageSize, PAGECOUNT);
            ViewBag.Pagination = pager;

            //라벨리스트
            DataSet dataLabel = boardDispatch.GetLabelList(15);
            ViewBag.LabelList = dataLabel.Tables[0].Rows;

            return View("Main", new SearchResult { SearchText = searchValue, IsValid = true, BoardTotalCount = totalCount, lstBoard = boardResults, ServerError = "" });
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using HTSql;

namespace hosungnotes.App_Code
{
    public class BoardDispatch : BizBase
    {
        public BoardDispatch()
        {
            this.SqlConnectString = System.Configuration.ConfigurationManager.AppSettings["SqlConn"];
        }

        public BoardDispatch(string sqlConnString)
            : base(sqlConnString)
        {
        }
  
        public DataSet GetBoardList(string searchKeyword, int start, int end)
        {
            string sqlStr = @"
                                Select
                                Seq
                                ,Subject
                                ,LabelNAme
                                ,Comment
                                ,RegDate
                                from
                                (select ROW_NUMBER() over(order by Seq desc) rownum,* from Board
                                where Subject like '%' + @searchKeyword + '%'
                                or LabelName like  '%' + @searchKeyword + '%'
                                or Comment like  '%' + @searchKeyword + '%'
                                ) B 
                                where rownum between @start and @end 
                                
                                select count(*) from Board
                                where Subject like '%' + @searchKeyword + '%'
                                or LabelName like  '%' + @searchKeyword + '%'
                                or Comment like  '%' + @searchKeyword + '%' ";

            base.DBConnection(false);
            DataSet ds = this.NTX_GetDataSet(sqlStr, CType.Query
                , new SqlParameter("@searchKeyword", searchKeyword)
                , new SqlParameter("@start", start)
                , new SqlParameter("@end", end)
                );

            base.DBConnClose();
            return ds;
        }


        public DataSet GetBoardDetail(int seq)
        {
            string sqlStr;

            sqlStr = @"select 
                         Seq
                        ,Subject
                        ,LabelNAme
                        ,Comment
                        ,RegDate
                        from Board a
                        where Seq = @Seq";        

            base.DBConnection(false);
            DataSet ds = this.NTX_GetDataSet(sqlStr, CType.Query , new SqlParameter("@Seq", seq));
            base.DBConnClose();
            return ds;
        }

        public DataSet GetLabelList(int top)
        {
            string sqlStr;

            sqlStr = @" select top " + top + @" LabelName
                         from board aa
                         , (  select max(RegDate) as d
                          from board
                          group by LabelName) bb 
                         where 1 = 1
                         and aa.RegDate = bb.d order by bb.d desc";

            base.DBConnection(false);
            DataSet ds = this.NTX_GetDataSet(sqlStr, CType.Query);
            base.DBConnClose();
            return ds;
        }

       

        public int InsertBoard(string label, string subject, string comment)
        {
            string sqlStr = @"insert into Board (
                                                    LabelName
                                                    ,Subject
                                                    ,Comment
                                                    ) values (
                                                    @LabelName
                                                    ,@Subject
                                                    ,@Comment)
                               select @@Identity";

            base.DBConnection(false);

            object retVal = this.NTX_GetScalarData(sqlStr, CType.Query
                                , new SqlParameter("@LabelName", label)
                                , new SqlParameter("@Subject", subject)
                                , new SqlParameter("@Comment", comment));            
            base.DBConnClose();

            return Convert.ToInt32(retVal);
        }

        public int DeleteBoard(int seq)
        {
            base.DBConnection(false);

            int retVal = this.NTX_ExeNonSPQuery("Delete Board where Seq = @seq", CType.Query
                                , new SqlParameter("@seq", seq));
            base.DBConnClose();

            return retVal;
        }

        public int UpdateBoard(int seq, string labelName, string subject, string comment)
        {
            string sqlStr = @"Update Board Set
                                                    labelName = @labelName
                                                    ,subject = @subject
                                                    ,comment = @comment
                                                    Where seq = @seq";

            base.DBConnection(false);

            int retVal = this.NTX_ExeNonSPQuery(sqlStr, CType.Query
                                , new SqlParameter("@labelName", labelName)
                                , new SqlParameter("@subject", subject)
                                , new SqlParameter("@comment", comment)
                                , new SqlParameter("@seq", seq));
            base.DBConnClose();

            return retVal;
        }

       
    }
}
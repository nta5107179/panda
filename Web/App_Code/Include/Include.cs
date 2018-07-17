using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CoreClass;
using Model;

namespace Web.App_Code.Include
{
    public class Include : AppCode
    {
        GetModelList m_gml = new GetModelList();

        /*
        ===========================================
        留言
        ===========================================
        */
        #region 留言
        /// <summary>
        /// 添加留言
        /// </summary>
        /// <param name="mod">mod</param>
        public bool AddMessage(g_message mod)
        {
            bool b = false;
            OpSql.Open();
            try
            {
                b = OpSql.Insert(new object[] { mod });
            }
            catch { }
            finally { OpSql.Close(); }
            return b;
        }
        /// <summary>
        /// 查询IP地址今日是否已留言
        /// </summary>
        public bool HasMessageToday()
        {
            bool b = false;
            DataSet ds = null;
            OpSql.Open();
            try
            {
                string sql = string.Format("select * from g_message where date(m_addtime)=CURDATE() and m_ip='{0}'", OpMemory.IPAddress);
                ds = OpSql.Select(sql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    b = true;
                }
            }
            catch { }
            finally { OpSql.Close(); }
            return b;
        }
        #endregion
        /*
        ===========================================
        信息
        ===========================================
        */
        #region 信息
        /// <summary>
		/// 获取信息列表
		/// </summary>
		/// <returns></returns>
		public List<Models> GetNewsList(string page, string limit, ref long total, string nt_id)
        {
            List<Models> list = new List<Models>();
            DataSet ds = null;
            OpSql.Open();
            try
            {
                string where = "";
                if (!string.IsNullOrEmpty(nt_id))
                {
                    where += string.Format(" and t1.nt_id = {0}", nt_id);
                }
                string sql = string.Format(@"
select SQL_CALC_FOUND_ROWS * from g_news as t1
left join g_newstype as t2 on t1.nt_id=t2.nt_id
where 1=1 and n_examine=1{2}
order by t1.nt_id asc, t1.n_top desc, t1.n_id asc
limit {0},{1};
select CAST(FOUND_ROWS() as SIGNED) as total;
                    ", (int.Parse(page) - 1) * int.Parse(limit), int.Parse(limit), where);
                ds = OpSql.Select(sql);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<g_news> list1 = m_gml.g_news(ds.Tables[0]);
                    List<g_newstype> list2 = m_gml.g_newstype(ds.Tables[0]);
                    for (int i = 0; i < list1.Count; i++)
                    {
                        Models mod = new Models();
                        mod.g_news = list1[i];
                        mod.g_newstype = list2[i];
                        list.Add(mod);
                    }
                    total = (long)ds.Tables[1].Rows[0]["total"];
                }
            }
            catch { }
            finally { OpSql.Close(); }
            return list;
        }
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <returns></returns>
        public Models GetNews(string nt_id, string n_id)
        {
            Models detail = new Models();
            DataSet ds = null;
            OpSql.Open();
            try
            {
                string sql = string.Format(@"
select * from g_news
where nt_id={0} and n_id={1}
                    ", nt_id, n_id);
                ds = OpSql.Select(sql);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<g_news> list1 = m_gml.g_news(ds.Tables[0]);
                    if (list1.Count > 0)
                    {
                        detail.g_news = list1[0];
                    }
                }
            }
            catch { }
            finally { OpSql.Close(); }
            return detail;
        }
        #endregion
    }
}

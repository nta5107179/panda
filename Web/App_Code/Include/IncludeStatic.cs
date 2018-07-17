using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CoreClass;

namespace Web.App_Code.Include
{
    public static class IncludeStatic
    {
        static OperateSqlClass m_sql = new OperateSqlClass();
        static GetModelList m_gml = new GetModelList();

        /// <summary>
		/// 获取信息类型列表
		/// </summary>
		/// <returns></returns>
		public static List<Models> GetNewsTypeList(string nt_pid)
        {
            List<Models> list = new List<Models>();
            DataSet ds = null;
            m_sql.Open();
            try
            {
                string sql = string.Format(@"
select * from g_newstype
where nt_examine=1 and nt_pid={0}
order by nt_pid asc, nt_top desc, nt_id asc
                    ", nt_pid);
                ds = m_sql.Select(sql);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<g_newstype> list1 = m_gml.g_newstype(ds.Tables[0]);
                    for (int i = 0; i < list1.Count; i++)
                    {
                        Models mod = new Models();
                        mod.g_newstype = list1[i];
                        list.Add(mod);
                    }
                }
            }
            catch { }
            finally { m_sql.Close(); }
            return list;
        }
    }
}

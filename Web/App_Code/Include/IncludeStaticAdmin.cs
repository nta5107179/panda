using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CoreClass;

namespace Web.App_Code.Include
{
    public static class IncludeStaticAdmin
    {
        static OperateSqlClass m_sql = new OperateSqlClass();
        static GetModelList m_gml = new GetModelList();

        /// <summary>
        /// 获取新消息数量
        /// </summary>
        /// <returns></returns>
        public static long MessageNumber
        {
            get
            {
                long number = 0;
                DataSet ds = null;
                m_sql.Open();
                try
                {
                    string sql = string.Format(@"
select count(1) as number from g_message
where m_isread=0
                    ");
                    ds = m_sql.Select(sql);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        number = (long)ds.Tables[0].Rows[0]["number"];
                    }
                }
                catch { }
                finally { m_sql.Close(); }
                return number;
            }
        }
    }
}

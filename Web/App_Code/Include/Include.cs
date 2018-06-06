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
		/*
        ===========================================
        留言
        ===========================================
        */
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
	}
}

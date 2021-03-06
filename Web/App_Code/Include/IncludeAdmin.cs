﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreClass;
using Model;

namespace Web.App_Code.Include
{
    public class IncludeAdmin : AppCode
	{
		GetModelList m_gml = new GetModelList();
        
        /*
		===========================================
		全局Cookie
		===========================================
		*/
        #region 全局Cookie
        /// <summary>
        /// 添加管理员登录Cookie
        /// </summary>
        /// <param name="s_id">管理员流水号</param>
        /// <param name="s_name">管理员用户名</param>
        /// <param name="isremember">是否记住</param>
        bool CreateCookie_Login(string a_id, string a_uname, bool isremember)
		{
			bool b = true;
			b = isremember? OpMemory.SetCookie("a_id", a_id, 365, 0, 0, 0, null): OpMemory.SetCookie("a_id", a_id, null);
			b = isremember ? OpMemory.SetCookie("a_uname", WebUtility.UrlEncode(a_uname), 365, 0, 0, 0, null): OpMemory.SetCookie("a_uname", WebUtility.UrlEncode(a_uname), null);
			return b;
		}
		/// <summary>
		/// 移除管理员登录Cookie
		/// </summary>
		bool RemoveCookie_Login()
		{
			bool b = true;
			b = OpMemory.RemoveCookie("a_id", null);
			b = OpMemory.RemoveCookie("a_uname", null);
			return b;
		}
		/// <summary>
		/// 获取Cookie中的管理员流水号
		/// </summary>
		public string GetCookieA_id
		{
			get
			{
				return OpMemory.GetCookie("a_id");
			}
		}
		/// <summary>
		/// 获取Cookie中的管理员名
		/// </summary>
		public string GetCookieA_uname
		{
			get
			{
				return WebUtility.UrlDecode(OpMemory.GetCookie("a_uname"));
			}
		}
		#endregion
		/*
		===========================================
		登录模块
		===========================================
		*/
		#region 管理员登录模块
		/// <summary>
		/// 检测管理员是否登录
		/// </summary>
		/// <returns></returns>
		public bool IsLogin
		{
			get
			{
				bool b = false;
				if (GetCookieA_id != null && GetCookieA_uname != null)
				{
					b = true;
				}
				return b;
			}
		}
		/// <summary>
		/// 管理员登录
		/// </summary>
		/// <param name="a_uname">管理员用户名</param>
		/// <param name="a_pwd">管理员密码(明文)</param>
		/// <param name="isremember">是否记住</param>
		public bool Login(string a_uname, string a_pwd, bool isremember)
		{
			bool b = false;
			OpSql.Open();
			try
			{
				string sql = string.Format("select * from G_Admin where a_uname='{0}' and a_pwd='{1}'", a_uname, OpString.DESEncryption(a_pwd));
				DataSet ds = OpSql.Select(sql);
				if (ds != null && ds.Tables.Count > 0)
				{
					DataTable dt = ds.Tables[0];
					if (dt.Rows.Count > 0)
					{
						if (CreateCookie_Login(dt.Rows[0]["a_id"].ToString(), dt.Rows[0]["a_uname"].ToString(), isremember))
						{
							b = true;
						}
					}
				}
			}
			catch { }
			finally { OpSql.Close(); }
			return b;
		}
		/// <summary>
		/// 管理员注销
		/// </summary>
		public void unLogin()
		{
			RemoveCookie_Login();
		}
		#endregion
		/*
		===========================================
		信息类型模块
		===========================================
		*/
		#region 信息类型模块
		/// <summary>
		/// 获取信息类型列表
		/// </summary>
		/// <returns></returns>
		public List<Models> GetNewsTypeList()
		{
			List<Models> list = new List<Models>();
			DataSet ds = null;
			OpSql.Open();
			try
			{
				string sql = string.Format(@"
select * from g_newstype
order by nt_pid asc, nt_top desc, nt_id asc
                    ");
				ds = OpSql.Select(sql);
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
			finally { OpSql.Close(); }
			return list;
		}
		/// <summary>
		/// 获取信息类型列表
		/// </summary>
		/// <returns></returns>
		public List<Models> GetNewsTypeList(string page, string limit, ref long total, string nt_name, string nt_pid, string nt_examine)
		{
			List<Models> list = new List<Models>();
			DataSet ds = null;
			OpSql.Open();
			try
			{
				string where = "";
				if (!string.IsNullOrEmpty(nt_name))
				{
					where += string.Format(" and t1.nt_name like '%{0}%'", nt_name);
				}
				if (!string.IsNullOrEmpty(nt_pid))
				{
					where += string.Format(" and t1.nt_pid = {0}", nt_pid);
				}
				if (!string.IsNullOrEmpty(nt_examine))
				{
					where += string.Format(" and t1.nt_examine = {0}", nt_examine);
				}
				string sql = string.Format(@"
select SQL_CALC_FOUND_ROWS t1.*,ifnull(t2.nt_name, '-') as nt_pname from g_newstype as t1
left join g_newstype as t2 on t1.nt_pid=t2.nt_id
where 1=1{2}
order by t1.nt_pid asc, t1.nt_top desc, t1.nt_id asc
limit {0},{1};
select CAST(FOUND_ROWS() as SIGNED) as total;
                    ", (int.Parse(page) - 1) * int.Parse(limit), int.Parse(limit), where);
				ds = OpSql.Select(sql);
				if (ds != null && ds.Tables.Count > 0)
				{
					List<g_newstype> list1 = m_gml.g_newstype(ds.Tables[0]);
					for (int i = 0; i < list1.Count; i++)
					{
						Models mod = new Models();
						mod.g_newstype = list1[i];
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
		/// 获取信息类型
		/// </summary>
		/// <returns></returns>
		public Models GetNewsType(string nt_id)
		{
			Models detail = new Models();
			DataSet ds = null;
			OpSql.Open();
			try
			{
				string sql = string.Format(@"
select * from g_newstype
where nt_id={0}
                    ", nt_id);
				ds = OpSql.Select(sql);
				if (ds != null && ds.Tables.Count > 0)
				{
					List<g_newstype> list1 = m_gml.g_newstype(ds.Tables[0]);
					if(list1.Count > 0)
					{
						detail.g_newstype = list1[0];
					}
				}
			}
			catch { }
			finally { OpSql.Close(); }
			return detail;
		}
		/// <summary>
		/// 添加信息类型
		/// </summary>
		/// <returns></returns>
		public bool AddNewsType(g_newstype mod)
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
		/// 修改信息类型
		/// </summary>
		/// <returns></returns>
		public bool EditNewsType(g_newstype mod, g_newstype mod2)
		{
			bool b = false;
			OpSql.Open();
			try
			{
				b = OpSql.Update(new object[] { mod }, new object[] { mod2 });
			}
			catch { }
			finally { OpSql.Close(); }
			return b;
		}
		/// <summary>
		/// 删除信息类型
		/// </summary>
		/// <returns></returns>
		public bool DelNewsType(g_newstype mod)
		{
			bool b = false;
			OpSql.Open();
			try
			{
				b = OpSql.Delete(new object[] { mod });
			}
			catch { }
			finally { OpSql.Close(); }
			return b;
		}
		#endregion
		/*
		===========================================
		信息模块
		===========================================
		*/
		#region 信息模块
		/// <summary>
		/// 获取信息列表
		/// </summary>
		/// <returns></returns>
		public List<Models> GetNewsList()
		{
			List<Models> list = new List<Models>();
			DataSet ds = null;
			OpSql.Open();
			try
			{
				string sql = string.Format(@"
select * from g_news
order by nt_id asc, n_top desc, n_id asc
                    ");
				ds = OpSql.Select(sql);
				if (ds != null && ds.Tables.Count > 0)
				{
					List<g_news> list1 = m_gml.g_news(ds.Tables[0]);
					for (int i = 0; i < list1.Count; i++)
					{
						Models mod = new Models();
						mod.g_news = list1[i];
						list.Add(mod);
					}
				}
			}
			catch { }
			finally { OpSql.Close(); }
			return list;
		}
		/// <summary>
		/// 获取信息列表
		/// </summary>
		/// <returns></returns>
		public List<Models> GetNewsList(string page, string limit, ref long total, string n_title, string nt_id, string n_examine)
		{
			List<Models> list = new List<Models>();
			DataSet ds = null;
			OpSql.Open();
			try
			{
				string where = "";
				if (!string.IsNullOrEmpty(n_title))
				{
					where += string.Format(" and t1.n_title like '%{0}%'", n_title);
				}
				if (!string.IsNullOrEmpty(nt_id))
				{
					where += string.Format(" and t1.nt_id = {0}", nt_id);
				}
				if (!string.IsNullOrEmpty(n_examine))
				{
					where += string.Format(" and t1.n_examine = {0}", n_examine);
				}
				string sql = string.Format(@"
select SQL_CALC_FOUND_ROWS * from g_news as t1
left join g_newstype as t2 on t1.nt_id=t2.nt_id
where 1=1{2}
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
		/// <summary>
		/// 添加信息
		/// </summary>
		/// <returns></returns>
		public bool AddNews(g_news mod)
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
		/// 修改信息
		/// </summary>
		/// <returns></returns>
		public bool EditNews(g_news mod, g_news mod2)
		{
			bool b = false;
			OpSql.Open();
			try
			{
				b = OpSql.Update(new object[] { mod }, new object[] { mod2 });
			}
			catch { }
			finally { OpSql.Close(); }
			return b;
		}
		/// <summary>
		/// 删除信息
		/// </summary>
		/// <returns></returns>
		public bool DelNews(g_news mod)
		{
			bool b = false;
			OpSql.Open();
			try
			{
				b = OpSql.Delete(new object[] { mod });
			}
			catch { }
			finally { OpSql.Close(); }
			return b;
		}
        #endregion
        /*
		===========================================
		留言模块
		===========================================
		*/
        #region 留言模块
        /// <summary>
        /// 获取留言列表
        /// </summary>
        /// <returns></returns>
        public List<Models> GetMessageList(string page, string limit, ref long total)
        {
            List<Models> list = new List<Models>();
            DataSet ds = null;
            OpSql.Open();
            try
            {
                string sql = string.Format(@"
select SQL_CALC_FOUND_ROWS * from g_message
order by m_isread asc, m_id desc
limit {0},{1};
select CAST(FOUND_ROWS() as SIGNED) as total;
                    ", (int.Parse(page) - 1) * int.Parse(limit), int.Parse(limit));
                ds = OpSql.Select(sql);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<g_message> list1 = m_gml.g_message(ds.Tables[0]);
                    for (int i = 0; i < list1.Count; i++)
                    {
                        Models mod = new Models();
                        mod.g_message = list1[i];
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
        /// 获取留言
        /// </summary>
        /// <returns></returns>
        public Models GetMessage(string m_id)
        {
            Models detail = new Models();
            DataSet ds = null;
            OpSql.Open();
            try
            {
                string sql = string.Format(@"
select * from g_message
where m_id={0}
                    ", m_id);
                ds = OpSql.Select(sql);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<g_message> list1 = m_gml.g_message(ds.Tables[0]);
                    if (list1.Count > 0)
                    {
                        detail.g_message = list1[0];
                    }
                }
            }
            catch { }
            finally { OpSql.Close(); }
            return detail;
        }
        /// <summary>
        /// 修改留言
        /// </summary>
        /// <returns></returns>
        public bool EditMessage(g_message mod, g_message mod2)
        {
            bool b = false;
            OpSql.Open();
            try
            {
                b = OpSql.Update(new object[] { mod }, new object[] { mod2 });
            }
            catch { }
            finally { OpSql.Close(); }
            return b;
        }
        /// <summary>
        /// 删除留言
        /// </summary>
        /// <returns></returns>
        public bool DelMessage(g_message mod)
        {
            bool b = false;
            OpSql.Open();
            try
            {
                b = OpSql.Delete(new object[] { mod });
            }
            catch { }
            finally { OpSql.Close(); }
            return b;
        }
        #endregion
    }
}

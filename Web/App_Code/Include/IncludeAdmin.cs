using System;
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
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using CoreClass;
using System.Net;
using System.IO;

/// <summary>
/// 公共方法库
/// </summary>
public class AppCode : Factory
{
	/// <summary>
	/// 初始化AppCode类型的新实例
	/// </summary>
	public AppCode()
	{
		
	}
	/// <summary>
	/// 未登录跳转
	/// </summary>
	public void Redirect()
	{
		HttpContext.Current.Response.Redirect(Config.g_root + "Login.aspx");
	}
	/// <summary>
	/// 未登录跳转
	/// </summary>
	public void RedirectAdmin()
	{
		HttpContext.Current.Response.Redirect(Config.g_root + "Admin/Login.aspx");
	}
	
}
using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

/// <summary>
/// 全局配置静态类
/// </summary>
public static class Config
{

	#region 静态配置变量
	/// <summary>
	/// 根目录
	/// </summary>
	public static string g_root = GetWebconfig("g_root");
	/// <summary>
	/// 文件目录
	/// </summary>
    public static string g_filepath = GetWebconfig("g_filepath");

	/// <summary>
	/// 网站标题
	/// </summary>
	public static string g_title = GetWebconfig("g_title");
	/// <summary>
	/// 首页标题
	/// </summary>
	public static string g_indextitle = GetWebconfig("g_indextitle");
	/// <summary>
	/// 网站关键字设置
	/// </summary>
	public static string g_keywords = GetWebconfig("g_keywords");
	/// <summary>
	/// 网站说明设置
	/// </summary>
	public static string g_description = GetWebconfig("g_description");

	/// <summary>
	/// 非法字符
	/// </summary>
	public static string g_illegal = ",|\"|'";
	/// <summary>
	/// 敏感字符
	/// </summary>
	public static string g_words = null;
	/// <summary>
	/// 允许上传图片类型
	/// </summary>
    public static string g_img = GetWebconfig("g_img");
	/// <summary>
	/// 允许上传文件类型
	/// </summary>
    public static string g_file = GetWebconfig("g_file");
	/// <summary>
	/// 允许上传图片最大大小(单位 B)
	/// </summary>
    public static int g_imgmax = int.Parse(GetWebconfig("g_imgmax"));
	/// <summary>
	/// 允许上传文件最大大小(单位 B)
	/// </summary>
    public static int g_filemax = int.Parse(GetWebconfig("g_filemax"));

	#endregion

	#region 读取appsettings.json配置
	/// <summary>
	/// 获取web.config文件appSettings配置节中的Add里的value属性
	/// </summary>
	public static string GetWebconfig(string key)
	{
		IConfigurationBuilder builder = new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json");
		IConfiguration Configuration = builder.Build();

		return Configuration[key];
	}
	#endregion
	

}
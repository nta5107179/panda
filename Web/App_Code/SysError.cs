using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// 系统错误公共类
/// </summary>
public static class SysError
{
	static Dictionary<string, string> dic = new Dictionary<string, string>();

	/// <summary>
	/// 错误提示配置
	/// </summary>
	static void SetError()
	{
		if (dic.Count <= 0)
		{
			//通用
			//异常
			dic.Add("ERROR000", "无系统错误！");
			dic.Add("ERROR001", "系统错误！");
			//非法字符
			dic.Add("SYS000", "传入数据正常");
			dic.Add("SYS001", "系统检测到非法字符，请使用全角字符代替，以下字符为非法字符 " + Config.g_illegal.Replace("'", "\\'").Replace("|", " "));
			dic.Add("SYS002", "参数错误，请按照要求输入！");
			//上传图片
			dic.Add("IMG000", "图片上传成功！");
			dic.Add("IMG001", "图片上传失败，请上传（格式：" + Config.g_imgtype + "）（大小：" + Config.g_imgmax / 1024 + "KB内）的图片！");

			//用户端用
			//留言
			dic.Add("MESSAGE000", "系统已收到，感谢您的留言！");
			dic.Add("MESSAGE001", "服务器繁忙，请稍后再试！");
			dic.Add("MESSAGE002", "同一IP每天只能留言一次，今天已经感谢过您了哦");


			//管理后台用
			//登录
			dic.Add("LOGIN000", "登录成功！");
			dic.Add("LOGIN001", "用户名或密码错误，登录失败！");
			dic.Add("LOGIN002", "帐号已过期，登录失败！");
			dic.Add("LOGIN003", "登录超时，请重新登录！");
			//添加
			dic.Add("ADD000", "添加成功！");
			dic.Add("ADD001", "服务器繁忙，请稍后再试！");
			//修改
			dic.Add("EDIT000", "修改成功！");
			dic.Add("EDIT001", "服务器繁忙，请稍后再试！");
			//删除
			dic.Add("DEL000", "删除成功！");
			dic.Add("DEL001", "服务器繁忙，请稍后再试！");
			dic.Add("DEL002", "未选定任何项，删除失败！");
			//查询
			dic.Add("SELECT001", "服务器繁忙，请稍后再试！");
		}
	}
	/// <summary>
	/// 获取错误代码对应的错误提示
	/// </summary>
	public static string GetErrorString(string key)
	{
		SetError();
		return dic[key];
	}
}
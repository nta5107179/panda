using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CoreClass
{
	/// <summary>
	/// 功能操作类
	/// </summary>
	public class OperateMemoryClass
	{
		/*
		============邮件操作模块============
		*/
		/// <summary>
		/// SMTP邮件群发函数
		/// </summary>
		/// <param name="SmtpClientHost">获取或设置用于 SMTP 事务的主机的名称或 IP 地址</param>
		/// <param name="MessageFromName">发件人名</param>
		/// <param name="MessageFrom">发件人邮箱</param>
		/// <param name="MessageFromPwd">发件人邮箱密码</param>
		/// <param name="MessageTo">收件人</param>
		/// <param name="MessageCC">抄送人</param>
		/// <param name="MessageSubject">邮件标题</param>
		/// <param name="MessageBody">邮件内容</param>
		/// <returns>布尔</returns>
		public bool SendMailToSmtp(string SmtpClientHost, string MessageFromName, string MessageFrom, string MessageFromPwd, string[] MessageTo, string[] MessageCC, string MessageSubject, string MessageBody)
		{
			bool b = false;
			MailMessage msg = new MailMessage();
			/*添加发送人*/
			for (int i = 0; i < MessageTo.Length; i++)
			{
				msg.To.Add(MessageTo[i]);
			}
			/*添加抄送人*/
            if (MessageCC != null && MessageCC.Length > 0)
            {
                for (int i = 0; i < MessageCC.Length; i++)
                {
                    msg.CC.Add(MessageCC[i]);
                }
            }
			msg.From = new MailAddress(MessageFrom, MessageFromName, System.Text.Encoding.UTF8);
			/* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
			msg.Subject = MessageSubject;//邮件标题
			msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码
			msg.Body = MessageBody;//邮件内容
			msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码
			msg.IsBodyHtml = true;//是否是HTML邮件
			msg.Priority = MailPriority.High;//邮件优先级

			SmtpClient client = new SmtpClient();
			client.Credentials = new System.Net.NetworkCredential(MessageFrom, MessageFromPwd);
			client.Host = SmtpClientHost;
			try
			{
				client.Send(msg);
				//简单一点儿可以client.Send(msg);
				b = true;
			}
			catch (SmtpException e) { throw e; }
			return b;
		}
		/// <summary>
		/// 虚拟SMTP邮件群发函数
		/// </summary>
		/// <param name="SmtpClientHost">获取或设置用于 SMTP 事务的主机的名称或 IP 地址</param>
		/// <param name="MessageFromName">发件人名</param>
		/// <param name="MessageFrom">发件人邮箱</param>
		/// <param name="MessageTo">收件人</param>
		/// <param name="MessageCC">抄送人</param>
		/// <param name="MessageSubject">邮件标题</param>
		/// <param name="MessageBody">邮件内容</param>
		/// <returns>布尔</returns>
		public bool SendMailToLocalhost(string SmtpClientHost, string MessageFromName, string MessageFrom, string[] MessageTo, string[] MessageCC, string MessageSubject, string MessageBody)
		{
			bool b = false;
			MailMessage msg = new MailMessage();
			/*添加发送人*/
			for (int i = 0; i < MessageTo.Length; i++)
			{
				msg.To.Add(MessageTo[i]);
			}
			/*添加抄送人*/
            if (MessageCC != null && MessageCC.Length > 0)
            {
                for (int i = 0; i < MessageCC.Length; i++)
                {
                    msg.CC.Add(MessageCC[i]);
                }
            }
			msg.From = new MailAddress(MessageFrom, MessageFromName, System.Text.Encoding.UTF8);
			/* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
			msg.Subject = MessageSubject;//邮件标题
			msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码
			msg.Body = MessageBody;//邮件内容
			msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码
			msg.IsBodyHtml = true;//是否是HTML邮件
			msg.Priority = MailPriority.High;//邮件优先级

			SmtpClient client = new SmtpClient();
			client.Host = SmtpClientHost;
			try
			{
				client.Send(msg);
				//简单一点儿可以client.Send(msg);
				b = true;
			}
			catch (SmtpException e) { throw e; }
			return b;
		}
		/*
		============IP地址操作模块============
		*/
		/// <summary>
		/// 获取真实IP
		/// </summary>
		public string IPAddress
		{
			get
			{
				string result = String.Empty;
				result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
				if (result != null && result != String.Empty)
				{
					//可能有代理
					if (result.IndexOf(".") == -1) //没有"."肯定是非IPv4格式
						result = null;
					else
					{
						if (result.IndexOf(",") != -1)
						{
							//有","，估计多个代理。取第一个不是内网的IP。
							result = result.Replace(" ", "").Replace(",", "");
							string[] temparyip = result.Split(",;".ToCharArray());
							for (int i = 0; i < temparyip.Length; i++)
							{
								if (IsIPAddress(temparyip[i])
									&& temparyip[i].Substring(0, 3) != "10."
									&& temparyip[i].Substring(0, 7) != "192.168"
									&& temparyip[i].Substring(0, 7) != "172.16.")
								{
									return temparyip[i]; //找到不是内网的地址
								}
							}
						}
						else if (IsIPAddress(result)) //代理即是IP格式
							return result;
						else
							result = null; //代理中的内容 非IP，取IP
					}
				}
				string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
				if (null == result || result == String.Empty)
					result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
				if (result == null || result == String.Empty)
					result = HttpContext.Current.Request.UserHostAddress;
				return result;
			}
		}
		/// <summary>
		/// 判断ip地址是否正确
		/// </summary>
		/// <param name="str1">待检测ip</param>
		/// <returns>布尔</returns>
		public bool IsIPAddress(string str1)
		{
			if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;
			string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";
			Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
			return regex.IsMatch(str1);
		}
		/*
		============缓存操作模块============
		*/
		/// <summary>
		/// 创建缓存
		/// </summary>
		/// <param name="key">键</param>
		/// <param name="obj">值</param>
		/// <returns>布尔</returns>
		public bool SetCache(string key, object obj)
		{
			bool b = false;
			try
			{
				if (HttpContext.Current.Cache[key] == null)
				{
					HttpContext.Current.Cache.Add(key, obj, null, Cache.NoAbsoluteExpiration, TimeSpan.Zero, CacheItemPriority.Normal, null);
					b = true;
				}
			}
			catch (Exception e) { throw e; }
			return b;
		}
		/// <summary>
		/// 创建缓存
		/// </summary>
		/// <param name="key">键</param>
		/// <param name="obj">值</param>
		/// <param name="cachedependency">文件依赖项</param>
		/// <returns>布尔</returns>
		public bool SetCache(string key, object obj, CacheDependency cachedependency)
		{
			bool b = false;
			try
			{
				if (HttpContext.Current.Cache[key] == null)
				{
					HttpContext.Current.Cache.Add(key, obj, cachedependency, Cache.NoAbsoluteExpiration, TimeSpan.Zero, CacheItemPriority.Normal, null);
					b = true;
				}
			}
			catch (Exception e) { throw e; }
			return b;
		}
		/// <summary>
		/// 创建缓存
		/// </summary>
		/// <param name="key">键</param>
		/// <param name="obj">值</param>
		/// <param name="sqlcachedependency">数据库依赖项</param>
		/// <returns>布尔</returns>
		public bool SetCache(string key, object obj, SqlCacheDependency sqlcachedependency)
		{
			bool b = false;
			try
			{
				if (HttpContext.Current.Cache[key] == null)
				{
					HttpContext.Current.Cache.Add(key, obj, sqlcachedependency, Cache.NoAbsoluteExpiration, TimeSpan.Zero, CacheItemPriority.Normal, null);
					b = true;
				}
			}
			catch (Exception e) { throw e; }
			return b;
		}
		/// <summary>
		/// 读取缓存
		/// </summary>
		/// <param name="key">键</param>
		/// <returns>对象</returns>
		public object GetCache(string key)
		{
			return HttpContext.Current.Cache.Get(key);
		}
		/// <summary>
		/// 移除缓存
		/// </summary>
		/// <param name="key">键</param>
		/// <returns>布尔</returns>
		public bool RemoveCache(string key)
		{
			bool b = false;
			try
			{
				HttpContext.Current.Cache.Remove(key);
				b = true;
			}
			catch (Exception e) { throw e; }
			return b;
		}
		/// <summary>
		/// 修改缓存项
		/// </summary>
		/// <param name="key">键</param>
		/// <param name="obj">值</param>
		/// <returns>布尔</returns>
		public bool EditCache(string key, object obj)
		{
			bool b = false;
			try
			{
				if (HttpContext.Current.Cache[key] != null)
				{
					HttpContext.Current.Cache.Insert(key, obj);
					b = true;
				}
			}
			catch (Exception e) { throw e; }
			return b;
		}
		/// <summary>
		/// 修改缓存项
		/// </summary>
		/// <param name="key">键</param>
		/// <param name="obj">值</param>
		/// <param name="cachedependency">依赖项</param>
		/// <returns>布尔</returns>
		public bool EditCache(string key, object obj, CacheDependency cachedependency)
		{
			bool b = false;
			try
			{
				if (HttpContext.Current.Cache[key] != null)
				{
					HttpContext.Current.Cache.Insert(key, obj, cachedependency);
					b = true;
				}
			}
			catch (Exception e) { throw e; }
			return b;
		}
		/*
		============cookie操作模块============
		*/
		/// <summary>
		/// 添加或修改cookie
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="value">值</param>
		/// <param name="domain">域名</param>
		/// <returns>布尔</returns>
		public bool SetCookie(string name, string value, string domain)
		{
			bool b = false;
			try
			{
				HttpCookie cookie = new HttpCookie(name);
				
				//cookie已存在
				if (HttpContext.Current.Request.Cookies[name] != null)
				{
					cookie = HttpContext.Current.Request.Cookies[name];
					//判断是否指定domain
					if (domain != null)
					{
						cookie.Domain = domain;
					}
					cookie.Value = value;
					//修改
					HttpContext.Current.Response.SetCookie(cookie);
				}
				//cookie不存在
				else
				{
					//判断是否指定domain
					if (domain != null)
					{
						cookie.Domain = domain;
					}
					cookie.Value = value;
					//添加
					HttpContext.Current.Response.AppendCookie(cookie);
				}
				b = true;
			}
			catch (Exception e) { throw e; }
			return b;
		}
		/// <summary>
		/// 添加或修改cookie
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="value">值</param>
		/// <param name="days">日</param>
		/// <param name="hours">时</param>
		/// <param name="minutes">分</param>
		/// <param name="second">秒</param>
		/// <param name="domain">域名</param>
		/// <returns>布尔</returns>
		public bool SetCookie(string name, string value, int days, int hours, int minutes, int second, string domain)
		{
			bool b = false;
			try
			{
				HttpCookie cookie = new HttpCookie(name);
				TimeSpan ts = new TimeSpan(days, hours, minutes, second);
				
				//cookie已存在
				if (HttpContext.Current.Request.Cookies[name] != null)
				{
					cookie = HttpContext.Current.Request.Cookies[name];
					cookie.Expires = DateTime.Now.Add(ts);
					//判断是否指定domain
					if (domain != null)
					{
						cookie.Domain = domain;
					}
					cookie.Value = value;
					//修改
					HttpContext.Current.Response.SetCookie(cookie);
				}
				//cookie不存在
				else
				{
					cookie.Expires = DateTime.Now.Add(ts);
					//判断是否指定domain
					if (domain != null)
					{
						cookie.Domain = domain;
					}
					cookie.Value = value;
					//添加
					HttpContext.Current.Response.AppendCookie(cookie);
				}
				b = true;
			}
			catch (Exception e) { throw e; }
			return b;
		}
		/// <summary>
		/// 获取cookie值
		/// </summary>
		/// <param name="name">名称</param>
		/// <returns>cookie值</returns>
		public string GetCookie(string name)
		{
			string cookie = null;
			try
			{
				cookie = HttpContext.Current.Request.Cookies[name].Value;
			}
			catch { }
			return cookie;
		}
		/// <summary>
		/// 移除cookie
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="domain">域名</param>
		/// <returns>布尔</returns>
		public bool RemoveCookie(string name, string domain)
		{
			bool b = false;
			if (HttpContext.Current.Request.Cookies[name] != null)
			{
				HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
				cookie.Expires = DateTime.Now.AddDays(-1);
				//判断是否指定domain
				if (domain != null)
				{
					cookie.Domain = domain;
				}
				HttpContext.Current.Response.Cookies.Set(cookie);
				b = true;
			}
			return b;
		}
		/// <summary>
		/// 添加或修改cookie，键值对的数量必须相同
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="key">键</param>
		/// <param name="value">值</param>
		/// <param name="domain">域名</param>
		/// <returns>布尔</returns>
		public bool SetCookie(string name, string[] key, string[] value, string domain)
		{
			bool b = false;
			try
			{
				HttpCookie cookie = new HttpCookie(name);

				//cookie已存在
				if (HttpContext.Current.Request.Cookies[name] != null)
				{
					cookie = HttpContext.Current.Request.Cookies[name];
					//判断是否指定domain
					if (domain != null)
					{
						cookie.Domain = domain;
					}
					for (int i = 0; i < key.Length; i++)
					{
						cookie.Values[key[i]] = value[i];
					}
					//修改
					HttpContext.Current.Response.SetCookie(cookie);
				}
				//cookie不存在
				else
				{
					//判断是否指定domain
					if (domain != null)
					{
						cookie.Domain = domain;
					}
					for (int i = 0; i < key.Length; i++)
					{
						cookie.Values[key[i]] = value[i];
					}
					//添加
					HttpContext.Current.Response.AppendCookie(cookie);
				}
				b = true;
			}
			catch (Exception e) { throw e; }
			return b;
		}
		/// <summary>
		/// 添加或修改cookie，键值对的数量必须相同
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="key">键</param>
		/// <param name="value">值</param>
		/// <param name="days">日</param>
		/// <param name="hours">时</param>
		/// <param name="minutes">分</param>
		/// <param name="second">秒</param>
		/// <param name="domain">域名</param>
		/// <returns>布尔</returns>
		public bool SetCookie(string name, string[] key, string[] value, int days, int hours, int minutes, int second, string domain)
		{
			bool b = false;
			try
			{
				HttpCookie cookie = new HttpCookie(name);
				TimeSpan ts = new TimeSpan(days, hours, minutes, second);

				//cookie已存在
				if (HttpContext.Current.Request.Cookies[name] != null)
				{
					cookie = HttpContext.Current.Request.Cookies[name];
					cookie.Expires = DateTime.Now.Add(ts);
					//判断是否指定domain
					if (domain != null)
					{
						cookie.Domain = domain;
					}
					for (int i = 0; i < key.Length; i++)
					{
						cookie.Values[key[i]] = value[i];
					}
					//修改
					HttpContext.Current.Response.SetCookie(cookie);
				}
				//cookie不存在
				else
				{
					cookie.Expires = DateTime.Now.Add(ts);
					//判断是否指定domain
					if (domain != null)
					{
						cookie.Domain = domain;
					}
					for (int i = 0; i < key.Length; i++)
					{
						cookie.Values[key[i]] = value[i];
					}
					//添加
					HttpContext.Current.Response.AppendCookie(cookie);
				}
				b = true;
			}
			catch (Exception e) { throw e; }
			return b;
		}
		/// <summary>
		/// 获取cookie值
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="key">键</param>
		/// <returns>cookie值</returns>
		public string GetCookieKey(string name, string key)
		{
			string cookie = null;
			try
			{
				cookie = HttpContext.Current.Request.Cookies[name].Values[key];
			}
			catch { }
			return cookie;
		}
		/// <summary>
		/// 移除cookie
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="key">键</param>
		/// <param name="domain">域名</param>
		/// <returns>布尔</returns>
		public bool RemoveCookieKey(string name, string key, string domain)
		{
			bool b = false;
			if (HttpContext.Current.Request.Cookies[name] != null)
			{
				HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
				//判断是否指定domain
				if (domain != null)
				{
					cookie.Domain = domain;
				}
				cookie.Values.Remove(key);
				b = true;
			}
			return b;
		}
		/// <summary>
		/// 将Net.CookieCollection转换成Web.HttpCookieCollection
		/// </summary>
		/// <param name="cookie">Net.CookieCollection</param>
		/// <returns></returns>
		public HttpCookieCollection NetCookieToHttpCookie(CookieCollection cookie)
		{
			HttpCookieCollection list = new HttpCookieCollection();
			try
			{
				foreach (Cookie c in cookie)
				{
					HttpCookie hc = new HttpCookie(c.Name, c.Value);
					try
					{
						hc.Domain = c.Domain;
					}
					catch { }
					list.Add(hc);
				}
			}
			catch (Exception ex) { throw ex; }
			return list;
		}
		/// <summary>
		/// 将Net.CookieCollection转换成Web.HttpCookieCollection
		/// </summary>
		/// <param name="cookie">Net.CookieCollection</param>
		/// <param name="domain">域</param>
		/// <returns></returns>
		public HttpCookieCollection NetCookieToHttpCookie(CookieCollection cookie, string domain)
		{
			HttpCookieCollection list = new HttpCookieCollection();
			try
			{
				foreach (Cookie c in cookie)
				{
					HttpCookie hc = new HttpCookie(c.Name, c.Value);
					try
					{
						hc.Domain = domain;
					}
					catch { }
					list.Add(hc);
				}
			}
			catch (Exception ex) { throw ex; }
			return list;
		}
		/// <summary>
		/// 将Web.HttpCookieCollection转换成Net.CookieCollection
		/// </summary>
		/// <param name="cookie">Web.HttpCookieCollection</param>
		/// <returns></returns>
		public CookieCollection HttpCookieToNetCookie(HttpCookieCollection cookie)
		{
			CookieCollection list = new CookieCollection();
			try
			{
				for (int i = 0; i < cookie.Count; i++)
				{
					Cookie hc = new Cookie(cookie[i].Name, cookie[i].Value);
					try
					{
						hc.Domain = cookie[0].Domain;
					}
					catch { }
					list.Add(hc);
				}
			}
			catch (Exception ex) { throw ex; }
			return list;
		}
		/// <summary>
		/// 将Web.HttpCookieCollection转换成Net.CookieCollection
		/// </summary>
		/// <param name="cookie">Web.HttpCookieCollection</param>
		/// <param name="domain">域</param>
		/// <returns></returns>
		public CookieCollection HttpCookieToNetCookie(HttpCookieCollection cookie, string domain)
		{
			CookieCollection list = new CookieCollection();
			try
			{
				for (int i = 0; i < cookie.Count; i++)
				{
					Cookie hc = new Cookie(cookie[i].Name, cookie[i].Value);
					try
					{
						hc.Domain = domain;
					}
					catch { }
					list.Add(hc);
				}
			}
			catch (Exception ex) { throw ex; }
			return list;
		}
		/*
		============session操作模块============
		*/
        /// <summary>
        /// 获取session id
        /// </summary>
        /// <returns>布尔</returns>
        public string GetSessionID()
        {
            return HttpContext.Current.Session.SessionID;
        }
		/// <summary>
		/// 添加或修改session
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="value">值</param>
		/// <param name="sessiontimeout">过期时间(分钟)</param>
		/// <returns>布尔</returns>
		public bool SetSession(string name, string value, int sessiontimeout)
		{
			bool b = false;
			try
			{
				HttpContext.Current.Session.Timeout = sessiontimeout;
				//cookie已存在
				if (HttpContext.Current.Session[name] != null)
				{
					HttpContext.Current.Session[name] = value;
				}
				//cookie不存在
				else
				{
					HttpContext.Current.Session.Add(name, value);
				}
				b = true;
			}
			catch (Exception e) { throw e; }
			return b;
		}
		/// <summary>
		/// 获取session值
		/// </summary>
		/// <param name="name">名称</param>
		/// <returns>session值</returns>
		public string GetSession(string name)
		{
			string session = null;
			try
			{
				session = HttpContext.Current.Session[name].ToString();
			}
			catch { }
			return session;
		}
		/// <summary>
		/// 移除session
		/// </summary>
		/// <param name="name">名称</param>
		/// <returns>布尔</returns>
		public bool RemoveSession(string name)
		{
			bool b = false;
			if (HttpContext.Current.Session[name] != null)
			{
				HttpContext.Current.Session.Remove(name);
				b = true;
			}
			return b;
		}
		/*
		============application操作模块============
		*/
		/// <summary>
		/// 添加或修改application
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="value">值</param>
		/// <returns>布尔</returns>
		public bool SetApplication(string name, object value)
		{
			bool b = false;
			try
			{
				HttpApplicationState appState = HttpContext.Current.Application;
				object app = appState.Get(name);
				//application已存在
				if (app != null)
				{
					appState.Set(name, value);
				}
				//application不存在
				else
				{
					appState.Add(name, value);
				}
				b = true;
			}
			catch (Exception e) { throw e; }
			return b;
		}
		/// <summary>
		/// 添加或修改application
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="value">值</param>
		/// <param name="hc">外部传入的HttpContext</param>
		/// <returns>布尔</returns>
		public bool SetApplication(string name, object value, ref HttpContext hc)
		{
			bool b = false;
			try
			{
				HttpApplicationState appState = hc.Application;
				object app = appState.Get(name);
				//application已存在
				if (app != null)
				{
					appState.Set(name, value);
				}
				//application不存在
				else
				{
					appState.Add(name, value);
				}
				b = true;
			}
			catch (Exception e) { throw e; }
			return b;
		}
		/// <summary>
		/// 获取application值
		/// </summary>
		/// <param name="name">名称</param>
		/// <returns>application值</returns>
		public object GetApplication(string name)
		{
			HttpApplicationState appState = HttpContext.Current.Application;
			object app = null;
			try
			{
				app = appState.Get(name);
			}
			catch { }
			return app;
		}
		/// <summary>
		/// 获取application值
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="hc">外部传入的HttpContext</param>
		/// <returns>application值</returns>
		public object GetApplication(string name, ref HttpContext hc)
		{
			HttpApplicationState appState = hc.Application;
			object app = null;
			try
			{
				app = appState.Get(name);
			}
			catch { }
			return app;
		}
		/// <summary>
		/// 移除application
		/// </summary>
		/// <param name="name">名称</param>
		/// <returns>布尔</returns>
		public bool RemoveApplication(string name)
		{
			bool b = false;
			HttpApplicationState appState = HttpContext.Current.Application;
			if (appState.Get(name) != null)
			{
				appState.Remove(name);
				b = true;
			}
			return b;
		}
		/// <summary>
		/// 移除application
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="hc">外部传入的HttpContext</param>
		/// <returns>布尔</returns>
		public bool RemoveApplication(string name, ref HttpContext hc)
		{
			bool b = false;
			HttpApplicationState appState = hc.Application;
			if (appState.Get(name) != null)
			{
				appState.Remove(name);
				b = true;
			}
			return b;
		}
		/*
		============跨域操作模块============
		*/
		/// <summary>
		/// 跨域访问
		/// </summary>
		/// <param name="url">访问地址</param>
		/// <param name="action">参数数组(如["action1=1","action2=2"]),可为null</param>
		/// <param name="domain">域名(如果是子域名跨域),可为null</param>
		/// <returns>返回访问页面的输出内容</returns>
		public string CDA(string url, string[] action, string domain)
		{
			CookieContainer craboCookie = new CookieContainer();
			HttpCookieCollection hcc = HttpContext.Current.Request.Cookies;
			CookieCollection ncc = new CookieCollection();
			for (int i = 0; i < hcc.Count; i++)
			{
				Cookie c = new Cookie(hcc[i].Name, hcc[i].Value);
				if (domain != null)
				{
					c.Domain = domain;
				}
				ncc.Add(c);
			}
			craboCookie.Add(ncc);

			string act = "";
			if (action != null)
			{
				act = "?" + string.Join("&", action);
			}
			HttpWebRequest wrq = (HttpWebRequest)WebRequest.Create(url + act);
			wrq.KeepAlive = false;
			wrq.CookieContainer = craboCookie;
			HttpWebResponse wrs = (HttpWebResponse)wrq.GetResponse();
			Stream sr = wrs.GetResponseStream();
			StreamReader reader = new StreamReader(sr, System.Text.Encoding.UTF8);
			string callback = reader.ReadToEnd();
			wrs.Close();
			return callback;
		}
		/*
		============序列化操作模块============
		*/
		/// <summary>
		/// 序列化
		/// </summary>
		/// <param name="dy">需要序列化的类型</param>
		/// <returns></returns>
        public byte[] Serialize(object dy)
		{
			byte[] _byte = null;
			BinaryFormatter bf = new BinaryFormatter();
			MemoryStream ms = new MemoryStream();
			bf.Serialize(ms, dy);
			_byte = ms.ToArray();
			return _byte;
		}
		/// <summary>
		/// 反序列化
		/// </summary>
		/// <param name="_byte">需要反序列化的byte[]</param>
		/// <returns></returns>
        public object Deserialize(byte[] _byte)
		{
			BinaryFormatter ou_bf = new BinaryFormatter();
			MemoryStream ms = new MemoryStream(_byte);
			ms.Seek(0, SeekOrigin.Begin);
			return ou_bf.Deserialize(ms);
		}
        /*
        ============HTTP方式提交与访问============
        */
        /// <summary>
        /// HTTP方式提交与访问
        /// </summary>
        /// <param name="type">提交方式（post,get）</param>
        /// <param name="url">地址</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public string HttpWebRequest(string type, string url, string data)
        {
            string str = "";
            byte[] dataArr = Encoding.UTF8.GetBytes(data);
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(url);
            switch (type.ToLower())
            {
                case "get":
                    hwr.Method = "get";
                    break;
                case "post":
                    hwr.ContentType = "application/x-www-form-urlencoded";
                    hwr.Method = "post";
                    hwr.ContentLength = dataArr.Length;
                    using (Stream s = hwr.GetRequestStream())
                    {
                        s.Write(dataArr, 0, dataArr.Length);
                    }
                    break;
            }
            WebResponse wr = hwr.GetResponse();
            using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
            {
                str = sr.ReadToEnd();
            }
            wr.Close();
            return str;
        }
        /// <summary>
        /// HTTP方式提交与访问
        /// </summary>
        /// <param name="type">提交方式（post,get）</param>
        /// <param name="url">地址</param>
        /// <param name="data">数据</param>
        /// <param name="cookies">cookies</param>
        /// <returns></returns>
        public string HttpWebRequest(string type, string url, string data, CookieContainer cookies)
        {
            string str = "";
            byte[] dataArr = Encoding.UTF8.GetBytes(data);
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(url);
            hwr.CookieContainer = cookies;
            switch (type.ToLower())
            {
                case "get":
                    hwr.Method = "get";
                    break;
                case "post":
                    hwr.ContentType = "application/x-www-form-urlencoded";
                    hwr.Method = "post";
                    hwr.ContentLength = dataArr.Length;
                    using (Stream s = hwr.GetRequestStream())
                    {
                        s.Write(dataArr, 0, dataArr.Length);
                    }
                    break;
            }
            WebResponse wr = hwr.GetResponse();
            using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
            {
                str = sr.ReadToEnd();
            }
            wr.Close();
            return str;
        }


	}
}

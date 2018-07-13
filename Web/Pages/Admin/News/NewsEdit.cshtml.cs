using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model;
using Newtonsoft.Json.Linq;
using Web.App_Code.Include;

namespace Web.Pages.Admin.News
{
    public class NewsEditModel : PageModel
	{
		IncludeAdmin m_incAdmin = new IncludeAdmin();

		public string m_newstypelist;

		public void OnGet()
		{
			if (!m_incAdmin.IsLogin)
			{
				m_incAdmin.RedirectAdmin();
			}

			List<Models> list = m_incAdmin.GetNewsTypeList();
			m_newstypelist = WebUtility.UrlDecode(m_incAdmin.OpString.ToJsonArray(list.ToList<object>()).ToString());
		}

		public IActionResult OnPostGetNews()
		{
			JObject result = new JObject();

			string n_id = Request.Form["n_id"];
			List<string> query = new List<string>() { n_id };

			if (!m_incAdmin.OpString.DecideNull(query.ToArray()))
			{
				if (!m_incAdmin.OpString.DetectSql(query.ToArray(), Config.g_illegal))
				{
					Models detail = m_incAdmin.GetNews(n_id);
					detail.g_news.n_content = m_incAdmin.OpString.unEscape(detail.g_news.n_content);

					result.Add("detail", m_incAdmin.OpString.ToJson(detail));
				}
				else
				{
					result.Add("error", SysError.GetErrorString("SYS001"));
				}
			}
			else
			{
				result.Add("error", SysError.GetErrorString("SYS002"));
			}

			return new JsonResult(result.ToString());
		}

		public IActionResult OnPostSaveNews()
		{
			JObject result = new JObject();

			string n_id = Request.Form["n_id"];
			string nt_id = Request.Form["nt_id"];
			string n_title = Request.Form["n_title"];
			string n_content = m_incAdmin.OpString.Escape(Request.Form["n_content"]);
			string n_filename = Request.Form["n_filename"];
			IFormFile file1 = Request.Form.Files["file1"];
			string n_url = Request.Form["n_url"];
			string n_top = Request.Form["n_top"];
			string n_examine = Request.Form["n_examine"];
			List<string> query = new List<string>() { nt_id, n_title, n_content, n_top, n_examine };

			if (!m_incAdmin.OpString.DecideNull(query.ToArray()))
			{
				query.AddRange(new string[] { n_id, n_filename, n_url });
				if (!m_incAdmin.OpString.DetectSql(query.ToArray(), Config.g_illegal))
				{
					DateTime n_addtime = (string)Request.Form["n_addtime"] == null ? DateTime.Now : DateTime.Parse(Request.Form["n_addtime"]);
					string imgpath = string.Format("{0}News/{1}/img/", Config.g_filepath, n_addtime.ToString("yyyy-MM-dd"));
					if (string.IsNullOrEmpty(n_id))
					{
						if (file1 != null && file1.Length > 0)
						{
							try
							{
								n_filename = m_incAdmin.OpFile.UploadFile(file1, imgpath, Config.g_imgtype, Config.g_imgmax);
							}
							catch (Exception e)
							{
								result.Add("error", e.Message);
								return new JsonResult(result.ToString());
							}
						}

						g_news mod = new g_news();
						mod.nt_id = int.Parse(nt_id);
						mod.n_title = n_title;
						mod.n_content = n_content;
						mod.n_filename = n_filename;
						mod.n_url = n_url;
						mod.n_top = int.Parse(n_top);
						mod.n_examine = bool.Parse(n_examine);
						mod.n_addtime = n_addtime;
						if (m_incAdmin.AddNews(mod))
						{
							result.Add("success", SysError.GetErrorString("ADD000"));
						}
						else
						{
							result.Add("error", SysError.GetErrorString("ADD001"));
						}
					}
					else
					{
						if (file1 != null && file1.Length > 0)
						{
							try
							{
								string n_filename_old = n_filename;
								n_filename = m_incAdmin.OpFile.UploadFile(file1, imgpath, Config.g_imgtype, Config.g_imgmax);
								m_incAdmin.OpFile.DelFile(imgpath + n_filename_old);
							}
							catch (Exception e)
							{
								result.Add("error", e.Message);
								return new JsonResult(result.ToString());
							}
						}

						g_news mod = new g_news();
						mod.n_id = int.Parse(n_id);
						g_news mod2 = new g_news();
						mod2.nt_id = int.Parse(nt_id);
						mod2.n_title = n_title;
						mod2.n_content = n_content;
						mod2.n_filename = n_filename;
						mod2.n_url = n_url;
						mod2.n_top = int.Parse(n_top);
						mod2.n_examine = bool.Parse(n_examine);
						if (m_incAdmin.EditNews(mod, mod2))
						{
							result.Add("success", SysError.GetErrorString("EDIT000"));
						}
						else
						{
							result.Add("error", SysError.GetErrorString("EDIT001"));
						}
					}
				}
				else
				{
					result.Add("error", SysError.GetErrorString("SYS001"));
				}
			}
			else
			{
				result.Add("error", SysError.GetErrorString("SYS002"));
			}

			return new JsonResult(result.ToString());
		}

	}
}
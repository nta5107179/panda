using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model;
using Newtonsoft.Json.Linq;
using Web.App_Code.Include;

namespace Web.Pages.Admin.News
{
    public class NewsListModel : PageModel
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

		public IActionResult OnPostGetNewsList()
		{
			JObject result = new JObject();

			string page = Request.Form["page"];
			string limit = Request.Form["limit"];
			string n_title = Request.Form["n_title"];
			string nt_id = Request.Form["nt_id"];
			string n_examine = Request.Form["n_examine"];
			List<string> query = new List<string>() { page, limit };

			if (!m_incAdmin.OpString.DecideNull(query.ToArray()))
			{
				query.AddRange(new string[] { n_title, nt_id, n_examine });
				if (!m_incAdmin.OpString.DetectSql(query.ToArray()))
				{
					long total = 0;
					List<Models> list = m_incAdmin.GetNewsList(page, limit, ref total, n_title, nt_id, n_examine);

					result.Add("list", m_incAdmin.OpString.ToJsonArray(list.ToList<object>()));
					result.Add("total", total);
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

		public IActionResult OnPostDelNews()
		{
			JObject result = new JObject();

			string n_id = Request.Form["n_id"];
			List<string> query = new List<string>() { n_id };

			if (!m_incAdmin.OpString.DecideNull(query.ToArray()))
			{
				if (!m_incAdmin.OpString.DetectSql(query.ToArray()))
				{
					g_news mod = new g_news();
					mod.n_id = int.Parse(n_id);
					if (m_incAdmin.DelNews(mod))
					{
						result.Add("success", SysError.GetErrorString("DEL000"));
					}
					else
					{
						result.Add("error", SysError.GetErrorString("DEL001"));
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
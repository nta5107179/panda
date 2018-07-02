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
    public class NewsTypeEditModel : PageModel
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

		public IActionResult OnPostGetNewsType()
		{
			JObject result = new JObject();

			string nt_id = Request.Form["nt_id"];
			List<string> query = new List<string>() { nt_id };

			if (!m_incAdmin.OpString.DecideNull(query.ToArray()))
			{
				if (!m_incAdmin.OpString.DetectSql(query.ToArray()))
				{
					Models detial = m_incAdmin.GetNewsType(nt_id);

					result.Add("detial", m_incAdmin.OpString.ToJson(detial));
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

		public IActionResult OnPostSaveNewsType()
		{
			JObject result = new JObject();

			string nt_id = Request.Form["nt_id"];
			string nt_name = Request.Form["nt_name"];
			string nt_pid = Request.Form["nt_pid"];
			string nt_top = Request.Form["nt_top"];
			string nt_examine = Request.Form["nt_examine"];
			List<string> query = new List<string>() { nt_name, nt_pid, nt_top, nt_examine };

			if (!m_incAdmin.OpString.DecideNull(query.ToArray()))
			{
				query.AddRange(new string[] { nt_id });
				if (!m_incAdmin.OpString.DetectSql(query.ToArray()))
				{
					if (string.IsNullOrEmpty(nt_id))
					{
						g_newstype mod = new g_newstype();
						mod.nt_name = nt_name;
						mod.nt_pid = int.Parse(nt_pid);
						mod.nt_top = int.Parse(nt_top);
						mod.nt_examine = bool.Parse(nt_examine);
						if (m_incAdmin.AddNewsType(mod))
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
						g_newstype mod = new g_newstype();
						mod.nt_id = int.Parse(nt_id);
						g_newstype mod2 = new g_newstype();
						mod2.nt_name = nt_name;
						mod2.nt_pid = int.Parse(nt_pid);
						mod2.nt_top = int.Parse(nt_top);
						mod2.nt_examine = bool.Parse(nt_examine);
						if (m_incAdmin.EditNewsType(mod, mod2))
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
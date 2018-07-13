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

namespace Web.Pages.Admin.Message
{
    public class MessageListModel : PageModel
	{
		IncludeAdmin m_incAdmin = new IncludeAdmin();

		public void OnGet()
		{
			if (!m_incAdmin.IsLogin)
			{
				m_incAdmin.RedirectAdmin();
			}
		}

		public IActionResult OnPostGetMessageList()
		{
			JObject result = new JObject();

			string page = Request.Form["page"];
			string limit = Request.Form["limit"];
			List<string> query = new List<string>() { page, limit };

			if (!m_incAdmin.OpString.DecideNull(query.ToArray()))
			{
				if (!m_incAdmin.OpString.DetectSql(query.ToArray(), Config.g_illegal))
				{
					long total = 0;
					List<Models> list = m_incAdmin.GetMessageList(page, limit, ref total);

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

		public IActionResult OnPostDelMessage()
		{
			JObject result = new JObject();

			string m_id = Request.Form["m_id"];
			List<string> query = new List<string>() { m_id };

			if (!m_incAdmin.OpString.DecideNull(query.ToArray()))
			{
				if (!m_incAdmin.OpString.DetectSql(query.ToArray(), Config.g_illegal))
				{
					g_message mod = new g_message();
					mod.m_id = int.Parse(m_id);
					if (m_incAdmin.DelMessage(mod))
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
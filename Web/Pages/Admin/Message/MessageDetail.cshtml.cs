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

namespace Web.Pages.Admin.Message
{
    public class MessageEditModel : PageModel
	{
		IncludeAdmin m_incAdmin = new IncludeAdmin();

		public void OnGet()
		{
			if (!m_incAdmin.IsLogin)
			{
				m_incAdmin.RedirectAdmin();
			}
		}

		public IActionResult OnPostGetMessage()
		{
			JObject result = new JObject();

			string m_id = Request.Form["m_id"];
			List<string> query = new List<string>() { m_id };

			if (!m_incAdmin.OpString.DecideNull(query.ToArray()))
			{
				if (!m_incAdmin.OpString.DetectSql(query.ToArray(), Config.g_illegal))
				{
					Models detail = m_incAdmin.GetMessage(m_id);

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

		public IActionResult OnPostSaveMessage()
		{
			JObject result = new JObject();

			string m_id = Request.Form["m_id"];
			string m_isread = Request.Form["m_isread"];
			List<string> query = new List<string>() { m_id, m_isread };

			if (!m_incAdmin.OpString.DecideNull(query.ToArray()))
			{
				if (!m_incAdmin.OpString.DetectSql(query.ToArray(), Config.g_illegal))
				{
					g_message mod = new g_message();
					mod.m_id = int.Parse(m_id);
					g_message mod2 = new g_message();
					mod2.m_isread = bool.Parse(m_isread);
					if (m_incAdmin.EditMessage(mod, mod2))
					{
						result.Add("success", SysError.GetErrorString("EDIT000"));
					}
					else
					{
						result.Add("error", SysError.GetErrorString("EDIT001"));
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
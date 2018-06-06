using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model;
using Newtonsoft.Json.Linq;
using Web.App_Code;
using Web.App_Code.Include;

namespace Web.Pages
{
    public class IndexModel : PageModel
    {
		Include m_inc = new Include();

		public void OnGet()
        {
			System.Net.IPAddress ip = HttpContext.Connection.RemoteIpAddress;

		}

		public IActionResult OnPostAddMessage()
		{
			JObject result = new JObject();

			string m_name = Request.Form["m_name"];
			string m_email = Request.Form["m_email"];
			string m_content = Request.Form["m_content"];
			List<string> list = new List<string>() { m_name, m_content };

			if (!m_inc.OpString.DecideNull(list.ToArray()))
			{
				list.Add(m_email);
				if (!m_inc.OpString.DetectSql(list.ToArray()))
				{
					if (!m_inc.HasMessageToday())
					{
						g_message mod = new g_message();
						mod.m_name = m_name;
						mod.m_email = m_email;
						mod.m_content = m_content;
						mod.m_ip = m_inc.OpMemory.IPAddress;
						if (m_inc.AddMessage(mod))
						{
							result.Add("success", SysError.GetErrorString("MESSAGE000"));
						}
						else
						{
							result.Add("error", SysError.GetErrorString("MESSAGE001"));
						}
					}
					else
					{
						result.Add("error", SysError.GetErrorString("MESSAGE002"));
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

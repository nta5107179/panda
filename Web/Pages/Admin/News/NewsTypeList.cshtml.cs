using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model;
using Newtonsoft.Json.Linq;
using Web.App_Code.Include;

namespace Web.Pages.Admin.News
{
    public class NewsTypeListModel : PageModel
	{
		IncludeAdmin m_incAdmin = new IncludeAdmin();

		public void OnGet()
        {
        }

		public IActionResult OnPostGetNewsTypeList()
		{
			JObject result = new JObject();

			string page = Request.Form["page"];
			string limit = Request.Form["limit"];
			string nt_name = Request.Form["nt_name"];
			string nt_pid = Request.Form["nt_pid"];
			string nt_examine = Request.Form["nt_examine"];
			List<string> query = new List<string>() { page, limit };

			if (!m_incAdmin.OpString.DecideNull(query.ToArray()))
			{
				query.AddRange(new string[] { nt_name, nt_pid, nt_examine });
				if (!m_incAdmin.OpString.DetectSql(query.ToArray()))
				{
					long total = 0;
					List<Models> list = m_incAdmin.GetNewsTypeList(page, limit, ref total, nt_name, nt_pid, nt_examine);
					if (list.Count>0)
					{
						result.Add("list", m_incAdmin.OpString.ToJsonArray(list.ToList<object>())["g_newstype"]);
					}
					else
					{
						result.Add("error", SysError.GetErrorString("SELECT001"));
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
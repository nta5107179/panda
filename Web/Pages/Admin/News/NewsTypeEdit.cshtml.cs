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
					List<Models> list = m_incAdmin.GetNewsType(nt_id);

					result.Add("list", m_incAdmin.OpString.ToJsonArray(list.ToList<object>()));
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
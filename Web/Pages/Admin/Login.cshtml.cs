using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Web.App_Code.Include;

namespace Web.Pages.Admin
{
    public class LoginModel : PageModel
	{
		IncludeAdmin m_incAdmin = new IncludeAdmin();

		public void OnGet()
        {
        }
		
		public IActionResult OnPostLogin()
		{
			JObject result = new JObject();

			string a_uname = Request.Form["a_uname"];
			string a_pwd = Request.Form["a_pwd"];
			string isremember = Request.Form["isremember"];
			List<string> query = new List<string>() { a_uname, a_pwd };

			if (!m_incAdmin.OpString.DecideNull(query.ToArray()))
			{
				query.Add(isremember);
				if (!m_incAdmin.OpString.DetectSql(query.ToArray(), Config.g_illegal))
				{
					if (m_incAdmin.Login(a_uname, a_pwd, string.IsNullOrEmpty(isremember)? false : bool.Parse(isremember)))
					{
						result.Add("success", SysError.GetErrorString("LOGIN000"));
					}
					else
					{
						result.Add("error", SysError.GetErrorString("LOGIN001"));
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
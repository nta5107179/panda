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
    public class unLoginModel : PageModel
	{
		IncludeAdmin m_incAdmin = new IncludeAdmin();
		
		public void OnGet()
        {
			m_incAdmin.unLogin();
			m_incAdmin.RedirectAdmin();
		}
		
	}
}
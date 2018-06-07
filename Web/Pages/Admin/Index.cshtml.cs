using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.App_Code.Include;

namespace Web.Pages.Admin
{
    public class IndexModel : PageModel
    {
		IncludeAdmin m_incAdmin = new IncludeAdmin();

        public void OnGet()
        {
			if (!m_incAdmin.IsLogin)
			{
				m_incAdmin.RedirectAdmin();
			}
        }
    }
}
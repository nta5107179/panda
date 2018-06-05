using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        }

		public IActionResult OnPostAddMessage()
		{
			JObject result = new JObject();

			result.Add("success", "成功啦");

			return new JsonResult(result.ToString());
		}

	}
}

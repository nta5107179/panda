using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoreClass;

namespace Web.Pages.Test
{
    public class bbModel : PageModel
    {
		public OperateMemoryClass m_memory = new OperateMemoryClass();

		public void OnGet()
        {
        }
    }
}
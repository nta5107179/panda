using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoreClass;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace Web.Pages.Test
{
    public class aaModel : PageModel
    {
		public OperateMemoryClass m_memory = new OperateMemoryClass();
		public OperateStringClass m_string = new OperateStringClass();
		public OperateSqlClass m_sql = new OperateSqlClass();
		public OperateFileClass m_file = new OperateFileClass();

		public string m_filename = "";

		public void OnGet()
        {
			
		}

		public void OnPost()
		{
			IFormFile file = Request.Form.Files["file1"];
			m_filename = m_file.UploadFile(file, "Files/Test/", null, 0);
		}
    }
}
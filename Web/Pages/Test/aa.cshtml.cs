using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoreClass;
using System.Data;

namespace Web.Pages.Test
{
    public class aaModel : PageModel
    {
		public OperateMemoryClass m_memory = new OperateMemoryClass();
		public OperateStringClass m_string = new OperateStringClass();
		public OperateSqlClass m_sql = new OperateSqlClass();

		public void OnGet()
        {
			m_sql.Open();
			try
			{
				string sql = "select top 10 * from account";
				DataSet ds = new DataSet();
				object[] objArr = { 1, "" };
				m_sql.Selectproce("test", objArr, ref ds);

			}
			catch (Exception e)
			{
				
			}
			finally
			{
				m_sql.Close();
			}
		}

		public void OnPost()
		{
			m_memory.RemoveApplication("test");
		}
    }
}
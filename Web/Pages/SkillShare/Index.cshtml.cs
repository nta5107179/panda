using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Model;
using Web.App_Code.Include;

namespace Web.Pages.SkillShare
{
    public class IndexModel : PageModel
    {
        public Include m_inc = new Include();
        Model.Pagination m_page = new Model.Pagination()
        {
            page = 1,
            limit = 10
        };
        public List<Models> m_newslist = new List<Models>();

        public void OnGet()
        {
            ViewData["page"] = m_page;
            GetNewsList();
        }

        void GetNewsList()
        {
            if(!string.IsNullOrEmpty(Request.Query["page"]))
                m_page.page = int.Parse(Request.Query["page"]);
            string nt_id = Request.Query["nt_id"];
            long total = 0;
            m_newslist = m_inc.GetNewsList(m_page.page.ToString(), m_page.limit.ToString(), ref total, nt_id);
            m_page.total = total;
        }
    }
}
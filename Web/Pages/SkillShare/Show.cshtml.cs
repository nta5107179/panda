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
    public class ShowModel : PageModel
    {
        public Include m_inc = new Include();
        public Models m_newsdetail = new Models();

        public void OnGet()
        {
            GetNews();
        }

        void GetNews()
        {
            string nt_id = Request.Query["nt_id"];
            string n_id = Request.Query["n_id"];
            m_newsdetail = m_inc.GetNews(nt_id, n_id);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Model
{
    /// <summary>
    /// 将数据集转化成实体数组
    /// </summary>
    public class GetModelList
    {
        /// <summary>
        /// 初始化GetModelList类型的新
        /// </summary>
        public GetModelList()
        {

        }

        /// <summary>
        /// 获取g_admin实体List
        /// </summary>
        /// <param name="dt">源数据表</param>
        /// <returns></returns>
        public List<g_admin> g_admin(DataTable dt)
        {
            List<g_admin> modelList = new List<g_admin>();
            g_admin model;
            for (int n = 0; n < dt.Rows.Count; n++)
            {
                model = new g_admin();
                try
                {
                    model.a_id = (int)dt.Rows[n]["a_id"];
                }
                catch { }
                model.a_uname = dt.Rows[n]["a_uname"].ToString();
                model.a_pwd = dt.Rows[n]["a_pwd"].ToString();
                try {
                    model.a_addtime=(DateTime)dt.Rows[n]["a_addtime"];
                }
                catch { }
                modelList.Add(model);
            }
            return modelList;
        }
		/// <summary>
		/// 获取g_newstype实体List
		/// </summary>
		/// <param name="dt">源数据表</param>
		/// <returns></returns>
		public List<g_newstype> g_newstype(DataTable dt)
        {
            List<g_newstype> modelList = new List<g_newstype>();
			g_newstype model;
            for (int n = 0; n < dt.Rows.Count; n++)
            {
                model = new g_newstype();
                try {
                    model.nt_id=(int)dt.Rows[n]["nt_id"];
                }
                catch { }
				try
				{
					model.nt_pid = (int)dt.Rows[n]["nt_pid"];
				}
				catch { }
				model.nt_pname = dt.Rows[n]["nt_pname"].ToString();
				model.nt_name = dt.Rows[n]["nt_name"].ToString();
				try
				{
					model.nt_top = (int)dt.Rows[n]["nt_top"];
				}
				catch { }
				try {
                    model.nt_examine= Convert.ToBoolean((sbyte)dt.Rows[n]["nt_examine"]);
                }
                catch { }
                try {
                    model.nt_addtime=(DateTime)dt.Rows[n]["nt_addtime"];
                }
                catch { }
                modelList.Add(model);
            }
            return modelList;
        }
		/// <summary>
		/// 获取g_news实体List
		/// </summary>
		/// <param name="dt">源数据表</param>
		/// <returns></returns>
		public List<g_news> g_news(DataTable dt)
		{
			List<g_news> modelList = new List<g_news>();
			g_news model;
			for (int n = 0; n < dt.Rows.Count; n++)
			{
				model = new g_news();
				try
				{
					model.n_id = (int)dt.Rows[n]["n_id"];
				}
				catch { }
				try
				{
					model.nt_id = (int)dt.Rows[n]["nt_id"];
				}
				catch { }
				model.n_title = dt.Rows[n]["n_title"].ToString();
				model.n_content = dt.Rows[n]["n_content"].ToString();
				model.n_filename = dt.Rows[n]["n_filename"].ToString();
				model.n_url = dt.Rows[n]["n_url"].ToString();
				try
				{
					model.n_top = (int)dt.Rows[n]["n_top"];
				}
				catch { }
				try
				{
					model.n_examine = Convert.ToBoolean((sbyte)dt.Rows[n]["n_examine"]);
				}
				catch { }
				try
				{
					model.n_addtime = (DateTime)dt.Rows[n]["n_addtime"];
				}
				catch { }
				modelList.Add(model);
			}
			return modelList;
		}
		/// <summary>
		/// 获取g_message实体List
		/// </summary>
		/// <param name="dt">源数据表</param>
		/// <returns></returns>
		public List<g_message> g_message(DataTable dt)
		{
			List<g_message> modelList = new List<g_message>();
			g_message model;
			for (int n = 0; n < dt.Rows.Count; n++)
			{
				model = new g_message();
				try
				{
					model.m_id = (int)dt.Rows[n]["m_id"];
				}
				catch { }
				model.m_name = dt.Rows[n]["m_name"].ToString();
				model.m_email = dt.Rows[n]["m_email"].ToString();
				model.m_content = dt.Rows[n]["m_content"].ToString();
				model.m_ip = dt.Rows[n]["m_ip"].ToString();
				try
				{
					model.m_isread = Convert.ToBoolean((sbyte)dt.Rows[n]["m_isread"]);
				}
				catch { }
				try
				{
					model.m_addtime = (DateTime)dt.Rows[n]["m_addtime"];
				}
				catch { }
				modelList.Add(model);
			}
			return modelList;
		}



		/*
        /// <summary>
        /// 获取G_Type实体List
        /// </summary>
        /// <param name="dt">源数据表</param>
        /// <returns></returns>
        public List<G_Type> G_Type(DataTable dt)
        {
            List<G_Type> modelList = new List<G_Type>();
            G_Type model;
            for (int n = 0; n < dt.Rows.Count; n++)
            {
                model = new G_Type();

                modelList.Add(model);
            }
            return modelList;
        }
        */
	}
}

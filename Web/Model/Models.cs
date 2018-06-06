using System;
using Model;

namespace Model
{
	/// <summary>
	/// 实体集合
	/// </summary>
	[Serializable]
	public class Models
	{
		public Models()
		{ }
		#region Model
        private g_admin _g_admin;
        private g_newstype _g_newstype;
        private g_news _g_news;
        private g_message _g_message;

        #endregion
        /// <summary>
        /// 
        /// </summary>
        public g_admin g_admin
        {
            set { _g_admin = value; }
            get { return _g_admin; }
        }
        /// <summary>
        /// 
        /// </summary>
        public g_newstype g_newstype
		{
            set { _g_newstype = value; }
            get { return _g_newstype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public g_news g_group
        {
            set { _g_news = value; }
            get { return _g_news; }
        }
        /// <summary>
        /// 
        /// </summary>
        public g_message g_message
		{
            set { _g_message = value; }
            get { return _g_message; }
        }

    }
}

using System;
namespace Model
{
    /// <summary>
    /// g_admin:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class g_news
	{
        public g_news()
        { }
		#region Model
		private int? _n_id;
		private int? _nt_id;
		private string _n_title;
		private string _n_content;
		private string _n_filename;
		private string _n_url;
		private int? _n_top;
        private bool? _n_examine;
        private DateTime? _n_addtime;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int? n_id
		{
            set { _n_id = value; }
            get { return _n_id; }
        }
		/// <summary>
		/// 
		/// </summary>
		public int? nt_id
		{
			set { _nt_id = value; }
			get { return _nt_id; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string n_title
		{
            set { _n_title = value; }
            get { return _n_title; }
        }
		/// <summary>
		/// 
		/// </summary>
		public string n_content
		{
            set { _n_content = value; }
            get { return _n_content; }
        }
		/// <summary>
		/// 
		/// </summary>
		public string n_filename
		{
            set { _n_filename = value; }
            get { return _n_filename; }
        }
		/// <summary>
		/// 
		/// </summary>
		public string n_url
		{
            set { _n_url = value; }
            get { return _n_url; }
        }
		/// <summary>
		/// 
		/// </summary>
		public int? n_top
		{
			set { _n_top = value; }
			get { return _n_top; }
		}
        /// <summary>
        /// 
        /// </summary>
        public bool? n_examine
		{
            set { _n_examine = value; }
            get { return _n_examine; }
        }
        /// <summary>
        /// on update CURRENT_TIMESTAMP
        /// </summary>
        public DateTime? n_addtime
		{
            set { _n_addtime = value; }
            get { return _n_addtime; }
        }
        #endregion Model

    }
}


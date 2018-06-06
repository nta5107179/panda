using System;
namespace Model
{
    /// <summary>
    /// g_admin:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class g_message
	{
        public g_message()
        { }
		#region Model
		private int? _m_id;
		private string _m_name;
		private string _m_email;
		private string _m_content;
		private string _m_ip;
        private bool? _m_isread;
        private DateTime? _m_addtime;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int? m_id
		{
            set { _m_id = value; }
            get { return _m_id; }
        }
		/// <summary>
		/// 
		/// </summary>
		public string m_name
		{
            set { _m_name = value; }
            get { return _m_name; }
        }
		/// <summary>
		/// 
		/// </summary>
		public string m_email
		{
            set { _m_email = value; }
            get { return _m_email; }
        }
		/// <summary>
		/// 
		/// </summary>
		public string m_content
		{
            set { _m_content = value; }
            get { return _m_content; }
        }
		/// <summary>
		/// 
		/// </summary>
		public string m_ip
		{
            set { _m_ip = value; }
            get { return _m_ip; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? m_isread
		{
            set { _m_isread = value; }
            get { return _m_isread; }
        }
        /// <summary>
        /// on update CURRENT_TIMESTAMP
        /// </summary>
        public DateTime? m_addtime
		{
            set { _m_addtime = value; }
            get { return _m_addtime; }
        }
        #endregion Model

    }
}


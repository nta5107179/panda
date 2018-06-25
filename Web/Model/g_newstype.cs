using System;
namespace Model
{
    /// <summary>
    /// g_admin:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class g_newstype
	{
        public g_newstype()
        { }
		#region Model
		private int? _nt_id;
		private int? _nt_pid;
		private string _nt_pname;
		private string _nt_name;
		private int? _nt_top;
        private bool? _nt_examine;
        private DateTime? _nt_addtime;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int? nt_id
		{
            set { _nt_id = value; }
            get { return _nt_id; }
        }
		/// <summary>
		/// 
		/// </summary>
		public int? nt_pid
		{
			set { _nt_pid = value; }
			get { return _nt_pid; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string nt_pname
		{
			set { _nt_pname = value; }
			get { return _nt_pname; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string nt_name
		{
            set { _nt_name = value; }
            get { return _nt_name; }
        }
		/// <summary>
		/// 
		/// </summary>
		public int? nt_top
		{
			set { _nt_top = value; }
			get { return _nt_top; }
		}
        /// <summary>
        /// 
        /// </summary>
        public bool? nt_examine
		{
            set { _nt_examine = value; }
            get { return _nt_examine; }
        }
        /// <summary>
        /// on update CURRENT_TIMESTAMP
        /// </summary>
        public DateTime? nt_addtime
		{
            set { _nt_addtime = value; }
            get { return _nt_addtime; }
        }
        #endregion Model

    }
}


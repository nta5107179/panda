using System;
namespace Model
{
    /// <summary>
    /// g_admin:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class g_admin
    {
        public g_admin()
        { }
        #region Model
        private int? _a_id;
        private string _a_uname;
        private string _a_pwd;
        private DateTime? _a_addtime;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int? a_id
        {
            set { _a_id = value; }
            get { return _a_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string a_uname
        {
            set { _a_uname = value; }
            get { return _a_uname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string a_pwd
        {
            set { _a_pwd = value; }
            get { return _a_pwd; }
        }
        /// <summary>
        /// on update CURRENT_TIMESTAMP
        /// </summary>
        public DateTime? a_addtime
        {
            set { _a_addtime = value; }
            get { return _a_addtime; }
        }
        #endregion Model

    }
}


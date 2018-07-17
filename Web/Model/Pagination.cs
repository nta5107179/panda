using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public partial class Pagination
    {
        public Pagination()
        { }
        int _page;
        int _limit;
        long _total;

        public int page
        {
            set { _page = value; }
            get { return _page; }
        }

        public int limit
        {
            set { _limit = value; }
            get { return _limit; }
        }

        public long total
        {
            set { _total = value; }
            get { return _total; }
        }
    }
}

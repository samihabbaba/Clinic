using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic.Helpers
{
    public class ReportParameters
    {
        //Standard Report Parameters
        private int _pageSize = 10;
        const int maxPageSize = 200;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        public string OrderBy { get; set; } = "Id";
        public string SortingStatus { get; set; }
        public string SearchQuery { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        


    }
}

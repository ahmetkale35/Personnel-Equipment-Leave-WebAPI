using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public abstract class RequestParameters
    {
        const int maxPageSize = 50;

        //Auto implemented properties
        public int PageNumber { get; set; }


        //Full property 
        private int _pageSize;

        // The setter ensures that the page size does not exceed the maximum allowed value
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }

        // Sorting order, e.g., "name desc" or "age asc"
        public String? OrderBy { get; set; }

        // Fields to be selected in the response, e.g., "name,age"
        public String? Fields { get; set; }
    }
}

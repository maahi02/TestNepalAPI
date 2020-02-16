using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNepal.Dtos
{
    public class PaginationViewModel
    {
        public string Sort { get; set; }
        public string Order { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
    }
}

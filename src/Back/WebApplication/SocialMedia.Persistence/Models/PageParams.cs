using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Persistence.Models
{
    public class PageParams
    {
        public int PageNumber { get; set; } = 1;

        public int pageSize = 10;

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value;
        }

    }
}

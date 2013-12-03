using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grain.Cache
{
    public class CacheProfile : ICacheProfile
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public TimeSpan? ExpiresIn { get; set; }
    }
}

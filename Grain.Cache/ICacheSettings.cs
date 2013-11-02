using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grain.Cache
{
    public interface ICacheProfile
    {
        string Name { get; set; }
        string Description { get; set; }
        string Group { get; set; }
        TimeSpan ExpiresIn { get; set; }
    }
}

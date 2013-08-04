using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grain.Attributes
{
    public partial class Cite : Attribute
    {
        public string Author { get; set; }
        public string Link { get; set; }
        public CiteType Type { get; set; }
        public string OtherType { get; set; }
        public string License { get; set; }
    }

    public enum CiteType 
    { 
        Adaptation,
        Copy,
        Inspiration,
        Research,
        Other
    }

    //public enum License 
    //{
    //    MIT,
    //    ApacheV2
    //}
}

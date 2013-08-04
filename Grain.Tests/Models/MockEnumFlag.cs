using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grain.Tests.Models.TestModels
{
    [Flags]
    public enum MockEnumFlag
    {
        None = 0,
        One = 1,
        Two = 2,
        Four = 4,
        Eight = 8,
        Sixteen = 16,
        ThirtyTwo = 32,
        SixtyFour = 64,
        OneTwentyEight = 128
    }

    [Flags]
    public enum MockLongEnumFlag : long
    {
        None = 0,
        One = 1,
        Two = 2,
        Four = 4,
        Eight = 8,
        Sixteen = 16,
        ThirtyTwo = 32,
        SixtyFour = 64,
        OneTwentyEight = 128
    }
}

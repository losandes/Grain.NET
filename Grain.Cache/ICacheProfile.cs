using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grain.Cache
{
    public interface ICacheProfile
    {
        /// <summary>
        /// The Name of the Cache Profile
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// A Description of what makes this profile unique
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The name of the group that will be used to organize items that are cached with this profile into.
        /// (i.e. with a group named "settings" and a key named, ".emailIsOn", the cache key might be "settings.emailIsOn" 
        /// (the actual concatenated value depends on the provider implementation)).
        /// </summary>
        string Group { get; set; }

        /// <summary>
        /// The duration that items with this profile are persisted in the cache (i.e. HH:MM:SS)
        /// </summary>
        TimeSpan? ExpiresIn { get; set; }
    }
}

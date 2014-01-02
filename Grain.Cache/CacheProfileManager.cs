using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grain.Configuration;

namespace Grain.Cache
{
    public static class CacheProfileManager
    {
        private static readonly ConcurrentDictionary<string, ICacheProfile> _profiles = new ConcurrentDictionary<string, ICacheProfile> { };

        /// <summary>
        /// Get a profile from configuration
        /// </summary>
        /// <param name="name">the name of the profile you wish to retrieve</param>
        /// <returns>ICacheProfile: the profile from config, if it exists</returns>
        public static ICacheProfile GetProfile(string name) 
        {
            if(_profiles.Any(p => p.Key == name))
                return _profiles.FirstOrDefault(p => p.Key == name).Value;

            RefreshProfiles();
            
            if (_profiles.Any(p => p.Key == name))
                return _profiles.FirstOrDefault(p => p.Key == name).Value;

            return null;
        }

        private static void RefreshProfiles() 
        {
            string _grainCacheSectionName = ConfigManager.TryGetValue("Grain.CacheConfigSectionName", "grainCache");
            var _section = ConfigManager.GetSection<GrainCacheProfiles>(_grainCacheSectionName);
            Dictionary<string, ICacheProfile> _tempProfiles = new Dictionary<string, ICacheProfile> { };

            foreach (ProfileElement profile in _section.Profiles)   // make sure all profiles found in the config are added to the concurrent dictionary
            {
                var _profile = profile.ToCacheProfile();
                if (!_profiles.Any(p => p.Key == _profile.Name))
                    _profiles.TryAdd(_profile.Name, _profile);

                _tempProfiles.Add(_profile.Name, _profile);
            }

            foreach (var profile in _profiles)                      // make sure there are no profiles in the dictionary, that are no longer in the config
            {
                if (!_tempProfiles.ContainsKey(profile.Key)) 
                {
                    ICacheProfile _temp = new CacheProfile();
                    _profiles.TryRemove(profile.Key, out _temp);
                }
            }
        }
    }
}

using System.Collections.Generic;
using System.Runtime.Serialization;
using Grain.Configuration;
using Grain.Extensions;
using Grain.Serialization;

namespace Grain.Repositories
{

    [DataContract]
    public partial class PluralizationRepository
    {
        private static object LOCK = new object();
        private static PluralizationRepository _repository = null;

        protected PluralizationRepository()
        {
            string _pluralizationWords = ConfigManager.TryGetValue("Grain.PluralizationWords");
            string _pluralizationOverrides = ConfigManager.TryGetValue("Grain.PluralizationOverrides");

            if (_pluralizationWords.IsNotEmptyOrWhiteSpace())
            {
                PluralizationWords = _pluralizationWords.ToList(',');
            }
            else 
            {
                PluralizationWords = new List<string> { };
            }

            if (_pluralizationOverrides.IsNotEmptyOrWhiteSpace())
            {
                PluralizationOverrides = _pluralizationOverrides.FromJson<Dictionary<string, string>>();
            }
            else
            {
                PluralizationOverrides = new Dictionary<string, string> { };
            }
        }

        /// <summary>
        /// Singleton
        /// 
        /// Sample Configuration:
        ///   add key="PluralizationWords" value="Status,Statuses" />
        ///   add key="PluralizationOverrides" value="{&quot;Status&quot;:&quot;Statuses&quot;}" />
        /// </summary>
        /// <returns>The One and Only AuthorizedClients Instance</returns>
        /// <remarks>
        /// For more information on Implementing Singltons in C#: http://msdn.microsoft.com/en-us/library/ff650316.aspx 
        /// </remarks>
        public static PluralizationRepository Instance()
        {
            lock (LOCK)
            {
                if (_repository == null)
                {
                    _repository = new PluralizationRepository();
                }
            }

            return _repository;
        }

        /// <summary>
        /// Non-English words or words that are known to invoke errors in the Pluralization library
        /// </summary>
        public virtual List<string> PluralizationWords { get; set; }

        /// <summary>
        /// A dictionary of overrides formatted using the convention, {singular, plural}
        /// </summary>
        public virtual Dictionary<string, string> PluralizationOverrides { get; set; }
    }
}
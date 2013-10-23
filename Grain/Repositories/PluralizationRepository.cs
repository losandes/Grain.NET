using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Grain.Configuration;
using Grain.Extensions;
using Grain.Serialization;

namespace Grain.Repositories
{
    [DataContract]
    public partial class PluralizationRepository 
    {
        private static Lazy<PluralizationRepository> instance =
            new Lazy<PluralizationRepository>(
                delegate
                {
                    return new PluralizationRepository();
                }
                    // LazyThreadSafetyMode.PublicationOnly means each thread accessing the unitialised Lazy will generate a new() on T 
                    // until the first to complete the initialisation. That value is then returned for all threads currently accessing .Value 
                    // and their own new() instances are discarded.
                , System.Threading.LazyThreadSafetyMode.PublicationOnly);

        private PluralizationRepository() 
        {
            PluralizationOverrides = new Dictionary<string, string> { };
            PluralizationWords = new HashSet<string> { };

            string _pluralizationOverrides = ConfigManager.TryGetValue("Grain.PluralizationOverrides");

            if (_pluralizationOverrides.IsNotEmptyOrWhiteSpace())
            {
                PluralizationOverrides = _pluralizationOverrides.FromJson<Dictionary<string, string>>();

                foreach (var pair in PluralizationOverrides)
                {
                    PluralizationWords.Add(pair.Key);
                    PluralizationWords.Add(pair.Value);
                }
            }
        }

        /// <summary>
        /// A list of the words, both singular and plural versions, that will be used when overriding Microsoft's pluralization library
        /// </summary>
        public ICollection<string> PluralizationWords { get; private set; }
        
        /// <summary>
        /// A dictionary of the words, where the Key is the singular version and the Value is the plural version, that will be used when overriding Microsoft's pluralization library
        /// </summary>
        public IDictionary<string, string> PluralizationOverrides { get; private set; }

        /// <summary>
        /// Get the PluralizationRepo instance
        /// </summary>
        public static PluralizationRepository Instance
        {
            get { return instance.Value; }
        }

        /// <summary>
        /// Get the PluralizationRepo instance
        /// </summary>
        public static PluralizationRepository GetInstance() 
        {
            return instance.Value;
        }

        /// <summary>
        /// Add a pluralization override to the repository
        /// </summary>
        /// <param name="singularForm">The singular form of a word (i.e. Quiz)</param>
        /// <param name="pluralForm">The plural form of a word (i.e Quizzes)</param>
        /// <returns>true if the addition completes successfully</returns>
        public bool AddPluralizationOverride(string singularForm, string pluralForm) 
        {
            if (PluralizationOverrides.ContainsKey(singularForm))
            {
                PluralizationOverrides.Replace(singularForm, pluralForm);
            }
            else 
            {
                PluralizationOverrides.Add(singularForm, pluralForm);
            }
            
            if(!PluralizationWords.Contains(singularForm))
                PluralizationWords.Add(singularForm);

            if (!PluralizationWords.Contains(pluralForm))
                PluralizationWords.Add(pluralForm);
            
            return true;
        }

        /// <summary>
        /// Remove a pluralization override from the repository
        /// </summary>
        /// <param name="singularForm">singular form of the word you wish to remove from the repository</param>
        /// <returns>true if the addition completes successfully</returns>
        public bool RemovePluralizationOverride(string singularForm) 
        {
            if (PluralizationOverrides.ContainsKey(singularForm))
            {
                var _kvp = PluralizationOverrides.FirstOrDefault(x => x.Key == singularForm);
                PluralizationWords.Remove(_kvp.Key);
                PluralizationWords.Remove(_kvp.Value);
                PluralizationOverrides.Remove(singularForm);
            }

            return true;        
        }

        /// <summary>
        /// Add a collection of pluralization overrides to the repository. This is not designed for best performance, as it is expected to be used in 
        /// an AppStart routine, rather than over and over again.
        /// </summary>
        /// <param name="wordPairs">a dictionary of singularForm, pluralForm values to add to the repository</param>
        /// <returns>true if the addition completes successfully</returns>
        public bool AddPluralizationOverrides(IDictionary<string, string> wordPairs) 
        {
            foreach (var pair in wordPairs) 
            {
                AddPluralizationOverride(pair.Key, pair.Value);
            }

            return true;
        }

        /// <summary>
        /// Removes a collection of pluralization overrides from the repository. This is not designed for best performance, as it is expected to be used in 
        /// an AppStart routine, rather than over and over again.
        /// </summary>
        /// <param name="singularForms">a list of the singular form of the words you wish to remove from the repository</param>
        /// <returns>true if the addition completes successfully</returns>
        public bool RemovePluralizationOverrides(ICollection<string> singularForms)
        {
            foreach (var word in singularForms)
            {
                RemovePluralizationOverride(word);
            }

            return true;
        }

        /// <summary>
        /// Removes a collection of pluralization overrides from the repository. This is not designed for best performance, as it is expected to be used in 
        /// an AppStart routine, rather than over and over again.
        /// </summary>
        /// <param name="wordPairs">a dictionary of singularForm, pluralForm values to remove from the repository</param>
        /// <returns>true if the addition completes successfully</returns>
        public bool RemovePluralizationOverrides(IDictionary<string, string> wordPairs)
        {
            foreach (var pair in wordPairs)
            {
                RemovePluralizationOverride(pair.Key);
            }

            return true;
        }
    }
}
using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using Grain.Repositories;

namespace Grain.Extensions
{
    public static partial class PluralizationExtensions
    {
        // TODO: use the singleton pattern to deliver this
        // TODO: add overloads that allow the culture to be set.

        /// <summary>
        /// Convenience method for pluralizing words
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string Pluralize(this string word)
        {
            if (word.IsEmptyOrWhiteSpace())
                throw new NullReferenceException("Null cannot be pluralized.");

            var _repository = PluralizationRepository.Instance;

            if (_repository.PluralizationWords.Contains(word)) 
            {
                var _row = _repository.PluralizationOverrides.FirstOrDefault(w => w.Key == word || w.Value == word);
                return _row.Value;
            }
            
            PluralizationService _svc = PluralizationService.CreateService(CultureInfo.CurrentCulture);

            if (_svc.IsSingular(word))
            {
                return _svc.Pluralize(word);
            }
            else return word;
        }

        /// <summary>
        /// Convenience method for singularizing words
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string Singularize(this string word)
        {
            if (word.IsEmptyOrWhiteSpace())
                throw new NullReferenceException("Null cannot be singularized.");

            var _repository = PluralizationRepository.Instance;

            if (_repository.PluralizationWords.Contains(word))
            {
                var _row = _repository.PluralizationOverrides.FirstOrDefault(w => w.Key == word || w.Value == word);
                return _row.Key;
            }

            PluralizationService _svc = PluralizationService.CreateService(CultureInfo.CurrentCulture);

            if (_svc.IsPlural(word))
            {
                return _svc.Singularize(word);
            }
            else return word;
        }
    }
}

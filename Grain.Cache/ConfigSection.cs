using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grain.Cache
{
    /// <summary>
    /// A class for the DataManager configuration settings section.
    /// </summary>
    public partial class GrainCacheProfiles : ConfigurationSection
    {
        /// <summary>
        /// Gets the Data Provider profiles.
        /// </summary>
        /// <value>The cache profiles.</value>
        [ConfigurationProperty("profiles")]
        public ProfileElementCollection Profiles
        {
            get
            {
                return this["profiles"] as ProfileElementCollection;
            }
        }
    }

    /// <summary>
    /// A collection for <see cref="ProfileElement"/>.
    /// </summary>    
    public partial class ProfileElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ProfileElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement"/> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            var profileElement = element as ProfileElement;

            if (profileElement == null)
                throw new ArgumentException("The specified element is not of the correct type.");

            return profileElement.Name;
        }
    }

    public partial class ProfileElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the name of the profile.
        /// </summary>
        /// <value>The name of the profile.</value>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the description for the profile.
        /// </summary>
        /// <value>The description for the profile.</value>
        [ConfigurationProperty("description", DefaultValue = "")]
        public string Description
        {
            get { return this["description"] as string; }
            set { this["description"] = value; }
        }

        /// <summary>
        /// Gets or sets the description for the profile.
        /// </summary>
        /// <value>The description for the profile.</value>
        [ConfigurationProperty("group", DefaultValue = "")]
        public string Group
        {
            get { return this["group"] as string; }
            set { this["group"] = value; }
        }

        /// <summary>
        /// Gets or sets the expiration duration.
        /// </summary>
        /// <value>The expiration duration.</value>
        [ConfigurationProperty("expiresIn", IsRequired = true)]
        public TimeSpan ExpiresIn
        {
            get { return (TimeSpan)this["expiresIn"]; }
            set { this["expiresIn"] = value; }
        }

        /// <summary>
        /// Casts the profile to a CacheProfile
        /// </summary>
        /// <returns></returns>
        public ICacheProfile ToCacheProfile() 
        {
            return new CacheProfile { 
                 Name = this.Name,
                 Description = this.Description,
                 Group = this.Group,
                 ExpiresIn = this.ExpiresIn
            };
        }
    }
}

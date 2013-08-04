using System;
using System.Configuration;
using Grain.Serialization;

namespace Grain.Configuration
{
    public partial class ConfigManager
    {
        /// <summary>
        /// Gets the connection string value from the configuration file
        /// </summary>
        /// <param name="name">string: the name of the connection string</param>
        /// <returns>string: a database connection string</returns>
        public static string GetConnectionString(string name)
        {
            if (name == null)
                throw new NullReferenceException("A name must be provided to get connection strings from the configuration file");

            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        /// <summary>
        /// Tries to get the connection string value from the configuration file.  Returns null if it is not found.
        /// </summary>
        /// <param name="name">string: the name of the connection string</param>
        /// <returns>string: a database connection string</returns>
        public static string TryGetConnectionString(string name)
        {
            return TryGetConnectionString(name, null);
        }

        /// <summary>
        /// Tries to get the connection string value from the configuration file.  Returns null if it is not found.
        /// </summary>
        /// <param name="name">string: the name of the connection string</param>
        /// <param name="defaultValue">string: the value to use if a connection string is not found</param>
        /// <returns>string: a database connection string</returns>
        public static string TryGetConnectionString(string name, string defaultValue)
        {
            if (name == null)
                throw new NullReferenceException("A name must be provided to get connection strings from the configuration file");

            try
            {
                return ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Tries to get a value from the config file (App.config, Web.config).  If no key is found, an exception is thrown.
        /// </summary>
        /// <param name="key">string: the key of the key value pair.</param>
        /// <returns>string: the value of the key value pair.</returns>
        public static string GetValue(string key)
        {
            if (key == null)
                throw new NullReferenceException("A key must be provided to get values from the configuration file");

            try
            {
                if (System.Web.HttpContext.Current == null)
                    return System.Configuration.ConfigurationManager.AppSettings[key];
                else
                    return System.Web.Configuration.WebConfigurationManager.AppSettings[key];
            }
            catch (Exception e)
            {
                throw new Exception("A setting with the key, " + key + ", does not exist in the config file: \r\n" + e.Message);
            }
        }

        /// <summary>
        /// Tries to get a value from the config file (App.config, Web.config).  Returns null if no key is found.
        /// </summary>
        /// <param name="key">string: the key of the key value pair.</param>
        /// <returns>string: the value of the key value pair or null.</returns>
        public static string TryGetValue(string key)
        {
            return TryGetValue(key, null);
        }

        /// <summary>
        /// Tries to get a value from the config file (App.config, Web.config).  Returns null if no key is found.
        /// </summary>
        /// <param name="key">string: the key of the key value pair.</param>
        /// <param name="defaultValue">string: the value to use of a setting with this key is not found</param>
        /// <returns>string: the value of the key value pair or null.</returns>
        public static string TryGetValue(string key, string defaultValue)
        {
            if (key == null)
                throw new NullReferenceException("A key must be provided to get values from the configuration file");

            try
            {
                var _result = GetValue(key);

                if (_result != null)
                    return _result;
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Tries to get a value from the config file (App.config, Web.config) as Type T.  Returns null if no key is found.
        /// </summary>
        /// <param name="key">string: the key of the key value pair.</param>
        /// <param name="defaultValue">string: the value to use of a setting with this key is not found</param>
        /// <returns>string: the value of the key value pair or null.</returns>
        public static T TryGetValueFromBinaryAs<T>(string key, T defaultValue)
        {
            if (key == null)
                throw new NullReferenceException("A key must be provided to get values from the configuration file");

            try
            {
                string _value = TryGetValue(key, defaultValue.ToString());
                return Conversions.TryConvert<T>(_value);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Tries to get a value from the config file (App.config, Web.config) as Type T.  Returns null if no key is found.
        /// </summary>
        /// <param name="key">string: the key of the key value pair.</param>
        /// <param name="type">SerializationTypes: the type of serializer to use</param>
        /// <param name="defaultValue">string: the value to use of a setting with this key is not found</param>
        /// <returns>string: the value of the key value pair or null.</returns>
        public static T TryGetValueAs<T>(string key, SerializationTypes type, T defaultValue)
        {
            if (key == null)
                throw new NullReferenceException("A key must be provided to get values from the configuration file");

            try
            {
                switch (type)
                {
                    case SerializationTypes.Xml:
                        return TryGetValueAs<T>(key, defaultValue, r => r.FromXml<T>(), r => r.ToXml<T>());
                    default:
                        return TryGetValueAs<T>(key, defaultValue, r => r.FromJson<T>(), r => r.ToJson<T>());
                }
                //string _value = TryGetValue(key, defaultValue.ToString());

                //switch (type) 
                //{ 
                //    case SerializationTypes.Xml:
                //        return _value.FromXml<T>();
                //    default:
                //        return _value.FromJson<T>();
                //}
            }
            catch
            {
                return (T)defaultValue;
            }
        }

        /// <summary>
        /// Tries to get a value from the config file (App.config, Web.config) as Type T.  Returns null if no key is found.
        /// </summary>
        /// <param name="key">string: the key of the key value pair.</param>
        /// <param name="defaultValue">string: the value to use of a setting with this key is not found</param>
        /// <param name="fromStringBinder">The factory that converts the string that is retrieved from data, to the final value</param>
        /// <param name="toStringBinder">the facotry that converts the defaultValue to a string, if the setting needs to be created</param>
        /// <returns>string: the value of the key value pair or null.</returns>
        public static T TryGetValueAs<T>(string key, T defaultValue, Func<string, T> fromStringBinder, Func<T, string> toStringBinder)
        {
            if (key == null)
                throw new NullReferenceException("A key must be provided to get values from the configuration file");

            try
            {
                string _value = TryGetValue(key, toStringBinder(defaultValue));
                return fromStringBinder(_value);
            }
            catch
            {
                return (T)defaultValue;
            }
        }

        /// <summary>
        /// Tries to get a value from the config file (App.config, Web.config) as an int.  Returns null if no key is found.
        /// </summary>
        /// <param name="key">string: the key of the key value pair.</param>
        /// <param name="defaultValue">string: the value to use of a setting with this key is not found</param>
        /// <returns>string: the value of the key value pair or null.</returns>
        public static int TryGetValueAsInt(string key, int defaultValue)
        {
            if (key == null)
                throw new NullReferenceException("A key must be provided to get values from the configuration file");

            try
            {
                int _setting = Int32.Parse(TryGetValue(key, defaultValue.ToString()));
                return _setting;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Tries to get a value from the config file (App.config, Web.config) as a bool.  Returns null if no key is found.
        /// </summary>
        /// <param name="key">string: the key of the key value pair.</param>
        /// <param name="defaultValue">string: the value to use of a setting with this key is not found</param>
        /// <returns>string: the value of the key value pair or null.</returns>
        public static bool TryGetValueAsBool(string key, bool defaultValue)
        {
            if (key == null)
                throw new NullReferenceException("A key must be provided to get values from the configuration file");

            try
            {
                bool _setting = bool.Parse(TryGetValue(key, defaultValue.ToString()));
                return _setting;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Tries to get a value from the config file (App.config, Web.config) as a DateTime.  Returns null if no key is found.
        /// </summary>
        /// <param name="key">string: the key of the key value pair.</param>
        /// <param name="defaultValue">string: the value to use of a setting with this key is not found</param>
        /// <returns>string: the value of the key value pair or null.</returns>
        public static DateTime TryGetValueAsDateTime(string key, DateTime defaultValue)
        {
            if (key == null)
                throw new NullReferenceException("A key must be provided to get values from the configuration file");

            try
            {
                DateTime _setting = DateTime.Parse(TryGetValue(key, defaultValue.ToString()));
                return _setting;
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}

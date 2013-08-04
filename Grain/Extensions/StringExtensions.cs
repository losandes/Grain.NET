using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Grain.Attributes;

namespace Grain.Extensions
{
	public static partial class StringExtensions
	{
		#region Adding, Replacing and Removing Characters

		/// <summary>
		/// Removes the given characters from the input string
		/// </summary>
		/// <param name="input">string: the string to process</param>
		/// <param name="characters">string: the characters to remove from the string</param>
		/// <returns>string: the input string without the characters that were to be removed.</returns>
		public static string Remove(this string input, string characters)
		{
			if (input == null || characters == null)
				return input;

			return input.Replace(characters, "");
		}

		/// <summary>
		/// Removes any spaces that occur more than once in a row (i.e. double spaces)
		/// </summary>
		/// <param name="input">string: the string to process</param>
		/// <returns>string: the formatted string</returns>
		public static string RemoveExtraSpaces(this string input)
		{
			if (input == null) { return input; }
			while (input.IndexOf("  ") != -1)           // while double spaces exist in the string
			{
				input = input.Replace("  ", " ");       // emilinate doulbe spaces
			}

			return input;
		}

		/// <summary>
		/// Removes spaces in a string
		/// </summary>
		/// <param name="input">string: the string to process</param>
		/// <returns>string: the formatted string</returns>
		public static string RemoveSpaces(this string input)
		{
			return input.ReplaceSpaces("");
		}

		/// <summary>
		/// Removes any spaces that occur more than once in a row (i.e. double spaces) and then replaces single spaces with the
		/// character that was passed as a parameter
		/// </summary>
		/// <param name="input">string: the string to process</param>
		/// <param name="characterToReplaceWith">string: the character to replace spaces with</param>
		/// <returns>string: the formatted string</returns>
		public static string ReplaceSpaces(this string input, string characterToReplaceWith)
		{
			if (input == null)
				return input;

			characterToReplaceWith = characterToReplaceWith != null ? characterToReplaceWith : "";

			input = input.RemoveExtraSpaces();
			input = input.Replace(" ", characterToReplaceWith);        // replace single spaces with the character that was specified

			return input.Trim();
		}

		/// <summary>
		/// Removes all punctuation in a string.  Special Characters are retained.
		/// </summary>
		/// <param name="input">string: the string to process</param>
		/// <returns>string: the formatted string</returns>
		public static string RemovePunctuation(this string input)
		{
			if (input == null)
				return input;

			var _chars = input.ToCharArray();
			return new string(_chars.Where(c => !char.IsPunctuation(c)).ToArray());
		}

		/// <summary>
		/// Removes all punctuation and special characters in a string
		/// </summary>
		/// <param name="input">string: the string to process</param>
		/// <returns>string: the formatted string</returns>
		public static string RemoveSpecialCharacters(this string input)
		{
			return input.RemoveSpecialCharacters(false);
		}

		/// <summary>
		/// Removes a subset of punctuation and special characters in a string.  If preserveUnderscores is true, then underscores are retained.
		/// </summary>
		/// <param name="input">string: the string to process</param>
		/// <param name="preserveUnderscores">bool: underscores are retained when this is true, otherwise they are removed</param>
		/// <returns>string: the formatted string</returns>
		public static string RemoveSpecialCharacters(this string input, bool preserveUnderscores)
		{
			if (input == null)
				return input;

			if (preserveUnderscores)
				return input.ReplaceSpecialCharacters("");

			return input.ReplaceSpecialCharacters("").Remove("_");
		}

		/// <summary>
		/// Replaces a subset of punctuation in a string with the character that is passed as a parameter
		/// </summary>
		/// <param name="input">string: the string to process</param>
		/// /// <param name="characterToReplaceWith">string: the character to replace spaces with</param>
		/// <returns>string: the formatted string</returns>
		public static string ReplaceSpecialCharacters(this string input, string characterToReplaceWith)
		{
			if (input == null)
				return input;

			characterToReplaceWith = characterToReplaceWith != null ? characterToReplaceWith : "";

			foreach (string character in Characters)
			{
				input = input.Replace(character, characterToReplaceWith);
			}

			return input;
		}

		/// <summary>
		/// Removes the content between the startAndEndChar character
		/// </summary>
		/// <param name="input">string: the string to process</param>
		/// <param name="startAndEndChar">char: the beginning and ending character deliminator</param>
		/// <returns>string: the formatted string</returns>
		public static string RemoveBetween(this string input, char startAndEndChar)
		{
			return input.RemoveBetween(startAndEndChar, startAndEndChar);
		}

		/// <summary>
		/// Removes the content between start and end characters
		/// </summary>
		/// <param name="input">string: the string to process</param>
		/// <param name="startChar">char: the beginning character deliminator</param>
		/// <param name="endChar">char: the ending character deliminator</param>
		/// <returns>string: the formatted string</returns>
		public static string RemoveBetween(this string input, char startChar, char endChar)
		{
			Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", startChar, endChar));
			return regex.Replace(input, string.Empty);
		}

		/// <summary>
		/// Gets the content between the startAndEndChar character and returns it in a list
		/// </summary>
		/// <param name="input">string: the string to process</param>
		/// <param name="startAndEndChar">char: the beginning and ending character deliminator</param>
		/// <returns>List of type string: the phrases that were found between the start and end characters</returns>
		public static List<string> GetBetween(this string input, char startAndEndChar)
		{
			return input.GetBetween(startAndEndChar, startAndEndChar);
		}

		/// <summary>
		/// Gets the content between the start and end characters and returns it in a list
		/// </summary>
		/// <param name="input">string: the string to process</param>
		/// <param name="startChar">char: the beginning character deliminator</param>
		/// <param name="endChar">char: the ending character deliminator</param>
		/// <returns>List of type string: the phrases that were found between the start and end characters</returns>
		public static List<string> GetBetween(this string input, char startChar, char endChar)
		{
			List<string> _phrases = new List<string> { };
			Match _match = Regex.Match(input, string.Format("\\{0}.*?\\{1}", startChar, endChar));
			foreach (Group group in _match.Groups)
			{
				string _value = group.Value.Replace(startChar.ToString(), string.Empty).Replace(endChar.ToString(), string.Empty);
				if (_value.Trim().Length > 0)
				{
					_phrases.Add(_value);
				}
			}

			return _phrases;
		}

		/// <summary>
		/// Adds a Css Class to an existing class string
		/// </summary>
		/// <param name="cssClass"></param>
		/// <param name="classToAdd"></param>
		/// <returns></returns>
		public static string AppendCssClass(this string cssClass, string classToAdd)
		{
			if (cssClass != null && cssClass.Contains(classToAdd))
			{
				return cssClass;
			}
			else if (cssClass != null && cssClass.Length > 0)
			{
				return cssClass.RemoveExtraSpaces().Trim() + " " + classToAdd.Trim();
			}
			else
			{
				return classToAdd.Trim();
			}
		}

		/// <summary>
		/// Ensures that the last character of a string is a backslash ( \ ), for use with path concatenation.
		/// For instance, if the path "C:\Users\blah" is provided as the input, the result will be "C:\Users\blah\". 
		/// If the backslash is already present, then nothing is changed.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string ForceLastCharacterBackSlash(this string input)
		{
			if (input.IsEmptyOrWhiteSpace())
				return @"\";

			if (input.Substring(Math.Max(0, input.Length - 1)) != @"\")
			{
				return input + @"\";
			}

			return input;
		}

		/// <summary>
		/// Ensures that the last character of a string is a forward slash ( / ), for use with path concatenation.
		/// For instance, if the path "/Controller/Action" is provided as the input, the result will be "/Controller/Action/". 
		/// If the forward slash is already present, then nothing is changed.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string ForceLastCharacterForwardSlash(this string input)
		{
			if (input.IsEmptyOrWhiteSpace())
				return @"/";

			if (input.Substring(Math.Max(0, input.Length - 1)) != @"/")
			{
				return input + @"/";
			}

			return input;
		}

		/// <summary>
		/// Ensures that the last character of a string is NOT a forward slash ( / ), for use with path concatenation.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string RemoveLastCharacterForwardSlash(this string input)
		{
			if (input.IsEmptyOrWhiteSpace())
				return @"/";

			while(input.Substring(Math.Max(0, input.Length - 1)) == @"/")
			{
				input = input.Substring(0, input.Length - 1);
			}

			return input;
		}

		/// <summary>
		/// Ensures that the first character of a string is a forward slash ( / ), for use with path concatenation.
		/// For instance, if the path "Controller/Action/" is provided as the input, the result will be "/Controller/Action/". 
		/// If the forward slash is already present, then nothing is changed.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string ForceFirstCharacterForwardSlash(this string input)
		{
			if (input.IsEmptyOrWhiteSpace())
				return @"/";

			if (input.Substring(0, 1) != @"/")
			{
				return @"/" + input;
			}

			return input;
		}

		/// <summary>
		/// Ensures that the first character of a string is NOT a forward slash ( / ), for use with path concatenation.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string RemoveFirstCharacterForwardSlash(this string input)
		{
			if (input.IsEmptyOrWhiteSpace())
				return @"/";

			while (input.Substring(0, 1) == @"/")
			{
				input = input.Substring(1, input.Length -1);
			}

			return input;
		}

		/// <summary>
		/// Ensures that the first and last characters of a string are forward slashes ( / ), for use with path concatenation.
		/// For instance, if the path "Controller/Action" is provided as the input, the result will be "/Controller/Action/". 
		/// If the forward slash is already present, then nothing is changed.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string ForceFirstAndLastCharacterForwardSlashes(this string input)
		{
			return input.ForceFirstCharacterForwardSlash().ForceLastCharacterForwardSlash();
		}

		public static string AppendUrl(this string url, string relativePath) 
		{
			return url.RemoveLastCharacterForwardSlash()
					+ relativePath.ForceFirstCharacterForwardSlash();
		}

		#endregion Adding, Replacing and Removing Characters

		#region Casts To Strings

		/// <summary>
		/// Casts a Guid to an upper case string
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		public static string ToUpper(this Guid guid)
		{
			return guid.ToString().ToUpper();
		}

		/// <summary>
		/// Casts a Guid to a lower case string
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		public static string ToLower(this Guid guid)
		{
			return guid.ToString().ToLower();
		}

		/// <summary>
		/// Converts a List of objects to a string.  This method assumes that the object type can be cast to a string
		/// </summary>
		/// <param name="input">The List of objects to stringify</param>
		/// <returns>string: the object contents as a list, if they can be cast to a string, otherwise a string filled with the object types</returns>
		public static string ToString<T>(this List<T> input)
		{
			return input.ToDeliminatedString<T>("", false);

			//string _outputString = "";
			//foreach (T _tag in input)
			//{
			//    _outputString += _tag.ToString();
			//}

			//return _outputString;
		}

		/// <summary>
		/// Converts a List of objects to a string and allows you to set the deliminator and choose whether or not to remove the final deliminator.  
		/// This method assumes that the object type can be cast to a string
		/// </summary>
		/// <param name="input">The List of objects to stringify</param>
		/// <param name="deliminator">string: the character to deliminate the List values with</param>
		/// <returns>string: the object contents as a list, if they can be cast to a string, otherwise a string filled with the object types</returns>
		public static string ToDeliminatedString<T>(this List<T> input, string deliminator)
		{
			return input.ToDeliminatedString(deliminator, true);
		}

		/// <summary>
		/// Converts a List of objects to a string and allows you to set the deliminator and choose whether or not to remove the final deliminator.  
		/// This method assumes that the object type can be cast to a string
		/// </summary>
		/// <param name="input">The List of objects to stringify</param>
		/// <param name="deliminator">string: the character to deliminate the List values with</param>
		/// <param name="removeLastDeliminator">bool: true, if the last deliminator should be removed from the string</param>
		/// <returns>string: the object contents as a list, if they can be cast to a string, otherwise a string filled with the object types</returns>
		public static string ToDeliminatedString<T>(this List<T> input, string deliminator, bool removeLastDeliminator)
		{
			if (input != null && input.Count() > 0)
			{
				string _outputString = "";
				foreach (T _tag in input)
				{
					_outputString += _tag.ToString() + deliminator;
				}

				if (removeLastDeliminator)
				{
					_outputString = _outputString.Remove(_outputString.Length - deliminator.Length, deliminator.Length);
				}

				return _outputString;
			}
			else return null;
		}

		/// <summary>
		/// Concatenates the value of the first string (the key, or name) with a deliminator and the value of the second string (the value).
		/// 
		/// For instance:
		/// string _identity = "Andy";
		/// string _guid = "90E852A1-FB91-4401-949A-AEF4905CA001";
		/// _identity.ToDeliminatedString(_guid, "^") will return "Andy^90E852A1-FB91-4401-949A-AEF4905CA001"
		/// </summary>
		/// <param name="key">string: the key or name</param>
		/// <param name="value">string: the value or second string</param>
		/// <param name="deliminator">string: the character to deliminate the strings with</param>        
		/// <returns>string: the two values with a deliminator in between</returns>>
		public static string ToDeliminatedString(this string key, string value, string deliminator)
		{
			string _output = key.Trim() + deliminator + value.Trim();
			return _output;
		}

		#endregion Casts To Strings

		#region Encoding

		/// <summary>
		/// Encrypts a string using MD5 and returns the encrypted string
		/// </summary>
		/// <param name="input">string: the string you wish to encode</param>
		/// <returns>string: the encrypted string</returns>
		public static string MD5Hash(this string input)
		{
			if (input == null)
				return input;

			System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
			bs = x.ComputeHash(bs);
			System.Text.StringBuilder s = new System.Text.StringBuilder();
			foreach (byte b in bs)
			{
				s.Append(b.ToString("x2").ToLower());
			}
			return s.ToString();
			//// Create a new instance of the MD5CryptoServiceProvider object.
			//MD5 md5Hasher = MD5.Create();

			//// Convert the input string to a byte array and compute the hash.
			//byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

			//// Create a new Stringbuilder to collect the bytes
			//// and create a string.
			//StringBuilder sBuilder = new StringBuilder();

			//// Loop through each byte of the hashed data 
			//// and format each one as a hexadecimal string.
			//for (int i = 0; i < data.Length; i++)
			//{
			//    sBuilder.Append(data[i].ToString("x2"));
			//}

			//// Return the hexadecimal string.
			//return sBuilder.ToString();
		}


		/// <summary>
		/// Verify's the content of an MD5 hash against the unencrypted string
		/// </summary>
		/// <param name="input">string: the unencrypted string</param>
		/// <param name="hash">string: the encrypted string</param>
		/// <returns>bool: true if the hash is valid</returns>
		public static bool VerifyMd5Hash(this string input, string hash)
		{
			if (input == null || hash == null)
				return false;

			// Create a StringComparer an compare the hashes.
			if (0 == StringComparer.OrdinalIgnoreCase.Compare(MD5Hash(input), hash))
				return true;

			return false;
		}

		/// <summary>
		/// Encodes a string as Base64
		/// </summary>
		/// <param name="input">string: the string to encode</param>
		/// <returns>string: the Base64 encoded string</returns>
		public static string ToBase64String(this string input) 
		{
			if (input == null)
				return input;

			byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(input);
			string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
			return returnValue;        
		}

		/// <summary>
		/// Decodes a string from Base64
		/// </summary>
		/// <param name="input">string: the string to decode</param>
		/// <returns>string: the string that was encoded as Base64</returns>
		public static string FromBase64String(this string input)
		{
			if (input == null)
				return input;

			byte[] encodedDataAsBytes = System.Convert.FromBase64String(input);
			string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
			return returnValue;
		}

		#endregion Encoding

		#region Lists and Dictionaries

		/// <summary>
		/// Determines whether an element is in the List, using StringComparison
		/// </summary>
		/// <param name="target">The string/target to compare against</param>
		/// <param name="value">The string to look for in the input/target</param>
		/// <param name="comparison">StringComparison: the comparison type</param>
		/// <returns>bool: true if the string contains the given value</returns>
		/// <example>myList.FindAll(s => s.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase));</example>
		[Cite(Link = "http://stackoverflow.com/questions/3107765/how-to-ignore-the-case-sensitivity-in-liststring")]
		public static bool Contains(this string target, string value, StringComparison comparison)
		{
			return target.IndexOf(value, comparison) >= 0;
		}

		/// <summary>
		/// A List of characters for use with string formatting
		/// </summary>
		public static List<string> Characters = new List<string>
		{
			"."
			,","
			,"-"
			,":"
			,";"
			,"'"
			,"\""
			,"/"
			,"\\"
			,"("
			,")"
			,"%"
			,"~"
			,"`"
			,"!"
			,"?"
			,"@"
			,"#"
			,"$"
			,"^"
			,"&"
			,"*"
			,"+"
			,"="
			,"["
			,"]"
			,"{"
			,"}"
			,"|"
			,"<"
			,">"
		};

		#endregion Lists and Dictionaries
	}
}

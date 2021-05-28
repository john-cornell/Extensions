
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;


public static class StringExtensions
{    
    public static string Reverse(this string me)
    {
        char[] charArray = me.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public static void ToConsole(this string me)
    {
        Console.WriteLine(me);
    }

    public static string RepairNewLines(this string me, char missingCharacter = '\r')
    {
        if (missingCharacter.NotIn('\r', '\n')) throw new InvalidOperationException($"Invalid new line character {missingCharacter}");
        char availableCharacter = missingCharacter == '\r' ? '\n' : '\r';
        //Tidy up in case some missing characters still there
        me = me.Replace(missingCharacter.ToString(), "");
        //Repair
        me = me.Replace(availableCharacter.ToString(), Environment.NewLine);

        return me;
    }

    public static bool FitsMask(this string me, string mask)
    {
        string pattern =
             '^' +
             Regex.Escape(mask.Replace(".", "__DOT__")
                             .Replace("*", "__STAR__")
                             .Replace("?", "__QM__"))
                 .Replace("__DOT__", "[.]")
                 .Replace("__STAR__", ".*")
                 .Replace("__QM__", ".")
             + '$';
        return new Regex(pattern, RegexOptions.IgnoreCase).IsMatch(me);
    }


    //For password comparison - already included in PasswordStorage.cs
    public static bool SlowEquals(this string me, string compare)
    {
        int diff = me.Length ^ compare.Length;

        for (int i = 0; i < me.Length && i < compare.Length; i++)
            diff |= me[i] ^ compare[i];

        return diff == 0;
    }

    public static string GenerateUniqueFileName(this string me)
    {
        if (File.Exists(me))
        {
            FileInfo info = new FileInfo(me);

            string extension = info.Extension;
            string fileStub = me.Left(me.Length - extension.Length);

            string newFile = "";

            int index = 1;

            do
            {
                index++;

                newFile = fileStub + " ({0})".F(index) + extension;

            }
            while (File.Exists(newFile));

            return newFile;
        }
        else
        {
            return me;
        }

    }

    public static bool StartsWith(this string me, IEnumerable<string> prefixes, StringComparison comparison = StringComparison.CurrentCulture)
    {
        foreach (string p in prefixes)
        {
            if (me.StartsWith(p, comparison)) return true;

        }

        return false;
    }

    public static IEnumerable<string> Lines(this string s)
    {
        StringBuilder builder = new StringBuilder();

        foreach (char item in s)
        {
            if (item == '\r')
            {
                yield return builder.ToString();
                builder.Clear();
            }

            if (item != '\n')
            {
                builder.Append(item);
            }
        }

        if (builder.Length > 0) yield return builder.ToString();
    }

    public static void Lines(this string s, Action<string> action)
    {
        StringBuilder builder = new StringBuilder();

        foreach (char item in s)
        {
            if (item == '\r')
            {
                action(builder.ToString());
                builder.Clear();
            }

            if (item != '\n')
            {
                builder.Append(item);
            }
        }

        if (builder.Length > 0) action(builder.ToString());
    }

    #region Strip
    /// <summary>
    /// Strip a string of the specified character.
    /// </summary>
    /// <param name="s">the string to process</param>
    /// <param name="char">character to remove from the string</param>
    /// <example>
    /// string s = "abcde";
    /// 
    /// s = s.Strip('b');  //s becomes 'acde;
    /// </example>
    /// <returns></returns>
    public static string Strip(this string s, char character)
    {
        s = s.Replace(character.ToString(), "");

        return s;
    }

    /// <summary>
    /// Strip a string of the specified characters.
    /// </summary>
    /// <param name="s">the string to process</param>
    /// <param name="chars">list of characters to remove from the string</param>
    /// <example>
    /// string s = "abcde";
    /// 
    /// s = s.Strip('a', 'd');  //s becomes 'bce;
    /// </example>
    /// <returns></returns>
    public static string Strip(this string s, params char[] chars)
    {
        foreach (char c in chars)
        {
            s = s.Replace(c.ToString(), "");
        }

        return s;
    }
    /// <summary>
    /// Strip a string of the specified substring.
    /// </summary>
    /// <param name="s">the string to process</param>
    /// <param name="subString">substring to remove</param>
    /// <example>
    /// string s = "abcde";
    /// 
    /// s = s.Strip("bcd");  //s becomes 'ae;
    /// </example>
    /// <returns></returns>
    public static string Strip(this string s, string subString)
    {
        s = s.Replace(subString, "");

        return s;
    }
    #endregion

    public static JObject XmlToJson(this string me)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(me);

        return JsonConvert.DeserializeObject<JObject>(Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc));
    }
    public static string EnsureFileNameIsUnique(this string intendedName)
    {
        if (!File.Exists(intendedName)) return intendedName;

        FileInfo file = new FileInfo(intendedName);
        string extension = file.Extension;
        string basePath = intendedName.Substring(0, intendedName.Length - extension.Length);

        int counter = 1;

        string newPath = "";

        do
        {
            newPath = string.Format("{0} - Copy{1}{2}", basePath, counter == 1 ? "" : string.Format(" ({0})", counter), extension);

            counter += 1;
        }
        while (File.Exists(newPath));

        return newPath;
    }

    #region Encryption

    /// <summary>
    /// Encryptes a string using the supplied key. Encoding is done using RSA encryption.
    /// </summary>
    /// <param name="stringToEncrypt">String that must be encrypted.</param>
    /// <param name="key">Encryptionkey.</param>
    /// <returns>A string representing a byte array separated by a minus sign.</returns>
    /// <exception cref="ArgumentException">Occurs when stringToEncrypt or key is null or empty.</exception>
    public static string Encrypt(this string stringToEncrypt, string key)
    {
        if (string.IsNullOrEmpty(stringToEncrypt))
        {
            throw new ArgumentException("An empty string value cannot be encrypted.");
        }

        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Cannot encrypt using an empty key. Please supply an encryption key.");
        }

        CspParameters cspp = new CspParameters();
        cspp.KeyContainerName = key;

        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspp))
        {
            rsa.PersistKeyInCsp = true;

            byte[] bytes = rsa.Encrypt(System.Text.UTF8Encoding.UTF8.GetBytes(stringToEncrypt), true);

            return BitConverter.ToString(bytes);
        }
    }

    /// <summary>
    /// Decryptes a string using the supplied key. Decoding is done using RSA encryption.
    /// </summary>
    /// <param name="stringToDecrypt">String that must be decrypted.</param>
    /// <param name="key">Decryptionkey.</param>
    /// <returns>The decrypted string or null if decryption failed.</returns>
    /// <exception cref="ArgumentException">Occurs when stringToDecrypt or key is null or empty.</exception>
    public static string Decrypt(this string stringToDecrypt, string key)
    {
        string result = null;

        if (string.IsNullOrEmpty(stringToDecrypt))
        {
            throw new ArgumentException("An empty string value cannot be encrypted.");
        }

        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Cannot decrypt using an empty key. Please supply a decryption key.");
        }

        try
        {
            CspParameters cspp = new CspParameters();
            cspp.KeyContainerName = key;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspp))
            {
                rsa.PersistKeyInCsp = true;

                string[] decryptArray = stringToDecrypt.Split(new string[] { "-" }, StringSplitOptions.None);
                byte[] decryptByteArray = Array.ConvertAll<string, byte>(decryptArray, (s => Convert.ToByte(byte.Parse(s, System.Globalization.NumberStyles.HexNumber))));


                byte[] bytes = rsa.Decrypt(decryptByteArray, true);

                result = System.Text.UTF8Encoding.UTF8.GetString(bytes);
            }

        }
        finally
        {
            // no need for further processing
        }

        return result;
    }

    #endregion

    #region Occurences

    public static int CountOccurences(this string me, string substring)
    {
        int count = 0, n = 0;

        while ((n = me.IndexOf(substring, n, StringComparison.InvariantCulture)) != -1)
        {
            n += substring.Length;
            ++count;
        }

        return count;
    }

    #endregion

    #region Left / Right

    public static string Left(this string me, int length)
    {
        length = Math.Max(length, 0);

        if (me.Length > length)
        {
            return me.Substring(0, length);
        }
        else
        {
            return me;
        }
    }

    public static string Right(this string me, int length)
    {
        length = Math.Max(length, 0);

        if (me.Length > length)
        {
            return me.Substring(me.Length - length, length);
        }
        else
        {
            return me;
        }
    }

    #endregion

    /// <summary>
    /// Replaces multiple concurrent spaces in string with single spaces
    /// </summary>        
    /// <returns></returns>
    public static string RemoveExcessSpacing(this string me)
    {
        while (me.CountOccurences("  ") > 0) me = me.Replace("  ", " ");

        return me.RemoveExcessSpacing(false);
    }

    public static T ToEnum<T>(this string value, T defaultValue) where T : struct
    {
        if (string.IsNullOrEmpty(value))
        {
            return defaultValue;
        }

        T result;

        return Enum.TryParse<T>(value, true, out result) ? result : defaultValue;
    }

    /// <summary>
    /// Replaces multiple concurrent spaces in string with single spaces
    /// </summary>
    /// <param name="trim">Perform trim operation on resulting string</param>
    /// <returns></returns>
    public static string RemoveExcessSpacing(this string me, bool trim)
    {
        while (me.CountOccurences("  ") > 0) me = me.Replace("  ", " ");

        if (trim) me.Trim();

        return me;
    }

    public static string F(this string s, params object[] args)
    {
        return string.Format(s, args);
    }

    public static bool HasValue(this string me)
    {
        return !(string.IsNullOrWhiteSpace(me));
    }

    public static int ParseToInt(this string me)
    {
        return int.Parse(me);
    }

    public static bool IsNumeric(this string me)
    {
        double retNum;

        bool isNum = Double.TryParse(me, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);

        return isNum;
    }

    public static string IfNullOrWhitespace(this string me, string replacement)
    {
        if (string.IsNullOrWhiteSpace(me)) return replacement;

        return me;
    }

    public static bool IsNullOrWhitespace(this string me)
    {
        return string.IsNullOrWhiteSpace(me);
    }

    public static bool IsNotNullOrWhitespace(this string me)
    {
        return !string.IsNullOrWhiteSpace(me);
    }

    public static byte[] ConvertFileToByteArray(this string me)
    {
        if (!File.Exists(me)) throw new FileNotFoundException("Cannot convert file to byte array, as file '{0}' is not available".F(me), me);

        using (Stream input = File.OpenRead(me))
        {
            byte[] buffer = new byte[16 * 1024];

            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }

    public static string UpperFirstCharacter(this string me, bool forceFollowingCharactersLower = false)
    {
        if (me.IsNullOrWhitespace()) return String.Empty;

        if (me.Length == 1) return me.ToUpperInvariant();

        string trail = me.Right(me.Length - 1);

        if (forceFollowingCharactersLower) trail = trail.ToLowerInvariant();

        return $"{me[0].ToString().ToUpper()}{trail}";
    }

    public static string RemoveWhiteSpace(this string me)
    {
        return String.Join("", me.Where(c => !char.IsWhiteSpace(c)));
    }
}

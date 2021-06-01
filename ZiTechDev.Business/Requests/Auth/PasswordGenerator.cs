using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ZiTechDev.Business.Requests.Auth
{
    public class PasswordGenerator
    {
        public int MinimumLengthPassword { get; private set; }
        public int MaximumLengthPassword { get; private set; }
        public int MinimumLowerCaseChars { get; private set; }
        public int MinimumUpperCaseChars { get; private set; }
        public int MinimumNumericChars { get; private set; }
        public int MinimumSpecialChars { get; private set; }

        public string AllLowerCaseChars { get; private set; }
        public string AllUpperCaseChars { get; private set; }
        public string AllNumericChars { get; private set; }
        public string AllSpecialChars { get; private set; }

        private readonly string _allAvailableChars;
        private readonly int _minimumNumberOfChars;
        private readonly RandomSecureVersion _randomSecure;

        public PasswordGenerator()
        {
            MinimumLengthPassword = 8;
            MaximumLengthPassword = 20;
            MinimumLowerCaseChars = 1;
            MinimumUpperCaseChars = 1;
            MinimumNumericChars = 1;
            MinimumSpecialChars = 1;
            AllLowerCaseChars = GetCharRange('a', 'z', exclusiveChars: "ilo");
            AllUpperCaseChars = GetCharRange('A', 'Z', exclusiveChars: "IO");
            AllNumericChars = GetCharRange('0', '9');
            AllSpecialChars = "!@#%*()$?+-=";

            _minimumNumberOfChars = MinimumLowerCaseChars
                + MinimumUpperCaseChars
                + MinimumNumericChars
                + MinimumSpecialChars;

            _allAvailableChars =
                OnlyIfOneCharIsRequired(MinimumLowerCaseChars, AllLowerCaseChars) +
                OnlyIfOneCharIsRequired(MinimumUpperCaseChars, AllUpperCaseChars) +
                OnlyIfOneCharIsRequired(MinimumNumericChars, AllNumericChars) +
                OnlyIfOneCharIsRequired(MinimumSpecialChars, AllSpecialChars);
            _randomSecure = new RandomSecureVersion();
        }

        private string OnlyIfOneCharIsRequired(int minimum, string allChars)
        {
            return minimum > 0 || _minimumNumberOfChars == 0 ? allChars : string.Empty;
        }

        private static string GetCharRange(char minimum, char maximum, string exclusiveChars = "")
        {
            var result = string.Empty;
            for (char value = minimum; value <= maximum; value++)
            {
                result += value;
            }
            if (!string.IsNullOrEmpty(exclusiveChars))
            {
                var inclusiveChars = result.Except(exclusiveChars).ToArray();
                result = new string(inclusiveChars);
            }
            return result;
        }

        public string Generate()
        {
            var lengthOfPassword = _randomSecure.Next(MinimumLengthPassword, MaximumLengthPassword);
            var minimumChars = GetRandomString(AllLowerCaseChars, MinimumLowerCaseChars) +
                            GetRandomString(AllUpperCaseChars, MinimumUpperCaseChars) +
                            GetRandomString(AllNumericChars, MinimumNumericChars) +
                            GetRandomString(AllSpecialChars, MinimumSpecialChars);
            var rest = GetRandomString(_allAvailableChars, lengthOfPassword - minimumChars.Length);
            var unshuffeledResult = minimumChars + rest;
            var result = unshuffeledResult.ShuffleTextSecure();
            return result;
        }

        private string GetRandomString(string possibleChars, int lenght)
        {
            var result = string.Empty;
            for (var position = 0; position < lenght; position++)
            {
                var index = _randomSecure.Next(possibleChars.Length);
                result += possibleChars[index];
            }
            return result;
        }
    }

    internal static class Extensions
    {
        private static readonly Lazy<RandomSecureVersion> RandomSecure =
            new Lazy<RandomSecureVersion>(() => new RandomSecureVersion());
        public static IEnumerable<T> ShuffleSecure<T>(this IEnumerable<T> source)
        {
            var sourceArray = source.ToArray();
            for (int counter = 0; counter < sourceArray.Length; counter++)
            {
                int randomIndex = RandomSecure.Value.Next(counter, sourceArray.Length);
                yield return sourceArray[randomIndex];

                sourceArray[randomIndex] = sourceArray[counter];
            }
        }

        public static string ShuffleTextSecure(this string source)
        {
            var shuffeldChars = source.ShuffleSecure().ToArray();
            return new string(shuffeldChars);
        }
    }

    internal class RandomSecureVersion
    {
        //Never ever ever never use Random() in the generation of anything that requires true security/randomness
        //and high entropy or I will hunt you down with a pitchfork!! Only RNGCryptoServiceProvider() is safe.
        private readonly RNGCryptoServiceProvider _rngProvider = new RNGCryptoServiceProvider();

        public int Next()
        {
            var randomBuffer = new byte[4];
            _rngProvider.GetBytes(randomBuffer);
            var result = BitConverter.ToInt32(randomBuffer, 0);
            return result;
        }

        public int Next(int maximumValue)
        {
            // Do not use Next() % maximumValue because the distribution is not OK
            return Next(0, maximumValue);
        }

        public int Next(int minimumValue, int maximumValue)
        {
            var seed = Next();

            //  Generate uniformly distributed random integers within a given range.
            return new Random(seed).Next(minimumValue, maximumValue);
        }
    }
}
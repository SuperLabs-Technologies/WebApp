using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApp
{
    internal class JSONUtilities
    {
        public static string Deserialize(string input)
        {
            input = Regex.Unescape(input);
            input = input.Remove(0, 1);
            input = input.Remove(input.Length - 1, 1);

            return input;
        }
    }
}

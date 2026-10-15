using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Slimey_Arcades
{
    public static class KeyManager
    {
        public static Dictionary<Keys, string> LowerOEMKeys = new Dictionary<Keys, string>()
        {
            {Keys.OemTilde, "`" },
            {Keys.OemMinus, "-"},
            {Keys.OemPlus, "="},
            {Keys.OemOpenBrackets, "[" },
            {Keys.OemCloseBrackets, "]" },
            {Keys.OemSemicolon, ";"},
            {Keys.OemQuotes, "\'"},
            {Keys.OemPipe, "\\" },
            {Keys.OemComma, ","},
            {Keys.OemPeriod, "."},
            {Keys.OemQuestion, "/" },
            {Keys.Divide, "/"},
            {Keys.Multiply, "*"},
            {Keys.Subtract, "-" },
            {Keys.Add, "+" },
            {Keys.Decimal, "." },
        };

        public static Dictionary<Keys, string> UpperOEMKeys = new Dictionary<Keys, string>()
        {
            {Keys.OemTilde, "~" },
            {Keys.OemMinus, "_"},
            {Keys.OemPlus, "+"},
            {Keys.OemOpenBrackets, "{" },
            {Keys.OemCloseBrackets, "}" },
            {Keys.OemSemicolon, ":"},
            {Keys.OemQuotes, "\""},
            {Keys.OemPipe, "|" },
            {Keys.OemComma, "<"},
            {Keys.OemPeriod, ">"},
            {Keys.OemQuestion, "?" },
            {Keys.D1, "!" },
            {Keys.D2, "@" },
            {Keys.D3, "#" },
            {Keys.D4, "$" },
            {Keys.D5, "%" },
            {Keys.D6, "^" },
            {Keys.D7, "&" },
            {Keys.D8, "*" },
            {Keys.D9, "(" },
            {Keys.D0, ")" },
        };

        public static Dictionary<Keys, string> NumPadKeys = new Dictionary<Keys, string>() 
        {
            {Keys.NumPad0, "0" },
            {Keys.NumPad1, "1" },
            {Keys.NumPad2, "2" },
            {Keys.NumPad3, "3" },
            {Keys.NumPad4, "4" },
            {Keys.NumPad5, "5" },
            {Keys.NumPad6, "6" },
            {Keys.NumPad7, "7" },
            {Keys.NumPad8, "8" },
            {Keys.NumPad9, "9" },
            {Keys.Divide, "/"},
            {Keys.Multiply, "*"},
            {Keys.Subtract, "-" },
            {Keys.Add, "+" },
            {Keys.Decimal, "." },
        };

        public static Dictionary<Keys, string> NumKeys = new Dictionary<Keys, string>()
        {
            {Keys.D1, "1" },
            {Keys.D2, "2" },
            {Keys.D3, "3" },
            {Keys.D4, "4" },
            {Keys.D5, "5" },
            {Keys.D6, "6" },
            {Keys.D7, "7" },
            {Keys.D8, "8" },
            {Keys.D9, "9" },
            {Keys.D0, "0" },
            {Keys.NumPad0, "0" },
            {Keys.NumPad1, "1" },
            {Keys.NumPad2, "2" },
            {Keys.NumPad3, "3" },
            {Keys.NumPad4, "4" },
            {Keys.NumPad5, "5" },
            {Keys.NumPad6, "6" },
            {Keys.NumPad7, "7" },
            {Keys.NumPad8, "8" },
            {Keys.NumPad9, "9" },
        };

        public static Dictionary<Keys, string> ArrowKeys = new Dictionary<Keys, string>()
        {
            {Keys.Left, "Left" },
            {Keys.Right, "Right" },
            {Keys.Up, "Up" },
            {Keys.Down, "Down" },
            {Keys.NumPad4, "Left" },
            {Keys.NumPad6, "Right" },
            {Keys.NumPad8, "Up" },
            {Keys.NumPad2, "Down" },
            {Keys.W, "Up" },
            {Keys.A, "Left" },
            {Keys.S, "Down" },
            {Keys.D, "Right" },
        };

        public static string GetKeyName(Keys Key, bool Shift = false)
        {
            string KeyName = Key.ToString();
            if (Shift)
            {
                if (KeyName.Length == 1) return KeyName.ToUpper();
                if (UpperOEMKeys.ContainsKey(Key)) return UpperOEMKeys[Key];
                if (NumPadKeys.ContainsKey(Key)) return NumPadKeys[Key];
            }
            else
            {
                if (KeyName.Length == 1) return KeyName.ToLower();
                if (LowerOEMKeys.ContainsKey(Key)) return LowerOEMKeys[Key];
                if (NumKeys.ContainsKey(Key)) return NumKeys[Key];
            }
            return "";
        }
    }
}

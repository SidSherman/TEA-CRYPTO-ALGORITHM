using System;

namespace TeaCrypto
{
  
    public class Util
    {
        
        public Util()
        {
        }

    
        public static uint ConvertStringToUInt(string Input)
        {
            uint output;
            output = ((uint)Input[0]);
            output += ((uint)Input[1] << 8);
            output += ((uint)Input[2] << 16);
            output += ((uint)Input[3] << 24);
            return output;
        }

        public static string ConvertUIntToString(uint Input)
        {
            System.Text.StringBuilder output = new System.Text.StringBuilder();
            output.Append((char)((Input & 0xFF)));
            output.Append((char)((Input >> 8) & 0xFF));
            output.Append((char)((Input >> 16) & 0xFF));
            output.Append((char)((Input >> 24) & 0xFF));
            return output.ToString();
        }
    }
}

using System;


namespace TeaCrypto
{
   
    public class Tea
    {
        
        public Tea()
        {
        }
        // Шифрование 
        public string EncryptString(string Data, string Key)
        {
            /*
             if (Data.Length == 0)
                 throw new ArgumentException("Не введен ключ.");
             */
            // Приведим ключ  нужному формату
            uint[] formattedKey = FormatKey(Key);

            // добавляем нуль, если длина текста не кратна 2
            if (Data.Length % 2 != 0) Data += '\0'; 		
            byte[] dataBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(Data);


            //разделдяем текст на блоки и шифруем 
            string cipher = string.Empty;
            uint[] tempData = new uint[2];
            for (int i = 0; i < dataBytes.Length; i += 2)
            {
                tempData[0] = dataBytes[i];
                tempData[1] = dataBytes[i + 1];
                code(tempData, formattedKey);
                cipher += Util.ConvertUIntToString(tempData[0]) + Util.ConvertUIntToString(tempData[1]);
            }

            return cipher;
        }
        // расшифрование
        public string Decrypt(string Data, string Key)
        {
            uint[] formattedKey = FormatKey(Key);

            int x = 0;
            uint[] tempData = new uint[2];
            byte[] dataBytes = new byte[Data.Length / 8 * 2];
            for (int i = 0; i < Data.Length; i += 8)
            {
                tempData[0] = Util.ConvertStringToUInt(Data.Substring(i, 4));
                tempData[1] = Util.ConvertStringToUInt(Data.Substring(i + 4, 4));
                decode(tempData, formattedKey);
                dataBytes[x++] = (byte)tempData[0];
                dataBytes[x++] = (byte)tempData[1];
            }

            string decipheredString = System.Text.ASCIIEncoding.ASCII.GetString(dataBytes, 0, dataBytes.Length);
            if (decipheredString[decipheredString.Length - 1] == '\0')
                decipheredString = decipheredString.Substring(0, decipheredString.Length - 1);
            return decipheredString;
        }
        // Если ключ меньше 128 битов то заполняем его пробелами и переводим в uint
        public uint[] FormatKey(string Key)
        {
            /*
           if (Key.Length == 0)
               throw new ArgumentException("Ключ должен быть размером длинной от 1 до 16 символов");
           */
            Key = Key.PadRight(16, ' ').Substring(0, 16); 
            uint[] formattedKey = new uint[4];

            
            int j = 0;
            for (int i = 0; i < Key.Length; i += 4)
                formattedKey[j++] = Util.ConvertStringToUInt(Key.Substring(i, 4));

            return formattedKey;
        }

        // основные пребразования
     /*   #region Tea Algorithm

        public void code(uint[] v, uint[] k)
        {
            uint y = v[0];
            uint z = v[1];
            uint sum = 0;
            uint delta = 0x9e3779b9;
            uint n = 32;

            while (n-- > 0)
            {
                sum += delta;
                y += (z << 4) + k[0] ^ z + sum ^ (z >> 5) + k[1];
                z += (y << 4) + k[2] ^ y + sum ^ (y >> 5) + k[3];
            }

            v[0] = y;
            v[1] = z;
        }

        public void decode(uint[] v, uint[] k)
        {
            uint n = 32;
            uint sum;
            uint y = v[0];
            uint z = v[1];
            uint delta = 0x9e3779b9;

            sum = delta << 5;

            while (n-- > 0)
            {
                z -= (y << 4) + k[2] ^ y + sum ^ (y >> 5) + k[3];
                y -= (z << 4) + k[0] ^ z + sum ^ (z >> 5) + k[1];
                sum -= delta;
            }

            v[0] = y;
            v[1] = z;
        }
        #endregion
     */
        
       #region Tea Algorithm

       public void code(uint[] v, uint[] k)
       {
           uint y = v[0];
           uint z = v[1];
           uint sum = 0;
           uint delta = 0x9e3779b9;
           uint n = 32;

           while (n-- > 0)
           {
               sum += delta;
               y += ((z << 4) + k[0]) ^ (z + sum) ^ ((z >> 5) + k[1]);
               z += ((y << 4) + k[2]) ^ (y + sum) ^ ((y >> 5) + k[3]);
           }

           v[0] = y;
           v[1] = z;
       }

       public void decode(uint[] v, uint[] k)
       {
           uint n = 32;
           uint sum;
           uint y = v[0];
           uint z = v[1];
           uint delta = 0x9e3779b9;

           sum = delta << 5;

           while (n-- > 0)
           {
               z -= ((y << 4) + k[2]) ^ (y + sum) ^ ((y >> 5) + k[3]);
               y -= ((z << 4) + k[0]) ^ (z + sum) ^ ((z >> 5) + k[1]);
               sum -= delta;
           }

           v[0] = y;
           v[1] = z;
       }
       #endregion




    }




}


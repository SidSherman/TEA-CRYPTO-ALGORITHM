using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using TeaCrypto;
namespace TEA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string cipher;
       // Thread t_key_length;
        Thread t_ascii_hex;
        //CSerpent serpent;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //потоки для обновления размера ключа и перевода текствого значения в шестнадцатиричное
            /*t_key_length = new Thread(KeyLength);
            t_key_length.IsBackground = true;
            t_key_length.Start();*/
            t_ascii_hex = new Thread(UpdateVal);
            t_ascii_hex.IsBackground = true;
            t_ascii_hex.Start();
           // tbx_plain_ascii.Multiline = true;
           // tbx_plain_ascii.ScrollBars = ScrollBars.Both;
        }

        
         /*  private void cipher_hex_KeyUp(object sender, KeyEventArgs e)
        {
            tbx_cipher.Text = HexToAnsiiString(tbx_cipher_hex.Text);
        }*/

        // Действие при вводе в поле открытого текста
        private void plaintext_KeyUp(object sender, KeyEventArgs e)
        {
            tbx_plain_bin.Text = ASCIIToBinString(tbx_plain_ascii.Text);
            tbx_plain_hex.Text = ASCIIToHexString(tbx_plain_ascii.Text);
        }
        // Действие при вводе в поле открытого текстав 2-ой системе
        private void plaintext_bin_KeyUp(object sender, KeyEventArgs e)
        {
            tbx_plain_ascii.Text = BinToASCIIString(tbx_plain_bin.Text);
        }
        // Действие при вводе в поле шифртекста текста
      /*  private void chipher_KeyUp(object sender, KeyEventArgs e)
        {
            tbx_cipher_hex.Text = ASCIIToHexString(tbx_cipher.Text);
        }*/

        // Действие при вводе в поле ключа
        private void key_KeyUp(object sender, KeyEventArgs e)
        {
            int klength;
                    
            klength = tbx_key.Text.Length * 8;
            if (klength == 128)
                lbl_key_length.Foreground = Brushes.Green;
            else if (klength < 128 && klength > 0)
                lbl_key_length.Foreground = Brushes.Orange;
            else
                lbl_key_length.Foreground = Brushes.Red;
            lbl_key_length.Content = klength + " Битов";
            tbx_key_bin.Text = ASCIIToBinString(tbx_key.Text);

        }
        // Действие при вводе в поле ключа 2-ой системе
        private void key_bin_KeyUp(object sender, KeyEventArgs e)
        {
            int klength;
            tbx_key.Text = BinToASCIIString(tbx_key_bin.Text);
            klength = tbx_key.Text.Length * 8;
            if (klength == 128)
                lbl_key_length.Foreground = Brushes.Green;
            else if (klength < 128 && klength > 0)
                lbl_key_length.Foreground = Brushes.Orange;
            else
                lbl_key_length.Foreground = Brushes.Red;
            lbl_key_length.Content = klength + " Битов";
            

        }

        // действие при нажатии на кнопку Зашифровать

        private void btn_encrypt_Click(object sender, RoutedEventArgs e)
        {
          //  ASCIIToHexString(tbx_plain_ascii.Text);
            if ((tbx_key.Text.Length * 8) > 128 || tbx_key.Text.Length == 0)
            {
                MessageBox.Show("Неверный размер ключа!");
                return;
            }
            Tea t = new Tea();
           
            cipher = t.EncryptString(tbx_plain_ascii.Text, tbx_key.Text);
            tbx_cipher.Text = cipher;




        }

        
     /*        private void btn_hex_from_Click(object sender, RoutedEventArgs e)
        {


            tbx_cipher_hex.Text = HexToAnsiiString(tbx_cipher.Text);

        }
        private void btn_hex_Click(object sender, RoutedEventArgs e)
        {


            tbx_cipher_hex.Text = ASCIIToHexString(tbx_cipher.Text);

        }*/


        // действие при нажатии на кнопку Расшифроваь

        private void btn_decrypt_Click(object sender, RoutedEventArgs e)
        {

            if ((tbx_key.Text.Length > 128 || tbx_key.Text.Length == 0))
            {
                MessageBox.Show("Неверный размер ключа!");
                return;
            }
            Tea t = new Tea();


            tbx_plain_ascii.Text = t.Decrypt(tbx_cipher.Text, tbx_key.Text);


        }

        // перевод из 16-тиричной в текст ascii
        private string ASCIIToBinString(string asciitext)
        {
            byte[] text = Encoding.Default.GetBytes(asciitext);
            string str = "";
            string formatted;
            for (int i = 0; i < text.Length; i++)
            {
                 formatted = Convert.ToString(text[i], 2);
                  str = str + formatted.PadLeft(8, '0');
                //str = str + (Convert.ToString(text[i], 2));
            }

            return str;
        }
        // перевод из текста ascii в 16-ую
        private string ASCIIToHexString(string asciitext)
        {
            byte[] text = Encoding.Default.GetBytes(asciitext);
            string str = "";
            string formatted;
            for (int i = 0; i < text.Length; i++)
            {
                str = str + text[i].ToString("X2");
                //str = str + (Convert.ToString(text[i], 2));
            }

            return str;
        }


        // перевод из текста ascii в 16-ую
        private string HexToAnsiiString(string asciitext)
        {
            byte[] text = Encoding.Default.GetBytes(asciitext);
            string str = "";
            uint formatted;
            
            for (int i = 0; i < text.Length; i += 2)
            {
                str = str + Convert.ToChar(Convert.ToUInt32(asciitext.Substring(i, 2), 16));
                //str = str + (Convert.ToString(text[i], 2));
            }

            return str;
        }



        // Перевод из двоичной системы в строку
        private string BinToASCIIString(string asciitext)
        {
            string text = asciitext;
            string str = "";
            uint formatted;
            if (text.Length %8 != 0)
                return "";
            for (int i = 0; i < text.Length; i += 8)
            {
                formatted = Convert.ToUInt32(text.Substring(i,  8), 2); ;
                str = str + Convert.ToChar(formatted);
                //str = str + (Convert.ToString(text[i], 2));
            }

            return str;
        }



        // функция каждые 250 ms перводит текст из поля с текстом ascii в поле с значениями в 16-тиричной системе
          private void UpdateVal()
          {
              int klength;
              while (true)
              {

                  Dispatcher.Invoke((Action)delegate
                  {
                     // tbx_cipher_hex.Text = ASCIIToHexString(tbx_cipher.Text);
                      tbx_cipher_bin.Text = ASCIIToBinString(tbx_cipher.Text);
                      
                  });

                  Thread.Sleep(250);
              }
          }

        // функция возвращает длинну ключа каждые 250ms 
        /*
         private void KeyLength()
         {
             int klength;
             while (true)
             {

                 Dispatcher.Invoke((Action)delegate
                 {
                     klength = tbx_key.Text.Length * 8;
                     if (klength == 128)
                         lbl_key_length.Foreground = Brushes.Green;
                     else if (klength < 128 && klength > 0)
                         lbl_key_length.Foreground = Brushes.Orange;
                     else
                         lbl_key_length.Foreground = Brushes.Red;
                     lbl_key_length.Content = klength + " Битов";
                 });
                 Thread.Sleep(250);
             }
         }
         */


    }
}



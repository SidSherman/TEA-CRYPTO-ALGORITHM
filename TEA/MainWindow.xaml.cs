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
        Thread t_key_length;
        Thread t_ascii_hex;
        //CSerpent serpent;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //потоки для обновления размера ключа и перевода текствого значения в шестнадцатиричное
            t_key_length = new Thread(KeyLength);
            t_key_length.IsBackground = true;
            t_key_length.Start();
            t_ascii_hex = new Thread(UpdateHexval);
            t_ascii_hex.IsBackground = true;
            t_ascii_hex.Start();
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
        private string ASCIIToHexString(string asciitext)
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


        // функция каждые 250 ms перводит текст из поля с текстом ascii в поле с значениями в 16-тиричной системе
        private void UpdateHexval()
        {

            while (true)
            {

                Dispatcher.Invoke((Action)delegate
                {
                    tbx_plain_bin.Text = ASCIIToHexString(tbx_plain_ascii.Text);
                    tbx_cipher_bin.Text = ASCIIToHexString(tbx_cipher.Text);

                });

                Thread.Sleep(250);
            }
        }

        // функция возвращает длинну ключа каждые 250ms 
        
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

         

    }
}



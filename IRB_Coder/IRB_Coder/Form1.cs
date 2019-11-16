using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace IRB_Coder
{
    public partial class Form1 : Form
    {
        static string path = @"D:\polytehnica\magDiploma\IRB_Coder\Output.txt";
        static string Text_to_code = "";
        static int[] Selected = new int[] { };
        static List<int[]> IRB = new List<int[]>();
        public Form1()
        {
            InitializeComponent();
            
            IRB.Add(new int[] { 1, 2, 4, 8, 16, 32, 27, 26, 11, 9, 45, 13, 10, 29, 5, 17, 18 });
            IRB.Add(new int[] { 1, 3, 12, 10, 31, 7, 27, 2, 6, 5, 19, 20, 62, 14, 9, 28, 17 });
            IRB.Add(new int[] { 1, 7, 3, 15, 33, 5, 24, 68, 2, 14, 6, 17, 4, 9, 19, 12, 34 });
            IRB.Add(new int[] { 1, 7, 31, 2, 11, 3, 9, 36, 17, 4, 22, 6, 18, 72, 5, 10, 19 });
            IRB.Add(new int[] { 1, 7, 12, 44, 25, 41, 9, 17, 4, 6, 22, 33, 13, 2, 3, 11, 23 });
            IRB.Add(new int[] { 1, 21, 11, 50, 39, 13, 6, 4, 14, 16, 25, 26, 3, 2, 7, 8, 27 });

            foreach (int[] item in IRB)
            {
                string s = "";
                foreach (int i in item)
                {
                    s = s + i + " ";
                }
                comboBox1.Items.Add(s);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Selected = IRB[comboBox1.SelectedIndex];
            richTextBox1.Clear();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Text_to_code = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.Text = Coding(textBox1.Text);
            for (int i = 0; i < richTextBox1.TextLength; i = i + Selected.Length)
            {
                richTextBox1.SelectionStart = i;
                richTextBox1.SelectionLength = 1;
                richTextBox1.SelectionColor = Color.Blue;
                File.WriteAllText(path, richTextBox1.Text); 
             }      
        }

        public string Coding(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            int[] result = Array.ConvertAll(bytes, Convert.ToInt32);
            string codes = "";
            foreach (int Number in result)
            {
                codes = codes + Monolite_code(Number, Selected);
            }
            return codes;
        }

        public string Monolite_code(int Number, int[] IRB)
        {
            char[] chs = new char[IRB.Length];
            for (int i = 0; i < IRB.Length; i++)
            {
                chs[i] = '0';
            }
   
            int j = 0;
            int temp = 0;
            int count = 0;

            while (temp != Number)
            {
                if (j == IRB.Length)
                {
                    j = 0;
                }
                temp += IRB[j];
                chs[j] = '1';
                j++;
                count++;
                if (temp > Number)
                {
                    for (int i = 0; i < IRB.Length; i++)
                    {
                        chs[i] = '0';
                    }
                    j = j - count +1;
                    if (j < 0)
                        j += 17;
                    count = 0;
                    temp = 0;
                }
            }

            string code = new string(chs);
            return code;
 
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        public string Decoding(string s)
        {
            List<int> decodeNumbers = new List<int>();
       
            for (int i = 0; i < s.Length; i += Selected.Length)
            {
                string word = s.Substring(i, Selected.Length);
                decodeNumbers.Add(Monolite_decode(word, Selected));
            }
            int[] biteArray = decodeNumbers.ToArray();
            byte[] bytes = biteArray.Select(i => (byte)i).ToArray();

            string cppString = Encoding.GetEncoding("iso-8859-1").GetString(bytes);
            byte[] decoded = Encoding.GetEncoding("iso-8859-1").GetBytes(cppString);
            string decodes = Encoding.UTF8.GetString(decoded);
            return decodes;
        }

        public int Monolite_decode(string s, int[] IRB)
        {
            int Number = 0;
            int i = 0;
            foreach(char ch in s) 
            {
                if (ch == '1')
                {
                    Number += IRB[i];
                }
                i++;
            }
            return Number;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox1.Text = Decoding(richTextBox1.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

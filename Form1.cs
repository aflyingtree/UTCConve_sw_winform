using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace UTC时间戳互转
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DateTime utcBegin = new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc);//utc转北京时间,时区小时用8

        /// <summary>
        /// 格式化输入十六进制数据
        /// </summary>
        /// <param name="input"></param>
        /// <param name="checkBuffer"></param>
        /// <returns></returns>
        private int GetInputData(string input, out byte[] checkBuffer)
        {
            StringBuilder inputData = new StringBuilder(input);//将要转化的数据复制为StringBuilder格式便于处理

            try
            {
                //初步进行格式化处理
                inputData.Replace("0x", "");
                inputData.Replace(",", "");
                inputData.Replace(" ", "");
                inputData.Replace("\r", "");
                inputData.Replace("\n", "");
                inputData.Replace("\t", "");

                if (inputData.Length == 1)//数据长度为1的时候，在数据前补0
                {
                    inputData.Insert(0, "0");
                }
                else if (inputData.Length % 2 != 0)//数据长度为奇数位时，去除最后一位数据
                {
                    inputData.Remove(inputData.Length - 1, 1);
                }
            }
            catch //无法转为16进制时，出现异常
            {
                MessageBox.Show("请输入正确的16进制数据");
                checkBuffer = null;
                return -1;//输入的16进制数据错误，无法发送，提示后返回
            }

            string checkData = inputData.ToString();//因为下面要使用Substring方法,所以需要转换为String类型
            checkBuffer = new byte[checkData.Length / 2];//使用了out参数则必须在被调用函数中完成数组的初始化.

            try
            {
                int i, j = 0;
                for (i = 0; i < checkData.Length; i += 2)
                {
                    checkBuffer[j] = Convert.ToByte(checkData.Substring(i, 2), 16);//如果不指定基数,会将12认为是默认十进制
                    j++;
                }
                return 0;
            }
            catch //无法转为16进制时，出现异常
            {
                MessageBox.Show("请输入正确的16进制数据");
                checkBuffer = null;
                return -1;//输入的16进制数据错误，无法发送，提示后返回
            }
        }

        /// <summary>
        /// 16进制的UTC时间戳转为北京时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (richTextBox6.Text.Length < 2)
            {
                richTextBox5.Text = "输入的数据异常";
                return;
            }
            byte[] checkBuffer;
            if (0 != GetInputData(richTextBox6.Text, out checkBuffer))//获取输入的数据
            {
                goto EndThisFun;
            }
            UInt32 utcData = BitConverter.ToUInt32(checkBuffer, 0);
            richTextBox5.Text = utcBegin.AddSeconds(utcData).ToString("yyyy-MM-dd HH:mm:ss");

            EndThisFun:
            return;
        }

        /// <summary>
        /// 双击复制数据到粘贴板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox5_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string willCpy = richTextBox5.Text;
            Clipboard.SetText(willCpy);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime inputTime;
            bool ret;

            if (richTextBox4.Text.Length < 2)
            {
                richTextBox3.Text = "输入的数据异常";
                return;
            }

            ret = DateTime.TryParseExact(richTextBox4.Text,
               "yyyy/MM/dd HH:mm:ss", null,
               System.Globalization.DateTimeStyles.None, out inputTime);
            if (ret == true)
            {
                goto EndThisFun;
            }

            ret = DateTime.TryParseExact(richTextBox4.Text,
                "yyyy-MM-dd HH:mm:ss", null,
                System.Globalization.DateTimeStyles.None, out inputTime);
            if (ret == true)
            {
                goto EndThisFun;
            }

            richTextBox3.Text = "无法解析输入的时间字符串";
            return;

            EndThisFun:
            // 定义日期时间格式  
            double secondsDifference = (inputTime - utcBegin).TotalSeconds;
            UInt32 dataout = (UInt32)secondsDifference;
            richTextBox3.Text = dataout.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DateTime inputTime;
            bool ret;

            if (richTextBox7.Text.Length < 2)
            {
                richTextBox8.Text = "输入的数据异常";
                return;
            }
            ret = DateTime.TryParseExact(richTextBox7.Text,
               "yyyy/MM/dd HH:mm:ss", null,
               System.Globalization.DateTimeStyles.None, out inputTime);
            if (ret == true)
            {
                goto EndThisFun;
            }

            ret = DateTime.TryParseExact(richTextBox7.Text,
                "yyyy-MM-dd HH:mm:ss", null,
                System.Globalization.DateTimeStyles.None, out inputTime);
            if (ret == true)
            {
                goto EndThisFun;
            }

            richTextBox8.Text = "无法解析输入的时间字符串";
            return;

            EndThisFun:
            double secondsDifference = (inputTime - utcBegin).TotalSeconds;
            UInt32 dataout = (UInt32)secondsDifference;

            // 将整数转换为字节数组（小端序）
            byte[] byteArray = BitConverter.GetBytes(dataout);

            // 如果运行环境是大端序，反转字节数组
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteArray);
            }

            // 将字节数组转换为 16 进制字符串
            string hexString = BitConverter.ToString(byteArray).Replace("-", " ");

            richTextBox8.Text = hexString;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double tmpData = Convert.ToDouble(richTextBox1.Text);

            UInt32 utcData = (UInt32)tmpData;

            richTextBox2.Text = utcBegin.AddSeconds(utcData).ToString("yyyy-MM-dd HH:mm:ss");

        }


        private void label10_Click(object sender, EventArgs e)
        {
            richTextBox4.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void label11_Click(object sender, EventArgs e)
        {
            richTextBox7.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 版本控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label8_Click(object sender, EventArgs e)
        {
            string version = "" +
                $"v0.0.1 软件第一版,可以正常进行时间格式的转换.\r\n" +
                $"v0.0.2 添加部分异常处理,公开.";
            MessageBox.Show(version);
        }
    }
}

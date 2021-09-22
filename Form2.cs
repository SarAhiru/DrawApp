using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

using System.Windows.Forms.DataVisualization.Charting;


namespace test3
{

    //ログ取得のため　のform
    //日時と何の動作をしたのか（ボタンを押した、線を描いた）

    /*
    form1から情報を送る
    form1で情報が送られ次第、form2にて表示
     */

    public partial class Form2 : Form
    {
        
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label1.Parent = panel1;

            logLabel.Text = "date,time,log";

            //label2.Text = "色変更0\n消しゴム選択0\n線の太さ変更0\n";
            //label3.Text = "画像出力0\n画像拡大0\n画像縮小0\n";
            //label4.Text = "保存0\nクリア0\n描画0\n";


            Series data = new Series();
            data.Points.AddXY("色変更", 10);
            data.Points.AddXY("消しゴム選択", 20);
            data.Points.AddXY("線の太さ変更", 30);
            data.Points.AddXY("画像出力", 40);
            data.Points.AddXY("画像拡大", 50);
            data.Points.AddXY("画像縮小", 50);
            data.Points.AddXY("保存", 50);
            data.Points.AddXY("クリア", 50);
            data.Points.AddXY("描画", 50);
            data.Name = "回数";

            data.IsValueShownAsLabel = true;

        }

        private void btnText_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string file = saveFileDialog1.FileName;
                StreamWriter sw = new StreamWriter(file, false, Encoding.GetEncoding("SHIFT_JIS"));

                //string[] lines = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string[] lines = label1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < lines.Length; i++)
                {
                    sw.WriteLine(lines[i]);
                }

                sw.Close();
            }
        }
    }
}

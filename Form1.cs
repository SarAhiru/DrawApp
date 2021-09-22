//定義
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
using System.Globalization;

namespace test3
{
    //お絵かきソフトの作成するよ

    public partial class Form1 : Form
    {
        Form2 form2 = new Form2();

        //大枠
        public Form1()
        {
            InitializeComponent();
            //new クラス　->　インスタンス生成　//描画するためにbitmapを作成
            bitmap3 = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            bitmap1 = new Bitmap(pictureBox3.Width, pictureBox3.Height);

            //ボタンの色設定
            eraserbtn.BackColor = Color.White;
            btn1.BackColor = Color.Black;
            btn2.BackColor = Color.Blue;
            btn3.BackColor = Color.Red;
            btn4.BackColor = Color.Green;
            btn5.BackColor = Color.Yellow;
            btn6.BackColor = Color.Pink;

            //太さのコンボボックスのデフォルト値の設定
            cmbWidth.SelectedIndex = 0;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //デバッグの出力
            Console.WriteLine("Hello　Form1");

            //親子関係にする
            pictureBox2.Parent = pictureBox1;
            pictureBox3.Parent = pictureBox2;

            form2.StartPosition = FormStartPosition.Manual;
            form2.DesktopLocation = new Point(900, 100);
            form2.Show();
            log(0);//お絵かきソフト起動
        }

        Bitmap bitmap3 = null;
        Bitmap bitmap1 = null;

        //最初は黒を選択しておく
        Color selectedColor = Color.Black;

        //すべての色ボタンクリックイベント
        private void btn_Click(object sender, EventArgs e)
        {
            //選択した色に設定する
            selectedColor = ((Button)sender).BackColor;

            log(1);//色変更
        }

        //消しゴムのクリックイベント
        private void eraserbtn_Click(object sender, EventArgs e)
        {
            selectedColor = Color.White;
            //selectedColor = Color.Transparent;
            //色透明にしたら消しゴムだ！！って思ったけれど透明はもはや見えないか…

            log(2);//消しゴム選択
        }

        //colorDialogBoxを使って色を拡張してみた
        private void colorBtn_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                //選択した色に設定する
                selectedColor = colorDialog1.Color;

                log(1);//色変更
            }
        }



        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            //最初の場所を保存
            oldLocation = e.Location;

            //描画中にする
            drawFlg = true;
            //pictureBox3_MouseMoveに移動
            pictureBox3_MouseMove(sender, e);

            log(3);//描画開始
        }

        bool drawFlg = false; //true:描画中

        Point oldLocation = new Point();

        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            //描画中でない場合は処理を抜ける
            if (drawFlg == false) return;

            //usingライブラリ。　使い捨てみたいなことをするときはusing
            //使い終わったら捨てる  //便利。
            //pictureBox3_MouseMoveの際に毎回実行 ->終わったら消去
            //Graphicsというクラス(箱)を作成(g と命名)　新しいイメージを作る
            //bitmapの中にgという箱を作る
            //書くための準備
            using (Graphics g = Graphics.FromImage(bitmap3))
            {
                //描画するdraw関数に移動
                draw(g, oldLocation, e.Location);

            }
            pictureBox3.Image = bitmap3;//Imageは隠れたりすると再描画してくれる
                                        //全体を書き直しているイメージ

            //新しい位置を保存する
            oldLocation = e.Location;
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            log(4);//描画終了

            //描画中を解除する
            drawFlg = false;
        }


        //線を描く操作
        private void draw(Graphics g, Point xy1, Point xy2)
        {
            //マウスの位置を表示
            //Console.WriteLine(xy1);

            //太さの値　//数値変換
            int penWidth = Int32.Parse(cmbWidth.SelectedItem.ToString());
            if (penWidth <= 1)
            {
                //1pxの円は描画できないため、線で描く
                //太さ1の場合は単純に線をかく
                Pen pen = new Pen(selectedColor, penWidth);
                //線をかく
                g.DrawLine(pen, xy1, xy2);
                return;
            }


            Brush brush = new SolidBrush(selectedColor);

            bool drawX = true; //true:X軸基準で描画
                               //false:Y軸基準で描画

            if (Math.Abs(xy2.X - xy1.X) >
                Math.Abs(xy2.Y - xy1.Y))
            {
                //幅の方が大きい場合 -> X軸でループ
                //xy2.X が大きくなるように入れ替える(右→左へ行く場合に)
                if (xy1.X > xy2.X)
                {
                    Point p = xy1;
                    xy1 = xy2;
                    xy2 = p;
                }
                drawX = true;
            }
            else
            {
                //高さの方が大きい場合 -> Y軸でループ
                //xy2.Y が大きくなるように入れ替える
                if (xy1.Y > xy2.Y)
                {
                    Point p = xy1;
                    xy1 = xy2;
                    xy2 = p;
                }
                drawX = false;
            }

            if (drawX == true)
            {
                //X軸基準で描画
                float y = (float)xy1.Y;

                //傾きの計算　y=ax+b
                float a =
                    ((float)xy2.Y - xy1.Y) / ((float)xy2.X - xy1.X);


                for (int x = xy1.X; x <= xy2.X; x++)
                {
                    //どんな大きさの円を描くのかを定義
                    RectangleF rect = new RectangleF(
                        x - (penWidth / 2),
                        y - (penWidth / 2),
                        penWidth,
                        penWidth);
                    //円を描く
                    g.FillEllipse(brush, rect);

                    y = y + a;
                }
            }
            else
            {
                //Y軸基準で描画
                float x = (float)xy1.X;

                //傾きの計算　y=ax+b
                float a =
                    ((float)xy2.X - xy1.X) / ((float)xy2.Y - xy1.Y);


                for (int y = xy1.Y; y <= xy2.Y; y++)
                {
                    RectangleF rect = new RectangleF(
                        x - (penWidth / 2),
                        y - (penWidth / 2),
                        penWidth,
                        penWidth);
                    g.FillEllipse(brush, rect);

                    x = x + a;
                }
            }
        }

        //画像の保存をしておく
        Image imageSave;
        int imageX;
        int imageY;

        //画像の表示
        private void pictureBtn_Click(object sender, EventArgs e)
        {
            //描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);

            // 画像ファイルを開く
            Image bmp = ImageFileOpen();
            imageSave = bmp;
            pictureBox2.SizeMode = PictureBoxSizeMode.Normal;

            if(bmp != null)
            {
                //画像のサイズを縮小してcanvasに描画する
                if (pictureBox2.Height < bmp.Height)
                {
                    g.DrawImage(bmp, 0, 0, bmp.Width * pictureBox2.Height / bmp.Height, pictureBox2.Height);
                    imageX = bmp.Width * pictureBox2.Height / bmp.Height;
                    imageY = pictureBox2.Height;
                }
                else
                {
                    g.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);
                    imageX = bmp.Width;
                    imageY = bmp.Height;
                }
                log(5);//画像出力
            }
            
            
            //Imageオブジェクトのリソースを解放する
            //bmp.Dispose();
            //Graphicsオブジェクトのリソースを解放する
            //g.Dispose();

            pictureBox2.Image = canvas;
            bmp2 = canvas;

            //描画面を最前面に配置する
            pictureBox3.BringToFront();
        }

        /// <summary> 
        /// ファイルを開くダイアログボックスを表示して画像ファイルを開く 
        /// </summary> 
        /// <returns>生成したBitmapクラスオブジェクト</returns>
        private Bitmap ImageFileOpen()
        {
            //ファイルを開くダイアログボックスの作成  
            var ofd = new OpenFileDialog();
            //ファイルフィルタ  
            ofd.Filter = "Image File(*.bmp,*.jpg,*.png,*.tif)|*.bmp;*.jpg;*.png;*.tif|Bitmap(*.bmp)|*.bmp|Jpeg(*.jpg)|*.jpg|PNG(*.png)|*.png";
            //ダイアログの表示 （Cancelボタンがクリックされた場合は何もしない）
            if (ofd.ShowDialog() == DialogResult.Cancel) return null;

            return ImageFileOpen(ofd.FileName);
        }


        Bitmap bmp2;

        /// <summary>
        /// ファイルパスを指定して画像ファイルを開く
        /// </summary>
        /// <param name="fileName">画像ファイルのファイルパスを指定します。</param>
        /// <returns>生成したBitmapクラスオブジェクト</returns>
        private Bitmap ImageFileOpen(string fileName)
        {
            // 指定したファイルが存在するか？確認
            if (System.IO.File.Exists(fileName) == false) return null;

            // 拡張子の確認
            var ext = System.IO.Path.GetExtension(fileName).ToLower();

            // ファイルの拡張子が対応しているファイルかどうか調べる
            if (
                (ext != ".bmp") &&
                (ext != ".jpg") &&
                (ext != ".png") &&
                (ext != ".tif")
                )
            {
                return null;
            }

            //Bitmap bmp2;

            // ファイルストリームでファイルを開く
            using (var fs = new System.IO.FileStream(
                fileName,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read))
            {
                bmp2 = new Bitmap(fs);
            }
            return bmp2;
        }

        

        //画像の保存
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Graphics g3 = Graphics.FromImage(bitmap1))
            {
                if(bmp2 != null)
                {
                    g3.DrawImage(bmp2, 0, 0);
                }
                
                g3.DrawImage(bitmap3, 1, 0);
            }



            // 画像ファイルを開く
            Console.WriteLine("来たよ");

            //ファイルフィルタ  
            sfd.Filter = "PNG(*.png)|*.png|Bitmap(*.bmp)|*.bmp|Jpeg(*.jpeg)|*.jpeg";

            sfd.ShowDialog();

        }

        private void SaveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            Console.WriteLine("飛んで来たよ");
            string extension = System.IO.Path.GetExtension(sfd.FileName);

            switch (extension.ToUpper())
            {
                case ".BMP":
                    // ★★★PictureBoxのイメージをBMP形式で保存する★★★
                    pictureBox1.Image.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    bitmap1.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    break;
                case ".JPEG":
                    // ★★★PictureBoxのイメージをJPEG形式で保存する★★★
                    pictureBox1.Image.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitmap1.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case ".PNG":
                    // ★★★PictureBoxのイメージをGIF形式で保存する★★★
                    //pictureBox1.Image.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    bitmap1.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    break;
            }
            log(6);//画像保存
        }


        //これをクリックしたら画像の拡大　×1.1
        private void zoomInBtn_Click(object sender, EventArgs e)
        {
            //描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);
            //pictureBox2.SizeMode = PictureBoxSizeMode.Normal;
            g.DrawImage(imageSave, 0, 0, imageX * 11 / 10, imageY * 11 / 10);

            imageX = imageX * 11 / 10;
            imageY = imageY * 11 / 10;

            //Imageオブジェクトのリソースを解放する
            //imageSave.Dispose();
            //Graphicsオブジェクトのリソースを解放する
            //g.Dispose();

            pictureBox2.Image = canvas;
            bmp2 = canvas;

            //描画面を最前面に配置する
            pictureBox3.BringToFront();
            log(7);//画像　拡大
        }

        //これをクリックしたら画像の縮小　×0.9
        private void zoomOutBtn_Click(object sender, EventArgs e)
        {
            //描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);
            //pictureBox2.SizeMode = PictureBoxSizeMode.Normal;
            g.DrawImage(imageSave, 0, 0, imageX * 9 / 10, imageY * 9 / 10);

            imageX = imageX * 9 / 10;
            imageY = imageY * 9 / 10;

            //Imageオブジェクトのリソースを解放する
            //imageSave.Dispose();
            //Graphicsオブジェクトのリソースを解放する
            //g.Dispose();

            pictureBox2.Image = canvas;
            bmp2 = canvas;

            //描画面を最前面に配置する
            pictureBox3.BringToFront();

            log(8);//画像　縮小
        }

        //描画をクリアしたい
        private void 新規ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //画像を消す
            pictureBox2.Image = null;

            //描画を消す
            pictureBox3.Image = null;
            //bitmap3 = null;
            bitmap3 = new Bitmap(pictureBox3.Width, pictureBox3.Height);

            log(9);//描画クリア
        }


        int cmbWidthCount = 0;
        private void cmbWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbWidthCount > 0)
            {
                log(10);//線の太さ変更
            }
            cmbWidthCount = cmbWidthCount + 1;
        }


        int[] logCount = new int[11];//回数カウント

        //ログの出力
        public void log(int num)
        {
            //時刻の表示　【yyyy：年　MM：月　dd：日　HH：時間（24時間表記）　mm：分　ss：秒】
            DateTime dt = DateTime.Now;
            //Console.WriteLine(dt.ToString("yyyy/MM/dd HH:mm:ss"));
            Console.WriteLine(num);

            form2.label1.Text += dt.ToString("yyyy/MM/dd,HH:mm:ss,");

            if (num == 0)
            {
                form2.label1.Text += dt.ToString("0,お絵かきソフト起動\n");
            }
            else if(num == 1)
            {
                form2.label1.Text += dt.ToString("1,色変更ボタン押された\n");

            }
            else if (num == 2)
            {
                form2.label1.Text += dt.ToString("2,消しゴム選択\n");
            }
            else if (num == 3)
            {
                form2.label1.Text += dt.ToString("3,描画開始\n");
            }
            else if (num == 4)
            {
                form2.label1.Text += dt.ToString("4,描画終了\n");
            }
            else if (num == 5)
            {
                form2.label1.Text += dt.ToString("5,画像出力\n");
            }
            else if (num == 6)
            {
                form2.label1.Text += dt.ToString("6,保存\n");
            }
            else if (num == 7)
            {
                form2.label1.Text += dt.ToString("7,画像拡大\n");
            }
            else if (num == 8)
            {
                form2.label1.Text += dt.ToString("8,画像縮小\n");
            }
            else if (num == 9)
            {
                form2.label1.Text += dt.ToString("9,描画クリア\n");
            }
            else if (num == 10)
            {
                form2.label1.Text += dt.ToString("10,線の太さ変更\n");
            }

            
            logCount[num] ++ ;
            form2.label2.Text = "色変更" + logCount[1] + "\n";
            form2.label2.Text += "消しゴム選択" + logCount[2] + "\n";
            form2.label2.Text += "線の太さ変更" + logCount[10] + "\n";

            form2.label3.Text = "画像出力" + logCount[5] + "\n";
            form2.label3.Text += "画像拡大" + logCount[7] + "\n";
            form2.label3.Text += "描画縮小" + logCount[8] + "\n";

            form2.label4.Text = "保存" + logCount[6] + "\n";
            form2.label4.Text += "クリア" + logCount[9] + "\n";
            form2.label4.Text += "描画" + logCount[3] + "\n";


            //form2.chart1.yValues = 1;


            //form2.label1.Text = dt.ToString("yyyy/MM/dd HH:mm:ss" );

            //var list = new List<string>();

            //list.Add(dt.ToString("yyyy/MM/dd HH:mm:ss" + "　") );

            //form2.label1.Text = dt.ToString("yyyy/MM/dd HH:mm:ss" + "　");

            //form2.SendData = dt.ToString("yyyy/MM/dd HH:mm:ss");
            //string sendText = dt.ToString("yyyy/MM/dd HH:mm:ss");
            //array[0] = dt.ToString("yyyy/MM/dd HH:mm:ss");


        }

    }
}

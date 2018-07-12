using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form7 : Form
    {
        public int iii = 0;
        public int ii = 0;
        List<Panel> ListPanel = new List<Panel>();
        public string TxtPath;
        public string VideioPath = "";
        public List<double> RepeatT = new List<double>();
        public List<double> RepeatTS = new List<double>();
        public List<double> RepeatTE = new List<double>();
        public Form7()
        {
            InitializeComponent();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            ListPanel.Add(panel1);
            ListPanel.Add(panel2);
            ListPanel[0].BringToFront();
            textBox10.Visible = false;
            label1.Visible = false;
            timer1.Interval = 100;
            checkBox1.Visible = false;
            timer1.Start();
        }

        private void button3_Click(object sender, EventArgs e)//動作模組
        {
            ListPanel[0].BringToFront();
        }

        private void button4_Click(object sender, EventArgs e)//時間模組
        {
            ListPanel[1].BringToFront();
        }

        private void button1_Click(object sender, EventArgs e) //選擇影片
        {
            OpenFileDialog videio = new OpenFileDialog();
            if (string.IsNullOrEmpty(videio.InitialDirectory))
            {
                videio.InitialDirectory = "C:\\Users\\msi\\Desktop";  // 預設路徑
                videio.Filter = "Video File|*.mp4;*.mkv;*.avi;)";
                videio.Title = "請選擇影片";
                if (videio.ShowDialog(this) == DialogResult.Cancel)
                    return;
                textBox1.Text = videio.FileName;
                VideioPath = videio.FileName;
                axWindowsMediaPlayer1.URL = VideioPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)//選擇動作檔
        {
            int actt = 0;
            string[][] acttary;
            OpenFileDialog ofd = new OpenFileDialog(); 
            if (string.IsNullOrEmpty(ofd.InitialDirectory))
                ofd.InitialDirectory = "C:\\Users\\msi\\Desktop";  // 預設路徑

            ofd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            ofd.Title = "請開啟文字檔案";
            if (ofd.ShowDialog(this) == DialogResult.Cancel)
                return;
            textBox2.Text = ofd.FileName;
            TxtPath = ofd.FileName;
            using (StreamReader file = new StreamReader(@TxtPath))
            {
                while (file.Peek() >= 0)
                {
                    string line = file.ReadLine();
                    actt++;
                }
            }
            acttary = new string[actt][];
            double[] x = new double[actt];
            double[] y = new double[actt];
            double[] z = new double[actt];
            using (StreamReader file = new StreamReader(@TxtPath))
            {
                for (int i = 0; i < actt; i++)
                {
                    string line = file.ReadLine();
                    acttary[i] = line.Split('	');
                    x[i] = Convert.ToDouble(acttary[i][1]);
                    y[i] = Convert.ToDouble(acttary[i][2]);
                    z[i] = Convert.ToDouble(acttary[i][3]);
                }
            }
            //曲線圖
            chart1.Series.Clear();
            chart1.Legends.Clear();
            chart1.Titles.Clear();
            ChartArea chartArea = chart1.ChartAreas[0];
            chartArea.AxisY.Minimum = 0d;
            chartArea.AxisY.Maximum = 100d;
            chartArea.AxisX.Minimum = 0;
            chartArea.BorderWidth = 1000;
            chartArea.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.CursorX.SelectionColor = System.Drawing.SystemColors.Highlight;
            chartArea.CursorY.IsUserEnabled = true;
            chartArea.CursorY.IsUserSelectionEnabled = true;
            chartArea.CursorY.SelectionColor = System.Drawing.SystemColors.Highlight;
            Series series1 = new Series("X", 50);
            Series series2 = new Series("Y", 50);
            Series series3 = new Series("Z", 50);
            series1.Color = Color.Blue;
            series2.Color = Color.Red;
            series3.Color = Color.Yellow;
            series1.ChartType = SeriesChartType.Spline;
            series2.ChartType = SeriesChartType.Spline;
            series3.ChartType = SeriesChartType.Spline;
            series1.BorderWidth = 1;
            series2.BorderWidth = 1;
            series3.BorderWidth = 1;
            int ii = 0;
            double ia = 0;
            for (double index = 0; index < actt; index++)
            {
                ia += 0.05;
                series1.Points.AddXY(ia.ToString("0.0"), x[ii]);
                series2.Points.AddXY(ia.ToString("0.0"), y[ii]);
                series3.Points.AddXY(ia.ToString("0.0"), z[ii]);
                ii++;
            }
            //將序列新增到圖上
            chart1.Series.Add(series1);
            chart1.Series.Add(series2);
            chart1.Series.Add(series3);
            chart1.Legends.Add("");
            chart1.Titles.Add(TxtPath);//標題
        }

        private void button8_Click(object sender, EventArgs e) //加入模組
        {
            ii = 0;
            if (VideioPath == "")
            {
                MessageBox.Show("請選擇影片");
                return;
            }
            else
            {
                if (comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("請選擇模組");
                    return;
                }
                if (comboBox2.SelectedIndex == -1)
                {
                    MessageBox.Show("請選擇時間");
                    return;
                }
                string NowTime;
                string[] NowTimeAry;
                double NowTimeD;

                NowTimeD = Math.Round(axWindowsMediaPlayer1.Ctlcontrols.currentPosition - Math.Truncate(axWindowsMediaPlayer1.Ctlcontrols.currentPosition), 1);
                NowTime = axWindowsMediaPlayer1.Ctlcontrols.currentPositionString;
                string selitem = comboBox1.SelectedItem.ToString();
                string selitem2 = comboBox2.SelectedItem.ToString();
                if (NowTime.Length == 5)
                {
                    NowTimeAry = NowTime.Split(':');
                    if (Convert.ToDouble(NowTimeAry[1]) < 10)
                    {
                        NowTime = "0:" + NowTimeAry[0] + ":0" + (Convert.ToDouble(NowTimeAry[1]) + NowTimeD).ToString();
                    }
                    else
                    {
                        NowTime = "0:" + NowTimeAry[0] + ":" + (Convert.ToDouble(NowTimeAry[1]) + NowTimeD).ToString();
                    }
                    NowTimeD = Convert.ToDouble(NowTimeAry[0]) * 60 + (Convert.ToDouble(NowTimeAry[1]) + NowTimeD);
                }
                else
                {

                    NowTimeAry = NowTime.Split(':');
                    if (Convert.ToDouble(NowTimeAry[2]) < 10)
                    {
                        NowTime = NowTimeAry[0] + ":" + NowTimeAry[1] + ":0" + (Convert.ToDouble(NowTimeAry[2]) + NowTimeD).ToString();
                    }
                    else
                    {
                        NowTime = NowTimeAry[0] + ":" + NowTimeAry[1] + ":" + (Convert.ToDouble(NowTimeAry[2]) + NowTimeD).ToString();
                    }
                    NowTimeD = Convert.ToDouble(NowTimeAry[0]) * 60 + Convert.ToDouble(NowTimeAry[1]) * 60 + (Convert.ToDouble(NowTimeAry[1]) + NowTimeD);
                }
                RepeatT.Add(NowTimeD);
                for (int i = 0; i < RepeatT.Count; i++)
                {
                    if (RepeatT[i] == NowTimeD)
                    {
                        ii += 1;
                        if (ii == 2)
                        {
                            MessageBox.Show("那個時間已經有新增模組囉");
                            ii = 0;
                            RepeatT.Remove(i);
                            return;
                        }
                    }
                }
                ListViewItem listViewItem = new ListViewItem(NowTime);
                listViewItem.SubItems.Add(selitem + "(" + selitem2 + ")");
                listView1.Items.Add(listViewItem);
            }
        }

        private void button5_Click(object sender, EventArgs e) //更改模組
        {
            if (listView1.SelectedItems.Count > 0)
            {

                listView1.SelectedItems[0].SubItems[1].Text = comboBox1.SelectedItem.ToString();
            }
        }

        private void button6_Click(object sender, EventArgs e) //刪除-模組
        {

            if (listView1.SelectedItems.Count > 0)
            {
                double dt = 0;
                string[] aa = listView1.SelectedItems[0].Text.Split(':');
                dt = Convert.ToDouble(aa[0]) * 60 + Convert.ToDouble(aa[1]) * 60 + (Convert.ToDouble(aa[2]));

                RepeatT.Remove(dt);
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }

        }

        private void button7_Click(object sender, EventArgs e) //送出-模組
        {
            if (VideioPath == "")
            {
                MessageBox.Show("請選擇影片");
                return;
            }
                if (listView1.Items.Count == 0)
                {
                    return;
                }
                string ActionTxtPath;
                char[] c = { '	' };
                int actcounter = 0;
                string NewTxt = textBox10.Text + ".txt";
                int actt = listView1.Items.Count;
                string[][][] ActAry;
                ActAry = new string[actt][][];
                actt = 0;
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Text Documents|*.txt", ValidateNames = true })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        while (listView1.Items.Count != 0)
                        {
                            ActionTxtPath = "模組\\" + listView1.Items[0].SubItems[1].Text + ".txt";
                            string[] ListTime = listView1.Items[0].SubItems[0].Text.Split(':');
                            using (StreamReader txt = new StreamReader(@ActionTxtPath))//讀動作檔長度
                            {
                                while (txt.Peek() >= 0)
                                {
                                    String line = txt.ReadLine();
                                    actcounter++;
                                }
                            }
                            ActAry[actt] = new string[actcounter][];

                            using (StreamReader txt = new StreamReader(@ActionTxtPath))//讀動作檔
                            {
                                for (int i = 0; i < actcounter; i++)
                                {
                                    String line1 = txt.ReadLine();
                                    ActAry[actt][i] = line1.Split(c);
                                    if ((Convert.ToDouble(ListTime[2]) + Convert.ToDouble(i) * 0.05) > 60)
                                    {
                                        ActAry[actt][i][0] = ListTime[0] + ":" + (Convert.ToInt32(ListTime[1]) + 1).ToString() + ":" + (Convert.ToDouble(ListTime[2]) + Convert.ToDouble(i) * 0.05 - 60).ToString("#0.000");
                                    }
                                    else
                                    {
                                        ActAry[actt][i][0] = ListTime[0] + ":" + ListTime[1] + ":" + (Convert.ToDouble(ListTime[2]) + Convert.ToDouble(i) * 0.05).ToString("#0.000");
                                    }

                                }
                            }
                            using (StreamWriter file1 = new StreamWriter(new FileStream(sfd.FileName, FileMode.Append)))
                            {
                                foreach (string[] i in ActAry[actt])
                                {
                                    foreach (string ii in i)
                                    {
                                        file1.Write(ii);
                                        file1.Write("	");
                                    }
                                    file1.WriteLine();
                                }

                            }

                        /*   using (TextWriter tw = new StreamWriter(new FileStream(sfd.FileName, FileMode.Create), Encoding.UTF8))
                           {
                               foreach (string[] i in ActAry[actt])
                               {
                                   foreach (string ii in i)
                                   {
                                       tw.Write(ii);
                                       tw.Write("	");
                                   }
                                   tw.WriteLine();
                               }
                           }
                      */
                        listView1.Items.RemoveAt(0);
                        actt++;
                        actcounter = 0;
                    }
                }
                    else
                    {
                        return;
                    }
            }
            MessageBox.Show("成功囉");
            RepeatT.Clear();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) //選擇新增檔案
        {
            if (checkBox1.Checked == true)
            {
                label1.Visible = true;
                textBox10.Visible = true;
            }
            else
            {
                label1.Visible = false;
                textBox10.Visible = false;
            }
        }

        private void button9_Click(object sender, EventArgs e) //修改-時間
        {
            if(textBox8.Text == "")
            {
                MessageBox.Show("請輸入修改秒數");
                return;
            }
            iii = 0;
            double StartTimeD;
            double EndTimeD; 
            StartTimeD = Convert.ToInt32(textBox3.Text) * 3600 + Convert.ToInt32(textBox4.Text) * 60 + Convert.ToDouble(textBox5.Text);
            EndTimeD = Convert.ToInt32(textBox6.Text) * 3600 + Convert.ToInt32(textBox7.Text) * 60 + Convert.ToDouble(textBox8.Text);
            RepeatTS.Add(StartTimeD);
            RepeatTE.Add(EndTimeD);
            for (int i = 0; i < RepeatTS.Count; i++)
            {
                if (RepeatTS[i] <= StartTimeD && RepeatTE[i] >= StartTimeD || RepeatTS[i] <= EndTimeD && RepeatTE[i] >= EndTimeD)
                {
                    iii++;
                    if (iii == 2)
                    {
                        MessageBox.Show("那個時間已經修改過囉");
                        iii = 0;
                        RepeatTS.Remove(i);
                        RepeatTE.Remove(i);
                        return;
                    }
                }
            }
            ListViewItem listViewItem2 = new ListViewItem(textBox3.Text + ":" + textBox4.Text + ":" + textBox5.Text + "-" + textBox6.Text + ":" + textBox7.Text + ":" + textBox8.Text);
            listViewItem2.SubItems.Add(textBox9.Text);
            listView2.Items.Add(listViewItem2);
        }

        private void button10_Click(object sender, EventArgs e)//送出-時間
        {
            if (TxtPath == null)
            {
                MessageBox.Show("請選擇動作檔位置");
                return;
            }
            bool start = false;
            bool pn = true;
            bool aa = true;
            int counter = 0;
            int deltimecounter = 0;
            char[] c = { '	' };
            double NumRange = 0;
            double InputNum = 0;
            using (StreamReader file = new StreamReader(@TxtPath))
            {
                while (file.Peek() >= 0)
                {
                    String line = file.ReadLine();
                    counter++;
                }
            }
            int actt = listView2.Items.Count;
            string[][] MyAry = new string[counter][];
            string[][][] TimeNum = new string[actt + 1][][];
            string[][] se = new string[actt][];
            string[][] ListTimeS = new string[actt][];
            string[][] ListTimeE = new string[actt][];
            //讀入動作檔
            using (StreamReader file = new StreamReader(@TxtPath))
            {
                for (int i = 0; i < counter; i++)
                {
                    string line = file.ReadLine();
                    MyAry[i] = line.Split(c);
                }
            }
            actt = 0;
            while (listView2.Items.Count != 0)
            {
                TimeNum[actt] = new string[counter][];
                se[actt] = listView2.Items[0].SubItems[0].Text.Split('-');
                ListTimeS[actt] = se[actt][0].Split(':');
                ListTimeE[actt] = se[actt][1].Split(':');
                InputNum = Convert.ToDouble(listView2.Items[0].SubItems[1].Text);
                int StartTimeNumH = Convert.ToInt32(ListTimeS[actt][0]);
                int StartTimeNumM = Convert.ToInt32(ListTimeS[actt][1]);
                double StartTimeNumS = Convert.ToDouble(ListTimeS[actt][2]);
                int EndTimeNumH = Convert.ToInt32(ListTimeE[actt][0]);
                int EndTimeNumM = Convert.ToInt32(ListTimeE[actt][1]);
                double EndTimeNumS = Convert.ToDouble(ListTimeE[actt][2]);
                NumRange = (EndTimeNumM - StartTimeNumM) * 60 + (EndTimeNumS - StartTimeNumS) * 20 + 1;//算出範圍
                deltimecounter = Math.Abs(Convert.ToInt32(InputNum * 10) * 2 );
                if (InputNum < 0)
                {
                    pn = false;
                }
                //減時間的情況
                if (!pn) 
                {
                    StartTimeNumS = StartTimeNumS + InputNum;
                    if (StartTimeNumS < 0)
                    {
                        StartTimeNumS = (StartTimeNumS) + 60;
                        StartTimeNumM = StartTimeNumM - 1;
                        if (Convert.ToInt32(StartTimeNumM) < 0)
                        {
                            StartTimeNumS = StartTimeNumM + 60;
                            StartTimeNumM = StartTimeNumH - 1;
                        }
                    }
                    //運算
                    for (int ii = 0; ii < counter; ii++)
                    {

                        if (MyAry[ii][0] != "a")
                        {
                            TimeNum[actt][ii] = MyAry[ii][0].Split(':');
                        }
                        else
                        {
                            continue;
                        }
                        if (Convert.ToInt32(TimeNum[actt][ii][0]) == StartTimeNumH && Convert.ToInt32(TimeNum[actt][ii][1]) == StartTimeNumM && Convert.ToDouble(TimeNum[actt][ii][2]) == StartTimeNumS)
                        {
                            start = true;
                        }
                        if (deltimecounter < 0 && NumRange > 0)
                        {
                            if (Convert.ToDouble(TimeNum[actt][ii][2]) + InputNum < 0)
                            {
                                TimeNum[actt][ii][2] = (Convert.ToDouble(TimeNum[actt][ii][2]) + InputNum + 60).ToString("#0.000");
                                TimeNum[actt][ii][1] = (Convert.ToInt32(TimeNum[actt][ii][1]) - 1).ToString();
                            }
                            else
                            {
                                TimeNum[actt][ii][2] = (Convert.ToDouble(TimeNum[actt][ii][2]) + InputNum).ToString("#0.000");
                            }
                            NumRange -= 1;
                        }
                        MyAry[ii][0] = TimeNum[actt][ii][0] + ":" + TimeNum[actt][ii][1] + ":" + TimeNum[actt][ii][2];//輸入秒數值
                        if (start)
                        {
                            MyAry[ii][0] = "a";
                            deltimecounter -= 1;
                            MessageBox.Show(deltimecounter.ToString());
                        }
                        if (deltimecounter == 0)
                        {
                            start = false;
                            deltimecounter -= 1;
                        }
                    }
                }
                //加時間的情況
                if (pn) 
                {
                    //運算
                    for (int ii = 0; ii < counter; ii++)
                    {
                        if (MyAry[ii][0] != "a")
                        {
                            TimeNum[actt][ii] = MyAry[ii][0].Split(':');
                        }
                        else
                        {
                            continue;
                        }
                        if (Convert.ToInt32(TimeNum[actt][ii][0]) == StartTimeNumH && Convert.ToInt32(TimeNum[actt][ii][1]) == StartTimeNumM && Convert.ToDouble(TimeNum[actt][ii][2]) == StartTimeNumS)
                        {
                            start = true;
                        }
                        if (start)
                        {
                            if (Convert.ToDouble(TimeNum[actt][ii][2]) + InputNum > 60)
                            {
                                TimeNum[actt][ii][2] = (Convert.ToDouble(TimeNum[actt][ii][2]) + InputNum - 60).ToString("#0.000");
                                TimeNum[actt][ii][1] = (Convert.ToInt32(TimeNum[actt][ii][1]) + 1).ToString();
                            }
                            else
                            {
                                TimeNum[actt][ii][2] = (Convert.ToDouble(TimeNum[actt][ii][2]) + InputNum).ToString("#0.000");
                            }
                            if (Convert.ToInt32(TimeNum[actt][ii][1]) > 60)
                            {
                                TimeNum[actt][ii][1] = (Convert.ToInt32(TimeNum[actt][ii][1]) - 60).ToString();
                                TimeNum[actt][ii][0] = (Convert.ToInt32(TimeNum[actt][ii][0]) + 1).ToString();
                            }
                            NumRange -= 1;
                        }
                        if (NumRange == 0)
                        {
                            start = false;
                            NumRange -= 1;
                        }
                        MyAry[ii][0] = TimeNum[actt][ii][0] + ":" + TimeNum[actt][ii][1] + ":" + TimeNum[actt][ii][2];//輸入秒數值
                        if (NumRange == -1 && deltimecounter > 0)
                        {
                            MyAry[ii][0] = "a";
                            deltimecounter -= 1;
                        }
                    }
                }
                listView2.Items.RemoveAt(0);
                actt++;
            }
            //寫入
            using (StreamWriter file1 = new StreamWriter(@TxtPath + "修改後.txt"))
            {
                foreach (string[] i in MyAry)
                {
                    foreach (string ii in i)
                    {
                        if (ii == "a")
                        {
                            aa = false;
                            break;
                        }
                        file1.Write(ii);
                        file1.Write("	");
                    }
                    if (!aa)
                    {
                        aa = true;
                        continue;
                    }
                    file1.WriteLine();
                }
            }
            MessageBox.Show("成功囉");
            RepeatTS.Clear();
            RepeatTE.Clear();
        }

        private void button11_Click(object sender, EventArgs e)//刪除-時間
        {
            if (listView2.SelectedItems.Count > 0)
            {
                listView2.Items.Remove(listView2.SelectedItems[0]);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double NowTimeD = Math.Round(axWindowsMediaPlayer1.Ctlcontrols.currentPosition - Math.Truncate(axWindowsMediaPlayer1.Ctlcontrols.currentPosition), 1);
            string NowTime = axWindowsMediaPlayer1.Ctlcontrols.currentPositionString;
            string[] NowTimeAry;
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying ) { 
            if (NowTime.Length == 5)
            {
                NowTimeAry = NowTime.Split(':');
                textBox3.Text = "0";
                textBox4.Text = NowTimeAry[0];
                textBox5.Text = (Convert.ToDouble(NowTimeAry[1])+NowTimeD).ToString();
                textBox6.Text = "0";
                textBox7.Text = NowTimeAry[0];
                textBox8.Text = (Convert.ToDouble(NowTimeAry[1]) + NowTimeD).ToString();
            }
            if(NowTime.Length >5)
            {
                NowTimeAry = NowTime.Split(':');
                textBox3.Text = NowTimeAry[0];
                textBox4.Text = NowTimeAry[1];
                textBox5.Text = (Convert.ToDouble(NowTimeAry[2]) + NowTimeD).ToString();
                textBox6.Text = NowTimeAry[0];
                textBox7.Text = NowTimeAry[1];
                textBox8.Text = (Convert.ToDouble(NowTimeAry[2]) + NowTimeD).ToString();

            }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MessageBox.Show(comboBox1.SelectedValue.ToString());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (label2.Visible )
            {
                label2.Visible = false;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (label3.Visible)
            {
                label3.Visible = false;
            }
        }
    }
}

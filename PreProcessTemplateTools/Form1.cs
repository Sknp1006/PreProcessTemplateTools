using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.IO;
using static MyTools.FileComparer;

namespace PreProcessTemplateTools
{
    public partial class Form1 : Form
    {
        #region 初始化变量
        int TotalFiles = 0;  // 文件总数
        int Index = 0;  // 索引计数
        List<HProcessH> ModelObjectList = new List<HProcessH>();
        List<string> ModelFilesPath = new List<string>();
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        #region 读取文件

        public void ClearModelObjectList()
        {
            if (ModelObjectList.Count != 0)
            {
                ModelObjectList.Clear();
            }
        }


        private void TSMI_OpenFile_Click(object sender, EventArgs e)
        {
            ClearModelObjectList();
            openFileDialog1.Title = "打开模板";
            openFileDialog1.Filter = "题卡模板|*.jpg;*.png;*.jpeg;*.bmp";
            openFileDialog1.Multiselect = true;
            openFileDialog1.InitialDirectory = @"";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileNames[0] == "openFileDialog1")
            {
                // 表示没选择文件
                Console.WriteLine("未选择模板文件");
                return;
            }
            ModelFilesPath = openFileDialog1.FileNames.ToList();
            string[] ModelFilesArray = ModelFilesPath.ToArray();
            Array.Sort(ModelFilesArray, new FileNameComparer1());
            ModelFilesPath = ModelFilesArray.ToList();

            foreach (var Model in ModelFilesPath)
            {
                HProcessH hProcessH = new HProcessH(hWindowControl, Model);
                ModelObjectList.Add(hProcessH);
            }
            TotalFiles = ModelObjectList.Count();

            // 显示第一张模板
            Index = 1;
            SwitchPage(Index);
        }


        private void TSMI_OpenFolder_Click(object sender, EventArgs e)
        {
            ClearModelObjectList();
            folderBrowserDialog1.Description = "打开模板文件夹";
            folderBrowserDialog1.ShowDialog();
            if (folderBrowserDialog1.SelectedPath == "")
            {
                // 表示未选择文件夹
                Console.WriteLine("未选择模板文件夹");
                return;
            }
            string ModelFolder = folderBrowserDialog1.SelectedPath;
            ModelFilesPath = Directory.GetFiles(ModelFolder).ToList();
            string[] ModelFilesArray = ModelFilesPath.ToArray();
            Array.Sort(ModelFilesArray, new FileNameComparer1());
            ModelFilesPath = ModelFilesArray.ToList();

            foreach (var Model in ModelFilesPath)
            {
                HProcessH hProcessH = new HProcessH(hWindowControl, Model);
                ModelObjectList.Add(hProcessH);
            }
            TotalFiles = ModelObjectList.Count();

            // 显示第一张模板
            Index = 1;
            SwitchPage(Index);
        }
        #endregion

        #region 翻页功能
        private void button_PageUp_Click(object sender, EventArgs e)
        {
            //Index--;
            //if (Index <= 0)
            //{
            //    Index++;
            //    Console.WriteLine("这是第一张");
            //}
            //else
            //{
            //    SwitchPage(Index);
            //}

            try
            {
                SwitchPage(--Index);
            }
            catch (System.Exception ex)
            {
                SwitchPage(++Index);
                Console.WriteLine(ex);
            }
        }


        private void button_PageDown_Click(object sender, EventArgs e)
        {
            //Index++;
            //if (Index > ModelFilesPath.Count)
            //{
            //    Index--;
            //    Console.WriteLine("已经是最后一张");
            //}
            //else
            //{
            //    SwitchPage(Index);
            //}

            try
            {
                SwitchPage(++Index);
            }
            catch (System.Exception ex)
            {
                SwitchPage(--Index);
                Console.WriteLine(ex);
            }
        }
        #endregion

        #region 显示结果
        private void SwitchPage(int index)
        {
            // ModelFilesPath[Index - 1]
            try
            {
                ModelObjectList[index - 1].SetZoomValue(1);
                ModelObjectList[index - 1].LoadModel();
                ModelObjectList[index - 1].ProcessTikaModel();
                ModelObjectList[index - 1].ShowImage();
            }
            catch(System.Exception ex)
            {
                throw ex;
            }
            // 
            label_modelNum.Text = index.ToString() + string.Format("/{0}", TotalFiles);
            textBox_FileName.Text = ModelFilesPath[index - 1];
            // Update_trackBar
            trackBar_a.Minimum = (int)ModelObjectList[index - 1].tikaModel.MinGrayValue.D;
            trackBar_a.Maximum = (int)ModelObjectList[index - 1].tikaModel.MaxGrayValue.D;
            trackBar_b.Minimum = (int)ModelObjectList[index - 1].tikaModel.MinGrayValue.D;
            trackBar_b.Maximum = (int)ModelObjectList[index - 1].tikaModel.MaxGrayValue.D;

            ////trackBar_a.Value = trackBar_a.Minimum;
            ////trackBar_b.Value = trackBar_a.Maximum;

            label_min_b.Text = ((int)ModelObjectList[index - 1].tikaModel.MinGrayValue.D).ToString();
            label_max_b.Text = ((int)ModelObjectList[index - 1].tikaModel.MaxGrayValue.D).ToString();
        }
        #endregion

        #region 调整trackBar
        private void trackBar_a_Scroll(object sender, EventArgs e)
        {
            // min < a < b < max
            int a = trackBar_a.Value;
            int b = trackBar_b.Value;
            if (a > b)
            {
                trackBar_b.Value = trackBar_a.Value;
            }
        }


        private void trackBar_b_Scroll(object sender, EventArgs e)
        {
            // min < a < b < max
            int a = trackBar_a.Value;
            int b = trackBar_b.Value;
            if (a > b)
            {
                trackBar_a.Value = trackBar_b.Value;
            }
        }
        #endregion

        #region 预览&保存
        private void button_Preview_Click(object sender, EventArgs e)
        {
            if (trackBar_a.Value == trackBar_b.Value)
            {
                MessageBox.Show("目前 a 值 等于 b 值，请确保b 值大于a 值。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                ModelObjectList[Index - 1].ProcessTikaModel(trackBar_a.Value, trackBar_b.Value);
                ModelObjectList[Index - 1].ReDraw(ModelObjectList[Index - 1].opt.UsedRegion);
                ModelObjectList[Index - 1].ShowImage('o');
            }
            catch
            {
                Console.WriteLine("预览失败");
            }

        }


        private void button_SavePreview_Click(object sender, EventArgs e)
        {
            ModelObjectList[Index - 1].SaveImage();
        }
        #endregion


        #region 鼠标事件
        Point startPoint = Point.Empty;
        Point endPoint = Point.Empty;
        int Delta = 0;  // 及时复位
        private void hWindowControl_HMouseWheel(object sender, HMouseEventArgs e)
        {
            HTuple row, column, button;
            try
            {
                HOperatorSet.GetMposition(ModelObjectList[Index - 1].HWindowControl_0.HalconWindow, out row, out column, out button);
                Console.WriteLine(string.Format("HMouseWheel触发：row:{0}  column:{1}  button:{2}", row, column, button));
            }
            catch (HalconDotNet.HOperatorException)
            {
                // 移到控件外触发异常
            }
            catch (System.ArgumentOutOfRangeException)
            {
                // 没加载模板就触发了HMouseWheel事件
                //Console.WriteLine("没加载模板就触发了HMouseWheel事件");
            }

            // 图像缩放（原理是假装控件大小变化，使图像适应窗口）
            Delta = Delta + e.Delta;
            if (Delta <= 0) 
            {
                Delta = 0;
                return;
            }
            if (Delta > 1200)
            {
                Delta = 1200;
                return;
            }
            Console.WriteLine("缩放比:" + Delta);

            try
            {
                HTuple row1_0, column1_0, row2_0, column2_0;
                HTuple row1, column1, row2, column2;
                ModelObjectList[Index - 1].HWindowControl_0.HalconWindow.GetPart(out row1_0, out column1_0, out row2_0, out column2_0);
                ModelObjectList[Index - 1].SetZoomValue(Delta / 100);
                ModelObjectList[Index - 1].CalScaleValue(ModelObjectList[Index - 1].tikaModel.Width, ModelObjectList[Index - 1].tikaModel.Height, ModelObjectList[Index - 1].HWindowControl_0, out row1, out column1, out row2, out column2);
                ModelObjectList[Index - 1].HWindowControl_0.HalconWindow.SetPart(row1, column1, row2, column2);
                ModelObjectList[Index - 1].HWindowControl_0.HalconWindow.GetPart(out row1, out column1, out row2, out column2);

                double dbRowMove, dbColMove;
                dbRowMove = row1_0 - row1;
                dbColMove = column1_0 - column1;

                ModelObjectList[Index - 1].ShowImage(dbRowMove, dbColMove);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return;
            }
        }


        private void hWindowControl_HMouseUp(object sender, HMouseEventArgs e)
        {
            HTuple row = new HTuple();
            HTuple column = new HTuple();
            HTuple button = new HTuple();

            endPoint = new Point((int)e.X, (int)e.Y);
            this.hWindowControl.Cursor = Cursors.Default;  // 指针（默认）

            try
            {
                HOperatorSet.GetMposition(ModelObjectList[Index - 1].HWindowControl_0.HalconWindow, out row, out column, out button);
                //Console.WriteLine(string.Format("HMouseUp触发：row:{0}  column:{1}  button:{2}", row, column, button));
                //Console.WriteLine("endPoint:" + endPoint);
            }
            catch (HalconDotNet.HOperatorException)
            {
                // 移到控件外触发异常
            }
            catch (System.ArgumentOutOfRangeException)
            {
                // 没加载模板就触发了HMouseUp事件
                //Console.WriteLine("没加载模板就触发了HMouseUp事件");
            }

            // 鼠标拖动
            if (e.Button == MouseButtons.Right)  // 右键滑动
                //if (e.Button == MouseButtons.Left && radio_select.Checked == true)
            {
                if (startPoint.X == 0 || startPoint.Y == 0)
                {
                    return;
                }
                try
                {
                    double dbRowMove, dbColMove;
                    dbRowMove = startPoint.Y - endPoint.Y;  //计算光标在X轴拖动的距离
                    dbColMove = startPoint.X - endPoint.X;  //计算光标在Y轴拖动的距离
                    ModelObjectList[Index - 1].ShowImage(dbRowMove, dbColMove);
                }
                catch (HalconException HDevExpDefaultException)
                {

                }
                catch (System.ArgumentOutOfRangeException)
                {

                }
            }
            // 画笔
            else if (e.Button == MouseButtons.Left && radio_brush.Checked == true)
            {
                try
                {
                    HTuple row1 = new HTuple(startPoint.Y);
                    HTuple column1 = new HTuple(startPoint.X);
                    HTuple row2 = new HTuple(endPoint.Y);
                    HTuple column2 = new HTuple(endPoint.X);
                    // 左上到右下的绘制方式
                    ModelObjectList[Index - 1].DrawRectangle1(row1, column1, row2, column2);

                }
                catch (HalconException HDevExpDefaultException)
                {

                }
                catch (System.ArgumentOutOfRangeException)
                {

                }

        }
            // 橡皮
            else if (e.Button == MouseButtons.Left && radio_eraser.Checked == true)
            {
                try
                {
                    HTuple row1 = new HTuple(startPoint.Y);
                    HTuple column1 = new HTuple(startPoint.X);
                    HTuple row2 = new HTuple(endPoint.Y);
                    HTuple column2 = new HTuple(endPoint.X);
                    // 左上到右下的绘制方式
                    ModelObjectList[Index - 1].DrawWhiteBlock(row1, column1, row2, column2);

                }
                catch (HalconException HDevExpDefaultException)
                {

                }
                catch (System.ArgumentOutOfRangeException)
                {

                }
            }

            row.Dispose();
            column.Dispose();
            button.Dispose();

        }


        private void hWindowControl_HMouseDown(object sender, HMouseEventArgs e)
        {
            //Console.WriteLine("鼠标按下");

            HTuple row = new HTuple();
            HTuple column = new HTuple();
            HTuple button = new HTuple();
            startPoint = new Point((int)e.X, (int)e.Y);
            this.hWindowControl.Cursor = Cursors.Default;

            try
            {
                HOperatorSet.GetMposition(ModelObjectList[Index - 1].HWindowControl_0.HalconWindow, out row, out column, out button);
                //Console.WriteLine(string.Format("HMouseDown触发：row:{0}  column:{1}  button:{2}", row, column, button));

                Console.WriteLine("startPoint:" + startPoint);
            }
            catch (HalconDotNet.HOperatorException)
            {
                // 移到控件外触发异常
            }
            catch (System.ArgumentOutOfRangeException)
            {
                // 没加载模板就触发了HMouseDown事件
                //Console.WriteLine("没加载模板就触发了HMouseDown事件");
            }

            row.Dispose();
            column.Dispose();
            button.Dispose();
        }

        
        private void hWindowControl_HMouseMove(object sender, HMouseEventArgs e)
        {
            HTuple row = new HTuple();
            HTuple column = new HTuple();
            HTuple button = new HTuple();

            try
            {
                HOperatorSet.GetMposition(ModelObjectList[Index - 1].HWindowControl_0.HalconWindow, out row, out column, out button);
                //Console.WriteLine(string.Format("HMouseMove触发：row:{0}  column:{1}  button:{2}", row, column, button));
            }
            catch (HalconDotNet.HOperatorException)
            {
                // 移到控件外触发异常
            }
            catch (System.ArgumentOutOfRangeException)
            {
                // 没加载模板就触发了HMouseMove事件
                //Console.WriteLine("没加载模板就触发了HMouseMove事件");
            }

            row.Dispose();
            column.Dispose();
            button.Dispose();
        }


        private void hWindowControl_MouseLeave(object sender, EventArgs e)
        {
            startPoint = Point.Empty;
            endPoint = Point.Empty;
            this.hWindowControl.Cursor = Cursors.Default;
        }

        #endregion


        private void button_Undo_Click(object sender, EventArgs e)
        {
            //pop()
            try
            {
                // 撤销新建了Image对象
                ModelObjectList[Index - 1].opt.SubOperation();  // 去掉最后一个
                //ModelObjectList[Index - 1].LoadModel();
                ModelObjectList[Index - 1].ProcessTikaModel(trackBar_a.Value, trackBar_b.Value);
                ModelObjectList[Index - 1].ReDraw(ModelObjectList[Index - 1].opt.UsedRegion);
            }
            catch
            {

            }
        }

        private void TSMI_Quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_OpenFodler_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "打开模板文件夹";
            folderBrowserDialog1.ShowDialog();
            if (folderBrowserDialog1.SelectedPath == "")
            {
                // 表示未选择文件夹
                Console.WriteLine("文件夹路径为空");
                return;
            }
            string ModelFolder = folderBrowserDialog1.SelectedPath;
            textBox_Fodler.Text = ModelFolder;
        }

        private void button_Extract_Click(object sender, EventArgs e)
        {
            Extract extract = new Extract(textBox_Fodler.Text, (int)KHlength.Value);
        }

        private void hWindowControl_SizeChanged(object sender, EventArgs e)
        {
            ModelObjectList[Index - 1].ShowImage();
        }
    }
}

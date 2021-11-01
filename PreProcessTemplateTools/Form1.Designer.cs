
namespace PreProcessTemplateTools
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.TSMI_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_OpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMI_Quit = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox_toolBox = new System.Windows.Forms.GroupBox();
            this.radio_select = new System.Windows.Forms.RadioButton();
            this.radio_eraser = new System.Windows.Forms.RadioButton();
            this.radio_brush = new System.Windows.Forms.RadioButton();
            this.button_Undo = new System.Windows.Forms.Button();
            this.groupBox_parameter = new System.Windows.Forms.GroupBox();
            this.button_SavePreview = new System.Windows.Forms.Button();
            this.button_Preview = new System.Windows.Forms.Button();
            this.button_AutoProcess = new System.Windows.Forms.Button();
            this.textBox_FileName = new System.Windows.Forms.TextBox();
            this.label_FileName = new System.Windows.Forms.Label();
            this.label_max_b = new System.Windows.Forms.Label();
            this.label_max_a = new System.Windows.Forms.Label();
            this.label_min_b = new System.Windows.Forms.Label();
            this.label_min_a = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBar_b = new System.Windows.Forms.TrackBar();
            this.trackBar_a = new System.Windows.Forms.TrackBar();
            this.button_PageUp = new System.Windows.Forms.Button();
            this.button_PageDown = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.hWindowControl = new HalconDotNet.HWindowControl();
            this.label_modelNum = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_Extract = new System.Windows.Forms.Button();
            this.KHlength = new System.Windows.Forms.NumericUpDown();
            this.label_KHlength = new System.Windows.Forms.Label();
            this.textBox_Fodler = new System.Windows.Forms.TextBox();
            this.button_OpenFodler = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.groupBox_toolBox.SuspendLayout();
            this.groupBox_parameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_b)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_a)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KHlength)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_Open});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1122, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // TSMI_Open
            // 
            this.TSMI_Open.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMI_OpenFile,
            this.TSMI_Quit});
            this.TSMI_Open.Name = "TSMI_Open";
            this.TSMI_Open.Size = new System.Drawing.Size(44, 21);
            this.TSMI_Open.Text = "打开";
            // 
            // TSMI_OpenFile
            // 
            this.TSMI_OpenFile.Name = "TSMI_OpenFile";
            this.TSMI_OpenFile.Size = new System.Drawing.Size(124, 22);
            this.TSMI_OpenFile.Text = "打开模板";
            this.TSMI_OpenFile.Click += new System.EventHandler(this.TSMI_OpenFile_Click);
            // 
            // TSMI_Quit
            // 
            this.TSMI_Quit.Name = "TSMI_Quit";
            this.TSMI_Quit.Size = new System.Drawing.Size(124, 22);
            this.TSMI_Quit.Text = "退出";
            this.TSMI_Quit.Click += new System.EventHandler(this.TSMI_Quit_Click);
            // 
            // groupBox_toolBox
            // 
            this.groupBox_toolBox.Controls.Add(this.radio_select);
            this.groupBox_toolBox.Controls.Add(this.radio_eraser);
            this.groupBox_toolBox.Controls.Add(this.radio_brush);
            this.groupBox_toolBox.Controls.Add(this.button_Undo);
            this.groupBox_toolBox.Location = new System.Drawing.Point(12, 29);
            this.groupBox_toolBox.Name = "groupBox_toolBox";
            this.groupBox_toolBox.Size = new System.Drawing.Size(230, 114);
            this.groupBox_toolBox.TabIndex = 2;
            this.groupBox_toolBox.TabStop = false;
            this.groupBox_toolBox.Text = "涂改工具";
            // 
            // radio_select
            // 
            this.radio_select.AutoSize = true;
            this.radio_select.Checked = true;
            this.radio_select.Enabled = false;
            this.radio_select.Location = new System.Drawing.Point(22, 23);
            this.radio_select.Name = "radio_select";
            this.radio_select.Size = new System.Drawing.Size(47, 16);
            this.radio_select.TabIndex = 6;
            this.radio_select.TabStop = true;
            this.radio_select.Text = "选择";
            this.radio_select.UseVisualStyleBackColor = true;
            this.radio_select.Visible = false;
            // 
            // radio_eraser
            // 
            this.radio_eraser.AutoSize = true;
            this.radio_eraser.Location = new System.Drawing.Point(22, 67);
            this.radio_eraser.Name = "radio_eraser";
            this.radio_eraser.Size = new System.Drawing.Size(95, 16);
            this.radio_eraser.TabIndex = 5;
            this.radio_eraser.TabStop = true;
            this.radio_eraser.Text = "橡皮（涂白）";
            this.radio_eraser.UseVisualStyleBackColor = true;
            // 
            // radio_brush
            // 
            this.radio_brush.AutoSize = true;
            this.radio_brush.Location = new System.Drawing.Point(22, 45);
            this.radio_brush.Name = "radio_brush";
            this.radio_brush.Size = new System.Drawing.Size(95, 16);
            this.radio_brush.TabIndex = 4;
            this.radio_brush.TabStop = true;
            this.radio_brush.Text = "画笔（涂黑）";
            this.radio_brush.UseVisualStyleBackColor = true;
            // 
            // button_Undo
            // 
            this.button_Undo.Location = new System.Drawing.Point(138, 45);
            this.button_Undo.Name = "button_Undo";
            this.button_Undo.Size = new System.Drawing.Size(75, 60);
            this.button_Undo.TabIndex = 1;
            this.button_Undo.Text = "撤销";
            this.button_Undo.UseVisualStyleBackColor = true;
            this.button_Undo.Click += new System.EventHandler(this.button_Undo_Click);
            // 
            // groupBox_parameter
            // 
            this.groupBox_parameter.Controls.Add(this.button_SavePreview);
            this.groupBox_parameter.Controls.Add(this.button_Preview);
            this.groupBox_parameter.Controls.Add(this.button_AutoProcess);
            this.groupBox_parameter.Controls.Add(this.textBox_FileName);
            this.groupBox_parameter.Controls.Add(this.label_FileName);
            this.groupBox_parameter.Controls.Add(this.label_max_b);
            this.groupBox_parameter.Controls.Add(this.label_max_a);
            this.groupBox_parameter.Controls.Add(this.label_min_b);
            this.groupBox_parameter.Controls.Add(this.label_min_a);
            this.groupBox_parameter.Controls.Add(this.label2);
            this.groupBox_parameter.Controls.Add(this.label1);
            this.groupBox_parameter.Controls.Add(this.trackBar_b);
            this.groupBox_parameter.Controls.Add(this.trackBar_a);
            this.groupBox_parameter.Location = new System.Drawing.Point(12, 149);
            this.groupBox_parameter.Name = "groupBox_parameter";
            this.groupBox_parameter.Size = new System.Drawing.Size(230, 287);
            this.groupBox_parameter.TabIndex = 3;
            this.groupBox_parameter.TabStop = false;
            this.groupBox_parameter.Text = "参数调整";
            // 
            // button_SavePreview
            // 
            this.button_SavePreview.Location = new System.Drawing.Point(116, 158);
            this.button_SavePreview.Name = "button_SavePreview";
            this.button_SavePreview.Size = new System.Drawing.Size(61, 28);
            this.button_SavePreview.TabIndex = 11;
            this.button_SavePreview.Text = "保存";
            this.button_SavePreview.UseVisualStyleBackColor = true;
            this.button_SavePreview.Click += new System.EventHandler(this.button_SavePreview_Click);
            // 
            // button_Preview
            // 
            this.button_Preview.Location = new System.Drawing.Point(49, 158);
            this.button_Preview.Name = "button_Preview";
            this.button_Preview.Size = new System.Drawing.Size(61, 28);
            this.button_Preview.TabIndex = 10;
            this.button_Preview.Text = "预览";
            this.button_Preview.UseVisualStyleBackColor = true;
            this.button_Preview.Click += new System.EventHandler(this.button_Preview_Click);
            // 
            // button_AutoProcess
            // 
            this.button_AutoProcess.Location = new System.Drawing.Point(9, 222);
            this.button_AutoProcess.Name = "button_AutoProcess";
            this.button_AutoProcess.Size = new System.Drawing.Size(213, 31);
            this.button_AutoProcess.TabIndex = 7;
            this.button_AutoProcess.Text = "自动";
            this.button_AutoProcess.UseVisualStyleBackColor = true;
            this.button_AutoProcess.Visible = false;
            // 
            // textBox_FileName
            // 
            this.textBox_FileName.Location = new System.Drawing.Point(66, 259);
            this.textBox_FileName.Name = "textBox_FileName";
            this.textBox_FileName.ReadOnly = true;
            this.textBox_FileName.Size = new System.Drawing.Size(147, 21);
            this.textBox_FileName.TabIndex = 9;
            // 
            // label_FileName
            // 
            this.label_FileName.AutoSize = true;
            this.label_FileName.Location = new System.Drawing.Point(7, 262);
            this.label_FileName.Name = "label_FileName";
            this.label_FileName.Size = new System.Drawing.Size(53, 12);
            this.label_FileName.TabIndex = 8;
            this.label_FileName.Text = "文件名：";
            // 
            // label_max_b
            // 
            this.label_max_b.AutoSize = true;
            this.label_max_b.Location = new System.Drawing.Point(190, 105);
            this.label_max_b.Name = "label_max_b";
            this.label_max_b.Size = new System.Drawing.Size(23, 12);
            this.label_max_b.TabIndex = 7;
            this.label_max_b.Text = "255";
            // 
            // label_max_a
            // 
            this.label_max_a.AutoSize = true;
            this.label_max_a.Location = new System.Drawing.Point(170, 45);
            this.label_max_a.Name = "label_max_a";
            this.label_max_a.Size = new System.Drawing.Size(53, 12);
            this.label_max_a.TabIndex = 6;
            this.label_max_a.Text = "maxwhite";
            // 
            // label_min_b
            // 
            this.label_min_b.AutoSize = true;
            this.label_min_b.Location = new System.Drawing.Point(20, 105);
            this.label_min_b.Name = "label_min_b";
            this.label_min_b.Size = new System.Drawing.Size(11, 12);
            this.label_min_b.TabIndex = 5;
            this.label_min_b.Text = "0";
            // 
            // label_min_a
            // 
            this.label_min_a.AutoSize = true;
            this.label_min_a.Location = new System.Drawing.Point(6, 45);
            this.label_min_a.Name = "label_min_a";
            this.label_min_a.Size = new System.Drawing.Size(53, 12);
            this.label_min_a.TabIndex = 4;
            this.label_min_a.Text = "minblack";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(113, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "b";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(111, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "a";
            // 
            // trackBar_b
            // 
            this.trackBar_b.Location = new System.Drawing.Point(8, 120);
            this.trackBar_b.Maximum = 255;
            this.trackBar_b.Name = "trackBar_b";
            this.trackBar_b.Size = new System.Drawing.Size(216, 45);
            this.trackBar_b.TabIndex = 1;
            this.trackBar_b.Scroll += new System.EventHandler(this.trackBar_b_Scroll);
            // 
            // trackBar_a
            // 
            this.trackBar_a.Location = new System.Drawing.Point(9, 60);
            this.trackBar_a.Maximum = 255;
            this.trackBar_a.Name = "trackBar_a";
            this.trackBar_a.Size = new System.Drawing.Size(214, 45);
            this.trackBar_a.TabIndex = 0;
            this.trackBar_a.Scroll += new System.EventHandler(this.trackBar_a_Scroll);
            // 
            // button_PageUp
            // 
            this.button_PageUp.Location = new System.Drawing.Point(11, 494);
            this.button_PageUp.Name = "button_PageUp";
            this.button_PageUp.Size = new System.Drawing.Size(79, 44);
            this.button_PageUp.TabIndex = 5;
            this.button_PageUp.Text = "上一张";
            this.button_PageUp.UseVisualStyleBackColor = true;
            this.button_PageUp.Click += new System.EventHandler(this.button_PageUp_Click);
            // 
            // button_PageDown
            // 
            this.button_PageDown.Location = new System.Drawing.Point(96, 494);
            this.button_PageDown.Name = "button_PageDown";
            this.button_PageDown.Size = new System.Drawing.Size(86, 44);
            this.button_PageDown.TabIndex = 6;
            this.button_PageDown.Text = "下一张";
            this.button_PageDown.UseVisualStyleBackColor = true;
            this.button_PageDown.Click += new System.EventHandler(this.button_PageDown_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // hWindowControl
            // 
            this.hWindowControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hWindowControl.AutoScroll = true;
            this.hWindowControl.BackColor = System.Drawing.Color.Green;
            this.hWindowControl.BorderColor = System.Drawing.Color.Green;
            this.hWindowControl.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl.Location = new System.Drawing.Point(0, 0);
            this.hWindowControl.Name = "hWindowControl";
            this.hWindowControl.Size = new System.Drawing.Size(864, 747);
            this.hWindowControl.TabIndex = 0;
            this.hWindowControl.WindowSize = new System.Drawing.Size(864, 747);
            this.hWindowControl.HMouseMove += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseMove);
            this.hWindowControl.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseDown);
            this.hWindowControl.HMouseUp += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseUp);
            this.hWindowControl.HMouseWheel += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseWheel);
            this.hWindowControl.SizeChanged += new System.EventHandler(this.hWindowControl_SizeChanged);
            this.hWindowControl.MouseLeave += new System.EventHandler(this.hWindowControl_MouseLeave);
            // 
            // label_modelNum
            // 
            this.label_modelNum.AutoSize = true;
            this.label_modelNum.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_modelNum.Location = new System.Drawing.Point(188, 503);
            this.label_modelNum.Name = "label_modelNum";
            this.label_modelNum.Size = new System.Drawing.Size(20, 20);
            this.label_modelNum.TabIndex = 7;
            this.label_modelNum.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(7, 456);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(170, 22);
            this.label3.TabIndex = 8;
            this.label3.Text = "提醒：换页前注意保存";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.hWindowControl);
            this.panel1.Location = new System.Drawing.Point(248, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(864, 747);
            this.panel1.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_Extract);
            this.groupBox1.Controls.Add(this.KHlength);
            this.groupBox1.Controls.Add(this.label_KHlength);
            this.groupBox1.Controls.Add(this.textBox_Fodler);
            this.groupBox1.Controls.Add(this.button_OpenFodler);
            this.groupBox1.Location = new System.Drawing.Point(12, 592);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 112);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "提取判错考生题号";
            // 
            // button_Extract
            // 
            this.button_Extract.Location = new System.Drawing.Point(149, 56);
            this.button_Extract.Name = "button_Extract";
            this.button_Extract.Size = new System.Drawing.Size(75, 50);
            this.button_Extract.TabIndex = 14;
            this.button_Extract.Text = "提取";
            this.button_Extract.UseVisualStyleBackColor = true;
            this.button_Extract.Click += new System.EventHandler(this.button_Extract_Click);
            // 
            // KHlength
            // 
            this.KHlength.Location = new System.Drawing.Point(84, 85);
            this.KHlength.Name = "KHlength";
            this.KHlength.Size = new System.Drawing.Size(43, 21);
            this.KHlength.TabIndex = 13;
            // 
            // label_KHlength
            // 
            this.label_KHlength.AutoSize = true;
            this.label_KHlength.Location = new System.Drawing.Point(8, 87);
            this.label_KHlength.Name = "label_KHlength";
            this.label_KHlength.Size = new System.Drawing.Size(77, 12);
            this.label_KHlength.TabIndex = 12;
            this.label_KHlength.Text = "考生号位数：";
            // 
            // textBox_Fodler
            // 
            this.textBox_Fodler.Location = new System.Drawing.Point(9, 22);
            this.textBox_Fodler.Name = "textBox_Fodler";
            this.textBox_Fodler.Size = new System.Drawing.Size(213, 21);
            this.textBox_Fodler.TabIndex = 10;
            // 
            // button_OpenFodler
            // 
            this.button_OpenFodler.Location = new System.Drawing.Point(8, 49);
            this.button_OpenFodler.Name = "button_OpenFodler";
            this.button_OpenFodler.Size = new System.Drawing.Size(77, 23);
            this.button_OpenFodler.TabIndex = 0;
            this.button_OpenFodler.Text = "打开文件夹";
            this.button_OpenFodler.UseVisualStyleBackColor = true;
            this.button_OpenFodler.Click += new System.EventHandler(this.button_OpenFodler_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 785);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label_modelNum);
            this.Controls.Add(this.button_PageDown);
            this.Controls.Add(this.button_PageUp);
            this.Controls.Add(this.groupBox_parameter);
            this.Controls.Add(this.groupBox_toolBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "模板预处理工具";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox_toolBox.ResumeLayout(false);
            this.groupBox_toolBox.PerformLayout();
            this.groupBox_parameter.ResumeLayout(false);
            this.groupBox_parameter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_b)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_a)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KHlength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Open;
        private System.Windows.Forms.ToolStripMenuItem TSMI_OpenFile;
        private System.Windows.Forms.ToolStripMenuItem TSMI_Quit;
        private System.Windows.Forms.GroupBox groupBox_toolBox;
        private System.Windows.Forms.Button button_Undo;
        private System.Windows.Forms.GroupBox groupBox_parameter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBar_b;
        private System.Windows.Forms.TrackBar trackBar_a;
        private System.Windows.Forms.Button button_PageUp;
        private System.Windows.Forms.Button button_PageDown;
        private System.Windows.Forms.Label label_max_b;
        private System.Windows.Forms.Label label_max_a;
        private System.Windows.Forms.Label label_min_b;
        private System.Windows.Forms.Label label_min_a;
        private System.Windows.Forms.TextBox textBox_FileName;
        private System.Windows.Forms.Label label_FileName;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button_AutoProcess;
        private HalconDotNet.HWindowControl hWindowControl;
        private System.Windows.Forms.Label label_modelNum;
        private System.Windows.Forms.Button button_Preview;
        private System.Windows.Forms.Button button_SavePreview;
        private System.Windows.Forms.RadioButton radio_eraser;
        private System.Windows.Forms.RadioButton radio_brush;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radio_select;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_Extract;
        private System.Windows.Forms.NumericUpDown KHlength;
        private System.Windows.Forms.Label label_KHlength;
        private System.Windows.Forms.TextBox textBox_Fodler;
        private System.Windows.Forms.Button button_OpenFodler;
    }
}


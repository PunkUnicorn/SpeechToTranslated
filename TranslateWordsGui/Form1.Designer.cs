namespace TranslateWordsGui
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            flowLayoutPanel7 = new FlowLayoutPanel();
            previewLabel = new Label();
            flowLayoutPanel8 = new FlowLayoutPanel();
            modelLabel = new Label();
            flowLayoutPanel2 = new FlowLayoutPanel();
            controlsButton1 = new Button();
            staticLabel4 = new Label();
            numericUpDown1 = new NumericUpDown();
            label1b = new Label();
            label2b = new Label();
            label3b = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            flowLayoutPanel7.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = SystemColors.ControlText;
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            splitContainer1.Panel1.Controls.Add(modelLabel);
            splitContainer1.Panel1.ForeColor = SystemColors.ControlDark;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(flowLayoutPanel2);
            splitContainer1.Panel2MinSize = 0;
            splitContainer1.Size = new Size(800, 450);
            splitContainer1.SplitterDistance = 421;
            splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(flowLayoutPanel7);
            splitContainer2.Panel1MinSize = 0;
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(flowLayoutPanel8);
            splitContainer2.Size = new Size(800, 421);
            splitContainer2.SplitterDistance = 25;
            splitContainer2.TabIndex = 0;
            // 
            // flowLayoutPanel7
            // 
            flowLayoutPanel7.AutoSize = true;
            flowLayoutPanel7.Controls.Add(previewLabel);
            flowLayoutPanel7.Dock = DockStyle.Bottom;
            flowLayoutPanel7.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            flowLayoutPanel7.Location = new Point(0, 5);
            flowLayoutPanel7.Name = "flowLayoutPanel7";
            flowLayoutPanel7.Size = new Size(800, 20);
            flowLayoutPanel7.TabIndex = 9;
            // 
            // previewLabel
            // 
            previewLabel.AutoSize = true;
            previewLabel.Dock = DockStyle.Top;
            previewLabel.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            previewLabel.ForeColor = SystemColors.ControlDarkDark;
            previewLabel.Location = new Point(3, 0);
            previewLabel.Name = "previewLabel";
            previewLabel.Size = new Size(0, 20);
            previewLabel.TabIndex = 3;
            // 
            // flowLayoutPanel8
            // 
            flowLayoutPanel8.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel8.AutoScroll = true;
            flowLayoutPanel8.Location = new Point(0, -1);
            flowLayoutPanel8.Name = "flowLayoutPanel8";
            flowLayoutPanel8.Size = new Size(800, 394);
            flowLayoutPanel8.TabIndex = 10;
            // 
            // modelLabel
            // 
            modelLabel.AutoSize = true;
            modelLabel.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            modelLabel.Location = new Point(6, 37);
            modelLabel.Name = "modelLabel";
            modelLabel.Size = new Size(88, 20);
            modelLabel.TabIndex = 11;
            modelLabel.Text = "modelLabel";
            modelLabel.Visible = false;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(controlsButton1);
            flowLayoutPanel2.Controls.Add(staticLabel4);
            flowLayoutPanel2.Controls.Add(numericUpDown1);
            flowLayoutPanel2.Controls.Add(label1b);
            flowLayoutPanel2.Controls.Add(label2b);
            flowLayoutPanel2.Controls.Add(label3b);
            flowLayoutPanel2.Dock = DockStyle.Fill;
            flowLayoutPanel2.Location = new Point(0, 0);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(800, 25);
            flowLayoutPanel2.TabIndex = 1;
            // 
            // controlsButton1
            // 
            controlsButton1.AutoSize = true;
            controlsButton1.Location = new Point(3, 1);
            controlsButton1.Margin = new Padding(3, 1, 3, 3);
            controlsButton1.Name = "controlsButton1";
            controlsButton1.Size = new Size(47, 25);
            controlsButton1.TabIndex = 4;
            controlsButton1.Text = "Close";
            controlsButton1.UseVisualStyleBackColor = true;
            // 
            // staticLabel4
            // 
            staticLabel4.AutoSize = true;
            staticLabel4.Location = new Point(56, 5);
            staticLabel4.Margin = new Padding(3, 5, 3, 0);
            staticLabel4.Name = "staticLabel4";
            staticLabel4.Size = new Size(57, 15);
            staticLabel4.TabIndex = 3;
            staticLabel4.Text = "Font Size:";
            // 
            // numericUpDown1
            // 
            numericUpDown1.AutoSize = true;
            numericUpDown1.Location = new Point(119, 3);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(41, 23);
            numericUpDown1.TabIndex = 2;
            // 
            // label1b
            // 
            label1b.AutoSize = true;
            label1b.Location = new Point(166, 0);
            label1b.Name = "label1b";
            label1b.Size = new Size(0, 15);
            label1b.TabIndex = 0;
            // 
            // label2b
            // 
            label2b.AutoSize = true;
            label2b.Location = new Point(172, 0);
            label2b.Name = "label2b";
            label2b.Size = new Size(0, 15);
            label2b.TabIndex = 1;
            // 
            // label3b
            // 
            label3b.AutoSize = true;
            label3b.Location = new Point(178, 0);
            label3b.Name = "label3b";
            label3b.Size = new Size(0, 15);
            label3b.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            ControlBox = false;
            Controls.Add(splitContainer1);
            DoubleBuffered = true;
            Name = "Form1";
            ShowIcon = false;
            Load += Form1_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel1.PerformLayout();
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            flowLayoutPanel7.ResumeLayout(false);
            flowLayoutPanel7.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private SplitContainer splitContainer1;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label label1b;
        private Label label2b;
        private Label label3b;
        private NumericUpDown numericUpDown1;
        private Label staticLabel4;
        private Button controlsButton1;
        private Label previewLabel;
        private FlowLayoutPanel flowLayoutPanel7;
        private FlowLayoutPanel flowLayoutPanel8;
        private Label modelLabel;
        private SplitContainer splitContainer2;
    }
}
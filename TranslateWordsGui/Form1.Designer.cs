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
            flowLayoutPanel1 = new FlowLayoutPanel();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            splitContainer1 = new SplitContainer();
            flowLayoutPanel2 = new FlowLayoutPanel();
            controlsButton1 = new Button();
            staticLabel4 = new Label();
            numericUpDown1 = new NumericUpDown();
            label1b = new Label();
            label2b = new Label();
            label3b = new Label();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(label2);
            flowLayoutPanel1.Controls.Add(label3);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(800, 421);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(0, 21);
            label1.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(9, 0);
            label2.Name = "label2";
            label2.Size = new Size(0, 21);
            label2.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(15, 0);
            label3.Name = "label3";
            label3.Size = new Size(0, 21);
            label3.TabIndex = 2;
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
            splitContainer1.Panel1.Controls.Add(flowLayoutPanel1);
            splitContainer1.Panel1.ForeColor = SystemColors.Control;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(flowLayoutPanel2);
            splitContainer1.Panel2MinSize = 0;
            splitContainer1.Size = new Size(800, 450);
            splitContainer1.SplitterDistance = 421;
            splitContainer1.TabIndex = 1;
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
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Label label1;
        private SplitContainer splitContainer1;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label label1b;
        private Label label2;
        private Label label2b;
        private Label label3;
        private Label label3b;
        private NumericUpDown numericUpDown1;
        private Label staticLabel4;
        private Button controlsButton1;
    }
}
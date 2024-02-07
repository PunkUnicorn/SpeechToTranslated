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
            translationFlowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel7 = new FlowLayoutPanel();
            previewLabel = new Label();
            modelLabel = new Label();
            flowLayoutPanel2 = new FlowLayoutPanel();
            controlsButton1 = new Button();
            checkBox1 = new CheckBox();
            label1 = new Label();
            previewNumericUpDown2 = new NumericUpDown();
            staticLabel4 = new Label();
            errorLabel = new Label();
            numericUpDown1 = new NumericUpDown();
            debugPreviewLabel = new Label();
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
            ((System.ComponentModel.ISupportInitialize)previewNumericUpDown2).BeginInit();
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
            splitContainer2.Panel1.Controls.Add(translationFlowLayoutPanel);
            splitContainer2.Panel1MinSize = 0;
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(flowLayoutPanel7);
            splitContainer2.Size = new Size(800, 421);
            splitContainer2.SplitterDistance = 392;
            splitContainer2.TabIndex = 0;
            // 
            // translationFlowLayoutPanel
            // 
            translationFlowLayoutPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            translationFlowLayoutPanel.AutoScroll = true;
            translationFlowLayoutPanel.Location = new Point(0, 0);
            translationFlowLayoutPanel.Name = "translationFlowLayoutPanel";
            translationFlowLayoutPanel.Size = new Size(800, 395);
            translationFlowLayoutPanel.TabIndex = 10;
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
            flowLayoutPanel2.Controls.Add(checkBox1);
            flowLayoutPanel2.Controls.Add(label1);
            flowLayoutPanel2.Controls.Add(previewNumericUpDown2);
            flowLayoutPanel2.Controls.Add(staticLabel4);
            flowLayoutPanel2.Controls.Add(numericUpDown1);
            flowLayoutPanel2.Controls.Add(errorLabel);
            flowLayoutPanel2.Controls.Add(debugPreviewLabel);
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
            controlsButton1.Size = new Size(110, 25);
            controlsButton1.TabIndex = 4;
            controlsButton1.Text = "Forced Shutdown";
            controlsButton1.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = CheckState.Checked;
            checkBox1.Location = new Point(119, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(101, 19);
            checkBox1.TabIndex = 5;
            checkBox1.Text = "Multicoloured";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(226, 5);
            label1.Margin = new Padding(3, 5, 3, 0);
            label1.Name = "label1";
            label1.Size = new Size(101, 15);
            label1.TabIndex = 3;
            label1.Text = "Preview Font Size:";
            // 
            // previewNumericUpDown2
            // 
            previewNumericUpDown2.AutoSize = true;
            previewNumericUpDown2.Location = new Point(333, 3);
            previewNumericUpDown2.Name = "previewNumericUpDown2";
            previewNumericUpDown2.Size = new Size(41, 23);
            previewNumericUpDown2.TabIndex = 2;
            // 
            // staticLabel4
            // 
            staticLabel4.AutoSize = true;
            staticLabel4.Location = new Point(380, 5);
            staticLabel4.Margin = new Padding(3, 5, 3, 0);
            staticLabel4.Name = "staticLabel4";
            staticLabel4.Size = new Size(117, 15);
            staticLabel4.TabIndex = 3;
            staticLabel4.Text = "Translation Font Size:";
            // 
            // errorLabel
            // 
            errorLabel.AutoSize = true;
            errorLabel.Location = new Point(550, 0);
            errorLabel.Name = "errorLabel";
            errorLabel.Size = new Size(0, 15);
            errorLabel.TabIndex = 1;
            // 
            // numericUpDown1
            // 
            numericUpDown1.AutoSize = true;
            numericUpDown1.Location = new Point(503, 3);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(41, 23);
            numericUpDown1.TabIndex = 2;
            // 
            // debugPreviewLabel
            // 
            debugPreviewLabel.AutoSize = true;
            debugPreviewLabel.Location = new Point(556, 0);
            debugPreviewLabel.Name = "debugPreviewLabel";
            debugPreviewLabel.Size = new Size(0, 15);
            debugPreviewLabel.TabIndex = 0;
            // 
            // label3b
            // 
            label3b.AutoSize = true;
            label3b.Location = new Point(562, 0);
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
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            flowLayoutPanel7.ResumeLayout(false);
            flowLayoutPanel7.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)previewNumericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private SplitContainer splitContainer1;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label debugPreviewLabel;
        private Label errorLabel;
        private Label label3b;
        private NumericUpDown numericUpDown1;
        private Label staticLabel4;
        private Button controlsButton1;
        private Label previewLabel;
        private FlowLayoutPanel flowLayoutPanel7;
        private FlowLayoutPanel translationFlowLayoutPanel;
        private Label modelLabel;
        private SplitContainer splitContainer2;
        private CheckBox checkBox1;
        private Label label1;
        private NumericUpDown previewNumericUpDown2;
    }
}
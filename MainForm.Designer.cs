
namespace printerAPI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.labelConnectionServer = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.labelName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelPrinterName = new System.Windows.Forms.Label();
            this.labelPrinterNamec = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Connection to Server";
            // 
            // labelConnectionServer
            // 
            this.labelConnectionServer.AutoSize = true;
            this.labelConnectionServer.Location = new System.Drawing.Point(142, 44);
            this.labelConnectionServer.Name = "labelConnectionServer";
            this.labelConnectionServer.Size = new System.Drawing.Size(67, 13);
            this.labelConnectionServer.TabIndex = 3;
            this.labelConnectionServer.Text = ": Connecting";
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::printerAPI.Properties.Resources.setting_black;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Location = new System.Drawing.Point(396, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 23);
            this.button1.TabIndex = 7;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(142, 27);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 9;
            this.labelName.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Name";
            // 
            // labelPrinterName
            // 
            this.labelPrinterName.AutoSize = true;
            this.labelPrinterName.Location = new System.Drawing.Point(142, 61);
            this.labelPrinterName.Name = "labelPrinterName";
            this.labelPrinterName.Size = new System.Drawing.Size(35, 13);
            this.labelPrinterName.TabIndex = 11;
            this.labelPrinterName.Text = "label2";
            // 
            // labelPrinterNamec
            // 
            this.labelPrinterNamec.AutoSize = true;
            this.labelPrinterNamec.Location = new System.Drawing.Point(12, 61);
            this.labelPrinterNamec.Name = "labelPrinterNamec";
            this.labelPrinterNamec.Size = new System.Drawing.Size(68, 13);
            this.labelPrinterNamec.TabIndex = 10;
            this.labelPrinterNamec.Text = "Printer Name";
            // 
            // labelStatus
            // 
            this.labelStatus.Location = new System.Drawing.Point(15, 106);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(410, 272);
            this.labelStatus.TabIndex = 12;
            this.labelStatus.Text = "";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(262, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 80);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 387);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelPrinterName);
            this.Controls.Add(this.labelPrinterNamec);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelConnectionServer);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MainForm";
            this.Text = "Printer Status";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelConnectionServer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelPrinterName;
        private System.Windows.Forms.Label labelPrinterNamec;
        public System.Windows.Forms.RichTextBox labelStatus;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}


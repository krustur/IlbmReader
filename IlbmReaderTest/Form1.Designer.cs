namespace IlbmReaderTest
{
    partial class Form1
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
            this.buttonLoadIff = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonLoadIff
            // 
            this.buttonLoadIff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoadIff.Location = new System.Drawing.Point(608, 426);
            this.buttonLoadIff.Name = "buttonLoadIff";
            this.buttonLoadIff.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadIff.TabIndex = 0;
            this.buttonLoadIff.Text = "Load iff/lbm";
            this.buttonLoadIff.UseVisualStyleBackColor = true;
            this.buttonLoadIff.Click += new System.EventHandler(this.buttonLoadIlbm_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 461);
            this.Controls.Add(this.buttonLoadIff);
            this.Name = "Form1";
            this.Text = "Form1";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonLoadIff;
    }
}


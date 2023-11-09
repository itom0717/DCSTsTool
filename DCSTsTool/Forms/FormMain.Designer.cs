namespace DCSTsTool.Forms
{
    partial class FormMain
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
            this.TextWriteButton = new System.Windows.Forms.Button();
            this.GetOrgTextButton = new System.Windows.Forms.Button();
            this.GetTranslatTextButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextWriteButton
            // 
            this.TextWriteButton.Location = new System.Drawing.Point(12, 203);
            this.TextWriteButton.Name = "TextWriteButton";
            this.TextWriteButton.Size = new System.Drawing.Size(132, 68);
            this.TextWriteButton.TabIndex = 0;
            this.TextWriteButton.Text = "テキスト書込み";
            this.TextWriteButton.UseVisualStyleBackColor = true;
            this.TextWriteButton.Click += new System.EventHandler(this.TextWriteButton_Click);
            // 
            // GetOrgTextButton
            // 
            this.GetOrgTextButton.Location = new System.Drawing.Point(12, 12);
            this.GetOrgTextButton.Name = "GetOrgTextButton";
            this.GetOrgTextButton.Size = new System.Drawing.Size(132, 70);
            this.GetOrgTextButton.TabIndex = 1;
            this.GetOrgTextButton.Text = "原文テキスト取得";
            this.GetOrgTextButton.UseVisualStyleBackColor = true;
            this.GetOrgTextButton.Click += new System.EventHandler(this.GetOrgTextButton_Click);
            // 
            // GetTranslatTextButton
            // 
            this.GetTranslatTextButton.Location = new System.Drawing.Point(12, 99);
            this.GetTranslatTextButton.Name = "GetTranslatTextButton";
            this.GetTranslatTextButton.Size = new System.Drawing.Size(132, 70);
            this.GetTranslatTextButton.TabIndex = 2;
            this.GetTranslatTextButton.Text = "翻訳テキスト取得";
            this.GetTranslatTextButton.UseVisualStyleBackColor = true;
            this.GetTranslatTextButton.Click += new System.EventHandler(this.GetTranslatTextButton_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 491);
            this.Controls.Add(this.GetTranslatTextButton);
            this.Controls.Add(this.GetOrgTextButton);
            this.Controls.Add(this.TextWriteButton);
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button TextWriteButton;
        private System.Windows.Forms.Button GetOrgTextButton;
        private System.Windows.Forms.Button GetTranslatTextButton;
    }
}
namespace UAAVDataApp
{
    partial class MainForm
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
            this.btnLoadXML = new System.Windows.Forms.Button();
            this.btnNovice = new System.Windows.Forms.Button();
            this.btnExpert = new System.Windows.Forms.Button();
            this.btnAdmin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLoadXML
            // 
            this.btnLoadXML.Location = new System.Drawing.Point(65, 40);
            this.btnLoadXML.Name = "btnLoadXML";
            this.btnLoadXML.Size = new System.Drawing.Size(100, 30);
            this.btnLoadXML.TabIndex = 1;
            this.btnLoadXML.Text = "Load XML";
            this.btnLoadXML.UseVisualStyleBackColor = true;
            this.btnLoadXML.Click += new System.EventHandler(this.btnLoadXML_Click);
            // 
            // btnNovice
            // 
            this.btnNovice.Location = new System.Drawing.Point(65, 107);
            this.btnNovice.Name = "btnNovice";
            this.btnNovice.Size = new System.Drawing.Size(100, 30);
            this.btnNovice.TabIndex = 2;
            this.btnNovice.Text = "Novice User";
            this.btnNovice.UseVisualStyleBackColor = true;
            this.btnNovice.Click += new System.EventHandler(this.btnNoviceUser_Click);
            // 
            // btnExpert
            // 
            this.btnExpert.Location = new System.Drawing.Point(65, 172);
            this.btnExpert.Name = "btnExpert";
            this.btnExpert.Size = new System.Drawing.Size(100, 30);
            this.btnExpert.TabIndex = 3;
            this.btnExpert.Text = "Expert User";
            this.btnExpert.UseVisualStyleBackColor = true;
            this.btnExpert.Click += new System.EventHandler(this.btnExpertUser_Click);
            // 
            // btnAdmin
            // 
            this.btnAdmin.Location = new System.Drawing.Point(65, 244);
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Size = new System.Drawing.Size(100, 30);
            this.btnAdmin.TabIndex = 4;
            this.btnAdmin.Text = "Admin";
            this.btnAdmin.UseVisualStyleBackColor = true;
            this.btnAdmin.Click += new System.EventHandler(this.btnAdmin_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 311);
            this.Controls.Add(this.btnAdmin);
            this.Controls.Add(this.btnExpert);
            this.Controls.Add(this.btnNovice);
            this.Controls.Add(this.btnLoadXML);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnLoadXML;
        private System.Windows.Forms.Button btnNovice;
        private System.Windows.Forms.Button btnExpert;
        private System.Windows.Forms.Button btnAdmin;
    }
}


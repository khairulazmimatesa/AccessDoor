namespace AccessDoorWinforms.Views
{
    partial class MainPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPage));
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnLogIn = new System.Windows.Forms.Button();
            this.logInPageUC1 = new AccessDoorWinforms.UserControls.LogInPageUC();
            this.LoadingImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingImage)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(380, 20);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(90, 40);
            this.btnLogout.TabIndex = 4;
            this.btnLogout.Text = "Log Out";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.BtnLogout_Click);
            // 
            // btnLogIn
            // 
            this.btnLogIn.Location = new System.Drawing.Point(198, 548);
            this.btnLogIn.Margin = new System.Windows.Forms.Padding(4);
            this.btnLogIn.Name = "btnLogIn";
            this.btnLogIn.Size = new System.Drawing.Size(100, 30);
            this.btnLogIn.TabIndex = 9;
            this.btnLogIn.Text = "Login";
            this.btnLogIn.UseVisualStyleBackColor = true;
            this.btnLogIn.Click += new System.EventHandler(this.BtnLogIn_Click);
            // 
            // logInPageUC1
            // 
            this.logInPageUC1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(198)))), ((int)(((byte)(164)))));
            this.logInPageUC1.FullUrl = null;
            this.logInPageUC1.Key = null;
            this.logInPageUC1.Location = new System.Drawing.Point(-9, -1);
            this.logInPageUC1.Name = "logInPageUC1";
            this.logInPageUC1.Size = new System.Drawing.Size(500, 670);
            this.logInPageUC1.TabIndex = 5;
            this.logInPageUC1.Token = null;
            this.logInPageUC1.Urltxt = null;
            this.logInPageUC1.Username = null;
            // 
            // LoadingImage
            // 
            this.LoadingImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadingImage.BackColor = System.Drawing.Color.White;
            this.LoadingImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.LoadingImage.Image = ((System.Drawing.Image)(resources.GetObject("LoadingImage.Image")));
            this.LoadingImage.Location = new System.Drawing.Point(-9, -1);
            this.LoadingImage.Name = "LoadingImage";
            this.LoadingImage.Size = new System.Drawing.Size(500, 670);
            this.LoadingImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.LoadingImage.TabIndex = 10;
            this.LoadingImage.TabStop = false;
            // 
            // MainPage
            // 
            this.AcceptButton = this.btnLogIn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(484, 661);
            this.Controls.Add(this.btnLogIn);
            this.Controls.Add(this.logInPageUC1);
            this.Controls.Add(this.LoadingImage);
            this.Controls.Add(this.btnLogout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainPage";
            this.Text = "AccessDoor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainPage_FormClosing);
            this.Load += new System.EventHandler(this.MainPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LoadingImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnLogout;
        private UserControls.LogInPageUC logInPageUC1;
        private System.Windows.Forms.Button btnLogIn;
        private System.Windows.Forms.PictureBox LoadingImage;
    }
}

namespace d2r_pwe_client
{
    partial class d2rpwe
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
            this.lbToken = new System.Windows.Forms.Label();
            this.tbToken = new System.Windows.Forms.TextBox();
            this.btnVerify = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.tbGame = new System.Windows.Forms.TextBox();
            this.lbGame = new System.Windows.Forms.Label();
            this.tbSec = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbToken
            // 
            this.lbToken.AutoSize = true;
            this.lbToken.Location = new System.Drawing.Point(11, 17);
            this.lbToken.Name = "lbToken";
            this.lbToken.Size = new System.Drawing.Size(38, 13);
            this.lbToken.TabIndex = 0;
            this.lbToken.Text = "Token";
            // 
            // tbToken
            // 
            this.tbToken.Location = new System.Drawing.Point(62, 14);
            this.tbToken.Name = "tbToken";
            this.tbToken.Size = new System.Drawing.Size(143, 20);
            this.tbToken.TabIndex = 1;
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(211, 12);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(52, 23);
            this.btnVerify.TabIndex = 2;
            this.btnVerify.Text = "Verify";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(211, 41);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(52, 23);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tbGame
            // 
            this.tbGame.Location = new System.Drawing.Point(62, 43);
            this.tbGame.Name = "tbGame";
            this.tbGame.ReadOnly = true;
            this.tbGame.Size = new System.Drawing.Size(99, 20);
            this.tbGame.TabIndex = 4;
            this.tbGame.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbGame
            // 
            this.lbGame.AutoSize = true;
            this.lbGame.Location = new System.Drawing.Point(11, 46);
            this.lbGame.Name = "lbGame";
            this.lbGame.Size = new System.Drawing.Size(35, 13);
            this.lbGame.TabIndex = 3;
            this.lbGame.Text = "Game";
            // 
            // tbSec
            // 
            this.tbSec.Location = new System.Drawing.Point(163, 43);
            this.tbSec.Name = "tbSec";
            this.tbSec.ReadOnly = true;
            this.tbSec.Size = new System.Drawing.Size(42, 20);
            this.tbSec.TabIndex = 6;
            this.tbSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // d2rpwe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 73);
            this.Controls.Add(this.tbSec);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.tbGame);
            this.Controls.Add(this.lbGame);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.tbToken);
            this.Controls.Add(this.lbToken);
            this.Name = "d2rpwe";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Lastemperor\'s PWE";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbToken;
        private System.Windows.Forms.TextBox tbToken;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox tbGame;
        private System.Windows.Forms.Label lbGame;
        private System.Windows.Forms.TextBox tbSec;
    }
}


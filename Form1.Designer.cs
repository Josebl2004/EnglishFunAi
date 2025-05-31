namespace EnglishFunAI
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
            this.btnChat = new System.Windows.Forms.Button();
            this.btnTranslation = new System.Windows.Forms.Button();
            this.btnGames = new System.Windows.Forms.Button();
            this.btnHistory = new System.Windows.Forms.Button();
            this.btnVocabulary = new System.Windows.Forms.Button();
            this.lblBienvenida = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblBienvenida
            // 
            this.lblBienvenida.Text = "¡Bienvenido a EnglishFun AI!";
            this.lblBienvenida.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold);
            this.lblBienvenida.ForeColor = System.Drawing.Color.MediumVioletRed;
            this.lblBienvenida.Location = new System.Drawing.Point(10, 10);
            this.lblBienvenida.Size = new System.Drawing.Size(270, 40);
            this.lblBienvenida.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnChat
            // 
            this.btnChat.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Bold);
            this.btnChat.Location = new System.Drawing.Point(40, 60);
            this.btnChat.Name = "btnChat";
            this.btnChat.Size = new System.Drawing.Size(200, 50);
            this.btnChat.TabIndex = 0;
            this.btnChat.Text = "Chat IA";
            this.btnChat.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnChat.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnChat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChat.FlatAppearance.BorderSize = 0;
            this.btnChat.UseVisualStyleBackColor = false;
            this.btnChat.Click += new System.EventHandler(this.btnChat_Click);
            // 
            // btnTranslation
            // 
            this.btnTranslation.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Bold);
            this.btnTranslation.Location = new System.Drawing.Point(40, 120);
            this.btnTranslation.Name = "btnTranslation";
            this.btnTranslation.Size = new System.Drawing.Size(200, 50);
            this.btnTranslation.TabIndex = 1;
            this.btnTranslation.Text = "Traducción";
            this.btnTranslation.BackColor = System.Drawing.Color.LightGreen;
            this.btnTranslation.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnTranslation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTranslation.FlatAppearance.BorderSize = 0;
            this.btnTranslation.UseVisualStyleBackColor = false;
            this.btnTranslation.Click += new System.EventHandler(this.btnTranslation_Click);
            // 
            // btnGames
            // 
            this.btnGames.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Bold);
            this.btnGames.Location = new System.Drawing.Point(40, 180);
            this.btnGames.Name = "btnGames";
            this.btnGames.Size = new System.Drawing.Size(200, 50);
            this.btnGames.TabIndex = 2;
            this.btnGames.Text = "Juegos";
            this.btnGames.BackColor = System.Drawing.Color.LightYellow;
            this.btnGames.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnGames.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGames.FlatAppearance.BorderSize = 0;
            this.btnGames.UseVisualStyleBackColor = false;
            this.btnGames.Click += new System.EventHandler(this.btnGames_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Bold);
            this.btnHistory.Location = new System.Drawing.Point(40, 240);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(200, 50);
            this.btnHistory.TabIndex = 3;
            this.btnHistory.Text = "Historial";
            this.btnHistory.BackColor = System.Drawing.Color.LightPink;
            this.btnHistory.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistory.FlatAppearance.BorderSize = 0;
            this.btnHistory.UseVisualStyleBackColor = false;
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // btnVocabulary
            // 
            this.btnVocabulary.Font = new System.Drawing.Font("Comic Sans MS", 14F, System.Drawing.FontStyle.Bold);
            this.btnVocabulary.Location = new System.Drawing.Point(40, 300);
            this.btnVocabulary.Name = "btnVocabulary";
            this.btnVocabulary.Size = new System.Drawing.Size(200, 50);
            this.btnVocabulary.TabIndex = 4;
            this.btnVocabulary.Text = "Vocabulario";
            this.btnVocabulary.BackColor = System.Drawing.Color.Plum;
            this.btnVocabulary.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.btnVocabulary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVocabulary.FlatAppearance.BorderSize = 0;
            this.btnVocabulary.UseVisualStyleBackColor = false;
            this.btnVocabulary.Click += new System.EventHandler(this.btnVocabulary_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(255, 230, 245, 255);
            this.ClientSize = new System.Drawing.Size(284, 380);
            this.Controls.Add(this.lblBienvenida);
            this.Controls.Add(this.btnVocabulary);
            this.Controls.Add(this.btnHistory);
            this.Controls.Add(this.btnGames);
            this.Controls.Add(this.btnTranslation);
            this.Controls.Add(this.btnChat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EnglishFun AI";
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnChat;
        private System.Windows.Forms.Button btnTranslation;
        private System.Windows.Forms.Button btnGames;
        private System.Windows.Forms.Button btnHistory;
        private System.Windows.Forms.Button btnVocabulary;
        private System.Windows.Forms.Label lblBienvenida;
    }
}

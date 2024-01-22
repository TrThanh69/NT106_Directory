namespace SignInLogIn
{
    partial class Text_translator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Text_translator));
            this.lang2 = new System.Windows.Forms.ComboBox();
            this.lang1 = new System.Windows.Forms.ComboBox();
            this.translate = new System.Windows.Forms.PictureBox();
            this.speech2text = new System.Windows.Forms.PictureBox();
            this.output = new System.Windows.Forms.RichTextBox();
            this.input = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.translate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speech2text)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lang2
            // 
            this.lang2.Font = new System.Drawing.Font("Cambria", 10F);
            this.lang2.FormattingEnabled = true;
            this.lang2.Items.AddRange(new object[] {
            "English",
            "Vietnamese"});
            this.lang2.Location = new System.Drawing.Point(1157, 114);
            this.lang2.Name = "lang2";
            this.lang2.Size = new System.Drawing.Size(231, 28);
            this.lang2.TabIndex = 80;
            // 
            // lang1
            // 
            this.lang1.Font = new System.Drawing.Font("Cambria", 10F);
            this.lang1.FormattingEnabled = true;
            this.lang1.Items.AddRange(new object[] {
            "English",
            "Vietnamese",
            "Detected language"});
            this.lang1.Location = new System.Drawing.Point(591, 114);
            this.lang1.Name = "lang1";
            this.lang1.Size = new System.Drawing.Size(231, 28);
            this.lang1.TabIndex = 79;
            // 
            // translate
            // 
            this.translate.Image = ((System.Drawing.Image)(resources.GetObject("translate.Image")));
            this.translate.Location = new System.Drawing.Point(799, 510);
            this.translate.Name = "translate";
            this.translate.Size = new System.Drawing.Size(80, 56);
            this.translate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.translate.TabIndex = 78;
            this.translate.TabStop = false;
            this.translate.Click += new System.EventHandler(this.translate_Click);
            // 
            // speech2text
            // 
            this.speech2text.Image = ((System.Drawing.Image)(resources.GetObject("speech2text.Image")));
            this.speech2text.Location = new System.Drawing.Point(799, 210);
            this.speech2text.Name = "speech2text";
            this.speech2text.Size = new System.Drawing.Size(82, 74);
            this.speech2text.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.speech2text.TabIndex = 77;
            this.speech2text.TabStop = false;
            this.speech2text.Click += new System.EventHandler(this.speech2text_Click);
            // 
            // output
            // 
            this.output.Font = new System.Drawing.Font("Cambria", 12F);
            this.output.Location = new System.Drawing.Point(852, 142);
            this.output.Name = "output";
            this.output.Size = new System.Drawing.Size(536, 571);
            this.output.TabIndex = 76;
            this.output.Text = "";
            // 
            // input
            // 
            this.input.Font = new System.Drawing.Font("Cambria", 12F);
            this.input.Location = new System.Drawing.Point(296, 142);
            this.input.Name = "input";
            this.input.Size = new System.Drawing.Size(527, 571);
            this.input.TabIndex = 75;
            this.input.Text = "";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-160, -79);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1642, 868);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 74;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.YellowGreen;
            this.button1.Font = new System.Drawing.Font("Tahoma", 16.2F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.LemonChiffon;
            this.button1.Location = new System.Drawing.Point(1246, 724);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(164, 52);
            this.button1.TabIndex = 81;
            this.button1.Text = "Back";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Text_translator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1478, 788);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lang2);
            this.Controls.Add(this.lang1);
            this.Controls.Add(this.translate);
            this.Controls.Add(this.speech2text);
            this.Controls.Add(this.output);
            this.Controls.Add(this.input);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Text_translator";
            this.Text = "Text_translator";
            this.Load += new System.EventHandler(this.Text_translator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.translate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speech2text)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox lang2;
        private System.Windows.Forms.ComboBox lang1;
        private System.Windows.Forms.PictureBox translate;
        private System.Windows.Forms.PictureBox speech2text;
        private System.Windows.Forms.RichTextBox output;
        private System.Windows.Forms.RichTextBox input;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
    }
}
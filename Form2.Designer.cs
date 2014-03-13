namespace AnalizatorNurkowaniaWFA
{
    partial class FormParams
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
         this.tabParameters = new System.Windows.Forms.TabControl();
         this.tabParDive = new System.Windows.Forms.TabPage();
         this.label8 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.predkoscWynurzania = new System.Windows.Forms.TextBox();
         this.predkoscZanurzania = new System.Windows.Forms.TextBox();
         this.ciezarWody = new System.Windows.Forms.TextBox();
         this.cisnienieAtmosferyczne = new System.Windows.Forms.TextBox();
         this.tabParDeco = new System.Windows.Forms.TabPage();
         this.radioZHL16C = new System.Windows.Forms.RadioButton();
         this.radioZHL16B = new System.Windows.Forms.RadioButton();
         this.radioZHL16A = new System.Windows.Forms.RadioButton();
         this.radioMF11F6 = new System.Windows.Forms.RadioButton();
         this.radioZHL12 = new System.Windows.Forms.RadioButton();
         this.radioWorkman = new System.Windows.Forms.RadioButton();
         this.tabParGeneral = new System.Windows.Forms.TabPage();
         this.buttonOK = new System.Windows.Forms.Button();
         this.buttonCancel = new System.Windows.Forms.Button();
         this.tabParameters.SuspendLayout();
         this.tabParDive.SuspendLayout();
         this.tabParDeco.SuspendLayout();
         this.SuspendLayout();
         // 
         // tabParameters
         // 
         this.tabParameters.Controls.Add(this.tabParDive);
         this.tabParameters.Controls.Add(this.tabParDeco);
         this.tabParameters.Controls.Add(this.tabParGeneral);
         this.tabParameters.Location = new System.Drawing.Point(12, 12);
         this.tabParameters.Name = "tabParameters";
         this.tabParameters.SelectedIndex = 0;
         this.tabParameters.Size = new System.Drawing.Size(416, 371);
         this.tabParameters.TabIndex = 0;
         // 
         // tabParDive
         // 
         this.tabParDive.Controls.Add(this.label8);
         this.tabParDive.Controls.Add(this.label7);
         this.tabParDive.Controls.Add(this.label6);
         this.tabParDive.Controls.Add(this.label5);
         this.tabParDive.Controls.Add(this.label4);
         this.tabParDive.Controls.Add(this.label3);
         this.tabParDive.Controls.Add(this.label2);
         this.tabParDive.Controls.Add(this.label1);
         this.tabParDive.Controls.Add(this.predkoscWynurzania);
         this.tabParDive.Controls.Add(this.predkoscZanurzania);
         this.tabParDive.Controls.Add(this.ciezarWody);
         this.tabParDive.Controls.Add(this.cisnienieAtmosferyczne);
         this.tabParDive.Location = new System.Drawing.Point(4, 22);
         this.tabParDive.Name = "tabParDive";
         this.tabParDive.Padding = new System.Windows.Forms.Padding(3);
         this.tabParDive.Size = new System.Drawing.Size(408, 345);
         this.tabParDive.TabIndex = 0;
         this.tabParDive.Text = "Parametry nurkowania";
         this.tabParDive.UseVisualStyleBackColor = true;
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(316, 40);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(40, 13);
         this.label8.TabIndex = 11;
         this.label8.Text = "[N/m3]";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(316, 14);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(28, 13);
         this.label7.TabIndex = 10;
         this.label7.Text = "[bar]";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(316, 66);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(42, 13);
         this.label6.TabIndex = 9;
         this.label6.Text = "[m/min]";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(316, 92);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(42, 13);
         this.label5.TabIndex = 8;
         this.label5.Text = "[m/min]";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(6, 92);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(108, 13);
         this.label4.TabIndex = 7;
         this.label4.Text = "Prędkość wynurzania";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(6, 66);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(106, 13);
         this.label3.TabIndex = 6;
         this.label3.Text = "Prędkośc zanurzania";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(6, 40);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(111, 13);
         this.label2.TabIndex = 5;
         this.label2.Text = "Ciężar właściwy wody";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(6, 14);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(120, 13);
         this.label1.TabIndex = 4;
         this.label1.Text = "Cisnienie atmosferyczne";
         // 
         // predkoscWynurzania
         // 
         this.predkoscWynurzania.Location = new System.Drawing.Point(176, 85);
         this.predkoscWynurzania.Name = "predkoscWynurzania";
         this.predkoscWynurzania.Size = new System.Drawing.Size(134, 20);
         this.predkoscWynurzania.TabIndex = 3;
         // 
         // predkoscZanurzania
         // 
         this.predkoscZanurzania.Location = new System.Drawing.Point(176, 59);
         this.predkoscZanurzania.Name = "predkoscZanurzania";
         this.predkoscZanurzania.Size = new System.Drawing.Size(134, 20);
         this.predkoscZanurzania.TabIndex = 2;
         // 
         // ciezarWody
         // 
         this.ciezarWody.Location = new System.Drawing.Point(176, 33);
         this.ciezarWody.Name = "ciezarWody";
         this.ciezarWody.Size = new System.Drawing.Size(134, 20);
         this.ciezarWody.TabIndex = 1;
         // 
         // cisnienieAtmosferyczne
         // 
         this.cisnienieAtmosferyczne.Location = new System.Drawing.Point(176, 7);
         this.cisnienieAtmosferyczne.Name = "cisnienieAtmosferyczne";
         this.cisnienieAtmosferyczne.Size = new System.Drawing.Size(134, 20);
         this.cisnienieAtmosferyczne.TabIndex = 0;
         // 
         // tabParDeco
         // 
         this.tabParDeco.Controls.Add(this.radioZHL16C);
         this.tabParDeco.Controls.Add(this.radioZHL16B);
         this.tabParDeco.Controls.Add(this.radioZHL16A);
         this.tabParDeco.Controls.Add(this.radioMF11F6);
         this.tabParDeco.Controls.Add(this.radioZHL12);
         this.tabParDeco.Controls.Add(this.radioWorkman);
         this.tabParDeco.Location = new System.Drawing.Point(4, 22);
         this.tabParDeco.Name = "tabParDeco";
         this.tabParDeco.Size = new System.Drawing.Size(408, 345);
         this.tabParDeco.TabIndex = 2;
         this.tabParDeco.Text = "Parametry dekompresji";
         this.tabParDeco.UseVisualStyleBackColor = true;
         // 
         // radioZHL16C
         // 
         this.radioZHL16C.AutoSize = true;
         this.radioZHL16C.Location = new System.Drawing.Point(30, 139);
         this.radioZHL16C.Name = "radioZHL16C";
         this.radioZHL16C.Size = new System.Drawing.Size(214, 17);
         this.radioZHL16C.TabIndex = 5;
         this.radioZHL16C.Text = "Bühlmann 16+1 tkanka  ZHL-16 C 1990";
         this.radioZHL16C.UseVisualStyleBackColor = true;
         // 
         // radioZHL16B
         // 
         this.radioZHL16B.AutoSize = true;
         this.radioZHL16B.Location = new System.Drawing.Point(30, 116);
         this.radioZHL16B.Name = "radioZHL16B";
         this.radioZHL16B.Size = new System.Drawing.Size(211, 17);
         this.radioZHL16B.TabIndex = 4;
         this.radioZHL16B.Text = "Bühlmann 16+1 tkanka ZHL-16 B 1990";
         this.radioZHL16B.UseVisualStyleBackColor = true;
         // 
         // radioZHL16A
         // 
         this.radioZHL16A.AutoSize = true;
         this.radioZHL16A.Checked = true;
         this.radioZHL16A.Location = new System.Drawing.Point(30, 93);
         this.radioZHL16A.Name = "radioZHL16A";
         this.radioZHL16A.Size = new System.Drawing.Size(211, 17);
         this.radioZHL16A.TabIndex = 3;
         this.radioZHL16A.TabStop = true;
         this.radioZHL16A.Text = "Bühlmann 16+1 tkanka ZHL-16 A 1990";
         this.radioZHL16A.UseVisualStyleBackColor = true;
         // 
         // radioMF11F6
         // 
         this.radioMF11F6.AutoSize = true;
         this.radioMF11F6.Location = new System.Drawing.Point(30, 70);
         this.radioMF11F6.Name = "radioMF11F6";
         this.radioMF11F6.Size = new System.Drawing.Size(203, 17);
         this.radioMF11F6.TabIndex = 2;
         this.radioMF11F6.TabStop = true;
         this.radioMF11F6.Text = "Hamilton  11 tkanek (DCAP) MF 11F6";
         this.radioMF11F6.UseVisualStyleBackColor = true;
         // 
         // radioZHL12
         // 
         this.radioZHL12.AutoSize = true;
         this.radioZHL12.Location = new System.Drawing.Point(30, 47);
         this.radioZHL12.Name = "radioZHL12";
         this.radioZHL12.Size = new System.Drawing.Size(189, 17);
         this.radioZHL12.TabIndex = 1;
         this.radioZHL12.TabStop = true;
         this.radioZHL12.Text = "Bühlmann 16 tkanek ZHL-12 1983";
         this.radioZHL12.UseVisualStyleBackColor = true;
         // 
         // radioWorkman
         // 
         this.radioWorkman.AutoSize = true;
         this.radioWorkman.Location = new System.Drawing.Point(30, 24);
         this.radioWorkman.Name = "radioWorkman";
         this.radioWorkman.Size = new System.Drawing.Size(143, 17);
         this.radioWorkman.TabIndex = 0;
         this.radioWorkman.TabStop = true;
         this.radioWorkman.Text = "Workman 9 tkanek 1965";
         this.radioWorkman.UseVisualStyleBackColor = true;
         // 
         // tabParGeneral
         // 
         this.tabParGeneral.Location = new System.Drawing.Point(4, 22);
         this.tabParGeneral.Name = "tabParGeneral";
         this.tabParGeneral.Padding = new System.Windows.Forms.Padding(3);
         this.tabParGeneral.Size = new System.Drawing.Size(408, 345);
         this.tabParGeneral.TabIndex = 1;
         this.tabParGeneral.Text = "Parametry ogólne";
         this.tabParGeneral.UseVisualStyleBackColor = true;
         // 
         // buttonOK
         // 
         this.buttonOK.Location = new System.Drawing.Point(348, 389);
         this.buttonOK.Name = "buttonOK";
         this.buttonOK.Size = new System.Drawing.Size(75, 23);
         this.buttonOK.TabIndex = 1;
         this.buttonOK.Text = "Zastosuj";
         this.buttonOK.UseVisualStyleBackColor = true;
         this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
         // 
         // buttonCancel
         // 
         this.buttonCancel.Location = new System.Drawing.Point(267, 389);
         this.buttonCancel.Name = "buttonCancel";
         this.buttonCancel.Size = new System.Drawing.Size(75, 23);
         this.buttonCancel.TabIndex = 2;
         this.buttonCancel.Text = "Anuluj";
         this.buttonCancel.UseVisualStyleBackColor = true;
         this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
         // 
         // FormParams
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(440, 416);
         this.Controls.Add(this.buttonCancel);
         this.Controls.Add(this.buttonOK);
         this.Controls.Add(this.tabParameters);
         this.Name = "FormParams";
         this.Text = "Parametry";
         this.tabParameters.ResumeLayout(false);
         this.tabParDive.ResumeLayout(false);
         this.tabParDive.PerformLayout();
         this.tabParDeco.ResumeLayout(false);
         this.tabParDeco.PerformLayout();
         this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabParameters;
        private System.Windows.Forms.TabPage tabParDive;
        private System.Windows.Forms.TabPage tabParDeco;
        private System.Windows.Forms.TabPage tabParGeneral;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.RadioButton radioZHL16C;
        private System.Windows.Forms.RadioButton radioZHL16B;
        private System.Windows.Forms.RadioButton radioZHL16A;
        private System.Windows.Forms.RadioButton radioMF11F6;
        private System.Windows.Forms.RadioButton radioZHL12;
        private System.Windows.Forms.RadioButton radioWorkman;
        private System.Windows.Forms.TextBox cisnienieAtmosferyczne;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox predkoscWynurzania;
        private System.Windows.Forms.TextBox predkoscZanurzania;
        private System.Windows.Forms.TextBox ciezarWody;
    }
}
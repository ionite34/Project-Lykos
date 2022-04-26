namespace Project_Lykos
{
    partial class AdditionalOptions
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button_resampler = new System.Windows.Forms.Button();
            this.button_settings = new System.Windows.Forms.Button();
            this.button_wordchecker = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.button_resampler, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button_settings, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.button_wordchecker, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(240, 189);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // button_resampler
            // 
            this.button_resampler.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_resampler.Location = new System.Drawing.Point(3, 3);
            this.button_resampler.Name = "button_resampler";
            this.button_resampler.Size = new System.Drawing.Size(234, 57);
            this.button_resampler.TabIndex = 0;
            this.button_resampler.Text = "Audio Resampler Tool";
            this.button_resampler.UseVisualStyleBackColor = true;
            this.button_resampler.Click += new System.EventHandler(this.button_resampler_Click);
            // 
            // button_settings
            // 
            this.button_settings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_settings.Location = new System.Drawing.Point(3, 129);
            this.button_settings.Name = "button_settings";
            this.button_settings.Size = new System.Drawing.Size(234, 57);
            this.button_settings.TabIndex = 1;
            this.button_settings.Text = "Advanced Settings";
            this.button_settings.UseVisualStyleBackColor = true;
            // 
            // button_wordchecker
            // 
            this.button_wordchecker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_wordchecker.Location = new System.Drawing.Point(3, 66);
            this.button_wordchecker.Name = "button_wordchecker";
            this.button_wordchecker.Size = new System.Drawing.Size(234, 57);
            this.button_wordchecker.TabIndex = 2;
            this.button_wordchecker.Text = "Word Checker Tool";
            this.button_wordchecker.UseVisualStyleBackColor = true;
            this.button_wordchecker.Click += new System.EventHandler(this.button_wordchecker_Click);
            // 
            // AdditionalOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 213);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdditionalOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button button_resampler;
        private Button button_settings;
        private Button button_wordchecker;
    }
}
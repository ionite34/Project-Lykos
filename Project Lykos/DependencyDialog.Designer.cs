namespace Project_Lykos
{
    partial class DependencyDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DependencyDialog));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button_retry = new System.Windows.Forms.Button();
            this.button_exit = new System.Windows.Forms.Button();
            this.label_1 = new System.Windows.Forms.Label();
            this.label_2 = new System.Windows.Forms.Label();
            this.linkLabel_download = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label_3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.button_retry, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button_exit, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(196, 220);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(334, 61);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // button_retry
            // 
            this.button_retry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_retry.Location = new System.Drawing.Point(10, 10);
            this.button_retry.Margin = new System.Windows.Forms.Padding(10);
            this.button_retry.Name = "button_retry";
            this.button_retry.Size = new System.Drawing.Size(147, 41);
            this.button_retry.TabIndex = 0;
            this.button_retry.Text = "Recheck";
            this.button_retry.UseVisualStyleBackColor = true;
            this.button_retry.Click += new System.EventHandler(this.Button_retry_Click);
            // 
            // button_exit
            // 
            this.button_exit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_exit.Location = new System.Drawing.Point(177, 10);
            this.button_exit.Margin = new System.Windows.Forms.Padding(10);
            this.button_exit.Name = "button_exit";
            this.button_exit.Size = new System.Drawing.Size(147, 41);
            this.button_exit.TabIndex = 1;
            this.button_exit.Text = "Exit";
            this.button_exit.UseVisualStyleBackColor = true;
            this.button_exit.Click += new System.EventHandler(this.Button_exit_Click);
            // 
            // label_1
            // 
            this.label_1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_1.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label_1.Location = new System.Drawing.Point(24, 29);
            this.label_1.Name = "label_1";
            this.label_1.Size = new System.Drawing.Size(497, 97);
            this.label_1.TabIndex = 1;
            this.label_1.Text = resources.GetString("label_1.Text");
            // 
            // label_2
            // 
            this.label_2.AutoSize = true;
            this.label_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_2.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label_2.Location = new System.Drawing.Point(3, 0);
            this.label_2.Name = "label_2";
            this.label_2.Size = new System.Drawing.Size(252, 33);
            this.label_2.TabIndex = 2;
            this.label_2.Text = "You can also download it from:";
            this.label_2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkLabel_download
            // 
            this.linkLabel_download.AutoSize = true;
            this.linkLabel_download.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabel_download.Location = new System.Drawing.Point(261, 0);
            this.linkLabel_download.Name = "linkLabel_download";
            this.linkLabel_download.Size = new System.Drawing.Size(239, 33);
            this.linkLabel_download.TabIndex = 3;
            this.linkLabel_download.TabStop = true;
            this.linkLabel_download.Text = "Nexus Mods";
            this.linkLabel_download.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel_download.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_download_LinkClicked);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label_2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.linkLabel_download, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(24, 135);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(503, 33);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // label_3
            // 
            this.label_3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_3.AutoSize = true;
            this.label_3.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label_3.Location = new System.Drawing.Point(27, 180);
            this.label_3.Name = "label_3";
            this.label_3.Size = new System.Drawing.Size(480, 24);
            this.label_3.TabIndex = 5;
            this.label_3.Text = "Place the file into the same directory as this tool to continue.";
            // 
            // DependencyDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 296);
            this.Controls.Add(this.label_3);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.label_1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DependencyDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Additional resource file required";
            this.Load += new System.EventHandler(this.DependencyDialog_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button button_retry;
        private Button button_exit;
        private Label label_1;
        private Label label_2;
        private LinkLabel linkLabel_download;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label_3;
    }
}
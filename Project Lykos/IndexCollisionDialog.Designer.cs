namespace Project_Lykos
{
    partial class IndexCollisionDialog
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
            this.button_continue = new System.Windows.Forms.Button();
            this.button_exit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.combo_textChoices = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBox_override = new System.Windows.Forms.CheckBox();
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
            this.tableLayoutPanel1.Controls.Add(this.button_continue, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button_exit, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(125, 254);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(374, 61);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // button_continue
            // 
            this.button_continue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_continue.Enabled = false;
            this.button_continue.Location = new System.Drawing.Point(10, 10);
            this.button_continue.Margin = new System.Windows.Forms.Padding(10);
            this.button_continue.Name = "button_continue";
            this.button_continue.Size = new System.Drawing.Size(167, 41);
            this.button_continue.TabIndex = 0;
            this.button_continue.Text = "Continue";
            this.button_continue.UseVisualStyleBackColor = true;
            // 
            // button_exit
            // 
            this.button_exit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_exit.Location = new System.Drawing.Point(197, 10);
            this.button_exit.Margin = new System.Windows.Forms.Padding(10);
            this.button_exit.Name = "button_exit";
            this.button_exit.Size = new System.Drawing.Size(167, 41);
            this.button_exit.TabIndex = 1;
            this.button_exit.Text = "Stop Indexing";
            this.button_exit.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(459, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Multiple text records were found for the same audio file:";
            // 
            // combo_textChoices
            // 
            this.combo_textChoices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.combo_textChoices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_textChoices.FormattingEnabled = true;
            this.combo_textChoices.Location = new System.Drawing.Point(3, 117);
            this.combo_textChoices.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.combo_textChoices.Name = "combo_textChoices";
            this.combo_textChoices.Size = new System.Drawing.Size(459, 32);
            this.combo_textChoices.TabIndex = 3;
            this.combo_textChoices.SelectedValueChanged += new System.EventHandler(this.Combo_textChoices_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(459, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "To continue, choose a corresponding text entry below:";
            // 
            // linkLabel1
            // 
            this.linkLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabel1.Location = new System.Drawing.Point(3, 28);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(459, 57);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "AudioFile";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.combo_textChoices, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.linkLabel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.checkBox_override, 0, 4);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(24, 25);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(465, 216);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // checkBox_override
            // 
            this.checkBox_override.AutoSize = true;
            this.checkBox_override.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_override.Location = new System.Drawing.Point(3, 156);
            this.checkBox_override.Name = "checkBox_override";
            this.checkBox_override.Size = new System.Drawing.Size(459, 57);
            this.checkBox_override.TabIndex = 6;
            this.checkBox_override.Text = "Remember my choice for this file name";
            this.checkBox_override.UseVisualStyleBackColor = true;
            this.checkBox_override.CheckedChanged += new System.EventHandler(this.checkBox_override_CheckedChanged);
            // 
            // IndexCollisionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(511, 327);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "IndexCollisionDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Audio file matches multiple text records";
            this.Load += new System.EventHandler(this.IndexCollisionDialog_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button button_continue;
        private Button button_exit;
        private Label label1;
        private ComboBox combo_textChoices;
        private Label label2;
        private LinkLabel linkLabel1;
        private TableLayoutPanel tableLayoutPanel2;
        private CheckBox checkBox_override;
    }
}
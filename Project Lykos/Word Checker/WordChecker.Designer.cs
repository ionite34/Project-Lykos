namespace Project_Lykos.Word_Checker
{
    partial class WordChecker
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
            this.Group_Paths = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.button_browse_dict = new System.Windows.Forms.Button();
            this.combo_dict = new System.Windows.Forms.ComboBox();
            this.label_dict = new System.Windows.Forms.Label();
            this.combo_csv = new System.Windows.Forms.ComboBox();
            this.label_csv = new System.Windows.Forms.Label();
            this.button_browse_csv = new System.Windows.Forms.Button();
            this.dataview_freq = new System.Windows.Forms.DataGridView();
            this.group_data_word = new System.Windows.Forms.GroupBox();
            this.group_filters = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel_filters = new System.Windows.Forms.TableLayoutPanel();
            this.check_ShowDict = new System.Windows.Forms.CheckBox();
            this.check_ShowIgnore = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel_data_parent = new System.Windows.Forms.TableLayoutPanel();
            this.group_data_lines = new System.Windows.Forms.GroupBox();
            this.dataview_usage = new System.Windows.Forms.DataGridView();
            this.group_actions = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel_actions = new System.Windows.Forms.TableLayoutPanel();
            this.button_refresh = new System.Windows.Forms.Button();
            this.Group_Paths.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataview_freq)).BeginInit();
            this.group_data_word.SuspendLayout();
            this.group_filters.SuspendLayout();
            this.tableLayoutPanel_filters.SuspendLayout();
            this.tableLayoutPanel_data_parent.SuspendLayout();
            this.group_data_lines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataview_usage)).BeginInit();
            this.group_actions.SuspendLayout();
            this.tableLayoutPanel_actions.SuspendLayout();
            this.SuspendLayout();
            // 
            // Group_Paths
            // 
            this.Group_Paths.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Group_Paths.Controls.Add(this.tableLayoutPanel2);
            this.Group_Paths.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Group_Paths.Location = new System.Drawing.Point(12, 12);
            this.Group_Paths.Name = "Group_Paths";
            this.Group_Paths.Size = new System.Drawing.Size(994, 105);
            this.Group_Paths.TabIndex = 11;
            this.Group_Paths.TabStop = false;
            this.Group_Paths.Text = "Paths";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.Controls.Add(this.button_browse_dict, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.combo_dict, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label_dict, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.combo_csv, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label_csv, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.button_browse_csv, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 27);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(988, 75);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // button_browse_dict
            // 
            this.button_browse_dict.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_browse_dict.Location = new System.Drawing.Point(941, 42);
            this.button_browse_dict.Name = "button_browse_dict";
            this.button_browse_dict.Size = new System.Drawing.Size(44, 33);
            this.button_browse_dict.TabIndex = 7;
            this.button_browse_dict.Text = "...";
            this.button_browse_dict.UseVisualStyleBackColor = true;
            this.button_browse_dict.Click += new System.EventHandler(this.Button_browse_dict_Click);
            // 
            // combo_dict
            // 
            this.combo_dict.Dock = System.Windows.Forms.DockStyle.Top;
            this.combo_dict.FormattingEnabled = true;
            this.combo_dict.Location = new System.Drawing.Point(159, 42);
            this.combo_dict.Name = "combo_dict";
            this.combo_dict.Size = new System.Drawing.Size(776, 32);
            this.combo_dict.TabIndex = 3;
            // 
            // label_dict
            // 
            this.label_dict.AutoSize = true;
            this.label_dict.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_dict.Location = new System.Drawing.Point(3, 39);
            this.label_dict.Name = "label_dict";
            this.label_dict.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label_dict.Size = new System.Drawing.Size(150, 29);
            this.label_dict.TabIndex = 2;
            this.label_dict.Text = "Dictionary Folder:";
            // 
            // combo_csv
            // 
            this.combo_csv.Dock = System.Windows.Forms.DockStyle.Top;
            this.combo_csv.FormattingEnabled = true;
            this.combo_csv.Location = new System.Drawing.Point(159, 3);
            this.combo_csv.Name = "combo_csv";
            this.combo_csv.Size = new System.Drawing.Size(776, 32);
            this.combo_csv.TabIndex = 0;
            // 
            // label_csv
            // 
            this.label_csv.AutoSize = true;
            this.label_csv.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_csv.Location = new System.Drawing.Point(3, 0);
            this.label_csv.Name = "label_csv";
            this.label_csv.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label_csv.Size = new System.Drawing.Size(150, 29);
            this.label_csv.TabIndex = 1;
            this.label_csv.Text = "CSV:";
            // 
            // button_browse_csv
            // 
            this.button_browse_csv.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_browse_csv.Location = new System.Drawing.Point(941, 3);
            this.button_browse_csv.Name = "button_browse_csv";
            this.button_browse_csv.Size = new System.Drawing.Size(44, 33);
            this.button_browse_csv.TabIndex = 6;
            this.button_browse_csv.Text = "...";
            this.button_browse_csv.UseVisualStyleBackColor = true;
            this.button_browse_csv.Click += new System.EventHandler(this.Button_browse_csv_Click);
            // 
            // dataview_freq
            // 
            this.dataview_freq.AllowUserToAddRows = false;
            this.dataview_freq.AllowUserToDeleteRows = false;
            this.dataview_freq.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataview_freq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataview_freq.Location = new System.Drawing.Point(3, 27);
            this.dataview_freq.Name = "dataview_freq";
            this.dataview_freq.ReadOnly = true;
            this.dataview_freq.RowHeadersWidth = 62;
            this.dataview_freq.RowTemplate.Height = 33;
            this.dataview_freq.Size = new System.Drawing.Size(681, 890);
            this.dataview_freq.TabIndex = 12;
            this.dataview_freq.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataview_freq_CellClick);
            // 
            // group_data_word
            // 
            this.group_data_word.Controls.Add(this.dataview_freq);
            this.group_data_word.Dock = System.Windows.Forms.DockStyle.Fill;
            this.group_data_word.Location = new System.Drawing.Point(3, 3);
            this.group_data_word.Name = "group_data_word";
            this.group_data_word.Size = new System.Drawing.Size(687, 920);
            this.group_data_word.TabIndex = 13;
            this.group_data_word.TabStop = false;
            this.group_data_word.Text = "Word Frequency";
            // 
            // group_filters
            // 
            this.group_filters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.group_filters.Controls.Add(this.tableLayoutPanel_filters);
            this.group_filters.Location = new System.Drawing.Point(1208, 12);
            this.group_filters.Name = "group_filters";
            this.group_filters.Size = new System.Drawing.Size(190, 105);
            this.group_filters.TabIndex = 14;
            this.group_filters.TabStop = false;
            this.group_filters.Text = "Filters";
            // 
            // tableLayoutPanel_filters
            // 
            this.tableLayoutPanel_filters.ColumnCount = 1;
            this.tableLayoutPanel_filters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_filters.Controls.Add(this.check_ShowDict, 0, 0);
            this.tableLayoutPanel_filters.Controls.Add(this.check_ShowIgnore, 0, 1);
            this.tableLayoutPanel_filters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_filters.Location = new System.Drawing.Point(3, 27);
            this.tableLayoutPanel_filters.Name = "tableLayoutPanel_filters";
            this.tableLayoutPanel_filters.RowCount = 2;
            this.tableLayoutPanel_filters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_filters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_filters.Size = new System.Drawing.Size(184, 75);
            this.tableLayoutPanel_filters.TabIndex = 0;
            // 
            // check_ShowDict
            // 
            this.check_ShowDict.AutoSize = true;
            this.check_ShowDict.Checked = true;
            this.check_ShowDict.CheckState = System.Windows.Forms.CheckState.Checked;
            this.check_ShowDict.Location = new System.Drawing.Point(3, 3);
            this.check_ShowDict.Name = "check_ShowDict";
            this.check_ShowDict.Size = new System.Drawing.Size(165, 28);
            this.check_ShowDict.TabIndex = 0;
            this.check_ShowDict.Text = "Show Dictionary";
            this.check_ShowDict.UseVisualStyleBackColor = true;
            this.check_ShowDict.CheckedChanged += new System.EventHandler(this.Check_Any_Changed);
            // 
            // check_ShowIgnore
            // 
            this.check_ShowIgnore.AutoSize = true;
            this.check_ShowIgnore.Checked = true;
            this.check_ShowIgnore.CheckState = System.Windows.Forms.CheckState.Checked;
            this.check_ShowIgnore.Location = new System.Drawing.Point(3, 40);
            this.check_ShowIgnore.Name = "check_ShowIgnore";
            this.check_ShowIgnore.Size = new System.Drawing.Size(145, 28);
            this.check_ShowIgnore.TabIndex = 1;
            this.check_ShowIgnore.Text = "Show Ignores";
            this.check_ShowIgnore.UseVisualStyleBackColor = true;
            this.check_ShowIgnore.CheckedChanged += new System.EventHandler(this.Check_Any_Changed);
            // 
            // tableLayoutPanel_data_parent
            // 
            this.tableLayoutPanel_data_parent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel_data_parent.ColumnCount = 2;
            this.tableLayoutPanel_data_parent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_data_parent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_data_parent.Controls.Add(this.group_data_lines, 1, 0);
            this.tableLayoutPanel_data_parent.Controls.Add(this.group_data_word, 0, 0);
            this.tableLayoutPanel_data_parent.Location = new System.Drawing.Point(12, 123);
            this.tableLayoutPanel_data_parent.Name = "tableLayoutPanel_data_parent";
            this.tableLayoutPanel_data_parent.RowCount = 1;
            this.tableLayoutPanel_data_parent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_data_parent.Size = new System.Drawing.Size(1386, 926);
            this.tableLayoutPanel_data_parent.TabIndex = 15;
            // 
            // group_data_lines
            // 
            this.group_data_lines.Controls.Add(this.dataview_usage);
            this.group_data_lines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.group_data_lines.Location = new System.Drawing.Point(696, 3);
            this.group_data_lines.Name = "group_data_lines";
            this.group_data_lines.Size = new System.Drawing.Size(687, 920);
            this.group_data_lines.TabIndex = 14;
            this.group_data_lines.TabStop = false;
            this.group_data_lines.Text = "Word Usages in Lines";
            // 
            // dataview_usage
            // 
            this.dataview_usage.AllowUserToAddRows = false;
            this.dataview_usage.AllowUserToDeleteRows = false;
            this.dataview_usage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataview_usage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataview_usage.Location = new System.Drawing.Point(3, 27);
            this.dataview_usage.Name = "dataview_usage";
            this.dataview_usage.ReadOnly = true;
            this.dataview_usage.RowHeadersWidth = 62;
            this.dataview_usage.RowTemplate.Height = 33;
            this.dataview_usage.Size = new System.Drawing.Size(681, 890);
            this.dataview_usage.TabIndex = 12;
            // 
            // group_actions
            // 
            this.group_actions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.group_actions.Controls.Add(this.tableLayoutPanel_actions);
            this.group_actions.Location = new System.Drawing.Point(1012, 12);
            this.group_actions.Name = "group_actions";
            this.group_actions.Size = new System.Drawing.Size(190, 105);
            this.group_actions.TabIndex = 15;
            this.group_actions.TabStop = false;
            this.group_actions.Text = "Actions";
            // 
            // tableLayoutPanel_actions
            // 
            this.tableLayoutPanel_actions.ColumnCount = 1;
            this.tableLayoutPanel_actions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_actions.Controls.Add(this.button_refresh, 0, 0);
            this.tableLayoutPanel_actions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_actions.Location = new System.Drawing.Point(3, 27);
            this.tableLayoutPanel_actions.Name = "tableLayoutPanel_actions";
            this.tableLayoutPanel_actions.RowCount = 1;
            this.tableLayoutPanel_actions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_actions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_actions.Size = new System.Drawing.Size(184, 75);
            this.tableLayoutPanel_actions.TabIndex = 0;
            // 
            // button_refresh
            // 
            this.button_refresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_refresh.Enabled = false;
            this.button_refresh.Location = new System.Drawing.Point(3, 3);
            this.button_refresh.Name = "button_refresh";
            this.button_refresh.Size = new System.Drawing.Size(178, 69);
            this.button_refresh.TabIndex = 0;
            this.button_refresh.Text = "Refresh";
            this.button_refresh.UseVisualStyleBackColor = true;
            this.button_refresh.Click += new System.EventHandler(this.Button_refresh_Click);
            // 
            // WordChecker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1410, 1061);
            this.Controls.Add(this.group_actions);
            this.Controls.Add(this.tableLayoutPanel_data_parent);
            this.Controls.Add(this.group_filters);
            this.Controls.Add(this.Group_Paths);
            this.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "WordChecker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WordChecker";
            this.Group_Paths.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataview_freq)).EndInit();
            this.group_data_word.ResumeLayout(false);
            this.group_filters.ResumeLayout(false);
            this.tableLayoutPanel_filters.ResumeLayout(false);
            this.tableLayoutPanel_filters.PerformLayout();
            this.tableLayoutPanel_data_parent.ResumeLayout(false);
            this.group_data_lines.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataview_usage)).EndInit();
            this.group_actions.ResumeLayout(false);
            this.tableLayoutPanel_actions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox Group_Paths;
        private TableLayoutPanel tableLayoutPanel2;
        private Button button_browse_dict;
        private ComboBox combo_dict;
        private Label label_dict;
        private ComboBox combo_csv;
        private Label label_csv;
        private Button button_browse_csv;
        private DataGridView dataview_freq;
        private GroupBox group_data_word;
        private GroupBox group_filters;
        private TableLayoutPanel tableLayoutPanel_filters;
        private CheckBox check_ShowDict;
        private CheckBox check_ShowIgnore;
        private TableLayoutPanel tableLayoutPanel_data_parent;
        private GroupBox group_data_lines;
        private DataGridView dataview_usage;
        private GroupBox group_actions;
        private TableLayoutPanel tableLayoutPanel_actions;
        private Button button_refresh;
    }
}
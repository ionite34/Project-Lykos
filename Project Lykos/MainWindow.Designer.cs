namespace Project_Lykos
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.button_preview = new System.Windows.Forms.Button();
            this.LayoutPanel_LowerButtons = new System.Windows.Forms.TableLayoutPanel();
            this.button_start_batch = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button_stop_batch = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.progress_total = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.combo_outputActions = new System.Windows.Forms.ComboBox();
            this.label_outputActions = new System.Windows.Forms.Label();
            this.combo_csvDelimiter = new System.Windows.Forms.ComboBox();
            this.label_csvDelimiter = new System.Windows.Forms.Label();
            this.combo_multiprocess_count = new System.Windows.Forms.ComboBox();
            this.label_multiprocess_count = new System.Windows.Forms.Label();
            this.combo_audio_preprocessing = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Group_Paths = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.button_browse_csv = new System.Windows.Forms.Button();
            this.button_browse_output = new System.Windows.Forms.Button();
            this.combo_csv = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.combo_output = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.combo_source = new System.Windows.Forms.ComboBox();
            this.label_source = new System.Windows.Forms.Label();
            this.button_browse_source = new System.Windows.Forms.Button();
            this.Group_ProcessingView = new System.Windows.Forms.GroupBox();
            this.Group_TotalProgress = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label_progress_value2A = new System.Windows.Forms.Label();
            this.label_progress_status2A = new System.Windows.Forms.Label();
            this.label_progress_value1A = new System.Windows.Forms.Label();
            this.label_progress_status1A = new System.Windows.Forms.Label();
            this.Group_Settings = new System.Windows.Forms.GroupBox();
            this.progress_batch = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label_batch_value = new System.Windows.Forms.Label();
            this.label_batch_status = new System.Windows.Forms.Label();
            this.LayoutPanel_LowerButtons.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.Group_Paths.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.Group_ProcessingView.SuspendLayout();
            this.Group_TotalProgress.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.Group_Settings.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_preview
            // 
            this.button_preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_preview.Enabled = false;
            this.button_preview.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button_preview.Location = new System.Drawing.Point(13, 13);
            this.button_preview.Name = "button_preview";
            this.button_preview.Size = new System.Drawing.Size(506, 49);
            this.button_preview.TabIndex = 0;
            this.button_preview.Text = "Preview Indexed Operations";
            this.button_preview.UseVisualStyleBackColor = true;
            this.button_preview.Click += new System.EventHandler(this.button_preview_Click);
            // 
            // LayoutPanel_LowerButtons
            // 
            this.LayoutPanel_LowerButtons.ColumnCount = 2;
            this.LayoutPanel_LowerButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.LayoutPanel_LowerButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.LayoutPanel_LowerButtons.Controls.Add(this.button_preview, 0, 0);
            this.LayoutPanel_LowerButtons.Controls.Add(this.button_start_batch, 1, 0);
            this.LayoutPanel_LowerButtons.Controls.Add(this.button3, 0, 1);
            this.LayoutPanel_LowerButtons.Controls.Add(this.button_stop_batch, 1, 1);
            this.LayoutPanel_LowerButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LayoutPanel_LowerButtons.Location = new System.Drawing.Point(0, 655);
            this.LayoutPanel_LowerButtons.Name = "LayoutPanel_LowerButtons";
            this.LayoutPanel_LowerButtons.Padding = new System.Windows.Forms.Padding(10, 10, 10, 24);
            this.LayoutPanel_LowerButtons.RowCount = 2;
            this.LayoutPanel_LowerButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.LayoutPanel_LowerButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.LayoutPanel_LowerButtons.Size = new System.Drawing.Size(1044, 144);
            this.LayoutPanel_LowerButtons.TabIndex = 1;
            // 
            // button_start_batch
            // 
            this.button_start_batch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_start_batch.Enabled = false;
            this.button_start_batch.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button_start_batch.Location = new System.Drawing.Point(525, 13);
            this.button_start_batch.Name = "button_start_batch";
            this.button_start_batch.Size = new System.Drawing.Size(506, 49);
            this.button_start_batch.TabIndex = 1;
            this.button_start_batch.Text = "Start Batch";
            this.button_start_batch.UseVisualStyleBackColor = true;
            this.button_start_batch.Click += new System.EventHandler(this.button_start_batch_Click);
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Enabled = false;
            this.button3.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button3.Location = new System.Drawing.Point(13, 68);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(506, 49);
            this.button3.TabIndex = 2;
            this.button3.Text = "Settings";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button_stop_batch
            // 
            this.button_stop_batch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_stop_batch.Enabled = false;
            this.button_stop_batch.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button_stop_batch.Location = new System.Drawing.Point(525, 68);
            this.button_stop_batch.Name = "button_stop_batch";
            this.button_stop_batch.Size = new System.Drawing.Size(506, 49);
            this.button_stop_batch.TabIndex = 3;
            this.button_stop_batch.Text = "Stop Batch";
            this.button_stop_batch.UseVisualStyleBackColor = true;
            this.button_stop_batch.Click += new System.EventHandler(this.button_stop_batch_Click);
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(3, 27);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(497, 602);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // progress_total
            // 
            this.progress_total.Location = new System.Drawing.Point(16, 29);
            this.progress_total.Name = "progress_total";
            this.progress_total.Size = new System.Drawing.Size(467, 36);
            this.progress_total.TabIndex = 3;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.combo_outputActions, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.label_outputActions, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.combo_csvDelimiter, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.label_csvDelimiter, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.combo_multiprocess_count, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label_multiprocess_count, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.combo_audio_preprocessing, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(9, 29);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(476, 193);
            this.tableLayoutPanel3.TabIndex = 8;
            // 
            // combo_outputActions
            // 
            this.combo_outputActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.combo_outputActions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_outputActions.FormattingEnabled = true;
            this.combo_outputActions.Items.AddRange(new object[] {
            "Skip .lip files in output",
            "Overwrite .lip files in output"});
            this.combo_outputActions.Location = new System.Drawing.Point(188, 117);
            this.combo_outputActions.Name = "combo_outputActions";
            this.combo_outputActions.Size = new System.Drawing.Size(285, 32);
            this.combo_outputActions.TabIndex = 7;
            // 
            // label_outputActions
            // 
            this.label_outputActions.AutoSize = true;
            this.label_outputActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_outputActions.Location = new System.Drawing.Point(3, 114);
            this.label_outputActions.Name = "label_outputActions";
            this.label_outputActions.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label_outputActions.Size = new System.Drawing.Size(179, 29);
            this.label_outputActions.TabIndex = 6;
            this.label_outputActions.Text = "Output Actions:";
            // 
            // combo_csvDelimiter
            // 
            this.combo_csvDelimiter.Dock = System.Windows.Forms.DockStyle.Top;
            this.combo_csvDelimiter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_csvDelimiter.FormattingEnabled = true;
            this.combo_csvDelimiter.Items.AddRange(new object[] {
            "Auto Detect",
            "Comma [ , ] ",
            "Semicolon [ ; ]",
            "Vertical Slash [ | ]"});
            this.combo_csvDelimiter.Location = new System.Drawing.Point(188, 79);
            this.combo_csvDelimiter.Name = "combo_csvDelimiter";
            this.combo_csvDelimiter.Size = new System.Drawing.Size(285, 32);
            this.combo_csvDelimiter.TabIndex = 5;
            // 
            // label_csvDelimiter
            // 
            this.label_csvDelimiter.AutoSize = true;
            this.label_csvDelimiter.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_csvDelimiter.Location = new System.Drawing.Point(3, 76);
            this.label_csvDelimiter.Name = "label_csvDelimiter";
            this.label_csvDelimiter.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label_csvDelimiter.Size = new System.Drawing.Size(179, 29);
            this.label_csvDelimiter.TabIndex = 4;
            this.label_csvDelimiter.Text = "CSV Delimiter:";
            // 
            // combo_multiprocess_count
            // 
            this.combo_multiprocess_count.Dock = System.Windows.Forms.DockStyle.Top;
            this.combo_multiprocess_count.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_multiprocess_count.FormattingEnabled = true;
            this.combo_multiprocess_count.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.combo_multiprocess_count.Location = new System.Drawing.Point(188, 41);
            this.combo_multiprocess_count.Name = "combo_multiprocess_count";
            this.combo_multiprocess_count.Size = new System.Drawing.Size(285, 32);
            this.combo_multiprocess_count.TabIndex = 3;
            // 
            // label_multiprocess_count
            // 
            this.label_multiprocess_count.AutoSize = true;
            this.label_multiprocess_count.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_multiprocess_count.Location = new System.Drawing.Point(3, 38);
            this.label_multiprocess_count.Name = "label_multiprocess_count";
            this.label_multiprocess_count.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label_multiprocess_count.Size = new System.Drawing.Size(179, 29);
            this.label_multiprocess_count.TabIndex = 2;
            this.label_multiprocess_count.Text = "Multiprocess Count:";
            // 
            // combo_audio_preprocessing
            // 
            this.combo_audio_preprocessing.Dock = System.Windows.Forms.DockStyle.Top;
            this.combo_audio_preprocessing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_audio_preprocessing.FormattingEnabled = true;
            this.combo_audio_preprocessing.Items.AddRange(new object[] {
            "None (Requires 16kHz 16bit mono)",
            "FaceFX Native",
            "Resampled (Standard)",
            "Biquadratic filtered (Safest)"});
            this.combo_audio_preprocessing.Location = new System.Drawing.Point(188, 3);
            this.combo_audio_preprocessing.Name = "combo_audio_preprocessing";
            this.combo_audio_preprocessing.Size = new System.Drawing.Size(285, 32);
            this.combo_audio_preprocessing.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label1.Size = new System.Drawing.Size(179, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Audio Preprocessing:";
            // 
            // Group_Paths
            // 
            this.Group_Paths.Controls.Add(this.tableLayoutPanel2);
            this.Group_Paths.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Group_Paths.Location = new System.Drawing.Point(14, 17);
            this.Group_Paths.Name = "Group_Paths";
            this.Group_Paths.Size = new System.Drawing.Size(499, 172);
            this.Group_Paths.TabIndex = 9;
            this.Group_Paths.TabStop = false;
            this.Group_Paths.Text = "Paths";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.Controls.Add(this.button_browse_csv, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.button_browse_output, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.combo_csv, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.combo_output, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.combo_source, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label_source, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.button_browse_source, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(9, 30);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(476, 136);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // button_browse_csv
            // 
            this.button_browse_csv.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_browse_csv.Location = new System.Drawing.Point(429, 83);
            this.button_browse_csv.Name = "button_browse_csv";
            this.button_browse_csv.Size = new System.Drawing.Size(44, 34);
            this.button_browse_csv.TabIndex = 8;
            this.button_browse_csv.Text = "...";
            this.button_browse_csv.UseVisualStyleBackColor = true;
            this.button_browse_csv.Click += new System.EventHandler(this.Button_browse_csv_Click);
            // 
            // button_browse_output
            // 
            this.button_browse_output.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_browse_output.Location = new System.Drawing.Point(429, 43);
            this.button_browse_output.Name = "button_browse_output";
            this.button_browse_output.Size = new System.Drawing.Size(44, 34);
            this.button_browse_output.TabIndex = 7;
            this.button_browse_output.Text = "...";
            this.button_browse_output.UseVisualStyleBackColor = true;
            this.button_browse_output.Click += new System.EventHandler(this.Button_browse_output_Click);
            // 
            // combo_csv
            // 
            this.combo_csv.Dock = System.Windows.Forms.DockStyle.Top;
            this.combo_csv.FormattingEnabled = true;
            this.combo_csv.Location = new System.Drawing.Point(87, 83);
            this.combo_csv.Name = "combo_csv";
            this.combo_csv.Size = new System.Drawing.Size(336, 32);
            this.combo_csv.TabIndex = 5;
            this.combo_csv.Enter += new System.EventHandler(this.Combo_Any_Enter_Leave);
            this.combo_csv.Leave += new System.EventHandler(this.Combo_Any_Enter_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(3, 80);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label3.Size = new System.Drawing.Size(78, 29);
            this.label3.TabIndex = 4;
            this.label3.Text = "CSV File:";
            // 
            // combo_output
            // 
            this.combo_output.Dock = System.Windows.Forms.DockStyle.Top;
            this.combo_output.FormattingEnabled = true;
            this.combo_output.Location = new System.Drawing.Point(87, 43);
            this.combo_output.Name = "combo_output";
            this.combo_output.Size = new System.Drawing.Size(336, 32);
            this.combo_output.TabIndex = 3;
            this.combo_output.Enter += new System.EventHandler(this.Combo_Any_Enter_Leave);
            this.combo_output.Leave += new System.EventHandler(this.Combo_Any_Enter_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(3, 40);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label2.Size = new System.Drawing.Size(78, 29);
            this.label2.TabIndex = 2;
            this.label2.Text = "Output:";
            // 
            // combo_source
            // 
            this.combo_source.Dock = System.Windows.Forms.DockStyle.Top;
            this.combo_source.FormattingEnabled = true;
            this.combo_source.Location = new System.Drawing.Point(87, 3);
            this.combo_source.Name = "combo_source";
            this.combo_source.Size = new System.Drawing.Size(336, 32);
            this.combo_source.TabIndex = 0;
            this.combo_source.Enter += new System.EventHandler(this.Combo_Any_Enter_Leave);
            this.combo_source.Leave += new System.EventHandler(this.Combo_Any_Enter_Leave);
            // 
            // label_source
            // 
            this.label_source.AutoSize = true;
            this.label_source.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_source.Location = new System.Drawing.Point(3, 0);
            this.label_source.Name = "label_source";
            this.label_source.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label_source.Size = new System.Drawing.Size(78, 29);
            this.label_source.TabIndex = 1;
            this.label_source.Text = "Source:";
            // 
            // button_browse_source
            // 
            this.button_browse_source.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_browse_source.Location = new System.Drawing.Point(429, 3);
            this.button_browse_source.Name = "button_browse_source";
            this.button_browse_source.Size = new System.Drawing.Size(44, 34);
            this.button_browse_source.TabIndex = 6;
            this.button_browse_source.Text = "...";
            this.button_browse_source.UseVisualStyleBackColor = true;
            this.button_browse_source.Click += new System.EventHandler(this.Button_browse_source_Click);
            // 
            // Group_ProcessingView
            // 
            this.Group_ProcessingView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Group_ProcessingView.Controls.Add(this.listView1);
            this.Group_ProcessingView.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Group_ProcessingView.Location = new System.Drawing.Point(528, 17);
            this.Group_ProcessingView.Name = "Group_ProcessingView";
            this.Group_ProcessingView.Size = new System.Drawing.Size(503, 632);
            this.Group_ProcessingView.TabIndex = 10;
            this.Group_ProcessingView.TabStop = false;
            this.Group_ProcessingView.Text = "Processing View";
            // 
            // Group_TotalProgress
            // 
            this.Group_TotalProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Group_TotalProgress.Controls.Add(this.tableLayoutPanel4);
            this.Group_TotalProgress.Controls.Add(this.progress_batch);
            this.Group_TotalProgress.Controls.Add(this.tableLayoutPanel1);
            this.Group_TotalProgress.Controls.Add(this.progress_total);
            this.Group_TotalProgress.Location = new System.Drawing.Point(13, 445);
            this.Group_TotalProgress.Name = "Group_TotalProgress";
            this.Group_TotalProgress.Size = new System.Drawing.Size(501, 204);
            this.Group_TotalProgress.TabIndex = 11;
            this.Group_TotalProgress.TabStop = false;
            this.Group_TotalProgress.Text = "Progress";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label_progress_value2A, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_progress_status2A, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_progress_value1A, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_progress_status1A, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(16, 71);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(467, 45);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // label_progress_value2A
            // 
            this.label_progress_value2A.AutoSize = true;
            this.label_progress_value2A.Location = new System.Drawing.Point(96, 24);
            this.label_progress_value2A.Name = "label_progress_value2A";
            this.label_progress_value2A.Size = new System.Drawing.Size(77, 24);
            this.label_progress_value2A.TabIndex = 7;
            this.label_progress_value2A.Text = "Value 2A";
            this.label_progress_value2A.Visible = false;
            // 
            // label_progress_status2A
            // 
            this.label_progress_status2A.AutoSize = true;
            this.label_progress_status2A.Location = new System.Drawing.Point(3, 24);
            this.label_progress_status2A.Name = "label_progress_status2A";
            this.label_progress_status2A.Size = new System.Drawing.Size(87, 24);
            this.label_progress_status2A.TabIndex = 6;
            this.label_progress_status2A.Text = "Status 2A:";
            this.label_progress_status2A.Visible = false;
            // 
            // label_progress_value1A
            // 
            this.label_progress_value1A.AutoSize = true;
            this.label_progress_value1A.Location = new System.Drawing.Point(96, 0);
            this.label_progress_value1A.Name = "label_progress_value1A";
            this.label_progress_value1A.Size = new System.Drawing.Size(74, 24);
            this.label_progress_value1A.TabIndex = 4;
            this.label_progress_value1A.Text = "Value 1A";
            this.label_progress_value1A.Visible = false;
            // 
            // label_progress_status1A
            // 
            this.label_progress_status1A.AutoSize = true;
            this.label_progress_status1A.Location = new System.Drawing.Point(3, 0);
            this.label_progress_status1A.Name = "label_progress_status1A";
            this.label_progress_status1A.Size = new System.Drawing.Size(84, 24);
            this.label_progress_status1A.TabIndex = 5;
            this.label_progress_status1A.Text = "Status 1A:";
            this.label_progress_status1A.Visible = false;
            // 
            // Group_Settings
            // 
            this.Group_Settings.Controls.Add(this.tableLayoutPanel3);
            this.Group_Settings.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Group_Settings.Location = new System.Drawing.Point(14, 195);
            this.Group_Settings.Name = "Group_Settings";
            this.Group_Settings.Size = new System.Drawing.Size(499, 244);
            this.Group_Settings.TabIndex = 12;
            this.Group_Settings.TabStop = false;
            this.Group_Settings.Text = "Settings";
            // 
            // progress_batch
            // 
            this.progress_batch.Location = new System.Drawing.Point(16, 122);
            this.progress_batch.Name = "progress_batch";
            this.progress_batch.Size = new System.Drawing.Size(467, 36);
            this.progress_batch.TabIndex = 6;
            this.progress_batch.Visible = false;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.label_batch_value, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label_batch_status, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(16, 161);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(467, 37);
            this.tableLayoutPanel4.TabIndex = 8;
            // 
            // label_batch_value
            // 
            this.label_batch_value.AutoSize = true;
            this.label_batch_value.Location = new System.Drawing.Point(133, 0);
            this.label_batch_value.Name = "label_batch_value";
            this.label_batch_value.Size = new System.Drawing.Size(47, 24);
            this.label_batch_value.TabIndex = 4;
            this.label_batch_value.Text = "5/85";
            this.label_batch_value.Visible = false;
            // 
            // label_batch_status
            // 
            this.label_batch_status.AutoSize = true;
            this.label_batch_status.Location = new System.Drawing.Point(3, 0);
            this.label_batch_status.Name = "label_batch_status";
            this.label_batch_status.Size = new System.Drawing.Size(124, 24);
            this.label_batch_status.TabIndex = 5;
            this.label_batch_status.Text = "Current Batch:";
            this.label_batch_status.Visible = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 799);
            this.Controls.Add(this.Group_Settings);
            this.Controls.Add(this.Group_TotalProgress);
            this.Controls.Add(this.Group_ProcessingView);
            this.Controls.Add(this.Group_Paths);
            this.Controls.Add(this.LayoutPanel_LowerButtons);
            this.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Project Lykos - Lip Generation Tool";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.LayoutPanel_LowerButtons.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.Group_Paths.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.Group_ProcessingView.ResumeLayout(false);
            this.Group_TotalProgress.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.Group_Settings.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Button button_preview;
        private TableLayoutPanel LayoutPanel_LowerButtons;
        private Button button_start_batch;
        private Button button3;
        private Button button_stop_batch;
        private ListView listView1;
        private ProgressBar progress_total;
        private TableLayoutPanel tableLayoutPanel3;
        private GroupBox Group_Paths;
        private GroupBox Group_ProcessingView;
        private GroupBox Group_TotalProgress;
        private GroupBox Group_Settings;
        private ComboBox combo_audio_preprocessing;
        private Label label1;
        private ComboBox combo_multiprocess_count;
        private Label label_multiprocess_count;
        private TableLayoutPanel tableLayoutPanel2;
        private ComboBox combo_output;
        private Label label2;
        private ComboBox combo_source;
        private Label label_source;
        private ComboBox combo_csv;
        private Label label3;
        private Button button_browse_csv;
        private Button button_browse_output;
        private Button button_browse_source;
        private Label label_progress_value1A;
        private ComboBox combo_csvDelimiter;
        private Label label_csvDelimiter;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label_progress_status1A;
        private Label label_progress_value2A;
        private Label label_progress_status2A;
        private ComboBox combo_outputActions;
        private Label label_outputActions;
        private TableLayoutPanel tableLayoutPanel4;
        private Label label_batch_value;
        private Label label_batch_status;
        private ProgressBar progress_batch;
    }
}
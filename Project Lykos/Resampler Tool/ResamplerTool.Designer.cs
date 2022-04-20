namespace Project_Lykos.Resampler_Tool
{
    partial class ResamplerTool
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
            this.button_browse_output = new System.Windows.Forms.Button();
            this.combo_output = new System.Windows.Forms.ComboBox();
            this.label_output = new System.Windows.Forms.Label();
            this.combo_source = new System.Windows.Forms.ComboBox();
            this.label_source = new System.Windows.Forms.Label();
            this.button_browse_source = new System.Windows.Forms.Button();
            this.group_options = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.combo_channels = new System.Windows.Forms.ComboBox();
            this.label_channels = new System.Windows.Forms.Label();
            this.combo_sampling_rate = new System.Windows.Forms.ComboBox();
            this.label_samplingRate = new System.Windows.Forms.Label();
            this.button_start = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_est = new System.Windows.Forms.Label();
            this.Group_Paths.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.group_options.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.Group_Paths.Size = new System.Drawing.Size(499, 120);
            this.Group_Paths.TabIndex = 10;
            this.Group_Paths.TabStop = false;
            this.Group_Paths.Text = "Paths";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.Controls.Add(this.button_browse_output, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.combo_output, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label_output, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.combo_source, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label_source, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.button_browse_source, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(9, 30);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(476, 82);
            this.tableLayoutPanel2.TabIndex = 9;
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
            this.button_browse_output.Click += new System.EventHandler(this.button_browse_output_Click);
            // 
            // combo_output
            // 
            this.combo_output.Dock = System.Windows.Forms.DockStyle.Top;
            this.combo_output.FormattingEnabled = true;
            this.combo_output.Location = new System.Drawing.Point(79, 43);
            this.combo_output.Name = "combo_output";
            this.combo_output.Size = new System.Drawing.Size(344, 32);
            this.combo_output.TabIndex = 3;
            this.combo_output.Enter += new System.EventHandler(this.Combo_Any_Enter_Leave);
            // 
            // label_output
            // 
            this.label_output.AutoSize = true;
            this.label_output.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_output.Location = new System.Drawing.Point(3, 40);
            this.label_output.Name = "label_output";
            this.label_output.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label_output.Size = new System.Drawing.Size(70, 29);
            this.label_output.TabIndex = 2;
            this.label_output.Text = "Output:";
            // 
            // combo_source
            // 
            this.combo_source.Dock = System.Windows.Forms.DockStyle.Top;
            this.combo_source.FormattingEnabled = true;
            this.combo_source.Location = new System.Drawing.Point(79, 3);
            this.combo_source.Name = "combo_source";
            this.combo_source.Size = new System.Drawing.Size(344, 32);
            this.combo_source.TabIndex = 0;
            this.combo_source.Enter += new System.EventHandler(this.Combo_Any_Enter_Leave);
            // 
            // label_source
            // 
            this.label_source.AutoSize = true;
            this.label_source.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_source.Location = new System.Drawing.Point(3, 0);
            this.label_source.Name = "label_source";
            this.label_source.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label_source.Size = new System.Drawing.Size(70, 29);
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
            this.button_browse_source.Click += new System.EventHandler(this.button_browse_source_Click);
            // 
            // group_options
            // 
            this.group_options.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.group_options.Controls.Add(this.tableLayoutPanel1);
            this.group_options.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.group_options.Location = new System.Drawing.Point(12, 138);
            this.group_options.Name = "group_options";
            this.group_options.Size = new System.Drawing.Size(499, 120);
            this.group_options.TabIndex = 11;
            this.group_options.TabStop = false;
            this.group_options.Text = "Options";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.combo_channels, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_channels, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.combo_sampling_rate, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_samplingRate, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 30);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(476, 82);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // combo_channels
            // 
            this.combo_channels.Dock = System.Windows.Forms.DockStyle.Top;
            this.combo_channels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_channels.FormattingEnabled = true;
            this.combo_channels.Items.AddRange(new object[] {
            "Same as source",
            "1 (Mono)",
            "2 (Stereo)"});
            this.combo_channels.Location = new System.Drawing.Point(136, 41);
            this.combo_channels.Name = "combo_channels";
            this.combo_channels.Size = new System.Drawing.Size(337, 32);
            this.combo_channels.TabIndex = 3;
            // 
            // label_channels
            // 
            this.label_channels.AutoSize = true;
            this.label_channels.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_channels.Location = new System.Drawing.Point(3, 38);
            this.label_channels.Name = "label_channels";
            this.label_channels.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label_channels.Size = new System.Drawing.Size(127, 29);
            this.label_channels.TabIndex = 2;
            this.label_channels.Text = "Channels:";
            // 
            // combo_sampling_rate
            // 
            this.combo_sampling_rate.Dock = System.Windows.Forms.DockStyle.Top;
            this.combo_sampling_rate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_sampling_rate.FormattingEnabled = true;
            this.combo_sampling_rate.Items.AddRange(new object[] {
            "22050",
            "32000",
            "44100",
            "48000",
            "88200",
            "96000",
            "176400",
            "192000"});
            this.combo_sampling_rate.Location = new System.Drawing.Point(136, 3);
            this.combo_sampling_rate.Name = "combo_sampling_rate";
            this.combo_sampling_rate.Size = new System.Drawing.Size(337, 32);
            this.combo_sampling_rate.TabIndex = 0;
            // 
            // label_samplingRate
            // 
            this.label_samplingRate.AutoSize = true;
            this.label_samplingRate.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_samplingRate.Location = new System.Drawing.Point(3, 0);
            this.label_samplingRate.Name = "label_samplingRate";
            this.label_samplingRate.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label_samplingRate.Size = new System.Drawing.Size(127, 29);
            this.label_samplingRate.TabIndex = 1;
            this.label_samplingRate.Text = "Sampling Rate:";
            // 
            // button_start
            // 
            this.button_start.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_start.Location = new System.Drawing.Point(12, 378);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(499, 44);
            this.button_start.TabIndex = 12;
            this.button_start.Text = "Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 31);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(473, 34);
            this.progressBar1.TabIndex = 13;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label_est);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox1.Location = new System.Drawing.Point(12, 264);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(499, 108);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Progress";
            // 
            // label_est
            // 
            this.label_est.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_est.Location = new System.Drawing.Point(12, 68);
            this.label_est.Name = "label_est";
            this.label_est.Size = new System.Drawing.Size(473, 37);
            this.label_est.TabIndex = 14;
            this.label_est.Text = "Time Remaining: 25 minutes";
            this.label_est.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_est.Visible = false;
            // 
            // ResamplerTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 434);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.group_options);
            this.Controls.Add(this.Group_Paths);
            this.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "ResamplerTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ResamplerTool";
            this.Group_Paths.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.group_options.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox Group_Paths;
        private TableLayoutPanel tableLayoutPanel2;
        private Button button_browse_output;
        private ComboBox combo_output;
        private Label label_output;
        private ComboBox combo_source;
        private Label label_source;
        private Button button_browse_source;
        private GroupBox group_options;
        private TableLayoutPanel tableLayoutPanel1;
        private ComboBox combo_channels;
        private Label label_channels;
        private ComboBox combo_sampling_rate;
        private Label label_samplingRate;
        private Button button_start;
        private ProgressBar progressBar1;
        private GroupBox groupBox1;
        private Label label_est;
    }
}
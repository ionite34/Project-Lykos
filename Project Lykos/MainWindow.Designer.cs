﻿namespace Project_Lykos
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
            this.comboBox_multiprocess_count = new System.Windows.Forms.ComboBox();
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
            this.Group_Settings = new System.Windows.Forms.GroupBox();
            this.LayoutPanel_LowerButtons.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.Group_Paths.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.Group_ProcessingView.SuspendLayout();
            this.Group_TotalProgress.SuspendLayout();
            this.Group_Settings.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_preview
            // 
            this.button_preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_preview.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button_preview.Location = new System.Drawing.Point(13, 13);
            this.button_preview.Name = "button_preview";
            this.button_preview.Size = new System.Drawing.Size(509, 49);
            this.button_preview.TabIndex = 0;
            this.button_preview.Text = "Preview Indexed Operations";
            this.button_preview.UseVisualStyleBackColor = true;
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
            this.LayoutPanel_LowerButtons.Location = new System.Drawing.Point(0, 540);
            this.LayoutPanel_LowerButtons.Name = "LayoutPanel_LowerButtons";
            this.LayoutPanel_LowerButtons.Padding = new System.Windows.Forms.Padding(10, 10, 10, 24);
            this.LayoutPanel_LowerButtons.RowCount = 2;
            this.LayoutPanel_LowerButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.LayoutPanel_LowerButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.LayoutPanel_LowerButtons.Size = new System.Drawing.Size(1050, 144);
            this.LayoutPanel_LowerButtons.TabIndex = 1;
            // 
            // button_start_batch
            // 
            this.button_start_batch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_start_batch.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button_start_batch.Location = new System.Drawing.Point(528, 13);
            this.button_start_batch.Name = "button_start_batch";
            this.button_start_batch.Size = new System.Drawing.Size(509, 49);
            this.button_start_batch.TabIndex = 1;
            this.button_start_batch.Text = "Start Batch";
            this.button_start_batch.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button3.Location = new System.Drawing.Point(13, 68);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(509, 49);
            this.button3.TabIndex = 2;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button_stop_batch
            // 
            this.button_stop_batch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_stop_batch.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button_stop_batch.Location = new System.Drawing.Point(528, 68);
            this.button_stop_batch.Name = "button_stop_batch";
            this.button_stop_batch.Size = new System.Drawing.Size(509, 49);
            this.button_stop_batch.TabIndex = 3;
            this.button_stop_batch.Text = "Stop Batch";
            this.button_stop_batch.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Location = new System.Drawing.Point(6, 32);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(497, 480);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
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
            this.tableLayoutPanel3.Controls.Add(this.comboBox_multiprocess_count, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label_multiprocess_count, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.combo_audio_preprocessing, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(9, 29);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(476, 193);
            this.tableLayoutPanel3.TabIndex = 8;
            // 
            // comboBox_multiprocess_count
            // 
            this.comboBox_multiprocess_count.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBox_multiprocess_count.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_multiprocess_count.FormattingEnabled = true;
            this.comboBox_multiprocess_count.Items.AddRange(new object[] {
            "None (Not Recommended)",
            "Resampled (Standard)",
            "Smoothed (Safest)"});
            this.comboBox_multiprocess_count.Location = new System.Drawing.Point(188, 41);
            this.comboBox_multiprocess_count.Name = "comboBox_multiprocess_count";
            this.comboBox_multiprocess_count.Size = new System.Drawing.Size(285, 32);
            this.comboBox_multiprocess_count.TabIndex = 3;
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
            this.label_multiprocess_count.Text = "Multithreading:";
            // 
            // combo_audio_preprocessing
            // 
            this.combo_audio_preprocessing.Dock = System.Windows.Forms.DockStyle.Top;
            this.combo_audio_preprocessing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_audio_preprocessing.FormattingEnabled = true;
            this.combo_audio_preprocessing.Items.AddRange(new object[] {
            "None (Not Recommended)",
            "Resampled (Standard)",
            "Smoothed (Safest)"});
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
            this.Group_ProcessingView.Controls.Add(this.listView1);
            this.Group_ProcessingView.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Group_ProcessingView.Location = new System.Drawing.Point(528, 17);
            this.Group_ProcessingView.Name = "Group_ProcessingView";
            this.Group_ProcessingView.Size = new System.Drawing.Size(509, 517);
            this.Group_ProcessingView.TabIndex = 10;
            this.Group_ProcessingView.TabStop = false;
            this.Group_ProcessingView.Text = "Processing View";
            // 
            // Group_TotalProgress
            // 
            this.Group_TotalProgress.Controls.Add(this.progress_total);
            this.Group_TotalProgress.Location = new System.Drawing.Point(13, 445);
            this.Group_TotalProgress.Name = "Group_TotalProgress";
            this.Group_TotalProgress.Size = new System.Drawing.Size(500, 90);
            this.Group_TotalProgress.TabIndex = 11;
            this.Group_TotalProgress.TabStop = false;
            this.Group_TotalProgress.Text = "Total Progress";
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
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 684);
            this.Controls.Add(this.Group_Settings);
            this.Controls.Add(this.Group_TotalProgress);
            this.Controls.Add(this.Group_ProcessingView);
            this.Controls.Add(this.Group_Paths);
            this.Controls.Add(this.LayoutPanel_LowerButtons);
            this.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
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
            this.Group_Settings.ResumeLayout(false);
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
        private ComboBox comboBox_multiprocess_count;
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
    }
}
namespace WAV_CUDA
{
	partial class Window_Main
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
			label_status = new Label();
			listBox_devices = new ListBox();
			groupBox_device = new GroupBox();
			button_resetContext = new Button();
			groupBox_pointers = new GroupBox();
			listBox_pointers = new ListBox();
			groupBox_files = new GroupBox();
			button_moveAll = new Button();
			button_import = new Button();
			button_move = new Button();
			listBox_files = new ListBox();
			button_clearFiles = new Button();
			button_clearPointers = new Button();
			waveViewer_track = new NAudio.Gui.WaveViewer();
			waveViewer_pointer = new NAudio.Gui.WaveViewer();
			label_file = new Label();
			label_pointer = new Label();
			label_pointerBpm = new Label();
			label_fileBpm = new Label();
			groupBox_waves = new GroupBox();
			button_pfile = new Button();
			button_ppointer = new Button();
			numericUpDown_threshold = new NumericUpDown();
			numericUpDown_cutoff = new NumericUpDown();
			button_export = new Button();
			button_stretch = new Button();
			numericUpDown_bpm = new NumericUpDown();
			button_scanBpm = new Button();
			pictureBox_cover = new PictureBox();
			pictureBox_banner = new PictureBox();
			groupBox_device.SuspendLayout();
			groupBox_pointers.SuspendLayout();
			groupBox_files.SuspendLayout();
			groupBox_waves.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) numericUpDown_threshold).BeginInit();
			((System.ComponentModel.ISupportInitialize) numericUpDown_cutoff).BeginInit();
			((System.ComponentModel.ISupportInitialize) numericUpDown_bpm).BeginInit();
			((System.ComponentModel.ISupportInitialize) pictureBox_cover).BeginInit();
			((System.ComponentModel.ISupportInitialize) pictureBox_banner).BeginInit();
			SuspendLayout();
			// 
			// label_status
			// 
			label_status.AutoSize = true;
			label_status.ForeColor = SystemColors.HotTrack;
			label_status.Location = new Point(6, 19);
			label_status.Name = "label_status";
			label_status.Size = new Size(137, 15);
			label_status.TabIndex = 1;
			label_status.Text = "Initialize CUDA-Device ...";
			label_status.Click += label_status_Click;
			// 
			// listBox_devices
			// 
			listBox_devices.FormattingEnabled = true;
			listBox_devices.ItemHeight = 15;
			listBox_devices.Location = new Point(6, 37);
			listBox_devices.Name = "listBox_devices";
			listBox_devices.Size = new Size(224, 124);
			listBox_devices.TabIndex = 2;
			// 
			// groupBox_device
			// 
			groupBox_device.BackColor = Color.Transparent;
			groupBox_device.Controls.Add(button_resetContext);
			groupBox_device.Controls.Add(label_status);
			groupBox_device.Controls.Add(listBox_devices);
			groupBox_device.FlatStyle = FlatStyle.Flat;
			groupBox_device.Location = new Point(12, 472);
			groupBox_device.Name = "groupBox_device";
			groupBox_device.Size = new Size(236, 197);
			groupBox_device.TabIndex = 3;
			groupBox_device.TabStop = false;
			groupBox_device.Text = "Device";
			// 
			// button_resetContext
			// 
			button_resetContext.Location = new Point(6, 168);
			button_resetContext.Name = "button_resetContext";
			button_resetContext.Size = new Size(97, 23);
			button_resetContext.TabIndex = 12;
			button_resetContext.Text = "Reset Context";
			button_resetContext.UseVisualStyleBackColor = true;
			button_resetContext.Click += button_resetContext_Click;
			// 
			// groupBox_pointers
			// 
			groupBox_pointers.BackColor = Color.Transparent;
			groupBox_pointers.Controls.Add(listBox_pointers);
			groupBox_pointers.FlatStyle = FlatStyle.Flat;
			groupBox_pointers.Location = new Point(12, 120);
			groupBox_pointers.Name = "groupBox_pointers";
			groupBox_pointers.Size = new Size(236, 346);
			groupBox_pointers.TabIndex = 4;
			groupBox_pointers.TabStop = false;
			groupBox_pointers.Text = "Pointers";
			// 
			// listBox_pointers
			// 
			listBox_pointers.FormattingEnabled = true;
			listBox_pointers.ItemHeight = 15;
			listBox_pointers.Location = new Point(6, 22);
			listBox_pointers.Name = "listBox_pointers";
			listBox_pointers.Size = new Size(224, 319);
			listBox_pointers.TabIndex = 5;
			listBox_pointers.SelectedIndexChanged += listBox_pointers_SelectedIndexChanged;
			// 
			// groupBox_files
			// 
			groupBox_files.BackColor = Color.Transparent;
			groupBox_files.Controls.Add(button_moveAll);
			groupBox_files.Controls.Add(button_import);
			groupBox_files.Controls.Add(button_move);
			groupBox_files.Controls.Add(listBox_files);
			groupBox_files.Controls.Add(button_clearFiles);
			groupBox_files.Controls.Add(button_clearPointers);
			groupBox_files.FlatStyle = FlatStyle.Flat;
			groupBox_files.Location = new Point(254, 120);
			groupBox_files.Name = "groupBox_files";
			groupBox_files.Size = new Size(438, 255);
			groupBox_files.TabIndex = 6;
			groupBox_files.TabStop = false;
			groupBox_files.Text = "Files";
			// 
			// button_moveAll
			// 
			button_moveAll.Location = new Point(335, 124);
			button_moveAll.Name = "button_moveAll";
			button_moveAll.Size = new Size(97, 23);
			button_moveAll.TabIndex = 12;
			button_moveAll.Text = "Move all";
			button_moveAll.UseVisualStyleBackColor = true;
			button_moveAll.Click += button_moveAll_Click;
			// 
			// button_import
			// 
			button_import.Location = new Point(335, 22);
			button_import.Name = "button_import";
			button_import.Size = new Size(97, 23);
			button_import.TabIndex = 7;
			button_import.Text = "Import file(s)";
			button_import.UseVisualStyleBackColor = true;
			button_import.Click += button_import_Click;
			// 
			// button_move
			// 
			button_move.Location = new Point(335, 95);
			button_move.Name = "button_move";
			button_move.Size = new Size(97, 23);
			button_move.TabIndex = 10;
			button_move.Text = "Move to device";
			button_move.UseVisualStyleBackColor = true;
			button_move.Click += button_move_Click;
			// 
			// listBox_files
			// 
			listBox_files.FormattingEnabled = true;
			listBox_files.ItemHeight = 15;
			listBox_files.Location = new Point(6, 22);
			listBox_files.Name = "listBox_files";
			listBox_files.Size = new Size(323, 229);
			listBox_files.TabIndex = 5;
			listBox_files.SelectedIndexChanged += listBox_files_SelectedIndexChanged;
			// 
			// button_clearFiles
			// 
			button_clearFiles.Location = new Point(335, 226);
			button_clearFiles.Name = "button_clearFiles";
			button_clearFiles.Size = new Size(97, 23);
			button_clearFiles.TabIndex = 8;
			button_clearFiles.Text = "Clear files";
			button_clearFiles.UseVisualStyleBackColor = true;
			button_clearFiles.Click += button_clearFiles_Click;
			// 
			// button_clearPointers
			// 
			button_clearPointers.Location = new Point(335, 197);
			button_clearPointers.Name = "button_clearPointers";
			button_clearPointers.Size = new Size(97, 23);
			button_clearPointers.TabIndex = 9;
			button_clearPointers.Text = "Clear pointers";
			button_clearPointers.UseVisualStyleBackColor = true;
			button_clearPointers.Click += button_clearPointers_Click;
			// 
			// waveViewer_track
			// 
			waveViewer_track.Location = new Point(8, 168);
			waveViewer_track.Name = "waveViewer_track";
			waveViewer_track.SamplesPerPixel = 128;
			waveViewer_track.Size = new Size(410, 31);
			waveViewer_track.StartPosition =  0L;
			waveViewer_track.TabIndex = 5;
			waveViewer_track.WaveStream = null;
			// 
			// waveViewer_pointer
			// 
			waveViewer_pointer.Location = new Point(6, 110);
			waveViewer_pointer.Name = "waveViewer_pointer";
			waveViewer_pointer.SamplesPerPixel = 128;
			waveViewer_pointer.Size = new Size(420, 30);
			waveViewer_pointer.StartPosition =  0L;
			waveViewer_pointer.TabIndex = 6;
			waveViewer_pointer.WaveStream = null;
			// 
			// label_file
			// 
			label_file.AutoSize = true;
			label_file.Location = new Point(37, 150);
			label_file.Name = "label_file";
			label_file.Size = new Size(57, 15);
			label_file.TabIndex = 7;
			label_file.Text = "From file:";
			// 
			// label_pointer
			// 
			label_pointer.AutoSize = true;
			label_pointer.Location = new Point(37, 87);
			label_pointer.Name = "label_pointer";
			label_pointer.Size = new Size(79, 15);
			label_pointer.TabIndex = 8;
			label_pointer.Text = "From pointer:";
			// 
			// label_pointerBpm
			// 
			label_pointerBpm.AutoSize = true;
			label_pointerBpm.Location = new Point(345, 92);
			label_pointerBpm.Name = "label_pointerBpm";
			label_pointerBpm.Size = new Size(73, 15);
			label_pointerBpm.TabIndex = 9;
			label_pointerBpm.Text = "Pointer BPM";
			// 
			// label_fileBpm
			// 
			label_fileBpm.AutoSize = true;
			label_fileBpm.Location = new Point(365, 150);
			label_fileBpm.Name = "label_fileBpm";
			label_fileBpm.Size = new Size(53, 15);
			label_fileBpm.TabIndex = 10;
			label_fileBpm.Text = "File BPM";
			// 
			// groupBox_waves
			// 
			groupBox_waves.BackColor = Color.Transparent;
			groupBox_waves.Controls.Add(button_pfile);
			groupBox_waves.Controls.Add(button_ppointer);
			groupBox_waves.Controls.Add(numericUpDown_threshold);
			groupBox_waves.Controls.Add(numericUpDown_cutoff);
			groupBox_waves.Controls.Add(label_fileBpm);
			groupBox_waves.Controls.Add(label_pointerBpm);
			groupBox_waves.Controls.Add(button_export);
			groupBox_waves.Controls.Add(button_stretch);
			groupBox_waves.Controls.Add(numericUpDown_bpm);
			groupBox_waves.Controls.Add(button_scanBpm);
			groupBox_waves.Controls.Add(label_pointer);
			groupBox_waves.Controls.Add(label_file);
			groupBox_waves.Controls.Add(waveViewer_pointer);
			groupBox_waves.Controls.Add(waveViewer_track);
			groupBox_waves.FlatStyle = FlatStyle.Flat;
			groupBox_waves.Location = new Point(260, 381);
			groupBox_waves.Name = "groupBox_waves";
			groupBox_waves.Size = new Size(432, 288);
			groupBox_waves.TabIndex = 11;
			groupBox_waves.TabStop = false;
			groupBox_waves.Text = "Scan BPM";
			// 
			// button_pfile
			// 
			button_pfile.Location = new Point(8, 146);
			button_pfile.Name = "button_pfile";
			button_pfile.Size = new Size(23, 23);
			button_pfile.TabIndex = 19;
			button_pfile.Text = ">";
			button_pfile.UseVisualStyleBackColor = true;
			button_pfile.Click += button_pfile_Click;
			// 
			// button_ppointer
			// 
			button_ppointer.Location = new Point(8, 83);
			button_ppointer.Name = "button_ppointer";
			button_ppointer.Size = new Size(23, 23);
			button_ppointer.TabIndex = 12;
			button_ppointer.Text = ">";
			button_ppointer.UseVisualStyleBackColor = true;
			button_ppointer.Click += button_ppointer_Click;
			// 
			// numericUpDown_threshold
			// 
			numericUpDown_threshold.DecimalPlaces = 2;
			numericUpDown_threshold.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
			numericUpDown_threshold.Location = new Point(69, 51);
			numericUpDown_threshold.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
			numericUpDown_threshold.Name = "numericUpDown_threshold";
			numericUpDown_threshold.Size = new Size(55, 23);
			numericUpDown_threshold.TabIndex = 18;
			numericUpDown_threshold.Value = new decimal(new int[] { 5, 0, 0, 65536 });
			// 
			// numericUpDown_cutoff
			// 
			numericUpDown_cutoff.DecimalPlaces = 1;
			numericUpDown_cutoff.Location = new Point(8, 51);
			numericUpDown_cutoff.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
			numericUpDown_cutoff.Name = "numericUpDown_cutoff";
			numericUpDown_cutoff.Size = new Size(55, 23);
			numericUpDown_cutoff.TabIndex = 17;
			numericUpDown_cutoff.Value = new decimal(new int[] { 150, 0, 0, 0 });
			// 
			// button_export
			// 
			button_export.Location = new Point(329, 22);
			button_export.Name = "button_export";
			button_export.Size = new Size(97, 23);
			button_export.TabIndex = 16;
			button_export.Text = "Export pointer";
			button_export.UseVisualStyleBackColor = true;
			button_export.Click += button_export_Click;
			// 
			// button_stretch
			// 
			button_stretch.Location = new Point(170, 22);
			button_stretch.Name = "button_stretch";
			button_stretch.Size = new Size(97, 23);
			button_stretch.TabIndex = 15;
			button_stretch.Text = "Stretch pointer";
			button_stretch.UseVisualStyleBackColor = true;
			button_stretch.Click += button_stretch_Click;
			// 
			// numericUpDown_bpm
			// 
			numericUpDown_bpm.Location = new Point(109, 22);
			numericUpDown_bpm.Maximum = new decimal(new int[] { 240, 0, 0, 0 });
			numericUpDown_bpm.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
			numericUpDown_bpm.Name = "numericUpDown_bpm";
			numericUpDown_bpm.Size = new Size(55, 23);
			numericUpDown_bpm.TabIndex = 14;
			numericUpDown_bpm.Value = new decimal(new int[] { 180, 0, 0, 0 });
			// 
			// button_scanBpm
			// 
			button_scanBpm.Location = new Point(6, 22);
			button_scanBpm.Name = "button_scanBpm";
			button_scanBpm.Size = new Size(97, 23);
			button_scanBpm.TabIndex = 13;
			button_scanBpm.Text = "Scan BPM";
			button_scanBpm.UseVisualStyleBackColor = true;
			button_scanBpm.Click += button_scanBpm_Click;
			// 
			// pictureBox_cover
			// 
			pictureBox_cover.Location = new Point(589, 12);
			pictureBox_cover.Name = "pictureBox_cover";
			pictureBox_cover.Size = new Size(102, 102);
			pictureBox_cover.TabIndex = 12;
			pictureBox_cover.TabStop = false;
			// 
			// pictureBox_banner
			// 
			pictureBox_banner.Location = new Point(18, 12);
			pictureBox_banner.Name = "pictureBox_banner";
			pictureBox_banner.Size = new Size(565, 50);
			pictureBox_banner.TabIndex = 13;
			pictureBox_banner.TabStop = false;
			// 
			// Window_Main
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(704, 681);
			Controls.Add(pictureBox_banner);
			Controls.Add(pictureBox_cover);
			Controls.Add(groupBox_waves);
			Controls.Add(groupBox_files);
			Controls.Add(groupBox_pointers);
			Controls.Add(groupBox_device);
			MaximizeBox = false;
			MaximumSize = new Size(720, 720);
			MinimumSize = new Size(720, 720);
			Name = "Window_Main";
			Text = "CUDA WAV Batch-Manipulation";
			groupBox_device.ResumeLayout(false);
			groupBox_device.PerformLayout();
			groupBox_pointers.ResumeLayout(false);
			groupBox_files.ResumeLayout(false);
			groupBox_waves.ResumeLayout(false);
			groupBox_waves.PerformLayout();
			((System.ComponentModel.ISupportInitialize) numericUpDown_threshold).EndInit();
			((System.ComponentModel.ISupportInitialize) numericUpDown_cutoff).EndInit();
			((System.ComponentModel.ISupportInitialize) numericUpDown_bpm).EndInit();
			((System.ComponentModel.ISupportInitialize) pictureBox_cover).EndInit();
			((System.ComponentModel.ISupportInitialize) pictureBox_banner).EndInit();
			ResumeLayout(false);
		}

		#endregion
		private Label label_status;
		private ListBox listBox_devices;
		private GroupBox groupBox_device;
		private GroupBox groupBox_pointers;
		private ListBox listBox_pointers;
		private GroupBox groupBox_files;
		private ListBox listBox_files;
		private Button button_import;
		private Button button_clearFiles;
		private Button button_clearPointers;
		private Button button_move;
		private Button button_resetContext;
		private Button button_moveAll;
		private NAudio.Gui.WaveViewer waveViewer_track;
		private NAudio.Gui.WaveViewer waveViewer_pointer;
		private Label label_file;
		private Label label_pointer;
		private Label label_pointerBpm;
		private Label label_fileBpm;
		private GroupBox groupBox_waves;
		private Button button_export;
		private Button button_stretch;
		private NumericUpDown numericUpDown_bpm;
		private Button button_scanBpm;
		private NumericUpDown numericUpDown_threshold;
		private NumericUpDown numericUpDown_cutoff;
		private Button button_ppointer;
		private Button button_pfile;
		private PictureBox pictureBox_cover;
		private PictureBox pictureBox_banner;
	}
}

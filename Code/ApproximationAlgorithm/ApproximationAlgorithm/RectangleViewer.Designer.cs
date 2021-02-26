namespace ApproximationAlgorithm
{
    partial class RectangleViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.zoomTrackBar = new System.Windows.Forms.TrackBar();
            this.zoomLabel = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // zoomTrackBar
            // 
            this.zoomTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.zoomTrackBar.AutoSize = false;
            this.zoomTrackBar.LargeChange = 10;
            this.zoomTrackBar.Location = new System.Drawing.Point(6, 20);
            this.zoomTrackBar.Margin = new System.Windows.Forms.Padding(0);
            this.zoomTrackBar.Maximum = 200;
            this.zoomTrackBar.Minimum = 10;
            this.zoomTrackBar.Name = "zoomTrackBar";
            this.zoomTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.zoomTrackBar.Size = new System.Drawing.Size(45, 302);
            this.zoomTrackBar.SmallChange = 10;
            this.zoomTrackBar.TabIndex = 1;
            this.zoomTrackBar.TickFrequency = 10;
            this.zoomTrackBar.Value = 100;
            // 
            // zoomLabel
            // 
            this.zoomLabel.Location = new System.Drawing.Point(6, 4);
            this.zoomLabel.Name = "zoomLabel";
            this.zoomLabel.Size = new System.Drawing.Size(45, 13);
            this.zoomLabel.TabIndex = 2;
            this.zoomLabel.Text = "100";
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.Location = new System.Drawing.Point(42, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(644, 326);
            this.pictureBox.TabIndex = 3;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
            // 
            // RectangleViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1, 1);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.zoomLabel);
            this.Controls.Add(this.zoomTrackBar);
            this.Name = "RectangleViewer";
            this.Size = new System.Drawing.Size(686, 326);
            ((System.ComponentModel.ISupportInitialize)(this.zoomTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label zoomLabel;
        private System.Windows.Forms.TrackBar zoomTrackBar;
        private System.Windows.Forms.PictureBox pictureBox;
    }
}

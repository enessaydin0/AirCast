namespace AirCast
{
    partial class grafikEkrani
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
            WeatherPictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)WeatherPictureBox).BeginInit();
            SuspendLayout();
            // 
            // WeatherPictureBox
            // 
            WeatherPictureBox.Dock = DockStyle.Fill;
            WeatherPictureBox.Location = new Point(0, 0);
            WeatherPictureBox.Name = "WeatherPictureBox";
            WeatherPictureBox.Size = new Size(1199, 638);
            WeatherPictureBox.TabIndex = 0;
            WeatherPictureBox.TabStop = false;
            // 
            // grafikEkrani
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1199, 638);
            Controls.Add(WeatherPictureBox);
            Name = "grafikEkrani";
            Text = "AirCast - Grafik Ekranı";
            Load += grafikEkrani_Load;
            ((System.ComponentModel.ISupportInitialize)WeatherPictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox WeatherPictureBox;
    }
}
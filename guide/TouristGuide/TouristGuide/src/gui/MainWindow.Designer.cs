namespace TouristGuide
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuStartDevice = new System.Windows.Forms.MenuItem();
            this.menuStartSymulator = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.labelPosition = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.mapPanel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.mapPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItem4);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.menuStartDevice);
            this.menuItem1.MenuItems.Add(this.menuStartSymulator);
            this.menuItem1.Text = "Gps";
            // 
            // menuStartDevice
            // 
            this.menuStartDevice.Checked = true;
            this.menuStartDevice.Text = "Start Device";
            this.menuStartDevice.Click += new System.EventHandler(this.menuStartDevice_Click);
            // 
            // menuStartSymulator
            // 
            this.menuStartSymulator.Text = "Start Symulator";
            this.menuStartSymulator.Click += new System.EventHandler(this.menuStartSymulator_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.MenuItems.Add(this.menuItem5);
            this.menuItem4.Text = "Guide";
            // 
            // menuItem5
            // 
            this.menuItem5.Text = "Exit";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.Text = "Position";
            // 
            // labelPosition
            // 
            this.labelPosition.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.labelPosition.Location = new System.Drawing.Point(60, 0);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(177, 20);
            this.labelPosition.Text = "---";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 20);
            this.label2.Text = "Speed";
            // 
            // labelSpeed
            // 
            this.labelSpeed.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.labelSpeed.Location = new System.Drawing.Point(60, 20);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(76, 20);
            this.labelSpeed.Text = "---";
            this.labelSpeed.ParentChanged += new System.EventHandler(this.labelSpeed_ParentChanged);
            // 
            // mapPanel
            // 
            this.mapPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.mapPanel.Controls.Add(this.pictureBox1);
            this.mapPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mapPanel.Location = new System.Drawing.Point(0, 46);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(240, 222);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-10, -3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(266, 279);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.mapPanel);
            this.Controls.Add(this.labelSpeed);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelPosition);
            this.Controls.Add(this.label1);
            this.Menu = this.mainMenu1;
            this.Name = "MainWindow";
            this.Text = "TouristGuide";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.mapPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuStartDevice;
        private System.Windows.Forms.MenuItem menuStartSymulator;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.Panel mapPanel;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}


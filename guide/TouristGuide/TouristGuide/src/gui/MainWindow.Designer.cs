namespace TouristGuide.gui
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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuStartDevice = new System.Windows.Forms.MenuItem();
            this.menuStartSymulator = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.mapPanel = new TouristGuide.gui.MapPanel();
            this.panelCoordinates = new System.Windows.Forms.Panel();
            this.labelPosition = new System.Windows.Forms.Label();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.mapMessageBox = new System.Windows.Forms.Label();
            this.mapPanel.SuspendLayout();
            this.panelCoordinates.SuspendLayout();
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
            this.menuItem4.MenuItems.Add(this.menuItem6);
            this.menuItem4.MenuItems.Add(this.menuItem2);
            this.menuItem4.MenuItems.Add(this.menuItem3);
            this.menuItem4.Text = "Guide";
            // 
            // menuItem5
            // 
            this.menuItem5.Text = "Exit";
            // 
            // menuItem6
            // 
            this.menuItem6.MenuItems.Add(this.menuItem7);
            this.menuItem6.Text = "Options";
            // 
            // menuItem7
            // 
            this.menuItem7.Text = "Coordinates";
            this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "Download Pois";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Text = "Poi Browser";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // mapPanel
            // 
            this.mapPanel.BackColor = System.Drawing.Color.White;
            this.mapPanel.Controls.Add(this.panelCoordinates);
            this.mapPanel.Controls.Add(this.mapMessageBox);
            this.mapPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapPanel.Location = new System.Drawing.Point(0, 0);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(240, 268);
            // 
            // panelCoordinates
            // 
            this.panelCoordinates.BackColor = System.Drawing.Color.Black;
            this.panelCoordinates.Controls.Add(this.labelPosition);
            this.panelCoordinates.Controls.Add(this.labelSpeed);
            this.panelCoordinates.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCoordinates.Location = new System.Drawing.Point(0, 0);
            this.panelCoordinates.Name = "panelCoordinates";
            this.panelCoordinates.Size = new System.Drawing.Size(240, 16);
            this.panelCoordinates.Visible = false;
            // 
            // labelPosition
            // 
            this.labelPosition.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.labelPosition.ForeColor = System.Drawing.Color.Gold;
            this.labelPosition.Location = new System.Drawing.Point(3, 0);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(152, 14);
            this.labelPosition.Text = "location";
            // 
            // labelSpeed
            // 
            this.labelSpeed.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.labelSpeed.ForeColor = System.Drawing.Color.Gold;
            this.labelSpeed.Location = new System.Drawing.Point(161, 0);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(76, 14);
            this.labelSpeed.Text = "speed";
            this.labelSpeed.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // mapMessageBox
            // 
            this.mapMessageBox.BackColor = System.Drawing.Color.Black;
            this.mapMessageBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mapMessageBox.Font = new System.Drawing.Font("Courier New", 8F, System.Drawing.FontStyle.Regular);
            this.mapMessageBox.ForeColor = System.Drawing.Color.Gold;
            this.mapMessageBox.Location = new System.Drawing.Point(0, 252);
            this.mapMessageBox.Name = "mapMessageBox";
            this.mapMessageBox.Size = new System.Drawing.Size(240, 16);
            this.mapMessageBox.Text = "Map message box.";
            this.mapMessageBox.Visible = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.mapPanel);
            this.Menu = this.mainMenu1;
            this.Name = "MainWindow";
            this.Text = "TouristGuide";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.mapPanel.ResumeLayout(false);
            this.panelCoordinates.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuStartDevice;
        private System.Windows.Forms.MenuItem menuStartSymulator;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private MapPanel mapPanel;
        private System.Windows.Forms.Label mapMessageBox;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.Panel panelCoordinates;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuItem7;
    }
}


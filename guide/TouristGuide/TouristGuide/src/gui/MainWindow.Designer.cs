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
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.labelPosition = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.mapPanel = new TouristGuide.gui.MapPanel();
            this.mapMessageBox = new System.Windows.Forms.Label();
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
            this.menuItem4.MenuItems.Add(this.menuItem2);
            this.menuItem4.Text = "Guide";
            // 
            // menuItem5
            // 
            this.menuItem5.Text = "Exit";
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "Download POIs";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 20);
            this.label1.Text = "Position";
            // 
            // labelPosition
            // 
            this.labelPosition.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.labelPosition.Location = new System.Drawing.Point(50, 0);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(177, 20);
            this.labelPosition.Text = "---";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.label2.Location = new System.Drawing.Point(3, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 20);
            this.label2.Text = "Speed";
            // 
            // labelSpeed
            // 
            this.labelSpeed.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.labelSpeed.Location = new System.Drawing.Point(50, 20);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(76, 20);
            this.labelSpeed.Text = "---";
            this.labelSpeed.ParentChanged += new System.EventHandler(this.labelSpeed_ParentChanged);
            // 
            // mapPanel
            // 
            this.mapPanel.BackColor = System.Drawing.Color.Khaki;
            this.mapPanel.Controls.Add(this.mapMessageBox);
            this.mapPanel.Location = new System.Drawing.Point(3, 43);
            this.mapPanel.Name = "mapPanel";
            this.mapPanel.Size = new System.Drawing.Size(234, 222);
            // 
            // mapMessageBox
            // 
            this.mapMessageBox.BackColor = System.Drawing.Color.Red;
            this.mapMessageBox.Font = new System.Drawing.Font("Courier New", 8F, System.Drawing.FontStyle.Regular);
            this.mapMessageBox.Location = new System.Drawing.Point(0, 0);
            this.mapMessageBox.Name = "mapMessageBox";
            this.mapMessageBox.Size = new System.Drawing.Size(234, 18);
            this.mapMessageBox.Text = "Map message box.";
            this.mapMessageBox.Visible = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.YellowGreen;
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
        private MapPanel mapPanel;
        private System.Windows.Forms.Label mapMessageBox;
        private System.Windows.Forms.MenuItem menuItem2;
    }
}


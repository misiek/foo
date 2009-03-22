namespace TestApp1
{
    partial class MainForm
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
            System.Windows.Forms.MainMenu mainMenu;
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuClear = new System.Windows.Forms.MenuItem();
            this.mnuExit = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mnuPenDemo = new System.Windows.Forms.MenuItem();
            this.mnuBrushDemo = new System.Windows.Forms.MenuItem();
            mainMenu = new System.Windows.Forms.MainMenu();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            mainMenu.MenuItems.Add(this.menuItem1);
            mainMenu.MenuItems.Add(this.menuItem2);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.mnuClear);
            this.menuItem1.MenuItems.Add(this.mnuExit);
            this.menuItem1.Text = "File";
            // 
            // mnuClear
            // 
            this.mnuClear.Text = "Clear";
            this.mnuClear.Click += new System.EventHandler(this.mnuClear_Click);
            // 
            // mnuExit
            // 
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.MenuItems.Add(this.mnuPenDemo);
            this.menuItem2.MenuItems.Add(this.mnuBrushDemo);
            this.menuItem2.Text = "More";
            // 
            // mnuPenDemo
            // 
            this.mnuPenDemo.Text = "Pen demo";
            this.mnuPenDemo.Click += new System.EventHandler(this.mnuPenDemo_Click);
            // 
            // mnuBrushDemo
            // 
            this.mnuBrushDemo.Text = "Brush demo";
            this.mnuBrushDemo.Click += new System.EventHandler(this.mnuBrushDemo_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Menu = mainMenu;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "GDI+ Demo";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem mnuClear;
        private System.Windows.Forms.MenuItem mnuExit;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem mnuPenDemo;
        private System.Windows.Forms.MenuItem mnuBrushDemo;
    }
}


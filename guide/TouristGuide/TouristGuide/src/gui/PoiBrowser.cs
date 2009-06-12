using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using TouristGuide.map.obj;
using TouristGuide.map.repository;

namespace TouristGuide.gui
{
    public class PoiBrowser : Form
    {
        private static int LEFT_MARGIN = 3;
        private static int FREE_Y_DEFAULT = 220;

        private TabControl tabControl;
        private TabPage tabPois;
        private TabPage tabTargets;
        private ListBox listBoxPois;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private TabPage tabPreview;
        private Label labelPoiName;
        private TextBox textBoxPoiDescr;
        private Microsoft.WindowsCE.Forms.Notification notification1;

        private Poi currentPoi;
        private List<Label> currentMediaFilesLinks;
        private List<Label> currentMainDetailsLinks;

        
        private int freeY;

        private PoiRepository poiRepository;
        public PoiRepository PoiRepository
        {
            set
            {
                this.poiRepository = value;
            }
        }
    
        public PoiBrowser()
        {
            InitializeComponent();
            this.currentMediaFilesLinks = new List<Label>();
            this.currentMainDetailsLinks = new List<Label>();
            this.Visible = false;
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            Debug.WriteLine("OnDeactivate: *********************", ToString());
            // clean current media file links list
            foreach (Label ll in this.currentMediaFilesLinks)
            {
                this.tabPreview.Controls.Remove(ll);
            }
            this.currentMediaFilesLinks = new List<Label>();
            // clean current main details links list
            foreach (Label ll in this.currentMainDetailsLinks)
            {
                this.tabPreview.Controls.Remove(ll);
            }
            this.currentMainDetailsLinks = new List<Label>();
            // free current poi media
            this.currentPoi.freeMedia();
        }

        public void show()
        {
            this.tabControl.SelectedIndex = 0;
            this.Visible = true;
        }

        public void preview(Poi poi)
        {
            if (poi.isMediaFree())
                this.poiRepository.loadMedia(poi);
            // TODO free poi when PoiBrowser is closed
            this.currentPoi = poi;
            this.tabControl.SelectedIndex = 1;

            this.freeY = FREE_Y_DEFAULT;
            updatePreview();
            this.Visible = true;
        }

        private void updatePreview()
        {
            this.labelPoiName.Text = this.currentPoi.getName();
            this.textBoxPoiDescr.Text = this.currentPoi.getDescr();
            displayMediaFiles();
            displayMainDetails();
        }

        private void displayMainDetails()
        {
            List<MainDetail> mainDetails = this.currentPoi.getMainDetails();
            if (mainDetails.Count > 0)
            {
                Label ll = new Label();
                ll.Text = "Main details:";
                ll.Location = new Point(LEFT_MARGIN, this.freeY);
                ll.TabIndex = 1;
                this.currentMainDetailsLinks.Add(ll);
                this.tabPreview.Controls.Add(ll);
                this.freeY += ll.Height + 3;
                foreach (MainDetail md in mainDetails)
                {
                    ll = new LinkLabel();
                    ll.Text = md.getTitle();
                    ll.Width = this.textBoxPoiDescr.Width;
                    ll.Location = new Point(LEFT_MARGIN, this.freeY);
                    ll.TabIndex = 1;
                    this.currentMainDetailsLinks.Add(ll);
                    this.tabPreview.Controls.Add(ll);
                    this.freeY += ll.Height + 3;
                }
            }
        }

        private void displayMediaFiles()
        {
            List<MediaFile> mediaFiles = this.currentPoi.getMediaFiles();
            if (mediaFiles.Count > 0)
            {
                Label ll = new Label();
                ll.Text = "Media files:";
                ll.Location = new Point(LEFT_MARGIN, this.freeY);
                ll.TabIndex = 1;
                this.currentMediaFilesLinks.Add(ll);
                this.tabPreview.Controls.Add(ll);
                this.freeY += ll.Height + 3;
                foreach (MediaFile mf in mediaFiles)
                {
                    ll = new LinkLabel();
                    ll.Text = mf.getTitle();
                    ll.Width = this.textBoxPoiDescr.Width;
                    ll.Location = new Point(LEFT_MARGIN, this.freeY);
                    ll.TabIndex = 1;
                    this.currentMediaFilesLinks.Add(ll);
                    this.tabPreview.Controls.Add(ll);
                    this.freeY += ll.Height + 3;
                }
            }
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem61 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem62 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem63 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem64 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem65 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem66 = new System.Windows.Forms.ListViewItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPois = new System.Windows.Forms.TabPage();
            this.listBoxPois = new System.Windows.Forms.ListBox();
            this.tabPreview = new System.Windows.Forms.TabPage();
            this.labelPoiName = new System.Windows.Forms.Label();
            this.tabTargets = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.textBoxPoiDescr = new System.Windows.Forms.TextBox();
            this.notification1 = new Microsoft.WindowsCE.Forms.Notification();
            this.tabControl.SuspendLayout();
            this.tabPois.SuspendLayout();
            this.tabPreview.SuspendLayout();
            this.tabTargets.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPois);
            this.tabControl.Controls.Add(this.tabPreview);
            this.tabControl.Controls.Add(this.tabTargets);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(240, 291);
            this.tabControl.TabIndex = 0;
            // 
            // tabPois
            // 
            this.tabPois.Controls.Add(this.listBoxPois);
            this.tabPois.Location = new System.Drawing.Point(0, 0);
            this.tabPois.Name = "tabPois";
            this.tabPois.Size = new System.Drawing.Size(240, 268);
            this.tabPois.Text = "Pois";
            // 
            // listBoxPois
            // 
            this.listBoxPois.Location = new System.Drawing.Point(7, 7);
            this.listBoxPois.Name = "listBoxPois";
            this.listBoxPois.Size = new System.Drawing.Size(226, 254);
            this.listBoxPois.TabIndex = 0;
            // 
            // tabPreview
            // 
            this.tabPreview.AutoScroll = true;
            this.tabPreview.Controls.Add(this.textBoxPoiDescr);
            this.tabPreview.Controls.Add(this.labelPoiName);
            this.tabPreview.Location = new System.Drawing.Point(0, 0);
            this.tabPreview.Name = "tabPreview";
            this.tabPreview.Size = new System.Drawing.Size(240, 268);
            this.tabPreview.Text = "Preview";
            // 
            // labelPoiName
            // 
            this.labelPoiName.Location = new System.Drawing.Point(3, 4);
            this.labelPoiName.Name = "labelPoiName";
            this.labelPoiName.Size = new System.Drawing.Size(212, 20);
            this.labelPoiName.Text = "poi name";
            // 
            // tabTargets
            // 
            this.tabTargets.Controls.Add(this.listView1);
            this.tabTargets.Location = new System.Drawing.Point(0, 0);
            this.tabTargets.Name = "tabTargets";
            this.tabTargets.Size = new System.Drawing.Size(240, 268);
            this.tabTargets.Text = "Targets";
            // 
            // listView1
            // 
            this.listView1.Columns.Add(this.columnHeader1);
            this.listView1.Columns.Add(this.columnHeader2);
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewItem61.Text = "aaaaa";
            listViewItem62.Text = "bbbb";
            listViewItem63.Text = "cccc";
            listViewItem64.Text = "ffff";
            listViewItem65.Text = "dddd";
            listViewItem66.Text = "eeee";
            this.listView1.Items.Add(listViewItem61);
            this.listView1.Items.Add(listViewItem62);
            this.listView1.Items.Add(listViewItem63);
            this.listView1.Items.Add(listViewItem64);
            this.listView1.Items.Add(listViewItem65);
            this.listView1.Items.Add(listViewItem66);
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(240, 268);
            this.listView1.TabIndex = 0;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 60;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 60;
            // 
            // textBoxPoiDescr
            // 
            this.textBoxPoiDescr.AcceptsReturn = true;
            this.textBoxPoiDescr.AcceptsTab = true;
            this.textBoxPoiDescr.Location = new System.Drawing.Point(3, 27);
            this.textBoxPoiDescr.Multiline = true;
            this.textBoxPoiDescr.Name = "textBoxPoiDescr";
            this.textBoxPoiDescr.ReadOnly = true;
            this.textBoxPoiDescr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxPoiDescr.Size = new System.Drawing.Size(212, 185);
            this.textBoxPoiDescr.TabIndex = 1;
            this.textBoxPoiDescr.Text = "description";
            // 
            // notification1
            // 
            this.notification1.Text = "notification1";
            // 
            // PoiBrowser
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.tabControl);
            this.Name = "PoiBrowser";
            this.Text = "Poi Browser";
            this.tabControl.ResumeLayout(false);
            this.tabPois.ResumeLayout(false);
            this.tabPreview.ResumeLayout(false);
            this.tabTargets.ResumeLayout(false);
            this.ResumeLayout(false);

        }

    }
}
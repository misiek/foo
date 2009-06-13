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

        private Poi currentPoi;
        private MediaFilesListPanel mediaFilesPanel;
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
            this.currentMainDetailsLinks = new List<Label>();
            this.Visible = false;
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            Debug.WriteLine("OnDeactivate: *********************", ToString());
            // clean media files list panel
            if (this.mediaFilesPanel != null)
                this.tabPreview.Controls.Remove(this.mediaFilesPanel);
            this.mediaFilesPanel = null;
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
                //ll.TabIndex = 1;
                this.currentMainDetailsLinks.Add(ll);
                this.tabPreview.Controls.Add(ll);
                this.freeY += ll.Height;
                foreach (MainDetail md in mainDetails)
                {
                    ll = new MainDetailLinkLabel(md, this);
                    ll.Text = md.getTitle();
                    ll.Width = this.textBoxPoiDescr.Width;
                    ll.Location = new Point(LEFT_MARGIN, this.freeY);
                    //ll.TabIndex = 1;
                    this.currentMainDetailsLinks.Add(ll);
                    this.tabPreview.Controls.Add(ll);
                    this.freeY += ll.Height;
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
                this.tabPreview.Controls.Add(ll);
                this.freeY += ll.Height;
                this.mediaFilesPanel = new MediaFilesListPanel(mediaFiles, this);
                this.mediaFilesPanel.Width = this.textBoxPoiDescr.Width;
                this.mediaFilesPanel.Location = new Point(LEFT_MARGIN, this.freeY);
                this.tabPreview.Controls.Add(this.mediaFilesPanel);
                this.freeY += this.mediaFilesPanel.Height;
            }
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem();
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPois = new System.Windows.Forms.TabPage();
            this.listBoxPois = new System.Windows.Forms.ListBox();
            this.tabPreview = new System.Windows.Forms.TabPage();
            this.textBoxPoiDescr = new System.Windows.Forms.TextBox();
            this.labelPoiName = new System.Windows.Forms.Label();
            this.tabTargets = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
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
            // textBoxPoiDescr
            // 
            this.textBoxPoiDescr.AcceptsReturn = true;
            this.textBoxPoiDescr.AcceptsTab = true;
            this.textBoxPoiDescr.Location = new System.Drawing.Point(3, 27);
            this.textBoxPoiDescr.Multiline = true;
            this.textBoxPoiDescr.Name = "textBoxPoiDescr";
            this.textBoxPoiDescr.ReadOnly = true;
            this.textBoxPoiDescr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxPoiDescr.Size = new System.Drawing.Size(220, 185);
            this.textBoxPoiDescr.TabIndex = 1;
            this.textBoxPoiDescr.Text = "description";
            // 
            // labelPoiName
            // 
            this.labelPoiName.Location = new System.Drawing.Point(3, 4);
            this.labelPoiName.Name = "labelPoiName";
            this.labelPoiName.Size = new System.Drawing.Size(220, 20);
            this.labelPoiName.Text = "poi name";
            // 
            // tabTargets
            // 
            this.tabTargets.Controls.Add(this.listView1);
            this.tabTargets.Location = new System.Drawing.Point(0, 0);
            this.tabTargets.Name = "tabTargets";
            this.tabTargets.Size = new System.Drawing.Size(232, 265);
            this.tabTargets.Text = "Targets";
            // 
            // listView1
            // 
            this.listView1.Columns.Add(this.columnHeader1);
            this.listView1.Columns.Add(this.columnHeader2);
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewItem7.Text = "aaaaa";
            listViewItem8.Text = "bbbb";
            listViewItem9.Text = "cccc";
            listViewItem10.Text = "ffff";
            listViewItem11.Text = "dddd";
            listViewItem12.Text = "eeee";
            this.listView1.Items.Add(listViewItem7);
            this.listView1.Items.Add(listViewItem8);
            this.listView1.Items.Add(listViewItem9);
            this.listView1.Items.Add(listViewItem10);
            this.listView1.Items.Add(listViewItem11);
            this.listView1.Items.Add(listViewItem12);
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(232, 265);
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
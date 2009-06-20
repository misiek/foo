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
using System.Collections;
using TouristGuide.map;

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
        private TabPage tabPreview;
        private Label labelPoiName;
        private TextBox textBoxPoiDescr;
 
        private Poi currentPoi;
        private Poi selectedPoi;
        private Poi selectedTargetPoi;
        private Poi targetToRemove;
        private List<Poi> allPois;
        private NamedArea currentNamedArea;
        private MediaFilesListPanel mediaFilesPanel;
        private List<Label> currentMainDetailsLinks;

        private int freeY;
        private ListBox listBoxTargets;
        private Button buttonAddTarget;
        private Button buttonPreview;
        private Button buttonDown;
        private Button buttonUp;
        private Button buttonRemove;
        private Button buttonPreview2;

        private PoiRepository poiRepository;
        public PoiRepository PoiRepository
        {
            set
            {
                this.poiRepository = value;
            }
        }

        private Targets targets;
        public Targets Targets
        {
            set
            {
                this.targets = value;
                this.targets.targetDone += new Targets.TargetDone(targetDone);
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
            cleanPreview();
        }

        private void targetDone(Poi target)
        {
            this.targetToRemove = target;
            this.listBoxTargets.Invoke(new EventHandler(removeTarget));
        }

        private void removeTarget(Object sender, EventArgs args)
        {
            if (this.targetToRemove != null)
            {
                this.listBoxTargets.Items.Remove(this.targetToRemove);
                this.targetToRemove = null;
            }
        }

        private void cleanPreview() {
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
            if (this.currentPoi != null)
                this.currentPoi.freeMedia();
        }

        public void display()
        {
            this.tabControl.SelectedIndex = 0;
            this.Visible = true;
            updatePoisList();
        }

        private void updatePoisList()
        {
            if (this.allPois == null)
            {
                // TODO user should be able to choose area, but now it is only for krakow
                Hashtable areas = this.poiRepository.getAreas();
                this.currentNamedArea = (NamedArea)areas["Kraków"];
                this.poiRepository.setCurrentArea(this.currentNamedArea);
                // get all pois list
                this.allPois = this.poiRepository.allPois();
                foreach (Poi p in this.allPois)
                {
                    this.listBoxPois.Items.Add(p);
                }
            }
        }

        public void preview(Poi poi)
        {
            cleanPreview();
            if (poi.isMediaFree() || poi.isDataFree())
                this.poiRepository.load(poi);
            this.currentPoi = poi;
            this.freeY = FREE_Y_DEFAULT;
            updatePreview();
            this.tabControl.SelectedIndex = 1;
            //if (!this.Visible)
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPois = new System.Windows.Forms.TabPage();
            this.buttonAddTarget = new System.Windows.Forms.Button();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.listBoxPois = new System.Windows.Forms.ListBox();
            this.tabPreview = new System.Windows.Forms.TabPage();
            this.textBoxPoiDescr = new System.Windows.Forms.TextBox();
            this.labelPoiName = new System.Windows.Forms.Label();
            this.tabTargets = new System.Windows.Forms.TabPage();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonPreview2 = new System.Windows.Forms.Button();
            this.listBoxTargets = new System.Windows.Forms.ListBox();
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
            this.tabPois.Controls.Add(this.buttonAddTarget);
            this.tabPois.Controls.Add(this.buttonPreview);
            this.tabPois.Controls.Add(this.listBoxPois);
            this.tabPois.Location = new System.Drawing.Point(0, 0);
            this.tabPois.Name = "tabPois";
            this.tabPois.Size = new System.Drawing.Size(240, 268);
            this.tabPois.Text = "Pois";
            // 
            // buttonAddTarget
            // 
            this.buttonAddTarget.Enabled = false;
            this.buttonAddTarget.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.buttonAddTarget.Location = new System.Drawing.Point(173, 7);
            this.buttonAddTarget.Name = "buttonAddTarget";
            this.buttonAddTarget.Size = new System.Drawing.Size(60, 15);
            this.buttonAddTarget.TabIndex = 3;
            this.buttonAddTarget.Text = "Add Target";
            this.buttonAddTarget.Click += new System.EventHandler(this.buttonAddTarget_Click);
            // 
            // buttonPreview
            // 
            this.buttonPreview.Enabled = false;
            this.buttonPreview.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.buttonPreview.Location = new System.Drawing.Point(7, 7);
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Size = new System.Drawing.Size(47, 15);
            this.buttonPreview.TabIndex = 2;
            this.buttonPreview.Text = "Preview";
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // listBoxPois
            // 
            this.listBoxPois.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBoxPois.Location = new System.Drawing.Point(0, 28);
            this.listBoxPois.Name = "listBoxPois";
            this.listBoxPois.Size = new System.Drawing.Size(240, 240);
            this.listBoxPois.TabIndex = 0;
            this.listBoxPois.SelectedIndexChanged += new System.EventHandler(this.listBoxPois_SelectedIndexChanged);
            // 
            // tabPreview
            // 
            this.tabPreview.AutoScroll = true;
            this.tabPreview.Controls.Add(this.textBoxPoiDescr);
            this.tabPreview.Controls.Add(this.labelPoiName);
            this.tabPreview.Location = new System.Drawing.Point(0, 0);
            this.tabPreview.Name = "tabPreview";
            this.tabPreview.Size = new System.Drawing.Size(232, 265);
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
            this.tabTargets.Controls.Add(this.buttonDown);
            this.tabTargets.Controls.Add(this.buttonUp);
            this.tabTargets.Controls.Add(this.buttonRemove);
            this.tabTargets.Controls.Add(this.buttonPreview2);
            this.tabTargets.Controls.Add(this.listBoxTargets);
            this.tabTargets.Location = new System.Drawing.Point(0, 0);
            this.tabTargets.Name = "tabTargets";
            this.tabTargets.Size = new System.Drawing.Size(240, 268);
            this.tabTargets.Text = "Targets";
            // 
            // buttonDown
            // 
            this.buttonDown.Enabled = false;
            this.buttonDown.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.buttonDown.Location = new System.Drawing.Point(198, 7);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(35, 15);
            this.buttonDown.TabIndex = 4;
            this.buttonDown.Text = "Down";
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Enabled = false;
            this.buttonUp.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.buttonUp.Location = new System.Drawing.Point(168, 7);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(24, 15);
            this.buttonUp.TabIndex = 3;
            this.buttonUp.Text = "Up";
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Enabled = false;
            this.buttonRemove.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.buttonRemove.Location = new System.Drawing.Point(61, 7);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(48, 15);
            this.buttonRemove.TabIndex = 2;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonPreview2
            // 
            this.buttonPreview2.Enabled = false;
            this.buttonPreview2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.buttonPreview2.Location = new System.Drawing.Point(7, 7);
            this.buttonPreview2.Name = "buttonPreview2";
            this.buttonPreview2.Size = new System.Drawing.Size(48, 15);
            this.buttonPreview2.TabIndex = 1;
            this.buttonPreview2.Text = "Preview";
            this.buttonPreview2.Click += new System.EventHandler(this.buttonPreview2_Click);
            // 
            // listBoxTargets
            // 
            this.listBoxTargets.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBoxTargets.Location = new System.Drawing.Point(0, 28);
            this.listBoxTargets.Name = "listBoxTargets";
            this.listBoxTargets.Size = new System.Drawing.Size(240, 240);
            this.listBoxTargets.TabIndex = 0;
            this.listBoxTargets.SelectedIndexChanged += new System.EventHandler(this.listBoxTargets_SelectedIndexChanged);
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

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            preview(this.selectedPoi);
        }

        private void buttonAddTarget_Click(object sender, EventArgs e)
        {
            // add target to map manager
            this.targets.add(this.selectedPoi);
            // also add it to targets list
            this.buttonAddTarget.Enabled = false;
            this.listBoxTargets.Items.Add(this.selectedPoi);
        }

        private void listBoxPois_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.listBoxPois.SelectedIndex;
            this.selectedPoi = (Poi)this.listBoxPois.Items[index];
            this.buttonPreview.Enabled = true;
            if (!this.listBoxTargets.Items.Contains(this.selectedPoi))
                this.buttonAddTarget.Enabled = true;
            else
                this.buttonAddTarget.Enabled = false;
        }

        private void buttonPreview2_Click(object sender, EventArgs e)
        {
            preview(this.selectedTargetPoi);
        }

        private void listBoxTargets_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.listBoxTargets.SelectedIndex;
            if (index < 0 || index >= this.listBoxTargets.Items.Count)
                return;
            this.selectedTargetPoi = (Poi)this.listBoxTargets.Items[index];
            this.buttonPreview2.Enabled = true;
            this.buttonRemove.Enabled = true;
            if (index > 0)
                this.buttonUp.Enabled = true;
            else
                this.buttonUp.Enabled = false;
            if (index < this.listBoxTargets.Items.Count - 1)
                this.buttonDown.Enabled = true;
            else
                this.buttonDown.Enabled = false;

        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            // remove target from map manager
            this.targets.remove(this.selectedTargetPoi);
            // also remove it from list
            this.listBoxTargets.Items.Remove(this.selectedTargetPoi);
            this.buttonPreview2.Enabled = false;
            this.buttonRemove.Enabled = false;
            this.buttonUp.Enabled = false;
            this.buttonDown.Enabled = false;
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            int index = this.listBoxTargets.SelectedIndex;
            Poi tmp = (Poi)this.listBoxTargets.Items[index];
            this.listBoxTargets.Items[index] = this.listBoxTargets.Items[index - 1];
            this.listBoxTargets.Items[index - 1] = tmp;
            this.listBoxTargets.SelectedIndex = index - 1;
            if (index - 1 == 0)
                this.buttonUp.Enabled = false;
            // move target in map manager, 1 place to the beginning of the list
            this.targets.move(this.selectedTargetPoi, -1);
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            int index = this.listBoxTargets.SelectedIndex;
            Poi tmp = (Poi)this.listBoxTargets.Items[index];
            this.listBoxTargets.Items[index] = this.listBoxTargets.Items[index + 1];
            this.listBoxTargets.Items[index + 1] = tmp;
            this.listBoxTargets.SelectedIndex = index + 1;
            if (index + 1 == this.listBoxTargets.Items.Count - 1)
                this.buttonDown.Enabled = false;
            // move target in map manager, 1 place to the end of the list
            this.targets.move(this.selectedTargetPoi, 1);
        }

    }
}
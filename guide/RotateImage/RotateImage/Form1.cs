using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using AForge.Imaging.Filters;

namespace RotateImage
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("time1: " + DateTime.Now, ToString());
            Rotate rotateFilter = new Rotate(-15, AForge.Imaging.InterpolationMethod.NearestNeighbor, true);
            //rotateFilter.KeepSize = true;
            Debug.WriteLine("time2: " + DateTime.Now, ToString());
            pictureBox1.Image = rotateFilter.Apply(new Bitmap(pictureBox1.Image));
            Debug.WriteLine("time3: " + DateTime.Now, ToString());
            pictureBox1.Refresh();
            Debug.WriteLine("time4: " + DateTime.Now, ToString());
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            // animated rotation
        }
    }
}
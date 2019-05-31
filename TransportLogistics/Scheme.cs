using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;

namespace TransportLogistics
{
    public partial class Scheme : Form
    {
        public Scheme()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Scheme_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main f1 = new Main();
            f1.Show();
        }
    }
}


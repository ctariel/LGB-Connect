using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LGBConnect
{
    /// <summary>
    /// Boite d'affichage colorée pour les messages, a la manière d'une MessageBox
    /// </summary>
    public partial class frm_MsgBox : Form
    {
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        public frm_MsgBox()
        {
            InitializeComponent();
        }

        private void frm_MsgBox_Load(object sender, EventArgs e)
        {
            t.Interval = 15000; // specify interval time as you want
            t.Tick += new EventHandler(timer_Tick);
            t.Start();

        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Show(String text, String title)
        {
            this.Text = title;
            lbl_Text.Text = text;
            this.Show();
            this.TopMost = true;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            t.Stop();
            this.Close();
        }
    }
}

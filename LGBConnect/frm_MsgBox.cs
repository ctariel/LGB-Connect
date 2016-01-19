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
        public frm_MsgBox()
        {
            InitializeComponent();
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
    }
}

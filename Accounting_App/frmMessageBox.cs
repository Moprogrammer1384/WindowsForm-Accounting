using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting_App
{
    public partial class frmMessageBox : Form
    {
        public frmMessageBox()
        {
            InitializeComponent();
        }

        internal void SendToBack2(string text)
        {
            btnOk.Text = text;
        }

        internal void SendToBack(string text)
        {
           
            lblMessage.Text = text;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        internal void SendToBack3(string text)
        {
            btnCancel.Text = text;
        }

        internal void SendToBack4()
        {
            throw new NotImplementedException();
        }

        internal void SendToBack4(bool visible)
        {
            btnCancel.Visible = visible;
        }

        internal void SendToBack5(Image image)
        {
            pcMessageImage.Image = image;
        }

        internal void SendToBack6(Image image)
        {
            pcMessageImage.Image = image;

        }
    }
}

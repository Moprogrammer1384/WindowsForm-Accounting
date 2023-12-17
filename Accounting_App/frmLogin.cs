using Accounting_DataLayer.Context;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace Accounting_App
{
    public partial class frmLogin : Form
    {
        public bool IsEdit = false;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (IsEdit == true)
                {
                    var login = db.LoginRepository.Get().First();
                    login.UserName = txtName.Text;
                    login.Password = txtPassword.Text;
                    db.LoginRepository.Update(login);
                    db.Save();
                    Application.Restart();
                }
                else
                {
                    if (db.LoginRepository.Get(l => l.UserName == txtName.Text && l.Password == txtPassword.Text).Any())
                    {
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        frmMessageBox me = new frmMessageBox();
                        string MsgText = "کاربری یافت نشد ";
                        MsgBox(me, MsgText);
                    }
                }
            }
        }
        void MsgBox(frmMessageBox me, string MsgText)
        {
            Label lb = new Label();
            Button btn = new Button();
            Button btnCancel = new Button();
            PictureBox pc = new PictureBox();
            lb.ForeColor = Color.White;
            lb.Text = MsgText;
            btn.Text = "تایید";
            btnCancel.Visible = false;

            pc.Image = Accounting_App.Properties.Resources.Error;


            me.SendToBack5(pc.Image);
            me.SendToBack4(btnCancel.Visible);
            me.SendToBack2(btn.Text);
            me.SendToBack(lb.Text);
            me.BackColor = Color.DarkCyan;
            me.ShowDialog();

        }
        private void FrmLogin_Load(object sender, EventArgs e)
        {
            if (txtName.Text == "" && txtPassword.Text == "")
            {
                btnLogin.Enabled = false;

            }
            else
            {
                btnLogin.Enabled = true;
            }
            if (IsEdit == true)
            {
                this.Text = "تنظیمات ورود به برنامه";
                btnLogin.Text = "ذخیره تغییرات";
                using (UnitOfWork db = new UnitOfWork())
                {
                    var login = db.LoginRepository.Get().First();
                    txtName.Text = login.UserName;
                    txtPassword.Text = login.Password;
                }
            }
        }

        private void PnlMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (txtName.Text != "" && txtPassword.Text != "")
            {
                btnLogin.Enabled = true;
            }
        }
    }
}

using Accounting_DataLayer.Context;
using System;
using System.Windows.Forms;


namespace Accounting_App
{
    public partial class frmEdit : Form
    {
        public int AccountID = 0;
        public frmEdit()
        {
            InitializeComponent();
        }

        private void BunifuTextBox1_TextChanged(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgvNameOfCustomer.AutoGenerateColumns = false;
                dgvNameOfCustomer.DataSource = db.CustomerRepository.GetNameCustomers(txtFilter.Text);
            }

        }

        private void BtnClose_Click_1(object sender, EventArgs e)
        {
            frmEdit frm = new frmEdit();
            DialogResult = DialogResult.Cancel;
        }

        private void FrmEdit_Load(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgvNameOfCustomer.AutoGenerateColumns = false;
                dgvNameOfCustomer.DataSource = db.CustomerRepository.GetNameCustomers();
                if (AccountID != 0)
                {
                    var account = db.AccountingRepository.GetById(AccountID);
                    txtAmount.Text = account.Amount.ToString();
                    txtDescription.Text = account.Descripation;
                    txtNameOfCustomer.Text = db.CustomerRepository.GetCustomerNameById(account.CustomerID);
                    if (account.TypeID == 2)
                    {
                        rbRecive.Checked = true;
                    }
                    else
                    {
                        rbPay.Checked = true;
                    }
                }





            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (rbPay.Checked || rbRecive.Checked)
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    Accounting_DataLayer.AccountingTB accounting = new Accounting_DataLayer.AccountingTB()
                    {
                        Amount = int.Parse(txtAmount.Value.ToString()),
                        CustomerID = db.CustomerRepository.GetCustomerIdByName(txtNameOfCustomer.Text),
                        TypeID = (rbRecive.Checked) ? 2 : 1,
                        DateTitle = DateTime.Now,
                        Descripation = txtDescription.Text

                    };

                    if (AccountID != 0)
                    {
                        accounting.ID = AccountID;
                        db.AccountingRepository.Update(accounting);

                    }
                    else
                    {
                        RtlMessageBox.Show("لطفا دوباره امتحان کنید","خطا",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                     

              
                    db.Save();
                  
                    DialogResult = DialogResult.OK;
                }
            }
            else
            {
                RtlMessageBox.Show("لطفا نوع تراکنش را انتخاب کنید");
            }
        }

        private void DgvNameOfCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtNameOfCustomer.Text = dgvNameOfCustomer.CurrentRow.Cells[0].Value.ToString();
        }
    }
}

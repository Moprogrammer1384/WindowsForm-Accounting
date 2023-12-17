using Accounting_DataLayer;
using Accounting_DataLayer.Context;
using Accounting_Utility.Convertor;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using Accounting_ViewModel.Customers;
using System.Collections.Generic;
using System.Data;

namespace Accounting_App
{
    public partial class Form1 : Form
    {
        public int customerId = 0;
        public int TypeID = 0;

        public Form1()
        {
            InitializeComponent();
        }



        private void BunifuFlatButton1_Click(object sender, EventArgs e)
        {
            pnlNewAccounting.Visible = false;

            CleartText();
            bunifuTransition2.Show(pnlAddOrEditCustomer);
            btnSave.ButtonText = "ثبت";

            pnlAddOrEditCustomer.Visible = true;
            dgvCustomers.Width = 496;
            dgvCustomers.Height = 436;
            customerId = 0;
        }

        private void PnlMain_Paint(object sender, PaintEventArgs e)
        {

        }

        void Filter()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<Accounting_DataLayer.AccountingTB> result = new List<Accounting_DataLayer.AccountingTB>();
                DateTime? startDate;
                DateTime? endDate;
                if ((int)dpCustomers.SelectedValue != 0)
                {
                    int customerId = int.Parse(dpCustomers.SelectedValue.ToString());
                    result.AddRange(db.AccountingRepository.Get(a => a.TypeID == TypeID && a.CustomerID == customerId));
                }
                else
                {
                    result.AddRange(db.AccountingRepository.Get(a => a.TypeID == TypeID));
                }

                if (txtFromDate.Text != "    /  /")
                {
                    startDate = Convert.ToDateTime(txtFromDate.Text);
                    startDate = DateConvertor.ToMiladi(startDate.Value);
                    result = result.Where(r => r.DateTitle >= startDate.Value).ToList();
                }
                if (txtUntilDate.Text != "    /  /")
                {
                    endDate = Convert.ToDateTime(txtUntilDate.Text);
                    endDate = DateConvertor.ToMiladi(endDate.Value);
                    result = result.Where(r => r.DateTitle <= endDate.Value).ToList();
                }

                dgvReport.Rows.Clear();
                foreach (var accounting in result)
                {
                    string customerName = db.CustomerRepository.GetCustomerNameById(accounting.CustomerID);
                    dgvReport.Rows.Add(accounting.ID, customerName, accounting.Amount, accounting.DateTitle.ToShamsi(), accounting.Descripation);
                }
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            frmLogin frmLogin = new frmLogin();
            if (frmLogin.ShowDialog() == DialogResult.OK)
            {
                this.Show();
                //lblDate.Text = DateConvertor.ToShamsi(DateTime.Now);
                //lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
                pnlWhite.Visible = false;
                pnlWhite1.Visible = false;
                pnlReport.Visible = false;
                pnlNewAccounting.Visible = false;
                pnlAddOrEditCustomer.Visible = false;
                pnlCustomers.Visible = false;
                dgvCustomers.Width = 683;
                dgvCustomers.Height = 439;
            }
            else
            {
                Application.Exit();
            }
           


        }
        void GridSize()
        {


        }
        private void BtnCustomers_Click(object sender, EventArgs e)
        {


            if (pnlCustomers.Visible == false)
            {
                bunifuTransition1.Show(pnlCustomers);
                //pnlNewAccounting.Visible = false;
                bunifuTransition.Hide(pnlNewAccounting);
                pnlWhite.Visible = false;
                pnlWhite1.Visible = false;
                BindGrid();
            }
            else
            {
                bunifuTransition1.Hide(pnlCustomers);
                BindGrid();
                //if (pnlNewAccounting.Visible != true)
                //{
                //    bunifuTransition1.Hide(pnlCustomers);
                //}
                //else
                //{

                //    pnlNewAccounting.Visible = false;
                //    bunifuTransition1.Show(pnlCustomers);
                //    BindGrid();
                //}
            }



        }
        void BindGrid()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgvCustomers.AutoGenerateColumns = false;
                dgvCustomers.DataSource = db.CustomerRepository.GetAllCustomers();
            }
        }

        private void TxtFilter_TextChange(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgvCustomers.DataSource = db.CustomerRepository.GetCusstomersByFilter(txtFilter.Text);
            }
        }

        private void BtnDeleteCustomer_Click(object sender, EventArgs e)
        {
            pnlNewAccounting.Visible = false;

            if (dgvCustomers.CurrentRow != null)
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    string name = dgvCustomers.CurrentRow.Cells[1].Value.ToString();

                    frmMessageBox me = new frmMessageBox();
                    string msgdelte = "آیا از حذف این شخص راضی هستید ؟";
                    int msgid = 2;
                    int pci3 = 3;
                    MsgBox(me, msgdelte, msgid, pci3);

                    if (me.DialogResult == DialogResult.OK)
                    {
                        int customerId = int.Parse(dgvCustomers.CurrentRow.Cells[0].Value.ToString());
                        db.CustomerRepository.DeleteCustomer(customerId);
                        db.Save();
                        BindGrid();
                    }
                }
            }

        }


        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            //pnlNewAccounting.Visible = false;

            BindGrid();
        }

        private void BunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TxtAddress_TextChanged(object sender, EventArgs e)
        {

        }




        void CleartText()
        {
            txtName.Text = "";
            txtMobile.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            pcCustomer.Image = Properties.Resources.download;

        }
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                // pcCustomer.ima
                pcCustomer.ImageLocation = openFile.FileName;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            bunifuTransition2.Hide(pnlAddOrEditCustomer);

            dgvCustomers.Width = 683;
            dgvCustomers.Height = 439;
            BindGrid();
        }

        private void BtnCustomers_DoubleClick(object sender, EventArgs e)
        {

        }

        private void Btnclose2_Click(object sender, EventArgs e)
        {

        }

        private void BtnEditCustomer_Click(object sender, EventArgs e)
        {
            pnlNewAccounting.Visible = false;
           
            if (dgvCustomers.CurrentRow != null)
            {

                bunifuTransition2.Show(pnlAddOrEditCustomer);
                customerId = int.Parse(dgvCustomers.CurrentRow.Cells[0].Value.ToString());
               
                if (customerId != 0)
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {

                        
                        var customer = db.CustomerRepository.GetCustomerById(customerId);

                        txtEmail.Text = customer.Email;
                        txtAddress.Text = customer.Address;
                        txtMobile.Text = customer.Mobile;
                        txtName.Text = customer.FullName;
                        pcCustomer.ImageLocation = Application.StartupPath + "/Images/" + customer.CustomerImage;

                    }
                }

              
                dgvCustomers.Width = 496;
                dgvCustomers.Height = 436;
                btnSave.ButtonText = "ویرایش";
            }


        }



        private void BtnSave_Click(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                bool isfull = true;
                string imageName = Guid.NewGuid().ToString() + Path.GetExtension(pcCustomer.ImageLocation);
                string path = Application.StartupPath + "/Images/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                pcCustomer.Image.Save(path + imageName);
                Customers customers = new Customers()
                {
                    Address = txtAddress.Text,
                    Email = txtEmail.Text,
                    FullName = txtName.Text,
                    Mobile = txtMobile.Text,
                    CustomerImage = imageName
                };
                if (txtName.Text == "")
                {
                    int n = 1;
                    int pci2 = 1;
                    frmMessageBox me = new frmMessageBox();
                    string msg = "لطفا نام را وارد کنید ";
                    MsgBox(me, msg, n, pci2);
                    isfull = false;
                }
                if (txtAddress.Text == "")
                {
                    int n = 1;
                    int pci3 = 1;
                    frmMessageBox me = new frmMessageBox();
                    string msg = "لطفا آدرس را وارد کنید ";
                    MsgBox(me, msg, n, pci3);
                    isfull = false;
                }
                if (txtMobile.Text == "")
                {
                    int n = 1;
                    int pci4 = 1;
                    frmMessageBox me = new frmMessageBox();
                    string msg = "لطفا موبایل را وارد کنید ";
                    MsgBox(me, msg, n, pci4);
                    isfull = false;
                }
                if (txtEmail.Text == "")
                {
                    int n = 1;
                    int pci5 = 1;
                    frmMessageBox me = new frmMessageBox();
                    string msg = "لطفا ایمیل را وارد کنید ";
                    MsgBox(me, msg, n, pci5);
                    isfull = false;
                }
                if (isfull == true)
                {

                    if (customerId == 0)
                    {
                        int m = 1;
                        int pci2 = 2;
                        frmMessageBox me = new frmMessageBox();
                        string msg = "شخص جدید با موفقیت اضافه شد";
                        MsgBox(me, msg, m, pci2);
                        db.CustomerRepository.InsertCustomer(customers);

                    }
                    else
                    {
                        int n = 1;
                        int pci = 2;
                        frmMessageBox me = new frmMessageBox();
                        string msg = "شخص  با موفقیت ویرایش شد";
                        MsgBox(me, msg, n, pci);

                        customers.CustomerID = customerId;
                        db.CustomerRepository.UpdateCustomer(customers);



                    }

                    db.Save();
                    BindGrid();
                    CleartText();
                }
            }
        }
        public void MsgBox(frmMessageBox me, string MsgText, int msgId, int pcimage)
        {
            if (msgId == 1)
            {
                Label lb = new Label();
                Button btn = new Button();
                Button btnCancel = new Button();
                PictureBox pc = new PictureBox();
                lb.ForeColor = Color.White;
                lb.Text = MsgText;
                btn.Text = "تایید";
                btnCancel.Visible = false;
                if (pcimage == 1)
                {
                    pc.Image = Accounting_App.Properties.Resources.Error;
                }
                if (pcimage == 2)
                {
                    pc.Image = Accounting_App.Properties.Resources.information;
                }
                me.SendToBack5(pc.Image);
                me.SendToBack4(btnCancel.Visible);
                me.SendToBack2(btn.Text);
                me.SendToBack(lb.Text);
                me.BackColor = Color.DarkCyan;
                me.ShowDialog();
            }
            if (msgId == 2)
            {


                Label lb = new Label();
                Button btnOk = new Button();
                Button btnCancel = new Button();
                PictureBox pc = new PictureBox();
                lb.ForeColor = Color.White;
                lb.Text = MsgText;
                btnOk.Text = "بله";
                btnCancel.Text = "خیر";
                if (pcimage == 3)
                {
                    pc.Image = Accounting_App.Properties.Resources.question;
                }
                me.SendToBack6(pc.Image);
                me.SendToBack3(btnCancel.Text);
                me.SendToBack2(btnOk.Text);
                me.SendToBack(lb.Text);
                me.BackColor = Color.DarkCyan;
                me.ShowDialog();
            }


        }
        private void BunifuRadioButton2_Click(object sender, EventArgs e)
        {

        }

        private void BunifuLabel6_Click(object sender, EventArgs e)
        {

        }

        private void BtnNewAccounting_Click(object sender, EventArgs e)
        {
            bunifuTransition2.Hide(pnlAddOrEditCustomer);
            pnlReport.Visible = false;
            pnlWhite.Visible = false;
            if (pnlCustomers.Visible == true && pnlNewAccounting.Visible == true)
            {

                bunifuTransition1.Hide(pnlCustomers);
                bunifuTransition.Hide(pnlNewAccounting);
                pnlNewAccounting.Visible = false;

            }
            else
            {
                bunifuTransition1.Show(pnlCustomers);
                pnlWhite1.Visible = true;
                bunifuTransition.Show(pnlNewAccounting);
                //pnlNewAccounting.Visible = true;

                using (UnitOfWork db = new UnitOfWork())
                {

                    dgvNameOfCustomer.AutoGenerateColumns = false;
                    dgvNameOfCustomer.DataSource = db.CustomerRepository.GetNameCustomers();



                }

            }
            if (pnlCustomers.Visible == true && pnlReport.Visible == true)
            {
                bunifuTransition.Show(pnlNewAccounting);
            }
        }




        private void BtnSaveAccounting_Click(object sender, EventArgs e)
        {
            if (txtNameOfCustomer.Text == "")
            {
                RtlMessageBox.Show("لطفا یک نام ر از لیست انتخاب انتخاب کنید ");
            }

            if (rbPay.Checked || rbRecive.Checked)
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    Accounting_DataLayer.AccountingTB accounting = new Accounting_DataLayer.AccountingTB()
                    {
                        Amount = int.Parse(txtAmount.Value.ToString()),
                        CustomerID = db.CustomerRepository.GetCustomerIdByName(txtNameOfCustomer.Text),
                        TypeID = (rbRecive.Checked) ? 1 : 2,
                        DateTitle = DateTime.Now,
                        Descripation = txtDescription.Text

                    };
                    db.AccountingRepository.Insert(accounting);
                    db.Save();

                }
                RtlMessageBox.Show("تراکنش شما با موفقیت انجام شد ", "اطلاعات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CleanUtilities();

            }
            else
            {
                RtlMessageBox.Show("لطفا نوع تراکنش را انتخاب کنید");
            }
        }
        void CleanUtilities()
        {
            txtAmount.Value = 1;
            txtNameOfCustomer.Text = "";
            txtDescription.Text = "";
            rbPay.Checked = false;
            rbRecive.Checked = false;


        }
        private void DgvNameOfCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtNameOfCustomer.Text = dgvNameOfCustomer.CurrentRow.Cells[0].Value.ToString();
        }

        private void TxtFilterName_TextChanged(object sender, EventArgs e)
        {
            dgvNameOfCustomer.AutoGenerateColumns = false;
            using (UnitOfWork db = new UnitOfWork())
            {
                dgvNameOfCustomer.DataSource = db.CustomerRepository.GetNameCustomers(txtFilterName.Text);

            }
        }

        private void BtnReportPay_Click(object sender, EventArgs e)
        {
            if (pnlReport.Visible == false)
            {
                pnlCustomers.Visible = true;
                //bunifuTransition1.Show(pnlCustomers);
                pnlWhite1.Visible = true;
                pnlNewAccounting.Visible = true;
                //bunifuTransition.Show(pnlNewAccounting);
                pnlWhite.Visible = true;
                bunifuTransitionReport.Show(pnlReport);

                TypeID = 2;
                Filter();


            }

            else
            {

                bunifuTransitionReport.Hide(pnlReport);
                bunifuTransition1.Hide(pnlCustomers);
                pnlNewAccounting.Visible = false;
                pnlWhite.Visible = true;
            }
            if (pnlCustomers.Visible == false && pnlNewAccounting.Visible == false)
            {
                pnlCustomers.Visible = true;
                pnlNewAccounting.Visible = true;
                pnlWhite.Visible = true;
                bunifuTransitionReport.Show(pnlReport);
                TypeID = 2;
                Filter();

            }



        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {

            Filter();


        }

        private void BtnReciveReport_Click(object sender, EventArgs e)
        {
            if (pnlReport.Visible == false)
            {
                pnlCustomers.Visible = true;
                pnlWhite1.Visible = true;
                pnlNewAccounting.Visible = true;
                pnlWhite.Visible = true;
                bunifuTransitionReport.Show(pnlReport);
                TypeID = 1;
                //Filter();
                pnlReportBind();

            }

            else
            {

                bunifuTransitionReport.Hide(pnlReport);
                bunifuTransition1.Hide(pnlCustomers);
                pnlNewAccounting.Visible = false;
                pnlWhite.Visible = false;
                dgvReport.DataSource = null;
                ClearSearch();
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var result = db.AccountingRepository.Get(a => a.TypeID == TypeID);
                dgvReport.Rows.Clear();
                foreach (var accounting in result)
                {
                    string customerName = db.CustomerRepository.GetCustomerNameById(accounting.CustomerID);
                    dgvReport.Rows.Add(accounting.ID, accounting.Descripation, accounting.DateTitle.ToShamsi(), accounting.Amount, customerName);
                }
                dgvReport.DataSource = db.CustomerRepository.GetCusstomersByFilter2(txtFilter.Text);
            }
        }

        private void BtnRefreshGrid_Click(object sender, EventArgs e)
        {
            Filter();
        }

        private void DgvReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DgvNameOfCustomer_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            txtNameOfCustomer.Text = dgvNameOfCustomer.CurrentRow.Cells[0].Value.ToString();
        }

        private void BtnSaveAccounting_Click_1(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (txtNameOfCustomer.Text == "")
                {

                    int nf = 1;
                    int pci4 = 1;
                    frmMessageBox me = new frmMessageBox();
                    string msg = "لطفا یک نام را از از لیست انتخاب کنید .";
                    MsgBox(me, msg, nf, pci4);
                    CleanUtilities();


                }
                if (txtAmount.Value == null)
                {
                    int nfc = 1;
                    int pci5 = 1;
                    frmMessageBox me = new frmMessageBox();
                    string msg = "لطفا مبلغ را وارد کنید ";
                    MsgBox(me, msg, nfc, pci5);
                    CleanUtilities();

                }
                if (rbPay.Checked || rbRecive.Checked)
                {
                    Accounting_DataLayer.AccountingTB accounting = new Accounting_DataLayer.AccountingTB()
                    {
                        Amount = int.Parse(txtAmount.Value.ToString()),
                        CustomerID = db.CustomerRepository.GetCustomerIdByName(txtNameOfCustomer.Text),
                        TypeID = (rbRecive.Checked) ? 1 : 2,
                        DateTitle = DateTime.Now,
                        Descripation = txtDescription.Text

                    };
                    db.AccountingRepository.Insert(accounting);
                    db.Save();

                    int nfd = 1;
                    int pci6 = 3;
                    frmMessageBox me = new frmMessageBox();
                    string msg = "تراکنش شما با موفقیت انجام شد";
                    MsgBox(me, msg, nfd, pci6);
                    CleanUtilities();
                }
                else
                {


                    int nfdc = 1;
                    int pci7 = 1;
                    frmMessageBox me = new frmMessageBox();
                    string msg = "لطفا نوع تراکنش را مشخص کنید";
                    MsgBox(me, msg, nfdc, pci7);
                    CleanUtilities();
                }

            }

        }
        void ClearUtiliies()
        {
            txtNameOfCustomer.Text = "";
            txtAmount.Value = 1;
            rbPay.Checked = false;
            rbRecive.Checked = false;


        }
        private void TxtFilterName_TextChange(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {

                dgvNameOfCustomer.AutoGenerateColumns = false;
                dgvNameOfCustomer.DataSource = db.CustomerRepository.GetNameCustomers(txtFilterName.Text);
            }

        }

        private void BtnRefreshGrid_Click_1(object sender, EventArgs e)
        {
            Filter();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvReport.CurrentRow != null)
            {
                int id = int.Parse(dgvReport.CurrentRow.Cells[0].Value.ToString());

                frmMessageBox me = new frmMessageBox();
                string msgdelte = "آیا از حذف این تراکنش راضی هستید؟";
                int msgid = 2;
                int pci8 = 3;
                MsgBox(me, msgdelte, msgid, pci8);
                if (me.DialogResult == DialogResult.OK)

                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        db.AccountingRepository.Delete(id);
                        db.Save();
                        Filter();
                    }
                }
            }
        }

        private void BtnReportPay_DoubleClick(object sender, EventArgs e)
        {
            if (pnlReport.Visible == false)
            {
                pnlCustomers.Visible = true;
                //bunifuTransition1.Show(pnlCustomers);
                pnlWhite1.Visible = true;
                pnlNewAccounting.Visible = true;
                //bunifuTransition.Show(pnlNewAccounting);
                pnlWhite.Visible = true;
                bunifuTransitionReport.Show(pnlReport);

                TypeID = 2;
                Filter();


            }

            else
            {

                bunifuTransitionReport.Hide(pnlReport);
                bunifuTransition1.Hide(pnlCustomers);
                pnlNewAccounting.Visible = false;
                pnlWhite.Visible = true;
            }
            if (pnlCustomers.Visible == false && pnlNewAccounting.Visible == false)
            {
                pnlCustomers.Visible = true;
                pnlNewAccounting.Visible = true;
                pnlWhite.Visible = true;
                bunifuTransitionReport.Show(pnlReport);
                TypeID = 2;
                Filter();

            }
        }

        private void BtnReciveReport_DoubleClick(object sender, EventArgs e)
        {
            if (pnlReport.Visible == false)
            {
                pnlCustomers.Visible = true;
                pnlWhite1.Visible = true;
                pnlNewAccounting.Visible = true;
                pnlWhite.Visible = true;
                bunifuTransitionReport.Show(pnlReport);
                TypeID = 1;
                Filter();


            }

            else
            {

                bunifuTransitionReport.Hide(pnlReport);
                bunifuTransition1.Hide(pnlCustomers);
                pnlNewAccounting.Visible = false;
                pnlWhite.Visible = false;
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvReport.CurrentRow != null)
            {
                int id = int.Parse(dgvReport.CurrentRow.Cells[0].Value.ToString());
                frmEdit frmNew = new frmEdit();
                frmNew.AccountID = id;
                if (frmNew.ShowDialog() == DialogResult.OK)
                {
                    Filter();
                }
                if (frmNew.DialogResult == DialogResult.Cancel)
                {
                    frmNew.Hide();
                }
            }
        }





        private void BunifuButton1_Click(object sender, EventArgs e)
        {
            frmMessageBox me = new frmMessageBox();
            Label lb = new Label();
            Button btn = new Button();
            lb.ForeColor = Color.White;
            lb.Text = "فعال است ";
            btn.Text = "بستن";
            me.SendToBack2(btn.Text);
            me.SendToBack(lb.Text);
            me.BackColor = Color.DarkCyan;
            me.ShowDialog();
        }

        private void BtnAddCustomer_ChangeUICues(object sender, UICuesEventArgs e)
        {

        }

        private void BtnEditCustomer_ChangeUICues(object sender, UICuesEventArgs e)
        {

        }
        void pnlReportBind()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<ListCustomerViewModel> list = new List<ListCustomerViewModel>();
                list.Add(new ListCustomerViewModel()
                {
                    CustomerID = 0,
                    FullName = "همه"
                });
                list.AddRange(db.CustomerRepository.GetNameCustomers());
                dpCustomers.DataSource = list;
                dpCustomers.DisplayMember = "FullName";
                dpCustomers.ValueMember = "CustomerID";
            }
        }
        private void BtnReportPay_Click_1(object sender, EventArgs e)
        {
            if (pnlReport.Visible == false)
            {
                pnlCustomers.Visible = true;
                pnlWhite1.Visible = true;
                pnlNewAccounting.Visible = true;
                pnlWhite.Visible = true;
                bunifuTransitionReport.Show(pnlReport);
                TypeID = 2;
                //Filter();
                pnlReportBind();

            }

            else
            {

                bunifuTransitionReport.Hide(pnlReport);
                bunifuTransition1.Hide(pnlCustomers);
                pnlNewAccounting.Visible = false;
                pnlWhite.Visible = false;
                dgvReport.DataSource = null;
                ClearSearch();
            }
        }
        void ClearSearch()
        {
            txtFromDate.Text = "";
            txtUntilDate.Text = "";
            dpCustomers.SelectedValue = 0;
            
        }
        private void BtnSearch_Click_1(object sender, EventArgs e)
        {
            Filter();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin();
            frmLogin.ShowDialog();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            DataTable dtPrint = new DataTable();
            dtPrint.Columns.Add("Customer");
            dtPrint.Columns.Add("Amount");
            dtPrint.Columns.Add("Date");
            dtPrint.Columns.Add("Description");
            foreach (DataGridViewRow item in dgvReport.Rows)
            {
                dtPrint.Rows.Add(
                    item.Cells[1].Value.ToString(),
                    item.Cells[2].Value.ToString(),
                    item.Cells[3].Value.ToString(),
                    item.Cells[4].Value.ToString()
                    );
            }
            stiPrint.Load(Application.StartupPath + "/Report.mrt");
            stiPrint.RegData("DT", dtPrint);
            stiPrint.Show();
        }
    }
}
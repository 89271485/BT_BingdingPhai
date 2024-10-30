using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT07_MainSub
{
    public partial class Form1 : Form
    {
        QLSV ds = new QLSV();
        QLSVTableAdapters.KHOATableAdapter adpKhoa = new QLSVTableAdapters.KHOATableAdapter();
        QLSVTableAdapters.SINHVIENTableAdapter adpSinhVien = new QLSVTableAdapters.SINHVIENTableAdapter();
        QLSVTableAdapters.KETQUATableAdapter adpKetQua = new QLSVTableAdapters.KETQUATableAdapter();
        QLSVTableAdapters.MONHOCTableAdapter adpMonHoc = new QLSVTableAdapters.MONHOCTableAdapter();
        BindingSource bsSV = new BindingSource();
        BindingSource bsKQ = new BindingSource();
        public Form1()
        {
            InitializeComponent();
            bsSV.CurrentChanged += BsSV_CurrentChanged;
        }

        private void BsSV_CurrentChanged(object sender, EventArgs e)
        {
            bdnSinhVien.BindingSource = bsSV;
            lblSTT.Text = (bsSV.Position + 1) + "/" + bsSV.Count;
            txtTongDiem.Text = TongDiem(txtMasv.Text).ToString();
            btnTruoc.Enabled = bsSV.Position > 0;
            btnSau.Enabled = bsSV.Position < bsSV.Count -1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Doc_Du_Lieu();
            Lien_Ket_Dieu_Khien();
            txtTongDiem.Text = TongDiem(txtMasv.Text).ToString();

        }

        private void Lien_Ket_Dieu_Khien()
        {
            foreach (Control ctl in this.Controls)
                if (ctl is TextBox && ctl.Name != "txtTongDiem")
                    ctl.DataBindings.Add("text", bsSV, ctl.Name.Substring(3), true);
                else if (ctl is ComboBox)
                    ctl.DataBindings.Add("Selectedvalue", bsSV, ctl.Name.Substring(3), true);
                else if (ctl is DateTimePicker)
                    ctl.DataBindings.Add("value", bsSV, ctl.Name.Substring(3), true);
                else if (ctl is CheckBox)
                    ctl.DataBindings.Add("checked", bsSV, ctl.Name.Substring(3), true);


        }
        private double TongDiem(string MSV)
        {
            double kq = 0;
            Object td = ds.Tables["KETQUA"].Compute("sum(Diem)", "MaSV='" + MSV + "'");

            if (td == DBNull.Value)
                kq = 0;
            else
                kq = Convert.ToDouble(td);
            return kq;
        }
        private void Doc_Du_Lieu()
        {
            adpKhoa.Fill(ds.KHOA);
            adpSinhVien.Fill(ds.SINHVIEN);
            adpMonHoc.Fill(ds.MONHOC);
            adpKetQua.Fill(ds.KETQUA);

            //2
            cboMaKH.DisplayMember = "TenKH";
            cboMaKH.ValueMember = "MaKH";
            cboMaKH.DataSource = ds.KHOA;

            //3
            bsSV.DataSource = ds.SINHVIEN;

            //4
            bsKQ.DataSource = bsSV;
            bsKQ.DataMember = "SINHVIENKETQUA";

            //5
            dgvKetQua.DataSource = bsKQ;

            //6
            dgvKetQua.Columns["MaSV"].Visible = false;
        }

        private void btTruot_Click(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
          
        }

        private void btnKhong_Click(object sender, EventArgs e)
        {
            
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            QLSV.SINHVIENRow rsv = (bsSV.Current as DataRowView).Row as QLSV.SINHVIENRow;
            if (rsv.GetKETQUARows().Length > 0)
            {
                MessageBox.Show("Sinh viên này đã thi, không huỷ được!", "Thông báo không huỷ sinh viên");
                return;

            }
            DialogResult tl;
            tl = MessageBox.Show("Bạn có muốn xoá sinh viên:" + "\r\n" +
                "+ MaSV: " + txtMasv.Text + "\r\n" +
                "+ Họ và tên: " + txtHosv + ' ' + txtTensv + "\r\n" +
                " Không (Y/N ?", "Cẩn thận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (tl ==DialogResult.Yes)
            {
                bsSV.RemoveCurrent();
                int n = adpSinhVien.Update(ds.SINHVIEN);
                if (n > 0)
                    MessageBox.Show("Xoá sinh viên thành công", "Thông báo Xoá sinh viên");
            }    
        }

        private void btnGhi_Click(object sender, EventArgs e)
        {
            if (txtMasv.ReadOnly == false)
            {
                QLSV.SINHVIENRow rsv = ds.SINHVIEN.FindByMaSV(txtMasv.Text);
                if (rsv != null)
                {
                    MessageBox.Show("Mã sinh viên: " + txtMasv.Text + "Vừa nhập bị trùng, mời nhập lại", "Thông báo lỗi trùng masv",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMasv.Clear();
                    txtMasv.Focus();
                    return;
                }    
            }
            txtMasv.ReadOnly = true;
            bsSV.EndEdit();
            int n = adpSinhVien.Update(ds.SINHVIEN);
            if (n > 0)
                MessageBox.Show("Cập nhật(THEM/SUA) cho SInh Viên" + "\r\n" +
                 "+ MaSV: " + txtMasv.Text + "\r\n" +
                "+ Họ và tên: " + txtHosv + ' ' + txtTensv + "\r\n" +
                "thành công", "Cập nhật sinh viên thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void dgvKetQua_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvKetQua.CurrentRow.IsNewRow == true) return;

            if(dgvKetQua.IsCurrentCellDirty==true)
            {
                if((dgvKetQua.CurrentRow.DataBoundItem as DataRowView).IsNew == true)
                {
                    if (ds.KETQUA.FindByMaSVMaMH(dgvKetQua.CurrentRow.Cells["MaSV"].Value.ToString(),
                      dgvKetQua.CurrentRow.Cells["colMAMH"].Value.ToString()) != null)
                    {
                        MessageBox.Show("Môn học này, sinh viên đã thi, vui lòng chọn môn học khác!",
                            "Thông báo lỗi bị trùng mã môn học", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                        dgvKetQua.CurrentCell = dgvKetQua.CurrentRow.Cells["colMAMH"];
                        return;
                    }
                }
                (dgvKetQua.CurrentRow.DataBoundItem as DataRowView).EndEdit();
                int n = adpKetQua.Update(ds.KETQUA);
                if (n > 0)
                    MessageBox.Show("Cập nhật điểm thi cho sinh viên thành công", "cập nhật kết quả thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

            } 
            
        }
    }
}

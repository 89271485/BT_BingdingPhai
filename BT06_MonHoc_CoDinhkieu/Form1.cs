using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT06_MonHoc_CoDinhkieu
{
    public partial class Form1 : Form
    {
        DataSet1 ds = new DataSet1();

        DataSet1TableAdapters.MONHOCTableAdapter adpMonHoc = new DataSet1TableAdapters.MONHOCTableAdapter();
        DataSet1TableAdapters.KETQUATableAdapter adpKetQua = new DataSet1TableAdapters.KETQUATableAdapter();
        BindingSource bs = new BindingSource();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Doc_Du_Lieu();
            Khoi_Tao_BindingSource();
            Lien_Ket_Dieu_Khien();

        }



        private void Lien_Ket_Dieu_Khien()
        {
            foreach (Control ctl in this.Controls)
                if (ctl is TextBox && ctl.Name != "txtTSSV" && ctl.Name != "txtTongDiem" && ctl.Name != "txtLoaiMH")
                    ctl.DataBindings.Add("text", bs, ctl.Name.Substring(3), true);
            Binding dbLoaiMh = new Binding("text", bs, "LoaiMH", true);
            //Sử dụng Các phuong thức : khi hiển thị nhận lại giá trị
            dbLoaiMh.Format += BdLoaiMh_format;
            dbLoaiMh.Parse += BdLoaiMH_parse;
            txtLoaiMH.DataBindings.Add(dbLoaiMh);


        }

        private void BdLoaiMH_parse(object sender, ConvertEventArgs e)
        {
            if (e.Value == null) return;
            e.Value = e.Value.ToString().ToUpper() == "BẤT BUỘC" ? true : false;
        }

        private void BdLoaiMh_format(object sender, ConvertEventArgs e)
        {
            if (e.Value == DBNull.Value || e.Value == null) return;
            e.Value = (Boolean)e.Value ? "Bắt buộc " : "Tự chọn";
        }
        private void Khoi_Tao_BindingSource()
        {
            bs.DataSource = ds;
            bs.DataMember = ds.MONHOC.TableName;
        }
        private void Doc_Du_Lieu()
        {
            adpMonHoc.Fill(ds.MONHOC);
            adpKetQua.Fill(ds.KETQUA);
        }

        private void lblSTT_Click(object sender, EventArgs e)
        {

        }

        private void btnKhong_Click(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {

        }

        private void btndau_Click(object sender, EventArgs e)
        {

        }

        private void txtTongDiem_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SDIV_FUNCTION_TEST
{
    class clshienthidata
    {
        public clshienthidata()
        {
           
        }

        public void hienthispec(DataGridView dtvSpec, DataTable Spectable)
        {
            dtvSpec.AutoGenerateColumns = false;
            dtvSpec.DataSource = Spectable;
            dtvSpec.Columns.Clear();

            DataGridViewTextBoxColumn col_seq = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_itemid = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_itemname = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_jtype = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_min = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_max = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_CH1 = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_CH2 = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_CH3 = new DataGridViewTextBoxColumn();

            col_seq.DataPropertyName = "seq_no";
            col_itemid.DataPropertyName = "item_id";
            col_itemname.DataPropertyName = "item_name";
            col_jtype.DataPropertyName = "jtype";
            col_min.DataPropertyName = "spec_value1";
            col_max.DataPropertyName = "spec_value2";

            col_seq.HeaderText = "STT";
            col_itemid.HeaderText = "Item ID";
            col_itemname.HeaderText = "Hạng Mục";
            col_jtype.HeaderText = "Đ/Giá";
            col_min.HeaderText = "Min";
            col_max.HeaderText = "Max";
            col_CH1.HeaderText = "CH1";
            col_CH2.HeaderText = "CH2";
            col_CH3.HeaderText = "CH3";

            col_seq.Name = "seq_no";
            col_itemid.Name = "item_id";
            col_itemname.Name = "item_name";
            col_jtype.Name = "jtype";
            col_min.Name = "spec_value1";
            col_max.Name = "spec_value2";

            col_seq.ReadOnly = true;
            col_itemid.ReadOnly = true;
            col_itemname.ReadOnly = true;
            col_jtype.ReadOnly = true;
            col_min.ReadOnly = true;
            col_max.ReadOnly = true;
            col_CH1.ReadOnly = true;
            col_CH2.ReadOnly = true;
            col_CH3.ReadOnly = true;


            col_seq.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_itemid.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_itemname.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_jtype.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_min.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_max.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_CH1.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_CH2.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_CH3.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
 

            col_seq.Width = 50;
            col_itemid.Width = 50;
            col_itemname.Width = 150;
            col_jtype.Width = 55;
            col_min.Width = 55;
            col_max.Width = 55;
            col_CH1.Width = 105;
            col_CH2.Width = 105;
            col_CH3.Width = 105;


            col_seq.CellTemplate.Style.BackColor = Color.AliceBlue;
            col_itemid.CellTemplate.Style.BackColor = Color.AliceBlue;
            col_itemname.CellTemplate.Style.BackColor = Color.AliceBlue;
            col_jtype.CellTemplate.Style.BackColor = Color.AliceBlue;
            col_min.CellTemplate.Style.BackColor = Color.AliceBlue;
            col_max.CellTemplate.Style.BackColor = Color.AliceBlue;
            col_CH1.CellTemplate.Style.BackColor = Color.White;
            col_CH2.CellTemplate.Style.BackColor = Color.White;
            col_CH3.CellTemplate.Style.BackColor = Color.White;


            col_seq.CellTemplate.Style.Font = new Font("Times New Roman", 9);
            col_itemid.CellTemplate.Style.Font = new Font("Times New Roman", 9);
            col_itemname.CellTemplate.Style.Font = new Font("Times New Roman", 9);
            col_jtype.CellTemplate.Style.Font = new Font("Times New Roman", 9);
            col_min.CellTemplate.Style.Font = new Font("Times New Roman", 9);
            col_max.CellTemplate.Style.Font = new Font("Times New Roman", 9);
            col_CH1.CellTemplate.Style.Font = new Font("Times New Roman", 9);
            col_CH2.CellTemplate.Style.Font = new Font("Times New Roman", 9);
            col_CH3.CellTemplate.Style.Font = new Font("Times New Roman", 9);


            col_seq.CellTemplate.Style.ForeColor = Color.Black;
            col_itemid.CellTemplate.Style.ForeColor = Color.Black;
            col_itemname.CellTemplate.Style.ForeColor = Color.Black;
            col_jtype.CellTemplate.Style.ForeColor = Color.Black;
            col_min.CellTemplate.Style.ForeColor = Color.Black;
            col_max.CellTemplate.Style.ForeColor = Color.Black;
            col_CH1.CellTemplate.Style.ForeColor = Color.Black;
            col_CH2.CellTemplate.Style.ForeColor = Color.Black;
            col_CH3.CellTemplate.Style.ForeColor = Color.Black;


            dtvSpec.Columns.Add(col_seq);
            dtvSpec.Columns.Add(col_itemid);
            dtvSpec.Columns.Add(col_itemname);
            dtvSpec.Columns.Add(col_jtype);
            dtvSpec.Columns.Add(col_min);
            dtvSpec.Columns.Add(col_max);
            dtvSpec.Columns.Add(col_CH1);
            dtvSpec.Columns.Add(col_CH2);
            dtvSpec.Columns.Add(col_CH3);

        }

        public void insertresult(DataGridView dtvspec, int dong, string data, Color maunen, Color mauchu, int kenh)
        {
            dtvspec.Rows[dong].Cells[kenh + 5].Style.BackColor = maunen;
            dtvspec.Rows[dong].Cells[kenh + 5].Style.ForeColor = mauchu;
            dtvspec.Rows[dong].Cells[kenh + 5].Value = data;
        }

        public void clearresult(DataGridView dtvspec, Label lb, ProgressBar pgb, int kenh)
        {
            for (int iRow = 0; iRow < dtvspec.RowCount; iRow++)
            {
                dtvspec.Rows[iRow].Cells[kenh + 5].Style.BackColor = Color.White;
                dtvspec.Rows[iRow].Cells[kenh + 5].Value = "";
            }
            lb.Text = "SẴN SÀNG";
            lb.Font = new Font(FontFamily.GenericSansSerif, 14);
            lb.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
            lb.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
            pgb.Value = 0;
        }
        public void hienthiitemcommandtheomodel(DataGridView dtvitemcommand, DataTable itemcommandtable)
        {
            dtvitemcommand.AutoGenerateColumns = false;
            dtvitemcommand.DataSource = itemcommandtable;
            dtvitemcommand.Columns.Clear();

            //DataGridViewTextBoxColumn col_itemid = new DataGridViewTextBoxColumn();
            //DataGridViewTextBoxColumn col_seq = new DataGridViewTextBoxColumn();
            //DataGridViewComboBoxColumn col_scpi = new DataGridViewComboBoxColumn();
            //DataGridViewTextBoxColumn col_cmd = new DataGridViewTextBoxColumn();
            //DataGridViewTextBoxColumn col_addr = new DataGridViewTextBoxColumn();
            //DataGridViewTextBoxColumn col_len = new DataGridViewTextBoxColumn();
            //DataGridViewTextBoxColumn col_data = new DataGridViewTextBoxColumn();
            //DataGridViewComboBoxColumn col_conv = new DataGridViewComboBoxColumn();
            //DataGridViewComboBoxColumn col_storage = new DataGridViewComboBoxColumn();

            DataGridViewTextBoxColumn col_itemid = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_seq = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_scpi = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_cmd = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_addr = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_len = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_data = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_conv = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_storage = new DataGridViewTextBoxColumn();

            col_itemid.DataPropertyName = "Item_ID";
            col_seq.DataPropertyName = "FUNC_SEQ";
            col_scpi.DataPropertyName = "SCPI";
            col_cmd.DataPropertyName = "CMD";
            col_addr.DataPropertyName = "ADDR";
            col_len.DataPropertyName = "LEN";
            col_data.DataPropertyName = "Data";
            col_conv.DataPropertyName = "CONVERSION_TYPE";
            col_storage.DataPropertyName = "Storage_type";

            col_itemid.HeaderText = "Item_ID";
            col_seq.HeaderText = "STT";
            col_scpi.HeaderText = "SCPI";
            col_cmd.HeaderText = "CMD";
            col_addr.HeaderText = "ADDR";
            col_len.HeaderText = "LEN";
            col_data.HeaderText = "Data";
            col_conv.HeaderText = "Chuyển Đổi";
            col_storage.HeaderText = "Lưu Trữ";

            col_itemid.Name = "Item_ID";
            col_seq.Name = "FUNC_SEQ";
            col_scpi.Name = "SCPI";
            col_cmd.Name = "CMD";
            col_addr.Name = "ADDR";
            col_len.Name = "LEN";
            col_data.Name = "Data";
            col_conv.Name = "CONVERSION_TYPE";
            col_storage.Name = "Storage_type";

            col_itemid.ReadOnly = true;
            col_seq.ReadOnly = true;
            col_scpi.ReadOnly = true;
            col_cmd.ReadOnly = true;
            col_addr.ReadOnly = true;
            col_len.ReadOnly = true;
            col_data.ReadOnly = true;
            col_conv.ReadOnly = true;
            col_storage.ReadOnly = true;

            col_itemid.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_seq.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_scpi.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_cmd.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_addr.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_len.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_data.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_conv.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_storage.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            col_itemid.Width = 50;
            col_seq.Width = 40;
            col_scpi.Width = 120;
            col_cmd.Width = 100;
            col_addr.Width = 100;
            col_len.Width = 100;
            col_data.Width = 120;
            col_conv.Width = 120;
            col_storage.Width = 120;

            //col_scpi.FlatStyle = FlatStyle.Popup;
            //col_conv.FlatStyle = FlatStyle.Popup;
            //col_storage.FlatStyle = FlatStyle.Popup;

            col_itemid.CellTemplate.Style.BackColor = Color.Yellow;
            col_seq.CellTemplate.Style.BackColor = Color.Yellow;
            col_scpi.CellTemplate.Style.BackColor = Color.LightCyan;
            col_cmd.CellTemplate.Style.BackColor = Color.LightCyan;
            col_addr.CellTemplate.Style.BackColor = Color.LightCyan;
            col_len.CellTemplate.Style.BackColor = Color.LightCyan;
            col_data.CellTemplate.Style.BackColor = Color.LightCyan;
            col_conv.CellTemplate.Style.BackColor = Color.LightCyan;
            col_storage.CellTemplate.Style.BackColor = Color.LightCyan;

            col_itemid.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_seq.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_scpi.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_cmd.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_addr.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_len.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_data.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_conv.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_storage.CellTemplate.Style.Font = new Font("Times New Roman", 7);


            col_itemid.CellTemplate.Style.ForeColor = Color.Black;
            col_seq.CellTemplate.Style.ForeColor = Color.Black;
            col_scpi.CellTemplate.Style.ForeColor = Color.Black;
            col_cmd.CellTemplate.Style.ForeColor = Color.Black;
            col_addr.CellTemplate.Style.ForeColor = Color.Black;
            col_len.CellTemplate.Style.ForeColor = Color.Black;
            col_data.CellTemplate.Style.ForeColor = Color.Black;
            col_conv.CellTemplate.Style.ForeColor = Color.Black;
            col_storage.CellTemplate.Style.ForeColor = Color.Black;

            dtvitemcommand.Columns.Add(col_itemid);
            dtvitemcommand.Columns.Add(col_seq);
            dtvitemcommand.Columns.Add(col_scpi);
            dtvitemcommand.Columns.Add(col_cmd);
            dtvitemcommand.Columns.Add(col_addr);
            dtvitemcommand.Columns.Add(col_len);
            dtvitemcommand.Columns.Add(col_data);
            dtvitemcommand.Columns.Add(col_conv);
            dtvitemcommand.Columns.Add(col_storage);
        }
        public void hienthiitemcommand(DataGridView dtvitemcommand, DataTable itemcommandtable)
        {
            dtvitemcommand.AutoGenerateColumns = false;
            dtvitemcommand.DataSource = itemcommandtable;
            dtvitemcommand.Columns.Clear();

            DataGridViewTextBoxColumn col_itemid = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_seq = new DataGridViewTextBoxColumn();
            DataGridViewComboBoxColumn col_scpi = new DataGridViewComboBoxColumn();
            DataGridViewTextBoxColumn col_cmd = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_addr = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_len = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_data = new DataGridViewTextBoxColumn();
            DataGridViewComboBoxColumn col_conv = new DataGridViewComboBoxColumn();
            DataGridViewComboBoxColumn col_storage = new DataGridViewComboBoxColumn();

            col_itemid.DataPropertyName = "Item_ID";
            col_seq.DataPropertyName = "FUNC_SEQ";
            col_scpi.DataPropertyName = "SCPI";
            col_cmd.DataPropertyName = "CMD";
            col_addr.DataPropertyName = "ADDR";
            col_len.DataPropertyName = "LEN";
            col_data.DataPropertyName = "Data";
            col_conv.DataPropertyName = "CONVERSION_TYPE";
            col_storage.DataPropertyName = "Storage_type";

            col_itemid.HeaderText = "Item_ID";
            col_seq.HeaderText = "STT";
            col_scpi.HeaderText = "SCPI";
            col_cmd.HeaderText = "CMD";
            col_addr.HeaderText = "ADDR";
            col_len.HeaderText = "LEN";
            col_data.HeaderText = "Data";
            col_conv.HeaderText = "Chuyển Đổi";
            col_storage.HeaderText = "Lưu Trữ";

            col_itemid.Name = "Item_ID";
            col_seq.Name = "FUNC_SEQ";
            col_scpi.Name = "SCPI";
            col_cmd.Name = "CMD";
            col_addr.Name = "ADDR";
            col_len.Name = "LEN";
            col_data.Name = "Data";
            col_conv.Name = "CONVERSION_TYPE";
            col_storage.Name = "Storage_type";

            col_itemid.ReadOnly = true;
            col_seq.ReadOnly = true;
            col_scpi.ReadOnly = false;
            col_cmd.ReadOnly = false;
            col_addr.ReadOnly = false;
            col_len.ReadOnly = false;
            col_data.ReadOnly = false;
            col_conv.ReadOnly = false;
            col_storage.ReadOnly = false;

            col_itemid.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_seq.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_scpi.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_cmd.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_addr.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_len.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_data.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_conv.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_storage.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            col_itemid.Width = 50;
            col_seq.Width = 40;
            col_scpi.Width = 120;
            col_cmd.Width = 100;
            col_addr.Width = 100;
            col_len.Width = 100;
            col_data.Width = 120;
            col_conv.Width = 120;
            col_storage.Width = 120;

            col_scpi.FlatStyle = FlatStyle.Popup;
            col_conv.FlatStyle = FlatStyle.Popup;
            col_storage.FlatStyle = FlatStyle.Popup;

            col_scpi.Items.Add("JUDGEMENT_TYPE");
            col_scpi.Items.Add("BARCODE_SCAN");
            col_scpi.Items.Add("WAKEUP");
            col_scpi.Items.Add("DELAY");
            col_scpi.Items.Add("DAQ");
            col_scpi.Items.Add("DVM");
            col_scpi.Items.Add("HIOKI");
            col_scpi.Items.Add("1W-RB?");
            col_scpi.Items.Add("1W-WC?");
            col_scpi.Items.Add("1W-WB?");
            col_scpi.Items.Add("SMB-RW?");
            col_scpi.Items.Add("SMB-WW?");
            col_scpi.Items.Add("SMB-RB?");
            col_scpi.Items.Add("SMB-WB?");
            col_scpi.Items.Add("CALL");
            col_scpi.Items.Add("CALC");
            col_scpi.Items.Add("MSG");
            col_scpi.Items.Add("MSG Y/N?");
            col_scpi.Items.Add("REPEAT");
            col_scpi.Items.Add("LED");

            col_conv.Items.Add("NO");
            col_conv.Items.Add("PCMDATE");
            col_conv.Items.Add("PACKDATE");
            col_conv.Items.Add("HEX2DEC");
            col_conv.Items.Add("C");
            col_conv.Items.Add("HEX2BIN");
            col_conv.Items.Add("LLHH");
            col_conv.Items.Add("LLHH_HEX");

            col_storage.Items.Add("NO");
            col_storage.Items.Add("PUSH");
            col_storage.Items.Add("PVM");

            col_itemid.CellTemplate.Style.BackColor = Color.Yellow;
            col_seq.CellTemplate.Style.BackColor = Color.Yellow;
            col_scpi.CellTemplate.Style.BackColor = Color.LightCyan;
            col_cmd.CellTemplate.Style.BackColor = Color.LightCyan;
            col_addr.CellTemplate.Style.BackColor = Color.LightCyan;
            col_len.CellTemplate.Style.BackColor = Color.LightCyan;
            col_data.CellTemplate.Style.BackColor = Color.LightCyan;
            col_conv.CellTemplate.Style.BackColor = Color.LightCyan;
            col_storage.CellTemplate.Style.BackColor = Color.LightCyan;

            col_itemid.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_seq.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_scpi.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_cmd.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_addr.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_len.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_data.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_conv.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_storage.CellTemplate.Style.Font = new Font("Times New Roman", 7);

            col_itemid.CellTemplate.Style.ForeColor = Color.Black;
            col_seq.CellTemplate.Style.ForeColor = Color.Black;
            col_scpi.CellTemplate.Style.ForeColor = Color.Black;
            col_cmd.CellTemplate.Style.ForeColor = Color.Black;
            col_addr.CellTemplate.Style.ForeColor = Color.Black;
            col_len.CellTemplate.Style.ForeColor = Color.Black;
            col_data.CellTemplate.Style.ForeColor = Color.Black;
            col_conv.CellTemplate.Style.ForeColor = Color.Black;
            col_storage.CellTemplate.Style.ForeColor = Color.Black;

            dtvitemcommand.Columns.Add(col_itemid);
            dtvitemcommand.Columns.Add(col_seq);
            dtvitemcommand.Columns.Add(col_scpi);
            dtvitemcommand.Columns.Add(col_cmd);
            dtvitemcommand.Columns.Add(col_addr);
            dtvitemcommand.Columns.Add(col_len);
            dtvitemcommand.Columns.Add(col_data);
            dtvitemcommand.Columns.Add(col_conv);
            dtvitemcommand.Columns.Add(col_storage);
        }

        public void hienthicaidatspec(DataGridView dtvSpec,DataTable Spectable)
        {
            dtvSpec.AutoGenerateColumns = false;
            dtvSpec.DataSource = Spectable;
            dtvSpec.Columns.Clear();

            DataGridViewTextBoxColumn col_seq = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_itemid = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_itemname = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_jtype = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_min = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_max = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_testsw = new DataGridViewTextBoxColumn();

            col_seq.DataPropertyName = "seq_no";
            col_itemid.DataPropertyName = "item_id";
            col_itemname.DataPropertyName = "item_name";
            col_jtype.DataPropertyName = "jtype";
            col_min.DataPropertyName = "spec_value1";
            col_max.DataPropertyName = "spec_value2";
            col_testsw.DataPropertyName = "test_sw";

            col_seq.HeaderText = "STT";
            col_itemid.HeaderText = "Item ID";
            col_itemname.HeaderText = "Hạng Mục";
            col_jtype.HeaderText = "Đ/Giá";
            col_min.HeaderText = "Min";
            col_max.HeaderText = "Max";
            col_testsw.HeaderText = "Test/Không";

            col_seq.Name = "seq_no";
            col_itemid.Name = "item_id";
            col_itemname.Name = "item_name";
            col_jtype.Name = "jtype";
            col_min.Name = "spec_value1";
            col_max.Name = "spec_value2";
            col_testsw.Name = "test_sw";

            col_seq.ReadOnly = true;
            col_itemid.ReadOnly = true;
            col_itemname.ReadOnly = false;
            col_jtype.ReadOnly = false;
            col_min.ReadOnly = false;
            col_max.ReadOnly = false;
            col_testsw.ReadOnly = false;

            col_seq.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_itemid.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_itemname.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_jtype.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_min.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_max.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_testsw.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            col_seq.Width = 40;
            col_itemid.Width = 40;
            col_itemname.Width = 100;
            col_jtype.Width = 40;
            col_min.Width = 40;
            col_max.Width = 40;
            col_testsw.Width = 80;

            col_seq.CellTemplate.Style.BackColor = Color.AliceBlue;
            col_itemid.CellTemplate.Style.BackColor = Color.AliceBlue;
            col_itemname.CellTemplate.Style.BackColor = Color.Yellow;
            col_jtype.CellTemplate.Style.BackColor = Color.Yellow;
            col_min.CellTemplate.Style.BackColor = Color.Yellow;
            col_max.CellTemplate.Style.BackColor = Color.Yellow;
            col_testsw.CellTemplate.Style.BackColor = Color.Yellow;

            col_seq.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_itemid.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_itemname.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_jtype.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_min.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_max.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_testsw.CellTemplate.Style.Font = new Font("Times New Roman", 7);

            col_seq.CellTemplate.Style.ForeColor = Color.Black;
            col_itemid.CellTemplate.Style.ForeColor = Color.Black;
            col_itemname.CellTemplate.Style.ForeColor = Color.Black;
            col_jtype.CellTemplate.Style.ForeColor = Color.Black;
            col_min.CellTemplate.Style.ForeColor = Color.Black;
            col_max.CellTemplate.Style.ForeColor = Color.Black;
            col_testsw.CellTemplate.Style.ForeColor = Color.Black;

            dtvSpec.Columns.Add(col_seq);
            dtvSpec.Columns.Add(col_itemid);
            dtvSpec.Columns.Add(col_itemname);
            dtvSpec.Columns.Add(col_jtype);
            dtvSpec.Columns.Add(col_min);
            dtvSpec.Columns.Add(col_max);
            dtvSpec.Columns.Add(col_testsw);
        }

        public void hienthiitemtheoic(DataGridView dtv, DataTable table)
        {
            dtv.AutoGenerateColumns = false;
            dtv.DataSource = table;
            dtv.Columns.Clear();

            DataGridViewTextBoxColumn col_icno = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_itemid = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn col_itemname = new DataGridViewTextBoxColumn();

            col_icno.DataPropertyName = "IC_NO";
            col_itemid.DataPropertyName = "Item_ID";
            col_itemname.DataPropertyName = "item_name";

            col_icno.HeaderText = "IC";
            col_itemid.HeaderText = "Item ID";
            col_itemname.HeaderText = "Tên Hạng Mục";

            col_icno.Name = "IC_NO";
            col_itemid.Name = "Item_ID";
            col_itemname.Name = "item_name";

            col_icno.ReadOnly = true;
            col_itemid.ReadOnly = true;
            col_itemname.ReadOnly = false;

            col_icno.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_itemid.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col_itemname.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            col_icno.Width = 60;
            col_itemid.Width = 60;
            col_itemname.Width = 120;

            col_icno.CellTemplate.Style.BackColor = Color.Yellow;
            col_itemid.CellTemplate.Style.BackColor = Color.Yellow;
            col_itemname.CellTemplate.Style.BackColor = Color.Yellow;

            col_icno.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_itemid.CellTemplate.Style.Font = new Font("Times New Roman", 7);
            col_itemname.CellTemplate.Style.Font = new Font("Times New Roman", 7);

            col_icno.CellTemplate.Style.ForeColor = Color.Black;
            col_itemid.CellTemplate.Style.ForeColor = Color.Black;
            col_itemname.CellTemplate.Style.ForeColor = Color.Black;

            dtv.Columns.Add(col_icno);
            dtv.Columns.Add(col_itemid);
            dtv.Columns.Add(col_itemname);
        }
    }
}

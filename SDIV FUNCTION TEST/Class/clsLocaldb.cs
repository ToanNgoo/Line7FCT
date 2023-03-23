using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;

namespace SDIV_FUNCTION_TEST
{
    class clsLocaldb
    {
        string constr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + Application.StartupPath + @"\Database.mdb";

        public bool checkconnection()
        {
            OleDbConnection cnn = new OleDbConnection(constr);
            try
            {
                cnn.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void bindataCSV(string filePath,DataGridView dtv)
        {
            DataTable dt = new DataTable();
            string[] lines = System.IO.File.ReadAllLines(filePath);
            if(lines.Length>0)
            {
                string firstline = lines[0];
                string[] headerLabels = firstline.Split(',');
                
                foreach(string headerWord in headerLabels)
                {
                    int i = 1;
                    try
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                        
                    }
                    catch (Exception)
                    {
                        try
                        {
                            i++;
                            dt.Columns.Add(new DataColumn(headerWord) + i.ToString());
                        }
                        catch (Exception)
                        {
                            i++;
                            dt.Columns.Add(new DataColumn(headerWord) + i.ToString());
                            
                        }
                        
                    }
                  
                        
                   
                }

                for(int r=1;r<lines.Length;r++)
                {
                    string[] dataWord = lines[r].Split(',');
                    DataRow dr = dt.NewRow();
                    int columIndex = 0;
                    foreach (string headerWord in headerLabels)
                    {
                        try
                        {
                            if (columIndex < dataWord.Length)
                            {
                                dr[headerWord] = dataWord[columIndex++];
                            }
                            else 
                            {
                                dr[headerWord] = " ";
                            }
                        }
                        catch (Exception)
                        {
                            
                            
                        }
                    }
                    
                    dt.Rows.Add(dr);

                       
                }
            }
            if(dt.Rows.Count>0)
            {
                dtv.DataSource = dt;
            }
        }

        private DataTable Receivedata(string str)
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(str,constr);
            da.Fill(dt);
            return dt;
        }

        private bool senddatatodb(string str)
        {
            OleDbConnection cnn = new OleDbConnection(constr);
            cnn.Open();

            OleDbCommand cmd = new OleDbCommand(str, cnn);
            try
            {
                cmd.ExecuteNonQuery();
                cnn.Close();
                return true;
            }
            catch (Exception)
            {
                cnn.Close();
                return false;
            }
        }

        public void loadmodellist(ComboBox cb)
        {
            string str = "select prd_cd, model_name from tb_std_model where PACK_TYPE = 'CP' order by model_name";
            DataTable dt = new DataTable();
            dt = Receivedata(str);

            foreach (DataRow dr in dt.Rows)
            {
                cb.Items.Add("[" + dr.ItemArray[0] + "]" + dr.ItemArray[1]);
            }
        }

        public DataTable loaditemcommand(string ic)
        {
            string str = "select item_id, seq, scpi, cmd, addr, len, data, conv, storage, ic from Command where ic = " 
                +  "'" + ic + "' order by seq";
            return Receivedata(str);
        }
        public DataTable loaditemcommand(string ic, string packcode)
        {
            string str = "select ic,item_id, seq, scpi, cmd, addr, len, data, conv, storage from Command where ic = "
                + "'" + ic + "' order by seq";
            return Receivedata(str);
        }
        public DataTable loaditemcommand(string ic, int item_id)
        {
            string str = "select item_id, seq, scpi, cmd, addr, len, data, conv, storage, ic from Command where "
                + "item_id = " + item_id + " and ic = '" + ic + "' order by seq";
            return Receivedata(str);
        }

        public DataTable loadspec(string packcode)
        {
            string str = "Select distinct TB_STD_TEST_SPEC_SDI.prd_cd, TB_STD_TEST_SPEC_SDI.SEQ_NO, "
                + "TB_STD_TEST_SPEC_SDI.Item_id, tb_std_item_sdi.item_name, tb_std_test_spec_sdi.jtype , "
                + "TB_STD_TEST_SPEC_SDI.spec_value1, TB_STD_TEST_SPEC_SDI.spec_value2, TB_STD_TEST_SPEC_SDI.test_sw "
                + "from TB_STD_TEST_SPEC_SDI right join tb_std_item_sdi on TB_STD_TEST_SPEC_SDI.item_id = tb_std_item_sdi.item_id " 
                + "where TB_STD_TEST_SPEC_SDI.prd_cd = '" + packcode + "' and TB_STD_TEST_SPEC_SDI.work_type = '0' " 
                + "and TB_STD_TEST_SPEC_SDI.test_sw = 'Y' " 
                + "order by TB_STD_TEST_SPEC_SDI.seq_no";
            return Receivedata(str);
        }
        public DataTable loadspectestpad(string packcode)
        {
            string str = "Select distinct TB_STD_TEST_SPEC_SDI.prd_cd, TB_STD_TEST_SPEC_SDI.SEQ_NO, "
                + "TB_STD_TEST_SPEC_SDI.Item_id, tb_std_item_sdi.item_name , "
                + "TB_STD_TEST_SPEC_SDI.spec_value1, TB_STD_TEST_SPEC_SDI.spec_value2, TB_STD_TEST_SPEC_SDI.spec_value3,"
                + "TB_STD_TEST_SPEC_SDI.spec_value4, TB_STD_TEST_SPEC_SDI.spec_value5,TB_STD_TEST_SPEC_SDI.test_sw "
                + "from TB_STD_TEST_SPEC_SDI right join tb_std_item_sdi on TB_STD_TEST_SPEC_SDI.item_id = tb_std_item_sdi.item_id "
                + "where TB_STD_TEST_SPEC_SDI.prd_cd = '" + packcode + "' and TB_STD_TEST_SPEC_SDI.work_type = '0' "
                //+ "and TB_STD_TEST_SPEC_SDI.test_sw = 'Y' "
                + "order by TB_STD_TEST_SPEC_SDI.seq_no";
            return Receivedata(str);
        }
        public DataTable loadspecsetup(string packcode)
        {
            string str = "Select distinct TB_STD_TEST_SPEC_SDI.prd_cd, TB_STD_TEST_SPEC_SDI.SEQ_NO, "
                + "TB_STD_TEST_SPEC_SDI.Item_id, tb_std_item_sdi.item_name, tb_std_test_spec_sdi.jtype , "
                + "TB_STD_TEST_SPEC_SDI.spec_value1, TB_STD_TEST_SPEC_SDI.spec_value2, TB_STD_TEST_SPEC_SDI.test_sw "
                + "from TB_STD_TEST_SPEC_SDI right join tb_std_item_sdi on TB_STD_TEST_SPEC_SDI.item_id = tb_std_item_sdi.item_id "
                + "where TB_STD_TEST_SPEC_SDI.prd_cd = '" + packcode + "' and TB_STD_TEST_SPEC_SDI.work_type = '0' "
                + "order by TB_STD_TEST_SPEC_SDI.seq_no";
            return Receivedata(str);
        }

        public string searchic(string packcode)
        {
            string str = @"Select ic_No_sdi, prd_cd from tb_std_model where prd_cd = '" + packcode + @"' and pack_type = 'CP'";
            DataTable dt = new DataTable();
            dt = Receivedata(str);

            foreach (DataRow dr in dt.Rows)
            {
                 return dr.ItemArray[0].ToString();
            }
            return "";
        }

        public void updatespectable(DataTable dtoracle)
        {
            senddatatodb("delete from TB_STD_TEST_SPEC_SDI");

            OleDbConnection cnn = new OleDbConnection(constr);
            cnn.Open();

            foreach (DataRow myrow in dtoracle.Rows)
            {
                string str = "INSERT INTO TB_STD_TEST_SPEC_SDI VALUES ('" + myrow.ItemArray[0] + "', '" 
                        + myrow.ItemArray[1] + "', '" + myrow.ItemArray[2] + "', '" + myrow.ItemArray[3] 
                        + "', '" + myrow.ItemArray[4] + "', '" + myrow.ItemArray[5] + "', '" 
                        + myrow.ItemArray[6] + "', '" + myrow.ItemArray[7] + "', '" + myrow.ItemArray[8] 
                        + "', '" + myrow.ItemArray[9] + "', '" + myrow.ItemArray[10] + "', '" 
                        + myrow.ItemArray[11] + "', '" + myrow.ItemArray[12] + "', '" + myrow.ItemArray[13] 
                        + "', '" + myrow.ItemArray[14] + "', '" + myrow.ItemArray[15] + "', '" 
                        + myrow.ItemArray[16] + "', '" + myrow.ItemArray[17] + "', '" + myrow.ItemArray[18] 
                        + "', '" + myrow.ItemArray[19] + "', '" + myrow.ItemArray[20] + "', '" 
                        + myrow.ItemArray[21] + "', '" + myrow.ItemArray[22] + "', '" + myrow.ItemArray[23] 
                        + "', '" + myrow.ItemArray[24] + "', '" + myrow.ItemArray[25] + "')" ;
                        //packcode	"P11G0T-A1-S02"	string

                OleDbCommand cmd = new OleDbCommand(str, cnn);
                cmd.ExecuteNonQuery();
            }
            cnn.Close();
        }

        public void updatemodeltable(DataTable dtoracle)
        {
            senddatatodb("Delete * from TB_STD_MODEL");

            OleDbConnection cnn = new OleDbConnection(constr);
            cnn.Open();

            foreach (DataRow myrow in dtoracle.Rows)
            {
                string str = "INSERT INTO TB_STD_MODEL VALUES ('" + myrow.ItemArray[0]
                    + "', '" + myrow.ItemArray[1] + "', '" + myrow.ItemArray[2] 
                    + "', '" + myrow.ItemArray[3] + "', '" + myrow.ItemArray[4] 
                    + "', '" + myrow.ItemArray[5] + "', '" + myrow.ItemArray[6] 
                    + "', '" + myrow.ItemArray[7] + "', '" + myrow.ItemArray[8] + "')";

                OleDbCommand cmd = new OleDbCommand(str, cnn);
                cmd.ExecuteNonQuery();
            }
            cnn.Close();
        }

        public void updateitemtable(DataTable dtoracle)
        {
            senddatatodb("Delete * from tb_std_item_sdi");

            OleDbConnection cnn = new OleDbConnection(constr);
            cnn.Open();

            foreach (DataRow myrow in dtoracle.Rows)
            {
                string str = "INSERT INTO tb_std_item_sdi VALUES ('" + myrow.ItemArray[0]
                    + "', '" + myrow.ItemArray[1] + "', '" + myrow.ItemArray[2] + "', '"
                    + myrow.ItemArray[3] + "', '" + myrow.ItemArray[4] + "', '" + myrow.ItemArray[5]
                    + "', '" + myrow.ItemArray[6] + "', '" + myrow.ItemArray[7] + "', '"
                    + myrow.ItemArray[8] + "', '" + myrow.ItemArray[9] + "', '" + ""
                    + "', '" + myrow.ItemArray[10] + "')";

                OleDbCommand cmd = new OleDbCommand(str, cnn);
                cmd.ExecuteNonQuery();
            }
            cnn.Close();
        }

        public void updatetestitemtable(DataTable dtoracle)
        {
            senddatatodb("Delete * from TB_STD_TEST_ITEM_SDI");

            OleDbConnection cnn = new OleDbConnection(constr);
            cnn.Open();

            foreach (DataRow myrow in dtoracle.Rows)
            {
                string str = "INSERT INTO TB_STD_TEST_ITEM_SDI VALUES ('" + myrow.ItemArray[0] 
                    + "', '" + myrow.ItemArray[1] + "', '" + myrow.ItemArray[2] 
                    + "', '" + myrow.ItemArray[3] + "', '" + myrow.ItemArray[4] 
                    + "', '" + myrow.ItemArray[5] + "', '" + myrow.ItemArray[6] 
                    + "', '" + myrow.ItemArray[7] + "', '" + myrow.ItemArray[8] 
                    + "', '" + myrow.ItemArray[9] + "', '" + myrow.ItemArray[10] 
                    + "', '" + myrow.ItemArray[11] + "', '" + myrow.ItemArray[12] 
                    + "', '" + myrow.ItemArray[13] + "', '" + myrow.ItemArray[14] 
                    + "', '" + myrow.ItemArray[15] + "', '" + myrow.ItemArray[16] 
                    + "', '" + myrow.ItemArray[17] + "')";

                OleDbCommand cmd = new OleDbCommand(str, cnn);
                cmd.ExecuteNonQuery();
            }

            cnn.Close();
        }

        public void updatecommandtable(DataTable dtoracle)
        {
            senddatatodb("Delete * from command");

            OleDbConnection cnn = new OleDbConnection(constr);
            cnn.Open();

            foreach (DataRow myrow in dtoracle.Rows)
            {
                string str = "INSERT INTO command VALUES ('" + myrow.ItemArray[0] 
                    + "', '" + myrow.ItemArray[1] + "', '" + myrow.ItemArray[2] 
                    + "', '" + myrow.ItemArray[3] + "', '" + myrow.ItemArray[4] 
                    + "', '" + myrow.ItemArray[5] + "', '" + myrow.ItemArray[6] 
                    + "', '" + myrow.ItemArray[7] + "', '" + myrow.ItemArray[8] 
                    + "', '" + myrow.ItemArray[9] + "')";

                OleDbCommand cmd = new OleDbCommand(str, cnn);
                cmd.ExecuteNonQuery();
            }
            cnn.Close();
        }

        public void loadBarcodeLen(ref int CPlenmin, ref int CPlenmax, ref int HPlenmin, ref int HPlenmax, DataTable dtspeclist, string IC)
        {
            string itemid_CP = "", itemid_HP = "";
            DataTable dttemp = new DataTable();
            dttemp = Receivedata("select * from Command where IC = '" + IC + "' and SCPI = 'BARCODE_SCAN' order by seq");

            foreach (DataRow dr in dttemp.Rows)
            {
                if (dr.ItemArray[7].ToString() == "CP")
                {
                    itemid_CP = dr.ItemArray[1].ToString();
                }
                else if (dr.ItemArray[7].ToString() == "HP")
                {
                    itemid_HP = dr.ItemArray[1].ToString();
                }
            }

            foreach (DataRow drspec in dtspeclist.Rows)
            {
                if (drspec.ItemArray[2].ToString() == itemid_CP)
                {
                    CPlenmin = int.Parse(drspec.ItemArray[5].ToString());
                    CPlenmax = int.Parse(drspec.ItemArray[6].ToString());
                }
                else if (drspec.ItemArray[2].ToString() == itemid_HP)
                {
                    HPlenmin = int.Parse(drspec.ItemArray[5].ToString());
                    HPlenmax = int.Parse(drspec.ItemArray[6].ToString());
                }
            }
        }

        public void updatedatabase(DataSet ds)
        {
            string str;
            str = "select PLANT_CD, FACTORY_CD, SHOP_CD, PRD_CD, WORK_TYPE, SEQ_NO, ITEM_ID, JTYPE, "
                + "SPEC_VALUE1, SPEC_VALUE2, SPEC_VALUE3, SPEC_VALUE4, SPEC_VALUE5, TEST_SW, COMMAND_TYPE, "
                + "WW1, WW_DATA1, WW2, WW_DATA2, RW, RESV, DEL_SW, WORKER_ID, TRANS_SRC, UPDATE_DATE, INSERT_DATE "
                + "from TB_STD_TEST_SPEC_SDI order by prd_cd, work_type, seq_no";
            OleDbDataAdapter da = new OleDbDataAdapter(str, constr);
            OleDbConnection cnn = new OleDbConnection(constr);
            OleDbCommand cmdthemspec = new OleDbCommand(@"insert into TB_STD_TEST_SPEC_SDI(PLANT_CD, FACTORY_CD, SHOP_CD, PRD_CD, "
                + "WORK_TYPE, SEQ_NO, ITEM_ID, JTYPE,SPEC_VALUE1, SPEC_VALUE2, SPEC_VALUE3, SPEC_VALUE4, SPEC_VALUE5, "
                + "TEST_SW, COMMAND_TYPE, WW1, WW_DATA1, WW2, WW_DATA2, RW, RESV, DEL_SW, WORKER_ID, TRANS_SRC, UPDATE_DATE, INSERT_DATE) "
                + "Values ()", cnn);
            OleDbCommand cmdxoaspec = new OleDbCommand(@"insert into TB_STD_TEST_SPEC_SDI(PLANT_CD, FACTORY_CD, SHOP_CD, PRD_CD, "
                + "WORK_TYPE, SEQ_NO, ITEM_ID, JTYPE,SPEC_VALUE1, SPEC_VALUE2, SPEC_VALUE3, SPEC_VALUE4, SPEC_VALUE5, "
                + "TEST_SW, COMMAND_TYPE, WW1, WW_DATA1, WW2, WW_DATA2, RW, RESV, DEL_SW, WORKER_ID, TRANS_SRC, UPDATE_DATE, INSERT_DATE) ", cnn);
            da.Update(ds,"tblspec");

            OleDbCommandBuilder dcb = new OleDbCommandBuilder(da);
            str = "select PLANT_CD, FACTORY_CD, SHOP_CD, ITEM_ID, ITEM_NAME, JTYPE, UNIT, "
                + "DEL_SW, WORKER_ID, TRANS_SRC, UPDATE_DATE "
                + "from tb_std_item_sdi order by item_id";
            da = new OleDbDataAdapter(str, constr);
            dcb = new OleDbCommandBuilder(da);
            da.Update(ds.Tables["tblitem"]);

            str = "select PLANT_CD, FACTORY_CD, SHOP_CD, IC_NO, PACK_TYPE, ITEM_ID, COMMAND_TYPE, "
                + "WW1, WW_DATA1, WW2, WW_DATA2, RW, RESV, DEL_SW, WORKER_ID, TRANS_SRC, UPDATE_DATE, INSERT_DATE "
                + "from TB_STD_TEST_ITEM_SDI order by item_id";
            da = new OleDbDataAdapter(str, constr);
            dcb = new OleDbCommandBuilder(da);
            da.Update(ds.Tables["tbltestitem"]);

            str = "select ic_no, item_id, func_seq, scpi, cmd, addr, len, data, conversion_type, storage_type "
                                           + "from command "
                                           + "where tester_id = 'NEW_FUNC_SDIV' "
                                           + "order by item_id, func_seq";
            da = new OleDbDataAdapter(str, constr);
            dcb = new OleDbCommandBuilder(da);
            da.Update(ds.Tables["tblcommand"]);

        }

        public void loadallitem(ComboBox cb)
        {
            string str = @"select item_id, item_name from tb_std_item_sdi order by item_name";
            DataTable dt = new DataTable();
            dt = Receivedata(str);

            foreach (DataRow dr in dt.Rows)
            {
                cb.Items.Add(dr.ItemArray[0] + "|" + dr.ItemArray[1]);
            }
        }

        public DataTable loaditemtheoic(string IC)
        {
            string str = "select distinct TB_STD_TEST_ITEM_SDI.IC_NO, TB_STD_TEST_ITEM_SDI.Item_ID, tb_std_item_sdi.item_name "
                + "from TB_STD_TEST_ITEM_SDI right join tb_std_item_sdi on TB_STD_TEST_ITEM_SDI.Item_ID = tb_std_item_sdi.item_id "
                + "where IC_NO = '" + IC + "' order by item_name";
            return Receivedata(str);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;

namespace SDIV_FUNCTION_TEST
{
    class clsAlarmMES
    {
        string constr = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source = " + Application.StartupPath + @"\MesMsg.mdb";

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

        private DataTable Receivedata(string str)
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(str, constr);
            da.Fill(dt);
            return dt;
        }

        private bool senddatatodb(string str)
        {
            OleDbConnection cnn = new OleDbConnection(constr);
            OleDbCommand cmd = new OleDbCommand(str, cnn);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string Vande_Xuly(string errorcode)
        {
            string str = @"select msg_id, info_en, method_en from Message where msg_id  = " + errorcode;
            DataTable dt = new DataTable();
            dt = Receivedata(str);

            foreach (DataRow dr in dt.Rows)
            {
                return "Alarm: " + errorcode + "\n" + dr.ItemArray[1].ToString() + "\n" + dr.ItemArray[2].ToString();
            }
            return "";
        }


    }
}

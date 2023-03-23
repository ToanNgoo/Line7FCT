using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports ;
using System.IO;
using System.Windows.Forms;

namespace SDIV_FUNCTION_TEST
{
    class clsTestPad
    {
        SerialPort Testpad1;
        bool _testpadflag = false;

        public bool Testpadflag
        {
            get { return _testpadflag; }
            set { _testpadflag = value; }
        }
        Testpad.BTS4003 BTS4003;
        private string _COMnum;
        Boolean _Flag_updateerro = false;
        String[] Testpad_data_name = new string[50], Testpad_data_value = new string[50], Testpad_data_result = new string[50];
        double  _rate_update = 0;
        private string _str_udpateerro = "";        
            string _pack_OCV;
            string _pack_fOCV;
            string _pack_IR;
            string _pack_SARST;
            string _pack_SARSV;
            string _pack_SARSRV;
            string _pack_DCCV;
            string _pack_CCCV;
            string _pack_CNTT1;
            string _pack_CNTV1;
            string _pack_CNTT2;
            string _pack_CNTV2;
            string _pack_CELLOCV;
            string _pack_VFRR1;
            string _pack_CFRR2;
            #region Myvar
            public string Pack_OCV
            {
              get { return _pack_OCV; }
              set { _pack_OCV = value; }
            }
            public string Pack_fOCV
            {
                get { return _pack_fOCV; }
                set { _pack_fOCV = value; }
            }
            public string Pack_IR
            {
                get { return _pack_IR; }
                set { _pack_IR = value; }
            }
            public string Pack_SARST
            {
                get { return _pack_SARST; }
                set { _pack_SARST = value; }
            }
            public string Pack_SARSV
            {
                get { return _pack_SARSV; }
                set { _pack_SARSV = value; }
            }
            public string Pack_SARSRV
            {
                get { return _pack_SARSRV; }
                set { _pack_SARSRV = value; }
            }
            public string Pack_DCCV
            {
                get { return _pack_DCCV; }
                set { _pack_DCCV = value; }
            }
            public string Pack_CCCV
            {
                get { return _pack_CCCV; }
                set { _pack_CCCV = value; }
            }
            public string Pack_CNTT1
            {
                get { return _pack_CNTT1; }
                set { _pack_CNTT1 = value; }
            }
             public string Pack_CNTV1
             {
                 get { return _pack_CNTV1; }
                 set { _pack_CNTV1 = value; }
             }
             public string Pack_CNTT2
             {
                 get { return _pack_CNTT2; }
                 set { _pack_CNTT2 = value; }
             }
             public string Pack_CNTV2
             {
                 get { return _pack_CNTV2; }
                 set { _pack_CNTV2 = value; }
             }
             public string Pack_CELLOCV
             {
                 get { return _pack_CELLOCV; }
                 set { _pack_CELLOCV = value; }
             }
             public string Pack_VFRR1
             {
                 get { return _pack_VFRR1; }
                 set { _pack_VFRR1 = value; }
             }
             public string Pack_CFRR2
             {
                 get { return _pack_CFRR2; }
                 set { _pack_CFRR2 = value; }
             }
            #endregion
        public  List<string> PortList1;
        //Public Portlist1 As New List(Of String)
        //Program.PortList = new List<string>();
        //event
        public event EventHandler eventtestpadupdate;
        public event EventHandler eventtestpadrecived;
        public double Rate_update
        {
            get { return _rate_update; }
            set { _rate_update = value; }
        }
        public Boolean Flag_updateerro
        {
            get { return _Flag_updateerro; }
            set { _Flag_updateerro = value; }
        }
        public string Str_udpateerro
        {
            get { return _str_udpateerro; }
            set { _str_udpateerro = value; }
        }
        public string COMnum
        {
            get { return _COMnum; }
            set { _COMnum = value; }
        }


        public clsTestPad ()
        {
            Testpad1 = new SerialPort();
            PortList1 = new List<string>();

        }
        public bool ketnoi()
        {

            try
            {
                PortList1.Add(COMnum);
                BTS4003 = new Testpad.BTS4003(PortList1.ToArray());
                BTS4003.Received += BTS4003_Received;
                BTS4003.UpdateSchedule += BTS4003_UpdateSchedule;
                return true;
            }
            catch (Exception)
            {
                Testpad1.Close();
                return false;
            }

        }

        void BTS4003_UpdateSchedule(object sender, Testpad.BTS4003.UpdateScheduleEventArgs e)
        {
            //Display(e.Message, e.Rate, e.IsError);
            _rate_update = e.Rate * 100;
            switch (e.IsError)
            {                
                case true:
                    _Flag_updateerro = true;
                    _str_udpateerro = "Tiến trình" + ": " + e.Rate*100  + " %  - Có lỗi - " + e.Message;
                    break;
                case false:
                    _Flag_updateerro = false;
                    _str_udpateerro = "Tiến trình" + ": " + e.Rate * 100 + " %  - Không có lỗi - " + e.Message;
                    break;
            }
            eventtestpadupdate(this, EventArgs.Empty);
        }


        void BTS4003_Received(object sender, Testpad.BTS4003.ReceivedEventArgs e)
        {
            //throw new NotImplementedException();            
            int i;
            for (i = 0; i < e.Items.Count;i++ )
            {
                Testpad_data_name[i] = "";
                Testpad_data_value[i] = "";
                Testpad_data_result[i] = "";
            }
            i = 0;
            for (i=0;i<e.Items.Count ;i++)
            {
                Testpad_data_name[i] = e.Items[i].Name;
                Testpad_data_value[i] = e.Items[i].Value;
                Testpad_data_result[i] = e.Items[i].Result;
                switch  (Testpad_data_name[i] )
                {
                    case "OCV":
                        Pack_OCV = e.Items[i].Value;
                        break;
                    case "IR":
                        Pack_IR = e.Items[i].Value;
                        break;
                    case "SAR-ST":
                        Pack_SARST  = e.Items[i].Value;
                        break;
                    case "SAR-SV":
                        Pack_SARSV  = e.Items[i].Value;
                        break;
                    case "SAR-SRV":
                        Pack_SARSRV  = e.Items[i].Value;
                        break;
                    case "DCCV":
                        Pack_DCCV = e.Items[i].Value;
                        break;
                    case "CCCV":
                        Pack_CCCV = e.Items[i].Value;
                        break;
                    case "CNT-T1":
                        Pack_CNTT1 = e.Items[i].Value;
                        break;
                    case "CNT-V1":
                        Pack_CNTV1 = e.Items[i].Value;
                        break;
                    case "CNT-T2":
                        Pack_CNTT2 = e.Items[i].Value;
                        break;
                    case "CNT-V2":
                        Pack_CNTV2 = e.Items[i].Value;
                        break;
                    case "CellOCV":
                        Pack_CELLOCV = e.Items[i].Value;
                        break;
                    case "R1/VFR":
                        Pack_VFRR1 = e.Items[i].Value;
                        break;
                    case "R2/CFR":
                        Pack_CFRR2 = e.Items[i].Value;
                        break;
                    case "fOCV":
                        Pack_fOCV = e.Items[i].Value;
                        break;
                }
            }
            eventtestpadrecived(this, EventArgs.Empty);
        }
        public void ngatketnoi()
        {
            try
            {
                Testpad1.Close();
            }
            catch (Exception)
            {

            }
        }
        
        /// <summary>
        /// Upgrade Firmware
        /// </summary>
        /// <param name="portname">Serial Port</param>
        /// <param name="filename">filename</param>
        public void  update(string portname, string filename)
        {
            FileInfo fInfo = new FileInfo(filename);
            if (fInfo.Exists)
            {
                using (FileStream fs = fInfo.OpenRead())
                {
                    byte[] array = new byte[Convert.ToInt32(fInfo.Length)];

                    if (fs.Read(array, 0, Convert.ToInt32(fInfo.Length)) == Convert.ToInt32(fInfo.Length))
                    {
                        BTS4003.BeginUpdate(portname, array);
                    }
                }
            }
        }

        public void loadspec(string items, string specmin, string specmax, string spec3, string spec4, string spec5, string sp1, string sp2,string sp3, string sp4, string sp5, bool check, float VoltageOffset, float IROffset)
            //, string specmin, string specmax,string spec3, string spec4, string spec5,string timecheckmin, string timecheckmax, bool check, string VoltageOffset, string IROffset
        {
            //spec3: Volt
            //spec4: Current
            //spec5: timecheck
            try
            {
                switch (items)
                {
                    case "set_OCV": //
                        BTS4003.Set_OCV(
                            float.Parse(specmin),
                            float.Parse(specmax)
                           );
                        break;
                    case "Set_fOCV": //
                        BTS4003.Set_fOCV(
                              float.Parse(specmin),
                              float.Parse(specmax),
                            check
                            );
                        break;
                    case "Set_IR"://
                        BTS4003.Set_IR(
                              float.Parse(specmin),
                              float.Parse(specmax),
                            check
                            );
                        break;
                    case "Set_SAR"://
                        BTS4003.Set_SAR(
                             UInt32.Parse(sp1),
                             UInt32.Parse(spec5),//
                              float.Parse(specmax),
                              float.Parse(specmin),//
                             check
                             );
                        break;
                    case "Set_DCCV"://
                        BTS4003.Set_DCCV(
                             float.Parse(spec4),
                           UInt32.Parse(spec5),
                             float.Parse(specmin),
                             float.Parse(specmax),
                           check
                           );
                        break;
                    case "Set_CCCV"://
                        BTS4003.Set_CCCV(
                             float.Parse(spec3),
                            float.Parse(spec4),
                            UInt32.Parse(spec5),
                              float.Parse(specmin),
                           float.Parse(specmax),
                            check
                            );
                        break;
                    case "Set_CNT"://
                        BTS4003.Set_CNT(
                         UInt32.Parse(sp1),
                          UInt32.Parse(sp2),
                          float.Parse(sp3),
                         UInt32.Parse(sp4),
                          UInt32.Parse(sp5),
                          float.Parse(specmin),//
                          check
                          );
                        break;
                    case "Set_CELL"://
                        BTS4003.Set_CELL(
                         float.Parse(specmin),
                         float.Parse(specmax),
                          check
                          );
                        break;
                    case "Set_VFR": //      
                        BTS4003.Set_VFR(
                            float.Parse(specmin),
                            float.Parse(specmax),
                           UInt32.Parse(spec5),
                            check
                            );
                        break;
                    case "Set_CFR"://
                        BTS4003.Set_CFR(
                            float.Parse(specmin),
                           float.Parse(specmax),
                            check
                            );
                        break;
                    case "Set_Offset"://                
                        BTS4003.Set_Offset(
                            (float)VoltageOffset,
                            (float)IROffset
                            );
                        break;
                }
                BTS4003.DownloadParameter();

            }
            catch (Exception)
            {

                MessageBox.Show("Kiểm tra lại SPEC và kết nối TestPad");
            }
          

            //BTS4003.Set_BarcodeLength((byte)numBarcodeLength.Value);

            //BTS4003.Set_Scanner(chk3310G.Checked);

            
        }
    }
}

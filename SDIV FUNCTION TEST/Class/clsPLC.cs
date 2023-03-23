using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
using System.Windows.Forms;
using ACTETHERLib;
using System.Threading;


namespace SDIV_FUNCTION_TEST
{
    public class clsPLC
    {

        //public  ACTETHERLib.ActFXENETTCP PLC = new ACTETHERLib.ActFXENETTCP();
        public  ACTETHERLib.IActFXENETTCP PLC = new ACTETHERLib.ActFXENETTCP();
        private int IRet = 0;
        private bool _PLC_flag = false;

        private int _ActCpuType1;
        private int _ActDestinationPortNumber1;
        private string _ActHostAddress1;
        private int _ActTimeOut1;
        public int ActCpuType1
        {
            get { return _ActCpuType1; }
            set { _ActCpuType1 = value; }
        }
        public int ActDestinationPortNumber1
        {
            get { return _ActDestinationPortNumber1; }
            set { _ActDestinationPortNumber1 = value; }
        }
        public string ActHostAddress1
        {
            get { return _ActHostAddress1; }
            set { _ActHostAddress1 = value; }
        }
        public int ActTimeOut1
        {
            get { return _ActTimeOut1; }
            set { _ActTimeOut1 = value; }
        }
        public clsPLC()
        {
           // thietlap();
        }
        public bool PLC_flag
        {
            get { return _PLC_flag; }
            set { _PLC_flag = value; }
        }
        public void readplc(string address, TextBox txtread)
        {
            string adrall = address;
            string[] adr = adrall.Split('\n');
            int IRET_read;
            int[] addlength = new int[adr.Length];
            IRET_read = PLC.ReadDeviceRandom(adrall, adr.Length, out addlength[0]);
            if (IRET_read == 0)
            {
                txtread.Text = addlength[0].ToString();
            }
            else
            {
                txtread.Text = "FAIL";
            }
        }
        public void readplc(string address, ref string data)
        {
            string adrall = address;
            string[] adr = adrall.Split('\n');
            int IRET_read;
            int[] addlength = new int[adr.Length];
            IRET_read = PLC.ReadDeviceRandom(adrall, adr.Length, out addlength[0]);
            if (IRET_read == 0)
            {
                data = addlength[0].ToString();
            }
            else
            {
                data = "FAIL";
            }
        }
        public string readplc(string address)
        {
            string adrall = address;
            string[] adr = adrall.Split('\n');
            int IRET_read;
            int[] addlength = new int[adr.Length];
            IRET_read = PLC.ReadDeviceRandom(adrall, adr.Length, out addlength[0]);
            if (IRET_read == 0)
            {
                return addlength[0].ToString();
            }
            else
            {
                return  "FAIL";
            }
        }
        public void Writeplc(string address, int value, TextBox txtresultwrite)
        {
            string adrall = address;
            string[] adr = adrall.Split('\n');
            int IRET_read;
            int[] addlength = new int[adr.Length];
            addlength[0] = value;
            IRET_read = PLC.WriteDeviceRandom(adrall, adr.Length, ref addlength[0]);
            if (IRET_read == 0)
            {
                txtresultwrite.Text = addlength[0].ToString();
            }
            else
            {
                txtresultwrite.Text = "FAIL";
            }
        }

        public void Writeplc(string address, int value, ref string data)
        {
            string adrall = address;
            string[] adr = adrall.Split('\n');
            int IRET_read;
            int[] addlength = new int[adr.Length];
            addlength[0] = value;
            IRET_read = PLC.WriteDeviceRandom(adrall, adr.Length, ref addlength[0]);
            if (IRET_read == 0)
            {
                data = addlength[0].ToString();
            }
            else
            {
                data = "FAIL";
            }
        }
        public void Writeplc(string address, int value)
        {
            string adrall = address;
            string[] adr = adrall.Split('\n');
            int IRET_read;
            int[] addlength = new int[adr.Length];
            addlength[0] = value;
            IRET_read = PLC.WriteDeviceRandom(adrall, adr.Length, ref addlength[0]);           
        }
        public bool ketnoi(Label lbPLCstatus)
        {
            IRet = PLC.Open();
            if (IRet == 0)
            {
                //lbPLCstatus.Text = "CONNECTED";
                lbPLCstatus.BackColor = Color.Green;
                _PLC_flag = true;
                return true;
            }
            else
            {
               //lbPLCstatus.Text = "DISCONNECTED";
                lbPLCstatus.BackColor = Color.Red;
                _PLC_flag = false;
                return false;
            }
        }
        public bool ketnoi(ToolStripStatusLabel lbPLCstatus)
        {
            IRet = PLC.Open();
            if (IRet == 0)
            {
                //lbPLCstatus.Text = "CONNECTED";
                lbPLCstatus.BackColor = Color.Green;
                _PLC_flag = true;
                return true;
            }
            else
            {
                //lbPLCstatus.Text = "DISCONNECTED";
                lbPLCstatus.BackColor = Color.Red;
                _PLC_flag = false;
                return false;
            }
        }
        public void thietlap(int ActCpuType, int ActDestinationPortNumber, string ActHostAddress, int ActTimeOut)
        {
            //PLC1.ActCpuType = 520;
            //PLC1.ActDestinationPortNumber = 1;
            //PLC1.ActHostAddress = "107.107.139.186";
            //PLC1.ActTimeOut = 60000;

            PLC.ActCpuType = ActCpuType;
            PLC.ActDestinationPortNumber = ActDestinationPortNumber;
            PLC.ActHostAddress = ActHostAddress;
            PLC.ActTimeOut = ActTimeOut;
        }
        public void thietlap()
        {
            PLC.ActCpuType = 520;
            PLC.ActDestinationPortNumber = 1;
            PLC.ActHostAddress = "107.107.222.145";
            PLC.ActTimeOut = 60000;

            //PLC.ActCpuType = _ActCpuType1;
            //PLC.ActDestinationPortNumber = _ActDestinationPortNumber1;
            //PLC.ActHostAddress = _ActHostAddress1;
            //PLC.ActTimeOut = _ActTimeOut1;
        }
        public string check_start(string adr)
        {
            string temp = null;
            readplc(adr, ref temp);
            return temp;
        }
        public bool ghi_OK(string adr)
        {
            string temp = null;
            Writeplc(adr, 1, ref temp);
            Thread.Sleep(100);
            Writeplc(adr, 0, ref temp);
            if (int.Parse(temp) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ghi_NG(string adr)
        {
            string temp = null;
            Writeplc(adr, 1, ref temp);
            Thread.Sleep(100);
            Writeplc(adr, 0, ref temp);
            if (int.Parse(temp) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ghi_random(string adr, int delay_ms)
        {
            string temp = null;
            try
            {
                Writeplc(adr, 1, ref temp);
                Thread.Sleep(delay_ms);
                Writeplc(adr, 0, ref temp);
                if (int.Parse(temp) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
            
        }
        public bool PLC_Status()
        {
            return true;
        }
    }
}

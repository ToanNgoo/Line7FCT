using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace SDIV_FUNCTION_TEST
{
    public class clsDVM
    {
        SerialPort DVM;
        clsdataconvert dataconvert;

        private string _COMnum;

        public string COMnum
        {
            get { return _COMnum; }
            set { _COMnum = value; }
        }

        public clsDVM()
        {
            DVM = new SerialPort();
            dataconvert = new clsdataconvert();
        }

        public bool ketnoi()
        {
            try
            {
                DVM.PortName = _COMnum;
                DVM.BaudRate = 9600;
                DVM.DataBits = 8;
                DVM.ReadBufferSize = 1024;
                DVM.WriteBufferSize = 512;
                DVM.Handshake = Handshake.None;
                DVM.Parity = Parity.None;
                DVM.DtrEnable = true;
                DVM.DataReceived += DVM_DataReceived;
                DVM.Open();
                DVM.Write("*CLS\r\n");
                Thread.Sleep(200);
                DVM.Write("SYST:REM\r\n");
                Thread.Sleep(200);
                DVM.Write("*IDN?\r\n");
                Thread.Sleep(700);
                string a = DVM.ReadExisting().Substring(0, 19);
                if (DVM.ReadExisting().Substring(0, 19) == "HEWLETT-PACKARD,344")
                {
                    return true;
                }
                else return false;
            }
            catch (Exception)
            {
                DVM.Close();
                return false;
            }

        }

        void DVM_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
        }

        public void ngatketnoi()
        {
            try
            {
                DVM.Close();
            }
            catch (Exception)
            {

            }
        }

        public double  dodienap()
        {
            DVM.Write("Read?\r\n");
            Thread.Sleep(600);
            return dataconvert.str2numdvm(DVM.ReadExisting());
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SDIV_FUNCTION_TEST
{
    public class clsHioki
    {
        SerialPort HIOKI;
        clsdataconvert dataconvert;

        private string _COMnum;

        public string COMnum
        {
            get { return _COMnum; }
            set { _COMnum = value; }
        }

        public clsHioki()
        {
            HIOKI = new SerialPort();
            dataconvert = new clsdataconvert();
        }

        public bool ketnoi()
        {
            try
            {
                HIOKI.PortName = _COMnum;
                HIOKI.BaudRate = 9600;
                HIOKI.DataBits = 8;
                HIOKI.ReadBufferSize = 1024;
                HIOKI.WriteBufferSize = 512;
                HIOKI.Handshake = Handshake.RequestToSend;
                HIOKI.Parity = Parity.None;
                HIOKI.DtrEnable = true;
                HIOKI.DataReceived += HIOKI_DataReceived;
                HIOKI.Open();
                HIOKI.Write("*IDN?\n");
                Thread.Sleep(700);
                //string a = HIOKI.ReadExisting().Substring(0, 5);
                if ( HIOKI.ReadExisting().Substring(0, 5) == "HIOKI")
                {
                    return true;
                }
                else return false;
            }
            catch (Exception)
            {
                HIOKI.Close();
                return false;
            }

        }

        void HIOKI_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
        }

        public void ngatketnoi()
        {
            try
            {
                HIOKI.Close();
            }
            catch (Exception)
            {

            }
        }

        public double dodienap()
        {
            HIOKI.Write("MEASURE:VOLTAGE?\r\n");
            Thread.Sleep(200);
            return dataconvert.str2num_Hioki(HIOKI.ReadExisting().Substring(0, 9)) * 1000;
        }

        public double dodientro()
        {
            HIOKI.Write("MEASURE:RESISTANCE?\r\n");
            Thread.Sleep(200);
            return float.Parse(HIOKI.ReadExisting().Substring(0, 3));
        }
    }
}

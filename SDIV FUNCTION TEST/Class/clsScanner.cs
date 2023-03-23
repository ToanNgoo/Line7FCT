using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace SDIV_FUNCTION_TEST
{
    public class clsScanner
    {
        public event SerialDataReceivedEventHandler Datareceived;
        SerialPort Scanner;
        Frmmain _frm;
        clsdataconvert dataconvert;

        private string _data;

        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }

        private string _COMnum;

        public string COMnum
        {
            get { return _COMnum; }
            set { _COMnum = value; }
        }

        public clsScanner(Frmmain frm)
        {
            _frm = frm;
            Scanner = new SerialPort();
            dataconvert = new clsdataconvert();
        }

        public bool ketnoi()
        {
            try
            {
                Scanner.PortName = _COMnum;
                Scanner.BaudRate = 9600;
                Scanner.DataBits = 8;
                Scanner.ReadBufferSize = 1024;
                Scanner.WriteBufferSize = 512;
                Scanner.Parity = Parity.None;
                Scanner.DtrEnable = true;
                Scanner.DataReceived += Scanner_DataReceived;
                Scanner.Open();
                Scanner.WriteLine("LON\r");
                Thread.Sleep(200);
                Scanner.WriteLine("LOFF\r");
                string a = Scanner.ReadLine();
              
                if(a!=null)
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
                Scanner.Close();
                return false;
            }
            
        }

        public void ngatketnoi()
        {
            try
            {
                Scanner.Close();
            }
            catch (Exception)
            {
                
            }
        }

        private void Scanner_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //try
            //{
            //    //_data = "";
            //    _data = Scanner.ReadLine();
            //    if (Datareceived != null)
            //    {
            //        Datareceived(this, e);
            //    }
            //}
            //catch (Exception)
            //{
                
            //    //throw;
            //}
            
        }
        public string Read_Scaner()
        {
            string code = "";
            try
            {                
                Scanner.WriteLine("LON\r");
                Thread.Sleep(300);
                code = Scanner.ReadLine();               
            }
            catch
            {
                code = "ERROR";
            }
            return code;
        }
    }
}

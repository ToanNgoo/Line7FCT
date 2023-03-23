using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace SDIV_FUNCTION_TEST
{
   public class clsPWS
    {
        SerialPort PWS;
        string _COMnum;
        private const string PWSON = "OUTP ON" + "\n";
        private const string PWSOF = "OUTP OFF" + "\n";
        private const string PWSCLR = "*rst" + "\n";
        private const string PWSOCPOFF = "Curr:Prot:Stat Off" + "\n";
        private const string PWSOVPOFF = "Volt:Prot:Stat Off" + "\n";
    //    Public PWSON As String = "OUTP ON" & vbCrLf
    //Public PWSOF As String = "OUTP OFF" & vbCrLf
    //Public PWS_SET_50V As String = "VOLT:RANG P50V" & vbCrLf
    //Public PWS_SET_25V As String = "VOLT:RANG P25V" & vbCrLf
    //Public PWS_SET_8V As String = "VOLT:RANG P8V" & vbCrLf
    //Public PWS_SET_20V As String = "VOLT:RANG P20V" & vbCrLf
    //Public PWS_VOUT_0V As String = "VOLT 0" & vbCrLf
    //Public PWS_VIN As String = "MEAS:VOLT?" & vbCrLf
    //Public PWS_IIN As String = "MEAS:CURR?" & vbCrLf
    //Public PWS_error As String = "Syst:Err?" & vbCrLf
    //Public PWS_clearerror As String = "*rst" & vbCrLf
    //Public OCPON As String = "Curr:Prot:Stat On" & vbCrLf
    //Public OCPOFF As String = "Curr:Prot:Stat Off" & vbCrLf
    //Public OVPON As String = "Volt:Prot:Stat On" & vbCrLf
    //Public OVPOFF As String = "Volt:Prot:Stat Off" & vbCrLf
    //Public PWS_CURR_0A As String = "CURR 0" & vbCrLf
        public string COMnum
        {
            get { return _COMnum; }
            set { _COMnum = value; }
        } 
        public clsPWS()
        {
            PWS = new SerialPort();
        }
        public bool ketnoi()
        {
            try
            {
                PWS.PortName = _COMnum;
                PWS.BaudRate = 9600;
                PWS.DataBits = 8;
                PWS.ReadBufferSize = 1024;
                PWS.WriteBufferSize = 512;
                PWS.Handshake = Handshake.None;
                PWS.Parity = Parity.None;
                PWS.DtrEnable = true;
                PWS.DataReceived +=PWS_DataReceived;
                PWS.Open();
                PWS.Write("*CLS\r\n");
                Thread.Sleep(200);
                PWS.Write("SYST:REM\r\n");
                Thread.Sleep(200);
                PWS.Write("*IDN?\r\n");
                Thread.Sleep(700);
                if (PWS.ReadExisting().Substring(0, 19)=="HEWLETT-PACKARD,E36")
                {
                    return true;
                }
                else return false;
            }
            catch (Exception)
            {
                PWS.Close();
                return false;
            }

        }
        public void ngatketnoi()
        {
            try
            {
                PWS.Close();
            }
            catch (Exception)
            {

            }
        }
        public void PWS_OUTPUT(double I, double V)
        {
            PWS_PROTECTION_CLEAR();
            PWS.Write("CURR " + I + "\n");
            PWS.Write("VOLT " + V + "\n");
            PWS_ON();
        }
        public void PWS_OUTPUT(double I, double V, string set)
        {            
            PWS_PROTECTION_CLEAR();
            Thread.Sleep(400);
            PWS.Write("CURR " + I + "\n");
            PWS.Write("VOLT " + V + "\n");
            if (set == "ON")
            {
                PWS_ON();
                
            }
            else if (set == "OFF")
            {
                PWS_OFF();
            }
        }
        public void PWS_ON() { PWS.Write(PWSON); }
        public void PWS_OFF() { PWS.Write(PWSOF); }
        private void PWS_PROTECTION_CLEAR()
        {
            PWS_SET_REZO();
            PWS.Write(PWSCLR);
            PWS.Write(PWSOCPOFF);
            PWS.Write(PWSOCPOFF);
        }
        public void PWS_SET_REZO()
        {
            PWS.Write("CURR 0" + "\n");
            PWS.Write("VOLT 0" + "\n");
        }

        void PWS_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
        }
    }
}

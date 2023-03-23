using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;


namespace SDIV_FUNCTION_TEST
{
    public partial class Frmmain : Form
    {
        #region mydefine
        clsLocaldb Localdb;
        clsConfig Config;
        clsScanner scanner1, scanner2, scanner3;
        public clscomboard com1, com2, com3;
        clsDAQ DAQ1, DAQ2, DAQ3;
        clsDVM DVM1, DVM2, DVM3;
        clsHioki HIOKI1, HIOKI2, HIOKI3;
        clsPWS PWS1, PWS2, PWS3;
        clsSDIVBoard SDIVBoard1, SDIVBoard2, SDIVBoard3, SDIVBoard4;
        clshienthidata hienthidata;
        clsMakelog Makelog;
        clsPLC PLC;
        public bool setflag1, setflag2,setflag3;
        public bool clt1, clt2,clt3,cld1,cld2,cld3;
        public bool flag_tactTime = true;
        public bool flag_testFinish = true;
        public int den1, den2, den3, den4,count1,count;
        //public int count1;
        Stopwatch stw2 = new Stopwatch();
        Stopwatch stw1 = new Stopwatch();
        Stopwatch stw3 = new Stopwatch();
        Stopwatch stw4 = new Stopwatch();
        public clstest testch1 = new clstest(), testch2 = new clstest(), testch3 = new clstest();
        Thread testkenh1;
        Thread testkenh2;
        Thread testkenh3;
        Thread runTactTime;
        Thread runTestFinish;
        string time;
        
        clsdataconvert dataconvert = new clsdataconvert();
        //My var
        public string ProgramProcess, packcode, icno;
        Process aProcess = Process.GetCurrentProcess();
        private int CPlenmin = 0, CPlenmax = 0, HPlenmin = 0, HPlenmax = 0;
        private int teststepch1 = 0;

        private float offset_OCV1 = 0, offset_OCV2 = 0,offset_OCV3=0;
        private float offset_IR1=0, offset_IR2=0,offset_IR3=0;//mA

        public int tabSlected; // 08.Sep-2022 add chức năng show or not show data tab Testing
        public Frmmain()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Show();
            init();
            count = 1;
            autoconnectcom();
            count1 = 0;
            
            Config.loadqty(lbOK, lbNG,lbl_ok2,lbl_ng2,lbOK3,lbNG3,ref count1,ref time);
            loadlastmodel(cblistmodel);
            //loadlastmodel(cbx_model_history);      
        }
     
        private void loadoffset()
        {
            offset_IR1 = float.Parse(tbiroff1.Text);
            offset_IR2 = float.Parse(tbiroff2.Text);
            offset_IR3 = float.Parse(tbiroff3.Text);
           
            offset_OCV1 = float.Parse(tbocvoff1.Text);
            offset_OCV2 = float.Parse(tbocvoff2.Text);
            offset_OCV3 = float.Parse(tbocvoff3.Text);
            
        }       
        public void init()
        {
            hienthidata = new clshienthidata();

            Makelog = new clsMakelog();

            Localdb = new clsLocaldb();

            //MES = new clsMES();
            Config = new clsConfig();

            com1 = new clscomboard();
            com2 = new clscomboard();
            com3 = new clscomboard();

            PLC = new clsPLC();

            scanner1 = new clsScanner(this);
            scanner2 = new clsScanner(this);
            scanner3 = new clsScanner(this);
           // scanner1.Datareceived += scanner1_Datareceived;
            //scanner2.Datareceived += scanner2_Datareceived;
            //scanner3.Datareceived += scanner3_Datareceived;

            DAQ1 = new clsDAQ();
            DAQ2 = new clsDAQ();
            DAQ3 = new clsDAQ();

            DVM1 = new clsDVM();
            DVM2 = new clsDVM();
            DVM3 = new clsDVM();

            HIOKI1 = new clsHioki();
            HIOKI2 = new clsHioki();
            HIOKI3 = new clsHioki();

            PWS1 = new clsPWS();
            PWS2 = new clsPWS();
            PWS3 = new clsPWS();

            SDIVBoard1 = new clsSDIVBoard();
            SDIVBoard2 = new clsSDIVBoard();
            SDIVBoard3 = new clsSDIVBoard();
            SDIVBoard4 = new clsSDIVBoard();

            //load config
            Config.loadconfig_device(cbscanner1, cbscanner2, cbscanner3,
                                     cbcomboard1, cbcomboard2, cbcomboard3,
                                     cbdaq1, cbdaq2, cbdaq3,
                                     cbdvm1, cbdvm2, cbdvm3,
                                     cbhioki1, cbhioki2, cbhioki3,
                                     cbpws1, cbpws2, cbpws3,
                                     cbsdivboard1, cbsdivboard2, cbsdivboard3);
            Config.loadconfig_offset(tbocvoff1, tbiroff1, tbocvoff2, tbiroff2, tbocvoff3, tbiroff3);

            
            //get config and then set to var
           
            //ProgramProcess = Config.loadprocess(cbprocess);
            loadoffset();

            //connection checking
            //-local databasse
            checklocaldb();
            check_conect_PLC();
            Localdb.loadmodellist(cblistmodel);
            //Localdb.loadmodellist(cbx_model_history);


            //-MES
            //socket1 = new clsSocket(this);
            //socket1.Lineno = tblineno.Text;
            //socket1.Mcid = tbmcid.Text;
            //socket1.Portprocess = tbport.Text;
            //socket1.Stnid1 = tbstnid1.Text;
            //socket1.Stnid2 = tbstnid2.Text;
            //socket1.Stnid3 = tbstnid3.Text;
            //socket1.Workerid = tbworkerid.Text;
            //socket1.Ipadd = tbIP.Text;
            //socket1.Port = int.Parse (txtskport.Text );


            //MES.Dbsource = tbdatabaseoracle.Text;
            //MES.User = tbuseroracle.Text;
            //MES.Pass = tbpasswordoracle.Text;


            //if (MES.checkconnection() == true && ProgramProcess != "OBA")
            //{
            //    //Thread thloading = new Thread(new ThreadStart(openloadingform));
            //    //thloading.IsBackground = true;
            //    //thloading.Start();

            //    MES.capnhatmodel();
            //    //thloading.Abort();
            //}

            //tmcheckoracle.Interval = 2000;
            //tmcheckoracle.Enabled = true;

            //tmchecksocket.Interval = 2000;
            //tmchecksocket.Enabled = true;
            //#region AXT
            //DIOload();
            //AXT1 = new clsAXT();
            //checkdio_open();
            //resetDIOCH(1);
            //resetDIOCH(2);
            //#endregion





            #region datainput
            testch1.ProgramProcess = ProgramProcess;
            testch2.ProgramProcess = ProgramProcess;
            testch3.ProgramProcess = ProgramProcess;
            #endregion
            //socket1.start();
        }

        private void check_conect_PLC()
        {
            PLC.thietlap();
            PLC.ketnoi(tssl_PLC_conect);

        }


        void scanner2_Datareceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (testch2.Testflag == false)
            {
              
                try
                {
                    string datareaded = dataconvert.removeline(scanner2.Data);
                    int barcodelength = datareaded.Length;
                    string barcode;

                    if (datareaded.Substring(0, 7) == "BARCODE")
                    {
                        barcode = datareaded.Substring(8, datareaded.Length - 8);
                    }
                    else
                    {
                        barcode = datareaded;
                    }

                    barcodestart(2, barcode);
                    datareaded = "";
                    barcodelength = 0;
                    //scanner2.Data = "";
                }
                catch (Exception)
                {

                }

            }
            scanner2.Data = "";
        }

    
        #region Bat_dau_test
        private void testthread1()
        {
            switch (chbmaster1.Checked)
            {
                
                case true:
                    testch1.test(dtvresult, lbketqua1, pgb1,lbOK,lbNG,lbl_tile3,cblistmodel.Text, 1, tbic.Text, false, "", "", float.Parse(tbocvoff1.Text), int.Parse(tbiroff1.Text), scanner1, com1, DAQ1, DVM1, HIOKI1, PWS1, SDIVBoard1, this, chbcheckfail.Checked, lb_repeat_couter,PLC,tbmodelname,stw1,count1,ckx_autoscroll.Checked,cbx_confirm.Checked, tabSlected);
                    break;
                case false:
                    //testch1.test(dtvresult, lbketqua1, pgb1, lbOK, lbNG, lbl_tile, cblistmodel.Text, 1, tbic.Text, true, "", "", float.Parse(tbocvoff1.Text), int.Parse(tbiroff1.Text), scanner1, com1, DAQ1, DVM1, HIOKI1, PWS1, SDIVBoard1, this, chbcheckfail.Checked, lb_repeat_couter, PLC, tbmodelname, stw2,count1);
                   break;
            }
        }
        private void testthread2()
        {
            switch (chbmaster2.Checked)
            {

                case true:
                    testch2.test(dtvresult,
                                lbketqua2,
                                pgb2,lbl_ok2,lbl_ng2,lbl_tile2,
                                cblistmodel.Text,
                                2, tbic.Text, 
                                false,
                                "", "",
                                float.Parse(tbocvoff2.Text), 
                                int.Parse(tbiroff2.Text),
                                scanner2,
                                com2, DAQ2, DVM2,
                                HIOKI2, PWS2, SDIVBoard2,
                                this, chbcheckfail.Checked,
                                lb_repeat_couter,PLC,tbmodelname,stw2,count1,ckx_autoscroll.Checked,cbx_confirm.Checked, tabSlected);
                    break;
                case false:
                    //testch2.test(dtvresult,
                    //            lbketqua2, pgb2, lbl_ok2, lbl_ng2, lbl_tile2, cblistmodel.Text,
                    //            2, tbic.Text, true, "", "", 
                    //            float.Parse(tbocvoff2.Text),
                    //            int.Parse(tbiroff2.Text),
                    //            scanner2, com2, DAQ2, DVM2,
                    //            HIOKI2,PWS2, SDIVBoard2,
                    //            this, chbcheckfail.Checked,
                    //            lb_repeat_couter,PLC,tbmodelname,stw2,count1);
                    break;
            }
        }

        private void testthread3()
        {
            switch (chbmaster3.Checked)
            {

                case true:
                    testch3.test(dtvresult, lbketqua3, pgb3, lbOK3, lbNG3, lbl_tile, cblistmodel.Text, 3, tbic.Text, false, "", "", float.Parse(tbocvoff3.Text), int.Parse(tbiroff3.Text), scanner3, com3, DAQ3, DVM3, HIOKI3, PWS3, SDIVBoard3, this, chbcheckfail.Checked, lb_repeat_couter, PLC, tbmodelname, stw3, count1,ckx_autoscroll.Checked,cbx_confirm.Checked, tabSlected);
                    break;
                case false:
                    //testch3.test(dtvresult, lbketqua3, pgb3, lbOK, lbNG, lbl_tile, cblistmodel.Text, 3, tbic.Text, true, "", "", float.Parse(tbocvoff3.Text), int.Parse(tbiroff3.Text), scanner3, com3, DAQ3, DVM3, HIOKI3, PWS3, SDIVBoard3, this, chbcheckfail.Checked, lb_repeat_couter, PLC, tbmodelname, stw3, count1);
                    break;
            }
        }

        #endregion
        void scanner1_Datareceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (testch1.Testflag == false)
            {
                
                try
                {
                    string datareaded = dataconvert.removeline(scanner1.Data);
                    int barcodelength = datareaded.Length;
                    string barcode;

                    if (datareaded.Substring(0, 7) == "BARCODE")
                    {
                        barcode = datareaded.Substring(8, datareaded.Length - 8);
                    }
                    else
                    {
                        barcode = datareaded;
                    }

                    barcodestart(1, barcode);
                    datareaded = "";
                    barcodelength = 0;
                    //scanner1.Data = "";
                }
                catch (Exception)
                {

                }
            }
            scanner1.Data = "";
        }
        private void barcodestart(int channel, string barcode)
        {
            switch (ProgramProcess)
            {
                case "CP-PT":
                    if (testch1.Testflag == false)
                    {
                        switch (teststepch1)
                        {
                            case 0:
                                if (barcode.Length >= CPlenmin && barcode.Length <= CPlenmax)
                                {
                                    
                                    lbketqua1.BackColor = Color.FromName("ControlDark");
                                    lbketqua1.Font = new Font("Times New Roman", 8);
                                    lbketqua1.ForeColor = Color.FromName("ControlText");
                                    lbketqua1.Text = "Đọc HP Barcode!";
                                    teststepch1++;
                                }
                                break;
                            case 1:
                                if (barcode.Length >= HPlenmin && barcode.Length <= HPlenmax )
                                {
                                    
                                    testkenh1 = new Thread(new ThreadStart(testthread1));
                                    testkenh1.Start();
                                    teststepch1 = 0;
                                }
                                break;
                        }
                    }
                    break;
                case "PBA":

                    //code moi o day
                    break;
            }
        }
        private void checklocaldb()
        {
            try
            {
                if (Localdb.checkconnection() == true)
                {
                    lb_database_connection.BackColor = Color.Blue;
                }
                else
                {
                    lb_database_connection.BackColor = Color.Red;
                }
            }
            catch (Exception)
            {
                lb_database_connection.BackColor = Color.Red;                
            }
          
        }
       
      
        private void saveoffset()
        {
            Config.saveconfig_offset(tbocvoff1, tbiroff1, tbocvoff2, tbiroff2, tbocvoff3, tbiroff3);
        }
        private void ketnoiscanner(ComboBox cb, Label lb, clsScanner com)
        {
            switch (lb.BackColor.ToString())
            {
                case "Color [Red]":
                    com.COMnum = cb.Text;
                    if (com.ketnoi())
                    {
                        lb.BackColor = Color.Blue;
                        saveconfigdevice();
                    }
                    break;
                case "Color [Blue]":
                    com.ngatketnoi();
                    lb.BackColor = Color.Red;
                    break;
                default:
                    break;
            }
        }
        private void ketnoicomboard(ComboBox cb, Label lb, clscomboard com)
        {
            switch (lb.BackColor.ToString())
            {
                case "Color [Red]":
                    com.COMnum = cb.Text;
                    if (com.ketnoi())
                    {
                        lb.BackColor = Color.Blue;
                        saveconfigdevice();
                    }
                    break;
                case "Color [Blue]":
                    com.ngatketnoi();
                    lb.BackColor = Color.Red;
                    break;
                default:
                    break;
            }
        }

        private void ketnoidaq(ComboBox cb, Label lb, clsDAQ daq)
        {
            switch (lb.BackColor.ToString())
            {
                case "Color [Red]":
                    daq.COMnum = cb.Text;
                    if (daq.ketnoi())
                    {
                        lb.BackColor = Color.Blue;
                        saveconfigdevice();
                    }
                    break;
                case "Color [Blue]":
                    daq.ngatketnoi();
                    lb.BackColor = Color.Red;
                    break;
                default:
                    break;
            }
        }

        private void ketnoidvm(ComboBox cb, Label lb, clsDVM dvm)
        {
            switch (lb.BackColor.ToString())
            {
                case "Color [Red]":
                    dvm.COMnum = cb.Text;
                    if (dvm.ketnoi())
                    {
                        lb.BackColor = Color.Blue;
                        saveconfigdevice();
                    }
                    break;
                case "Color [Blue]":
                    dvm.ngatketnoi();
                    lb.BackColor = Color.Red;
                    break;
                default:
                    break;
            }
        }

        private void ketnoihioki(ComboBox cb, Label lb, clsHioki hioki)
        {
            switch (lb.BackColor.ToString())
            {
                case "Color [Red]":
                    hioki.COMnum = cb.Text;
                    if (hioki.ketnoi())
                    {
                        lb.BackColor = Color.Blue;
                        saveconfigdevice();
                    }
                    break;
                case "Color [Blue]":
                    hioki.ngatketnoi();
                    lb.BackColor = Color.Red;
                    break;
                default:
                    break;
            }
        }
        private void ketnoisdivboard(ComboBox cb, Label lb, clsSDIVBoard sdivboard)
        {
            switch (lb.BackColor.ToString())
            {
                case "Color [Red]":
                    sdivboard.COMnum = cb.Text;
                    if (sdivboard.ketnoi())
                    {
                        lb.BackColor = Color.Blue;
                        saveconfigdevice();
                    }
                    break;
                case "Color [Blue]":
                    sdivboard.ngatketnoi();
                    lb.BackColor = Color.Red;
                    break;
                default:
                    break;
            }
        }
        private void ketnoipws(ComboBox cb, Label lb, clsPWS  pws)
        {
            switch (lb.BackColor.ToString())
            {
                case "Color [Red]":
                    pws.COMnum = cb.Text;
                    if (pws.ketnoi())
                    {
                        lb.BackColor = Color.Blue;
                        saveconfigdevice();
                       // pws.PWS_OUTPUT(0.5, 20, "ON"); Sửa đổi check item button SW

                    }
                    break;
                case "Color [Blue]":
                    pws.ngatketnoi();
                    lb.BackColor = Color.Red;
                    break;
                default:
                    break;
            }
        }
       
        private void saveconfigdevice()
        {
            Config.saveconfig_device(cbscanner1, cbscanner2,cbscanner3,
                                    cbcomboard1, cbcomboard2,cbcomboard3,
                                    cbdaq1, cbdaq2,cbdaq3,
                                    cbdvm1, cbdvm2,cbdvm3,
                                    cbhioki1, cbhioki2,cbhioki3,
                                    cbpws1, cbpws2, cbpws3,
                                    cbsdivboard1, cbsdivboard2,cbsdivboard3);

        }
        private void autoconnectcom()
        {
            ketnoiscanner(cbscanner1, lbscanner1, scanner1);
            ketnoiscanner(cbscanner2, lbscanner2, scanner2);
            ketnoiscanner(cbscanner3, lbscanner3, scanner3);
            

            ketnoicomboard(cbcomboard1, lbcomboard1, com1);
            ketnoicomboard(cbcomboard2, lbcomboard2, com2);
            ketnoicomboard(cbcomboard3, lbcomboard3, com3);

            ketnoidvm(cbdvm1, lbdvm1, DVM1);
            ketnoidvm(cbdvm2, lbdvm2, DVM2);
            ketnoidvm(cbdvm3, lbdvm3, DVM3);
            

            ketnoihioki(cbhioki1, lbhioki1, HIOKI1);
            ketnoihioki(cbhioki2, lbhioki2, HIOKI2);
            ketnoihioki(cbhioki3, lbhioki3, HIOKI3);

           // ketnoipws()

            ketnoidaq(cbdaq1, lbdaq1, DAQ1);
            ketnoidaq(cbdaq2, lbdaq2, DAQ2);
            ketnoidaq(cbdaq3, lbdaq3, DAQ3);
           

            ketnoisdivboard(cbsdivboard1, lbsdivboard1, SDIVBoard1);
            ketnoisdivboard(cbsdivboard2, lbsdivboard2, SDIVBoard2); //board LED
            ketnoisdivboard(cbsdivboard3, lbsdivboard3, SDIVBoard3);

            ketnoipws(cbpws1, lbpws1, PWS1);
            ketnoipws(cbpws2, lbpws2, PWS2);
            ketnoipws(cbpws3, lbpws3, PWS3);
           


        }
        private void loadlastmodel(ComboBox cbx)
        {
            try
            {

                string model = Config.loadlastmodel();
                if (model.Substring(17, 2) == "14")
                {
                    PLC.Writeplc("Y14", 1);
                }
                else
                {
                    PLC.Writeplc("Y14", 0);
                }
                if (model != "")
                {

                    cbx.Text = model;
                    btnupdatespec.PerformClick();

                }
            }
            catch (Exception)
            {
                cblistmodel.SelectedIndex = 0;
            }
        }
        #endregion

        private void btnsaveoff1_Click(object sender, EventArgs e)
        {
            saveoffset();
            loadoffset();
        }

        private void btnsaveoff2_Click(object sender, EventArgs e)
        {
            saveoffset(); 
            loadoffset();
        }

        private void btnsaveoff3_Click(object sender, EventArgs e)
        {
            saveoffset();
            loadoffset();
        }

        private void btnscanner1_Click(object sender, EventArgs e)
        {
            ketnoiscanner(cbscanner1, lbscanner1, scanner1);
        }

        private void btnscanner2_Click(object sender, EventArgs e)
        {
            ketnoiscanner(cbscanner2, lbscanner2, scanner2);
        }

        private void btncomboard1_Click(object sender, EventArgs e)
        {
            ketnoicomboard(cbcomboard1, lbcomboard1, com1);
        }

        private void btndaq1_Click(object sender, EventArgs e)
        {
            ketnoidaq(cbdaq1, lbdaq1, DAQ1);
        }

        private void btndvm1_Click(object sender, EventArgs e)
        {
            ketnoidvm(cbdvm1, lbdvm1, DVM1);
        }

        private void btnhioki1_Click(object sender, EventArgs e)
        {
            ketnoihioki(cbhioki1, lbhioki1, HIOKI1);
        }

        private void btnsdivboard1_Click(object sender, EventArgs e)
        {
            ketnoisdivboard(cbsdivboard1, lbsdivboard1, SDIVBoard1);
        }

        private void cbscanner1_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbscanner1);
        }

        private void cbcomboard1_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbcomboard1);
        }

        private void cbdaq1_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbdaq1);
        }

        private void cbdvm1_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbdvm1);
        }

        private void cbhioki1_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbhioki1);
        }

        private void cbsdivboard1_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbsdivboard1);
        }

        private void btnupdatespec_Click(object sender, EventArgs e)
        {
            Config.loadlog(ref count1, ref time);
            if(dataconvert.comparedatelog(time)==0)
            {
                count1++;
                Config.saveqty(lbOK, lbNG, lbl_ok2, lbl_ng2, lbOK3, lbNG3, count1, DateTime.Now.ToString("yyyyMMdd"));
            }
            else
            {
                count1=1;
                Config.saveqty(lbOK, lbNG, lbl_ok2, lbl_ng2,lbOK3,lbNG3, count1, DateTime.Now.ToString("yyyyMMdd"));
            }
            
            packcode = tbpackcode.Text = cblistmodel.Text.Substring(1, 13);
            tbmodelname.Text = cblistmodel.Text.Substring(15, cblistmodel.Text.Length - 15);
            icno = tbic.Text = Localdb.searchic(tbpackcode.Text);
            Config.savemodelname(cblistmodel);


            if (cblistmodel.Text.Substring(17, 2) == "14")
            {
                PLC.Writeplc("Y13", 1);
            }
            else
            {
                PLC.Writeplc("Y13", 0);
            }

            //if (MES.checkconnection() == true && ProgramProcess != "OBA")
            //{
            //    //Thread thloading = new Thread(new ThreadStart(openloadingform));
            //    //thloading.IsBackground = true;
            //    //thloading.Start();

            //    MES.capnhattieuchuan(tbpackcode.Text, tbic.Text);

            //    //thloading.Abort();
            //}

            DataTable dt = new DataTable();
            dt = Localdb.loadspec(tbpackcode.Text);



            Localdb.loadBarcodeLen(ref CPlenmin, ref CPlenmax, ref HPlenmin, ref HPlenmax, dt, tbic.Text);

            hienthidata.hienthispec(dtvresult, dt);


            
            //lbcp3.Text = "";

           
            //lbhp3.Text = "";

            //Hien thi test items 
            dt = Localdb.loaditemcommand(icno, packcode);
            //hienthidata.hienthiitemcommandtheomodel(dtv_Schedule, dt);

            

            lbketqua1.Text = "SẴN SÀNG";
            lbketqua1.BackColor = Color.FromName("ControlDark");
            lbketqua1.Font = new Font("Times New Roman", 12);
            lbketqua1.ForeColor = Color.FromName("ControlText");

            lbketqua2.Text = "SẴN SÀNG";
            lbketqua2.BackColor = Color.FromName("ControlDark");
            lbketqua2.Font = new Font("Times New Roman", 12);
            lbketqua2.ForeColor = Color.FromName("ControlText");

            lbketqua3.Text = "SẴN SÀNG";
            lbketqua3.BackColor = Color.FromName("ControlDark");
            lbketqua3.Font = new Font("Times New Roman", 12);
            lbketqua3.ForeColor = Color.FromName("ControlText");

            Makelog.createlog(cblistmodel.Text, dtvresult,count1);
            Makelog.createlog_NG(cblistmodel.Text, dtvresult,count1);
            teststepch1 = 0;
        }

        private void btnclearqty_Click(object sender, EventArgs e)
        {
            Config.saveqty(lbOK, lbNG,lbl_ok2,lbl_ng2,lbOK3,lbNG3,count1,DateTime.Now.ToString("yyyyMMdd"));
            lbOK.Text = "0";
            lbNG.Text = "0";
            lbl_ok2.Text = "0";
            lbl_ng2.Text = "0";
            lbOK3.Text = "0";
            lbNG3.Text = "0";
        }
        
        private void cbpws1_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbpws1);
        }
       

        private void btnmaster1_Click(object sender, EventArgs e)
        {
            if (testch1.Testflag == false && chbmaster1.Checked == true)
            {
                timer3.Enabled = true;
                //stw1.Reset();
                //stw1.Start();
                testch1.Stopflag = false;
                ThreadStart t1 = new ThreadStart(testthread1);
                testkenh1 = new Thread(t1);
                testkenh1.Start();
            }
        }

       
        private void btnstop1_Click(object sender, EventArgs e)
        {
        
          lbketqua1.Font = new Font(FontFamily.GenericSansSerif, 16);
          lbketqua1.Text = "RESET";
          lbketqua1.BackColor = Color.YellowGreen;
          testch1.Stopflag = true;
          testch1.Testflag = false;
          testch1.error_flag = 1;
          //com1.Offcharge();
        }

        private void btnstop2_Click(object sender, EventArgs e)
        {
           
            lbketqua2.Font = new Font(FontFamily.GenericSansSerif, 16);
            lbketqua2.Text = "RESET";
            lbketqua2.BackColor = Color.YellowGreen;
            testch2.Stopflag = true;
            testch2.Testflag = false;
            testch2.error_flag = 1;
            //com2.Offcharge();
        }       
        private void cbscanner2_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbscanner2);
        }

        private void btnmaster2_Click(object sender, EventArgs e)
        {
            if (testch2.Testflag == false && chbmaster2.Checked == true)
            {
                timer3.Enabled = true;
                //stw2.Reset();
                //stw2.Start();
                testch2.Stopflag = false;
                ThreadStart t2 = new ThreadStart(testthread2);
                testkenh2 = new Thread(t2);
                testkenh2.Start();
            }
        }

        private void btnpws1_Click(object sender, EventArgs e)
        {
            ketnoipws(cbpws1, lbpws1, PWS1 );
        }

        private void btnpws2_Click(object sender, EventArgs e)
        {
            ketnoipws(cbpws2, lbpws2, PWS2);
        }

        

        private void button2_Click_1(object sender, EventArgs e)
        {
            PWS1.PWS_OUTPUT(double.Parse(txt_PWS_Iset_1.Text), double.Parse(txt_PWS_Vset_1 .Text ));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PWS1.PWS_ON();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PWS1.PWS_OFF();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void btn_len_on_Click(object sender, EventArgs e)
        {
            PWS1.PWS_OUTPUT(1, 20, "ON");
            Thread.Sleep(200);
            com1.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com1.WriteCommand_1W("DA", "31", "01");
            Thread.Sleep(1000);
           // PWS1.PWS_OUTPUT(0, 0, "OFF");
           
        }

        private void btn_led_off_Click(object sender, EventArgs e)
        {
            com1.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com1.WriteCommand_1W("DA", "34", "01");
            PWS1.PWS_OUTPUT(0, 0, "OFF");
        }

        private void btn_led_chan_Click(object sender, EventArgs e)
        {
            PWS1.PWS_OUTPUT(1, 20, "ON");
            Thread.Sleep(200);
            com1.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com1.WriteCommand_1W("DA", "33", "01");
        }

        private void btn_led_le_Click(object sender, EventArgs e)
        {
            PWS1.PWS_OUTPUT(1, 20, "ON");
            Thread.Sleep(200);
            com1.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com1.WriteCommand_1W("DA", "32", "01");
            PWS1.PWS_OUTPUT(0, 0, "0FF");
        }
 

        private void btn_led21_Click(object sender, EventArgs e)
        {
            if (txt_led21os.Text == "")
            {
                MessageBox.Show("Bạn hãy thiết lập ngưỡng sau khi hiệu chuẩn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SDIVBoard2.offset(txt_led21os.Text,"1");
            }
        }

        private void btn_led_on2_Click(object sender, EventArgs e)
        {
            PWS2.PWS_OUTPUT(1, 20, "ON");
            Thread.Sleep(200);
            com2.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com2.WriteCommand_1W("DA", "31", "01");
        }

        private void btn_led_off2_Click(object sender, EventArgs e)
        {
            com2.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com2.WriteCommand_1W("DA", "34", "01");
            Thread.Sleep(1000);
            PWS2.PWS_OUTPUT(0, 0, "OFF");
        }

        private void btn_led_chan2_Click(object sender, EventArgs e)
        {
            com2.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com2.WriteCommand_1W("DA", "33", "01");
        }

        private void btn_led_le2_Click(object sender, EventArgs e)
        {
            com2.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com2.WriteCommand_1W("DA", "32", "01");
        }


        private void btncomboard2_Click(object sender, EventArgs e)
        {
            ketnoicomboard(cbcomboard2, lbcomboard2, com2);
        }

        private void btndaq2_Click(object sender, EventArgs e)
        {
            ketnoidaq(cbdaq2, lbdaq2, DAQ2);
        }

        private void btndvm2_Click(object sender, EventArgs e)
        {
            ketnoidvm(cbdvm2, lbdvm2, DVM2);
        }

        private void btnhioki2_Click(object sender, EventArgs e)
        {
            ketnoihioki(cbhioki2, lbhioki2, HIOKI2);
        }

        private void btnsdivboard2_Click(object sender, EventArgs e)
        {
            ketnoisdivboard(cbsdivboard2, lbsdivboard2, SDIVBoard2);
        }

        private void cbcomboard2_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbcomboard2);
        }

        private void cbdaq2_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbdaq2);
        }

        private void cbdvm2_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbdvm2);
        }

        private void cbhioki2_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbhioki2);
        }

        private void cbpws2_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbpws2);
        }

        private void cbsdivboard2_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbsdivboard2);
        }

        private void btnsdivboard3_Click(object sender, EventArgs e)
        {
            ketnoisdivboard(cbsdivboard3, lbsdivboard3, SDIVBoard3);
        }

        

        private void cbsdivboard3_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbsdivboard3);
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(chx_tudong.Checked)
            {
                cbx_confirm.Checked = false;
                cbx_confirm.Enabled = false;
            }
            else
            {
                cbx_confirm.Enabled = true;
            }

            if(cbx_confirm.Checked)
            {
                if(txt_test_No.Text==""||txt_delay_time.Text=="")
                {
                    MessageBox.Show("Input data", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    
                    chx_tudong.Checked = false;
                    chx_tudong.Enabled = false;
                    chbcheckfail.Checked = false;
                    chbcheckfail.Enabled = false;
                    
                }               
            }
            else
            {
                chx_tudong.Enabled = true;
            }
            if(PLC.readplc("X0")=="0")
            {
                cbx_confirm.Checked = false;
                txt_delay_time.Text = "";
                txt_test_No.Text = "";
            }
            lb_datetime.Text = DateTime.Now.ToString();
            try
            {
                lbl_total_OK.Text = (int.Parse(lbOK.Text) + int.Parse(lbl_ok2.Text) + int.Parse(lbOK3.Text)).ToString();
                lbl_total_NG.Text = (int.Parse(lbNG.Text) + int.Parse(lbl_ng2.Text) + int.Parse(lbNG3.Text)).ToString();

                lbl_total_rate.Text = (float.Parse(lbl_tile.Text)/3+float.Parse(lbl_tile2.Text)/3+float.Parse(lbl_tile3.Text)/3).ToString();

                if (testch1.Testflag == false && testch2.Testflag == false && testch3.Testflag == false&&count==0)
                {
                    txt_wait_time.Text = stw4.Elapsed.ToString().Substring(3, 8);
                }
            }
            catch (Exception)
            {
                
               
            }
            
            
        }

        private void chx_tudong_CheckedChanged(object sender, EventArgs e)
        {
            if (chx_tudong.Checked)
            {
                PLC.Writeplc("M14", 1);
                timer2.Enabled = true;
                timer6.Enabled = false;
            }
            else
            {
                PLC.Writeplc("M14", 0);
                timer2.Enabled = false;
                timer6.Enabled = true;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {          


            if (PLC.readplc("X4") == "1" && PLC.readplc("M14") == "1")
            {
                //txt_wait_time.Text = stw4.Elapsed.ToString().Substring(3, 8);

                PLC.Writeplc("M2", 0);
                PLC.Writeplc("M3", 0);
                PLC.Writeplc("M4", 0);
                PLC.Writeplc("M5", 0);
                PLC.Writeplc("M6", 0);
                PLC.Writeplc("M7", 0);

                PLC.Writeplc("M11", 0);
                PLC.Writeplc("M12", 0);
                PLC.Writeplc("M13", 0);
                PLC.Writeplc("Y2", 0);
                PLC.Writeplc("Y3", 0);
                PLC.Writeplc("Y4", 0);
            }


            if (PLC.readplc("M8") == "1" || PLC.readplc("M9") == "1" || PLC.readplc("M10") == "1")
            {
                //timer4.Enabled = false;
                PLC.Writeplc("Y14", 0);
                PLC.Writeplc("Y15", 0);
                PLC.Writeplc("Y16", 0);

                PLC.Writeplc("Y20", 0);
                PLC.Writeplc("Y21", 0);
                PLC.Writeplc("Y22", 0);
               
                if (count == 0)
                {
                    hienthidata.clearresult(dtvresult, lbketqua1, pgb1, 1);
                    hienthidata.clearresult(dtvresult, lbketqua2, pgb2, 2);
                    hienthidata.clearresult(dtvresult, lbketqua3, pgb3, 3);
                    count++;
                    if ((stw1.Elapsed > stw2.Elapsed) && (stw1.Elapsed > stw3.Elapsed))
                    {

                        TimeSpan timespan = (stw1.Elapsed + stw4.Elapsed);
                        txt_total_time.Text = timespan.ToString().Substring(3, 8);
                        stw4.Stop();
                    }
                    else if ((stw2.Elapsed > stw1.Elapsed) && (stw2.Elapsed > stw3.Elapsed))
                    {
                        TimeSpan timespan = (stw2.Elapsed + stw4.Elapsed);
                        txt_total_time.Text = timespan.ToString().Substring(3, 8);
                        stw4.Stop();
                    }
                    else if ((stw3.Elapsed > stw2.Elapsed) && (stw3.Elapsed > stw1.Elapsed))
                    {
                        TimeSpan timespan = (stw3.Elapsed + stw4.Elapsed);
                        txt_total_time.Text = timespan.ToString().Substring(3, 8);
                        stw4.Stop();
                    }
                    else
                    {
                        TimeSpan timespan = (stw1.Elapsed + stw4.Elapsed);
                        txt_total_time.Text = timespan.ToString().Substring(3, 8);
                        stw4.Stop();
                    }
                }
                //timer3.Enabled = true;


                if (PLC.readplc("M8") == "1" & testch1.Testflag == false)
                {
                    PLC.Writeplc("M11", 1);

                    //PLC.Writeplc("Y2", 1);

                    testch1.Stopflag = false;
                    Thread.Sleep(100);
                  
                    ThreadStart t1 = new ThreadStart(testthread1);
                    testkenh1 = new Thread(t1);
                    testkenh1.Start();


                }

                if (PLC.readplc("M9") == "1" & testch2.Testflag == false)
                {
                    //testch2.Testflag = true;
                    PLC.Writeplc("M12", 1);
                    
                    //PLC.Writeplc("Y3", 1);
                    

                    testch2.Stopflag = false;
                    Thread.Sleep(100);
                    
                    ThreadStart t2 = new ThreadStart(testthread2);
                    testkenh2 = new Thread(t2);
                    testkenh2.Start();
                }

                if (PLC.readplc("M10") == "1" & testch3.Testflag == false)
                {
                    PLC.Writeplc("M13", 1);
                   
                    //PLC.Writeplc("Y4", 1);
                    

                    testch3.Stopflag = false;
                    Thread.Sleep(100);
                   
                    ThreadStart t3 = new ThreadStart(testthread3);
                    testkenh3 = new Thread(t3);
                    testkenh3.Start();
                }
                timer3.Enabled = true;
            }
            else if (PLC.readplc("X0") == "0")
            {
                try
                {
                    if (testkenh1.IsAlive == true) testkenh1.Abort(); // Ngắt hoạt động thread test CH1
                    if (testkenh2.IsAlive == true) testkenh2.Abort(); // Ngắt hoạt động thread test CH2
                    if (testkenh3.IsAlive == true) testkenh3.Abort(); // Ngắt hoạt động thread test CH3
                    if (runTactTime.IsAlive == true) runTactTime.Abort();
                }
                catch (Exception)
                { }

                lbketqua1.Font = new Font(FontFamily.GenericSansSerif, 16);
                lbketqua1.Text = "RESET";
                lbketqua1.BackColor = Color.YellowGreen;
                testch1.Stopflag = true;
                testch1.Testflag = false;
                testch1.error_flag = 1;
                //PLC.Writeplc("Y2", 0);

                lbketqua2.Font = new Font(FontFamily.GenericSansSerif, 16);
                lbketqua2.Text = "RESET";
                lbketqua2.BackColor = Color.YellowGreen;
                testch2.Stopflag = true;
                testch2.Testflag = false;
                testch2.error_flag = 1;
                //PLC.Writeplc("Y3", 0);

                lbketqua3.Font = new Font(FontFamily.GenericSansSerif, 16);
                lbketqua3.Text = "RESET";
                lbketqua3.BackColor = Color.YellowGreen;
                testch3.Stopflag = true;
                testch3.Testflag = false;
                testch3.error_flag = 1;
                //PLC.Writeplc("Y4", 0);                

                flag_tactTime = true;
                flag_testFinish = true;

                PLC.Writeplc("M11", 0);
                PLC.Writeplc("M12", 0);
                PLC.Writeplc("M13", 0);
            }              
            
        }

        public void TactTime()
        {
            _check:
            try
            {
                while (testch1.Testflag || testch2.Testflag || testch3.Testflag)
                {
                    if (testch1.Testflag == true)
                    {
                        txt_time1.Text = stw1.Elapsed.ToString().Substring(3, 8);
                    }
                    else
                    {
                        stw1.Stop();
                    }
                    if (testch2.Testflag == true)
                    {
                        txt_time2.Text = stw2.Elapsed.ToString().Substring(3, 8);
                    }
                    else
                    {
                        stw2.Stop();
                    }
                    if (testch3.Testflag == true)
                    {
                        txt_time3.Text = stw3.Elapsed.ToString().Substring(3, 8);
                    }
                    else
                    {
                        stw3.Stop();
                    }
                }
            }
            catch (Exception)
            {
                goto _check;
            }
            flag_tactTime = true;
            flag_testFinish = true;
            stw4.Reset();
            stw4.Start();
            count = 0;
        }

        public void testFinish()
        {
            PLC.Writeplc("M2", 0);
            PLC.Writeplc("M3", 0);
            PLC.Writeplc("M4", 0);
            PLC.Writeplc("M5", 0);
            PLC.Writeplc("M6", 0);
            PLC.Writeplc("M7", 0);

            PLC.Writeplc("M11", 0);
            PLC.Writeplc("M12", 0);
            PLC.Writeplc("M13", 0);
            PLC.Writeplc("Y2", 0);
            PLC.Writeplc("Y3", 0);
            PLC.Writeplc("Y4", 0); 
        }


        private void timer3_Tick(object sender, EventArgs e)
        {
            if ((testch1.Testflag && testch2.Testflag && testch3.Testflag) && flag_tactTime)
            {
                flag_tactTime = false;
                runTactTime = new Thread(new ThreadStart(TactTime));
                runTactTime.IsBackground = true;
                runTactTime.Start();
                timer3.Enabled = false;
            }

           
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            //txt_time2.Text = stw2.Elapsed.ToString().Substring(3,8);
            //if (testch2.Stopflag == true || testch2.ktrkq != 0)
            //{
            //    timer4.Enabled = false;
            //}
        }

       

        private void timer6_Tick(object sender, EventArgs e)
        {           
            if (PLC.readplc("X0") == "0")
            {
                lbl_EMG.BackColor = Color.Blue;
            }
            else
            {
                lbl_EMG.BackColor = Color.Red;
            }
            if (PLC.readplc("X1") == "1")
            {
                lbl_start1.BackColor = Color.Blue;
            }
            else
            {
                lbl_start1.BackColor = Color.Red;
            }
            if (PLC.readplc("X2") == "1")
            {
                lbl_start2.BackColor = Color.Blue;
            }
            else
            {
                lbl_start2.BackColor = Color.Red;
            }
            if (PLC.readplc("X3") == "1")
            {
                lbl_safety.BackColor = Color.Blue;
            }
            else
            {
                lbl_safety.BackColor = Color.Red;
            }
            if (PLC.readplc("X4") == "1")
            {
                lbl_up_sen.BackColor = Color.Blue;
            }
            else
            {
                lbl_up_sen.BackColor = Color.Red;
            }
            if (PLC.readplc("X5") == "1")
            {
                lbl_down_sen.BackColor = Color.Blue;
            }
            else
            {
                lbl_down_sen.BackColor = Color.Red;
            }
            if (PLC.readplc("X6") == "1")
            {
                lbl_PCM1_sen.BackColor = Color.Blue;
            }
            else
            {
                lbl_PCM1_sen.BackColor = Color.Red;
            }
            if (PLC.readplc("X7") == "1")
            {
                lbl_PCM2_sen.BackColor = Color.Blue;
            }
            else
            {
                lbl_PCM2_sen.BackColor = Color.Red;
            }
            if (PLC.readplc("X10") == "1")
            {
                lbl_PCM3_sen.BackColor = Color.Blue;
            }
            else
            {
                lbl_PCM3_sen.BackColor = Color.Red;
            }
        }

        private void btn_up_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("M22", 0);
            PLC.Writeplc("M23", 1);
        }

        private void btn_down_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("M23", 0);
            PLC.Writeplc("M22", 1);
        }

        private void btn_cnt1_insert_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("M19", 1);
        }

        private void btn_cnt1_out_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("M19", 0);
        }

        private void btn_cnt2_insert_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("M20", 1);
        }

        private void btn_cnt2_out_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("M20", 0);
        }

        private void btn_sw1_insert_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y5", 1);
        }

        private void btn_sw1_out_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y5", 0);
        }

        private void btn_sw2_insert_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y6", 1);
        }

        private void btn_sw2_out_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y6", 0);
        }

       

        private void btn_led2_ok_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y21", 0);
            PLC.Writeplc("Y15", 1);
        }

        private void btn_led2_NG_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y15", 0);
            PLC.Writeplc("Y21", 1);
        }

        private void btn_vcc1_on_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y10", 1);
        }

        private void btn_vcc1_off_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y10", 0);
        }

        private void btn_vcc2_on_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y11", 1);
        }

        private void btn_vcc2_off_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y11", 0);
        }

        private void timer4_Tick_1(object sender, EventArgs e)
        {            
            
        }

        private void chbmaster1_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void chbmaster2_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void btn_select_history_Click(object sender, EventArgs e)
        {
            //string filename;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();
            txt_filepatch.Text = dlg.FileName;


            Localdb.bindataCSV(txt_filepatch.Text, dt_history);

        }

        private void Frmmain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Config.saveqty(lbOK, lbNG, lbl_ok2, lbl_ng2,lbOK3,lbNG3,count1,DateTime.Now.ToString("yyyyMMdd"));
        }

        private void btn_LED1ON_Click(object sender, EventArgs e)
        {
            PWS1.PWS_OUTPUT(1, 20, "ON");
            Thread.Sleep(200);
            com1.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com1.WriteCommand_1W("DA", "31", "01");
            //Thread.Sleep(1000);
        }

        private void btn_LED1OFF_Click(object sender, EventArgs e)
        {
            com1.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com1.WriteCommand_1W("DA", "34", "01");
            //PWS1.PWS_OUTPUT(0, 0, "OFF");
        }

        private void btn_LED1chan_Click(object sender, EventArgs e)
        {
            PWS1.PWS_OUTPUT(1, 20, "ON");
            Thread.Sleep(200);
            com1.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com1.WriteCommand_1W("DA", "33", "01");
        }

        private void btn_LED1le_Click(object sender, EventArgs e)
        {
            PWS1.PWS_OUTPUT(1, 20, "ON");
            Thread.Sleep(1000);
            com1.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com1.WriteCommand_1W("DA", "32", "01");
            
        }

        private void btn_LED1hieuchuan_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn không?", "Yes or No", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                PWS1.PWS_OUTPUT(1, 20, "ON");
                Thread.Sleep(200);
                setflag1 = true;
                clt1 = false;
                cld1 = false;
                MessageBox.Show("Yêu cầu đặt PCM có Led và kết nối với Jig để kiểm tra", "Thông báo");
                lbl_led1bd.BackColor = Color.Blue;
                lbl_led1tren.BackColor = Color.Black;
                lbl_led1duoi.BackColor = Color.Black;
                lbl_led1tc.BackColor = Color.Black;
                Thread.Sleep(800);
                com1.WriteCommand_1W("D9", "A596", "02");
                Thread.Sleep(100);
                com1.WriteCommand_1W("DA", "32", "01");
                Thread.Sleep(500);
                SDIVBoard1.hieuchuanmuctren();
                
                do
                {
                    lbl_led1bd.BackColor = Color.Blue;
                    lbl_led1tren.BackColor = Color.Blue;
                    lbl_led1duoi.BackColor = Color.Black;
                    lbl_led1tc.BackColor = Color.Black;
                    Thread.Sleep(1000);
                } while (SDIVBoard1.Clt == false);
                com1.WriteCommand_1W("DA", "33", "01");
                Thread.Sleep(500);
                SDIVBoard1.hieuchuanmucduoi();
               
                do
                {
                    lbl_led1bd.BackColor = Color.Blue;
                    lbl_led1tren.BackColor = Color.Blue;
                    lbl_led1duoi.BackColor = Color.Blue;
                    lbl_led1tc.BackColor = Color.Black;
                    Thread.Sleep(1000);
                } while (SDIVBoard1.Cld == false);

                lbl_led1bd.BackColor = Color.Blue;
                lbl_led1tren.BackColor = Color.Blue;
                lbl_led1duoi.BackColor = Color.Blue;
                lbl_led1tc.BackColor = Color.Blue;
                Thread.Sleep(1000);
                com1.WriteCommand_1W("D9", "A5A5", "02");
                cld1 = false;
                clt1 = false;
                setflag1 = false;
                PWS1.PWS_OUTPUT(0, 0, "OFF");
            }
            else
            {
                PWS1.PWS_OUTPUT(0, 0, "OFF");
            }
        }

        private void btn_LED1thietlap_Click(object sender, EventArgs e)
        {
            SDIVBoard1.thietlapnguong(ref den1, ref den2, ref den3, ref den4);

            txt_led11os.Text = den1.ToString();
            txt_led12os.Text = den2.ToString();
            txt_led13os.Text = den3.ToString();
            txt_led14os.Text = den4.ToString();
        }

        private void btn_LED1ADC_Click(object sender, EventArgs e)
        {
            SDIVBoard1.docADC(ref den1, ref den2, ref den3, ref den4);
            txt_led11os.Text = den1.ToString();
            txt_led12os.Text = den2.ToString();
            txt_led13os.Text = den3.ToString();
            txt_led14os.Text = den4.ToString();
        }

        private void btn_LED1kq_Click(object sender, EventArgs e)
        {
            //SDIVBoard1.docketqua();
            MessageBox.Show(SDIVBoard1.docketqua());
        }

        private void btn_led2on_Click(object sender, EventArgs e)
        {
            PWS2.PWS_OUTPUT(1, 20, "ON");
            Thread.Sleep(200);
            com2.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com2.WriteCommand_1W("DA", "31", "01");
            Thread.Sleep(1000);
        }

        private void btn_led2off_Click(object sender, EventArgs e)
        {
            com2.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com2.WriteCommand_1W("DA", "34", "01");
            Thread.Sleep(1000);
            //PWS2.PWS_OUTPUT(0, 0, "OFF");


            
        }

        private void btn_led2chan_Click(object sender, EventArgs e)
        {
            com2.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com2.WriteCommand_1W("DA", "33", "01");
        }

        private void btn_led2le_Click(object sender, EventArgs e)
        {
            PWS1.PWS_OUTPUT(1, 20, "ON");
            Thread.Sleep(1000);
            com2.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com2.WriteCommand_1W("DA", "32", "01");
        }

        private void btn_led2hieuchuan_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn không?", "Yes or No", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                PWS2.PWS_OUTPUT(1, 20, "ON");
                Thread.Sleep(200);
                setflag2 = true;
                clt2 = false;
                cld2 = false;
                MessageBox.Show("Yêu cầu đặt PCM có Led và kết nối với Jig để kiểm tra", "Thông báo");
                lbl_led2bd.BackColor = Color.Blue;
                lbl_led2tren.BackColor = Color.Black;
                lbl_led2duoi.BackColor = Color.Black;
                lbl_led2tc.BackColor = Color.Black;
                Thread.Sleep(800);
                com2.WriteCommand_1W("D9", "A596", "02");
                Thread.Sleep(100);
                com2.WriteCommand_1W("DA", "32", "01");
                Thread.Sleep(500);
                SDIVBoard2.hieuchuanmuctren();
               
                do
                {
                    lbl_led2bd.BackColor = Color.Blue;
                    lbl_led2tren.BackColor = Color.Blue;
                    lbl_led2duoi.BackColor = Color.Black;
                    lbl_led2tc.BackColor = Color.Black;
                    Thread.Sleep(1000);
                } while (SDIVBoard2.Clt == false);
                com2.WriteCommand_1W("DA", "33", "01");
                Thread.Sleep(500);
                SDIVBoard2.hieuchuanmucduoi();
                do
                {
                    lbl_led2bd.BackColor = Color.Blue;
                    lbl_led2tren.BackColor = Color.Blue;
                    lbl_led2duoi.BackColor = Color.Blue;
                    lbl_led2tc.BackColor = Color.Black;
                    Thread.Sleep(1000);
                } while (SDIVBoard2.Cld == false);

                lbl_led2bd.BackColor = Color.Blue;
                lbl_led2tren.BackColor = Color.Blue;
                lbl_led2duoi.BackColor = Color.Blue;
                lbl_led2tc.BackColor = Color.Blue;
                Thread.Sleep(1000);
                com2.WriteCommand_1W("D9", "A5A5", "02");
                cld2 = false;
                clt2 = false;
                setflag2 = false;
                PWS2.PWS_OUTPUT(0, 0, "OFF");
            }
            else
            {
                PWS2.PWS_OUTPUT(0, 0, "OFF");
            }
        }

        private void btn_led2adc_Click(object sender, EventArgs e)
        {
            SDIVBoard2.docADC(ref den1, ref den2, ref den3, ref den4);
            txt_led21os.Text = den1.ToString();
            txt_led22os.Text = den2.ToString();
            txt_led23os.Text = den3.ToString();
            txt_led24os.Text = den4.ToString();
        }

        private void btn_led2kq_Click(object sender, EventArgs e)
        {
            MessageBox.Show(SDIVBoard1.docketqua());
        }

        private void btn_led3on_Click(object sender, EventArgs e)
        {
            PWS3.PWS_OUTPUT(1, 20, "ON");
            Thread.Sleep(200);
            com3.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com3.WriteCommand_1W("DA", "31", "01");
            Thread.Sleep(1000);
        }

        private void btn_led3off_Click(object sender, EventArgs e)
        {
            com3.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com3.WriteCommand_1W("DA", "34", "01");
            Thread.Sleep(1000);
            //PWS3.PWS_OUTPUT(0, 0, "OFF");
        }

        private void btn_led3chan_Click(object sender, EventArgs e)
        {
            com3.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com3.WriteCommand_1W("DA", "33", "01");
        }

        private void btn_led3le_Click(object sender, EventArgs e)
        {
            PWS1.PWS_OUTPUT(1, 20, "ON");
            Thread.Sleep(1000);
            com3.WriteCommand_1W("D9", "A596", "02");
            Thread.Sleep(200);
            com3.WriteCommand_1W("DA", "32", "01");
        }

        private void btn_led3hieuchuan_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn không?", "Yes or No", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                PWS3.PWS_OUTPUT(1, 20, "ON");
                Thread.Sleep(200);
                setflag3 = true;
                clt3 = false;
                cld3 = false;
                MessageBox.Show("Yêu cầu đặt PCM có Led và kết nối với Jig để kiểm tra", "Thông báo");
                lbl_led3bd.BackColor = Color.Blue;
                lbl_led3tren.BackColor = Color.Black;
                lbl_led3duoi.BackColor = Color.Black;
                lbl_led3tc.BackColor = Color.Black;
                Thread.Sleep(800);
                com3.WriteCommand_1W("D9", "A596", "02");
                Thread.Sleep(100);
                com3.WriteCommand_1W("DA", "32", "01");
                Thread.Sleep(500);
                SDIVBoard3.hieuchuanmuctren();
                
                do
                {
                    lbl_led3bd.BackColor = Color.Blue;
                    lbl_led3tren.BackColor = Color.Blue;
                    lbl_led3duoi.BackColor = Color.Black;
                    lbl_led3tc.BackColor = Color.Black;
                    Thread.Sleep(1000);
                } while (SDIVBoard3.Clt == false);
                com3.WriteCommand_1W("DA", "33", "01");
                Thread.Sleep(500);
                SDIVBoard3.hieuchuanmucduoi();
                do
                {
                    lbl_led3bd.BackColor = Color.Blue;
                    lbl_led3tren.BackColor = Color.Blue;
                    lbl_led3duoi.BackColor = Color.Blue;
                    lbl_led3tc.BackColor = Color.Black;
                    Thread.Sleep(1000);
                } while (SDIVBoard3.Cld == false);

                lbl_led3bd.BackColor = Color.Blue;
                lbl_led3tren.BackColor = Color.Blue;
                lbl_led3duoi.BackColor = Color.Blue;
                lbl_led3tc.BackColor = Color.Blue;
                Thread.Sleep(1000);
                com3.WriteCommand_1W("D9", "A5A5", "02");
                cld3 = false;
                clt3 = false;
                setflag3 = false;
                PWS3.PWS_OUTPUT(0, 0, "OFF");
            }
            else
            {
                PWS3.PWS_OUTPUT(0, 0, "OFF");
            }
        }

        private void btn_led2thietlap_Click(object sender, EventArgs e)
        {
            SDIVBoard2.thietlapnguong(ref den1, ref den2, ref den3, ref den4);
            txt_led21os.Text = den1.ToString();
            txt_led22os.Text = den2.ToString();
            txt_led23os.Text = den3.ToString();
            txt_led24os.Text = den4.ToString();
        }

        private void btn_led3thietlap_Click(object sender, EventArgs e)
        {
            SDIVBoard3.thietlapnguong(ref den1, ref den2, ref den3, ref den4);
            txt_led31os.Text = den1.ToString();
            txt_led32os.Text = den2.ToString();
            txt_led33os.Text = den3.ToString();
            txt_led34os.Text = den4.ToString();
        }

        private void btn_led3adc_Click(object sender, EventArgs e)
        {
            SDIVBoard3.docADC(ref den1, ref den2, ref den3, ref den4);
            txt_led31os.Text = den1.ToString();
            txt_led32os.Text = den2.ToString();
            txt_led33os.Text = den3.ToString();
            txt_led34os.Text = den4.ToString();
        }

        private void btn_led3kq_Click(object sender, EventArgs e)
        {
            MessageBox.Show(SDIVBoard1.docketqua());
        }

        private void btn_LED11os_Click(object sender, EventArgs e)
        {
            if (txt_led11os.Text == "")
            {
                MessageBox.Show("Bạn hãy thiết lập ngưỡng sau khi hiệu chuẩn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SDIVBoard1.offset(txt_led11os.Text, "1");
            }
        }

        private void btn_LED12os_Click(object sender, EventArgs e)
        {
            if (txt_led12os.Text == "")
            {
                MessageBox.Show("Bạn hãy thiết lập ngưỡng sau khi hiệu chuẩn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SDIVBoard1.offset(txt_led12os.Text, "2");
            }
        }

        private void btn_LED13os_Click(object sender, EventArgs e)
        {
            if (txt_led13os.Text == "")
            {
                MessageBox.Show("Bạn hãy thiết lập ngưỡng sau khi hiệu chuẩn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SDIVBoard1.offset(txt_led13os.Text, "3");
            }
        }

        private void btn_LED14os_Click(object sender, EventArgs e)
        {
            if (txt_led14os.Text == "")
            {
                MessageBox.Show("Bạn hãy thiết lập ngưỡng sau khi hiệu chuẩn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SDIVBoard1.offset(txt_led14os.Text, "4");
            }
        }

        private void btn_led21os_Click(object sender, EventArgs e)
        {
            if (txt_led21os.Text == "")
            {
                MessageBox.Show("Bạn hãy thiết lập ngưỡng sau khi hiệu chuẩn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SDIVBoard2.offset(txt_led21os.Text, "1");
            }
        }

        private void btn_led22os_Click(object sender, EventArgs e)
        {
            if (txt_led22os.Text == "")
            {
                MessageBox.Show("Bạn hãy thiết lập ngưỡng sau khi hiệu chuẩn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SDIVBoard2.offset(txt_led22os.Text, "2");
            }
        }

        private void btn_led23os_Click(object sender, EventArgs e)
        {
            if (txt_led23os.Text == "")
            {
                MessageBox.Show("Bạn hãy thiết lập ngưỡng sau khi hiệu chuẩn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SDIVBoard2.offset(txt_led23os.Text, "3");
            }
        }

        private void btn_led24os_Click(object sender, EventArgs e)
        {
            if (txt_led24os.Text == "")
            {
                MessageBox.Show("Bạn hãy thiết lập ngưỡng sau khi hiệu chuẩn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SDIVBoard2.offset(txt_led24os.Text, "4");
            }
        }

        private void btn_led31os_Click(object sender, EventArgs e)
        {
            if (txt_led31os.Text == "")
            {
                MessageBox.Show("Bạn hãy thiết lập ngưỡng sau khi hiệu chuẩn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SDIVBoard3.offset(txt_led31os.Text, "1");
            }
        }

        private void btn_led32os_Click(object sender, EventArgs e)
        {
            if (txt_led32os.Text == "")
            {
                MessageBox.Show("Bạn hãy thiết lập ngưỡng sau khi hiệu chuẩn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SDIVBoard3.offset(txt_led32os.Text, "1");
            }
        }

        private void btn_led33os_Click(object sender, EventArgs e)
        {
            if (txt_led33os.Text == "")
            {
                MessageBox.Show("Bạn hãy thiết lập ngưỡng sau khi hiệu chuẩn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SDIVBoard3.offset(txt_led33os.Text, "1");
            }
        }

        private void btn_led34os_Click(object sender, EventArgs e)
        {
            if (txt_led34os.Text == "")
            {
                MessageBox.Show("Bạn hãy thiết lập ngưỡng sau khi hiệu chuẩn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SDIVBoard3.offset(txt_led34os.Text, "1");
            }
        }

        private void btnscanner3_Click(object sender, EventArgs e)
        {
            ketnoiscanner(cbscanner3, lbscanner3, scanner3);
        }

        private void btncomboard3_Click(object sender, EventArgs e)
        {
            ketnoicomboard(cbcomboard3, lbcomboard3, com3);
        }

        private void btndaq3_Click(object sender, EventArgs e)
        {
            ketnoidaq(cbdaq3, lbdaq3, DAQ3);
        }

        private void btndvm3_Click(object sender, EventArgs e)
        {
            ketnoidvm(cbdvm3, lbdvm3, DVM3);
        }

        private void btnhioki3_Click(object sender, EventArgs e)
        {
            ketnoihioki(cbhioki3, lbhioki3, HIOKI3);
        }

        private void btnpws3_Click(object sender, EventArgs e)
        {
            ketnoipws(cbpws3, lbpws3, PWS3);
        }

        private void cbscanner3_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbscanner3);
        }

        private void cbcomboard3_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbcomboard3);
        }

        private void cbdaq3_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbdaq3);
        }

        private void cbdvm3_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbdvm3);
        }

        private void cbhioki3_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbhioki3);
        }

        private void cbpws3_Click(object sender, EventArgs e)
        {
            Config.loadlistcom(cbpws3);
        }

        private void btnsaveoff3_Click_1(object sender, EventArgs e)
        {
            saveoffset();
            loadoffset();
        }

        private void btnmaster3_Click(object sender, EventArgs e)
        {
            if (testch3.Testflag == false && chbmaster3.Checked == true)
            {
                timer3.Enabled = true;
                //stw2.Reset();
                //stw2.Start();
                testch3.Stopflag = false;
                ThreadStart t3 = new ThreadStart(testthread3);
                testkenh3 = new Thread(t3);
                testkenh3.Start();
            }
        }

        private void btnstop3_Click(object sender, EventArgs e)
        {
            lbketqua3.Font = new Font(FontFamily.GenericSansSerif, 16);
            lbketqua3.Text = "RESET";
            lbketqua3.BackColor = Color.YellowGreen;
            testch3.Stopflag = true;
            testch3.Testflag = false;
            testch3.error_flag = 1;
            //com2.Offcharge();
        }

        private void btn_cnt3_insert_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("M21", 1);
        }

        private void btn_cnt3_out_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("M21", 0);
        }

        private void btn_sw3_insert_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y7", 1);
        }

        private void btn_sw3_out_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y7", 0);
        }

        private void btn_vcc3_on_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y12", 1);
        }

        private void btn_vcc3_off_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y12", 0);
        }

        private void btn_led1_ng_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y14", 0);
            PLC.Writeplc("Y20", 1);
        }

        private void btn_led3_ng_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y16", 0);
            PLC.Writeplc("Y22", 1);
        }

        private void btn_led3_ok_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y22", 0);
            PLC.Writeplc("Y16", 1);
        }

        private void btn_led1_ok_Click(object sender, EventArgs e)
        {
            PLC.Writeplc("Y20", 0);
            PLC.Writeplc("Y14", 1);
        }

        private void cbx_confirm_CheckedChanged(object sender, EventArgs e)
        {
            if(cbx_confirm.Checked)
            {
                txt_delay_time.Enabled = false;
                txt_test_No.Enabled = false;
            }
            else
            {
                txt_delay_time.Enabled = true;
                txt_test_No.Enabled = true;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    tabSlected = 0;
                    break;
                case 1:
                    tabSlected = 1;
                    break;
                case 2:

                    break;
                case 3:

                    break;
                case 4:

                    break;
                case 5:

                    break;

            }
        }   

    }
}

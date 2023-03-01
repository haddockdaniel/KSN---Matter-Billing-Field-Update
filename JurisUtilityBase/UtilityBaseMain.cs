using System;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using Gizmox.Controls;
using JDataEngine;
using JurisAuthenticator;
using JurisUtilityBase.Properties;



namespace JurisUtilityBase
{
    public partial class UtilityBaseMain : Form
    {
        #region Private  members

        private JurisUtility _jurisUtility;

        #endregion

        #region Public properties

        public string CompanyCode { get; set; }

        public string JurisDbName { get; set; }

        public string JBillsDbName { get; set; }

        public int FldClient { get; set; }

        public int FldMatter { get; set; }

        #endregion

        #region Constructor

        public UtilityBaseMain()
        {
            InitializeComponent();
            _jurisUtility = new JurisUtility();
        }

        #endregion

        #region Public methods

        public void LoadCompanies()
        {
            var companies = _jurisUtility.Companies.Cast<object>().Cast<Instance>().ToList();
            //            listBoxCompanies.SelectedIndexChanged -= listBoxCompanies_SelectedIndexChanged;
            listBoxCompanies.ValueMember = "Code";
            listBoxCompanies.DisplayMember = "Key";
            listBoxCompanies.DataSource = companies;
            //            listBoxCompanies.SelectedIndexChanged += listBoxCompanies_SelectedIndexChanged;
            var defaultCompany = companies.FirstOrDefault(c => c.Default == Instance.JurisDefaultCompany.jdcJuris);
            if (companies.Count > 0)
            {
                listBoxCompanies.SelectedItem = defaultCompany ?? companies[0];
            }
        }

        #endregion

        #region MainForm events

        public string FileName;

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }


        private void btnFS_Click(object sender, EventArgs e)
        {
        


            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Matter Billing Field File";
            theDialog.Filter = "XLS files|*.xls*";
            theDialog.InitialDirectory = @"C:\Juris";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                
                string filePath = theDialog.FileName.ToString();
                FileName = filePath.ToString();
                System.Data.DataTable dtexcel = new System.Data.DataTable();
                String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                filePath.ToString() +
                                ";Extended Properties='Excel 8.0;HDR=YES;';";

                OleDbConnection con2 = new OleDbConnection(constr);
                OleDbCommand oconn2 = new OleDbCommand("Select * From [MBF$] where [Client]>'' and [Matter] > ''", con2);
                con2.Open();
        

                OleDbDataAdapter sda = new OleDbDataAdapter(oconn2);
                System.Data.DataTable data = new System.Data.DataTable();
                sda.Fill(data);
                grid_items.DataSource = data;
                this.grid_items.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                this.grid_items.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                foreach (DataGridViewColumn c in this.grid_items.Columns)
                    c.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;


                theDialog.Dispose();

            }

            string Client = "";
            string Matter = "";
            string NewText = "";
            string OldText = "";
            string JClient = "";
            string JMatter = "";
            string JSQL = "";
            string BF = cbBF.Text.ToString();
            string BFN = BF.Substring(0, 2);
     
            System.Data.DataTable dtGridSource = (System.Data.DataTable)grid_items.DataSource;

            foreach (DataRow dr in dtGridSource.Rows)
            {
                Client = dr["Client"].ToString();
                Matter = dr["Matter"].ToString();
                NewText = dr["BFText"].ToString();
                JSQL = @"select dbo.jfn_formatclientcode(clicode) as Client, dbo.jfn_FormatMatterCode(matcode) as Matter, case when " + BFN.ToString() + @"='01' then case when  matbillingfield01 is null then ' ' else ltrim(rtrim(cast(matbillingfield01 as varchar(8000)))) end
when " + BFN.ToString() + @" = '02' then case when  matbillingfield02 is null then ' ' else ltrim(rtrim(cast(matbillingfield02 as varchar(8000))))  end 
when " + BFN.ToString() + @" = '03' then case when  matbillingfield03 is null then ' ' else ltrim(rtrim(cast(matbillingfield03 as varchar(8000)))) end 
when " + BFN.ToString() + @" = '04' then case when  matbillingfield04 is null then ' ' else ltrim(rtrim(cast(matbillingfield04 as varchar(8000))))  end 
when " + BFN.ToString() + @" = '05' then case when  matbillingfield05 is null then ' ' else ltrim(rtrim(cast(matbillingfield05  as varchar(8000)))) end 
when " + BFN.ToString() + @" = '06' then case when  matbillingfield06 is null then ' ' else ltrim(rtrim(cast(matbillingfield06  as varchar(8000)))) end 
when " + BFN.ToString() + @" = '07' then case when  matbillingfield07 is null then ' ' else ltrim(rtrim(cast(matbillingfield07  as varchar(8000)))) end 
when " + BFN.ToString() + @" = '08' then case when  matbillingfield08 is null then ' ' else ltrim(rtrim(cast(matbillingfield08  as varchar(8000)))) end 
when " + BFN.ToString() + @" = '09' then case when  matbillingfield09 is null then ' ' else ltrim(rtrim(cast(matbillingfield09  as varchar(8000)))) end 
when " + BFN.ToString() + @" = '11' then case when  matbillingfield11 is null then ' ' else ltrim(rtrim(cast(matbillingfield11  as varchar(8000)))) end  
when " + BFN.ToString() + @" = '10' then case when  matbillingfield10 is null then ' ' else ltrim(rtrim(cast(matbillingfield10  as varchar(8000)))) end 
when " + BFN.ToString() + @" = '12' then case when  matbillingfield12 is null then ' ' else ltrim(rtrim(cast(matbillingfield12  as varchar(8000)))) end 
when " + BFN.ToString() + @" = '13' then case when  matbillingfield13 is null then ' ' else ltrim(rtrim(cast(matbillingfield13  as varchar(8000)))) end 
when " + BFN.ToString() + @" = '14' then case when  matbillingfield14 is null then ' ' else ltrim(rtrim(cast(matbillingfield14  as varchar(8000)))) end 
when " + BFN.ToString() + @" = '15' then case when  matbillingfield15 is null then ' ' else ltrim(rtrim(cast(matbillingfield15  as varchar(8000)))) end 
when " + BFN.ToString() + @" = '16' then case when  matbillingfield16 is null then ' ' else ltrim(rtrim(cast(matbillingfield16  as varchar(8000)))) end 
when " + BFN.ToString() + @" = '17' then case when  matbillingfield17 is null then '  ' else ltrim(rtrim(cast(matbillingfield17  as varchar(8000)))) end 
when " + BFN.ToString() + @" = '18' then case when  matbillingfield18 is null then '' else ltrim(rtrim(cast(matbillingfield18  as varchar(8000)))) end 
when " + BFN.ToString() + @" = '19' then case when  matbillingfield19 is null then ' ' else ltrim(rtrim(cast(matbillingfield19  as varchar(8000)))) end 
when " + BFN.ToString() + @" = '20' then case when  matbillingfield20 is null then ' ' else ltrim(rtrim(cast(matbillingfield20  as varchar(8000)))) end end as BField 
from matter inner join client on clisysnbr = matclinbr where dbo.jfn_formatclientcode(clicode) = '" + Client.ToString() + "' and dbo.jfn_FormatMatterCode(matcode) = '" + Matter.ToString() + "'";

                DataSet CliData = _jurisUtility.RecordsetFromSQL(JSQL);

                if (CliData.Tables[0].Rows.Count == 0)
                {
                    dr["CCValidation"] = "No Valid Client or Matter";
                    dr["Updateable?"] = "N";
                }
                else
                {
                    foreach (System.Data.DataTable table in CliData.Tables)
                    {

                        foreach (DataRow dt2 in table.Rows)
                        {
                            JClient = dt2["Client"].ToString();
                            JMatter = dt2["Matter"].ToString();
                            OldText = dt2["BField"].ToString();
                            dr["CCValidation"] = JClient.ToString();
                            dr["Existing BF Data"] = OldText.ToString();                    
                        
                        

                            if (rb1.Checked == true && OldText.Length > 1)
                            {
                                dr["CCValidation"] = "Field already populated";
                                dr["Updateable?"] = "N";

                            }
                        }

                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }
           




                private void listBoxCompanies_SelectedIndexChanged(object sender, EventArgs e)
        {



            if (_jurisUtility.DbOpen)
            {
                _jurisUtility.CloseDatabase();
            }
            CompanyCode = "Company" + listBoxCompanies.SelectedValue;
            _jurisUtility.SetInstance(CompanyCode);
            JurisDbName = _jurisUtility.Company.DatabaseName;
            JBillsDbName = "JBills" + _jurisUtility.Company.Code;
            _jurisUtility.OpenDatabase();
            if (_jurisUtility.DbOpen)
            {
                string CBF;
                cbBF.ClearItems();
                string SQLCli = "select right(spname,2) + ' - '  + left(sptxtvalue, charindex(',', sptxtvalue) - 1) as BF from sysparam where spname like 'FldMatterBF%'";
                DataSet myRSCli = _jurisUtility.RecordsetFromSQL(SQLCli);

                if (myRSCli.Tables[0].Rows.Count == 0)
                    cbBF.SelectedIndex = 0;
                else
                {
                    foreach (System.Data.DataTable table in myRSCli.Tables)
                    {

                        foreach (DataRow dr in table.Rows)
                        {
                            CBF = dr["BF"].ToString();
                            cbBF.Items.Add(CBF);
                        }
                    }

                }
            }




        }        

    

        private void DoDaFix()
        {
            
            string BN = cbBF.Text.ToString();
            string BNbr = BN.Substring(0, 2);
            string CCode = "";
            string MCode = "";
            string BFText = "";
            string VField = "";
            string OText = "";
            string Opt = "";
            string SQLCBF = "";
            string BField = "matBillingField" + BNbr.ToString();
            if (rb1.Checked == true)
            { Opt = "Text will be inserted into the billing field if billing field is blank."; }
            if (rb2.Checked == true)
            { Opt = "Text will be added to the beginning of the billing field."; }
            if (rb3.Checked == true)
            { Opt = "Text will be added to the end of the billing field."; }
            if (rb4.Checked == true)
            { Opt = "Text will replace the existing billing field value."; }
            DialogResult dialogResult = MessageBox.Show("Matter Billing Field " + BNbr.ToString() + " will be updated from spreadsheet " + FileName.ToString() + " using the following option: " + Opt.ToString() + ".  Do you wish to continue?", "Billing Field Update", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                System.Data.DataTable dtGrid = (System.Data.DataTable)grid_items.DataSource;

                foreach (DataRow dr in dtGrid.Rows)
                {
                    CCode = dr["Client"].ToString();
                    MCode = dr["Matter"].ToString();
                    BFText = dr["BFText"].ToString();
                    BFText = BFText.Replace("'", "''");
                    VField = dr["Updateable?"].ToString();
                    OText = dr["Existing BF Data"].ToString();
                    OText = OText.Replace("'", "''");

                    if (getCliSysNbr(CCode) == 0 || getMatSysNbr(CCode, MCode) == 0)
                    {
                        dr["CCValidation"] = "Client/Matter Number does not exist";
                        dr["Updateable?"] = "N";

                    }
                    else
                    {
                        string NewBF = "";
                        string[] lines = BFText.ToString().Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        for (int i = 0; i < lines.Count(); i++)
                        {
                            if (i < lines.Count() - 1)
                                NewBF = NewBF + lines[i].ToString() + "||";
                            else
                                NewBF = NewBF + lines[i].ToString();

                        }

                        Cursor.Current = Cursors.WaitCursor;

                        toolStripStatusLabel.Text = "Updating " + BField.ToString() + " for Client/Matter " + CCode.ToString() + "/" + MCode.ToString() + "....";
                        statusStrip.Refresh();

                        if (rb1.Checked == true)
                        {
                            SQLCBF = "update m set m." + BField.ToString() + "=replace('" + NewBF.ToString() + @"','||', char(13) + char(10)) from matter m inner join client c on c.clisysnbr = m.matclinbr where '" + VField.ToString() + "'<>'N' and dbo.jfn_formatclientcode(clicode) = '" + CCode.ToString() + "' and dbo.jfn_FormatMatterCode(matcode) = '" + MCode.ToString() + "'";
                            _jurisUtility.ExecuteNonQueryCommand(0, SQLCBF);
                        }
                        if (rb2.Checked == true)
                        {
                            SQLCBF = "update m set m." + BField.ToString() + "=replace('" + NewBF.ToString() + @"','||', char(13) + char(10)) + '  '  + '" + OText.ToString() + @"' from matter m inner join client c on c.clisysnbr = m.matclinbr where dbo.jfn_formatclientcode(clicode) = '" + CCode.ToString() + "' and dbo.jfn_FormatMatterCode(matcode) = '" + MCode.ToString() + "'";
                            _jurisUtility.ExecuteNonQueryCommand(0, SQLCBF);
                        }
                        if (rb3.Checked == true)
                        {
                            SQLCBF = "update m set m." + BField.ToString() + "='" + OText.ToString() + "' + ' ' + replace('" + NewBF.ToString() + @"','||', char(13) + char(10)) from matter m inner join client c on c.clisysnbr = m.matclinbr  where dbo.jfn_formatclientcode(clicode) = '" + CCode.ToString() + "' and dbo.jfn_FormatMatterCode(matcode) = '" + MCode.ToString() + "'";
                            _jurisUtility.ExecuteNonQueryCommand(0, SQLCBF);
                        }
                        if (rb4.Checked == true)
                        {
                            SQLCBF = "update m set m." + BField.ToString() + "=replace('" + NewBF.ToString() + @"','||', char(13) + char(10)) from matter m inner join client c on c.clisysnbr = m.matclinbr where dbo.jfn_formatclientcode(clicode) = '" + CCode.ToString() + "' and dbo.jfn_FormatMatterCode(matcode) = '" + MCode.ToString() + "'";
                            _jurisUtility.ExecuteNonQueryCommand(0, SQLCBF);
                        }
                    }
                }

                string rowFilter = string.Format("[{0}] = '{1}'", "Updateable?", "N");
                (grid_items.DataSource as System.Data.DataTable).DefaultView.RowFilter = rowFilter;
                Cursor.Current = Cursors.Default;

                toolStripStatusLabel.Text = "Update Complete.";
                statusStrip.Refresh();

                MessageBox.Show("Matter Billing Fields Updated.  View exceptions in grid on screen", "Update Complete!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            else if (dialogResult == DialogResult.No)
            {
                Cursor.Current = Cursors.Default;
                toolStripStatusLabel.Text = "Execution Cancelled.";
                statusStrip.Refresh();
            }

        }


        private int getCliSysNbr(string clicode)
        {
            int clisysnbr = 0;
            string sql = "select clisysnbr from client where dbo.jfn_FormatClientCode(clicode) = '" + clicode + "'";
            DataSet dds = _jurisUtility.RecordsetFromSQL(sql);
            if (dds != null && dds.Tables.Count > 0)
            {
                foreach (DataRow dr in dds.Tables[0].Rows)
                {
                    clisysnbr = Convert.ToInt32(dr[0].ToString());
                }
            }
            return clisysnbr;
        }

        private int getMatSysNbr(string clicode, string matcode)
        {

            int matsysnbr = 0;
            int clisysnbr = getCliSysNbr(clicode);
            string sql = "select matsysnbr from matter where matclinbr = " + clisysnbr.ToString() + " and dbo.jfn_FormatMatterCode(matcode) = '" + matcode + "'";
            DataSet dds = _jurisUtility.RecordsetFromSQL(sql);
            if (dds != null && dds.Tables.Count > 0)
            {
                foreach (DataRow dr in dds.Tables[0].Rows)
                {
                    matsysnbr = Convert.ToInt32(dr[0].ToString());
                    break;
                }

            }
            return matsysnbr;
        }

        private bool VerifyFirmName()
        {
            //    Dim SQL     As String
            //    Dim rsDB    As ADODB.Recordset
            //
            //    SQL = "SELECT CASE WHEN SpTxtValue LIKE '%firm name%' THEN 'Y' ELSE 'N' END AS Firm FROM SysParam WHERE SpName = 'FirmName'"
            //    Cmd.CommandText = SQL
            //    Set rsDB = Cmd.Execute
            //
            //    If rsDB!Firm = "Y" Then
            return true;
            //    Else
            //        VerifyFirmName = False
            //    End If

        }

   



        private static bool IsDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private static bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        private void WriteLog(string comment)
        {
            var sql =
                string.Format("Insert Into UtilityLog(ULTimeStamp,ULWkStaUser,ULComment) Values('{0}','{1}', '{2}')",
                    DateTime.Now, GetComputerAndUser(), comment);
            _jurisUtility.ExecuteNonQueryCommand(0, sql);
        }

        private string GetComputerAndUser()
        {
            var computerName = Environment.MachineName;
            var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var userName = (windowsIdentity != null) ? windowsIdentity.Name : "Unknown";
            return computerName + "/" + userName;
        }

        /// <summary>
        /// Update status bar (text to display and step number of total completed)
        /// </summary>
        /// <param name="status">status text to display</param>
        /// <param name="step">steps completed</param>
        /// <param name="steps">total steps to be done</param>
        private void UpdateStatus(string status, long step, long steps)
        {



        }
        private void DeleteLog()
        {
            string AppDir = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            string filePathName = Path.Combine(AppDir, "VoucherImportLog.txt");
            if (File.Exists(filePathName + ".ark5"))
            {
                File.Delete(filePathName + ".ark5");
            }
            if (File.Exists(filePathName + ".ark4"))
            {
                File.Copy(filePathName + ".ark4", filePathName + ".ark5");
                File.Delete(filePathName + ".ark4");
            }
            if (File.Exists(filePathName + ".ark3"))
            {
                File.Copy(filePathName + ".ark3", filePathName + ".ark4");
                File.Delete(filePathName + ".ark3");
            }
            if (File.Exists(filePathName + ".ark2"))
            {
                File.Copy(filePathName + ".ark2", filePathName + ".ark3");
                File.Delete(filePathName + ".ark2");
            }
            if (File.Exists(filePathName + ".ark1"))
            {
                File.Copy(filePathName + ".ark1", filePathName + ".ark2");
                File.Delete(filePathName + ".ark1");
            }
            if (File.Exists(filePathName))
            {
                File.Copy(filePathName, filePathName + ".ark1");
                File.Delete(filePathName);
            }

        }



        private void LogFile(string LogLine)
        {
            string AppDir = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            string filePathName = Path.Combine(AppDir, "VoucherImportLog.txt");
            using (StreamWriter sw = File.AppendText(filePathName))
            {
                sw.WriteLine(LogLine);
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            DoDaFix();
        }

        private void rb4_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
        }


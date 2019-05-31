using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;

namespace TransportLogistics
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }
        private bool _isReportViewerLoaded;

        private void Report_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\TransportLogistics\Database1.mdf;Integrated Security=True";
            DataSet dataset = new DataSet();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Cargo", connection);
            adapter.Fill(dataset);
            if (!_isReportViewerLoaded)
            {
                ReportDataSource reportDataSource1 = new ReportDataSource();

                reportDataSource1.Name = "Cargo"; //Name of the report dataset in our .RDLC file
                reportDataSource1.Value = dataset.Tables[0];
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "TransportLogistics.ListofCargo.rdlc";

                reportViewer1.RefreshReport();

                _isReportViewerLoaded = true;
            }
        }
    }
}

namespace TransportLogistics
{
    partial class Report
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.Database1DataSet2 = new TransportLogistics.Database1DataSet2();
            this.CargoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.CargoTableAdapter = new TransportLogistics.Database1DataSet2TableAdapters.CargoTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.Database1DataSet2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CargoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            reportDataSource1.Name = "Cargo";
            reportDataSource1.Value = this.CargoBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "TransportLogistics.ListofCargo.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(12, 12);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(673, 771);
            this.reportViewer1.TabIndex = 0;
            // 
            // Database1DataSet2
            // 
            this.Database1DataSet2.DataSetName = "Database1DataSet2";
            this.Database1DataSet2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // CargoBindingSource
            // 
            this.CargoBindingSource.DataMember = "Cargo";
            this.CargoBindingSource.DataSource = this.Database1DataSet2;
            // 
            // CargoTableAdapter
            // 
            this.CargoTableAdapter.ClearBeforeFill = true;
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 795);
            this.Controls.Add(this.reportViewer1);
            this.Name = "Report";
            this.Text = "Report";
            this.Load += new System.EventHandler(this.Report_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Database1DataSet2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CargoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource CargoBindingSource;
        private Database1DataSet2 Database1DataSet2;
        private Database1DataSet2TableAdapters.CargoTableAdapter CargoTableAdapter;
    }
}
using Microsoft.Reporting.WinForms;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransportLogistics
{
    public partial class Main : Form
    {
        DataSet ds = new DataSet();
        DataSet reportD = new DataSet();
        bool a = true;
        SqlDataAdapter adapter;
        SqlDataAdapter adapterDGV;
        SqlConnection connection;
        DataTable dt;
        BindingSource bindingSource1 = new BindingSource();
        DataTable table;
        string truck;
        DirectoryInfo info = new DirectoryInfo(".");
        string connectionString;
        //string connectionString = @"Data Source =.\SQLEXPRESS;Database=myuniquedb;Initial Catalog=Database1.mdf;Integrated Security = True; User Instance = True";
        string tableName = "Truck"; string colomnName = "carrying as 'Грузоподъемность'";

        public Main()
        {
            connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + info.FullName.Substring(0, info.FullName.Length - 10) + "\\Database1.mdf;Integrated Security=true";
            InitializeComponent();
            loadComboBox();
        }

        private void loadComboBox()
        {
            DataSet datas = new DataSet();
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter("SELECT * FROM Cargo; SELECT * FROM Truck", connection);
            adapter.Fill(datas);
            cargoBox.DataSource = datas.Tables[0];
            cargoBox.DisplayMember = "name";
            cargoBox.ValueMember = "Id";
            truckBox.DataSource = datas.Tables[1];
            truckBox.DisplayMember = "name";
            truckBox.ValueMember = "Id";
        }

        private void GetData(string selectCommand)
        {
            try
            {
                adapterDGV = new SqlDataAdapter(selectCommand, connectionString);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapterDGV);
                table = new DataTable
                {
                    Locale = CultureInfo.InvariantCulture
                };
                adapterDGV.Fill(table);
                bindingSource1.DataSource = table;
                

                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.AllowUserToAddRows = false;
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки данных!");
            }
        }

        private void loadData_Click(object sender, EventArgs e)
        {
            if (tableBox.Text == "Товары") { tableName = "Cargo"; colomnName = "weight as 'Вес'"; }
            else { tableName = "Truck"; colomnName = "carrying as 'Грузоподъемность'"; }
            GetData("select Id, name as 'Название', width as 'Ширина', height as 'Высота', length as 'Длинна', " + colomnName + " from " + tableName);
            dataGridView1.DataSource = bindingSource1;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.AutoResizeColumn(1);
        }

        private void addData_Click(object sender, EventArgs e)
        {
            if(dataGridView1.DataSource != null) table.Rows.Add();
            else MessageBox.Show("Выберите и загрузить таблицу");
        }

        private void deleteData_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "Удалить выбранную строку?",
            "Удаление",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button1,
            MessageBoxOptions.DefaultDesktopOnly);

            if (result == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.Remove(row);
                }
                this.TopMost = true;
            }
        }

        public void saveData_Click(object sender, EventArgs e)
        {
            try
            {
                adapterDGV.Update((DataTable)bindingSource1.DataSource);
                GetData(adapterDGV.SelectCommand.CommandText);
                loadComboBox();
            }
            catch
            {
                MessageBox.Show("Ошибка при сохранении данных");
            }
        }

        private void chooseTruck_Click(object sender, EventArgs e)
        {
            ds.Clear();
            adapter = new SqlDataAdapter("SELECT * FROM Truck where Id =" + truckBox.SelectedValue, connection);
            adapter.Fill(ds);
            if (radioTruckBD.Checked)
            {
                selectedTruck.Text = "Параметры выбранного транспорта:" + "\nШирина: " + ds.Tables[0].Rows[0].ItemArray[2] + "\nВысота: " + ds.Tables[0].Rows[0].ItemArray[3]
                    + "\nДлинна: " + ds.Tables[0].Rows[0].ItemArray[4] + "\nГрузоподъемность: " + ds.Tables[0].Rows[0].ItemArray[5];
                truck = truckBox.Text + "(" + ds.Tables[0].Rows[0].ItemArray[2] + "," + ds.Tables[0].Rows[0].ItemArray[3] + "," + ds.Tables[0].Rows[0].ItemArray[4] + "," + ds.Tables[0].Rows[0].ItemArray[5]+")";
            }
            else if(radioTruck.Checked)
            {
                selectedTruck.Text = "Параметры выбранного транспорта:" + "\nШирина: " + numericUpDown1.Value + "\nВысота: " + numericUpDown2.Value + "\nДлинна: " + numericUpDown3.Value + "\nГрузоподъемность: " + numericUpDown4.Value;
                truck = "" + numericUpDown1.Value + "," + numericUpDown2.Value + "," + numericUpDown3.Value + "," + numericUpDown4.Value;
            }
            else MessageBox.Show("Выберите вариант задания параметров транспортного средства");
        }

        private void chooseCargo_Click(object sender, EventArgs e)
        {
            ds.Clear();
            if (a)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("name", typeof(string)));
                dt.Columns.Add(new DataColumn("width", typeof(int)));
                dt.Columns.Add(new DataColumn("length", typeof(int)));
            }
            
            DataRow dr = dt.NewRow();
            if (numericUpDown10.Value <= 0) MessageBox.Show("Выберите количесвто товаров для погрузки");
            else
            {
                adapter = new SqlDataAdapter("SELECT * FROM Cargo where Id =" + cargoBox.SelectedValue, connection);
                adapter.Fill(ds);
                if (radioCargoBD.Checked)
                {
                    listBox1.Items.Add(cargoBox.Text + " " + numericUpDown10.Value + " шт.");
                    dr[0] = cargoBox.Text; dr[1] = numericUpDown10.Value; dr[2] = numericUpDown9.Value;
                    dt.Rows.Add(dr);
                }
                else if (radioCargo.Checked)
                {
                    if (String.IsNullOrWhiteSpace(textBox1.Text)) MessageBox.Show("Введите название для товара");
                    else
                    {
                        listBox1.Items.Add(textBox1.Text + " " + numericUpDown10.Value + " шт.");
                        dr[0] = textBox1.Text; dr[1] = numericUpDown10.Value; dr[2] = numericUpDown9.Value;
                        dt.Rows.Add(dr);
                    }
                }
                else MessageBox.Show("Выберите вариант задания параметров грузов");
                
            }
            a = false;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("В " + (e.ColumnIndex+1).ToString() + " столбце введены неправильные данные");
        }

        private void deleteCargo_Click(object sender, EventArgs e)
        {
            try { listBox1.Items.RemoveAt(listBox1.SelectedIndex); }
            catch { MessageBox.Show("Выберите груз из списка"); }
            
        }

        private void madeScheme_Click(object sender, EventArgs e)
        {
            Scheme f1 = new Scheme();
            f1.Show();
            this.Hide();
        }

        private void createReport_Click(object sender, EventArgs e)
        {
            reportD.Clear();
            reportD.Tables.Add(dt);
            Report form = new Report();

            LocalReport localReport = form.reportViewer1.LocalReport;
            form.reportViewer1.LocalReport.ReportEmbeddedResource = "TransportLogistics.ListofCargo.rdlc";
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            reportParameters.Add(new ReportParameter("truck", truck));


            form.reportViewer1.ProcessingMode = ProcessingMode.Local;
            form.reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource reportCargo = new ReportDataSource("Cargo", reportD.Tables[0]);
            form.reportViewer1.LocalReport.DataSources.Add(reportCargo);
            //reportCargo.Value = (ds.Tables[0]);
            //localReport.DataSources.Add(new ReportDataSource("Cargo", reportCargo.Value));

            localReport.SetParameters(reportParameters);
            form.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            form.reportViewer1.RefreshReport();
            form.Show();
        }
    }
}

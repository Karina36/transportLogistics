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
        DataTable dt = new DataTable();
        DataTable cargoData = new DataTable();
        BindingSource bindingSource1 = new BindingSource();
        DataTable table;
        string[] truck = new string[6];
        DirectoryInfo info = new DirectoryInfo(".");
        string connectionString;
        string tableName = "Truck"; string colomnName = "carrying as 'Грузоподъемность'";

        public Main()
        {
            connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + info.FullName.Substring(0, info.FullName.Length - 10) + "\\Database1.mdf;Integrated Security=true";
            InitializeComponent();
            loadComboBox();
            cargoData.Columns.Add(new DataColumn("name", typeof(string)));
            cargoData.Columns.Add(new DataColumn("sum", typeof(int)));
            cargoData.Columns.Add(new DataColumn("prior", typeof(int)));
            cargoData.Columns.Add(new DataColumn("height", typeof(int)));
            cargoData.Columns.Add(new DataColumn("width", typeof(int)));
            cargoData.Columns.Add(new DataColumn("length", typeof(int)));
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
            if (dataGridView1.DataSource != null) table.Rows.Add();
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
                truck[0] = truckBox.Text; truck[1] = ds.Tables[0].Rows[0].ItemArray[2].ToString(); truck[2] = ds.Tables[0].Rows[0].ItemArray[3].ToString(); truck[3] = ds.Tables[0].Rows[0].ItemArray[4].ToString(); truck[4] = ds.Tables[0].Rows[0].ItemArray[5].ToString();
            }
            else if (radioTruck.Checked)
            {
                selectedTruck.Text = "Параметры выбранного транспорта:" + "\nШирина: " + numericUpDown1.Value + "\nВысота: " + numericUpDown2.Value + "\nДлинна: " + numericUpDown3.Value + "\nГрузоподъемность: " + numericUpDown4.Value;
                truck[0] = "Транспорт"; truck[1] = numericUpDown1.Value.ToString(); truck[2] = numericUpDown2.Value.ToString(); truck[3] = numericUpDown3.Value.ToString(); truck[4] = numericUpDown4.Value.ToString();
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
            DataRow cargoRow = cargoData.NewRow();
            if (numericUpDown10.Value <= 0) MessageBox.Show("Выберите количесвто товаров для погрузки");
            else
            {
                adapter = new SqlDataAdapter("SELECT * FROM Cargo where Id =" + cargoBox.SelectedValue, connection);
                adapter.Fill(ds);
                if (radioCargoBD.Checked)
                {
                    if (listBox1.Items.Contains(cargoBox.Text)) MessageBox.Show("Груз уже добавлен");
                    else
                    {
                        listBox1.Items.Add(cargoBox.Text);
                        dr[0] = cargoBox.Text; dr[1] = numericUpDown10.Value; dr[2] = numericUpDown9.Value;
                        cargoRow[0] = cargoBox.Text; cargoRow[1] = numericUpDown10.Value; cargoRow[2] = numericUpDown9.Value; cargoRow[3] = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                        cargoRow[4] = ds.Tables[0].Rows[0].ItemArray[2].ToString(); cargoRow[5] = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                        cargoData.Rows.Add(cargoRow);
                        dt.Rows.Add(dr);
                    }
                }
                else if (radioCargo.Checked)
                {
                    if (String.IsNullOrWhiteSpace(textBox1.Text)) MessageBox.Show("Введите название для товара");
                    else
                    {
                        if (listBox1.Items.Contains(textBox1.Text)) MessageBox.Show("Груз уже добавлен");
                        else
                        {
                            listBox1.Items.Add(textBox1.Text);
                            dr[0] = textBox1.Text; dr[1] = numericUpDown10.Value; dr[2] = numericUpDown9.Value;
                            cargoRow[0] = cargoBox.Text; cargoRow[1] = numericUpDown10.Value; cargoRow[2] = numericUpDown9.Value; cargoRow[3] = numericUpDown7.Value;
                            cargoRow[4] = numericUpDown8.Value; cargoRow[5] = numericUpDown5.Value;
                            cargoData.Rows.Add(cargoRow);
                            dt.Rows.Add(dr);
                        }
                    }
                }
                else MessageBox.Show("Выберите вариант задания параметров грузов");

            }
            a = false;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("В " + (e.ColumnIndex + 1).ToString() + " столбце введены неправильные данные");
        }

        private void deleteCargo_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dt.Rows[i];
                    if (dr["name"].Equals(listBox1.SelectedItem))
                        dr.Delete();
                }
                dt.AcceptChanges();
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
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
            calc();
            reportD.Tables.Clear();
            try
            {
                reportD.Tables.Add(dt);
                Report form = new Report();

                LocalReport localReport = form.reportViewer1.LocalReport;
                form.reportViewer1.LocalReport.ReportEmbeddedResource = "TransportLogistics.ListofCargo.rdlc";
                List<ReportParameter> reportParameters = new List<ReportParameter>();
                reportParameters.Add(new ReportParameter("truck", truck[0] + "(" + truck[1] + "," + truck[2] + "," + truck[3] + "," + truck[4] + ")"));


                form.reportViewer1.ProcessingMode = ProcessingMode.Local;
                form.reportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource reportCargo = new ReportDataSource("Cargo", reportD.Tables[0]);
                form.reportViewer1.LocalReport.DataSources.Add(reportCargo);

                localReport.SetParameters(reportParameters);
                form.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                form.reportViewer1.RefreshReport();
                form.Show();
            }
            catch { MessageBox.Show("Ошибка при выборе грузов"); }


        }
        private void calc()
        {
            int truckW = Convert.ToInt32(truck[1]); int truckH = Convert.ToInt32(truck[2]); int truckL = Convert.ToInt32(truck[3]);
            //Первая партия
            int width = Convert.ToInt32(truck[1]) / Convert.ToInt32(cargoData.Rows[0][4]); int length = Convert.ToInt32(truck[3]) / Convert.ToInt32(cargoData.Rows[0][5]);
            int height = Convert.ToInt32(truck[2]) / Convert.ToInt32(cargoData.Rows[0][3]);
            int[] wlh1 = new int[4] { width, length, height, width*length*height };

            width = Convert.ToInt32(truck[1]) / Convert.ToInt32(cargoData.Rows[0][3]); height = Convert.ToInt32(truck[2]) / Convert.ToInt32(cargoData.Rows[0][4]);
            int[] hlw1 = new int[4] { width, length, height, width * length * height };

            width = Convert.ToInt32(truck[1]) / Convert.ToInt32(cargoData.Rows[0][4]); length = Convert.ToInt32(truck[3]) / Convert.ToInt32(cargoData.Rows[0][3]);
            height = Convert.ToInt32(truck[2]) / Convert.ToInt32(cargoData.Rows[0][5]);
            int[] whl1 = new int[4] { width, length, height, width * length * height };

            width = Convert.ToInt32(truck[1]) / Convert.ToInt32(cargoData.Rows[0][5]); length = Convert.ToInt32(truck[3]) / Convert.ToInt32(cargoData.Rows[0][4]);
            height = Convert.ToInt32(truck[2]) / Convert.ToInt32(cargoData.Rows[0][3]);
            int[] lwh1 = new int[4] { width, length, height, width * length * height };

            length = Convert.ToInt32(truck[3]) / Convert.ToInt32(cargoData.Rows[0][3]); height = Convert.ToInt32(truck[2]) / Convert.ToInt32(cargoData.Rows[0][4]);
            int[] lhw1 = new int[4] { width, length, height, width * length * height };

            width = Convert.ToInt32(truck[1]) / Convert.ToInt32(cargoData.Rows[0][3]); length = Convert.ToInt32(truck[3]) / Convert.ToInt32(cargoData.Rows[0][4]);
            height = Convert.ToInt32(truck[2]) / Convert.ToInt32(cargoData.Rows[0][5]);
            int[] hwl1 = new int[4] { width, length, height, width * length * height };

            int[] max1 = new int[6] { wlh1[3], hlw1[3], whl1[3],  lwh1[3], lhw1[3], hwl1[3]};

            if (Convert.ToInt32(dt.Rows[0].ItemArray[1]) > max1.Max()) dt.Rows[0].SetField(1, max1.Max());

            //Свободное пространство
            int[] widthF = new int[6] { truckW - wlh1[0], truckW - hlw1[0], truckW - whl1[0], truckW - lwh1[0], truckW - lhw1[0], truckW - hwl1[0] };
            int[] lengthF = new int[6] { truckL - wlh1[1], truckL - hlw1[1], truckL - whl1[1], truckL - lwh1[1], truckL - lhw1[1], truckL - hwl1[1] };
            int[] heightF = new int[6] { truckH - wlh1[2], truckH - hlw1[2], truckH - whl1[2], truckH - lwh1[2], truckH - lhw1[2], truckH - hwl1[2] };

            //Свободный объем
            int[] widthV = new int[6] { widthF[0] * truckL* truckH, widthF[1] * truckL * truckH, widthF[2] * truckL * truckH, widthF[3] * truckL * truckH, widthF[4] * truckL * truckH, widthF[5] * truckL * truckH };
            int[] lengthV = new int[6] { lengthF[0] * (truckL - heightF[0]) * (truckW- widthF[0]), lengthF[1] * (truckL - heightF[1]) * (truckW - widthF[1]), lengthF[2] * (truckL - heightF[2]) * (truckW - widthF[2]),
                 lengthF[3] * (truckL - heightF[3]) * (truckW- widthF[3]),  lengthF[4] * (truckL - heightF[4]) * (truckW- widthF[4]),  lengthF[5] * (truckL - heightF[5]) * (truckW- widthF[5])};
            int[] heightV = new int[6] { lengthF[0]* (truckW - widthF[0]) * truckH, lengthF[1] * (truckW - widthF[1]) * truckH, lengthF[2] * (truckW - widthF[2]) * truckH,
                lengthF[3]* (truckW - widthF[3]) * truckH, lengthF[4]* (truckW - widthF[4]) * truckH, lengthF[5]* (truckW - widthF[5]) * truckH};
            int[] sumV = new int[6] { widthV[0] + lengthV[0] + heightV[0], widthV[1] + lengthV[1] + heightV[1], widthV[2] + lengthV[2] + heightV[2], widthV[3] + lengthV[3] + heightV[3], widthV[4] + lengthV[4] + heightV[4],
            widthV[5] + lengthV[5] + heightV[5]};

        }
    }
}

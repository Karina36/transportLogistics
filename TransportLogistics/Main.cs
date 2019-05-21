using System;
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
        SqlDataAdapter adapter;
        SqlDataAdapter adapterDGV;
        SqlConnection connection;
        private BindingSource bindingSource1 = new BindingSource();
        DataTable table;
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";
        string tableName = "Truck";

        public Main()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter("SELECT * FROM Cargo; SELECT * FROM Truck", connection);
            adapter.Fill(ds);
            cargoBox.DataSource = ds.Tables[0];
            cargoBox.DisplayMember = "name";
            cargoBox.ValueMember = "Id";
            truckBox.DataSource = ds.Tables[1];
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
            catch (SqlException)
            {
                MessageBox.Show("To run this example, replace the value of the " +
                    "connectionString variable with a connection string that is " +
                    "valid for your system.");
            }
        }

        private void loadData_Click(object sender, EventArgs e)
        {
            if (tableBox.Text == "Товары") tableName = "Cargo"; else tableName = "Truck";
            dataGridView1.DataSource = bindingSource1;
            GetData("select * from " + tableName);
            dataGridView1.Columns[0].ReadOnly = true;
        }

        private void addData_Click(object sender, EventArgs e)
        {
            table.Rows.Add();
        }

        private void deleteData_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.Remove(row);
            }
        }

        private void saveData_Click(object sender, EventArgs e)
        {
            try
            {
                adapterDGV.Update((DataTable)bindingSource1.DataSource);
            }
            catch
            {
                MessageBox.Show("Ошибка при сохранении данных");
            }
            GetData(adapterDGV.SelectCommand.CommandText);
        }

        private void chooseTruck_Click(object sender, EventArgs e)
        {
            ds = new DataSet();
            adapter = new SqlDataAdapter("SELECT * FROM Truck where Id =" + truckBox.SelectedValue, connection);
            adapter.Fill(ds);
            if (radioTruckBD.Checked)
            {
                selectedTruck.Text = "Параметры выбранного транспорта:" + "\nШирина: " + ds.Tables[0].Rows[0].ItemArray[2] + "\nВысота: " + ds.Tables[0].Rows[0].ItemArray[3] 
                    + "\nДлинна: " + ds.Tables[0].Rows[0].ItemArray[4] + "\nГрузоподъемность: " + ds.Tables[0].Rows[0].ItemArray[5];
            }
            else selectedTruck.Text = "Параметры выбранного транспорта:" + "\nШирина: " + numericUpDown1.Value + "\nВысота: " + numericUpDown2.Value + "\nДлинна: " + numericUpDown3.Value + "\nГрузоподъемность: " + numericUpDown4.Value;
        }

        private void chooseCargo_Click(object sender, EventArgs e)
        {
            ds = new DataSet();
            adapter = new SqlDataAdapter("SELECT * FROM Truck where Id =" + truckBox.SelectedValue, connection);
            adapter.Fill(ds);
            if (radioCargoBD.Checked)
            {
                listBox1.Items.Add(cargoBox.Text + " " + numericUpDown10.Value + " шт.");

            }
            else
            {
                if (String.IsNullOrWhiteSpace(textBox1.Text)) MessageBox.Show("Введите название для товара"); 
                else listBox1.Items.Add(textBox1.Text + " " + numericUpDown10.Value + " шт.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }
    }
}

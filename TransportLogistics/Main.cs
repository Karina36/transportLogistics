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
        private BindingSource bindingSource1 = new BindingSource();
        DataTable table;
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";
        string tableName = "Truck";

        public Main()
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter("SELECT * FROM Cargo", connection);
            adapter.Fill(ds);
            cargoBox.DataSource = ds.Tables[0];
            cargoBox.DisplayMember = "name";
            cargoBox.ValueMember = "Id";
            adapter = new SqlDataAdapter("SELECT * FROM Truck", connection);
            ds = new DataSet();
            adapter.Fill(ds);
            truckBox.DataSource = ds.Tables[0];
            truckBox.DisplayMember = "name";
            truckBox.ValueMember = "Id";
        }

        private void GetData(string selectCommand)
        {
            try
            {
                adapter = new SqlDataAdapter(selectCommand, connectionString);
                //SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                table = new DataTable
                {
                    Locale = CultureInfo.InvariantCulture
                };
                adapter.Fill(table);
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
            adapter.Update((DataTable)bindingSource1.DataSource);
            GetData(adapter.SelectCommand.CommandText);
            //try
            //{
            //    OracleCommandBuilder builder = new OracleCommandBuilder(oraAdap);
            //    oraAdap.Update(journal);
            //    MessageBox.Show("Данные сохранены");
            //}
            //catch
            //{
            //    MessageBox.Show("Ошибка при вводе данных");
            //}
        }

        
    }
}

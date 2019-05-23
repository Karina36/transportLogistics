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
        BindingSource bindingSource1 = new BindingSource();
        DataTable table;
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=F:\Моя_папка\агу\ВКР\прога\TransportLogistics\TransportLogistics\Database1.mdf;Integrated Security=True";
        string tableName = "Truck";

        public Main()
        {
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
                MessageBox.Show("");
            }
        }

        private void loadData_Click(object sender, EventArgs e)
        {
            if (tableBox.Text == "Товары") tableName = "Cargo"; else tableName = "Truck";
            GetData("select * from " + tableName);
            dataGridView1.DataSource = bindingSource1;
            dataGridView1.Columns[0].ReadOnly = true;
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
            string truck;
            adapter = new SqlDataAdapter("SELECT * FROM Truck where Id =" + truckBox.SelectedValue, connection);
            adapter.Fill(ds);
            if (radioTruckBD.Checked)
            {
                selectedTruck.Text = "Параметры выбранного транспорта:" + "\nШирина: " + ds.Tables[0].Rows[0].ItemArray[2] + "\nВысота: " + ds.Tables[0].Rows[0].ItemArray[3]
                    + "\nДлинна: " + ds.Tables[0].Rows[0].ItemArray[4] + "\nГрузоподъемность: " + ds.Tables[0].Rows[0].ItemArray[5];
                truck = "" + ds.Tables[0].Rows[0].ItemArray[2] + "," + ds.Tables[0].Rows[0].ItemArray[3] + "," + ds.Tables[0].Rows[0].ItemArray[4] + "," + ds.Tables[0].Rows[0].ItemArray[5];
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
            string a  = truckBox.Text;
            adapter = new SqlDataAdapter("SELECT * FROM Cargo where Id =" + cargoBox.SelectedValue, connection);
            adapter.Fill(ds);
            if (radioCargoBD.Checked)
            {
                listBox1.Items.Add(cargoBox.Text + " " + numericUpDown10.Value + " шт.");

            }
            else if(radioCargo.Checked)
            {
                if (String.IsNullOrWhiteSpace(textBox1.Text)) MessageBox.Show("Введите название для товара"); 
                else listBox1.Items.Add(textBox1.Text + " " + numericUpDown10.Value + " шт.");
            }
            else MessageBox.Show("Выберите вариант задания параметров грузов");
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
            Report f1 = new Report();
            f1.Show();
        }
    }
}

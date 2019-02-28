using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TiPEIS
{
    public partial class FormSuppliers : Form
    {
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private string sPath = Path.Combine(Application.StartupPath, "mydb.db");
        public FormSuppliers()
        {
            InitializeComponent();
        }

        private void FormSuppliers_Load(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            String selectCommand = "Select * from Suppliers";
            selectTable(ConnectionString, selectCommand);
        }

        public void selectTable(string ConnectionString, String selectCommand)
        {
            SQLiteConnection connect = new SQLiteConnection(ConnectionString);
            try
            {
                connect.Open();
            }
            catch (SqlException se)
            {
                Console.WriteLine("Ошибка подключения:{0}", se.Message);
                MessageBox.Show("Ошибка");
                return;
            }
            SQLiteDataAdapter dataAdapter = new
            SQLiteDataAdapter(selectCommand, connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = ds.Tables[0].ToString();
            connect.Close();
        }

        public void changeValue(string ConnectionString, String selectCommand)
        {
            SQLiteConnection connect = new SQLiteConnection(ConnectionString);
            connect.Open(); SQLiteTransaction trans; SQLiteCommand cmd = new SQLiteCommand();
            trans = connect.BeginTransaction();
            cmd.Connection = connect; cmd.CommandText = selectCommand;
            cmd.ExecuteNonQuery();
            trans.Commit();
            connect.Close();
        }

        public void refreshForm(string ConnectionString, String selectCommand)
        {
            selectTable(ConnectionString, selectCommand);
            dataGridView1.Update();
            dataGridView1.Refresh();
            toolStripTextBox1.Text = "";
        }
        private void ExecuteQuery(string txtQuery)
        {
            sql_con = new SQLiteConnection("Data Source=" + sPath + ";Version=3;New=False;Compress=True;");
            sql_con.Open(); sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = txtQuery; sql_cmd.ExecuteNonQuery();
            sql_con.Close();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            String selectCommand = "select MAX(idSuppliers) from Suppliers";
            object maxValue = selectValue(ConnectionString, selectCommand);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            if (String.IsNullOrWhiteSpace(toolStripTextBox1.Text))
            {
                MessageBox.Show("Заполнены не все поля");
            }
            else if (toolStripTextBox1.Text.Length > 50)
            {
                MessageBox.Show("Поле Имя должно содержать менее 50 символов");
                toolStripTextBox1.Text = "";
            }
            else
            {
                string txtSQLQuery = "insert into Suppliers (idSuppliers, Name) values ("
                + (Convert.ToInt32(maxValue) + 1) + ", '" + toolStripTextBox1.Text + "')";
                ExecuteQuery(txtSQLQuery);
                selectCommand = "select * from Suppliers";
                refreshForm(ConnectionString, selectCommand);
                toolStripTextBox1.Text = "";
            }

        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            String selectCommand = "delete from Suppliers where idSuppliers=" + valueId;
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            changeValue(ConnectionString, selectCommand);
            selectCommand = "select * from Suppliers";
            refreshForm(ConnectionString, selectCommand);
            toolStripTextBox1.Text = "";

        }

        private void toolStripButtonChange_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            string changeName = toolStripTextBox1.Text;
            if (String.IsNullOrWhiteSpace(toolStripTextBox1.Text))
            {
                MessageBox.Show("Заполнены не все поля");
            }
            else if (toolStripTextBox1.Text.Length > 50)
            {
                MessageBox.Show("Поле Имя должно содержать менее 50 символов");
                toolStripTextBox1.Text = "";
            }
            else
            {
                String selectCommand = "update Suppliers set Name='" + changeName + "' where idSuppliers=" + valueId;
                string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
                changeValue(ConnectionString, selectCommand);
                selectCommand = "select * from Suppliers";
                refreshForm(ConnectionString, selectCommand);
                toolStripTextBox1.Text = "";
            }
        }

            public object selectValue(string ConnectionString, String selectCommand)
            {
                SQLiteConnection connect = new SQLiteConnection(ConnectionString);
                connect.Open();
                SQLiteCommand command = new SQLiteCommand(selectCommand, connect);
                SQLiteDataReader reader = command.ExecuteReader();
                object value = "";
                while (reader.Read())
                {
                    value = reader[0];
                }
                connect.Close();
                return value;
            }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            string nameId = dataGridView1[1, CurrentRow].Value.ToString();
            toolStripTextBox1.Text = nameId;
        }
    }
}
    


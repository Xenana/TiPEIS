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
    public partial class FormMaterials : Form
    {
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private string sPath = Path.Combine(Application.StartupPath, "mydb.db");
        public FormMaterials()
        {
            InitializeComponent();
        }

        Regex regPrice = new Regex(@"^[0-9]{0,12}(?:[.,][0-9]{0,2})?\z");
        Regex regName = new Regex(@"^[a-zA-Zа-яА-ЯёЁ\s]{1,80}\z");

        private void FormMaterials_Load(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            String selectCommand = "Select * from Materials";
            selectTable(ConnectionString, selectCommand);
            String selectSubaccount = "SELECT idChart, NumAccounts FROM ChartOfAccounts";
            selectCombo(ConnectionString, selectSubaccount, toolStripComboBox_Subaccount, "NumAccounts", "idChart");
            toolStripComboBox_Subaccount.SelectedIndex = -1;

        }

        public void selectCombo(string ConnectionString, String selectCommand, ToolStripComboBox comboBox, string displayMember, string valueMember)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteDataAdapter dataAdapter = new
           SQLiteDataAdapter(selectCommand, connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            toolStripComboBox_Subaccount.ComboBox.DataSource = ds.Tables[0];
            toolStripComboBox_Subaccount.ComboBox.DisplayMember = displayMember;
            toolStripComboBox_Subaccount.ComboBox.ValueMember = valueMember;
            connect.Close();
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
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(selectCommand, connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = ds.Tables[0].ToString();
            connect.Close();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            //Для правильной записи нового кода необходимо запросить максимальное значение idMOL из таблицы MOL
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            String selectCommand = "select MAX(idMaterials) from Materials";
            object maxValue = selectValue(ConnectionString, selectCommand);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            string cost = "";
            cost = toolStripTextBox2.Text.Replace(",", ".");
            if (String.IsNullOrWhiteSpace(toolStripTextBox2.Text) || String.IsNullOrWhiteSpace(toolStripTextBox1.Text))
            {
                MessageBox.Show("Заполнены не все поля");
            }
            else if ((cost.IndexOf('.') == -1) && (cost.Length > 12))
            {
                MessageBox.Show("Цена должна быть менее 12 цифр");
            }
            else if ((cost.IndexOf('.') != -1) && (cost.Substring(0, cost.LastIndexOf('.')).Length > 12))
            {
                MessageBox.Show("Цена должна быть менее 12 цифр");
            }
            else if (!regPrice.IsMatch(toolStripTextBox2.Text))
            {
                MessageBox.Show("Несоответствие формату");
            }
            else
            {
                string txtSQLQuery = "insert into Materials (idMaterials, Name, Cost, Subaccount) values ("
                + (Convert.ToInt32(maxValue) + 1) + ", '" + toolStripTextBox1.Text + "', " + cost + ", '" + toolStripComboBox_Subaccount.Text + "')";


                ExecuteQuery(txtSQLQuery);

                //обновление dataGridView1
                selectCommand = "select * from Materials";
                refreshForm(ConnectionString, selectCommand);
                toolStripTextBox1.Text = "";
                toolStripTextBox2.Text = "";
                toolStripComboBox_Subaccount.SelectedIndex = -1;


            }
        }



        private void ExecuteQuery(string txtQuery)
        {
            sql_con = new SQLiteConnection("Data Source=" + sPath + ";Version=3;New=False;Compress=True;");
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = txtQuery;
            sql_cmd.ExecuteNonQuery();
            sql_con.Close();
        }

        public void refreshForm(string ConnectionString, String selectCommand)
        {
            selectTable(ConnectionString, selectCommand);
            dataGridView1.Update();
            dataGridView1.Refresh();
            toolStripTextBox1.Text = "";
            toolStripTextBox2.Text = "";
            toolStripComboBox_Subaccount.SelectedIndex = -1;
        }

     

        public object selectValue(string ConnectionString, String selectCommand)
        {
            SQLiteConnection connect = new
            SQLiteConnection(ConnectionString);
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

        private void toolStripButtonChange_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            //выбрана строка CurrentRow 
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение Name выбранной строки 
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            string changeName = toolStripTextBox1.Text;
            string changeCost = toolStripTextBox2.Text;
            string changeSubaccount = toolStripComboBox_Subaccount.Text;
            changeCost = toolStripTextBox2.Text.Replace(",", ".");
            //обновление Название 
            if (String.IsNullOrWhiteSpace(changeName))
            {
                MessageBox.Show("Поле Название не заполнено");
            }
            else
            {
                String selectCommand = "update Materials set Name='" + changeName + "' where idMaterials=" + valueId;
                string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
                changeValue(ConnectionString, selectCommand);
            }
            //обновление Цена 
            if (String.IsNullOrWhiteSpace(changeCost))
            {
                MessageBox.Show("Поле Цена не заполнено");
            }
            else if ((changeCost.IndexOf('.') == -1) && (changeCost.Length > 12))
            {
                MessageBox.Show("Цена должна быть менее 12 цифр");
            }
            else if ((changeCost.IndexOf('.') != -1) && (changeCost.Substring(0, changeCost.LastIndexOf('.')).Length > 12))
            {
                MessageBox.Show("Цена должна быть менее 12 цифр");
            }
            else if (!regPrice.IsMatch(toolStripTextBox2.Text))
            {
                MessageBox.Show("Несоответствие формату");
            }
            else
            {
                String selectCommand2 = "update Materials set Cost='" + changeCost + "' where idMaterials=" + valueId;
                string ConnectionString2 = @"Data Source=" + sPath + ";New=False;Version=3";
                changeValue(ConnectionString2, selectCommand2);
                
            }
            //обновление субсчета
            if (String.IsNullOrWhiteSpace(changeSubaccount))
            {
                MessageBox.Show("Поле Субсчёт не заполнено");
            }
            else
            {
                String selectCommand3 = "update Materials set Subaccount='" + changeSubaccount + "' where idMaterials=" + valueId;
                string ConnectionString3 = @"Data Source=" + sPath + ";New=False;Version=3";
                changeValue(ConnectionString3, selectCommand3);
                //обновление dataGridView1 
                selectCommand3 = "select * from Materials";
                refreshForm(ConnectionString3, selectCommand3);
                toolStripTextBox1.Text = "";
                toolStripTextBox2.Text = "";
                toolStripComboBox_Subaccount.SelectedIndex = -1;
            }

        }

        public void changeValue(string ConnectionString, String selectCommand)
        {
            SQLiteConnection connect = new
            SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteTransaction trans;
            SQLiteCommand cmd = new SQLiteCommand();
            trans = connect.BeginTransaction();
            cmd.Connection = connect;
            cmd.CommandText = selectCommand;
            cmd.ExecuteNonQuery();
            trans.Commit();
            connect.Close();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение Name выбранной строки
            string nameId = dataGridView1[1, CurrentRow].Value.ToString();
            string costId = dataGridView1[2, CurrentRow].Value.ToString();
            toolStripTextBox1.Text = nameId;
            toolStripTextBox2.Text = costId;
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение idMOL выбранной строки
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            String selectCommand = "delete from Materials where idMaterials=" + valueId;
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            changeValue(ConnectionString, selectCommand);
            //обновление dataGridView1
            selectCommand = "select * from Materials";
            refreshForm(ConnectionString, selectCommand);
            toolStripTextBox1.Text = "";
            toolStripTextBox2.Text = "";
        }
    }
}

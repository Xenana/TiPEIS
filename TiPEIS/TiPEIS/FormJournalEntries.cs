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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TiPEIS
{
    public partial class FormJournalEntries : Form
    {
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private string sPath = Path.Combine(Application.StartupPath, "mydb.db");

        public FormJournalEntries()
        {
            InitializeComponent();
        }

        private void FormJournalEntries_Load(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            selectTable(ConnectionString);
            // выбрать значения из справочников для отображения в comboBox
            String selectMaterial = "Select idMaterials, Name from Materials";
            selectCombo(ConnectionString, selectMaterial, comboBoxMaterials, "Name", "idMaterials");
            String selectStock = "SELECT idStock, Name FROM Stock";
            selectCombo(ConnectionString, selectStock, comboBoxStocks, "Name", "idStock");
            String selectMOL = "SELECT idMOL, Name FROM MOL";
            selectCombo(ConnectionString, selectMOL, comboBoxMOL, "Name", "idMOL");
            comboBoxMaterials.SelectedIndex = -1;
            comboBoxStocks.SelectedIndex = -1;
            comboBoxMOL.SelectedIndex = -1;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            // находим максимальное значение кода проводок для записи первичного ключа
            String mValue = "select MAX(IdJournalEntries) from JournalEntries";
            object maxValue = selectValue(ConnectionString, mValue);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            // Обнулить значения переменных
            string sum = "0";
            string count = "0";
            string coment = null;
            string material = null;
            string stock = null;
            string mol = null;
            if (comboBoxMaterials.Text != "")
            {
                //ОС
                material = comboBoxMaterials.SelectedValue.ToString();
            }
            if (comboBoxStocks.Text != "")
            {
                //Подразделение
                stock = comboBoxStocks.SelectedValue.ToString();
            }
            if (comboBoxMOL.Text != "")
            {
                //МОЛ
                mol = comboBoxMOL.SelectedValue.ToString();
            }

            //Поле количество
            if (textBoxCount.Text != "")
            {
                count = textBoxCount.Text;
            }
            //Поиск по базе данных значений
            object Cost = selectValue(ConnectionString, "select Cost from Materials where IdMaterials=" + material + ";");
            double Summa = Convert.ToDouble(Cost) * Convert.ToDouble(count);
            String selectDT = "select idChart from ChartOfAccounts where NumAccounts = 10";
            object DT = selectValue(ConnectionString, selectDT);
            String selectKT = "select idChart from ChartOfAccounts where NumAccounts = 60";
            object KT = selectValue(ConnectionString, selectKT);
            string add = "INSERT INTO JournalEntries (IdJournalEntries, Date, Comment, DT, SubkontoDT1, SubkontoDT2, SubkontoDT3, KT, Count, Summa) VALUES(" + 
                (Convert.ToInt32(maxValue) + 1) + ",'" + maskedTextBox1.Text + "'," + "'Поступление_Материалов'"  + "," + DT.ToString() + ", " + Convert.ToInt32(material) + "," + Convert.ToInt32(stock) + "," + 
                Convert.ToInt32(mol) + "," + KT.ToString() + "," + Convert.ToDouble(count) + "," + Summa.ToString().Replace(',' , '.') + ")";
                ExecuteQuery(add);
                selectTable(ConnectionString);
            String selectCommand = "select MAX(IdJournalEntries) from JournalEntries";
            selectCommand = "select * from JournalEntries";
            refreshForm(ConnectionString, selectCommand);
        }

        public void refreshForm(string ConnectionString, String selectCommand)
        {
            selectTable(ConnectionString, selectCommand);
            dataGridView1.Update();
            dataGridView1.Refresh();
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

        private void ExecuteQuery(string txtQuery)
        {
            sql_con = new SQLiteConnection("Data Source=" + sPath + ";Version=3;New=False;Compress=True;");
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = txtQuery;
            sql_cmd.ExecuteNonQuery();
            sql_con.Close();
        }
        // метод выгрузки таблицы на форму
        public void selectTable(string ConnectionString)
        {
            try
            {
               SQLiteConnection connect = new
               SQLiteConnection(ConnectionString);
               connect.Open();
               SQLiteDataAdapter dataAdapter = new
               SQLiteDataAdapter("Select IdJournalEntries, Date,Comment, DT, SubkontoDT1, SubkontoDT2, SubkontoDT3, KT, SubkontoKT1, SubkontoKT2, SubkontoKT3, Count, Summa, IDOperation from JournalEntries", connect);
               DataSet ds = new DataSet();
               dataAdapter.Fill(ds);
               dataGridView1.DataSource = ds;
               dataGridView1.DataMember = ds.Tables[0].ToString();
               connect.Close();
               dataGridView1.Columns["IdJournalEntries"].DisplayIndex = 0;
               dataGridView1.Columns["Date"].DisplayIndex = 1;
                dataGridView1.Columns["Comment"].DisplayIndex = 2;
               dataGridView1.Columns["DT"].DisplayIndex = 3;
                dataGridView1.Columns["SubkontoDT1"].DisplayIndex = 4;
               dataGridView1.Columns["SubkontoDT2"].DisplayIndex = 5;
               dataGridView1.Columns["SubkontoDT3"].DisplayIndex = 6;
               dataGridView1.Columns["KT"].DisplayIndex = 7;
            
               dataGridView1.Columns["SubkontoKT1"].DisplayIndex = 8;
               dataGridView1.Columns["SubkontoKT2"].DisplayIndex = 9;
               dataGridView1.Columns["SubkontoKT3"].DisplayIndex = 10;
               dataGridView1.Columns["Count"].DisplayIndex = 11;
               dataGridView1.Columns["Summa"].DisplayIndex = 12;
               dataGridView1.Columns["IDOperation"].DisplayIndex = 13;
            }
            catch
            {

            }
        }  
        //метод отображения в ComboBox значений из базы данных
        public void selectCombo(string ConnectionString, String selectCommand, ComboBox comboBox, string displayMember, string valueMember)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteDataAdapter dataAdapter = new
           SQLiteDataAdapter(selectCommand, connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            comboBox.DataSource = ds.Tables[0];
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;
            connect.Close();
        }
        // метод возвращает значение записи из таблицы
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

        private void buttonClear_Click(object sender, EventArgs e)
        {
            comboBoxMaterials.SelectedIndex = -1;
            comboBoxStocks.SelectedIndex = -1;
            comboBoxMOL.SelectedIndex = -1;
            textBoxComment.Clear();
            textBoxNamOper.Clear();
            textBoxCount.Clear();
            maskedTextBox1.Clear();
        }
    }
}

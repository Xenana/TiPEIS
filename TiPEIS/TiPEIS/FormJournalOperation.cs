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
    public partial class FormJournalOperation : Form
    {
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private string sPath = Path.Combine(Application.StartupPath, "mydb.db");

        public int Id { set { id = value; } }

        private int id;

        private object maxValueOperation;

        //FormAddOperation f = new FormAddOperation();

        public FormJournalOperation()
        {
            InitializeComponent();
        }

        private void FormJournalOperation_Load(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            String selectCommand = "Select * from JournalOperation";
            selectTable(ConnectionString, selectCommand);
            
        }

        private void ExecuteQuery(string txtQuery)
        {
            sql_con = new SQLiteConnection("Data Source=" + sPath + ";Version=3;New=False;Compress=True;");
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = txtQuery;
            sql_cmd.ExecuteNonQuery();
            sql_con.Close();
        }        public void selectTable(string ConnectionString)
        {
            try
            {
                SQLiteConnection connect = new
               SQLiteConnection(ConnectionString);
                connect.Open();
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("Select IDJournalOperation, DateOperation, Summa, StockSender, StockRecipient, MOLSender, MOLRecipient" + "where IDJournalOperation=" + id, connect);

                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = ds.Tables[0].ToString();
                connect.Close();
                dataGridView1.Columns["IDJournalOperation"].DisplayIndex = 0;
                dataGridView1.Columns["DateOperation"].DisplayIndex = 1;
                dataGridView1.Columns["Summa"].DisplayIndex = 2;
                dataGridView1.Columns["StockSender"].DisplayIndex = 3;
                dataGridView1.Columns["StockRecipient"].DisplayIndex = 4;
                dataGridView1.Columns["MOLSender"].DisplayIndex = 5;
                dataGridView1.Columns["MOLRecipient"].DisplayIndex = 6;
            }
            catch (Exception ex)
            {

            }
            }

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

        public void refreshForm(string ConnectionString, String selectCommand)
        {
            selectTable(ConnectionString, selectCommand);
            dataGridView1.Update();
            dataGridView1.Refresh();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {

            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            //посчитать максВалуе+1 от операций, и передать
            String mValueOperation = "select MAX(IDJournalOperation) from JournalOperation";
            maxValueOperation = selectValue(ConnectionString, mValueOperation);
            if (Convert.ToString(maxValueOperation) == "")
                maxValueOperation = 0;

            //  f.Id = 1 + Convert.ToInt32(maxValueOperation);

            FormAddOperation f = new FormAddOperation();
            f.ShowDialog();

            String selectCommand = "select MAX(IDJournalOperation) from JournalOperation";
            selectCommand = "select * from JournalOperation";
            refreshForm(ConnectionString, selectCommand);


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

        

      

        public void changeValue(string ConnectionString, String selectCommand)
        {
            SQLiteConnection connect = new SQLiteConnection(ConnectionString);
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

     /*   private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение Name выбранной строки
            string date = dataGridView1[1, CurrentRow].Value.ToString();
            string summ = dataGridView1[2, CurrentRow].Value.ToString();
            string stockS = dataGridView1[3, CurrentRow].Value.ToString();
            string stockR = dataGridView1[4, CurrentRow].Value.ToString();
            string molS = dataGridView1[5, CurrentRow].Value.ToString();
            string molR = dataGridView1[6, CurrentRow].Value.ToString();
            dateTimePicker1.Text = date;
            textBoxSumma.Text = summ;
            comboBoxStockSend.Text = stockS;
            comboBoxStockRec.Text = stockR;
            comboBoxMOLSend.Text = molS;
            comboBoxMOLRec.Text = molR;
        }
        */

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Данные отсутствуют. Что удалять?", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //выбрана строка CurrentRow 
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение idOS выбранной строки 
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            String selectCommand = "delete from JournalOperation where IDJournalOperation=" + valueId;
            String selectCommand1 = "delete from TablePart where IdOperation=" + valueId;
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            changeValue(ConnectionString, selectCommand);
            changeValue(ConnectionString, selectCommand1);
            //обновление dataGridView1 
            selectCommand = "select * from JournalOperation";
            selectCommand1 = "select * from TablePart";
            refreshForm(ConnectionString, selectCommand);

        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Данные отсутствуют. Что изменять?", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            id = Convert.ToInt32(valueId);

            FormAddOperation f = new FormAddOperation();
            f.Id = id;
            f.ShowDialog();

            String selectCommand = "select MAX(IDJournalOperation) from JournalOperation";
            selectCommand = "select * from JournalOperation";
            refreshForm(ConnectionString, selectCommand);

        }
    }
}


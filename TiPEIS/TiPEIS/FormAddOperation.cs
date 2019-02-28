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
    public partial class FormAddOperation : Form
    {
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private static string sPath = Path.Combine(Application.StartupPath, "mydb.db");
        string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";

        public int Id { set { id = value; } }

        private int id=0;

        private object maxValueOperation;

        private bool btnAddOperation = false;

        private new bool Update = false;

        Regex regexCount = new Regex(@"^[0-9]{1,5}$");

        public FormAddOperation()
        {
            InitializeComponent();
        }

        private void FormAddOperation_Load(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            String selectCommand = "select T.idTable, T.Count, T.Summa, T.IdOperation, M.Name from TablePart T left outer join Materials M on (T.Materials = M.idMaterials) where T.IdOperation=" + id;
            selectTable(ConnectionString, selectCommand);
            // выбрать значения из справочников для отображения в comboBox
            String selectMat = "Select idMaterials, Name from Materials";
            selectCombo(ConnectionString, selectMat, comboBoxMaterial, "Name", "idMaterials");
            String selectMOL = "SELECT idMOL, Name FROM MOL";
            selectCombo(ConnectionString, selectMOL, comboBoxMOLSend, "Name", "idMOL");
            selectCombo(ConnectionString, selectMOL, comboBoxMOLRec, "Name", "idMOL");
            String selectStock = "SELECT idStock, Name FROM Stock";
            selectCombo(ConnectionString, selectStock, comboBoxStockSend, "Name", "idStock");
            selectCombo(ConnectionString, selectStock, comboBoxStockRec, "Name", "idStock");
            textBoxSumma.Text = "";
            comboBoxMaterial.SelectedIndex = -1;
            comboBoxMOLSend.SelectedIndex = -1;
            comboBoxMOLRec.SelectedIndex = -1;            comboBoxStockSend.SelectedIndex = -1;
            comboBoxStockRec.SelectedIndex = -1;

            CalcTotalSum();
            if (id != 0)
            {
                btnAddOperation = true;
                Update = true;
                LoadOldInf();
            }
            if (id == 0) //новая операция
            {
                String mValueOperation = "select MAX(IDJournalOperation) from JournalOperation";
                maxValueOperation = selectValue(ConnectionString, mValueOperation);
                if (Convert.ToString(maxValueOperation) == "")
                    maxValueOperation = 0;
                id = Convert.ToInt32(maxValueOperation) + 1;
            }
            
        }

        private void LoadOldInf()
        {
            try
            {
                string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
                object idStockSend = selectValue(ConnectionString, "select StockSender from JournalOperation where IDJournalOperation=" + id + ";");
                comboBoxStockSend.SelectedValue = idStockSend;
                object idStockRec = selectValue(ConnectionString, "select StockRecipient from JournalOperation where IDJournalOperation=" + id + ";");
                comboBoxStockRec.SelectedValue = idStockRec;
                object idMOLSend = selectValue(ConnectionString, "select MOLSender from JournalOperation where IDJournalOperation=" + id + ";");
                comboBoxMOLSend.SelectedValue = idMOLSend;
                object idMOLRec = selectValue(ConnectionString, "select MOLRecipient from JournalOperation where IDJournalOperation=" + id + ";");
                comboBoxMOLRec.SelectedValue = idMOLRec;
            }
            catch {
            }
        }

       /* public void selectTable(string ConnectionString)
        {
            try
            {
                SQLiteConnection connect = new SQLiteConnection(ConnectionString);
                connect.Open();
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("Select idTable, Count, Summa, IdOperation, Materials from TablePart " + "where IdOperation=" + id, connect);

                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = ds.Tables[0].ToString();
                connect.Close();
                dataGridView1.Columns["idTable"].DisplayIndex = 0;
                dataGridView1.Columns["Count"].DisplayIndex = 1;
                dataGridView1.Columns["Summa"].DisplayIndex = 2;
                dataGridView1.Columns["IdOperation"].DisplayIndex = 3;
                dataGridView1.Columns["Materials"].DisplayIndex = 4;
            }
            catch
            {

            }
        }*/

        public void selectCombo(string ConnectionString, String selectCommand, ComboBox comboBox, string displayMember, string valueMember)
        {
            SQLiteConnection connect = new SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(selectCommand, connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            comboBox.DataSource = ds.Tables[0];
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;
            connect.Close();
        }

        private void buttonCalc_Click(object sender, EventArgs e)
        {
            string count = textBoxCount.Text;
            string material = null;
            if (comboBoxMaterial.Text == "")
            {
                MessageBox.Show("Выберете материал", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBoxCount.Text == "")
            {
                MessageBox.Show("Укажите количество материала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!regexCount.IsMatch(textBoxCount.Text))
            {
                MessageBox.Show("Несоответсвие формату", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (count.Length > 10)
            {
                MessageBox.Show("Количество должно быть менее 10 цифр");
            }
            else if (Convert.ToInt32(count) == 0)
            {
                MessageBox.Show("Укажите количество материала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (comboBoxMaterial.Text != "")
            {
                material = comboBoxMaterial.SelectedValue.ToString();
            }
            if (textBoxCount.Text != "")
            {
                count = textBoxCount.Text;
            }
                string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
                object Cost = selectValue(ConnectionString, "select Cost from Materials where IdMaterials=" + material + ";");
                double Summa = Convert.ToDouble(Cost) * Convert.ToDouble(count);
                textBoxSumma.Text = Summa.ToString();
            
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

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (comboBoxMaterial.Text == "")
            {
                MessageBox.Show("Выберете материал", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            

            if (textBoxCount.Text == "")
            {
                MessageBox.Show("Укажите количество материала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Regex regexCount = new Regex(@"^[0-9]+$");
            

            // находим максимальное значение кода проводок для записи первичного ключа
            String mValue = "select MAX(idTable) from TablePart";
            object maxValue = selectValue(ConnectionString, mValue);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            // Обнулить значения переменных
            string count = textBoxCount.Text;
            string material = null;
            if (comboBoxMaterial.Text != "")
            {
                material = comboBoxMaterial.SelectedValue.ToString();
            }
            //Поле количество
            if (!regexCount.IsMatch(textBoxCount.Text))
            {
                MessageBox.Show("Несоответсвие формату", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if  (count.Length > 10)
            {
                MessageBox.Show("Количество должно быть менее 10 цифр");
            }
            else if (Convert.ToInt32(count) == 0)
            {
                MessageBox.Show("Укажите количество материала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                count = textBoxCount.Text;
                object Cost = selectValue(ConnectionString, "select Cost from Materials where IdMaterials=" + material + ";");
                double Summa = Convert.ToDouble(Cost) * Convert.ToDouble(count);
                if (dataGridView1.Rows.Count == 0)
                {

                    string add = "INSERT INTO TablePart (idTable, " + "Count, Summa, idOperation, Materials) VALUES("
                        + (Convert.ToInt32(maxValue) + 1) + "," + count + "," + Summa.ToString().Replace(",", ".") + "," + id + ","
                        + material + ")";

                    ExecuteQuery(add);

                }
                else
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (((dataGridView1.Rows[i].Cells["Name"].Value).ToString() == comboBoxMaterial.Text))

                        {
                            int countAll = Convert.ToInt32(dataGridView1.Rows[i].Cells["Count"].Value) + Convert.ToInt32(count);
                            double sumAll = Summa + Convert.ToDouble(dataGridView1.Rows[i].Cells["Summa"].Value);
                            string upd = "update TablePart set  Count=" + countAll + ",Summa='" + sumAll.ToString().Replace(",", ".") + "',idOperation="
                            + id + ",Materials=" + material + " where idTable=" + (dataGridView1.Rows[i].Cells["idTable"].Value);
                            changeValue(ConnectionString, upd);
                            string ConnectionString4 = @"Data Source=" + sPath + ";New=False;Version=3";
                            String selectCommand4 = "select T.idTable, T.Count, T.Summa, T.IdOperation, M.Name from TablePart T left outer join Materials M on (T.Materials = M.idMaterials) where T.IdOperation=" + id;
                            selectTable(ConnectionString4, selectCommand4);
                            return;
                        }
                    }
                    string add = "INSERT INTO TablePart (idTable, Count, Summa, idOperation, Materials) VALUES("
                       + (Convert.ToInt32(maxValue) + 1) + "," + count + "," + Summa.ToString().Replace(",", ".") + "," + id + ","
                       + material + ")";

                    ExecuteQuery(add);


                  /*  for (int i = 0; ((dataGridView1.Rows[i].Cells["Name"].Value).ToString() != comboBoxMaterial.Text); i++)
                    {
                        if (i < dataGridView1.Rows.Count)
                        {
                            string add = "INSERT INTO TablePart (idTable, " + "Count, Summa, idOperation, Materials) VALUES("
                        + (Convert.ToInt32(maxValue) + 1) + "," + count + "," + Summa.ToString().Replace(",", ".") + "," + id + "," + material + ")";

                            ExecuteQuery(add);
                        }


                    }*/
                }
                }
                /*string add = "INSERT INTO TablePart (idTable, " + "Count, Summa, idOperation, Materials) VALUES("
                    + (Convert.ToInt32(maxValue) + 1) + "," + count + "," + Summa.ToString().Replace(",", ".") + "," + id + ","
                    + material + ")";

                ExecuteQuery(add);*/
                String selectCommand = "select T.idTable, T.Count, T.Summa, T.IdOperation, M.Name from TablePart T left outer join Materials M on (T.Materials = M.idMaterials) where T.IdOperation="+ id;
                selectTable(ConnectionString, selectCommand);
               // refreshForm(ConnectionString, selectCommand);
                comboBoxMaterial.SelectedIndex = -1;
                textBoxCount.Text = "";
                textBoxSumma.Text = "";
                CalcTotalSum();
            }
            //Поиск по базе данных значений
            

        

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

        public void refreshForm(string ConnectionString, String selectCommand)
        {
            String selectComand = "select T.idTable, T.Count, T.Summa, T.IdOperation, M.Name from TablePart T left outer join Materials M on (T.Materials = M.idMaterials) where T.IdOperation=" + id;
            selectTable(ConnectionString, selectComand);
            dataGridView1.Update();
            dataGridView1.Refresh();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            btnAddOperation = true;

            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Добавьте материал", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (comboBoxStockSend.Text == "")
            {
                MessageBox.Show("Выберете Склад-отправитель", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (comboBoxStockRec.Text == "")
            {
                MessageBox.Show("Выберете Склад-получатель", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (comboBoxMOLSend.Text == "")
            {
                MessageBox.Show("Выберете МОЛ-отправитель", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (comboBoxMOLRec.Text == "")
            {
                MessageBox.Show("Выберете МОЛ-получатель", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if(comboBoxMOLSend.Text == comboBoxMOLRec.Text)
            {
                MessageBox.Show("МОЛ-отправитель и МОЛ-получатель должны различаться", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (comboBoxStockSend.Text == comboBoxStockRec.Text)
            {
                MessageBox.Show("Склад-отправитель и Склад-получатель должны различаться", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            // находим максимальное значение кода проводок для записи первичного ключа 
            String mValue = "select MAX(IDJournalOperation) from JournalOperation";
            object maxValue = selectValue(ConnectionString, mValue);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;

            // Обнулить значения переменных
            string date = dateTimePicker1.Text;
            string sum = textBoxTotal.Text;
            string ValueStockSend = null;
            string ValueStockRec = null;
            string ValueMOLSend = null;
            string ValueMOLRec = null;

            if (comboBoxStockSend.Text != "")
            {
                //StockSend 
                ValueStockSend = comboBoxStockSend.SelectedValue.ToString();
            }
            if (comboBoxStockRec.Text != "")
            {
                //StockRec
                ValueStockRec = comboBoxStockRec.SelectedValue.ToString();
            }
            if (comboBoxMOLSend.Text != "")
            {
                //MOLSend 
                ValueMOLSend = comboBoxMOLSend.SelectedValue.ToString();
            }
            if (comboBoxMOLRec.Text != "")
            {
                //MOLRec
                ValueMOLRec = comboBoxMOLRec.SelectedValue.ToString();
            }
            //найти все табличные части, с таким id
            //

            if (Update == true)
            {

                string selectCommandUp = "update JournalOperation set IDJournalOperation=" + (Convert.ToInt32(maxValue)) + ", DateOperation='" + dateTimePicker1.Text + "',Summa='" + sum.ToString().Replace(",", ".") + "',StockSender="
                    + Convert.ToInt32(ValueStockSend) + ",StockRecipient=" + Convert.ToInt32(ValueStockRec) + ",MOLSender=" + Convert.ToDouble(ValueMOLSend) + ",MOLRecipient=" + Convert.ToInt32(ValueMOLRec) + " where IDJournalOperation=" + id;
                changeValue(ConnectionString, selectCommandUp);

                DialogResult = DialogResult.Cancel;
                Close();
            }
            else
            {
                string add = "INSERT INTO JournalOperation (IDJournalOperation, " + "DateOperation, Summa, StockSender, StockRecipient, MOLSender, MOLRecipient) VALUES("
                + (Convert.ToInt32(maxValue) + 1) + ",'" + dateTimePicker1.Text + "','" + sum.ToString().Replace(",", ".") + "'," + Convert.ToInt32(ValueStockSend) + ","
                + Convert.ToInt32(ValueStockRec) + "," + Convert.ToDouble(ValueMOLSend) + "," + Convert.ToInt32(ValueMOLRec) + ")";
                ExecuteQuery(add);
                string selectcomand = "Select idTable, Count, Summa, IdOperation, Materials from TablePart " + "where IdOperation=" + id;
                selectTable(ConnectionString, selectcomand);

                DialogResult = DialogResult.Cancel;
                Close();
            }
            

        }

        public void CalcTotalSum()
        {
            double sum = 0;
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                sum = sum + Convert.ToDouble(dr.Cells["Summa"].Value);
            }
            textBoxTotal.Text = sum.ToString();
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            //выбрана строка CurrentRow 
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение Name выбранной строки 
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            string changeMaterial = comboBoxMaterial.SelectedValue.ToString();
            string changeCount = textBoxCount.Text;
            string count = "0";
            string material = null;
            if (comboBoxMaterial.Text != "")
            {
                material = comboBoxMaterial.SelectedValue.ToString();
            }
            //Поле количество
            if (textBoxCount.Text != "")
            {
                count = textBoxCount.Text;
            }
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            object Cost = selectValue(ConnectionString, "select Cost from Materials where IdMaterials=" + material + ";");
            string changeSumma = (Convert.ToDouble(Cost) * Convert.ToDouble(count)).ToString().Replace(",", ".");
            
            if (String.IsNullOrWhiteSpace(changeMaterial))
            {
                MessageBox.Show("Выберите материал");
            }
            else
            {

                String selectCommand = "update TablePart set Materials=" + changeMaterial + " where idTable=" + valueId;
                string ConnectionString5 = @"Data Source=" + sPath + ";New=False;Version=3";
                changeValue(ConnectionString5, selectCommand);
            }
            if (String.IsNullOrWhiteSpace(changeCount))
            {
                MessageBox.Show("Введите количество");
            }
            if (!regexCount.IsMatch(textBoxCount.Text))
            {
                MessageBox.Show("Несоответсвие формату", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (count.Length > 10)
            {
                MessageBox.Show("Количество должно быть менее 10 цифр");
            }
            else if (Convert.ToInt32(count) == 0)
            {
                MessageBox.Show("Укажите количество материала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                String selectCommand2 = "update TablePart set Count='" + changeCount + "' where idTable=" + valueId;
                string ConnectionString2 = @"Data Source=" + sPath + ";New=False;Version=3";
                changeValue(ConnectionString2, selectCommand2);
            }
            
                String selectCommand3 = "update TablePart set Summa='" + changeSumma + "' where idTable=" + valueId;
                string ConnectionString3 = @"Data Source=" + sPath + ";New=False;Version=3";
                changeValue(ConnectionString3, selectCommand3);
               
            
            string ConnectionString4 = @"Data Source=" + sPath + ";New=False;Version=3";
            String selectCommand4 = "select T.idTable, T.Count, T.Summa, T.IdOperation, M.Name from TablePart T left outer join Materials M on (T.Materials = M.idMaterials) where T.IdOperation=" + id;
            selectTable(ConnectionString4, selectCommand4);
            CalcTotalSum();
          //  refreshForm(ConnectionString4, selectCommand4);
            textBoxCount.Text = "";
            textBoxSumma.Text = "";
            comboBoxMaterial.Text = "";
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

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение idMOL выбранной строки
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            String selectCommand = "delete from TablePart where idTable=" + valueId;
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
            changeValue(ConnectionString, selectCommand);
            //обновление dataGridView1
            selectCommand = "select T.idTable, T.Count, T.Summa, T.IdOperation, M.Name from TablePart T left outer join Materials M on (T.Materials = M.idMaterials)  where T.IdOperation=" + id;
            selectTable(ConnectionString, selectCommand);
            CalcTotalSum();
        }
        

        private void FormAddOperation_FormClosed(object sender, FormClosedEventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";

            if (!btnAddOperation)
            {

                String selectCommand = "delete from TablePart where IdOperation=" + id;
                changeValue(ConnectionString, selectCommand);
                selectCommand = "select * from TablePart";
            }

        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //выбрана строка CurrentRow 
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;

            string countId = dataGridView1[1, CurrentRow].Value.ToString();
            string summaId = dataGridView1[2, CurrentRow].Value.ToString();
            string matName = dataGridView1[4, CurrentRow].Value.ToString();

            object mat = selectValue(ConnectionString, "select idMaterials from Materials where Name='" + matName + "';");
           // comboBoxStockSend.SelectedValue = idStockSend;


            textBoxCount.Text = countId;
            textBoxSumma.Text = summaId;
            comboBoxMaterial.SelectedValue = mat;

        }
    }
}

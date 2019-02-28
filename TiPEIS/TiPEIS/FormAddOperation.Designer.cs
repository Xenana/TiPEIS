namespace TiPEIS
{
    partial class FormAddOperation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxCount = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonCalc = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxMaterial = new System.Windows.Forms.ComboBox();
            this.comboBoxMOLRec = new System.Windows.Forms.ComboBox();
            this.comboBoxMOLSend = new System.Windows.Forms.ComboBox();
            this.comboBoxStockRec = new System.Windows.Forms.ComboBox();
            this.comboBoxStockSend = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSumma = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBoxTotal = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonChange = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxCount
            // 
            this.textBoxCount.Location = new System.Drawing.Point(940, 302);
            this.textBoxCount.Name = "textBoxCount";
            this.textBoxCount.Size = new System.Drawing.Size(95, 22);
            this.textBoxCount.TabIndex = 38;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(844, 302);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 17);
            this.label7.TabIndex = 37;
            this.label7.Text = "Количество:";
            // 
            // buttonCalc
            // 
            this.buttonCalc.Location = new System.Drawing.Point(891, 381);
            this.buttonCalc.Name = "buttonCalc";
            this.buttonCalc.Size = new System.Drawing.Size(116, 26);
            this.buttonCalc.TabIndex = 36;
            this.buttonCalc.Text = "Рассчитать";
            this.buttonCalc.UseVisualStyleBackColor = true;
            this.buttonCalc.Click += new System.EventHandler(this.buttonCalc_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(844, 242);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 17);
            this.label6.TabIndex = 35;
            this.label6.Text = "Материал:";
            // 
            // comboBoxMaterial
            // 
            this.comboBoxMaterial.FormattingEnabled = true;
            this.comboBoxMaterial.Location = new System.Drawing.Point(844, 262);
            this.comboBoxMaterial.Name = "comboBoxMaterial";
            this.comboBoxMaterial.Size = new System.Drawing.Size(228, 24);
            this.comboBoxMaterial.TabIndex = 34;
            // 
            // comboBoxMOLRec
            // 
            this.comboBoxMOLRec.FormattingEnabled = true;
            this.comboBoxMOLRec.Location = new System.Drawing.Point(706, 118);
            this.comboBoxMOLRec.Name = "comboBoxMOLRec";
            this.comboBoxMOLRec.Size = new System.Drawing.Size(228, 24);
            this.comboBoxMOLRec.TabIndex = 33;
            // 
            // comboBoxMOLSend
            // 
            this.comboBoxMOLSend.FormattingEnabled = true;
            this.comboBoxMOLSend.Location = new System.Drawing.Point(706, 56);
            this.comboBoxMOLSend.Name = "comboBoxMOLSend";
            this.comboBoxMOLSend.Size = new System.Drawing.Size(228, 24);
            this.comboBoxMOLSend.TabIndex = 32;
            // 
            // comboBoxStockRec
            // 
            this.comboBoxStockRec.FormattingEnabled = true;
            this.comboBoxStockRec.Location = new System.Drawing.Point(170, 117);
            this.comboBoxStockRec.Name = "comboBoxStockRec";
            this.comboBoxStockRec.Size = new System.Drawing.Size(228, 24);
            this.comboBoxStockRec.TabIndex = 31;
            // 
            // comboBoxStockSend
            // 
            this.comboBoxStockSend.FormattingEnabled = true;
            this.comboBoxStockSend.Location = new System.Drawing.Point(170, 59);
            this.comboBoxStockSend.Name = "comboBoxStockSend";
            this.comboBoxStockSend.Size = new System.Drawing.Size(228, 24);
            this.comboBoxStockSend.TabIndex = 30;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(567, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 17);
            this.label5.TabIndex = 29;
            this.label5.Text = "МОЛ-получатель:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(567, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 17);
            this.label4.TabIndex = 28;
            this.label4.Text = "МОЛ-отправитель:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(844, 333);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 17);
            this.label3.TabIndex = 27;
            this.label3.Text = "Сумма:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 17);
            this.label2.TabIndex = 26;
            this.label2.Text = "Склад-получатель:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 17);
            this.label1.TabIndex = 25;
            this.label1.Text = "Склад-отправитель:";
            // 
            // textBoxSumma
            // 
            this.textBoxSumma.Location = new System.Drawing.Point(847, 353);
            this.textBoxSumma.Name = "textBoxSumma";
            this.textBoxSumma.ReadOnly = true;
            this.textBoxSumma.Size = new System.Drawing.Size(200, 22);
            this.textBoxSumma.TabIndex = 24;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(2, 242);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(836, 292);
            this.dataGridView1.TabIndex = 39;
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(12, 12);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 22);
            this.dateTimePicker1.TabIndex = 40;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(726, 187);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(172, 29);
            this.buttonSave.TabIndex = 41;
            this.buttonSave.Text = "Сохранить операцию";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxTotal
            // 
            this.textBoxTotal.Location = new System.Drawing.Point(170, 187);
            this.textBoxTotal.Name = "textBoxTotal";
            this.textBoxTotal.ReadOnly = true;
            this.textBoxTotal.Size = new System.Drawing.Size(228, 22);
            this.textBoxTotal.TabIndex = 42;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 190);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(117, 17);
            this.label8.TabIndex = 43;
            this.label8.Text = "Итоговая сумма:";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(891, 429);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(116, 23);
            this.buttonAdd.TabIndex = 44;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(891, 503);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(116, 23);
            this.buttonDelete.TabIndex = 45;
            this.buttonDelete.Text = "Удалить";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonChange
            // 
            this.buttonChange.Location = new System.Drawing.Point(891, 468);
            this.buttonChange.Name = "buttonChange";
            this.buttonChange.Size = new System.Drawing.Size(116, 23);
            this.buttonChange.TabIndex = 46;
            this.buttonChange.Text = "Изменить";
            this.buttonChange.UseVisualStyleBackColor = true;
            this.buttonChange.Click += new System.EventHandler(this.buttonChange_Click);
            // 
            // FormAddOperation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 538);
            this.Controls.Add(this.buttonChange);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxTotal);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBoxCount);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.buttonCalc);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBoxMaterial);
            this.Controls.Add(this.comboBoxMOLRec);
            this.Controls.Add(this.comboBoxMOLSend);
            this.Controls.Add(this.comboBoxStockRec);
            this.Controls.Add(this.comboBoxStockSend);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSumma);
            this.Name = "FormAddOperation";
            this.Text = "FormAddOperation";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormAddOperation_FormClosed);
            this.Load += new System.EventHandler(this.FormAddOperation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonCalc;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxMaterial;
        private System.Windows.Forms.ComboBox comboBoxMOLRec;
        private System.Windows.Forms.ComboBox comboBoxMOLSend;
        private System.Windows.Forms.ComboBox comboBoxStockRec;
        private System.Windows.Forms.ComboBox comboBoxStockSend;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSumma;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBoxTotal;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonChange;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TiPEIS
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void планСчетовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form1 = new FormChartOfAccounts();
            form1.Show();
        }

        private void материальноответственныеЛицаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form1 = new FormMOL();
            form1.Show();
        }

        private void складыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form1 = new FormStock();
            form1.Show();
        }

        private void материалыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form1 = new FormMaterials();
            form1.Show();
        }

        private void поставщикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form1 = new FormSuppliers();
            form1.Show();
        }

        private void журналОперацийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form1 = new FormJournalOperation();
            form1.Show();
        }

        private void журналПроводокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form1 = new FormJournalEntries();
            form1.Show();
        }
    }
}

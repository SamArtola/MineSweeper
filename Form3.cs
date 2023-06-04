using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class Form3 : Form
    {
        public int Row;
        public int Column;
        public int Mines;
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Row = (int)numericUpDown1.Value;
            Column = (int)numericUpDown2.Value;
            Mines = (int)numericUpDown3.Value;
            if (Row <= 0 || Column <= 0 || Mines <= 0 || Row * Column < 18 || Mines > Row * Column / 2)
            {
                MessageBox.Show("Invalid parameters. Please try again.");
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace MineSweeper
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private int totalMines;

        public Form2(String text, int row, int col, int size, int mines) : this()
        {
            this.Text = text;
            totalMines = mines;
            label2.Text = "Mines: " + totalMines;
            field = new Field(row, col, mines);

            this.ClientSize = new Size(col * (size), row * size);

            buttons = new Button[row][];
            for (int i = 0; i < row; i++)
                buttons[i] = new Button[col];

            foreach (int i in Enumerable.Range(0, row))
                foreach (int j in Enumerable.Range(0, col))
                {
                    buttons[i][j] = new Button();
                    buttons[i][j].Text = "";
                    buttons[i][j].BackColor = Color.White;
                    buttons[i][j].Name = i + "," + j;
                    buttons[i][j].Size = new Size(size, size);
                    buttons[i][j].Location = new Point((size * j), (size * i));
                    buttons[i][j].UseVisualStyleBackColor = false;
                    buttons[i][j].MouseUp += new MouseEventHandler(Button_Click);
                    this.Controls.Add(buttons[i][j]);
                }
        }
        private void Button_Click(object sender, MouseEventArgs e)
        {
            Button b = (Button)sender;
            int temp = b.Name.IndexOf(",");
            int click_x = Int16.Parse(b.Name.Substring(0, temp));
            int click_y = Int16.Parse(b.Name.Substring(temp + 1));
            switch (e.Button)
            {
                case MouseButtons.Left:
                    // Left click

                    if (!this.field.Started)
                    {
                        this.field.Initialize(click_x, click_y);
                        InitializeTimer();
                        timer_flag= true;
                        timer1.Start();
                    }
                    int n = this.field.CountMines(click_x, click_y);
                    if (this.field.IsMine(click_x, click_y))
                    {
                        b.BackColor = Color.Red;
                        MessageBox.Show("Game Over! You clicked on a mine!");
                        Close();
                        break;
                    }
                    if (this.field.Discovered.Contains(click_x * buttons[0].Length + click_y))
                        break;
                    foreach (int k in this.field.GetSafeIsland(click_x, click_y))
                    {
                        int i = k / buttons[0].Length;
                        int j = k % buttons[0].Length;
                        buttons[i][j].BackColor = Color.LightGray;
                        int m = this.field.CountMines(i, j);
                        if (m > 0)
                        {
                            buttons[i][j].Text = m + "";
                            buttons[i][j].BackColor = Color.LightBlue;
                        }
                        else
                            buttons[i][j].Enabled = false;
                    }
                    if (field.Win())
                    {
                        MessageBox.Show("Congratulations! You discovered all safe squares! You won in " + (seconds / 2) + " seconds!");
                        Close();
                    }
                    break;
                case MouseButtons.Right:
                    // Right click
                    if (this.field.Discovered.Contains(click_x * buttons[0].Length + click_y))
                        break;
                    if (field.Flagged.Contains(click_x * buttons[0].Length + click_y))
                    {
                        b.BackColor = Color.White;
                        field.Flagged.Remove(click_x * buttons[0].Length + click_y);
                        totalMines += 1;
                        label2.Text = "Mines" + totalMines;
                    }
                    else
                    {
                        b.BackColor = Color.Green;
                        field.Flagged.Add(click_x * buttons[0].Length + click_y);
                        totalMines -= 1;
                        label2.Text = "Mines: " + totalMines;
                    }
                    break;
                case MouseButtons.Middle:
                    if (!this.field.Discovered.Contains(click_x * buttons[0].Length + click_y))
                        break;
                    int Flagged_Count = 0;
                    foreach (int k in this.field.GetNeighbors(click_x, click_y))
                        if (field.Flagged.Contains(k))
                            Flagged_Count++;
                    if (this.field.CountMines(click_x, click_y) != Flagged_Count)
                        break;
                    foreach (int k in this.field.GetNeighbors(click_x, click_y))
                    {
                        if (field.Flagged.Contains(k) || field.Discovered.Contains(k))
                            continue;
                        if (this.field.IsMine(k / buttons[0].Length, k % buttons[0].Length))
                        {
                            b.BackColor = Color.Red;
                            MessageBox.Show("Game Over! You clicked on a mine!");
                            Close();

                        }
                        foreach (int l in this.field.GetSafeIsland(k / buttons[0].Length, k % buttons[0].Length))
                        {
                            int i = l / buttons[0].Length;
                            int j = l % buttons[0].Length;
                            buttons[i][j].BackColor = Color.LightGray;
                            int m = this.field.CountMines(i, j);
                            if (m > 0)
                            {
                                buttons[i][j].Text = m + "";
                                buttons[i][j].BackColor = Color.LightBlue;
                            }
                            else
                                buttons[i][j].Enabled = false;
                        }
                        if (field.Win())
                        {
                            MessageBox.Show("Congratulations! You discovered all safe squares! You won in " + (seconds / 2) + " seconds!");
                            Close();
                        }
                    }
                    break;
            }


        }
        private Button[][] buttons;
        private Field field;

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }


        private void expert_Click(object sender, EventArgs e)
        {
            int row = 16;
            int col = 30;
            int mines = 99;
            String text = "Expert";

            int size = Math.Min(30, 1000 / Math.Max(row, col));
            Form2 f = new Form2(text, row, col, size, mines);
            f.Show();
        }

        private void medium_Click(object sender, EventArgs e)
        {
            int row = 0, col = 0, mines = 0;//row*col >=18, mines <= row*col/2
            String text = "";
            row = col = 16;
            text = "Medium";

            int size = Math.Min(30, 1000 / Math.Max(row, col));
            Form2 f = new Form2(text, row, col, size, mines);
            f.Show();
        }

        private void easy_Click(object sender, EventArgs e)
        {
            int row = 0, col = 0, mines = 0;//row*col >=18, mines <= row*col/2
            String text = "";
            row = col = 9;
            mines = 10;
            text = "Easy";


            int size = Math.Min(30, 1000 / Math.Max(row, col));
            Form2 f = new Form2(text, row, col, size, mines);
            f.Show();
        }

        private int seconds;
        bool timer_flag=false;
        private void InitializeTimer()
        {
            seconds = 0;
            timer1.Enabled = true;
            timer1.Tick += new EventHandler(timer1_Tick);

        }

        private void Form2_Deactivate(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void Form2_Activate(object sender, EventArgs e)
        {

            label3.Text = "Open Games: " + (System.Windows.Forms.Application.OpenForms.OfType<Form2>().Count()).ToString();
            if (timer_flag)
            {
                timer1.Start();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            seconds++;
            label1.Text = "Timer: " + (seconds / 2).ToString();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void closeAllGamesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            foreach (Form form in System.Windows.Forms.Application.OpenForms.OfType<Form2>().ToList())
            {
                form.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (seconds > 0)
            {
                string msg = "Do you want to close this game?";
                string title = "Close Game";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(msg, title, buttons);
                if (result == DialogResult.Yes)
                {
                    Close();
                }
            }
            else
                Close();

        }

        private void customToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int row = 0, col = 0, mines = 0;//row*col >=18, mines <= row*col/2
            String text = "";

            Form3 cust = new Form3();

            DialogResult result = cust.ShowDialog();
            row = cust.Row;
            col = cust.Column; mines = cust.Mines;
            text = "Custom";

            int size = Math.Min(30, 1000 / Math.Max(row, col));
            Form2 f = new Form2(text, row, col, size, mines);
            f.Show();
        }
    }
}

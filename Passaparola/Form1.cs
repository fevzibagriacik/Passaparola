using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Passaparola
{
    public partial class Form1 : Form
    {
        Button[] buttonsArray;
        SqlConnection conn = new SqlConnection(@"Data Source=Fevzi\SQLEXPRESS;Initial Catalog=Passaparola;Integrated Security=True");
        int questionNo = 0, t = 0, f = 0, score = 0, timer = 30;

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer--;
            lblTimer.Text = timer.ToString();

            if(timer == 0)
            {
                timer1.Stop();
                if(MessageBox.Show("Try Again.", "Time Out!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        string answer;
        public Form1()
        {
            InitializeComponent();

            buttonsArray = new Button[]
            {
                button1, button2, button3, button4, button5, button6, button7, button8,
                button9, button10, button11, button12, button13, button14, button15, button16,
                button17, button18, button19, button20, button21, button22, button23, button24
            };
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (questionNo == 24)
            {
                timer1.Stop();
                MessageBox.Show("Score: " + score.ToString(), "Congratulations", MessageBoxButtons.OK ,MessageBoxIcon.Information);
                Application.Exit();
                return;
            }

            timer = 30;
            lblTimer.Text = timer.ToString();
            button25.Text = buttonsArray[questionNo].Text;

            if (answer == textBox1.Text.ToLower())
            {
                buttonsArray[questionNo - 1].BackColor = Color.Green;
                t++;
                score += 5;
                lblTrue.Text = t.ToString();
            }
            else
            {
                buttonsArray[questionNo - 1].BackColor = Color.Red;
                f++;
                lblFalse.Text = f.ToString();
                score -= 5;
            }

            textBox1.Text = "";
            textBox1.Focus();

            questionNo++;
            this.Text = questionNo.ToString();
            buttonsArray[questionNo - 1].BackColor = Color.Yellow;

            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM TblQuestion where Question_No = @p1", conn);
            cmd.Parameters.AddWithValue("@p1", questionNo);

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                richTextBox1.Text = dr[2].ToString();
                answer = dr[3].ToString();
            }

            conn.Close();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.Visible = false;
            linkLabel2.Visible = true;
            questionNo++;
            this.Text = questionNo.ToString();
            buttonsArray[questionNo -1].BackColor = Color.Yellow;
            textBox1.Enabled = true;

            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM TblQuestion where Question_No = @p1", conn);
            cmd.Parameters.AddWithValue("@p1", questionNo);

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                richTextBox1.Text = dr[2].ToString();
                answer = dr[3].ToString();
            }

            conn.Close();

            button25.Text = buttonsArray[questionNo - 1].Text;
            timer1.Start();
        }
    }
}

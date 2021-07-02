using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormDB
{
    public partial class Form1 : Form
    {
        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=product;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)//ถ้าเป็นสมาชิกจะสามารถเข้าระบบได้
        {
            MySqlConnection conn = databaseConnection();
            conn.Open();
            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            
            cmd.CommandText = "SELECT status FROM member WHERE username = '" + textBox1.Text + "'And password ='" + textBox2.Text + "'";

            MySqlDataReader row = cmd.ExecuteReader();
            if (row.Read())
            {
                string status = row.GetString(0);//status admin
                MessageBox.Show("Log in comlplete :)");
                if (status == "admin") //ถ้าแอดมินจะไปform6
                {
                    Form6 a = new Form6();
                    this.Hide();
                    a.Show();
                }
                else//ถ้าไม่ใช่จะไปform5
                {
                    Program.username = textBox1.Text;
                    Form5 a = new Form5();
                    this.Hide();
                    a.Show();
                }
                
            }
            else//ถ้าไม่ใช่สมาชิกจะเข้าระบบไม่ได้
            {
                MessageBox.Show("Log in incomplete!!");
                conn.Close();

            }
           
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)//ไปที่ หน้าregister
        {
            Form4 a = new Form4();
            this.Hide();
            a.Show();
        }
    }
}

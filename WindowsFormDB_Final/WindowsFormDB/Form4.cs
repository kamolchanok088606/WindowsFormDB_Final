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
    public partial class Form4 : Form
    {
        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=product;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) //เช็คว่ามีผู้สมัครนี้แล้วหรือไม่
        {
            string[] txsplit =  textBox4.Text.Split('@') ;//ในtextbox4ถ้ามี@จะerror
          
            if (checkmember() == true)
            {
                MessageBox.Show("มีusernameนี้อยู่แล้ว", "โปรดใช้usernameใหม่", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox3.Focus();
            }
            else if (txsplit.Count() > 1)
            {
                MessageBox.Show("ไม่ต้องใส่@");
            }
            else if (textBox1.Text == "" || textBox2.Text == ""||textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "")//ถ้ามีช่องว่างจะแจ้ง
            {
                MessageBox.Show("กรุณากรอกช่องที่ว่าง", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox5.Text == textBox6.Text)//passwordตรงกันหรือไม่ ใช้ try catch
            {
                try//try catchดักจับerror
                {
                    MySqlConnection conn = databaseConnection();

                    String sql = "INSERT INTO member(fname,lname,username,email,password,status) VALUES('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text +comboBox1.Text+ "','" + textBox5.Text + "',\"user\")";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    conn.Close();
                    if (rows > 0)//insertสำเร็จให้ทุกช่องว่างเปล่า
                    {
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";
                        textBox5.Text = "";
                        textBox6.Text = "";
                        MessageBox.Show("สมัครสมาชิกสำเร็จ", "ยินดีต้อนรับ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form1 a = new Form1();
                        this.Hide();
                        a.Show();
                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else//ถ้าpasswordไม่ตรงกันจะแสดงelseนี้
            {
                MessageBox.Show("Passwordไม่สัมพันธ์กัน ลองใหม่อีกครั้ง", "ไม่สามารถสมัครสมาชิกได้", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox5.Text = "";
                textBox6.Text = "";
                textBox5.Focus();

              

            }




        }
        public Boolean checkmember()//เช็คuserในdata base ว่าซ้ำไหม
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=product";
            MySqlConnection conn = new MySqlConnection(connectionString);
            string username = textBox3.Text;
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM member WHERE username = @username", conn);

            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            
            if(table.Rows.Count > 0)//ถ้ามีจะเข้าifแรก
            {
                return true;
            }
            else//ถ้าไม่มีจะเข้าif2
            {
                return false;
            }

        }





        private void button2_Click(object sender, EventArgs e)//กลับไปหน้าlog in
        {
            Form1 a = new Form1();
            this.Hide();
            a.Show();
        }

        private void Form4_Load(object sender, EventArgs e)//เลือก@...com
        {
            comboBox1.SelectedIndex = 0;
        }

        
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (System.Text.Encoding.UTF8.GetByteCount(new char[] { e.KeyChar }) > 1)
            {
                e.Handled = true;
            }
        }

        private void textBox2_Keypress(object sender, KeyPressEventArgs e)
        {
            if (System.Text.Encoding.UTF8.GetByteCount(new char[] { e.KeyChar }) > 1)
            {
                e.Handled = true;
            }
        }
        
        private void textBox5_Keypress(object sender, KeyPressEventArgs e)
        {
            if (System.Text.Encoding.UTF8.GetByteCount(new char[] { e.KeyChar }) > 1)
            {
                e.Handled = true;
           
            }
        }
        private void textBox6_Keypress(object sender, KeyPressEventArgs e)
        {
            if (System.Text.Encoding.UTF8.GetByteCount(new char[] { e.KeyChar }) > 1)
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyPress_1(object sender, KeyPressEventArgs e)//กรอกได้แค่ตัวเลขพิมเล็กพิมใหญ่
        {
            if (System.Text.Encoding.UTF8.GetByteCount(new char[] { e.KeyChar }) > 1)
            {
                e.Handled = true;
            }
        }
    }

    }



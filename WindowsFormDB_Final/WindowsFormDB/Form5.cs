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
using System.IO;

namespace WindowsFormDB
{
    public partial class Form5 : Form
    {
        int listamount;
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            showlist();
           
        }
        private MySqlConnection databaseConnection() //เชื่อมดาต้าเบส
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=product;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        private void showlist() //แสดงสินค้าบนdatdGridview
        {
            MySqlConnection conn = databaseConnection();
            DataSet ds = new DataSet();
            conn.Open();

            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM list ";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);


            conn.Close();
            dataGridView1.DataSource = ds.Tables[0].DefaultView;



        }

        private void button1_Click(object sender, EventArgs e) //เพิ่มสินค้า
        {
            MySqlConnection conn = databaseConnection();
            String sql = "INSERT INTO things (name,amount,price,image) VALUES('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + Path.GetFileName(pictureBox2.ImageLocation) + "')";
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            conn.Open();

            int rows = cmd.ExecuteNonQuery();

            conn.Close();

            if (rows > 0)
            {
                MessageBox.Show("เพิ่มรายการสินค้าสำเร็จ");
                showlist();
            }



        }
        string x;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            {
              



            }


        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {



        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)//แสดงข้อความในtextboxและแสดงรูปภาพในpicturebox
        {

                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["name"].FormattedValue.ToString();
                textBox2.Text = "0";
                x = dataGridView1.Rows[e.RowIndex].Cells["price"].FormattedValue.ToString();
                textBox3.Text = Convert.ToString(int.Parse(textBox2.Text) * int.Parse(x));
                listamount = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["amount"].Value.ToString());
                dataGridView1.CurrentCell.Selected = true;
                int selectedRows = dataGridView1.CurrentCell.RowIndex;
                int image = Convert.ToInt32(dataGridView1.Rows[selectedRows].Cells["ID"].Value);
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["name"].FormattedValue.ToString();
               
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["price"].FormattedValue.ToString();
                string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=product;charset=utf8;";
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT image FROM list WHERE ID =\"{image}\"", conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MemoryStream ms = new MemoryStream((byte[])ds.Tables[0].Rows[0]["image"]);//เก็บรูปลงฐานข้อมูลในรูปแบบstream โดยใช้ memorystream
                    
                    pictureBox2.Image = new Bitmap(ms);

                }
          
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void addproduct_Click(object sender, EventArgs e)
        {
             
        }

        private void button2_Click(object sender, EventArgs e)//pick upสินค้า
        {
            MySqlConnection conn = databaseConnection();
            String sql = "INSERT INTO things (name,amount,price,image) VALUES('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','"+pictureBox2+ "')";
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            conn.Open();

            int rows = cmd.ExecuteNonQuery();

            conn.Close();

            if (rows > 0)
            {
                MessageBox.Show("ADD COMPLETE");
                showlist();
            }
           

        }

      
       
        private void button1_Click_1(object sender, EventArgs e)//clickแล้วไปที่หน้าตะกร้าสินค้า
        {
            
            Form7 a = new Form7();
            this.Hide();
            a.Show();
        }

        private void button5_Click(object sender, EventArgs e)//รวมราคาสินค้าเมื่อกด
        {
            textBox3.Text = Convert.ToString(int.Parse(textBox2.Text) * int.Parse(x));
        }

        private void button2_Click_1(object sender, EventArgs e)//กดหยิบสินค้าเมื่อต้องการ
        {
            if(textBox1.Text == ""|textBox2.Text == "" | textBox3.Text == "")//ถ้ามีช่องว่างจะแจ้ง
            {
                MessageBox.Show("กรอกข้อมูลให้ครบถ้วน"); 
            }
            else if (Convert.ToInt32(textBox2.Text) > listamount)//ถ้าซื้อสินค้าเกินจำนวนที่มีในstockจะแจ้ง
            {
                MessageBox.Show("ไม่สามารถซื้อในสต็อคได้");
            }
            else//ไม่เข้าเงื่อนไขไหนเลย
            {
                MySqlConnection conn5 = databaseConnection();
                String sql5 = "SELECT amount,price FROM history WHERE name= '"+textBox1.Text+ "'and status = \"unpay\" ";//
                MySqlCommand cmd5 = new MySqlCommand(sql5, conn5);

                conn5.Open();
                MySqlDataReader dr5 = cmd5.ExecuteReader();
                if (dr5.Read())//ถ้ามันอ่านได้แปลว่าข้อมูลเดียวกันแล้วชื่อสเตตีสเป้นยังไม่จ่ายunpay
                {
                    int oldamount = Convert.ToInt32(dr5.GetString(0));//เอาจำนวนจากhistoryมาใส่oldamount
                    int amount = int.Parse(textBox2.Text);//เอาจำนวนที่พิมพ์เก็บไว้amount
                    int newamount = oldamount + amount;//newamountมีค่าเท่ากับจำนวนในตะกร้ากับจำนวนที่ต้องการซื้อเพิ่ม
                    int price = Convert.ToInt32(dr5.GetString(1));//เอาราคาจากhistoryมาไว้ที่price
                    int newprice = int.Parse(textBox3.Text)+price;//เอาราคาเก่าจากตะกร้ามาบวกกับจำนวนที่เราที่เราซื้อเพิ่ม
                    MySqlConnection conn = databaseConnection();
                    String sql = "UPDATE history SET amount = '"+newamount+"',price = '"+newprice+"' WHERE name= '" + textBox1.Text + "'and status = \"unpay\" ";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        listamount -= amount;//listamountคือจำนวนในสต็อค มาลบกับจำนวนที่ซื้อเพิ่ม
                        MySqlConnection conn2 = databaseConnection();
                        String sql2 = "UPDATE list SET amount = '" + listamount + "' WHERE name= '" + textBox1.Text + "' ";
                        MySqlCommand cmd2 = new MySqlCommand(sql2, conn2);

                        conn2.Open();
                        int rows2 = cmd2.ExecuteNonQuery();
                        if (rows2 > 0)
                        {
                            MessageBox.Show("ADD COMPLETE");
                            showlist();//resetตาราง
                        }
                    }
                }
                else
                {
                    string date = DateTime.Now.ToShortDateString();//เอาวันที่มาเก็บไว้ในdate
                    MySqlConnection conn = databaseConnection();
                    int selectedRow = dataGridView1.CurrentCell.RowIndex;
                    int amo = Convert.ToInt32(dataGridView1.Rows[selectedRow].Cells["amount"].Value);
                    int xx = Convert.ToInt32(textBox2.Text);
                    String sql = "INSERT INTO history (username,name,amount,price,status,date) VALUES('" + Program.username + "','" + textBox1.Text + "','" + Convert.ToInt32(textBox2.Text) + "','" + Convert.ToInt32(textBox3.Text) + "',\"unpay\",'" + date + "')";

                    String sql2 = "UPDATE list SET amount = '" + amo + "'-'" + Convert.ToInt32(xx) + "',count = '" + textBox2.Text + "'WHERE name='" + textBox1.Text + "'";
                    //amoจำนวนในสต็อค-xxจำนวนที่ซื้อ

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
                    conn.Open();

                    int rows = cmd.ExecuteNonQuery();
                    int row2 = cmd2.ExecuteNonQuery();
                    conn.Close();

                    if (rows > 0)
                    {
                        MessageBox.Show("ADD COMPLETE");
                        showlist();
                    }
                }

            }

        }
       
    }


    }

  






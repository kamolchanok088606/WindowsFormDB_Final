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
    public partial class Form6 : Form
    {
        private List<forprint> allbook = new List<forprint>();
        bool nameserch;
        string picturepath;
        string selectedid;

        public Form6()
        {
            InitializeComponent();
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
            if(nameserch == true)
            {
                cmd.CommandText = "SELECT * FROM list WHERE name like '%"+textBox4.Text+"%'";
            }
            else
            {
                cmd.CommandText = "SELECT * FROM list ";
            }
            

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);//ดึงข้อมูลมาแสดงที่datagridview


            conn.Close();
            dataGridView1.DataSource = ds.Tables[0].DefaultView;



        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            showlist();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)//browseรูป
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                pictureBox1.ImageLocation = openFileDialog1.FileName;
                picturepath = openFileDialog1.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)//เพิ่มstorck
        {
            if (textBox1.Text == "" | textBox2.Text == "" | textBox3.Text == "")//ถ้าใส่ข้อมูลไม่ครบจะแจ้ง
            {
                MessageBox.Show("กรอกข้อมูลให้ครบ");
            }
            else//เพิ่มสินค้าใหม่
            {
                MySqlConnection conn = databaseConnection();
                MySqlCommand cmd = conn.CreateCommand();
                conn.Open();
                byte[] bytes = File.ReadAllBytes(picturepath);
                cmd.CommandText = "INSERT INTO list (name,amount,price,image,picturepath)VALUE(@name,@amount,@price,@image,@picturepath)";//insertเข้าลิสต์
                cmd.Parameters.AddWithValue("@name", textBox1.Text);
                cmd.Parameters.AddWithValue("@amount", textBox2.Text);
                cmd.Parameters.AddWithValue("@price", textBox3.Text);
                cmd.Parameters.AddWithValue("@image", bytes);
                cmd.Parameters.AddWithValue("@picturepath", picturepath);
                int row = cmd.ExecuteNonQuery();
                if (row > 0)
                {
                    MessageBox.Show("เพิ่มสินค้าเรียบร้อย");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    showlist();
                }



            }
        }

        private void button2_Click(object sender, EventArgs e)//แก้ไขข้อมูลstock
        {
            if (textBox1.Text == "" | textBox2.Text == "" | textBox3.Text == "")
            {
                MessageBox.Show("กรอกข้อมูลให้ครบ");
            }
            else
            {
                MySqlConnection conn = databaseConnection();
                MySqlCommand cmd = conn.CreateCommand();
                conn.Open();
                byte[] bytes = File.ReadAllBytes(picturepath);
                cmd.CommandText = "UPDATE list SET name=@name,amount=@amount,price=@price,image=@image,picturepath = @picturepath WHERE id = '" + selectedid + "'";
                cmd.Parameters.AddWithValue("@name", textBox1.Text);
                cmd.Parameters.AddWithValue("@amount", textBox2.Text);
                cmd.Parameters.AddWithValue("@price", textBox3.Text);
                cmd.Parameters.AddWithValue("@image", bytes);
                cmd.Parameters.AddWithValue("@picturepath", picturepath);
                int row = cmd.ExecuteNonQuery();
                if (row > 0)
                {
                    MessageBox.Show("แก้ไขสินค้าเรียบร้อย");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    showlist();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)//ลบstock
        {
            if (textBox1.Text == "" | textBox2.Text == "" | textBox3.Text == "")//ถ้ามีช่องที่ว่างจะแจ้ง
            {
                MessageBox.Show("กรอกข้อมูลให้ครบ");
            }
            else
            {
                MySqlConnection conn = databaseConnection();
                MySqlCommand cmd = conn.CreateCommand();
                conn.Open();

                cmd.CommandText = "DELETE FROM list WHERE id = '" + selectedid + "'";

                int row = cmd.ExecuteNonQuery();
                if (row > 0)
                {
                    MessageBox.Show("ลบสินค้าเรียบร้อย");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    showlist();
                }
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)//clickที่cellแล้วแสดงที่textboxและpicturebox
        {
            MySqlConnection conn = databaseConnection();
            MySqlCommand cmd = conn.CreateCommand();
            conn.Open();
            dataGridView1.CurrentRow.Selected = true;
            int selectedRow = dataGridView1.CurrentCell.RowIndex;
            selectedid = dataGridView1.Rows[selectedRow].Cells["id"].Value.ToString();

            textBox1.Text = dataGridView1.Rows[selectedRow].Cells["name"].Value.ToString();
            textBox2.Text = dataGridView1.Rows[selectedRow].Cells["amount"].Value.ToString();
            textBox3.Text = dataGridView1.Rows[selectedRow].Cells["price"].Value.ToString();
            cmd.CommandText = "SELECT picturepath FROM list WHERE id = '" + selectedid + "'";
            MySqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                picturepath = dr.GetString(0);
                pictureBox1.ImageLocation = picturepath;
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)//คลิกแล้วแสดงหน้าformhistory
        {
            history a = new history();
            this.Hide();
            a.Show();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)//textboxเสิร์ช
        {
            if (textBox4.Text != "")
            {
                nameserch = true;
                showlist();
            }
            else 
            {
                nameserch = false;
                showlist();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)//ปริ้นstock
        {
            Image logo = Image.FromFile(@"C:\product\logo450.jpg");
            e.Graphics.DrawImage(logo, new PointF(330, 5));
            e.Graphics.DrawString("         C  O  S  M  E  T  I  C", new Font("TH SarabunPSK", 40, FontStyle.Bold), Brushes.Orange, new PointF(120, 140));
            e.Graphics.DrawString("-----------------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(50, 180));
            e.Graphics.DrawString("TEL : 0886063254 ADDRESS : KANGSADAL ", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(70, 215));
            e.Graphics.DrawString("TIME " + System.DateTime.Now.ToString("dd / MM / yyyy   HH : mm : ss น."), new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(70, 235));
            e.Graphics.DrawString("-----------------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(50, 255));
            e.Graphics.DrawString("NAME", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(100, 270));
            e.Graphics.DrawString("AMOUNT", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(400, 270));
            e.Graphics.DrawString("PRICE", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(600, 270));
            e.Graphics.DrawString("-----------------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(50, 285));

            int y = 320;
            loaddata();
            foreach (var i in allbook)
            {
                e.Graphics.DrawString(i.name, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(100, y));
                e.Graphics.DrawString(i.amount, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(400, y));
                e.Graphics.DrawString(i.price, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(600, y));
                y = y + 20;
            }
            e.Graphics.DrawString("-----------------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(50, y));
        }
        private void loaddata()//เอาค่าจากดาต้าเบสมาเก็บไว้forprint
        {
            allbook.Clear();
            MySqlConnection conn = new MySqlConnection("host=127.0.0.1;username=root;password=;database=product;");

            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            if (nameserch == true)
            {
                cmd.CommandText = "SELECT * FROM list WHERE name like '%" + textBox4.Text + "%'";
            }
            else
            {
                cmd.CommandText = "SELECT * FROM list ";
            }
            MySqlDataReader adapter = cmd.ExecuteReader();

            while (adapter.Read())
            {
                Program.name = adapter.GetString("name");
                Program.amount = adapter.GetString("amount");
                Program.price = adapter.GetString("price");
               
                forprint item = new forprint()
                {
                    name = Program.name,
                    amount = Program.amount,
                    price = Program.price,
                    


                };
                allbook.Add(item);

            }

        }

        private void button6_Click(object sender, EventArgs e)//printstock
        {
            allbook.Clear();
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }
    }
}


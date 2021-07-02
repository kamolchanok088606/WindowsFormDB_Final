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
    public partial class Form7 : Form
    {
        int listamount;
        int listprice;
        int amount;
        string name;
        private List<forprint> allbook = new List<forprint>();
        public Form7()
        {
            InitializeComponent();
        }
        private MySqlConnection databaseConnection() //เชื่อมดาต้าเบส
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=product;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void showlist(string status) //แสดงสินค้าบนdatdGridview
        {
            MySqlConnection conn = databaseConnection();
            DataSet ds = new DataSet();
            conn.Open();

            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM history WHERE username = '" + Program.username + "'and status = '" + status + "' ";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);


            conn.Close();
            dataGridView1.DataSource = ds.Tables[0].DefaultView;



        }

        private void Form7_Load(object sender, EventArgs e)
        {

            showlist("unpay");
            total("unpay");

        }


        private void total(string status)//รวมเงิน
        {
            int sum = 0;
            MySqlConnection conn = databaseConnection();

            conn.Open();

            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM history WHERE username = '"+Program.username+"'and status = '"+status+"' ";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            MySqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                int y = read.GetInt32("price");//เป็นการวนดึงค่าจากดาต้าเบส ในคอลัมน์price มาเก็บในตัวแปร y ทีละตัว จนครบทุกตัว
                sum = sum + y;
            }
            textBox1.Text = Convert.ToString(sum);


            conn.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)//ชำระเงิน
        {
            if (Convert.ToInt32(textBox2.Text) < Convert.ToInt32(textBox1.Text))
            {
                MessageBox.Show("NOT ENOUGE!!");

            }
            else
            {
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox2.Text) - Convert.ToInt32(textBox1.Text));
                allbook.Clear();
                printPreviewDialog1.Document = printDocument1;
                printPreviewDialog1.ShowDialog();
                
                MySqlConnection conn = databaseConnection();
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE history SET status =\"pay\"WHERE username = '" + Program.username + "'and status = \"unpay\"";
                cmd.ExecuteNonQuery();
            }
            recount2();
            showlist("unpay");
            total("unpay");

            

        }

        private void button2_Click(object sender, EventArgs e)//ลบสินค้าจากตะกร้าเมื่อลบแล้วจำนวนสินค้าจะคืนกลับstock
        {
            int newamount = 0;
            MySqlConnection conn = databaseConnection();
            conn.Open();
            MySqlConnection conn2 = databaseConnection();
            conn2.Open();
            MySqlConnection conn3 = databaseConnection();
            conn3.Open();

            MySqlCommand cmd = conn.CreateCommand();
            MySqlCommand cmd2 = conn2.CreateCommand();
            MySqlCommand cmd3 = conn3.CreateCommand();
            cmd.CommandText = "SELECT amount FROM list WHERE name = '" + name + "' ";
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                newamount += Convert.ToInt32(dr.GetValue(0).ToString());
                newamount += amount;
                cmd2.CommandText = "UPDATE list SET amount = '" + newamount + "' WHERE name = '" + name + "'";
                int row = cmd2.ExecuteNonQuery();
                if (row > 0)
                {
                    cmd3.CommandText = "DELETE FROM history WHERE name = '"+name+"'";
                    cmd3.ExecuteNonQuery();
                    MessageBox.Show("ลบเสร็จสิ้น");
                }

            }
            showlist("unpay");
            total("unpay");

        }

        private void button3_Click(object sender, EventArgs e)//printใบเสร็จ
        {
           
        }



        private void loaddata()//เอาค่าจากดาต้าเบสมาเก็บไว้forprint
        {
            allbook.Clear();
            MySqlConnection conn = new MySqlConnection("host=127.0.0.1;username=root;password=;database=product;");

            conn.Open();

            MySqlCommand cmd = new MySqlCommand($"SELECT name,amount,price,date FROM history WHERE username = '"+Program.username+"'and status = \"unpay\" ", conn);
            MySqlDataReader adapter = cmd.ExecuteReader();

            while (adapter.Read())
            {
                Program.name = adapter.GetString("name");
                Program.amount = adapter.GetString("amount");
                Program.price = adapter.GetString("price");
                Program.date = adapter.GetString("date");
                forprint item = new forprint()
                {
                    name = Program.name,
                    amount = Program.amount,
                    price = Program.price,
                    date = Program.date,


                };
                allbook.Add(item);

            }

        }

        private void printDocument1_PrintPage_1(object sender, System.Drawing.Printing.PrintPageEventArgs e)//สร้างหน้าใบเสร็จ
        {
            Image logo = Image.FromFile(@"C:\product\logo450.jpg");
            e.Graphics.DrawImage(logo, new PointF(330, 5));
            e.Graphics.DrawString("         C  O  S  M  E  T  I  C", new Font("TH SarabunPSK", 40, FontStyle.Bold), Brushes.Orange, new PointF(120, 140));
            e.Graphics.DrawString("-----------------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(50, 180));
            e.Graphics.DrawString("TEL : 0886063254 ADDRESS : KANGSADAL ", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(70, 215));
            e.Graphics.DrawString("TIME " + System.DateTime.Now.ToString("dd / MM / yyyy   HH : mm : ss น."), new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(70, 235));
            e.Graphics.DrawString("-----------------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(50, 255));
            e.Graphics.DrawString("NAME", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(100, 270));
            e.Graphics.DrawString("AMOUNT", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(300, 270));
            e.Graphics.DrawString("PRICE", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(550, 270));
            e.Graphics.DrawString("DATE", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(600, 270));
            e.Graphics.DrawString("-----------------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(50, 285));
           

            int y = 320;
            loaddata();
            foreach (var i in allbook)
            {
                e.Graphics.DrawString(i.name, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(90, y));
                e.Graphics.DrawString(i.amount, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(330, y));
                e.Graphics.DrawString(i.price, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(550, y));
                e.Graphics.DrawString(i.date, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(600, y));
                y = y + 20;
            }
            e.Graphics.DrawString("-----------------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(50, y));
            e.Graphics.DrawString("ราคารวม" + textBox1.Text, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(540, y+50));
            e.Graphics.DrawString("รับเงิน" + textBox2.Text, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(540, y+80));
            e.Graphics.DrawString("เงินทอน" + textBox3.Text, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(540, y+110));
        }
        private void refcount()//คืนจำนวนสินค้าที่ยกเลิกกลับคืนstock
        {
            int selectedRow = dataGridView1.CurrentCell.RowIndex;
            string name = Convert.ToString(dataGridView1.Rows[selectedRow].Cells["name"].Value);

            MySqlConnection conn = databaseConnection();
            String sql = "UPDATE list SET amount = amount+count WHERE name = '" + name + "' ";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();

            int rows = cmd.ExecuteNonQuery();

            conn.Close();
        }
        private void recount2()//คืนจำนวนสินค้าที่ยกเลิกกลับคืนstock
        {

            MySqlConnection conn = databaseConnection();
            String sql = "UPDATE list SET count = '" + Convert.ToInt32("0") + "' ";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();

            int rows = cmd.ExecuteNonQuery();

            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)//Backกลับไปformซื้อสินค้า
        {

            Form5 a = new Form5();
            this.Hide();
            a.Show();
        }
        

        

        private void button6_Click(object sender, EventArgs e)//ดูประวัติการซื้อ
        {
            if (button6.Text== "HISTORY")
            {
                button6.Text = "MY CART";
                showlist("pay");
            }
            else
            {
                button6.Text = "HISTORY";
                showlist("unpay");
                   
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)//clickที่datagridviewแล้วแก้ไขจำนวนที่combobox
        {
            textBox4.Clear();
            dataGridView1.CurrentRow.Selected = true;
            int selectedRow = dataGridView1.CurrentCell.RowIndex;
            amount = Convert.ToInt32(dataGridView1.Rows[selectedRow].Cells["amount"].Value.ToString());
            name = dataGridView1.Rows[selectedRow].Cells["name"].Value.ToString();
            textBox4.Text = dataGridView1.Rows[selectedRow].Cells["amount"].Value.ToString();
           
            MySqlConnection conn = databaseConnection();
            String sql = "SELECT amount,price FROM list WHERE name = '"+name+"'";//select จำนวน,ราคา มาจาก table stock โดยค้นหาจากชื่อสินค้า
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                listamount = Convert.ToInt32(dr.GetString(0));//จำนวนที่เหลืออยู่ในstock
                listprice = Convert.ToInt32(dr.GetString(1));//ราคาสินค้า
            }
        }
        


        private void button5_Click(object sender, EventArgs e)//ปุ่มEditแก้ไขจำนวนการซื้อ
        {
            int allamount = amount + listamount;//จำนวนสินค้าในตะกร้า+จำนวนสินค้าในสต็อค
            int newamount = int.Parse(textBox4.Text);//จำนวนสินค้าในตะกร้าใหม่
            if (newamount <= allamount)//ถ้าจำนวนสินค้าใหม่น้อยกว่าหรือเท่ากับจำนวนรวมสินค้าทั้งหมด
            {
                int newlist = allamount - newamount;//เอาจำนวนรวมสินค้าทั้งหมด-จำนวนสินค้าใหม่ในสินค้าในตะกร้า
                int newprice = listprice * newamount;//เอาราคาสินค้า*จำนวนสินค้าใหม่ในตะกร้า
                MySqlConnection conn = databaseConnection();
                String sql = "UPDATE history SET amount = '"+newamount+"',price = '"+newprice+"'WHERE name = '" + name + "'and status =\"unpay\"";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();//
                if (rows > 0)//ถ้าrows>0เข้าเงื่อนไขสำเร็จ
                {
                    MySqlConnection conn2 = databaseConnection();
                    String sql2 = "UPDATE list SET amount = '" + newlist + "' WHERE name = '" + name + "'";
                    MySqlCommand cmd2 = new MySqlCommand(sql2, conn2);
                    conn2.Open();
                    int rows2 = cmd2.ExecuteNonQuery();
                    if (rows2 > 0)
                    {
                        MessageBox.Show("แก้ไขจำนวนสินค้าเสร็จสิ้น");
                    }
                }
            }
            else//จำนวนสินค้าที่คีย์ใหม่>จำนวนสินค้าทั้งหมด
            {
                MessageBox.Show("จำนวนสินค้าไม่เพียงพอ");
            }
            showlist("unpay");
            total("unpay");
              

          
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        
        
        

    }
}


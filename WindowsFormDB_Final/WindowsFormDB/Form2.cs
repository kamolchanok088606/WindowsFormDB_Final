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
    public partial class history : Form
    {
        string total;
        private List<forprint> allbook = new List<forprint>();
        bool nameserch, dateserch;
        public history()
        {
            InitializeComponent();
        }
        private MySqlConnection databaseConnection() //เชื่อมดาต้าเบส
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=product;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        private void history_Load(object sender, EventArgs e)
        {
            
           
            string now = DateTime.Now.ToShortDateString();//ขึ้นวันเวลาเป็นปัจจุบัน
            dateTimePicker1.Value = Convert.ToDateTime(now);
            dateserch = false;
            showhistory();

        }
        private void showhistory() //แสดงสินค้าบนdatdGridview เอาข้อมูลจากตารางhistoryมาแสดงที่datagridview
        {
            MySqlConnection conn = databaseConnection();
            DataSet ds = new DataSet();
            conn.Open();

            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            if (nameserch == true)
            {
                cmd.CommandText = "SELECT * FROM history WHERE username like '%" + textBox1.Text + "%'";
            }
            else if (dateserch == true)
            {
                cmd.CommandText = "SELECT * FROM history WHERE date like '%" + dateTimePicker1.Value.ToShortDateString() + "%'";
            }
            else
            {
                cmd.CommandText = "SELECT * FROM history ";
            }
            

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);


            conn.Close();
            dataGridView1.DataSource = ds.Tables[0].DefaultView;



        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)//เลือกวันที่ที่ต้องการดูประวัติ
        {
            nameserch = false;
            dateserch = true;
            showhistory();
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)//searchดูชื่อประวัติผู้ซื้อ
        {
            dateserch = false;
            if (textBox1.Text != "")
            {
                nameserch = true;
                showhistory();
            }
            else
            {
                nameserch = false;
                showhistory();
            }
          

        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)//สร้างหน้าใบเสร็จ
        {
            Image logo = Image.FromFile(@"C:\product\logo450.jpg");
            e.Graphics.DrawImage(logo, new PointF(330, 5));
            e.Graphics.DrawString("         C  O  S  M  E  T  I  C", new Font("TH SarabunPSK", 40, FontStyle.Bold), Brushes.Orange, new PointF(120, 140));
            e.Graphics.DrawString("-----------------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(50, 180));
            e.Graphics.DrawString("TEL : 0886063254 ADDRESS : KANGSADAL ", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(70, 215));
            e.Graphics.DrawString("TIME " + System.DateTime.Now.ToString("dd / MM / yyyy   HH : mm : ss น."), new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(70, 235));
            e.Graphics.DrawString("-----------------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(50, 255));
            e.Graphics.DrawString("USERNAME", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(100, 270)); 
            e.Graphics.DrawString("NAME", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(200, 270));
            e.Graphics.DrawString("AMOUNT", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(350, 270));
            e.Graphics.DrawString("PRICE", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(440, 270));
            e.Graphics.DrawString("STATUS", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(520, 270));
            e.Graphics.DrawString("DATE", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(600, 270));
            e.Graphics.DrawString("-----------------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(50, 285));

            int y = 320;
            loaddata();
            foreach (var i in allbook)
            {
                e.Graphics.DrawString(i.username, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(100, y));
                e.Graphics.DrawString(i.name, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(200, y));
                e.Graphics.DrawString(i.amount, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(350, y));
                e.Graphics.DrawString(i.price, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(440, y));
                e.Graphics.DrawString(i.status, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(520, y));
                e.Graphics.DrawString(i.date, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(600, y));
                y = y + 20;
            }
            e.Graphics.DrawString("-----------------------------------------------------------------------------------------------------------------------------------------------", new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(50, y));
            e.Graphics.DrawString("ราคารวม" + total, new Font("TH SarabunPSK", 16, FontStyle.Bold), Brushes.Black, new PointF(540, y+100));

        }

        private void button1_Click(object sender, EventArgs e)//ปริ้นใบเสร็จ
        {
            allbook.Clear();
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void loaddata()//เอาค่าจากดาต้าเบสมาเก็บไว้forprint
        {
            allbook.Clear();
            MySqlConnection conn = new MySqlConnection("host=127.0.0.1;username=root;password=;database=product;");

            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            MySqlConnection conn2 = new MySqlConnection("host=127.0.0.1;username=root;password=;database=product;");
            MySqlCommand cmd2 = conn2.CreateCommand();
            conn2.Open();
            if (nameserch == true)
            {
                cmd.CommandText = "SELECT * FROM history WHERE username like '%" + textBox1.Text + "%'";
                cmd2.CommandText = "SELECT SUM(price) FROM history WHERE username like '%" + textBox1.Text + "%'";

            }
            else if (dateserch == true)
            {
                cmd.CommandText = "SELECT * FROM history WHERE date like '%" + dateTimePicker1.Value.ToShortDateString() + "%'";
                cmd2.CommandText = "SELECT SUM(price) FROM history WHERE date like '%" + dateTimePicker1.Value.ToShortDateString() + "%'";
            }
            else
            {
                cmd.CommandText = "SELECT * FROM history ";
                cmd2.CommandText = "SELECT SUM(price) FROM history ";
            }

            MySqlDataReader adapter = cmd.ExecuteReader();
            object price = cmd2.ExecuteScalar();
            total = Convert.ToString(price);
            while (adapter.Read())
            {
                Program.name = adapter.GetString("name");
                Program.amount = adapter.GetString("amount");
                Program.price = adapter.GetString("price");
                Program.date = adapter.GetString("date");
                Program.username = adapter.GetString("username");
                Program.status = adapter.GetString("status");
                forprint item = new forprint()
                {
                    name = Program.name,
                    amount = Program.amount,
                    price = Program.price,
                    date = Program.date,
                    username = Program.username,
                    status = Program.status,

                };
                allbook.Add(item);

            }

        }

    }
}

using System.Data;
using System.Data.SQLite;


namespace vopbot
{
    public partial class form_vop_bot_main : Form
    {
        public form_vop_bot_main()
        {
            InitializeComponent();
        }

        private void form_vop_bot_main_Load(object sender, EventArgs e)
        {
            // dataGridView1.AutoGenerateColumns = false;
            string cs = @"URI=file:test.db";
            using var con = new SQLiteConnection(cs);
            con.Open();
            try
            {
                string stm = "SELECT * FROM cars LIMIT 5";
                using var selectcmd = new SQLiteCommand(stm, con);
                using SQLiteDataReader rdr = selectcmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine($"{rdr.GetInt32(0)} {rdr.GetString(1)} {rdr.GetInt32(2)}");
                }
            }
            catch { 
                MessageBox.Show("Table not initilized"); 
            } 
            finally { 
                con.Close(); 
            }
            
            var table = new DataTable();
            table.Columns.Add("VM-Name", typeof(string));
            table.Columns.Add("Tenant", typeof(string));

            table.Rows.Add("Hi", "100");
            table.Rows.Add("Ki", "30");

            var column = new DataGridViewComboBoxColumn();
            column.HeaderText = "Acknowledge";
            column.DataPropertyName = "Acknowledge";
            column.DataSource = new List<string>() { "yes", "no", "unknown" };
            dataGridView1.DataSource = table;
            dataGridView1.Columns.Add(column);

            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Any questions - send mail");
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void operationsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void installToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cs = @"URI=file:test.db";
            using var con = new SQLiteConnection(cs);
            con.Open();
            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = @"CREATE TABLE cars(id INTEGER PRIMARY KEY,
            name TEXT, price INT)";
            cmd.ExecuteNonQuery();
        }
    }
}

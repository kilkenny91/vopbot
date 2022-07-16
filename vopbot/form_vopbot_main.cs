using System.Data;
using System.Data.SQLite;
using System.Diagnostics;

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
                string stm = "SELECT * FROM vmBackupTagsAck LIMIT 5";
                using var selectcmd = new SQLiteCommand(stm, con);

                using (var reader = selectcmd.ExecuteReader())
                {
                    string test = "";

                    // do we have any data to read?
                    //DONE: try not building string but using formatting (or string interpolation)
                    var table = new DataTable();
                    table.Columns.Add("VM-Name", typeof(string));
                    table.Columns.Add("Tenant", typeof(string));
                    table.Columns.Add("Acknowledged", typeof(string));
                    while (reader.Read())
                    {
                        table.Rows.Add(reader["vmname"], reader["tenant"], reader["ack"]);
                    }
                    var column = new DataGridViewComboBoxColumn();
                    column.HeaderText = "Acknowledge";
                    column.DataPropertyName = "Acknowledge";
                    column.DataSource = new List<string>() { "yes", "no", "unknown" };
                    dataGridView1.DataSource = table;
                    dataGridView1.Columns.Add(column);
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        dataGridView1.Rows[i].Cells[3].Value = dataGridView1.Rows[i].Cells[2].Value;
                    }
                    dataGridView1.Columns[0].ReadOnly = true;
                    dataGridView1.Columns[1].ReadOnly = true;
                }
            }
            catch {
                MessageBox.Show("Table not initilized");
            }
            finally {

                con.Close();
            }
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

            cmd.CommandText = @"CREATE TABLE vmBackupTagsAck(id INTEGER PRIMARY KEY,
            vmname TEXT, tenant TEXT, ack TEXT)";
            cmd.ExecuteNonQuery();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            label1.Text = "unsaved rows";
        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void form_vop_bot_main_Shown(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cs = @"URI=file:test.db";
            using var con = new SQLiteConnection(cs);
            con.Open();
            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = @"DROP TABLE vmBackupTagsAck";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE vmBackupTagsAck(id INTEGER PRIMARY KEY,
            vmname TEXT, tenant TEXT, ack TEXT)";
            cmd.ExecuteNonQuery();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                try
                {

                    cmd.CommandText = @"INSERT INTO vmBackupTagsAck(vmname,tenant,ack) values ('" +
                    dataGridView1.Rows[i].Cells[0].Value.ToString() + "','" +
                    dataGridView1.Rows[i].Cells[1].Value.ToString() + "','" +
                    dataGridView1.Rows[i].Cells[3].Value.ToString() + "')";
                    cmd.ExecuteNonQuery();

                }
                catch { }
                finally {
                    label1.Text = "Rows saved";
                    reload();
                }
            }
        }
        private void reload()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();

            string cs = @"URI=file:test.db";
            using var con = new SQLiteConnection(cs);
            con.Open();
            try
            {
                string stm = "SELECT * FROM vmBackupTagsAck LIMIT 5";
                using var selectcmd = new SQLiteCommand(stm, con);

                using (var reader = selectcmd.ExecuteReader())
                {
                    string test = "";

                    // do we have any data to read?
                    //DONE: try not building string but using formatting (or string interpolation)
                    var table = new DataTable();
                    table.Columns.Add("VM-Name", typeof(string));
                    table.Columns.Add("Tenant", typeof(string));
                    table.Columns.Add("Acknowledged", typeof(string));
                    while (reader.Read())
                    {
                        table.Rows.Add(reader["vmname"], reader["tenant"], reader["ack"]);
                    }
                    var column = new DataGridViewComboBoxColumn();
                    column.HeaderText = "Acknowledge";
                    column.DataPropertyName = "Acknowledge";
                    column.DataSource = new List<string>() { "yes", "no", "unknown" };
                    dataGridView1.DataSource = table;
                    dataGridView1.Columns.Add(column);
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        dataGridView1.Rows[i].Cells[3].Value = dataGridView1.Rows[i].Cells[2].Value;
                    }
                    dataGridView1.Columns[0].ReadOnly = true;
                    dataGridView1.Columns[1].ReadOnly = true;
                }
            }
            catch
            {
                MessageBox.Show("Table not initilized");
            }
            finally
            {

                con.Close();
            }
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reload();
        }

        private void openDBLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenExplorer(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
        }
        private static void OpenExplorer(string path)
        {
            if (Directory.Exists(path))
                Process.Start("explorer.exe", path);
        }
    }
}

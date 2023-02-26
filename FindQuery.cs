using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Threading;

namespace Tracker
{
    public partial class FindQuery : Form
    {
        private List<int> indexToRefresh = new List<int>();
        private ListViewItem li;
        private int X;
        private int Y;
        private int index;
        private static Mutex mutex = new Mutex();

        private readonly string connectionString = "datasource=192.168.1.10;port=3307;username=root;password=bomar771;database=bomartrans";

        public FindQuery()
        {
            InitializeComponent();

            listView1.View = View.Details;
            listView1.LabelEdit = false;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            listView1.Columns.Add("PublicationID", -1, HorizontalAlignment.Left);
            listView1.Columns.Add("Claim Address", -1, HorizontalAlignment.Left);
            listView1.Columns.Add("Drop Address", -1, HorizontalAlignment.Left);
            listView1.Columns.Add("Pallet For Exchange", -1, HorizontalAlignment.Left);
            listView1.Columns.Add("Exchanged Pallets", -1, HorizontalAlignment.Left);
            listView1.Columns.Add("Saldo", -1, HorizontalAlignment.Left);
            listView1.Columns.Add("Truck", -1, HorizontalAlignment.Left);
            listView1.Columns.Add("Company", -1, HorizontalAlignment.Left);

            var newWidth = listView1.Width / listView1.Columns.Count;
            foreach (ColumnHeader column in listView1.Columns)
            {
                column.Width = newWidth;
            }
        }

        private void CalculateSaldo(ref ListViewItem item)
        {
            item.SubItems[5].Text = Convert.ToString(Convert.ToInt32(item.SubItems[3].Text) - Convert.ToInt32(item.SubItems[4].Text));
            if (item.SubItems[5].Text.Contains("-"))
            {
                throw new Exception("Saldo can't be negative !");
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            if (string.IsNullOrEmpty(tbCompany.Text) && string.IsNullOrEmpty(tbNumber.Text))
            {
                return;
            }

            string query = "SELECT * FROM publications WHERE ";
            Boolean flag = false;

            if (!string.IsNullOrEmpty(tbCompany.Text))
            {
                query += $"Company LIKE '{tbCompany.Text}%'";
                flag = true;
            }
            if (!string.IsNullOrEmpty(tbNumber.Text))
            {
                if (flag)
                {
                    query += " AND ";
                }
                query += $"PublicationID LIKE '{tbNumber.Text}%';";
            }

            
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Example to save in the listView1 :
                        string[] row = { reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetString(7) };
                        var listViewItem = new ListViewItem(row);
                        listView1.Items.Add(listViewItem);
                    }
                }
                else
                {
                    MessageBox.Show("No such entries!");
                }

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                index = listView1.Items.IndexOf(listView1.SelectedItems[0]);
            }
            li = listView1.GetItemAt(e.X, e.Y);
            X = e.X;
            Y = e.Y;
        }

        private static void RevertChanges(ref ListViewItem item, ListViewItem copy)
        {
            item.SubItems[3].Text = copy.SubItems[3].Text;
            item.SubItems[4].Text = copy.SubItems[4].Text;
            item.SubItems[5].Text = copy.SubItems[5].Text;
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            int nStart = X;
            int spos = 0;
            int epos = listView1.Columns[1].Width;
            int subItemSelected = 0;

            for (int i = 0; i < listView1.Columns.Count; i++)
            {
                if (nStart > spos && nStart < epos)
                {
                    subItemSelected = i;
                    break;
                }

                spos = epos;
                epos += listView1.Columns[i].Width;
            }

            if(li == null)
            {
                return; // invalid item
            }
            if (spos == 0)
            {
                tbNumber.Text = li.SubItems[subItemSelected].Text;
                return; // cant change ID but can be copied for easy deletion
            }
            if(spos == listView1.Columns[0].Width * 5)
            {
                return; // cant change Saldo
            }

            ListViewItem item = (ListViewItem)li.Clone();

            string input = Interaction.InputBox(li.SubItems[subItemSelected].Text, listView1.Columns[subItemSelected].Text);
            if(input != "")
            {
                li.SubItems[subItemSelected].Text = input;
                try
                {
                    CalculateSaldo(ref li);
                }
                catch(Exception)
                {
                    RevertChanges(ref li, item);
                    return;
                }
                indexToRefresh.Add(listView1.SelectedIndices[0]);
            }
        }

        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {
            switch (keys)
            {
                case Keys.S | Keys.Control:
                    button2_Click(null, null);
                    break;
            }

            // run base implementation
            return base.ProcessCmdKey(ref message, keys);
        }

        private void SaveDataToDB(ref MySqlDataReader reader, ref MySqlCommand commandDatabase)
        {
            reader = commandDatabase.ExecuteReader();
        }

        
        private void button2_Click(object sender, EventArgs e) //save
        {
            if (indexToRefresh.Count == 0)
            {
                MessageBox.Show("No changes made!");
                return;
            }

            string query;
            progressBar1.Maximum = indexToRefresh.Count;
            progressBar1.Step = 1;

            for (int i = 0; i < indexToRefresh.Count; i++)
            {
                li = listView1.Items[indexToRefresh[i]];
                query = $"UPDATE publications SET ClaimAddress = '{li.SubItems[1].Text}', DropAddress = '{li.SubItems[2].Text}'," +
                    $"PalletForExchange = '{li.SubItems[3].Text}', ExchangedPallet = '{li.SubItems[4].Text}'," +
                    $"Saldo = '{li.SubItems[5].Text}', Truck = '{li.SubItems[6].Text}', Company = '{li.SubItems[7].Text}'" +
                    $"WHERE PublicationID = '{li.SubItems[0].Text}'";


                try
                {
                    MySql.Data.MySqlClient.MySqlConnection conn;

                    conn = new MySql.Data.MySqlClient.MySqlConnection();
                    conn.ConnectionString = connectionString;

                    MySqlCommand commandDatabase = new MySqlCommand(query, conn);
                    commandDatabase.CommandTimeout = 50;

                    conn.Open();
                    MySqlDataReader myReader = null;

                    Thread myNewThread = new Thread(() => SaveDataToDB(ref myReader, ref commandDatabase));
                    myNewThread.Start();

                    progressBar1.PerformStep();
                    myNewThread.Join();
                    conn.Close();

                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            indexToRefresh.Clear();

            MessageBox.Show("Updated successfully");
            progressBar1.Value = 0;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbCompany.Text = "";
            tbNumber.Text = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(tbNumber.Text))
            {
                MessageBox.Show("Insert Publication ID");
                return;
            }

            DialogResult dr = MessageBox.Show($"Are you sure you want to delete Publication {tbNumber.Text}?",
                      "!", MessageBoxButtons.YesNo);

            if(dr == DialogResult.Yes)
            {
                try
                {
                    MySql.Data.MySqlClient.MySqlConnection conn;

                    string query = $"DELETE FROM publications WHERE PublicationID = '{tbNumber.Text}'";

                    conn = new MySql.Data.MySqlClient.MySqlConnection();
                    conn.ConnectionString = connectionString;

                    MySqlCommand commandDatabase = new MySqlCommand(query, conn);
                    commandDatabase.CommandTimeout = 50;

                    conn.Open();
                    MySqlDataReader myReader = commandDatabase.ExecuteReader();

                    MessageBox.Show("Successfully deleted");

                    conn.Close();

                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}

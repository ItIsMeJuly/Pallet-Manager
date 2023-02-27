using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Tracker
{
    public partial class NewQuerry : Form
    {
        
        public NewQuerry()
        {
            InitializeComponent();
        }

        private void Hide()
        {
            lblQuery.Visible = false;
            lblClaim.Visible = false;
            lblDrop.Visible = false;
            lblCompany.Visible = false;
            lblPallet.Visible = false;
            lblTruck.Visible = false;
            lblForExchange.Visible = false;
        }


        private void CheckEmptyAndFormat(ref Boolean valid)
        {
            var textboxes = this.Controls.OfType<TextBox>()
                          .Where(c => c.Name.StartsWith("tb"))
                          .ToList();
            if(!new Regex("[A-Z]{1,2}[0-9]{4}[A-Z]{2}$").IsMatch(tbTruck.Text))
            {
                lblTruck.Visible = true;
                valid = false;
            }
            if(tbCompany.Text == "")
            {
                lblCompany.Visible = true;
                valid = false;
            }
            if(!new Regex("[A-Z]{1,2}[0-9]+$").IsMatch(tbFrom.Text))
            {
                lblClaim.Visible = true;
                valid = false;
            }
            if (!new Regex("[A-Z]{1,2}[0-9]+$").IsMatch(tbTo.Text))
            {
                lblDrop.Visible = true;
                valid = false;
            }
            if(!new Regex("[0-9]+-[0-9]{4}$").IsMatch(tbID.Text))
            {
                lblQuery.Visible = true;
                valid = false;
            }
            if(!new Regex("^[0-9]{1,4}$").IsMatch(tbNumOfPallet.Text))
            {
                lblPallet.Visible = true;
                valid = false;
            }
            if (!new Regex("^[0-9]{1,4}$").IsMatch(tbPalletToExchange.Text))
            {
                lblForExchange.Visible = true;
                valid = false;
            }
        }

        private void WriteToDB()
        {
            int allPallets = Convert.ToInt32(tbNumOfPallet.Text);
            int exchangedPallets = Convert.ToInt32(tbPalletToExchange.Text);

            MySql.Data.MySqlClient.MySqlConnection conn;
            string myConnectionString = "datasource=192.168.0.119;port=3307;username=user;password=;database=bomartrans";

            string query = "INSERT INTO publications (`PublicationID`, `ClaimAddress`, `DropAddress`, `PalletForExchange`, `ExchangedPallet`, `Saldo`, `Truck`, `Company`)" +
                " VALUES ('" + tbID.Text + "', '" + tbFrom.Text + "', '" + tbTo.Text + "', '" + tbNumOfPallet.Text + "', '" + tbPalletToExchange.Text + "', '" + (allPallets - exchangedPallets) + "', '" + tbTruck.Text + "', '" + tbCompany.Text + "');";

            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = myConnectionString;

            MySqlCommand commandDatabase = new MySqlCommand(query, conn);
            commandDatabase.CommandTimeout = 50;

            conn.Open();
            MySqlDataReader myReader = commandDatabase.ExecuteReader();

            conn.Close();
        }

        private void btnAddQuery_Click(object sender, EventArgs e)
        {
            Hide();

            Boolean valid = true;

            CheckEmptyAndFormat(ref valid);

            if(valid)
            {
                
                try
                {
                    Thread thread = new Thread(new ThreadStart(WriteToDB));
                    thread.Start();
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                MessageBox.Show("Published successfully");
            }

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            var textboxes = Controls.OfType<TextBox>()
                          .Where(c => c.Name.StartsWith("tb"))
                          .ToList();

            foreach(var tb in textboxes)
            {
                tb.Clear();
            }
        }

        private void NewQuerry_Load(object sender, EventArgs e)
        {

        }

        private void lblQuery_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip = new ToolTip();

            ToolTip.ToolTipIcon = ToolTipIcon.Info;
            ToolTip.ShowAlways = true;

            ToolTip.SetToolTip(lblQuery, "Format <ID>-<Year> ex. 123-1234");
        }

        private void lblClaim_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip = new ToolTip();

            ToolTip.ToolTipIcon = ToolTipIcon.Info;
            ToolTip.ShowAlways = true;

            ToolTip.SetToolTip(lblClaim, "Format <Contry code><City code> ex. D1234");
        }

        private void lblDrop_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip = new ToolTip();

            ToolTip.ToolTipIcon = ToolTipIcon.Info;
            ToolTip.ShowAlways = true;

            ToolTip.SetToolTip(lblDrop, "Format <Country code><CityCode> ex. D1234");
        }

        private void lblCompany_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip = new ToolTip();

            ToolTip.ToolTipIcon = ToolTipIcon.Info;
            ToolTip.ShowAlways = true;

            ToolTip.SetToolTip(lblCompany, "Format <Company name> ex. Schenker, Discordia");
        }

        private void lblTruck_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip = new ToolTip();

            ToolTip.ToolTipIcon = ToolTipIcon.Info;
            ToolTip.ShowAlways = true;

            ToolTip.SetToolTip(lblTruck, "Format <Registration number> ex. CA1234CC");
        }

        private void lblPallet_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip = new ToolTip();

            ToolTip.ToolTipIcon = ToolTipIcon.Info;
            ToolTip.ShowAlways = true;

            ToolTip.SetToolTip(lblPallet, "Format <Number> ex. 123");
        }

        private void lblForExchange_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip = new ToolTip();

            ToolTip.ToolTipIcon = ToolTipIcon.Info;
            ToolTip.ShowAlways = true;

            ToolTip.SetToolTip(lblForExchange, "Format <Number> ex. 123");
        }
    }
}

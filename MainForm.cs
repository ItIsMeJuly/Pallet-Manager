using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tracker
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            NewQuerry form = new NewQuerry();
            form.Show();
        }

        private void btnFindQuery_Click(object sender, EventArgs e)
        {
            FindQuery window = new FindQuery();
            window.Show();
        }

        private void btnDeleteQuery_Click(object sender, EventArgs e)
        {

        }
    }
}

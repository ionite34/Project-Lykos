using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Lykos
{
    public partial class DependencyDialog : Form
    {
        public DependencyDialog()
        {
            InitializeComponent();
            button_exit.DialogResult = DialogResult.Cancel;
            button_retry.DialogResult = DialogResult.Retry;
        }

        private void DependencyDialog_Load(object sender, EventArgs e)
        {

        }
    }
}

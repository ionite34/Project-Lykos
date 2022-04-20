using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Project_Lykos.Resampler_Tool;

namespace Project_Lykos
{
    public partial class AdditionalOptions : Form
    {
        public AdditionalOptions()
        {
            InitializeComponent();
        }

        private void button_resampler_Click(object sender, EventArgs e)
        {
            ResamplerTool form = new();
            form.ShowDialog();
            Close();
        }
    }
}

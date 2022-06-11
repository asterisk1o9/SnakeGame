using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PA5_Draft
{
    public partial class UserInput : Form
    {
        public UserInput()
        {
            InitializeComponent();
        }

        public int TakeInput()
        {
            return Convert.ToInt32(textBox1.Text);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using imcpkg;

namespace Example
{
    public partial class Jog : Form
    {
        public Jog()
        {
            InitializeComponent();
        }

        private void Jog_Load(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < Global.g_naxis; i++)
            {
                comboBox1.Items.Add("轴" + i.ToString());
            }
            comboBox1.SelectedItem = comboBox1.SelectedIndex = 0;

        }
        //匀速运动
        private void button1_Click(object sender, EventArgs e)
        {
            int st;
            string err;
            int axis = comboBox1.SelectedIndex;
            double acc = Convert.ToDouble(textBox1.Text);
            double startvel = Convert.ToDouble(textBox2.Text);
            double tgvel = Convert.ToDouble(textBox3.Text);
            if (!Global.isOpen())
            {
                MessageBox.Show(("设备没有打开！"));
                return;
            }

            st = IMC_Pkg.PKG_IMC_SetAccel(Global.g_handle, acc, acc, axis);

            if (st != 0)
            {
                st = IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, startvel, tgvel, axis);
            }

            if (st == 0)
            {
                err = Global.GetFunErrStr();
                MessageBox.Show(err);
            }	

        }
        //停止匀速运动
        private void button2_Click(object sender, EventArgs e)
        {
            int st;
            string err;
            int axis = comboBox1.SelectedIndex;
            if (!Global.isOpen())
            {
                MessageBox.Show(("设备没有打开！"));
                return;
            }
            st = IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, 0, 0, axis);
            if (st == 0)
            {
                err = Global.GetFunErrStr();
                MessageBox.Show(err);
            }	
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            int axis = comboBox1.SelectedIndex;
            double acc = Convert.ToDouble(textBox1.Text);
            double startvel = Convert.ToDouble(textBox2.Text);
            double tgvel = Convert.ToDouble(textBox3.Text);
            IMC_Pkg.PKG_IMC_SetAccel(Global.g_handle, acc, acc, axis);
            IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, startvel, tgvel, axis);
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            int axis = comboBox1.SelectedIndex;
            double acc = Convert.ToDouble(textBox1.Text);
            double startvel = Convert.ToDouble(textBox2.Text);
            double tgvel = Convert.ToDouble(textBox3.Text);
            IMC_Pkg.PKG_IMC_SetAccel(Global.g_handle, acc, acc, axis);
            IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, -startvel, -tgvel, axis);
        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            int axis = comboBox1.SelectedIndex;
            IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, 0, 0, axis);
        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            int axis = comboBox1.SelectedIndex;
            IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, 0, 0, axis);
        }
    }
}

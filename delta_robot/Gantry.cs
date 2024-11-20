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
    public partial class Gantry : Form
    {
        public Gantry()
        {
            InitializeComponent();
        }

        private void Gantry_Load(object sender, EventArgs e)
        {            
            int i;
            for (i = 0; i < Global.g_naxis; i++)
            {
                ComboBox1.Items.Add("轴" + i.ToString());
                ComboBox2.Items.Add("轴" + i.ToString());
            }
            ComboBox1.SelectedIndex = 1;
            ComboBox2.SelectedIndex = 0;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            double kp;
            ushort limit;
            int maxis , axis, st;
            String err;

            maxis = ComboBox1.SelectedIndex;
            axis = ComboBox2.SelectedIndex;
            if (maxis <= axis) {
                MessageBox.Show("主动轴的轴号需要大于从动轴的轴号");
            }

            kp = Convert.ToDouble(TextBox1.Text);
            limit = Convert.ToUInt16(TextBox2.Text);
            st = IMC_Pkg.PKG_IMC_SetGantry(Global.g_handle, kp, limit, maxis, axis);
            if (st == 0) {
                err = Global.GetFunErrStr();
                MessageBox.Show(err);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            int axis, st;
            String err;
            axis = ComboBox2.SelectedIndex;
            st = IMC_Pkg.PKG_IMC_DisGantry(Global.g_handle, axis);
            if (st == 0) {
                err = Global.GetFunErrStr();
                MessageBox.Show(err);
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            int axis, pos;
            double acc, startvel, endvel, tgvel;
            int st;
            string err;

            axis = ComboBox1.SelectedIndex;
            acc = Convert.ToDouble(TextBox8.Text);
            startvel = Convert.ToDouble(TextBox7.Text);
            endvel = Convert.ToDouble(TextBox6.Text);
            tgvel = Convert.ToDouble(textBox3.Text);
            pos = Convert.ToInt32(textBox4.Text);

            st = IMC_Pkg.PKG_IMC_SetAccel(Global.g_handle, acc, acc, axis);
            if (st != 0) {
                st = IMC_Pkg.PKG_IMC_MoveAbs(Global.g_handle, pos, startvel, endvel, tgvel, 0, axis);
            }
            if (st == 0) {
                err = Global.GetFunErrStr();
                MessageBox.Show(err);
            }

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            int axis, dist;
            double acc, startvel, endvel, tgvel;
            int st;
            string err;

            axis = ComboBox1.SelectedIndex;
            acc = Convert.ToDouble(TextBox8.Text);
            startvel = Convert.ToDouble(TextBox7.Text);
            endvel = Convert.ToDouble(TextBox6.Text);
            tgvel = Convert.ToDouble(textBox3.Text);
            dist = Convert.ToInt32(textBox5.Text);

            st = IMC_Pkg.PKG_IMC_SetAccel(Global.g_handle, acc, acc, axis);
            if (st != 0)
            {
                st = IMC_Pkg.PKG_IMC_MoveDist(Global.g_handle, dist, startvel, endvel, tgvel, 0, axis);
            }
            if (st == 0)
            {
                err = Global.GetFunErrStr();
                MessageBox.Show(err);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int axis;
            double tgvel;
            int st;
            string err;

            axis = ComboBox1.SelectedIndex;
            tgvel = Convert.ToDouble(textBox3.Text);

            st = IMC_Pkg.PKG_IMC_P2Pvel(Global.g_handle, tgvel, axis);
            if (st == 0)
            {
                err = Global.GetFunErrStr();
                MessageBox.Show(err);
            }
        }
    }
}

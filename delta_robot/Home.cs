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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < Global.g_naxis; i++)
            {
                comboBox1.Items.Add("轴" + i.ToString());
            }
            comboBox1.SelectedItem = comboBox1.SelectedIndex = 0;
            comboBox2.SelectedItem = comboBox2.SelectedIndex = 0;
            comboBox3.SelectedItem = comboBox3.SelectedIndex = 0;
            comboBox4.SelectedItem = comboBox4.SelectedIndex = 0;
            comboBox5.SelectedItem = comboBox5.SelectedIndex = 0;
            comboBox6.SelectedItem = comboBox6.SelectedIndex = 0;
            comboBox7.SelectedItem = comboBox7.SelectedIndex = 0;

        }
        //搜零
        private void button1_Click(object sender, EventArgs e)
        {
            int st;
            string err;
			int orgSW;					//零点开关选择
			int dir;					//搜零方向。零：正方向搜零；非零：负方向搜零；
			int stopmode;				//搜索到原点后的停止方式，零：立即停止在原点位置；非零：减速停止。
			int riseEdge;				//指定原点位置的边沿；零：下降沿； 非零：上升沿
			int edir;					//从哪个移动方向来判断原点位置边沿；零：正方向移动；非零：负方向移动；
			int reducer;				//减速开关选择
			int pos;					//设置零点时刻索引信号所在的位置值
			double hightvel;			//搜零时使用的高速度
			double lowvel;				//搜零时使用的低速度
            double acc;
			int axis;					//轴号

            axis = comboBox1.SelectedIndex;
            orgSW = comboBox2.SelectedIndex;
            dir = comboBox3.SelectedIndex;
            riseEdge = comboBox4.SelectedIndex;
            stopmode = comboBox5.SelectedIndex;
            edir = comboBox6.SelectedIndex;
            reducer = comboBox7.SelectedIndex;
            hightvel = Convert.ToDouble(textBox1.Text);
            lowvel = Convert.ToDouble(textBox2.Text);
            pos = Convert.ToInt32(textBox4.Text);
            acc = Convert.ToDouble(textBox3.Text);

            if (!Global.isOpen())
            {
                MessageBox.Show(("设备没有打开！"));
                return;
            }
            //设置搜零轴的加减速度
            st = IMC_Pkg.PKG_IMC_SetAccel(Global.g_handle, acc, acc, axis);
            if (st == 0)
            {
                err = Global.GetFunErrStr();
                MessageBox.Show(err);
                return;
            }
            if(checkBox1.Checked == true)
                st = IMC_Pkg.PKG_IMC_HomeORGIndex(Global.g_handle, orgSW, dir, stopmode, riseEdge, edir, reducer, pos, hightvel, lowvel, 0, axis);
            else
                st = IMC_Pkg.PKG_IMC_HomeORG(Global.g_handle, orgSW, dir, stopmode, riseEdge, edir, reducer, pos, hightvel, lowvel, 0, axis);
                
            if (st == 0)
            {
                err = Global.GetFunErrStr();
                MessageBox.Show(err);
            }

        }
        //停止搜零
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
            st = IMC_Pkg.PKG_IMC_HomeStop(Global.g_handle, axis);
            if (st == 0)
            {
                err = Global.GetFunErrStr();
                MessageBox.Show(err);
            }
        }
        //设置位置
        private void button3_Click(object sender, EventArgs e)
        {
            int st;
            string err;
            int axis = comboBox1.SelectedIndex;
            int pos = Convert.ToInt32(textBox7.Text);
            if (!Global.isOpen())
            {
                MessageBox.Show(("设备没有打开！"));
                return;
            }
            //设置当前位置为指定值
            st = IMC_Pkg.PKG_IMC_SetPos(Global.g_handle, pos, axis);
            if (st == 0)
            {
                err = Global.GetFunErrStr();
                MessageBox.Show(err);
            }
        }
    }
}

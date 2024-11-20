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
    public partial class AD : Form
    {
        double[] m_ad = new double[8];

        public AD()
        {
            InitializeComponent();
        }

        private void AD_Load(object sender, EventArgs e)
        {
            
            int i;
            ListViewItem lvitem ;

            ListView1.Columns.Add("AD通道", 60, HorizontalAlignment.Left);
            ListView1.Columns.Add("AD值", 80, HorizontalAlignment.Left);
            ListView1.Columns.Add("电压值", 80, HorizontalAlignment.Left);
            for( i = 0; i<8; i++)
            {
                lvitem = ListView1.Items.Add(i.ToString());
                lvitem.SubItems.Add(("0"));
                lvitem.SubItems.Add(("0.0"));
                m_ad[i] = 0;
            }
            
            for( i = 0; i<8; i++)
                ComboBox1.Items.Add(i.ToString());
            ComboBox1.SelectedIndex = 0;

            ComboBox2.Items.Add("进给倍率");
            for( i = 0 ; i< Global.g_naxis; i++)
                ComboBox2.Items.Add("轴" + i.ToString() + "的位置");
 
            ComboBox2.SelectedIndex = 0;

            //使能全部AD采样
            for( i = 0; i<8; i++)
                IMC_Pkg.PKG_IMC_SetADena(Global.g_handle, 1, i);


            timer1.Enabled = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            double ADstart, ADend, tgstart , tgend ;
            int ch, id;
            int st, sel;
            string err;

            tgstart = Convert.ToDouble(TextBox3.Text);
            tgend = Convert.ToDouble(TextBox4.Text);
            ADstart = Convert.ToDouble(TextBox1.Text);
            ADend = Convert.ToDouble(TextBox2.Text);
            ch = ComboBox1.SelectedIndex;
            sel = ComboBox2.SelectedIndex;

            //控制卡中总共有g_naxis个控制模块，每个模块都是相互独立的
            //在这里全部使用1个控制模块，即模块0
            id = 0;

            st = IMC_Pkg.PKG_IMC_SetADCtrlEX(Global.g_handle, ADstart, ADend, ch, tgstart, tgend, sel, id);
            if (sel > 0) {
                if (st > 0)
                    st = IMC_Pkg.PKG_IMC_SetAccel(Global.g_handle, 10, 10, sel - 1);
                if (st > 0)
                    st = IMC_Pkg.PKG_IMC_P2Pvel(Global.g_handle, 10.0, sel - 1);
            }
            if (st > 0) 
                st = IMC_Pkg.PKG_IMC_SetDAena(Global.g_handle, 1, ch);
            if (st == 0) {
                err = Global.GetFunErrStr();
                MessageBox.Show(err);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            int st, id;
            string err;
            id = 0;  //模块ID， 应与设置时相同
            st = IMC_Pkg.PKG_IMC_DisADCtrl(Global.g_handle, id);
            if (st == 0)
            {
                err = Global.GetFunErrStr();
                MessageBox.Show(err);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double ad ;
            int st, i , data;
            string err;

            ad = 0;
            for( i = 0;i<8; i++)
            {
                st = IMC_Pkg.PKG_IMC_GetADdata(Global.g_handle, ref ad, i);
                if (st == 0) {
                    err = Global.GetFunErrStr();
                }else{
                    if (m_ad[i] != ad) {
                        m_ad[i] = ad;
                        data = (int)(m_ad[i] / 10 * 4096 + 0.5);
                        ListView1.Items[i].SubItems[1].Text = data.ToString();
                        ListView1.Items[i].SubItems[2].Text = m_ad[i].ToString();
                    }
                }
            }
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sel;
            sel = ComboBox2.SelectedIndex;
            if (sel > 0) {
                Label12.Text = "（单位为脉冲）";
            }else{
                Label12.Text = "（有效范围是0到65535.99999）";
            }
        }
    }
}

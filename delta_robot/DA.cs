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
    public partial class DA : Form
    {
        public DA()
        {
            InitializeComponent();
        }

        private void DA_Load(object sender, EventArgs e)
        {
            int i;
            short data;
            double da;

            for (i = 0; i<8; i++)
                ComboBox1.Items.Add(i.ToString());
            ComboBox1.SelectedIndex = 0;
            
            for (i = 0; i<8; i++)
                ComboBox2.Items.Add("通道" + i.ToString() + "的模拟量输入");
            for (i = 1; i<3; i++)
                ComboBox2.Items.Add("PFIFO" + i.ToString() + "的速度");
            for( i = 0 ; i<Global.g_naxis; i++)
                ComboBox2.Items.Add("轴" + i.ToString() + "的速度");
            ComboBox2.SelectedIndex = 0;

            data = 0;
            IMC_Pkg.PKG_IMC_GetParam16(Global.g_handle, ParamDef.DAenaLoc, ref data, 0);
            if (data != 0) 
                CheckBox1.Checked = true;
            else
                CheckBox1.Checked = false;
            IMC_Pkg.PKG_IMC_GetParam16(Global.g_handle, ParamDef.DAshiftLoc, ref data, 0);
            da = 10.0 * data / 4096;
            TextBox5.Text = da.ToString();

            IMC_Pkg.PKG_IMC_GetParam16(Global.g_handle, ParamDef.DAenaLoc, ref data, 1);
            if (data != 0) 
                CheckBox2.Checked = true;
            else
                CheckBox2.Checked = false;
            IMC_Pkg.PKG_IMC_GetParam16(Global.g_handle, ParamDef.DAshiftLoc, ref data, 1);
            da = 10.0 * data / 4096;
            TextBox6.Text = da.ToString();

            IMC_Pkg.PKG_IMC_GetParam16(Global.g_handle, ParamDef.DAenaLoc, ref data, 2);
            if (data != 0) 
                CheckBox3.Checked = true;
            else
                CheckBox3.Checked = false;
            IMC_Pkg.PKG_IMC_GetParam16(Global.g_handle, ParamDef.DAshiftLoc, ref data, 2);
            da = 10.0 * data / 4096;
            TextBox7.Text = da.ToString();

            IMC_Pkg.PKG_IMC_GetParam16(Global.g_handle, ParamDef.DAenaLoc, ref data, 3);
            if (data != 0) 
                CheckBox4.Checked = true;
            else
                CheckBox4.Checked = false;
            IMC_Pkg.PKG_IMC_GetParam16(Global.g_handle, ParamDef.DAshiftLoc, ref data, 3);
            da = 10.0 * data / 4096;
            TextBox8.Text = da.ToString();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            double DAstart , DAend , tgstart , tgend ;
            int ch, id;
            int st , sel ;
            string err;

            tgstart = Convert.ToDouble(TextBox3.Text);
            tgend = Convert.ToDouble(TextBox4.Text);
            DAstart = Convert.ToDouble(TextBox1.Text);
            DAend = Convert.ToDouble(TextBox2.Text);
            ch = ComboBox1.SelectedIndex;
            sel = ComboBox2.SelectedIndex;

            if (sel == 0) {
                if (tgstart > 10)  tgstart = 10;
                if (tgend > 10)  tgend = 10;
                if (tgstart < 0)  tgstart = 0;
                if (tgend < 0)  tgend = 0;
            }

            st = IMC_Pkg.PKG_IMC_SetDAFollowEX(Global.g_handle, DAstart, DAend, ch, tgstart, tgend, sel);
            if (st == 0)
            {
                err = Global.GetFunErrStr();
                MessageBox.Show(err);
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            double data;
            if (CheckBox1.Checked == true ){
                IMC_Pkg.PKG_IMC_SetDAena(Global.g_handle, 1, 0);
                data = Convert.ToDouble(TextBox5.Text);
                IMC_Pkg.PKG_IMC_SetDAout(Global.g_handle, data, 0);
            }else
                IMC_Pkg.PKG_IMC_SetDAena(Global.g_handle, 0, 0);
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            double data;
            if (CheckBox2.Checked == true)
            {
                IMC_Pkg.PKG_IMC_SetDAena(Global.g_handle, 1, 1);
                data = Convert.ToDouble(TextBox6.Text);
                IMC_Pkg.PKG_IMC_SetDAout(Global.g_handle, data, 1);
            }
            else
                IMC_Pkg.PKG_IMC_SetDAena(Global.g_handle, 0, 1);
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            double data;
            if (CheckBox3.Checked == true)
            {
                IMC_Pkg.PKG_IMC_SetDAena(Global.g_handle, 1, 2);
                data = Convert.ToDouble(TextBox7.Text);
                IMC_Pkg.PKG_IMC_SetDAout(Global.g_handle, data, 2);
            }
            else
                IMC_Pkg.PKG_IMC_SetDAena(Global.g_handle, 0, 2);
        }

        private void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            double data;
            if (CheckBox4.Checked == true)
            {
                IMC_Pkg.PKG_IMC_SetDAena(Global.g_handle, 1, 3);
                data = Convert.ToDouble(TextBox8.Text);
                IMC_Pkg.PKG_IMC_SetDAout(Global.g_handle, data, 3);
            }
            else
                IMC_Pkg.PKG_IMC_SetDAena(Global.g_handle, 0, 3);
        }

        private void TextBox5_TextChanged(object sender, EventArgs e)
        {
            double data;
            data = Convert.ToDouble(TextBox5.Text);
            if (data > 10 )
                TextBox5.Text = "10";
            else if( data < -10 )
                TextBox5.Text = "-10";
            else
                IMC_Pkg.PKG_IMC_SetDAout(Global.g_handle, data, 0);
        }

        private void TextBox6_TextChanged(object sender, EventArgs e)
        {
            double data;
            data = Convert.ToDouble(TextBox6.Text);
            if (data > 10)
                TextBox6.Text = "10";
            else if (data < -10)
                TextBox6.Text = "-10";
            else
                IMC_Pkg.PKG_IMC_SetDAout(Global.g_handle, data, 1);
        }

        private void TextBox7_TextChanged(object sender, EventArgs e)
        {
            double data;
            data = Convert.ToDouble(TextBox7.Text);
            if (data > 10)
                TextBox7.Text = "10";
            else if (data < -10)
                TextBox7.Text = "-10";
            else
                IMC_Pkg.PKG_IMC_SetDAout(Global.g_handle, data, 2);
        }

        private void TextBox8_TextChanged(object sender, EventArgs e)
        {
            double data;
            data = Convert.ToDouble(TextBox8.Text);
            if (data > 10)
                TextBox8.Text = "10";
            else if (data < -10)
                TextBox8.Text = "-10";
            else
                IMC_Pkg.PKG_IMC_SetDAout(Global.g_handle, data, 3);
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sel;
            sel = ComboBox2.SelectedIndex;
            if (sel < 8) 
                Label11.Text = "（单位为伏特，有效范围是0到10）";
            else
                Label11.Text = "（单位为脉冲/毫秒，有效范围是0到32767.99999）";
        }
    }
}

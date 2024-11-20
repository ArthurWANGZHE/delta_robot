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
    public partial class PWM : Form
    {
        public PWM()
        {
            InitializeComponent();
        }

        private void PWM_Load(object sender, EventArgs e)
        {            
            int st, i;
            short data;
            double freq, dd;
            int pwm ;

            for (i = 0; i<8;i++){
                ComboBox1.Items.Add(i.ToString());
                ComboBox6.Items.Add(i.ToString());
            }
            ComboBox1.SelectedIndex = 0;
            ComboBox6.SelectedIndex = 0;
            
            for (i = 0; i<8;i++){
                ComboBox2.Items.Add("通道" + i.ToString() + "的模拟量输入");
                ComboBox4.Items.Add("通道" + i.ToString() + "的模拟量输入");
            }
            for (i = 1; i<3;i++){
                ComboBox2.Items.Add("PFIFO" + i.ToString() + "的速度");
                ComboBox4.Items.Add("PFIFO" + i.ToString() + "的速度");
            }
            for (i = 0; i<Global.g_naxis; i++){
                ComboBox2.Items.Add("轴" + i.ToString() + "的速度");
                ComboBox4.Items.Add("轴" + i.ToString() + "的速度");
            }
            ComboBox2.SelectedIndex = 0;
            ComboBox4.SelectedIndex = 0;

            ComboBox3.SelectedIndex = 0;
            ComboBox5.SelectedIndex = 0;

            ComboBox7.SelectedIndex = 0;
            ComboBox8.SelectedIndex = 0;
            ComboBox9.SelectedIndex = 0;
            ComboBox10.SelectedIndex = 0;

            data = 0;
            st = IMC_Pkg.PKG_IMC_GetParam16(Global.g_handle, ParamDef.PWMctrLoc, ref data, 0);
            if ((data & 1) == 1) 
                CheckBox1.Checked = true;
            else
                CheckBox1.Checked = false;
            st = IMC_Pkg.PKG_IMC_GetParam16(Global.g_handle, ParamDef.PWMpropLoc, ref data, 0);
            pwm = (int)(100.0 * data / 65536 + 0.5);
            TextBox11.Text = pwm.ToString();
            st = IMC_Pkg.PKG_IMC_GetParam32(Global.g_handle, ParamDef.PWMfreqLoc, ref pwm, 0);
            freq = 1000.0 * pwm / 65536;
            TextBox15.Text = freq.ToString();


            st = IMC_Pkg.PKG_IMC_GetParam16(Global.g_handle, ParamDef.PWMctrLoc, ref data, 1);
            if ((data & 1) == 1) 
                CheckBox2.Checked = true;
            else
                CheckBox2.Checked = false;
            st = IMC_Pkg.PKG_IMC_GetParam16(Global.g_handle, ParamDef.PWMpropLoc, ref data, 1);
            pwm = (int)(100.0 * data / 65536 + 0.5);
            TextBox12.Text = pwm.ToString();
            st = IMC_Pkg.PKG_IMC_GetParam32(Global.g_handle, ParamDef.PWMfreqLoc, ref pwm, 1);
            freq = 1000.0 * pwm / 65536;
            TextBox16.Text = freq.ToString();


            st = IMC_Pkg.PKG_IMC_GetParam16(Global.g_handle, ParamDef.PWMctrLoc, ref data, 2);
            if ((data & 1) == 1) 
                CheckBox3.Checked = true;
            else
                CheckBox3.Checked = false;
            st = IMC_Pkg.PKG_IMC_GetParam16(Global.g_handle, ParamDef.PWMpropLoc, ref data, 2);
            pwm = (int)(100.0 * data / 65536 + 0.5);
            TextBox13.Text = pwm.ToString();
            st = IMC_Pkg.PKG_IMC_GetParam32(Global.g_handle, ParamDef.PWMfreqLoc, ref pwm, 2);
            freq = 1000.0 * pwm / 65536;
            TextBox17.Text = freq.ToString();


            st = IMC_Pkg.PKG_IMC_GetParam16(Global.g_handle, ParamDef.PWMctrLoc, ref data, 3);
            if ((data & 1) == 1) 
                CheckBox4.Checked = true;
            else
                CheckBox4.Checked = false;
            st = IMC_Pkg.PKG_IMC_GetParam16(Global.g_handle, ParamDef.PWMpropLoc, ref data, 3);
            pwm = (int)(100.0 * data / 65536 + 0.5);
            TextBox14.Text = pwm.ToString();
            st = IMC_Pkg.PKG_IMC_GetParam32(Global.g_handle, ParamDef.PWMfreqLoc, ref pwm, 3);
            freq = 1000.0 * pwm / 65536;
            TextBox18.Text = freq.ToString();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
            int polar;
            double freq , PWMstart , PWMend , tgStart , tgEnd ;
            int ch , sel;
            int st;
            string err;

            PWMstart = Convert.ToDouble(TextBox1.Text);
            PWMend = Convert.ToDouble(TextBox2.Text);
            tgStart = Convert.ToDouble(TextBox3.Text);
            tgEnd = Convert.ToDouble(TextBox4.Text);
            ch = ComboBox1.SelectedIndex;
            polar = ComboBox3.SelectedIndex;
            freq = Convert.ToDouble(TextBox5.Text);

            sel = ComboBox2.SelectedIndex;
            if (sel == 0) {
                if (tgStart > 10)  tgStart = 10;
                if (tgEnd > 10)  tgEnd = 10;
                if (tgStart < 0)  tgStart = 0;
                if (tgEnd < 0)  tgEnd = 0;
            }

            PWMstart = PWMstart / 100;
            PWMend = PWMend / 100;

            st = IMC_Pkg.PKG_IMC_PWMpropFollowEX(Global.g_handle, polar, freq, PWMstart, PWMend, 0, ch, tgStart, tgEnd, sel);
            if (st == 0) {
                err = Global.GetFunErrStr();
                MessageBox.Show(err);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            
            int polar;
            double prop , PWMstart , PWMend , tgStart , tgEnd ;
            int ch , sel;
            int st;
            string err;

            PWMstart = Convert.ToDouble(TextBox10.Text);
            PWMend = Convert.ToDouble(TextBox9.Text);
            tgStart = Convert.ToDouble(TextBox7.Text);
            tgEnd = Convert.ToDouble(TextBox6.Text);
            ch = ComboBox6.SelectedIndex;
            polar = ComboBox5.SelectedIndex;
            prop = Convert.ToDouble(TextBox8.Text);

            sel = ComboBox4.SelectedIndex;
            if (sel == 0) {
                if (tgStart > 10)  tgStart = 10;
                if (tgEnd > 10)  tgEnd = 10;
                if (tgStart < 0)  tgStart = 0;
                if (tgEnd < 0)  tgEnd = 0;
            }

            prop = prop / 100;

            st = IMC_Pkg.PKG_IMC_PWMfreqFollowEX(Global.g_handle, polar, prop, PWMstart, PWMend, 0, ch, tgStart, tgEnd, sel);
            if (st == 0) {
                err = Global.GetFunErrStr();
                MessageBox.Show(err);
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {            
            int polar , st;
            double data;

            polar = ComboBox7.SelectedIndex;
            if (CheckBox1.Checked == true) {
                st = IMC_Pkg.PKG_IMC_SetPWMena(Global.g_handle, 1, polar, 0);
                data = Convert.ToDouble(TextBox11.Text);
                data = data / 100;
                st = IMC_Pkg.PKG_IMC_SetPWMprop(Global.g_handle, data, 0);
                data = Convert.ToDouble(TextBox15.Text);
                if (data > 65535999.99) 
                    data = 65535999.99;
                st = IMC_Pkg.PKG_IMC_SetPWMfreq(Global.g_handle, data, 0);
            }else
                st = IMC_Pkg.PKG_IMC_SetPWMena(Global.g_handle, 0, polar, 0);
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            int polar, st;
            double data;

            polar = ComboBox8.SelectedIndex;
            if (CheckBox2.Checked == true)
            {
                st = IMC_Pkg.PKG_IMC_SetPWMena(Global.g_handle, 1, polar, 1);
                data = Convert.ToDouble(TextBox12.Text);
                data = data / 100;
                st = IMC_Pkg.PKG_IMC_SetPWMprop(Global.g_handle, data, 1);
                data = Convert.ToDouble(TextBox16.Text);
                if (data > 65535999.99)
                    data = 65535999.99;
                st = IMC_Pkg.PKG_IMC_SetPWMfreq(Global.g_handle, data, 1);
            }
            else
                st = IMC_Pkg.PKG_IMC_SetPWMena(Global.g_handle, 0, polar, 1);
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            int polar, st;
            double data;

            polar = ComboBox9.SelectedIndex;
            if (CheckBox3.Checked == true)
            {
                st = IMC_Pkg.PKG_IMC_SetPWMena(Global.g_handle, 1, polar, 2);
                data = Convert.ToDouble(TextBox13.Text);
                data = data / 100;
                st = IMC_Pkg.PKG_IMC_SetPWMprop(Global.g_handle, data, 2);
                data = Convert.ToDouble(TextBox17.Text);
                if (data > 65535999.99)
                    data = 65535999.99;
                st = IMC_Pkg.PKG_IMC_SetPWMfreq(Global.g_handle, data, 2);
            }
            else
                st = IMC_Pkg.PKG_IMC_SetPWMena(Global.g_handle, 0, polar, 2);
        }

        private void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            int polar, st;
            double data;

            polar = ComboBox10.SelectedIndex;
            if (CheckBox4.Checked == true)
            {
                st = IMC_Pkg.PKG_IMC_SetPWMena(Global.g_handle, 1, polar, 3);
                data = Convert.ToDouble(TextBox14.Text);
                data = data / 100;
                st = IMC_Pkg.PKG_IMC_SetPWMprop(Global.g_handle, data, 3);
                data = Convert.ToDouble(TextBox18.Text);
                if (data > 65535999.99)
                    data = 65535999.99;
                st = IMC_Pkg.PKG_IMC_SetPWMfreq(Global.g_handle, data, 3);
            }
            else
                st = IMC_Pkg.PKG_IMC_SetPWMena(Global.g_handle, 0, polar, 3);
        }

        private void TextBox11_TextChanged(object sender, EventArgs e)
        {
            double prop;
            int st;

            prop = Convert.ToDouble(TextBox11.Text);
            prop = prop / 100;
            st = IMC_Pkg.PKG_IMC_SetPWMprop(Global.g_handle, prop, 0);
        }

        private void TextBox12_TextChanged(object sender, EventArgs e)
        {
            double prop;
            int st;

            prop = Convert.ToDouble(TextBox12.Text);
            prop = prop / 100;
            st = IMC_Pkg.PKG_IMC_SetPWMprop(Global.g_handle, prop, 1);
        }

        private void TextBox13_TextChanged(object sender, EventArgs e)
        {
            double prop;
            int st;

            prop = Convert.ToDouble(TextBox13.Text);
            prop = prop / 100;
            st = IMC_Pkg.PKG_IMC_SetPWMprop(Global.g_handle, prop, 2);
        }

        private void TextBox14_TextChanged(object sender, EventArgs e)
        {
            double prop;
            int st;

            prop = Convert.ToDouble(TextBox14.Text);
            prop = prop / 100;
            st = IMC_Pkg.PKG_IMC_SetPWMprop(Global.g_handle, prop, 3);
        }

        private void TextBox15_TextChanged(object sender, EventArgs e)
        {
            double freq;
            int st;

            freq = Convert.ToDouble(TextBox15.Text);
            st = IMC_Pkg.PKG_IMC_SetPWMfreq(Global.g_handle, freq, 0);
        }

        private void TextBox16_TextChanged(object sender, EventArgs e)
        {
            double freq;
            int st;

            freq = Convert.ToDouble(TextBox16.Text);
            st = IMC_Pkg.PKG_IMC_SetPWMfreq(Global.g_handle, freq, 1);
        }

        private void TextBox17_TextChanged(object sender, EventArgs e)
        {
            double freq;
            int st;

            freq = Convert.ToDouble(TextBox17.Text);
            st = IMC_Pkg.PKG_IMC_SetPWMfreq(Global.g_handle, freq, 2);
        }

        private void TextBox18_TextChanged(object sender, EventArgs e)
        {
            double freq;
            int st;

            freq = Convert.ToDouble(TextBox18.Text);
            st = IMC_Pkg.PKG_IMC_SetPWMfreq(Global.g_handle, freq, 3);
        }

        private void ComboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            int polar, ena , st;

            polar = ComboBox7.SelectedIndex;
            ena = CheckBox1.Checked == true ? 1 : 0;
            st = IMC_Pkg.PKG_IMC_SetPWMena(Global.g_handle, ena, polar, 0);
        }

        private void ComboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            int polar, ena, st;

            polar = ComboBox8.SelectedIndex;
            ena = CheckBox2.Checked == true ? 1 : 0;
            st = IMC_Pkg.PKG_IMC_SetPWMena(Global.g_handle, ena, polar, 1);
        }

        private void ComboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            int polar, ena, st;

            polar = ComboBox9.SelectedIndex;
            ena = CheckBox3.Checked == true ? 1 : 0;
            st = IMC_Pkg.PKG_IMC_SetPWMena(Global.g_handle, ena, polar, 2);
        }

        private void ComboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            int polar, ena, st;

            polar = ComboBox10.SelectedIndex;
            ena = CheckBox4.Checked == true ? 1 : 0;
            st = IMC_Pkg.PKG_IMC_SetPWMena(Global.g_handle, ena, polar, 3);
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sel;
            sel = ComboBox2.SelectedIndex;
            if( sel < 8)
                Label11.Text = "（单位为伏特，有效范围是0到10）";
            else
                Label11.Text = "（单位为脉冲/毫秒，有效范围是0到32767.99999）";
        }

        private void ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sel;
            sel = ComboBox4.SelectedIndex;
            if (sel < 8)
                Label22.Text = "（单位为伏特，有效范围是0到10）";
            else
                Label22.Text = "（单位为脉冲/毫秒，有效范围是0到32767.99999）";
        }
    }
}

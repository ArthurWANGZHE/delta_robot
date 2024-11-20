using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

using imcpkg;
using Example;
using System.Numerics;
using Delta_10._31_;
using System.Reflection;



namespace delta_robot
{
    public partial class Form1 : Form
    {
        int[] m_encp = new int[Global.MAX_NAXIS];
        int[] m_curpos = new int[Global.MAX_NAXIS];
        ushort[] m_error = new ushort[Global.MAX_NAXIS];
        int[] m_encp_t = new int[Global.MAX_NAXIS];
        int[] m_curpos_t = new int[Global.MAX_NAXIS];
        ushort[] m_error_t = new ushort[Global.MAX_NAXIS];
        double flag = 0;


         
        RobotArm robotarm = new RobotArm(173, 35, 270,0.001, 100, 100, 100, 0.01);
        WhiteboardControl whiteboardControl = new WhiteboardControl();



        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            byte[] info = new byte[16 * 256];
            int num = 0, i;
            string str;


            //程序初始化时搜索PC中的网卡
            if (IMC_Pkg.PKG_IMC_FindNetCard(info, ref num) != 0)
            {
                for (i = 0; i < num; i++)
                {
                    str = System.Text.Encoding.Default.GetString(info, i * 256, 256);
                    comboBox1.Items.Add(str);
                }
                if (comboBox1.Items.Count > 0)
                    comboBox1.SelectedItem = comboBox1.SelectedIndex = 0;
            }
            for (i = 0; i < 64; i++)
            {
                str = i.ToString();
                comboBox2.Items.Add(str);
            }
            comboBox2.SelectedItem = comboBox2.SelectedIndex = 0;


            //listView1.Columns.Add("轴号", 70, HorizontalAlignment.Left);
            //listView1.Columns.Add("指令位置（脉冲）", 140, HorizontalAlignment.Left);
            //listView1.Columns.Add("机械位置（脉冲）", 140, HorizontalAlignment.Left);
            //listView1.Columns.Add("错误寄存器", 140, HorizontalAlignment.Left);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }



        private void button16_Click(object sender, EventArgs e)
        {
            IMC_Pkg.PKG_IMC_Emstop(Global.g_handle, 1);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            double velRPM = 200.00;     //窗口输入值，单位为RPM
            double vel = 0.106667 * velRPM;

            for (int i = 0; i < 3; i++)
            {
                int orgSW = 1;              //orgSW：零点开关选择
                int dir = 0;                //dir：开始搜零时的方向。零：正方向开始搜零；非零：负方向开始搜零
                int stopmode = 0;           //stopmode：搜索到原点后的停止方式。零：立即停止在原点位置；非零：减速停止
                int riseEdge = 0;           //riseEdge：指定原点位置的边沿；零：下降沿； 非零：上升沿
                int edir = 0;               //edir：从哪个移动方向来判断原点位置边沿。零：正方向移动时遇到的开关边沿为有效的原点边沿；非零：负方向移动时遇到的开关边沿为有效的原点边沿
                int reducer = 0;            //reducer：减速装置选择
                int pos = 0;                //pos：设置零点时刻零点开关的位置值
                double hightvel = vel;      //hightvel：搜零时使用的高速度
                double lowvel = vel;        //lowvel：搜零时使用的低速度
                int wait = 0;               //wait：是否等待搜零结束后函数再返回
                int axis = i;               //axis：轴号

                double accel = 6;           //加速度
                double decel = 6;           //减速度

                IMC_Pkg.PKG_IMC_SetAccel(Global.g_handle, accel, decel, axis);
                IMC_Pkg.PKG_IMC_HomeORG(Global.g_handle, orgSW, dir, stopmode, riseEdge,
                                            edir, reducer, pos, hightvel, lowvel, wait, axis);
            }

            //int fifo = (int)FIFO_SEL.SEL_PFIFO1;
            //while (IMC_Pkg.PKG_IMC_isPstop(Global.g_handle, fifo) == 0) { }

            double t1 = 0, t2 = 0, t3 = 0;
            var result1 = robotarm.InverseKinematics(t1, t2, t3);
            double x = result1.x;
            double y = result1.y;
            double z = result1.z;
            double roundedx = Math.Round(x, 2);
            double roundedy = Math.Round(y, 2);
            double roundedz = Math.Round(z, 2);
            textBox7.Text = roundedx.ToString();
            textBox8.Text = roundedy.ToString();
            textBox9.Text = roundedz.ToString();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //打开设备
        private void button1_Click(object sender, EventArgs e)
        {
            int netid, imcid, i;
            string err;
            netid = comboBox1.SelectedIndex;
            imcid = comboBox2.SelectedIndex;
            if (Global.isOpen())//设备已打开则关闭
            {
                IMC_Pkg.PKG_IMC_Close(Global.g_handle);
                Global.g_handle = IntPtr.Zero;
                button1.Text = "打开设备";
                timer1.Enabled = false;
            }
            else
            {
                Global.g_handle = IMC_Pkg.PKG_IMC_Open(netid, imcid);
                if (Global.isOpen())
                {
                    Global.g_imcType = IMC_Pkg.PKG_IMC_GetType(Global.g_handle);
                    if (!Global.Is4xxxIMC(Global.g_imcType))
                    {
                        IMC_Pkg.PKG_IMC_Close(Global.g_handle);
                        Global.g_handle = IntPtr.Zero;
                        MessageBox.Show("软件与控制卡型号不匹配！");
                        return;
                    }
                    button1.Text = ("关闭设备");
                    i = IMC_Pkg.PKG_IMC_InitCfg(Global.g_handle);
                    if (i == 0)
                    {
                        err = Global.GetFunErrStr();
                        MessageBox.Show(err);
                    }
                    //timer1.Enabled = true;	//启动定时器，定时读取位置信息

                    Global.g_naxis = IMC_Pkg.PKG_IMC_GetNaxis(Global.g_handle);//获得控制卡最大轴数
                    if (Global.g_naxis > 0)
                    {
                        //listView1.Items.Clear();
                        for (i = 0; i < Global.g_naxis; i++)
                        {
                            //ListViewItem lvitem = listView1.Items.Add("轴" + i.ToString());
                            //lvitem.SubItems.Add(("0"));
                            //lvitem.SubItems.Add(("0"));
                            //lvitem.SubItems.Add(("0x0000"));
                            //m_error_t[i] = 0;
                            //m_curpos_t[i] = 0;
                            //m_encp_t[i] = 0;
                        }
                    }
                    else
                    {
                        err = Global.GetFunErrStr();
                        MessageBox.Show(err);
                    }

                }
                else
                {
                    MessageBox.Show(("无法打开设备！\r\n\r\n请检查网卡和控制卡ID是否选择正确！"));
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cfg cfgForm = new Cfg();
            if (!Global.isOpen())
            {
                MessageBox.Show("请先打开设备!");
                return;
            }
            cfgForm.ShowDialog();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int i;
            if (!Global.isOpen())
                return;
            //   listView1.BeginUpdate();

            if (IMC_Pkg.PKG_IMC_GetCurpos(Global.g_handle, m_curpos, Global.g_naxis) != 0)//获得指令位置
            {
                for (i = 0; i < Global.g_naxis; i++)
                {
                    if (m_curpos[i] != m_curpos_t[i])
                    {
                        m_curpos_t[i] = m_curpos[i];
                        listView1.Items[i].SubItems[1].Text = m_curpos[i].ToString();
                    }
                }
            }
            if (IMC_Pkg.PKG_IMC_GetEncp(Global.g_handle, m_encp, Global.g_naxis) != 0)//获得反馈编码器位置
            {
                for (i = 0; i < Global.g_naxis; i++)
                {
                    if (m_encp[i] != m_encp_t[i])
                    {
                        m_encp_t[i] = m_encp[i];
                        listView1.Items[i].SubItems[2].Text = m_encp[i].ToString("D");
                    }
                }
            }
            if (IMC_Pkg.PKG_IMC_GetErrorReg(Global.g_handle, m_error, Global.g_naxis) != 0)//获得错误寄存器值
            {
                for (i = 0; i < Global.g_naxis; i++)
                {
                    if (m_error[i] != m_error_t[i])
                    {
                        m_error_t[i] = m_error[i];
                        listView1.Items[i].SubItems[3].Text = "0x" + m_error[i].ToString("X4");
                    }
                }
            }
            //      listView1.EndUpdate();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double x2 = Convert.ToDouble(textBox3.Text);
            double y2 = Convert.ToDouble(textBox4.Text);
            double z2 = Convert.ToDouble(textBox5.Text);
            var (deltaT1Pulse, deltaT2Pulse, deltaT3Pulse) = robotarm.CoordinateConvertPulse(x1, y1, z1, x2, y2, z2);


            //启用插补空间
            int[] axis = new int[] { 0, 1, 2 };
            int axisnum = 3;
            int fifo;
            fifo = (int)FIFO_SEL.SEL_PFIFO1;

            IMC_Pkg.PKG_IMC_PFIFOclear(Global.g_handle, fifo);              //清除插补空间中未被执行的插补指令
            IMC_Pkg.PKG_IMC_AxisMap(Global.g_handle, axis, axisnum, fifo);  //轴映射
            IMC_Pkg.PKG_IMC_PFIFOrun(Global.g_handle, fifo);                //启用插补空间

            int mode = 0;
            IMC_Pkg.PKG_IMC_SetPFIFOvelMode(Global.g_handle, mode, fifo);   //插补速度控制模式

            int accel = 6;
            IMC_Pkg.PKG_IMC_SetPFIFOaccel(Global.g_handle, accel, fifo);    //插补加速度

            //开始相对运动直线插补
            int[] dist = new int[] { deltaT1Pulse, deltaT2Pulse, deltaT3Pulse };

            double velmmPs = Convert.ToDouble(textBox1.Text);
            double velPulsePms = 72 * velmmPs / 1000 / 0.05625;     //窗口输入值 mm/s 转化成 pulse/ms
            double tgvel = velPulsePms;                             //目标速度
            double endvel = 0;                                      //末端速度

            int wait = 0;
            IMC_Pkg.PKG_IMC_Line_Dist(Global.g_handle, dist, axisnum, tgvel, endvel, wait, fifo);

            //while (IMC_Pkg.PKG_IMC_isPstop(Global.g_handle, fifo) == 0) { }

            textBox7.Text = x2.ToString();
            textBox8.Text = y2.ToString();
            textBox9.Text = z2.ToString();
        }


        private void button3_MouseDown(object sender, MouseEventArgs e)
        {

            int axis = 0;                                           
            double accRPM = 100.00;       
            double startvelRPM = 100.00;   
            double tgvelRPM = 100.00;      

            double acc = 0.106667 * accRPM;                         
            double startvel = 0.106667 * startvelRPM;               
            double tgvel = 0.106667 * tgvelRPM;                     

            IMC_Pkg.PKG_IMC_SetAccel(Global.g_handle, acc, acc, axis);
            IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, startvel, tgvel, axis);


        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            int axis = 0;                                           
            IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, 0, 0, axis);   
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            
            {
                int axis = 0;                                          
                double accRPM = 100.00;      
                double startvelRPM = 100.00;  
                double tgvelRPM = 100.00;      

                double acc = 0.106667 * accRPM;                         
                double startvel = 0.106667 * startvelRPM;               
                double tgvel = 0.106667 * tgvelRPM;                     

                IMC_Pkg.PKG_IMC_SetAccel(Global.g_handle, acc, acc, axis);
                IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, -startvel,-tgvel, axis);
            }
        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            int axis = 0;
            IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, 0, 0, axis);
        }

        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            
            {
                int axis = 1;                                         
                double accRPM = 100.00;        
                double startvelRPM = 100.00;   
                double tgvelRPM = 100.00;      

                double acc = 0.106667 * accRPM;                        
                double startvel = 0.106667 * startvelRPM;               
                double tgvel = 0.106667 * tgvelRPM;                     

                IMC_Pkg.PKG_IMC_SetAccel(Global.g_handle, acc, acc, axis);
                IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, startvel, tgvel, axis);
            }
        }

        private void button5_MouseUp(object sender, MouseEventArgs e)
        {
            int axis = 1;
            IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, 0, 0, axis);

        }

        private void button6_MouseDown(object sender, MouseEventArgs e)
        {
           
            {
                int axis = 1;                                      
                double accRPM = 100.00;        
                double startvelRPM = 100.00;   
                double tgvelRPM = 100.00;      

                double acc = 0.106667 * accRPM;                         
                double startvel = 0.106667 * startvelRPM;              
                double tgvel = 0.106667 * tgvelRPM;                   

                IMC_Pkg.PKG_IMC_SetAccel(Global.g_handle, acc, acc, axis);
                IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, -startvel, -tgvel, axis);
            }
        }

        private void button6_MouseUp(object sender, MouseEventArgs e)
        {
            int axis = 1;
            IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, 0, 0, axis);
        }

        private void button7_MouseDown(object sender, MouseEventArgs e)
        {
       
            {
                int axis = 2;                                      
                double accRPM = 100.00;        
                double startvelRPM = 100.00;  
                double tgvelRPM = 100.00;      

                double acc = 0.106667 * accRPM;                    
                double startvel = 0.106667 * startvelRPM;            
                double tgvel = 0.106667 * tgvelRPM;                    

                IMC_Pkg.PKG_IMC_SetAccel(Global.g_handle, acc, acc, axis);
                IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, startvel, tgvel, axis);
            }
        }

        private void button7_MouseUp(object sender, MouseEventArgs e)
        {
            int axis = 2;
            IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, 0, 0, axis);

        }

        private void button8_MouseDown(object sender, MouseEventArgs e)
        {
         
            {
                int axis = 2;                                      
                double accRPM = 100.00;        
                double startvelRPM = 100.00;   
                double tgvelRPM = 100.00;     

                double acc = 0.106667 * accRPM;                   
                double startvel = 0.106667 * startvelRPM;      
                double tgvel = 0.106667 * tgvelRPM;                   

                IMC_Pkg.PKG_IMC_SetAccel(Global.g_handle, acc, acc, axis);
                IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, -startvel, -tgvel, axis);
            }
        }

        private void button8_MouseUp(object sender, MouseEventArgs e)
        {
            int axis = 2;
            IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, 0, 0, axis);

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {

        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double distance = Convert.ToDouble(textBox11.Text);
            double x2 = x1 + distance;                                  //x轴寸动
            double y2 = y1;
            double z2 = z1;
            var (deltaT1Pulse, deltaT2Pulse, deltaT3Pulse) = robotarm.CoordinateConvertPulse(x1, y1, z1, x2, y2, z2);

            //启用插补空间
            int[] axis = new int[] { 0, 1, 2 };
            int axisnum = 3;
            int fifo;
            fifo = (int)FIFO_SEL.SEL_PFIFO1;

            IMC_Pkg.PKG_IMC_PFIFOclear(Global.g_handle, fifo);              //清除插补空间中未被执行的插补指令
            IMC_Pkg.PKG_IMC_AxisMap(Global.g_handle, axis, axisnum, fifo);  //轴映射
            IMC_Pkg.PKG_IMC_PFIFOrun(Global.g_handle, fifo);                //启用插补空间

            int mode = 0;
            IMC_Pkg.PKG_IMC_SetPFIFOvelMode(Global.g_handle, mode, fifo);   //插补速度控制模式

            int accel = 6;
            IMC_Pkg.PKG_IMC_SetPFIFOaccel(Global.g_handle, accel, fifo);    //插补加速度

            //开始相对运动直线插补
            int[] dist = new int[] { deltaT1Pulse, deltaT2Pulse, deltaT3Pulse };

            double velmmPs = Convert.ToDouble(textBox10.Text);
            double velPulsePms = 72 * velmmPs / 1000 / 0.05625;     //窗口输入值 mm/s 转化成 pulse/ms
            double tgvel = velPulsePms;                             //目标速度
            double endvel = 0;                                      //末端速度

            int wait = 0;
            IMC_Pkg.PKG_IMC_Line_Dist(Global.g_handle, dist, axisnum, tgvel, endvel, wait, fifo);

            //while (IMC_Pkg.PKG_IMC_isPstop(Global.g_handle, fifo) == 0) { }

            textBox7.Text = x2.ToString();
            textBox8.Text = y2.ToString();
            textBox9.Text = z2.ToString();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void button21_Click(object sender, EventArgs e)
        {
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double distance = Convert.ToDouble(textBox11.Text);
            double x2 = x1 - distance;                                  //x轴寸动
            double y2 = y1;
            double z2 = z1;
            var (deltaT1Pulse, deltaT2Pulse, deltaT3Pulse) = robotarm.CoordinateConvertPulse(x1, y1, z1, x2, y2, z2);

            //启用插补空间
            int[] axis = new int[] { 0, 1, 2 };
            int axisnum = 3;
            int fifo;
            fifo = (int)FIFO_SEL.SEL_PFIFO1;

            IMC_Pkg.PKG_IMC_PFIFOclear(Global.g_handle, fifo);              //清除插补空间中未被执行的插补指令
            IMC_Pkg.PKG_IMC_AxisMap(Global.g_handle, axis, axisnum, fifo);  //轴映射
            IMC_Pkg.PKG_IMC_PFIFOrun(Global.g_handle, fifo);                //启用插补空间

            int mode = 0;
            IMC_Pkg.PKG_IMC_SetPFIFOvelMode(Global.g_handle, mode, fifo);   //插补速度控制模式

            int accel = 6;
            IMC_Pkg.PKG_IMC_SetPFIFOaccel(Global.g_handle, accel, fifo);    //插补加速度

            //开始相对运动直线插补
            int[] dist = new int[] { deltaT1Pulse, deltaT2Pulse, deltaT3Pulse };

            double velmmPs = Convert.ToDouble(textBox10.Text);
            double velPulsePms = 72 * velmmPs / 1000 / 0.05625;     //窗口输入值 mm/s 转化成 pulse/ms
            double tgvel = velPulsePms;                             //目标速度
            double endvel = 0;                                      //末端速度

            int wait = 0;
            IMC_Pkg.PKG_IMC_Line_Dist(Global.g_handle, dist, axisnum, tgvel, endvel, wait, fifo);

            //while (IMC_Pkg.PKG_IMC_isPstop(Global.g_handle, fifo) == 0) { }

            textBox7.Text = x2.ToString();
            textBox8.Text = y2.ToString();
            textBox9.Text = z2.ToString();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double distance = Convert.ToDouble(textBox11.Text);
            double x2 = x1;                                  //x轴寸动
            double y2 = y1 + distance;
            double z2 = z1;
            var (deltaT1Pulse, deltaT2Pulse, deltaT3Pulse) = robotarm.CoordinateConvertPulse(x1, y1, z1, x2, y2, z2);

            //启用插补空间
            int[] axis = new int[] { 0, 1, 2 };
            int axisnum = 3;
            int fifo;
            fifo = (int)FIFO_SEL.SEL_PFIFO1;

            IMC_Pkg.PKG_IMC_PFIFOclear(Global.g_handle, fifo);              //清除插补空间中未被执行的插补指令
            IMC_Pkg.PKG_IMC_AxisMap(Global.g_handle, axis, axisnum, fifo);  //轴映射
            IMC_Pkg.PKG_IMC_PFIFOrun(Global.g_handle, fifo);                //启用插补空间

            int mode = 0;
            IMC_Pkg.PKG_IMC_SetPFIFOvelMode(Global.g_handle, mode, fifo);   //插补速度控制模式

            int accel = 6;
            IMC_Pkg.PKG_IMC_SetPFIFOaccel(Global.g_handle, accel, fifo);    //插补加速度

            //开始相对运动直线插补
            int[] dist = new int[] { deltaT1Pulse, deltaT2Pulse, deltaT3Pulse };

            double velmmPs = Convert.ToDouble(textBox10.Text);
            double velPulsePms = 72 * velmmPs / 1000 / 0.05625;     //窗口输入值 mm/s 转化成 pulse/ms
            double tgvel = velPulsePms;                             //目标速度
            double endvel = 0;                                      //末端速度

            int wait = 0;
            IMC_Pkg.PKG_IMC_Line_Dist(Global.g_handle, dist, axisnum, tgvel, endvel, wait, fifo);

            //while (IMC_Pkg.PKG_IMC_isPstop(Global.g_handle, fifo) == 0) { }

            textBox7.Text = x2.ToString();
            textBox8.Text = y2.ToString();
            textBox9.Text = z2.ToString();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double distance = Convert.ToDouble(textBox11.Text);
            double x2 = x1;                                  //x轴寸动
            double y2 = y1 - distance;
            double z2 = z1;
            var (deltaT1Pulse, deltaT2Pulse, deltaT3Pulse) = robotarm.CoordinateConvertPulse(x1, y1, z1, x2, y2, z2);

            //启用插补空间
            int[] axis = new int[] { 0, 1, 2 };
            int axisnum = 3;
            int fifo;
            fifo = (int)FIFO_SEL.SEL_PFIFO1;

            IMC_Pkg.PKG_IMC_PFIFOclear(Global.g_handle, fifo);              //清除插补空间中未被执行的插补指令
            IMC_Pkg.PKG_IMC_AxisMap(Global.g_handle, axis, axisnum, fifo);  //轴映射
            IMC_Pkg.PKG_IMC_PFIFOrun(Global.g_handle, fifo);                //启用插补空间

            int mode = 0;
            IMC_Pkg.PKG_IMC_SetPFIFOvelMode(Global.g_handle, mode, fifo);   //插补速度控制模式

            int accel = 6;
            IMC_Pkg.PKG_IMC_SetPFIFOaccel(Global.g_handle, accel, fifo);    //插补加速度

            //开始相对运动直线插补
            int[] dist = new int[] { deltaT1Pulse, deltaT2Pulse, deltaT3Pulse };

            double velmmPs = Convert.ToDouble(textBox10.Text);
            double velPulsePms = 72 * velmmPs / 1000 / 0.05625;     //窗口输入值 mm/s 转化成 pulse/ms
            double tgvel = velPulsePms;                             //目标速度
            double endvel = 0;                                      //末端速度

            int wait = 0;
            IMC_Pkg.PKG_IMC_Line_Dist(Global.g_handle, dist, axisnum, tgvel, endvel, wait, fifo);

            //while (IMC_Pkg.PKG_IMC_isPstop(Global.g_handle, fifo) == 0) { }

            textBox7.Text = x2.ToString();
            textBox8.Text = y2.ToString();
            textBox9.Text = z2.ToString();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double distance = Convert.ToDouble(textBox11.Text);
            double x2 = x1;                                  //x轴寸动
            double y2 = y1;
            double z2 = z1 - distance;
            var (deltaT1Pulse, deltaT2Pulse, deltaT3Pulse) = robotarm.CoordinateConvertPulse(x1, y1, z1, x2, y2, z2);

            //启用插补空间
            int[] axis = new int[] { 0, 1, 2 };
            int axisnum = 3;
            int fifo;
            fifo = (int)FIFO_SEL.SEL_PFIFO1;

            IMC_Pkg.PKG_IMC_PFIFOclear(Global.g_handle, fifo);              //清除插补空间中未被执行的插补指令
            IMC_Pkg.PKG_IMC_AxisMap(Global.g_handle, axis, axisnum, fifo);  //轴映射
            IMC_Pkg.PKG_IMC_PFIFOrun(Global.g_handle, fifo);                //启用插补空间

            int mode = 0;
            IMC_Pkg.PKG_IMC_SetPFIFOvelMode(Global.g_handle, mode, fifo);   //插补速度控制模式

            int accel = 6;
            IMC_Pkg.PKG_IMC_SetPFIFOaccel(Global.g_handle, accel, fifo);    //插补加速度

            //开始相对运动直线插补
            int[] dist = new int[] { deltaT1Pulse, deltaT2Pulse, deltaT3Pulse };

            double velmmPs = Convert.ToDouble(textBox10.Text);
            double velPulsePms = 72 * velmmPs / 1000 / 0.05625;     //窗口输入值 mm/s 转化成 pulse/ms
            double tgvel = velPulsePms;                             //目标速度
            double endvel = 0;                                      //末端速度

            int wait = 0;
            IMC_Pkg.PKG_IMC_Line_Dist(Global.g_handle, dist, axisnum, tgvel, endvel, wait, fifo);

            //while (IMC_Pkg.PKG_IMC_isPstop(Global.g_handle, fifo) == 0) { }

            textBox7.Text = x2.ToString();
            textBox8.Text = y2.ToString();
            textBox9.Text = z2.ToString();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double distance = Convert.ToDouble(textBox11.Text);
            double x2 = x1;                                  //x轴寸动
            double y2 = y1;
            double z2 = z1 + distance;
            var (deltaT1Pulse, deltaT2Pulse, deltaT3Pulse) = robotarm.CoordinateConvertPulse(x1, y1, z1, x2, y2, z2);

            //启用插补空间
            int[] axis = new int[] { 0, 1, 2 };
            int axisnum = 3;
            int fifo;
            fifo = (int)FIFO_SEL.SEL_PFIFO1;

            IMC_Pkg.PKG_IMC_PFIFOclear(Global.g_handle, fifo);              //清除插补空间中未被执行的插补指令
            IMC_Pkg.PKG_IMC_AxisMap(Global.g_handle, axis, axisnum, fifo);  //轴映射
            IMC_Pkg.PKG_IMC_PFIFOrun(Global.g_handle, fifo);                //启用插补空间

            int mode = 0;
            IMC_Pkg.PKG_IMC_SetPFIFOvelMode(Global.g_handle, mode, fifo);   //插补速度控制模式

            int accel = 6;
            IMC_Pkg.PKG_IMC_SetPFIFOaccel(Global.g_handle, accel, fifo);    //插补加速度

            //开始相对运动直线插补
            int[] dist = new int[] { deltaT1Pulse, deltaT2Pulse, deltaT3Pulse };

            double velmmPs = Convert.ToDouble(textBox10.Text);
            double velPulsePms = 72 * velmmPs / 1000 / 0.05625;     //窗口输入值 mm/s 转化成 pulse/ms
            double tgvel = velPulsePms;                             //目标速度
            double endvel = 0;                                      //末端速度

            int wait = 0;
            IMC_Pkg.PKG_IMC_Line_Dist(Global.g_handle, dist, axisnum, tgvel, endvel, wait, fifo);

            //while (IMC_Pkg.PKG_IMC_isPstop(Global.g_handle, fifo) == 0) { }

            textBox7.Text = x2.ToString();
            textBox8.Text = y2.ToString();
            textBox9.Text = z2.ToString();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            IMC_Pkg.PKG_IMC_Emstop(Global.g_handle, 0);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button9_MouseDown(object sender, MouseEventArgs e)
        {
            flag = 1;
            while (flag == 1) ;
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double distance = Convert.ToDouble(textBox11.Text);
            double x2 = x1 + 5;                                  //x轴寸动
            double y2 = y1;
            double z2 = z1;
            var (deltaT1Pulse, deltaT2Pulse, deltaT3Pulse) = robotarm.CoordinateConvertPulse(x1, y1, z1, x2, y2, z2);

            //启用插补空间
            int[] axis = new int[] { 0, 1, 2 };
            int axisnum = 3;
            int fifo;
            fifo = (int)FIFO_SEL.SEL_PFIFO1;

            IMC_Pkg.PKG_IMC_PFIFOclear(Global.g_handle, fifo);              //清除插补空间中未被执行的插补指令
            IMC_Pkg.PKG_IMC_AxisMap(Global.g_handle, axis, axisnum, fifo);  //轴映射
            IMC_Pkg.PKG_IMC_PFIFOrun(Global.g_handle, fifo);                //启用插补空间

            int mode = 0;
            IMC_Pkg.PKG_IMC_SetPFIFOvelMode(Global.g_handle, mode, fifo);   //插补速度控制模式

            int accel = 6;
            IMC_Pkg.PKG_IMC_SetPFIFOaccel(Global.g_handle, accel, fifo);    //插补加速度

            //开始相对运动直线插补
            int[] dist = new int[] { deltaT1Pulse, deltaT2Pulse, deltaT3Pulse };

            double velmmPs = Convert.ToDouble(textBox10.Text);
            double velPulsePms = 72 * velmmPs / 1000 / 0.05625;     //窗口输入值 mm/s 转化成 pulse/ms
            double tgvel = velPulsePms;                             //目标速度
            double endvel = 0;                                      //末端速度

            int wait = 0;
            IMC_Pkg.PKG_IMC_Line_Dist(Global.g_handle, dist, axisnum, tgvel, endvel, wait, fifo);

            //while (IMC_Pkg.PKG_IMC_isPstop(Global.g_handle, fifo) == 0) { }

            textBox7.Text = x2.ToString();
            textBox8.Text = y2.ToString();
            textBox9.Text = z2.ToString();
            flag = 0;
        }

        private void button18_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button9_MouseUp(object sender, MouseEventArgs e)
        {
            flag = 0;
        }

        private void button10_MouseDown(object sender, MouseEventArgs e)
        {
            flag = 1;
            while (flag == 1) ;
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double distance = Convert.ToDouble(textBox11.Text);
            double x2 = x1 - 5;                                  //x轴寸动
            double y2 = y1;
            double z2 = z1;
            var (deltaT1Pulse, deltaT2Pulse, deltaT3Pulse) = robotarm.CoordinateConvertPulse(x1, y1, z1, x2, y2, z2);

            //启用插补空间
            int[] axis = new int[] { 0, 1, 2 };
            int axisnum = 3;
            int fifo;
            fifo = (int)FIFO_SEL.SEL_PFIFO1;

            IMC_Pkg.PKG_IMC_PFIFOclear(Global.g_handle, fifo);              //清除插补空间中未被执行的插补指令
            IMC_Pkg.PKG_IMC_AxisMap(Global.g_handle, axis, axisnum, fifo);  //轴映射
            IMC_Pkg.PKG_IMC_PFIFOrun(Global.g_handle, fifo);                //启用插补空间

            int mode = 0;
            IMC_Pkg.PKG_IMC_SetPFIFOvelMode(Global.g_handle, mode, fifo);   //插补速度控制模式

            int accel = 6;
            IMC_Pkg.PKG_IMC_SetPFIFOaccel(Global.g_handle, accel, fifo);    //插补加速度

            //开始相对运动直线插补
            int[] dist = new int[] { deltaT1Pulse, deltaT2Pulse, deltaT3Pulse };

            double velmmPs = Convert.ToDouble(textBox10.Text);
            double velPulsePms = 72* velmmPs / 1000 / 0.05625;     //窗口输入值 mm/s 转化成 pulse/ms
            double tgvel = velPulsePms;                             //目标速度
            double endvel = 0;                                      //末端速度

            int wait = 0;
            IMC_Pkg.PKG_IMC_Line_Dist(Global.g_handle, dist, axisnum, tgvel, endvel, wait, fifo);

            //while (IMC_Pkg.PKG_IMC_isPstop(Global.g_handle, fifo) == 0) { }

            textBox7.Text = x2.ToString();
            textBox8.Text = y2.ToString();
            textBox9.Text = z2.ToString();
            flag = 0;
        }

        private void button10_MouseUp(object sender, MouseEventArgs e)
        {
            flag = 0;
        }

        private void button11_MouseDown(object sender, MouseEventArgs e)
        {
            flag = 1;
            while (flag == 1) ;
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double distance = Convert.ToDouble(textBox11.Text);
            double x2 = x1;                                  //x轴寸动
            double y2 = y1 + 5 ;
            double z2 = z1;
            var (deltaT1Pulse, deltaT2Pulse, deltaT3Pulse) = robotarm.CoordinateConvertPulse(x1, y1, z1, x2, y2, z2);

            //启用插补空间
            int[] axis = new int[] { 0, 1, 2 };
            int axisnum = 3;
            int fifo;
            fifo = (int)FIFO_SEL.SEL_PFIFO1;

            IMC_Pkg.PKG_IMC_PFIFOclear(Global.g_handle, fifo);              //清除插补空间中未被执行的插补指令
            IMC_Pkg.PKG_IMC_AxisMap(Global.g_handle, axis, axisnum, fifo);  //轴映射
            IMC_Pkg.PKG_IMC_PFIFOrun(Global.g_handle, fifo);                //启用插补空间

            int mode = 0;
            IMC_Pkg.PKG_IMC_SetPFIFOvelMode(Global.g_handle, mode, fifo);   //插补速度控制模式

            int accel = 6;
            IMC_Pkg.PKG_IMC_SetPFIFOaccel(Global.g_handle, accel, fifo);    //插补加速度

            //开始相对运动直线插补
            int[] dist = new int[] { deltaT1Pulse, deltaT2Pulse, deltaT3Pulse };

            double velmmPs = Convert.ToDouble(textBox10.Text);
            double velPulsePms = 72 * velmmPs / 1000 / 0.05625;     //窗口输入值 mm/s 转化成 pulse/ms
            double tgvel = velPulsePms;                             //目标速度
            double endvel = 0;                                      //末端速度

            int wait = 0;
            IMC_Pkg.PKG_IMC_Line_Dist(Global.g_handle, dist, axisnum, tgvel, endvel, wait, fifo);

            //while (IMC_Pkg.PKG_IMC_isPstop(Global.g_handle, fifo) == 0) { }

            textBox7.Text = x2.ToString();
            textBox8.Text = y2.ToString();
            textBox9.Text = z2.ToString();
            flag = 0;
        }

        private void button11_MouseUp(object sender, MouseEventArgs e)
        {
            flag = 0;
        }

        private void button12_MouseDown(object sender, MouseEventArgs e)
        {
            flag = 1;
            while (flag == 1) ;
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double distance = Convert.ToDouble(textBox11.Text);
            double x2 = x1;                                  //x轴寸动
            double y2 = y1-5;
            double z2 = z1;
            var (deltaT1Pulse, deltaT2Pulse, deltaT3Pulse) = robotarm.CoordinateConvertPulse(x1, y1, z1, x2, y2, z2);

            //启用插补空间
            int[] axis = new int[] { 0, 1, 2 };
            int axisnum = 3;
            int fifo;
            fifo = (int)FIFO_SEL.SEL_PFIFO1;

            IMC_Pkg.PKG_IMC_PFIFOclear(Global.g_handle, fifo);              //清除插补空间中未被执行的插补指令
            IMC_Pkg.PKG_IMC_AxisMap(Global.g_handle, axis, axisnum, fifo);  //轴映射
            IMC_Pkg.PKG_IMC_PFIFOrun(Global.g_handle, fifo);                //启用插补空间

            int mode = 0;
            IMC_Pkg.PKG_IMC_SetPFIFOvelMode(Global.g_handle, mode, fifo);   //插补速度控制模式

            int accel = 6;
            IMC_Pkg.PKG_IMC_SetPFIFOaccel(Global.g_handle, accel, fifo);    //插补加速度

            //开始相对运动直线插补
            int[] dist = new int[] { deltaT1Pulse, deltaT2Pulse, deltaT3Pulse };

            double velmmPs = Convert.ToDouble(textBox10.Text);
            double velPulsePms = 72 * velmmPs / 1000 / 0.05625;     //窗口输入值 mm/s 转化成 pulse/ms
            double tgvel = velPulsePms;                             //目标速度
            double endvel = 0;                                      //末端速度

            int wait = 0;
            IMC_Pkg.PKG_IMC_Line_Dist(Global.g_handle, dist, axisnum, tgvel, endvel, wait, fifo);

            //while (IMC_Pkg.PKG_IMC_isPstop(Global.g_handle, fifo) == 0) { }

            textBox7.Text = x2.ToString();
            textBox8.Text = y2.ToString();
            textBox9.Text = z2.ToString();
            flag = 0;
        }

        private void button12_MouseUp(object sender, MouseEventArgs e)
        {
            flag = 0;
        }

        private void button13_MouseDown(object sender, MouseEventArgs e)
        {
            flag = 1;
            while (flag == 1) ;
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double distance = Convert.ToDouble(textBox11.Text);
            double x2 = x1;                                  //x轴寸动
            double y2 = y1;
            double z2 = z1 + 5;
            var (deltaT1Pulse, deltaT2Pulse, deltaT3Pulse) = robotarm.CoordinateConvertPulse(x1, y1, z1, x2, y2, z2);

            //启用插补空间
            int[] axis = new int[] { 0, 1, 2 };
            int axisnum = 3;
            int fifo;
            fifo = (int)FIFO_SEL.SEL_PFIFO1;

            IMC_Pkg.PKG_IMC_PFIFOclear(Global.g_handle, fifo);              //清除插补空间中未被执行的插补指令
            IMC_Pkg.PKG_IMC_AxisMap(Global.g_handle, axis, axisnum, fifo);  //轴映射
            IMC_Pkg.PKG_IMC_PFIFOrun(Global.g_handle, fifo);                //启用插补空间

            int mode = 0;
            IMC_Pkg.PKG_IMC_SetPFIFOvelMode(Global.g_handle, mode, fifo);   //插补速度控制模式

            int accel = 6;
            IMC_Pkg.PKG_IMC_SetPFIFOaccel(Global.g_handle, accel, fifo);    //插补加速度

            //开始相对运动直线插补
            int[] dist = new int[] { deltaT1Pulse, deltaT2Pulse, deltaT3Pulse };

            double velmmPs = Convert.ToDouble(textBox10.Text);
            double velPulsePms = 72 * velmmPs / 1000 / 0.05625;     //窗口输入值 mm/s 转化成 pulse/ms
            double tgvel = velPulsePms;                             //目标速度
            double endvel = 0;                                      //末端速度

            int wait = 0;
            IMC_Pkg.PKG_IMC_Line_Dist(Global.g_handle, dist, axisnum, tgvel, endvel, wait, fifo);

            //while (IMC_Pkg.PKG_IMC_isPstop(Global.g_handle, fifo) == 0) { }

            textBox7.Text = x2.ToString();
            textBox8.Text = y2.ToString();
            textBox9.Text = z2.ToString();
            flag = 0;
        }

        private void button13_MouseUp(object sender, MouseEventArgs e)
        {
            flag = 0;
        }

        private void button14_MouseDown(object sender, MouseEventArgs e)
        {
            flag = 1;
            while (flag == 1) ;
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double distance = Convert.ToDouble(textBox11.Text);
            double x2 = x1;                                  //x轴寸动
            double y2 = y1;
            double z2 = z1 - 5;
            var (deltaT1Pulse, deltaT2Pulse, deltaT3Pulse) = robotarm.CoordinateConvertPulse(x1, y1, z1, x2, y2, z2);

            //启用插补空间
            int[] axis = new int[] { 0, 1, 2 };
            int axisnum = 3;
            int fifo;
            fifo = (int)FIFO_SEL.SEL_PFIFO1;

            IMC_Pkg.PKG_IMC_PFIFOclear(Global.g_handle, fifo);              //清除插补空间中未被执行的插补指令
            IMC_Pkg.PKG_IMC_AxisMap(Global.g_handle, axis, axisnum, fifo);  //轴映射
            IMC_Pkg.PKG_IMC_PFIFOrun(Global.g_handle, fifo);                //启用插补空间

            int mode = 0;
            IMC_Pkg.PKG_IMC_SetPFIFOvelMode(Global.g_handle, mode, fifo);   //插补速度控制模式

            int accel = 6;
            IMC_Pkg.PKG_IMC_SetPFIFOaccel(Global.g_handle, accel, fifo);    //插补加速度

            //开始相对运动直线插补
            int[] dist = new int[] { deltaT1Pulse, deltaT2Pulse, deltaT3Pulse };

            double velmmPs = Convert.ToDouble(textBox10.Text);
            double velPulsePms = 72 * velmmPs / 1000 / 0.05625;     //窗口输入值 mm/s 转化成 pulse/ms
            double tgvel = velPulsePms;                             //目标速度
            double endvel = 0;                                      //末端速度

            int wait = 0;
            IMC_Pkg.PKG_IMC_Line_Dist(Global.g_handle, dist, axisnum, tgvel, endvel, wait, fifo);

            //while (IMC_Pkg.PKG_IMC_isPstop(Global.g_handle, fifo) == 0) { }

            textBox7.Text = x2.ToString();
            textBox8.Text = y2.ToString();
            textBox9.Text = z2.ToString();
            flag = 0;
        }

        private void button14_MouseUp(object sender, MouseEventArgs e)
        {
            flag = 0;
        }

        private void textBox10_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button25_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < 3; i++)
            {
                IMC_Pkg.PKG_IMC_ClearIMC(Global.g_handle);
                IMC_Pkg.PKG_IMC_ClearAxis(Global.g_handle, i);
                IMC_Pkg.PKG_IMC_SetPulWidth(Global.g_handle, 4000, i);            //4000:有效电平脉冲宽度
                IMC_Pkg.PKG_IMC_SetPulPolar(Global.g_handle, 1, 1, i);
                IMC_Pkg.PKG_IMC_SetEncpEna(Global.g_handle, 0, i);
                IMC_Pkg.PKG_IMC_SetVelAccLim(Global.g_handle, 200, 200, i);   //极限速度、极限加速度
                IMC_Pkg.PKG_IMC_Setlimit(Global.g_handle, 1, 0, 1, 0, i);
                IMC_Pkg.PKG_IMC_SetAlm(Global.g_handle, 0, 0, i);
                IMC_Pkg.PKG_IMC_SetEmstopPolar(Global.g_handle, 0);             //急停输入端有效极性，0为低电平有效
                IMC_Pkg.PKG_IMC_SetStopfilt(Global.g_handle, 1, i);
                IMC_Pkg.PKG_IMC_SetExitfilt(Global.g_handle, 0, i);
                IMC_Pkg.PKG_IMC_SetEna(Global.g_handle, 1, i);
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double distance = 50.00;
            double x2 = x1;                                  //x轴寸动
            double y2 = y1;
            double z2 = z1 + distance;
            var (deltaT1Pulse, deltaT2Pulse, deltaT3Pulse) = robotarm.CoordinateConvertPulse(x1, y1, z1, x2, y2, z2);


            double x3 = x2 - 20;
            double y3 = y2 + 20;
            double z3 = z2;
            var (deltaT11Pulse, deltaT12Pulse, deltaT13Pulse) = robotarm.CoordinateConvertPulse(x2, y2, z2, x3, y3, z3);

            ////

            double x04 = x3;
            double y04 = y3;
            double z04 = z3+5;
            var (deltaT021Pulse, deltaT022Pulse, deltaT023Pulse) = robotarm.CoordinateConvertPulse(x3, y3, z3, x04, y04, z04);

            /////
            ///
            double x4 = x04 +40;
            double y4 = y04 ;
            double z4 = z04;
            var (deltaT21Pulse, deltaT22Pulse, deltaT23Pulse) = robotarm.CoordinateConvertPulse(x04, y04, z04, x4, y4, z4);

            double x5 = x4;
            double y5 = y4-40;
            double z5 = z4;
            var (deltaT31Pulse, deltaT32Pulse, deltaT33Pulse) = robotarm.CoordinateConvertPulse(x4, y4, z4, x5, y5, z5);

            double x6 = x5 -40;
            double y6 = y5;
            double z6 = z5;
            var (deltaT41Pulse, deltaT42Pulse, deltaT43Pulse) = robotarm.CoordinateConvertPulse(x5, y5, z5, x6, y6, z6);

            double x7 = x6;
            double y7 = y6+40;
            double z7 = z6;
            var (deltaT51Pulse, deltaT52Pulse, deltaT53Pulse) = robotarm.CoordinateConvertPulse(x6, y6, z6, x7, y7, z7);

            /////
            double x07 = x7;
            double y07 = y7;
            double z07 = z7-5;
            var (deltaT051Pulse, deltaT052Pulse, deltaT053Pulse) = robotarm.CoordinateConvertPulse(x7, y7, z7, x07, y07, z07);

            double x8 = x7+20;
            double y8 = y7 -20;
            double z8 = z07;
            var (deltaT61Pulse, deltaT62Pulse, deltaT63Pulse) = robotarm.CoordinateConvertPulse(x07, y07, z07, x8, y8, z8);


            long[,] dist2D = new long[8, 3] // 5段路径，每个轴的移动距离
{
    // x1, y1, z1 到 x2, y2, z2
    //{deltaT1Pulse, deltaT2Pulse, deltaT3Pulse},
    // x2, y2, z2 到 x3, y3, z3
    {deltaT11Pulse, deltaT12Pulse, deltaT13Pulse},

    //
    {deltaT021Pulse, deltaT022Pulse, deltaT023Pulse },


    // x3, y3, z3 到 x4, y4, z4
    {deltaT21Pulse, deltaT22Pulse, deltaT23Pulse},
    // x4, y4, z4 到 x5, y5, z5
    {deltaT31Pulse, deltaT32Pulse, deltaT33Pulse},
    // x5, y5, z5 到 x6, y6, z6
    {deltaT41Pulse, deltaT42Pulse, deltaT43Pulse},
    // x6, y6, z6 到 x7, y7, z7
    {deltaT51Pulse, deltaT52Pulse, deltaT53Pulse},


    //

    { deltaT051Pulse, deltaT052Pulse, deltaT053Pulse},

    {deltaT61Pulse, deltaT62Pulse, deltaT63Pulse }
};


            int[] dist = new int[dist2D.Length];
            for (int i = 0; i < dist2D.GetLength(0); i++)
            {
                for (int j = 0; j < dist2D.GetLength(1); j++)
                {
                    dist[i * dist2D.GetLength(1) + j] = (int)dist2D[i, j];
                }
            }




            //启用插补空间
            int[] axis = new int[] { 0, 1, 2 };
            int axisnum = 3;
            int fifo;
            fifo = (int)FIFO_SEL.SEL_PFIFO1;

            IMC_Pkg.PKG_IMC_PFIFOclear(Global.g_handle, fifo);              //清除插补空间中未被执行的插补指令
            IMC_Pkg.PKG_IMC_AxisMap(Global.g_handle, axis, axisnum, fifo);  //轴映射
            IMC_Pkg.PKG_IMC_PFIFOrun(Global.g_handle, fifo);                //启用插补空间

            int mode = 0;
            IMC_Pkg.PKG_IMC_SetPFIFOvelMode(Global.g_handle, mode, fifo);   //插补速度控制模式

            int accel = 6;
            IMC_Pkg.PKG_IMC_SetPFIFOaccel(Global.g_handle, accel, fifo);    //插补加速度


            double velmmPs = 10.00;
            double velPulsePms = 72 * velmmPs / 1000 / 0.05625;     //窗口输入值 mm/s 转化成 pulse/ms
            double tgvel = velPulsePms;                             //目标速度
            double endvel = 0;                                      //末端速度

            int wait = 0;

            int segNum = 8;


            IMC_Pkg.PKG_IMC_MulLine_Dist(Global.g_handle, dist, axisnum, segNum,tgvel, endvel, wait, fifo);


        }

        private void whiteboard_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button28_Click(object sender, EventArgs e)
        {
            whiteboardControl.StopRecording(); // 停止记录
            MessageBox.Show("轨迹已停止记录并保存。");
        }

        private void button27_Click(object sender, EventArgs e)
        {
            whiteboard.StartRecording(); // 开始记录轨迹
            MessageBox.Show("开始记录！");
        }

        private void button29_Click(object sender, EventArgs e)
        {
            //whiteboard.get_points();
            run_points();
            //textBox12.Text = Point_.ToString();
            
        }


        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        public void run_points()
        {
            var coor = whiteboard.get_paths();
            long[,] dist2D = new long[coor.Count, 3];


            var (x0, y0, z0) = coor[0];
            Console.WriteLine($"1 {x0} {y0}");

            var (deltaT00Pulse, deltaT01Pulse, deltaT02Pulse) = robotarm.CoordinateConvertPulse(0,0,z0, x0, y0, z0);

            dist2D[0, 0] = deltaT00Pulse;
            dist2D[0, 1] = deltaT01Pulse;
            dist2D[0, 2] = deltaT02Pulse;


            for (int i = 0; i < coor.Count - 1; i++)
            {
                var (x01, y01, z01) = coor[i];

                Console.WriteLine($"{x01} {y01}");
                var (x02, y02, z02) = coor[i + 1];

                var (deltaT0Pulse, deltaT1Pulse, deltaT2Pulse) = robotarm.CoordinateConvertPulse(x01, y01, z01, x02, y02, z02);


                dist2D[i+1, 0] = deltaT0Pulse;
                dist2D[i+1, 1] = deltaT1Pulse;
                dist2D[i + 1, 2] = deltaT2Pulse;


            }
            int[] dist = new int[dist2D.Length];
            for (int i = 0; i < dist2D.GetLength(0); i++)
            {
                for (int j = 0; j < dist2D.GetLength(1); j++)
                {
                    dist[i * dist2D.GetLength(1) + j] = (int)dist2D[i, j];
                }
            }

            var rowCount=dist2D.GetLength(0);
            Console.WriteLine($"allpath is {rowCount}");

            //启用插补空间
            int[] axis = new int[] { 0, 1, 2 };
            int axisnum = 3;
            int fifo;
            fifo = (int)FIFO_SEL.SEL_PFIFO1;

            IMC_Pkg.PKG_IMC_PFIFOclear(Global.g_handle, fifo);              //清除插补空间中未被执行的插补指令
            IMC_Pkg.PKG_IMC_AxisMap(Global.g_handle, axis, axisnum, fifo);  //轴映射
            IMC_Pkg.PKG_IMC_PFIFOrun(Global.g_handle, fifo);                //启用插补空间

            int mode = 0;
            IMC_Pkg.PKG_IMC_SetPFIFOvelMode(Global.g_handle, mode, fifo);   //插补速度控制模式

            int accel = 6;
            IMC_Pkg.PKG_IMC_SetPFIFOaccel(Global.g_handle, accel, fifo);    //插补加速度


            double velmmPs = 10.00;
            double velPulsePms = 72 * velmmPs / 1000 / 0.05625;     //窗口输入值 mm/s 转化成 pulse/ms
            double tgvel = velPulsePms;                             //目标速度
            double endvel = 0;                                      //末端速度

            int wait = 0;

            int segNum = rowCount;


            IMC_Pkg.PKG_IMC_MulLine_Dist(Global.g_handle, dist, axisnum, segNum, tgvel, endvel, wait, fifo);
        }

        private void whiteboard_Paint_1(object sender, PaintEventArgs e)
        {
             
        }
    }
}


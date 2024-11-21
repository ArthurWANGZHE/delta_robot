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


    //初始化设置

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

        //归零
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


        //归零复位
        private void button24_Click(object sender, EventArgs e)
        {
            IMC_Pkg.PKG_IMC_Emstop(Global.g_handle, 0);
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



        //默认配置脉冲宽度
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








        //没用的初始化

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

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

        private void button18_Click_1(object sender, EventArgs e)
        {


        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }




        //点到点运动
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



        //0轴正向运动
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


        //0轴停止
        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            int axis = 0;                                           
            IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, 0, 0, axis);   
        }

        //0轴负向运动
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


        //0轴停止
        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            int axis = 0;
            IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, 0, 0, axis);
        }


        //1轴正向运动
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


        //1轴停止
        private void button5_MouseUp(object sender, MouseEventArgs e)
        {
            int axis = 1;
            IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, 0, 0, axis);

        }


        //1轴负向运动
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


        //1轴停止
        private void button6_MouseUp(object sender, MouseEventArgs e)
        {
            int axis = 1;
            IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, 0, 0, axis);
        }


        //2轴正向运动
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


        //2轴停止
        private void button7_MouseUp(object sender, MouseEventArgs e)
        {
            int axis = 2;
            IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, 0, 0, axis);

        }


        //2轴负向运动
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


        //2轴停止
        private void button8_MouseUp(object sender, MouseEventArgs e)
        {
            int axis = 2;
            IMC_Pkg.PKG_IMC_MoveVel(Global.g_handle, 0, 0, axis);

        }


        //末端执行器x轴寸动_正向
        private void button18_Click(object sender, EventArgs e)
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


        //末端执行器x轴寸动_负向
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


        //末端执行器y轴寸动_正向
        private void button19_Click(object sender, EventArgs e)
        {
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double distance = Convert.ToDouble(textBox11.Text);
            double x2 = x1;                                  
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


        //末端执行器y轴寸动_负向
        private void button22_Click(object sender, EventArgs e)
        {
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double distance = Convert.ToDouble(textBox11.Text);
            double x2 = x1;                                  
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


        //末端执行器z轴寸动_正向
        private void button20_Click(object sender, EventArgs e)
        {
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double distance = Convert.ToDouble(textBox11.Text);
            double x2 = x1;                                  
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


        //末端执行器z轴寸动_负向
        private void button23_Click(object sender, EventArgs e)
        {
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);
            double distance = Convert.ToDouble(textBox11.Text);
            double x2 = x1;                                  
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


        //末端执行器x轴点动_正向
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


        //末端执行器x轴点动_停止
        private void button9_MouseUp(object sender, MouseEventArgs e)
        {
            flag = 0;
        }


        //末端执行器x轴点动_负向
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


        //末端执行器x轴点动_停止
        private void button10_MouseUp(object sender, MouseEventArgs e)
        {
            flag = 0;
        }


        //末端执行器y轴点动_正向
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


        //末端执行器y轴点动_停止
        private void button11_MouseUp(object sender, MouseEventArgs e)
        {
            flag = 0;
        }


        //末端执行器y轴点动_负向
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


        //末端执行器y轴点动_停止
        private void button12_MouseUp(object sender, MouseEventArgs e)
        {
            flag = 0;
        }


        //末端执行器z轴点动_正向
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


        //末端执行器z轴点动_停止
        private void button13_MouseUp(object sender, MouseEventArgs e)
        {
            flag = 0;
        }


        //末端执行器z轴点动_负向
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


        //末端执行器z轴点动_停止
        private void button14_MouseUp(object sender, MouseEventArgs e)
        {
            flag = 0;
        }



        //正方形轨迹

        private void button26_Click(object sender, EventArgs e)
        {
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = Convert.ToDouble(textBox9.Text);


            //第一段路径移动到初始点
            double x2 = x1;                                  
            double y2 = y1;
            double z2 = z1 + 50;
            var (deltaT1Pulse, deltaT2Pulse, deltaT3Pulse) = robotarm.CoordinateConvertPulse(x1, y1, z1, x2, y2, z2);


            //第二段路径移动到第一个点
            double x3 = x2 - 20;
            double y3 = y2 + 20;
            double z3 = z2;
            var (deltaT11Pulse, deltaT12Pulse, deltaT13Pulse) = robotarm.CoordinateConvertPulse(x2, y2, z2, x3, y3, z3);

            
            //下降5mm补偿
            double x04 = x3;
            double y04 = y3;
            double z04 = z3+5;
            var (deltaT021Pulse, deltaT022Pulse, deltaT023Pulse) = robotarm.CoordinateConvertPulse(x3, y3, z3, x04, y04, z04);


            //第三段路径移动到第二个点
            double x4 = x04 +40;
            double y4 = y04 ;
            double z4 = z04;
            var (deltaT21Pulse, deltaT22Pulse, deltaT23Pulse) = robotarm.CoordinateConvertPulse(x04, y04, z04, x4, y4, z4);


            //第四段路径移动到第三个点
            double x5 = x4;
            double y5 = y4-40;
            double z5 = z4;
            var (deltaT31Pulse, deltaT32Pulse, deltaT33Pulse) = robotarm.CoordinateConvertPulse(x4, y4, z4, x5, y5, z5);


            //第五段路径移动到第四个点
            double x6 = x5 -40;
            double y6 = y5;
            double z6 = z5;
            var (deltaT41Pulse, deltaT42Pulse, deltaT43Pulse) = robotarm.CoordinateConvertPulse(x5, y5, z5, x6, y6, z6);


            //第六段路径移动到第五个点(初始点)
            double x7 = x6;
            double y7 = y6+40;
            double z7 = z6;
            var (deltaT51Pulse, deltaT52Pulse, deltaT53Pulse) = robotarm.CoordinateConvertPulse(x6, y6, z6, x7, y7, z7);


            //上升5mm补偿
            double x07 = x7;
            double y07 = y7;
            double z07 = z7-5;
            var (deltaT051Pulse, deltaT052Pulse, deltaT053Pulse) = robotarm.CoordinateConvertPulse(x7, y7, z7, x07, y07, z07);


            //回到原点
            double x8 = x7 + 20;
            double y8 = y7 - 20;
            double z8 = z07;
            var (deltaT61Pulse, deltaT62Pulse, deltaT63Pulse) = robotarm.CoordinateConvertPulse(x07, y07, z07, x8, y8, z8);


            long[,] dist2D = new long[8, 3] // 5段路径，每个轴的移动距离
                {

                    {deltaT11Pulse, deltaT12Pulse, deltaT13Pulse},

                    {deltaT021Pulse, deltaT022Pulse, deltaT023Pulse},

                    {deltaT21Pulse, deltaT22Pulse, deltaT23Pulse},

                    {deltaT31Pulse, deltaT32Pulse, deltaT33Pulse},

                    {deltaT41Pulse, deltaT42Pulse, deltaT43Pulse},

                    {deltaT51Pulse, deltaT52Pulse, deltaT53Pulse},

                    { deltaT051Pulse, deltaT052Pulse, deltaT053Pulse},

                    {deltaT61Pulse, deltaT62Pulse, deltaT63Pulse }
                };

            //将二维数组转化为一维数组
            int[] dist = new int[dist2D.Length];
            for (int i = 0; i < dist2D.GetLength(0); i++)
            {
                for (int j = 0; j < dist2D.GetLength(1); j++)
                {
                    dist[i * dist2D.GetLength(1) + j] = (int)dist2D[i, j];
                }
            }


            //多段轨迹插补

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




 
        //白板逻辑


        private void whiteboard_Paint(object sender, PaintEventArgs e)
        {

        }


        //开始记录
        private void button27_Click(object sender, EventArgs e)
        {
            whiteboard.StartRecording(); // 开始记录轨迹
            MessageBox.Show("开始记录！");
        }


        //停止记录
        private void button28_Click(object sender, EventArgs e)
        {
            whiteboardControl.StopRecording(); // 停止记录
            MessageBox.Show("轨迹已停止记录并保存。");
        }


        //执行轨迹
        private void button29_Click(object sender, EventArgs e)
        {
            //whiteboard.get_points();
            //whiteboard.get_multi_paths();
            //run_points();
            //textBox12.Text = Point_.ToString();
            run_multi_path();


        }



        //运行轨迹
        public void run_points()
        {

            //获取白板轨迹
            var coor = whiteboard.get_paths();
            long[,] dist2D = new long[coor.Count, 3];

            //获取第一个点,从原点到第一个点的距离
            var (x0, y0, z0) = coor[0];
            double x1 = Convert.ToDouble(textBox7.Text);
            double y1 = Convert.ToDouble(textBox8.Text);
            double z1 = 60.00;

            //Console.WriteLine($" {x0} {y0}");

            var (deltaT00Pulse, deltaT01Pulse, deltaT02Pulse) = robotarm.CoordinateConvertPulse(x1,y1,z1, x0, y0, z1);

            dist2D[0, 0] = deltaT00Pulse;
            dist2D[0, 1] = deltaT01Pulse;
            dist2D[0, 2] = deltaT02Pulse;


            //通过for循环,计算后续点之间距离
            for (int i = 0; i < coor.Count - 1; i++)
            {
                var (x01, y01, z01) = coor[i];
                var (x02, y02, z02) = coor[i + 1];

                //Console.WriteLine($"{x01} {y01}");
                

                var (deltaT0Pulse, deltaT1Pulse, deltaT2Pulse) = robotarm.CoordinateConvertPulse(x01, y01, z01, x02, y02, z02);

                //保存距离
                dist2D[i+1, 0] = deltaT0Pulse;
                dist2D[i+1, 1] = deltaT1Pulse;
                dist2D[i + 1, 2] = deltaT2Pulse;

            }

            //将二维数组转化为一维数组
            int[] dist = new int[dist2D.Length];

            Console.WriteLine($"dist is {dist2D.Length}");

            for (int i = 0; i < dist2D.GetLength(0); i++)
            {
                for (int j = 0; j < dist2D.GetLength(1); j++)
                {
                    dist[i * dist2D.GetLength(1) + j] = (int)dist2D[i, j];
                }
            }

            var rowCount=dist2D.GetLength(0);

            Console.WriteLine($"allpath is {rowCount}");




            //多段轨迹插补
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


            var (x2, y2, z2) = coor[coor.Count - 1];
            textBox7.Text = x2.ToString();
            textBox8.Text = y2.ToString();
            textBox9.Text = z2.ToString();
        }


        public void run_multi_path()
        {
            //获取多段路径
            var multi_path = whiteboard.get_multi_paths();
            int path_num = multi_path.Count;
            List<int> distList = new List<int>();

            Console.WriteLine("---开始运行---");

            //获取每段路径点
            for (int i =0; i<path_num;i++)
            {
                var coor = multi_path[i];
                long[,] dist2D = new long[coor.Count+2, 3];
                int[] dist = new int[dist2D.Length];

                //获取第一个点
                var (x0, y0, z0) = coor[0];

                Console.WriteLine($"第{i+1}段轨迹");
                

                //获取当前位置
                double x1 = Convert.ToDouble(textBox7.Text);
                double y1 = Convert.ToDouble(textBox8.Text);
                double z1 = Convert.ToDouble(textBox9.Text);

                Console.WriteLine($" {x1} {y1} {z1}");

                //z轴抬升
                var (deltaT000Pulse, deltaT001Pulse, deltaT002Pulse) = robotarm.CoordinateConvertPulse(x1, y1, z1, x1, y1, z1-5);

                dist2D[0, 0] = deltaT000Pulse;
                dist2D[0, 1] = deltaT001Pulse;
                dist2D[0, 2] = deltaT002Pulse;

                Console.WriteLine($" {x1} {y1} {z1+5}");

                //移动到第一个点
                var (deltaT010Pulse, deltaT011Pulse, deltaT012Pulse) = robotarm.CoordinateConvertPulse(x1, y1, z1-5, x0, y0, z1-5);

                dist2D[1,0] = deltaT010Pulse;
                dist2D[1, 1]=deltaT011Pulse;
                dist2D[1, 2] = deltaT012Pulse;

                Console.WriteLine($" {x0} {y0} {z1 + 5}");

                //落笔
                var (deltaT020Pulse, deltaT021Pulse, deltaT022Pulse) = robotarm.CoordinateConvertPulse(x0, y0, z1 - 5, x0, y0, z1);

                dist2D[2, 0] = deltaT020Pulse;
                dist2D[2, 1] = deltaT021Pulse;
                dist2D[2, 2] = deltaT022Pulse;

                Console.WriteLine("---落笔---");
                Console.WriteLine($" {x0} {y0} {z1}");

                //后续点的轨迹
                //通过for循环,计算后续点之间距离
                for (int j = 0; j < coor.Count-1; j++)
                {
                    var (x01, y01, z01) = coor[j];
                    var (x02, y02, z02) = coor[j + 1];

                    Console.WriteLine($" 第{j+1} {x01} {y01} {z1}");


                    var (deltaT0Pulse, deltaT1Pulse, deltaT2Pulse) = robotarm.CoordinateConvertPulse(x01, y01, z1, x02, y02, z1);

                    //保存距离
                    dist2D[j + 3, 0] = deltaT0Pulse;
                    dist2D[j + 3, 1] = deltaT1Pulse;
                    dist2D[j + 3, 2] = deltaT2Pulse;

                }

                Console.WriteLine($"dist is {dist2D.Length}");


                for (int k = 0; k < dist2D.GetLength(0); k++)
                {
                    for (int j = 0; j < dist2D.GetLength(1); j++)
                    {
                        dist[k * dist2D.GetLength(1) + j] = (int)dist2D[k, j];
                    }
                }

                var rowCount = dist2D.GetLength(0);

                var (x2, y2, z2) = coor[coor.Count - 1];
                textBox7.Text = x2.ToString();
                textBox8.Text = y2.ToString();
                textBox9.Text = z1.ToString();
                distList.AddRange(dist);
                //Console.WriteLine($"dist is {dist}");

            }



            //将list转换为数组

            int[] dist_all = distList.ToArray();


            //执行多段轨迹插补

            //多段轨迹插补
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
            int segNum = dist_all.Length/3;
            Console.WriteLine($"segNum {segNum}");

            IMC_Pkg.PKG_IMC_MulLine_Dist(Global.g_handle, dist_all, axisnum, segNum, tgvel, endvel, wait, fifo);


        }
    }
}


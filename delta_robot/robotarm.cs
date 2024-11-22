using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using static System.Resources.ResXFileRef;


namespace Delta_10._31_
{
    public class RobotArm
    {
        // 机器人手臂的参数
        public double BaseRadius { get; set; }  // 静平台的外接圆半径
        public double TopRadius { get; set; }   // 动平台的外接圆半径
      
        public double LinkLength { get; set; }  // 连杆长度
        public double TimeStep { get; set; }    // 时间步长
        public double MaxSpeed { get; set; }     // 最大速度
        public double MaxAcceleration { get; set; } // 最大加速度
        public double MaxJerk { get; set; }      // 最大加加速度
        public double MyDt { get; set; } //微分时间

        // 构造函数
        public RobotArm(double baseRadius, double topRadius, double linkLength, double timeStep, double maxSpeed,
                                                double maxAcceleration, double maxJerk, double mydt)
        {
            BaseRadius = baseRadius;
            TopRadius = topRadius;
            LinkLength = linkLength;
            TimeStep = timeStep;
            MaxSpeed = maxSpeed;
            MaxAcceleration = maxAcceleration;
            MaxJerk = maxJerk;
            MyDt = mydt;
        }

        //xyz到t123
        public (double t1, double t2, double t3) ForwardKinematics(double x, double y, double z)
        {
            // 根据动平台中心坐标计算滑块距离
            double R = BaseRadius - TopRadius;
            double l = LinkLength;
            double t3 = -z - Math.Sqrt(l * l - x * x - (y + R) * (y + R));
            double t2 = -z - Math.Sqrt(l * l - (x - R * Math.Sqrt(3) / 2) * (x - R * Math.Sqrt(3) / 2) - (R / 2 - y) * (R / 2 - y));
            double t1 = -z - Math.Sqrt(l * l - (Math.Sqrt(3) / 2 * R + x) * (Math.Sqrt(3) / 2 * R + x) - (R / 2 - y) * (R / 2 - y));

            return (-t1, -t2, -t3);
        }

        //t123到xyz

        public (double x, double y, double z) InverseKinematics(double t1, double t2, double t3)
        {
            double R1 = BaseRadius;  // 静平台的外接圆半径
            double R2 = TopRadius;   // 动平台的外接圆半径
            double R = R1 - R2;      // 三角锥法后移动到一点后的向xoy投影的三角形的半径

            float degree30 = 30f; // 将度数转换为float
            Vector3 OB = new Vector3(-(float)R * (float)Math.Cos(degree30 * (float)Math.PI / 180f), (float)R * (float)Math.Sin(degree30 * (float)Math.PI / 180f), -(float)t2);
            Vector3 OC = new Vector3((float)R * (float)Math.Cos(degree30 * (float)Math.PI / 180f), (float)R * (float)Math.Sin(degree30 * (float)Math.PI / 180f), -(float)t1);
            Vector3 OD = new Vector3(0f, -(float)R, -(float)t3);
            Vector3 OB2 = new Vector3(-(float)R * (float)Math.Cos(degree30 * (float)Math.PI / 180f), (float)R * (float)Math.Sin(degree30 * (float)Math.PI / 180f), 0f);
            Vector3 OC2 = new Vector3((float)R * (float)Math.Cos(degree30 * (float)Math.PI / 180f), (float)R * (float)Math.Sin(degree30 * (float)Math.PI / 180f), 0f);
            Vector3 OD2 = new Vector3(0f, -(float)R, 0f);

            Vector3 E = new Vector3(0, 0, -1); // 垂直的单位向量

            Vector3 pmB = Vector3.Cross(E, OB2); // 面法线
            Vector3 pmC = Vector3.Cross(E, OC2);
            Vector3 pmD = Vector3.Cross(E, OD2);

            double AB = LinkLength; // 定义AB为杆长

            Vector3 BC = OC - OB;
            Vector3 CD = OD - OC;
            Vector3 BD = OD - OB;

            double lbe = BC.Length() / 2;

            Vector3 BCCD = Vector3.Cross(BC, CD);
            Vector3 nef = Vector3.Cross(BCCD, BC) / (BCCD.Length() * BC.Length());

            double a = Math.Sqrt((t3 - t2) * (t3 - t2) + (Math.Sqrt(3) * R) * (Math.Sqrt(3) * R));
            double b = Math.Sqrt((t2 - t1) * (t2 - t1) + (Math.Sqrt(3) * R) * (Math.Sqrt(3) * R));
            double c = Math.Sqrt((t3 - t1) * (t3 - t1) + (Math.Sqrt(3) * R) * (Math.Sqrt(3) * R));

            double p = (a + b + c) / 2;
            double S = Math.Sqrt(p * (p - a) * (p - b) * (p - c));

            double lbf = a * b * c / (4 * S);

            float Lef = (float)(Math.Sqrt(lbf * lbf - lbe * lbe));
            Vector3 EF = new Vector3(Lef * nef.X, Lef * nef.Y, Lef * nef.Z);

            double Lfa = Math.Sqrt(AB * AB - lbf * lbf);

            Vector3 nfa = Vector3.Normalize(Vector3.Cross(BC, CD));
            Vector3 FA = new Vector3((float)Lfa * nfa.X, (float)Lfa * nfa.Y, (float)Lfa * nfa.Z);

            Vector3 OF = (OB + OC) / 2 + EF;

            Vector3 OA = OF + FA;

            Vector3 AB_vec = OB - OA;
            Vector3 AC_vec = OC - OA;
            Vector3 AD_vec = OD - OA;

            double cosbb = Vector3.Dot(pmB, AB_vec) / (Vector3.Normalize(pmB).Length() * Vector3.Normalize(AB_vec).Length());
            double coscc = Vector3.Dot(pmC, AC_vec) / (Vector3.Normalize(pmC).Length() * Vector3.Normalize(AC_vec).Length());
            double cosdd = Vector3.Dot(pmD, AD_vec) / (Vector3.Normalize(pmD).Length() * Vector3.Normalize(AD_vec).Length());

            double x = OA.X;
            double y = OA.Y;
            double z = OA.Z;

            return (x, y, z);
        }
        public (List<Vector3>, List<double>, List<double>, List<double>, List<double>) PointToPointSs(
       double x1, double y1, double z1,
       double x2, double y2, double z2)
        {
            Vector3 P1 = new Vector3((float)x1, (float)y1, (float)z1);
            Vector3 P2 = new Vector3((float)x2, (float)y2, (float)z2);

            double R1 = BaseRadius;
            double R2 = TopRadius;
            double R = R1 - R2;
            double l = LinkLength;
            double mydt = MyDt;
            double vmax = MaxSpeed;
            double amax = MaxAcceleration;
            double jmax = MaxJerk;

            double q1 = Vector3.Distance(P1, P2);
            double q0 = 0;
            double v0 = 0;
            double v1 = 0;

            double Tj1, Ta, alima, Tj2, Td, alimd, Tv;

            if ((vmax - v0) * jmax < amax * amax)
            {
                if (v0 > vmax)
                {
                    Tj1 = 0;
                    Ta = 0;
                    alima = 0;
                }
                else
                {
                    Tj1 = Math.Sqrt((vmax - v0) / jmax);
                    Ta = 2 * Tj1;
                    alima = Tj1 * jmax;
                }
            }
            else
            {
                Tj1 = amax / jmax;
                Ta = Tj1 + (vmax - v0) / amax;
                alima = amax;
            }

            if ((vmax - v1) * jmax < amax * amax)
            {
                Tj2 = Math.Sqrt((vmax - v1) / jmax);
                Td = 2 * Tj2;
                alimd = Tj2 * jmax;
            }
            else
            {
                Tj2 = amax / jmax;
                Td = Tj2 + (vmax - v1) / amax;
                alimd = amax;
            }

            Tv = (q1 - q0) / vmax - Ta / 2 * (1 + v0 / vmax) - Td / 2 * (1 + v1 / vmax);
            if (Tv <= 0)
            {
                Tv = 0;
                double amax_org = amax;

                while (Ta < 2 * Tj1 || Td < 2 * Tj2)
                {
                    amax = amax - amax_org * 0.1;
                    alima = amax;
                    alimd = amax;

                    double delta = (amax * amax * amax * amax) / (jmax * jmax) + 2 * (v0 * v0 + v1 * v1) + amax *
                        (4 * (q1 - q0) - 2 * amax / jmax * (v0 + v1));

                    Tj1 = amax / jmax;
                    Ta = (amax * amax / jmax - 2 * v0 + Math.Sqrt(delta)) / (2 * amax);
                    Tj2 = amax / jmax;
                    Td = (amax * amax / jmax - 2 * v1 + Math.Sqrt(delta)) / (2 * amax);
                }
            }

            List<Vector3> points = new List<Vector3>();
            List<double> p = new List<double>();
            List<double> vc = new List<double>();
            List<double> ac = new List<double>();
            List<double> jc = new List<double>();

            double T;
            double vlim = Tv > 0 ? vmax : v0 + (Ta - Tj1) * alima;
            T = Tv + Ta + Td;

            Vector3 direction = (P2 - P1) / (float)q1;

            for (double t = 0; t <= T + mydt; t += mydt)
            {
                double q = q0, v = v0, a = 0, j = 0;  // 初始化变量

                if (t >= 0 && t < Tj1)
                {
                    // 段1：加加速度段
                    q = q0 + v0 * t + jmax * t * t * t / 6;
                    v = v0 + jmax * t * t / 2;
                    a = jmax * t;
                    j = jmax;
                }
                else if (t >= Tj1 && t < Ta - Tj1)
                {
                    // 段2：匀加速度段
                    q = q0 + v0 * t + alima / 6 * (3 * t * t - 3 * Tj1 * t + Tj1 * Tj1);
                    v = v0 + alima * (t - Tj1 / 2);
                    a = alima;
                    j = 0;
                }
                else if (t >= Ta - Tj1 && t < Ta)
                {
                    // 段3：减加速度段
                    q = q0 + (vlim + v0) * Ta / 2 - vlim * (Ta - t) + jmax * (Ta - t) * (Ta - t) * (Ta - t) / 6;
                    v = vlim - jmax * (Ta - t) * (Ta - t) / 2;
                    a = jmax * (Ta - t);
                    j = -jmax;
                }
                else if (t >= Ta && t < Ta + Tv)
                {
                    // 段4：匀速段
                    q = q0 + (vlim + v0) * Ta / 2 + vlim * (t - Ta);
                    v = vlim;
                    a = 0;
                    j = 0;
                }
                else if (t >= Ta + Tv && t < T - Td + Tj2)
                {
                    // 段5：减加速度段
                    q = q1 - (vlim + v1) * Td / 2 + vlim * (t - T + Td) - jmax * (t - T + Td) * (t - T + Td) * (t - T + Td) / 6;
                    v = vlim - jmax * (t - T + Td) * (t - T + Td) / 2;
                    a = -jmax * (t - T + Td);
                    j = -jmax;
                }
                else if (t >= T - Td + Tj2 && t < T - Tj2)
                {
                    // 段6：匀减速度段
                    q = q1 - (vlim + v1) * Td / 2 + vlim * (t - T + Td) - alimd / 6 * (
                        3 * (t - T + Td) * (t - T + Td) - 3 * Tj2 * (t - T + Td) + Tj2 * Tj2);
                    v = vlim - alimd * (t - T + Td - Tj2 / 2);
                    a = -alimd;
                    j = 0;
                }
                else if (t >= T - Tj2 && t <= T)
                {
                    // 段7：减减速度段
                    q = q1 - v1 * (T - t) - jmax * (T - t) * (T - t) * (T - t) / 6;
                    v = v1 + jmax * (T - t) * (T - t) / 2;
                    a = -jmax * (T - t);
                    j = jmax;
                }

                // 将位置向量投影到三维空间
                Vector3 pos = P1 + (float)q * direction;
                points.Add(pos);
                p.Add(q);
                vc.Add(v);
                ac.Add(a);
                jc.Add(j);
            }
            return (points, p, vc, ac, jc);

        }

        public List<(double t1Distance, double t2Distance, double t3Distance)> CalculateDistances(
    List<Vector3> points, List<double> p, List<double> vc, List<double> ac, List<double> jc)
        {
            List<(double t1Distance, double t2Distance, double t3Distance)> distances = new List<(double, double, double)>();

            for (int i = 0; i < points.Count - 1; i++)
            {
                // 从 points 列表中获取当前点和下一个点的坐标
                Vector3 currentPoint = points[i];
                Vector3 nextPoint = points[i + 1];

                double t1Current = p[i];
                double t2Current = vc[i];
                double t3Current = ac[i];
                double t1Next = p[i + 1];
                double t2Next = vc[i + 1];
                double t3Next = ac[i + 1];

                // 调用 InverseKinematics 方法获取 x, y, z 坐标
                (double xCurrent, double yCurrent, double zCurrent) = InverseKinematics(t1Current, t2Current, t3Current);
                (double xNext, double yNext, double zNext) = InverseKinematics(t1Next, t2Next, t3Next);

                // 计算 t1, t2, t3 的距离
                double t1Distance = Math.Abs(xNext - xCurrent);
                double t2Distance = Math.Abs(yNext - yCurrent);
                double t3Distance = Math.Abs(zNext - zCurrent);

                distances.Add((t1Distance, t2Distance, t3Distance));
            }
            return distances;
        }

        public (int deltaT1Pulse, int deltaT2Pulse, int deltaT3Pulse) CoordinateConvertPulse(
    double x1, double y1, double z1, double x2, double y2, double z2)
        {
            // 通过逆解计算起始点和终点的 t1, t2, t3
            var (t1_1, t2_1, t3_1) = ForwardKinematics(x1, y1, z1);
            var (t1_2, t2_2, t3_2) = ForwardKinematics(x2, y2, z2);

            // 计算t的变化量
            double deltaT1 = t1_2 - t1_1;
            double deltaT2 = t2_2 - t2_1;
            double deltaT3 = t3_2 - t3_1;

            int deltaT1Pulse = 1280 * (int)deltaT1;        
            int deltaT2Pulse = 1280 * (int)deltaT2;
            int deltaT3Pulse = 1280 * (int)deltaT3;

            return (deltaT1Pulse, deltaT2Pulse, deltaT3Pulse);
        }

        public static List<double[]> GenerateCurve(double Xa, double Ya, double Za, double Xh, double Yh, double Zh)
        {
            List<double[]> S = new List<double[]>(); // 用于存储路径点
            double h = 30;
            double e = 180;
            double d = 25;

            double delta_h = Zh - Za;
            double h1, h2;
            if (delta_h > 0)
            {
                h1 = h + delta_h;
                h2 = h;
            }
            else if (delta_h < 0)
            {
                h1 = h;
                h2 = h + Math.Abs(delta_h);
            }
            else
            {
                h1 = h;
                h2 = h;
            }
            double workspace_max = 360;
            double[] HA = { Xh - Xa, Yh - Ya };
            double lengh = Math.Sqrt(HA[0] * HA[0] + HA[1] * HA[1]);
            double factor = lengh / workspace_max;
            e *= factor;
            d *= factor;

            double T_max = 5;
            double T = factor * T_max;
            if (T <= 3)
            {
                T = 3;
            }

            double Xb = Xa, Yb = Ya, Zb = Za + h1;
            double Xg = Xh, Yg = Yh, Zg = Zh + h2;
            double BG = Math.Sqrt((Xg - Xb) * (Xg - Xb) + (Yg - Yb) * (Yg - Yb));
            double c = Math.Sqrt((Xa - Xh) * (Xa - Xh) + (Ya - Yh) * (Ya - Yh)) / 2 - d;
            double Xd = Xa + (Xh - Xa) * d / BG;
            double Yd = Ya + (Yh - Ya) * d / BG;
            double Zd = Za + h1 + e;

            double t1 = Xd, t2 = Yd, t3 = Za + h1;
            double r1 = (Xa - t1) / d;
            double r4 = (Ya - t2) / d;
            double r7 = (Za + h1 - t3) / d;
            double r2 = (Xd - t1) / e;
            double r5 = (Yd - t2) / e;
            double r8 = (Zd - t3) / e;

            double[,] H =
            {
            { r1, r2, 0, t1 },
            { r4, r5, 0, t2 },
            { r7, r8, 0, t3 },
            { 0, 0, 0, 1 }
        };

            Func<double, double> f1 = x => ((1 + Math.Pow(Math.Tan(x), 2)) * Math.Sqrt(Math.Pow(d, 2) * Math.Pow(Math.Pow(Math.Tan(x), 4), 2) + Math.Pow(e, 2))) / Math.Pow(1 + Math.Pow(Math.Tan(x), 3), 4 / 3);
            Func<double, double> f2 = x => (1 + Math.Pow(Math.Tan(x), 2)) * Math.Sqrt(Math.Pow(e, 2) * Math.Pow(Math.Pow(Math.Tan(x), 4), 2) + Math.Pow(d, 2)) / Math.Pow(1 + Math.Pow(Math.Tan(x), 3), 4 / 3);

            double theta1 = 0;
            double theta2 = Math.PI / 4;
            double Lc = Integrate(f1, theta1, theta2, 1000) + Integrate(f2, theta1, theta2, 1000);
            double L_sum = h1 + h2 + 2 * (Lc + c);

            int i = 0, j = 0, k = 0;
            //List<double[]> S = new List<double[]>();
            double v_i = 0, u_i = 0;  // 初始值
            double v_init = 0;
            double u_init = 0;


            for (double t = 0; t <= T; t += 0.01)
            {
                double tao = t / T;
                double tao_ = (t + 0.01) / T;
                double P_tao = -10 * Math.Pow(tao, 6) + 36 * Math.Pow(tao, 5) - 45 * Math.Pow(tao, 4) + 20 * Math.Pow(tao, 3);
                double P_tao_ = -10 * Math.Pow(tao_, 6) + 36 * Math.Pow(tao_, 5) - 45 * Math.Pow(tao_, 4) + 20 * Math.Pow(tao_, 3);
                double P_tao_diff1 = -60 * Math.Pow(tao, 5) + 180 * Math.Pow(tao, 4) - 180 * Math.Pow(tao, 3) + 60 * Math.Pow(tao, 2);

                double S_t = L_sum * P_tao;
                double S_t_diff1 = L_sum * (P_tao_diff1) / T;
                double S_t_ = L_sum * P_tao_;

                if (S_t >= 0 && S_t < h1)
                {
                    S.Add(new double[] { Xa, Ya, Za + S_t });
                }
                else if (h1 <= S_t && S_t < h1 + Lc)
                {
                    if (i == 0)
                    {
                        v_init = S_t - h1;
                        u_init = d * Math.Pow(1 - Math.Pow(v_init / e, 3), 1.0 / 3);
                        i++;
                    }

                    if (j == 0)
                    {
                        v_i = v_init;
                        u_i = u_init;
                        j++;
                    }

                    double delta_s_s = S_t_ - S_t;
                    double u_diff_v = -((Math.Pow(v_i, 2) * Math.Pow(d, 3)) / (Math.Pow(u_i, 2) * Math.Pow(e, 3)));

                    if (Math.Abs(u_diff_v) <= 10)
                    {
                        double delta_v = delta_s_s / Math.Sqrt(1 + Math.Pow(u_diff_v, 2));
                        v_i += delta_v;
                        u_i = d * Math.Pow(1 - Math.Pow(v_i / e, 3), 1.0 / 3);
                    }
                    else
                    {
                        double v_diff_u = -((Math.Pow(u_i, 2) * Math.Pow(e, 3)) / (Math.Pow(v_i, 2) * Math.Pow(d, 3)));
                        double delta_u = -delta_s_s / Math.Sqrt(1 + Math.Pow(v_diff_u, 2));
                        u_i += delta_u;
                        v_i = e * Math.Pow(1 - Math.Pow(u_i / d, 3), 1.0 / 3);
                    }

                    double[] S2 = new double[] { r1 * u_i + r2 * v_i + t1, r4 * u_i + r5 * v_i + t2, r7 * u_i + r8 * v_i + t3 };
                    S.Add(S2);
                }
                else if (S_t >= h1 + Lc && S_t < h1 + Lc + 2 * c)
                {
                    double deta = Math.Atan2(Yh - Ya, Xh - Xa);
                    double l_dp = S_t - h1 - Lc;
                    S.Add(new double[] { Xd + l_dp * Math.Cos(deta), Yd + l_dp * Math.Sin(deta), Zd });
                }
                else if (S_t >= h1 + Lc + 2 * c && S_t < h1 + 2 * Lc + 2 * c)
                {
                    double delta_s_s = S_t_ - S_t;
                    if (k == 0)
                    {
                        double s_init = S_t - h1 - Lc - 2 * c;
                        u_init = -s_init;
                        v_init = e * Math.Pow(1 + Math.Pow(u_init / d, 3), 1.0 / 3);
                        u_i = u_init;
                        v_i = v_init;
                        k++;
                    }

                    double u_diff_v = ((Math.Pow(d, 3) * Math.Pow(v_i, 2)) / (Math.Pow(u_i, 2) * Math.Pow(e, 3)));
                    double v_diff_u = ((Math.Pow(u_i, 2) * Math.Pow(e, 3)) / (Math.Pow(d, 3) * Math.Pow(v_i, 2)));

                    if (Math.Abs(u_diff_v) <= 10)
                    {
                        double delta_v = -delta_s_s / Math.Sqrt(1 + Math.Pow(u_diff_v, 2));
                        v_i += delta_v;
                        double result = Math.Sign(-1 + Math.Pow(v_i / e, 3)) * Math.Pow(Math.Abs(-1 + Math.Pow(v_i / e, 3)), 1.0 / 3);
                        double u = d * result;
                        u_i = u;
                    }
                    else
                    {
                        double delta_u = -delta_s_s / Math.Sqrt(1 + Math.Pow(v_diff_u, 2));
                        u_i += delta_u;
                        double result = Math.Sign(1 + Math.Pow(u_i / d, 3)) * Math.Pow(Math.Abs(1 + Math.Pow(u_i / d, 3)), 1.0 / 3);
                        v_i = e * result;
                    }

                    u_i -= 2 * c;
                    double[] S4 = new double[] { r1 * u_i + r2 * v_i + t1, r4 * u_i + r5 * v_i + t2, r7 * u_i + r8 * v_i + t3 };
                    S.Add(S4);
                }
                else if (S_t >= h1 + 2 * Lc + 2 * c && S_t < L_sum)
                {
                    S.Add(new double[] { Xh, Yh, Zh + L_sum - S_t });
                }
            }

            return S;
        }
            

        // 计算 Lc 的函数
        public static double CalculateLc(double d, double e)
        {
            Func<double, double> f1 = (x) => ((1 + Math.Pow(Math.Tan(x), 2)) * Math.Sqrt(Math.Pow(d, 2) * Math.Pow(Math.Tan(x), 4) + Math.Pow(e, 2))) / Math.Pow(1 + Math.Pow(Math.Tan(x), 3), 4.0 / 3.0);
            Func<double, double> f2 = (x) => (1 + Math.Pow(Math.Tan(x), 2)) * Math.Sqrt(Math.Pow(e, 2) * Math.Pow(Math.Tan(x), 4) + Math.Pow(d, 2)) / Math.Pow(1 + Math.Pow(Math.Tan(x), 3), 4.0 / 3.0);

            double theta1 = 0;
            double theta2 = Math.PI / 4;

            double Lc = 0;
            Lc += Integrate(f1, theta1, theta2);
            Lc += Integrate(f2, theta1, theta2);

            return Lc;
        }

        // 数值积分方法
        public static double Integrate(Func<double, double> f, double a, double b, int n = 1000)
        {
            double h = (b - a) / n;
            double result = 0.5 * (f(a) + f(b));

            for (int i = 1; i < n; i++)
            {
                result += f(a + i * h);
            }

            result *= h;
            return result;
        }
    }
}
 
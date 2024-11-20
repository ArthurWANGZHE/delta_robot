using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace delta_robot
{
    internal class WhiteboardControl : Panel
    {
        private bool isDrawing = false;
        private List<Point> points = new List<Point>();  // 当前路径
        private List<List<Point>> allPaths = new List<List<Point>>();  // 所有路径

        public WhiteboardControl()
        {
            this.DoubleBuffered = true;  // 减少闪烁
            this.BackColor = Color.White;  // 设置背景色为白色
            this.MouseDown += WhiteboardControl_MouseDown;
            this.MouseMove += WhiteboardControl_MouseMove;
            this.MouseUp += WhiteboardControl_MouseUp;
        }

        // 鼠标按下事件，开始绘制
        private void WhiteboardControl_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            points.Clear();  // 清除当前路径
            points.Add(e.Location);  // 添加起始点

        }

        // 鼠标移动事件，记录坐标并绘制
        private void WhiteboardControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                allPaths.Add(new List<Point>(points));
                points.Add(e.Location);  // 添加当前位置
                this.Invalidate();  // 重新绘制
            }
        }

        // 鼠标抬起事件，结束绘制并保存轨迹
        private void WhiteboardControl_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;

            // 保存当前路径
            if (points.Count > 0)
            {
                allPaths.Add(new List<Point>(points));
            }
        }
        // 开始记录
        public void StartRecording()
        {
            allPaths.Clear();  // 清空之前的路径
            points.Clear();    // 清空当前路径
        }

        // 停止记录
        public void StopRecording()
        {
            isDrawing = false;
        }

        // 重绘事件
        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.Clear(BackColor); // 清空白板

            // 绘制所有路径
            foreach (var path in allPaths)
            {
                for (int i = 1; i < path.Count; i++)
                {
                    graphics.DrawLine(Pens.Black, path[i - 1], path[i]);
                }
            }

            // 绘制当前路径
            if (isDrawing && points.Count > 1)
            {
                for (int i = 1; i < points.Count; i++)
                {
                    graphics.DrawLine(Pens.Black, points[i - 1], points[i]);
                }
            }
        }

        // 清空白板
        public void ClearBoard()
        {
            allPaths.Clear();  // 清空路径列表
            points.Clear();    // 清空当前路径
            isDrawing = false; // 停止绘制
            this.Invalidate(); // 请求重新绘制
        }




        public void get_points()
        {
            var result= string.Join("; ", allPaths.SelectMany(list => list.Select(point => $"({point.X}, {point.Y})")));
            //var result = string.Join("; ", points.Select(point => $"({point.X}, {point.Y})"));
            var x1 = allPaths[10][1].Y;
            var x2 = allPaths[10][2].Y;
            var x3 = allPaths[10][3].Y;
            var x4 = allPaths[10][4].Y;
            var x5 = allPaths[10][5].Y;
            var x6 = allPaths[10][6].Y;
            var x7 = allPaths[10][7].Y;
            var x8 = allPaths[10][8].Y;
            var x9 = allPaths[10][9].Y;
            var x10 = allPaths[10][10].Y;

            int rows = allPaths.Count;
            int cols = allPaths[0].Count;


            Console.WriteLine($"Array is {rows}x{cols}  {x1} {x2} {x3} {x4} {x5} {x6} {x7} {x8} {x9} {x10}");
        }
        public List<(double x, double y, double z)> get_paths()
        {
            int rowCount = allPaths.Count;
            //Console.WriteLine($"allpath is {rowCount}");

            List<(double x, double y, double z)> coordinates = new List<(double, double, double)>();
            //var x = allPaths[rowCount-1][0].X / 2;
            //var y = allPaths[rowCount-1][0].Y / 2;

            var x2 = 0;
            var y2 = 0;

            //x = x - 90;

            //y= 80 - y;



            //coordinates.Add((x, y, 60));

            for (int i = 0; i < rowCount; i++)
            {
                //var(x0, y0, z0) = coordinates[coordinates.Count - 1];


                //var x1 = allPaths[rowCount - 1][i - 1].X ;
                //var y1 = allPaths[rowCount - 1][i - 1].Y ;


                //double distance = Math.Sqrt(Math.Pow(x1 - x0, 2) + Math.Pow(y1 - y0, 2));

                //if (distance > 1)
                //{

                    x2 = allPaths[rowCount - 1][i].X / 4;
                    y2 = allPaths[rowCount - 1][i].Y / 4;


                        x2 = x2 - 45;


                        y2 = 40 - y2;

                    //var (x_, y_, z_) = (allPaths[i][0].X / 4, allPaths[i][0].Y / 4, 60);
                    coordinates.Add((x2, y2, 60));
                    //Console.WriteLine($"{x2} {y2}");
                    //Console.WriteLine($"{x1} {y1}");
                //}
            }
            

            

            var coor_counts = coordinates.Count();
            //Console.WriteLine($"coordinates is {coor_counts}");
            return coordinates;
        }


        public void run_points()
        {
            var coor = get_paths();
            for (int i = 0; i < coor.Count-1; i++)
            {
                var (x01, y01, z01) = coor[i];
                var (x02,y02,z02) = coor[i+1];

            }
        }
    }
}

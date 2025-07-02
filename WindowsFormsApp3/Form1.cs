using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public double InitT = 0, LastT = 6.3 * 1.2;
        public double Step = 0.1;
        public int cX = 300, cY = 300;
        public int R_base = 20;
        public List<PointF> p = new List<PointF>();

        public float x, y;
        //Данные пар:

        public int par1 =50, par2=100, par3=50;

        public float angel_rotate =0;

        public bool Puls =false;

        public bool kons = true;

        public bool Cir = false;

        public bool TrDash = true;

        public int red = 255, green = 128, blue = 0;

        private PointF[] Parallelogram_By_2Sides_And_Angle(PointF pointA, float side1, float side2, float angleDegrees)
        {
            double angleRadians = angleDegrees * Math.PI / 180.0;

            // Точка B: смещение от A вдоль оси X на длину side1
            double xB = pointA.X + side1;
            double yB = pointA.Y;
            PointF pointB = new PointF((float)xB, (float)yB);

            // Точка D: смещение от A с учетом угла и длины side2
            double xD = pointA.X + side2 * Math.Cos(angleRadians);
            double yD = pointA.Y + side2 * Math.Sin(angleRadians);
            PointF pointD = new PointF((float)xD, (float)yD);

            // Точка C: C = B + (D - A)
            double xC = xB + (xD - pointA.X);
            double yC = yB + (yD - pointA.Y);
            PointF pointC = new PointF((float)xC, (float)yC);

            return new PointF[] { pointA, pointB, pointC, pointD };
        }
        PointF[] Parallelogram_By_Side_And_2Angles(PointF pointA, float side1, float angle1Degrees, float angle2Degrees)
        {
            // Проверка на корректность входных данных
            if (side1 <= 0)
                throw new ArgumentException("Длина стороны должна быть положительной.");
            if (Math.Abs(angle2Degrees % 180) < 0.001 || Math.Abs(angle2Degrees % 180 - 180) < 0.001)
                throw new ArgumentException("Угол между сторонами не должен быть 0 или 180 градусов, чтобы избежать вырождения.");

            double angle1Radians = angle1Degrees * Math.PI / 180.0;
            double angle2Radians = angle2Degrees * Math.PI / 180.0;

            // Точка B: смещение от A по углу angle1 и длине side1
            double xB = pointA.X + side1 * Math.Cos(angle1Radians);
            double yB = pointA.Y + side1 * Math.Sin(angle1Radians);
            PointF pointB = new PointF((float)xB, (float)yB);

            // Точка D: смещение от A по углу (angle1 + angle2) и длине side1
            double angleDRadians = angle1Radians + angle2Radians; // Угол для AD
            double xD = pointA.X + side1 * Math.Cos(angleDRadians);
            double yD = pointA.Y + side1 * Math.Sin(angleDRadians);
            PointF pointD = new PointF((float)xD, (float)yD);

            // Точка C: C = B + (D - A)
            double xC = xB + (xD - pointA.X);
            double yC = yB + (yD - pointA.Y);
            PointF pointC = new PointF((float)xC, (float)yC);

            return new PointF[] { pointA, pointB, pointC, pointD };
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Cir=(Cir ? false :true);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string selectedText = comboBox1.SelectedItem.ToString();
                if (selectedText== "2 стороны и угол")
                {
                    kons = true;
                    label3.Text = "1 сторонa";
                    label4.Text = "2 сторонa";
                    label5.Text = "угол";
                }
                else
                {
                    kons = false;
                    label3.Text = "сторона";
                    label4.Text = "1 угол";
                    label5.Text = "2 угол";
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.All(char.IsDigit))
            {
                int.TryParse(this.textBox2.Text, out par1);
            }
            else
            {
                par1 = 10;
            }
            if (par1 < 2)
            {
                par1 = 2;
            }
            else if (par1 > 250)
            {
                par1 = 250;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
     
            if (textBox3.Text.All(char.IsDigit))
            {
                int.TryParse(this.textBox3.Text, out par2);
            }
            else
            {
                par2 = 10;
            }
            if (par2 < 0)
            {
                par2 = 2;
            }
            else if (par2 > 250)
            {
                par2 = 250;
            }
        
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text.All(char.IsDigit))
            {
                int.TryParse(this.textBox4.Text, out par3);
            }
            else
            {
                par3 = 10;
            }
            if (par3 < 0)
            {
                par3 = 2;
            }
            else if (par3 > 250)
            {
                par3 = 250;
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Puls = (Puls ? false : true);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text.All(char.IsDigit))
            {
                int.TryParse(this.textBox5.Text, out red);
            }
            else
            {
                red = 255;
            }
            if (red < 0)
            {
                red = 0;
            }
            else if (red > 255)
            {   
                red = 255;
            }


        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text.All(char.IsDigit))
            {
                int.TryParse(this.textBox6.Text, out green);
            }
            else
            {
                green = 255;
            }
            if (green < 0)
            {
                green = 0;
            }
            else if (green > 255)
            {
                green = 255;
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text.All(char.IsDigit))
            {
                int.TryParse(this.textBox7.Text, out blue);
            }
            else
            {
                blue = 255;
            }
            if (blue < 0)
            {
                blue = 0;
            }
            else if (blue > 255)
            {
                blue = 255;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                string selectedText = comboBox2.SelectedItem.ToString();
                TrDash = (selectedText == "сплошная" ? true : false);
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text.All(char.IsDigit))
            {
                float.TryParse(this.textBox8.Text, out angel_rotate);
            }
            else
            {
                par1 = 0;
            }
            if (par1 < 0)
            {
                par1 = 0;
            }
            else if (par1 > 180)
            {
                par1 = 180;
            }
        }

        private PointF[] RotateParallelogram(PointF[] parallelogram, PointF pivot, float angleDegrees)
        {
            double angleRadians = angleDegrees * Math.PI / 180.0;
            PointF[] rotatedPoints = new PointF[parallelogram.Length];

            rotatedPoints[0] = pivot;

            for (int i = 1; i < parallelogram.Length; i++)
            {
                double x = parallelogram[i].X;
                double y = parallelogram[i].Y;
                double xPivot = pivot.X;
                double yPivot = pivot.Y;

                double xNew = xPivot + (x - xPivot) * Math.Cos(angleRadians) + (y - yPivot) * Math.Sin(angleRadians);
                double yNew = yPivot - (x - xPivot) * Math.Sin(angleRadians) + (y - yPivot) * Math.Cos(angleRadians);

                rotatedPoints[i] = new PointF((float)xNew, (float)yNew);
            }

            return rotatedPoints;
        }

        private void Paint_Graphic(List<PointF> p, int cX = 250, int cY = 250)
        {

            p.Clear();
            Graphics Graf = pictureBox1.CreateGraphics();
            double angle = InitT;
            Graf.Clear(BackColor);
            if (Cir==true) { 
                Graf.DrawEllipse(Pens.Red, cX - (int)R_base, cY - (int)R_base, (int)R_base * 2, (int)R_base * 2);
            }
            while (angle <= LastT)
            {
                x = (float)(R_base * (Math.Cos(angle) + angle * Math.Sin(angle)));
                y = (float)(R_base * (Math.Sin(angle) - angle * Math.Cos(angle)));
                p.Add(new PointF(cX + (int)x, cY + (int)y));
                angle += Step;

            }

            PointF center = new PointF(250, 250);
            PointF[] p_arr = p.ToArray();
            if (p_arr.Length > 2)
            {
                Graf.DrawLines(Pens.Black, p_arr); // траектория

                using (Pen pen = new Pen(Color.Black, 2))
                {

                    pen.DashStyle = (TrDash? DashStyle.Solid : DashStyle.Dash);

                    Graf.DrawLines(pen, p_arr);

                }

                //using (Pen pen = new Pen(Color.Red, 2)) // Жирность 3 пикселя
                //{
                //    pen.DashStyle = DashStyle.Dash; // Пунктирный стил
                //    foreach (PointF sp in p_arr)
                //    {
                //        PointF[] line = new PointF[2];
                //        line[0] = center;
                //        line[1] = sp;
                //        Graf.DrawLines(pen, line);
                //    }
                //}
            }


        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Paint_Graphic(p, cX, cY);
            float start_rot = 0;
            Graphics Graf = pictureBox1.CreateGraphics();
            PointF[] p_arr = p.ToArray();
            float koef = 1;
            float step = 0;
            foreach (var item in p_arr)
            {        
                if (Puls)
                {
                    if (koef > 2)
                    {
                        step = (float)-0.1;
                    } else if (koef == 1)
                    {
                        step = (float)0.1;
                    }

                }
                Paint_Graphic(p, cX, cY);
                PointF[] par;
                if (kons)
                {
                    par = Parallelogram_By_2Sides_And_Angle(item, par1 * koef, par2 * koef, par3);
                }
                else
                {
                    par = Parallelogram_By_Side_And_2Angles(item, par1 * koef, par2, par3);
                }
                par = RotateParallelogram(par, item, start_rot);
                start_rot += angel_rotate;

                using (SolidBrush brush = new SolidBrush(Color.FromArgb(red, green, blue)))
                {
                    Graf.FillPolygon(brush, par); // Заливка
                }
                using (Pen pen = new Pen(Color.Black, 2)) // Жирность 3
                    {
                        pen.DashStyle = DashStyle.Solid; // Пунктир
                        Graf.DrawPolygon(pen, par);
                }
                koef += step;
                Thread.Sleep(40);




                
         
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.All(char.IsDigit))
            {
                int.TryParse(this.textBox1.Text, out R_base);
            }
            else
            {
                R_base = 100;
            }
            if (R_base < 2)
            {
                R_base = 2;
            }
            else if (R_base > 250)
            {
                R_base = 250;
            }

        }
    }
}

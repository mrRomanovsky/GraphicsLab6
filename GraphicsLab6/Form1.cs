using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphicsLab6
{
    public partial class Form1 : Form
    {
        private string figType = "";
        private string mode = "";
        private Polyhedron figure;
        private Pen redPen = new Pen(Color.Red);
        private List<System.Windows.Forms.Panel> tasksPanels;
        private Edge polyhrdron2D;
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            tasksPanels = new List<Panel>(7);
            InitializeTasksPanels();
        }

        private void InitializeTasksPanels()
        {
            Initialize2TaskPanel();
           // Initialize4TaskPanel();
            //I7nitialize5TaskPanel();
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            figType = (string)((ListBox)sender).SelectedItem;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (figType != "" && textBox1.Text != "" && mode == "")
            {
                switch (figType)
                {
                    case "Тетраэдр":
                        figure = new Polyhedron(PolyhedronType.Tetrahedron, int.Parse(textBox1.Text));
                        break;
                    case "Гексаэдр":
                        figure = new Polyhedron(PolyhedronType.Hexahedron, int.Parse(textBox1.Text));
                        break;
                    case "Октаэдр":
                        figure = new Polyhedron(PolyhedronType.Octahedron, int.Parse(textBox1.Text));
                        break;
                }
                DrawPolyhedron(figure, pictureBox1.Size);
                pictureBox1.Invalidate();
            }
        }

        private void DrawPolyhedron(Polyhedron polyhedron, Size size)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            var res = new List<PointF>();
            var x = size.Width / 2 - polyhedron.SegmentLength / 2;
            var y = size.Height / 2 - polyhedron.SegmentLength / 2;
            var z = 1.0;

            using (var g = Graphics.FromImage(pictureBox1.Image))
            {
                foreach (var item in polyhedron.vertexes)
                {
                    if (item.Z != 0)
                        z = item.Z;
                    var scaledPoint = new PointF((float)(item.X / z + x), (float)(item.Y / z) + y);
                    foreach (var neighbour in item.Neighbours)
                    {
                        var scaledNeighbour = new PointF((float)((float)neighbour.X / (float)z + x), (float)((float)neighbour.Y / (float)z) + y);
                        g.DrawLine(redPen, scaledPoint, scaledNeighbour);
                    }
                    res.Add(new PointF((float)(item.X / z + x), (float)(item.Y / z) + y));
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            figType = "";
            mode = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (listBox2.Text)
            {
                case "Задание 1":
                    mode = "";
                    break;
                case "Задание 7":
                    var c = pictureBox1.Height / 2;
                    var perspective = new List<List<double>> { new List<double> { 1, 0, 0, 0 }, new List<double> { 0, 1, 0, 0 }, new List<double> { 0, 0, 0, -1 / c }, new List<double> { 0, 0, 0, 1 } };

                    foreach (var item in figure.vertexes)
                        item.MultiplyByMatrix(perspective);
                    DrawPolyhedron(figure, pictureBox1.Size);
                    pictureBox1.Invalidate();
                    break;
                case "Задание 8":
                    //double[,] perspective = new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
                    //Изометрическая
                    var pers = new List<List<double>> { new List<double> { Math.Sqrt(0.5), -1 / Math.Sqrt(6), 0, 0 }, new List<double> { 0, Math.Sqrt(2) / Math.Sqrt(3), 0, 0 }, new List<double> { -1 / Math.Sqrt(2), -1 / Math.Sqrt(6), 0, 0 }, new List<double> { 0, 0, 0, 1 } };
                    //double[,] perspective = new double[,] { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 0, -1 / c }, { 0, 0, 0, 1 } };

                    foreach (var item in figure.vertexes)
                        item.MultiplyByMatrix(pers);
                    DrawPolyhedron(figure, pictureBox1.Size);
                    pictureBox1.Invalidate();
                    break;
                case "Задание 5":
                    using (var centreLineDialog = new LineThroughCenter())
                    {
                        var dialogRes = centreLineDialog.ShowDialog();
                        if (dialogRes == DialogResult.OK)
                        {
                            var axes = centreLineDialog.AxisChosen;
                            Point3D parallelUnitVector;
                            switch (axes)
                            {
                                case "x":
                                    parallelUnitVector = new Point3D(1, 0, 0);
                                    break;
                                case "y":
                                    parallelUnitVector = new Point3D(0, 1, 0);
                                    break;
                                default:
                                    parallelUnitVector = new Point3D(0, 0, 1);
                                    break;
                            }
                            figure.RotateAroundLine(figure.Centre, parallelUnitVector, centreLineDialog.RotationAngleRadians);
                            DrawPolyhedron(figure, pictureBox1.Size);
                            pictureBox1.Invalidate();
                        }
                    }
                    break;
                case "Задание 6":
                    using (var lineRotateDialog = new LineToRotate())
                    {
                        var dialogRes = lineRotateDialog.ShowDialog();
                        if (dialogRes == DialogResult.OK)
                        {
                            var point1 = lineRotateDialog.Point1;
                            var point2 = lineRotateDialog.Point2;
                            var parallelVector = new Point3D(point2.X - point1.X, point2.Y - point1.Y, point2.Z - point1.Z);
                            var vectorLength = Math.Sqrt(parallelVector.X * parallelVector.X + parallelVector.Y * parallelVector.Y + parallelVector.Z * parallelVector.Z);
                            parallelVector.X /= vectorLength;
                            parallelVector.Y /= vectorLength;
                            parallelVector.Z /= vectorLength;
                            figure.RotateAroundLine(point2, parallelVector, lineRotateDialog.AngleRad);
                            DrawPolyhedron(figure, pictureBox1.Size);
                            pictureBox1.Invalidate();
                        }
                    }
                    break;

            }
        }

        #region task2
        private List<string> activeItems = new List<string>();

        private void Initialize2TaskPanel()
        {
            var task2Panel = new Panel();
            task2Panel.Visible = false;
            task2Panel.Enabled = false;
            task2Panel.Location = new Point(600, 220);
            task2Panel.Size = new Size(190, 180);
            this.Controls.Add(task2Panel);
            tasksPanels.Add(task2Panel);
            var task2LabelAbout = new Label();
            task2LabelAbout.Location = new Point(22, 2);
            task2LabelAbout.Size = new Size(180, 50);
            task2LabelAbout.Text
                = "Применение аффинных преобразований к примитиву: смещение, поворот, масштаб.";
            var task2ButtonMove = new Button();
            task2ButtonMove.Text = "C";
            task2ButtonMove.Location = new Point(27, 47);
            task2ButtonMove.Size = new Size(40, 22);
            task2ButtonMove.FlatStyle = FlatStyle.Flat;
            task2ButtonMove.Click += moveButton_Click;
            var task2ButtonTurn = new Button();
            task2ButtonTurn.Text = "П";
            task2ButtonTurn.Click += turnButton_Click;
            task2ButtonTurn.Location = new Point(85, 47);
            task2ButtonTurn.Size = new Size(40, 22);
            task2ButtonTurn.FlatStyle = FlatStyle.Flat;
            var task2ButtonScale = new Button();
            task2ButtonScale.Text = "М";
            task2ButtonScale.Location = new Point(140, 47);
            task2ButtonScale.Click += scaleButton_Click;
            task2ButtonScale.Size = new Size(40, 22);
            task2ButtonScale.FlatStyle = FlatStyle.Flat;
            task2Panel.Controls.Add(task2ButtonScale);
            task2Panel.Controls.Add(task2ButtonTurn);
            task2Panel.Controls.Add(task2ButtonMove);
            task2Panel.Controls.Add(task2LabelAbout);
            var task2LabelDirection = new Label();
            task2LabelDirection.Text = "Задайте смещение: ";
            task2LabelDirection.Name = "task2LabelDirection";
            task2LabelDirection.Location = new Point(22, 80);
            task2LabelDirection.Size = new Size(160, 30);
            task2LabelDirection.Enabled = false;
            task2LabelDirection.Visible = false;
            task2Panel.Controls.Add(task2LabelDirection);

            var task2XLabel = new Label();
            task2XLabel.Text = "X: ";
            task2XLabel.Name = "task2XLabel";
            task2XLabel.Location = new Point(30, 110);
            task2XLabel.Size = new Size(20, 20);
            task2XLabel.Enabled = false;
            task2XLabel.Visible = false;
            task2Panel.Controls.Add(task2XLabel);
            var task2XTextBox = new TextBox();
            task2XTextBox.Text = "0";
            task2XTextBox.Name = "task2XTextBox";
            task2XTextBox.Location = new Point(60, 110);
            task2XTextBox.Size = new Size(30, 20);
            task2XTextBox.Enabled = false;
            task2XTextBox.Visible = false;
            task2Panel.Controls.Add(task2XTextBox);

            var task2YLabel = new Label();
            task2YLabel.Text = "Y: ";
            task2YLabel.Name = "task2YLabel";
            task2YLabel.Location = new Point(100, 110);
            task2YLabel.Size = new Size(20, 20);
            task2YLabel.Enabled = false;
            task2YLabel.Visible = false;
            task2Panel.Controls.Add(task2YLabel);
            var task2YTextBox = new TextBox();
            task2YTextBox.Text = "0";
            task2YTextBox.Name = "task2YTextBox";
            task2YTextBox.Location = new Point(130, 110);
            task2YTextBox.Size = new Size(30, 20);
            task2YTextBox.Enabled = false;
            task2YTextBox.Visible = false;
            task2Panel.Controls.Add(task2YTextBox);

            var task2AngleLabel = new Label();
            task2AngleLabel.Text = "Угол: ";
            task2AngleLabel.Name = "task2AngleLabel";
            task2AngleLabel.Location = new Point(50, 110);
            task2AngleLabel.Size = new Size(40, 20);
            task2AngleLabel.Enabled = false;
            task2AngleLabel.Visible = false;
            task2Panel.Controls.Add(task2AngleLabel);
            var task2AngleTextBox = new TextBox();
            task2AngleTextBox.Text = "0";
            task2AngleTextBox.Name = "task2AngleTextBox";
            task2AngleTextBox.Location = new Point(95, 110);
            task2AngleTextBox.Size = new Size(30, 20);
            task2AngleTextBox.Enabled = false;
            task2AngleTextBox.Visible = false;
            task2Panel.Controls.Add(task2AngleTextBox);

            var task2ButtonOk = new Button();
            task2ButtonOk.Text = "Сместить";
            task2ButtonOk.Location = new Point(60, 150);
            task2ButtonOk.Name = "task2ButtonOk";
           // task2ButtonOk.Click += task2ButtonOk_Click;
            task2ButtonOk.Size = new Size(80, 23);
            task2ButtonOk.Enabled = false;
            task2ButtonOk.Visible = false; task2ButtonOk.FlatStyle = FlatStyle.Flat;
            task2Panel.Controls.Add(task2ButtonOk);
        }

        private void moveButton_Click(object sender, EventArgs e)
        { 
            mode = "move";
            foreach (var ai in activeItems)
            {
                var item = tasksPanels[0].Controls.Find(ai, false).First();
                item.Visible = false;
                item.Enabled = false;
            }
            var strs = new List<string> { "task2LabelDirection", "task2XLabel", "task2YLabel", "task2XTextBox", "task2YTextBox", "task2ButtonOk" };
            foreach (var s in strs)
            {
                var item = tasksPanels[0].Controls.Find(s, false).First();
                if (item.Name == "task2LabelDirection")
                    item.Text = "Задайте смещение: ";
                else if (item.Name == "task2ButtonOk")
                    item.Text = "Сместить";
                item.Visible = true;
                item.Enabled = true;
            }
            activeItems = strs;
        }

        private void turnButton_Click(object sender, EventArgs e)
        { 
            mode = "turn";
            foreach (var ai in activeItems)
            {
                var item = tasksPanels[0].Controls.Find(ai, false).First();
                item.Visible = false;
                item.Enabled = false;
            }
            var strs = new List<string> { "task2LabelDirection", "task2AngleLabel", "task2AngleTextBox", "task2ButtonOk" };
            foreach (var s in strs)
            {
                var item = tasksPanels[0].Controls.Find(s, false).First();
                if (item.Name == "task2LabelDirection")
                    item.Text = "Задайте поворот: ";
                else if (item.Name == "task2ButtonOk")
                    item.Text = "Повернуть";
                item.Visible = true;
                item.Enabled = true;
            }
            activeItems = strs;
        }

        private void scaleButton_Click(object sender, EventArgs e)
        { 
            mode = "scale";
            foreach (var ai in activeItems)
            {
                var item = tasksPanels[0].Controls.Find(ai, false).First();
                item.Visible = false;
                item.Enabled = false;
            }
            var strs = new List<string> { "task2LabelDirection", "task2AngleLabel", "task2AngleTextBox", "task2ButtonOk" };
            foreach (var s in strs)
            {
                var item = tasksPanels[0].Controls.Find(s, false).First();
                if (item.Name == "task2AngleLabel")
                    item.Text = "Значение: ";
                else if (item.Name == "task2LabelDirection")
                    item.Text = "Задайте масштаб: ";
                else if (item.Name == "task2ButtonOk")
                    item.Text = "Масштабировать";
                item.Visible = true;
                item.Enabled = true;
            }
            activeItems = strs;
        }

        //private void ScaleFigure()
        //{
        //    var t1 = double.Parse(tasksPanels[0].Controls.Find("task2AngleTextBox", false).First().Text);
        //    if (t1 == 0)
        //        return;
        //    var matrix = new double[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, t1 } };
        //            DrawPolygon(polygon, new Pen(this.BackColor));
        //            var matrix2 = new double[polygon.corners.Count, 3];
        //            for (int i = 0; i < polygon.corners.Count; ++i)
        //            {
        //                matrix2[i, 0] = polygon.corners[i].X;
        //                matrix2[i, 1] = polygon.corners[i].Y;
        //                matrix2[i, 2] = 1;
        //            }
        //            var res2 = MultMatrix(matrix2, matrix);
        //            bool correctValue = true;
        //            var p = new Polygon();
        //            for (int i = 0; i < polygon.corners.Count(); ++i)
        //            {
        //                if (!CheckCorrectPoint(res2[i, 0] / t1, res2[i, 1] / t1))
        //                {
        //                    correctValue = false;
        //                    break;
        //                }
        //                p.corners.Add(new Point((int)(res2[i, 0] / t1), (int)(res2[i, 1] / t1)));
        //            }
        //            if (correctValue)
        //            {
        //                polygon = p;
        //            }
        //            DrawPolygon(polygon, redPen);
        //            pictureBox1.Invalidate(); break;
        //        default:
        //            break;
        //    }
        //}

    //    private void TurnFigure()
    //    {
    //        var t1 = double.Parse(tasksPanels[0].Controls.Find("task2AngleTextBox", false).First().Text);
    //        var matrix = new double[,] { { Math.Cos(Math.PI * t1 / 180), Math.Sin(Math.PI * t1 / 180), 0 }, { -Math.Sin(Math.PI * t1 / 180), Math.Cos(Math.PI * t1 / 180), 0 }, { 0, 0, 1 } };
    //        switch (figType)
    //        {
    //            case FigType.Point:
    //                break;
    //            case FigType.Segment:
    //                DrawSegment(segment, new Pen(this.BackColor));
    //                var res1 = MultMatrix(new double[,] { { segment.start.X, segment.start.Y, 1 }, { segment.end.X, segment.end.Y, 1 } }, matrix);
    //                if (CheckCorrectPoint(res1[0, 0], res1[0, 1]) && CheckCorrectPoint(res1[1, 0], res1[1, 1]))
    //                {
    //                    segment.start = new Point((int)res1[0, 0], (int)res1[0, 1]);
    //                    segment.end = new Point((int)res1[1, 0], (int)res1[1, 1]);
    //                }
    //                DrawSegment(segment, redPen);
    //                pictureBox1.Invalidate();
    //                break;
    //            case FigType.Polygon:
    //                DrawPolygon(polygon, new Pen(this.BackColor));
    //                double[,] matrix2 = new double[polygon.corners.Count, 3];
    //                for (int i = 0; i < polygon.corners.Count; ++i)
    //                {
    //                    matrix2[i, 0] = polygon.corners[i].X;
    //                    matrix2[i, 1] = polygon.corners[i].Y;
    //                    matrix2[i, 2] = 1;
    //                }
    //                var res2 = MultMatrix(matrix2, matrix);
    //                bool correctValue = true;
    //                var p = new Polygon();
    //                for (int i = 0; i < polygon.corners.Count(); ++i)
    //                {
    //                    if (!CheckCorrectPoint(res2[i, 0], res2[i, 1]))
    //                    {
    //                        correctValue = false;
    //                        break;
    //                    }
    //                    p.corners.Add(new Point((int)res2[i, 0], (int)res2[i, 1]));
    //                }
    //                if (correctValue)
    //                {
    //                    polygon = p;
    //                }
    //                DrawPolygon(polygon, redPen);
    //                pictureBox1.Invalidate(); break;
    //            default:
    //                break;
    //        }
    //    }

    //    void MoveFigure()
    //    {
    //        var t1 = tasksPanels[0].Controls.Find("task2XTextBox", false).First().Text;
    //        var t2 = tasksPanels[0].Controls.Find("task2YTextBox", false).First().Text;
    //        var matrix = new int[,] { { 1, 0, 0 }, { 0, 1, 0 }, { int.Parse(t1), int.Parse(t2), 1 } };
    //        switch (figType)
    //        {
    //            case FigType.Point:
    //                ((Bitmap)pictureBox1.Image).SetPixel(point.X, point.Y, this.BackColor);
    //                var res = MultMatrix(new int[,] { { point.X, point.Y, 1 } }, matrix);
    //                if (CheckCorrectPoint(res[0, 0], res[0, 1]))
    //                    point = new Point(res[0, 0], res[0, 1]);
    //                ((Bitmap)pictureBox1.Image).SetPixel(point.X, point.Y, redPen.Color);
    //                pictureBox1.Invalidate();
    //                break;
    //            case FigType.Segment:
    //                DrawSegment(segment, new Pen(this.BackColor));
    //                var res1 = MultMatrix(new int[,] { { segment.start.X, segment.start.Y, 1 }, { segment.end.X, segment.end.Y, 1 } }, matrix);
    //                if (CheckCorrectPoint(res1[0, 0], res1[0, 1]) && CheckCorrectPoint(res1[1, 0], res1[1, 1]))
    //                {
    //                    segment.start = new Point(res1[0, 0], res1[0, 1]);
    //                    segment.end = new Point(res1[1, 0], res1[1, 1]);
    //                }
    //                DrawSegment(segment, redPen);
    //                pictureBox1.Invalidate();
    //                break;
    //            case FigType.Polygon:
    //                DrawPolygon(polygon, new Pen(this.BackColor));
    //                int[,] matrix2 = new int[polygon.corners.Count, 3];
    //                for (int i = 0; i < polygon.corners.Count; ++i)
    //                {
    //                    matrix2[i, 0] = polygon.corners[i].X;
    //                    matrix2[i, 1] = polygon.corners[i].Y;
    //                    matrix2[i, 2] = 1;
    //                }
    //                var res2 = MultMatrix(matrix2, matrix);
    //                bool correctValue = true;
    //                var p = new Polygon();
    //                for (int i = 0; i < polygon.corners.Count(); ++i)
    //                {
    //                    if (!CheckCorrectPoint(res2[i, 0], res2[i, 1]))
    //                    {
    //                        correctValue = false;
    //                        break;
    //                    }
    //                    p.corners.Add(new Point(res2[i, 0], res2[i, 1]));
    //                }
    //                if (correctValue)
    //                {
    //                    polygon = p;
    //                }
    //                DrawPolygon(polygon, redPen);
    //                pictureBox1.Invalidate();
    //                break;
    //            default:
    //                break;
    //        }
    //    }

    //private void task2ButtonOk_Click(object sender, EventArgs e)
    //    {
    //        if (mode == "move")
    //            MoveFigure();
    //        else if (mode == "turn")
    //            TurnFigure();
    //        else if (mode == "scale")
    //            ScaleFigure();
    //    }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace calculus2
{
    public partial class Form1 : Form
    {
        class row
        {
            public double time;
            public double voltage;
            public double current;
            public double voltageDerivative;
            public double charge;
        }

        List<row> table = new List<row>();

        void tableSort()
        {
            table = table.OrderBy(x => x.time).ToList();
        }


        void derivative()
        {
            for (int i = 1; i < table.Count; i++)
            {
                double dV = table[i].voltage - table[i - 1].voltage;
                double dt = table[i].time - table[i - 1].time;
                table[i].voltageDerivative = dV / dt;
            }
        }



        public Form1()
        {
            InitializeComponent();

            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)


        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "CSV Files|*.csv";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {


                        {
                            string line = sr.ReadLine();
                            while (!sr.EndOfStream)
                            {
                                table.Add(new row());
                                string[] l = sr.ReadLine().Split(',');
                                table.Last().time = double.Parse(l[0]);
                                table.Last().voltage = double.Parse(l[1]);
                                table.Last().current = double.Parse(l[2]);
                            }
  
                        }
                    }

                }

                catch (IOException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " Failed to open");

                }
                catch (FormatException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " is not in the requiered format");
                }

                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " is not in the required format");
                }






            }

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void voltTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series1 = new Series
            {
                Name = "Points",
                Color = Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2

            };

            chart1.Series.Add(series1);
            for (int i = 0; i < table.Count; i++)
            {
                series1.Points.AddXY(table[i].time, table[i].voltage);
            }

            chart1.ChartAreas[0].AxisX.Title = "time / s";
            chart1.ChartAreas[0].AxisY.Title = "voltage / V";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void dVdtTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            derivative();

            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series1 = new Series
            {
                Name = "Points",
                Color = Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2

            };

            chart1.Series.Add(series1);
            for (int i = 0; i < table.Count; i++)
            {
                series1.Points.AddXY(table[i].time, table[i].voltageDerivative);
            }

            chart1.ChartAreas[0].AxisX.Title = "time / s";
            chart1.ChartAreas[0].AxisY.Title = "voltage / V";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void saveGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.SaveImage(, ChartImageFormat.Png);
        }
    }
}

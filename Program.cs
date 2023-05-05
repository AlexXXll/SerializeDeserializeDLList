using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;

namespace SerializeDeserializeDLList
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create Chart control
            Chart chart = new Chart();
            chart.Size = new System.Drawing.Size(800, 600);

            // Set Chart properties
            chart.Titles.Add("ListRand Performance Test");
            chart.Titles[0].Text += " (serialization and deserialization)";
            chart.ChartAreas.Add("ChartArea1");
            chart.ChartAreas["ChartArea1"].AxisX.Title = "List size";
            chart.ChartAreas["ChartArea1"].AxisX.Minimum = 1;
            chart.ChartAreas["ChartArea1"].AxisX.Maximum = 1000;
            chart.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            chart.ChartAreas["ChartArea1"].AxisY.Title = "Time (ms)";

            // Add Series to Chart
            Series series1 = new Series();
            series1.Name = "Serialize";
            series1.ChartType = SeriesChartType.Line;
            series1.BorderWidth = 2;
            series1.Color = System.Drawing.Color.Blue;
            chart.Series.Add(series1);

            Series series2 = new Series();
            series2.Name = "Deserialize";
            series2.ChartType = SeriesChartType.Line;
            series2.BorderWidth = 2;
            series2.Color = System.Drawing.Color.Red;
            chart.Series.Add(series2);

            // Test ListRand performance for different list sizes
            for (int size = 1; size <= 1000; size += 1)
            {
                // Create ListRand instance and fill it with data
                ListRand list = new ListRand();
                list.Head = new ListNode() { Data = "Node 1" };
                list.Tail = list.Head;
                list.Count = 1;

                for (int i = 2; i <= size; i++)
                {
                    ListNode node = new ListNode() { Data = "Node " + i };
                    node.Prev = list.Tail;
                    list.Tail.Next = node;
                    list.Tail = node;
                    list.Count++;
                }

                // Serialize ListRand and measure time
                Stopwatch sw1 = Stopwatch.StartNew();
                using (FileStream stream = new FileStream("list.bin", FileMode.Create))
                {
                    list.Serialize(stream);
                }
                sw1.Stop();
                double serializeTime = sw1.Elapsed.TotalMilliseconds;

                // Deserialize ListRand and measure time
                Stopwatch sw2 = Stopwatch.StartNew();
                using (FileStream stream = new FileStream("list.bin", FileMode.Open))
                {
                    list.Deserialize(stream);
                }
                sw2.Stop();
                double deserializeTime = sw2.Elapsed.TotalMilliseconds;

                // Add data to Chart Series
                series1.Points.AddXY(size, serializeTime);
                series2.Points.AddXY(size, deserializeTime);
            }

            // Save Chart to file
            string path = @"C:\Users\alexholyshift\Documents\chart.png";
            chart.SaveImage(path, ChartImageFormat.Png);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace monitor
{
    public partial class MainForm : Form
    {
        static public string Exeptions { get; set; } = null;
        static private string _date;
        string url;
        private string _key = "9a06c1e545d005b9b5f722434665d5d4";
        private string _base = "RUB";   
        List<string> listValue = new List<string>();
        Random rnd = new Random();
        private double _sizeZoom = 1;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var countDay = DateTime.Now.Day;

            for (int i = 0; i < countDay - 1; i++)
            {
                /*_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - i).ToString("yyyy-MM-ddT00:00:00"); //"dd / mm / yyyy" 2018-02-12T15:00:00
                url = $"https://currate.ru/api/?get=rates&pairs=USDRUB,EURRUB&key={_key}";//$"http://data.fixer.io/api/{_date}?access_key={_key}&symbols={_base}";
                var request = new GetRequest(url);
                request.Run();
                var json = JObject.Parse(request.Response);
                var value = json["data"]["USDRUB"];*/
                var value = rnd.Next(70000, 75000) / 1000.0;
                listValue.Add(value.ToString());
            }
            listValue.Reverse();

            /*Очищаем график, отрисовываем первые точки и устанавливаем границы по Y*/
            chartEUR.Series[0].Points.Clear();
            chartEUR.Series[1].Points.Clear();

            chartEUR.ChartAreas[0].AxisY.Minimum = double.Parse(listValue.Min()) - 1;
            chartEUR.ChartAreas[0].AxisY.Maximum = double.Parse(listValue.Max()) + 1;

            chartEUR.Series[0].Points.Add(double.Parse(listValue[0]));
            chartEUR.Series[1].Points.Add(double.Parse(listValue[0])).Color = Color.Green;

            for (int i = 1; i < listValue.Count; i++)
            {
                chartEUR.Series[0].Points.Add(double.Parse(listValue[i]));
                if (listValue[i].CompareTo(listValue[i - 1]) > 0)
                {
                    chartEUR.Series[1].Points.Add(double.Parse(listValue[i])).Color = Color.Green;
                    chartEUR.Series[1].Points[i].LabelForeColor = Color.Green;
                }
                else
                {
                    chartEUR.Series[1].Points.Add(double.Parse(listValue[i])).Color = Color.Red;
                    chartEUR.Series[1].Points[i].LabelForeColor = Color.Red;
                }
            }
        }
        private void chartEUR_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                chartEUR.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                _sizeZoom = 1;
            }
            else if (e.Button == MouseButtons.Left)
            {
                var res = chartEUR.HitTest(e.X, e.Y);
                if (res.Series != null && res.Series == chartEUR.Series[1] && res.PointIndex!= 0)
                {
                    var difference = res.Series.Points[res.PointIndex].YValues[0] - res.Series.Points[res.PointIndex - 1].YValues[0];
                    res.Series.Points[res.PointIndex].Label = $"{(difference / res.Series.Points[res.PointIndex - 1].YValues[0] * 100).ToString("0.00")}%\r\n{difference.ToString("0.00")} руб.";
                }
            }
        }

        private void chartEUR_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                chartEUR.ChartAreas[0].AxisX.ScaleView.Zoom(_sizeZoom, 10);
                _sizeZoom *= 2;
            }
        }
    }
}

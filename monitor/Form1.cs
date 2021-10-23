using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace monitor
{
    public partial class MainForm : Form
    {
        static public string Exeptions { get; set; } = null;
        static private string _date;
        string url; //  http://www.cbr.ru/scripts/XML_daily.asp?date_req=02/03/2002
        private string _key = "9a06c1e545d005b9b5f722434665d5d4";
        private string _base = "RUB";   
        List<string> listValue = new List<string>();
        Random rnd = new Random();

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
            chartEUR.ChartAreas[0].AxisY.Minimum = double.Parse(listValue.Min()) - 1;
            chartEUR.ChartAreas[0].AxisY.Maximum = double.Parse(listValue.Max()) + 1;
            chartEUR.Series[0].Points.Clear();
            chartEUR.Series[1].Points.Clear();

            chartEUR.Series[0].Points.Add(double.Parse(listValue[0]));
            chartEUR.Series[1].Points.Add(double.Parse(listValue[0])).Color = Color.Green;
            chartEUR.Series[1].Points[0].MarkerSize = 10;

            for (int i = 1; i < listValue.Count; i++)
            {
                chartEUR.Series[0].Points.Add(double.Parse(listValue[i]));
                if (listValue[i].CompareTo(listValue[i - 1]) > 0)
                    chartEUR.Series[1].Points.Add(double.Parse(listValue[i])).Color = Color.Green;
                else chartEUR.Series[1].Points.Add(double.Parse(listValue[i])).Color = Color.Red;
            }
        }

        private void chartEUR_Click(object sender, EventArgs e)
        {
            chartEUR.ChartAreas[0].InnerPlotPosition.Width = chartEUR.ChartAreas[0].InnerPlotPosition.Width * ((float)1.1);
        }
    }
}

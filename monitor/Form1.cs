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
    public partial class Form1 : Form
    {
        static public string Exeptions { get; set; } = null;
        static private string _date;
        string url; //  http://www.cbr.ru/scripts/XML_daily.asp?date_req=02/03/2002
        List<string> listValue = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var countDay = DateTime.Now.Day;

            for (int i = 0; i < countDay; i++)
            {
                _date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - i).ToString("yyyy-MM-ddT00:00:00"); //"dd / mm / yyyy" 
                url = $"https://www.cbr-xml-daily.ru/daily_json.js?date_req={_date}";
                var request = new GetRequest(url);
                request.Run();
                var json = JObject.Parse(request.Response);
                var value = json["Valute"]["USD"]["Value"];
                listValue.Add(value.ToString());
            }
            listValue.Reverse(); 
        }
    }
}

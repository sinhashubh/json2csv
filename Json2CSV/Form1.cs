using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using CsvHelper;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Json2CSV
{
    public partial class Form1 : Form
    {
        DataTable dw = new DataTable();
        List<JObject> job = new List<JObject>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string filelocation =  textBox1.Text;
                string exportfilename =  textBox2.Text;
                using (StreamReader r = new StreamReader(filelocation))
                {
                    string json = r.ReadToEnd();
                    string tolocation ="";
                    File.WriteAllText(tolocation + exportfilename, jsonToCSV(json, ","));
                }
                label1.Text = "Converted";
            }
            catch (Exception r)
            {
                label1.Text = r.ToString();
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public DataTable jsonStringToTable(string jsonContent)
        {
            JObject stuff = JObject.Parse(jsonContent);
            int i = 0;

            recurse(i, jsonContent);
            
            foreach (JObject r in job)
            {
                DataRow dr = dw.NewRow();

                foreach (var k in r)
                {
                    dr[k.Key] = k.Value;
                }
                dw.Rows.Add(dr);
            }

            return dw;
        }

        public JObject recur(JObject j, JObject newjo, string innerkey, int i)
        {
            IDictionary<string, JToken> Jsondata = j;
            foreach (KeyValuePair<string, JToken> element in Jsondata)
            {
                string innerKey = innerkey + element.Key;
                if (element.Value is JArray)
                {
                    recur(element.Value as JArray, innerKey, i);
                }
                else if (element.Value is JObject)
                {
                    newjo = recur(element.Value as JObject, newjo, innerKey, i);
                }
                else
                {

                    newjo.Add(innerKey, element.Value);
                    if (i == 0)
                    {
                        dw.Columns.Add(innerKey);
                    }
                }
            }
            return newjo;
        }
        public void recur(JArray j, string innerkey, int i)
        {
            int jarr0 = 0;
            int k = i;
            foreach (var jo in j)
            {

                
                    if (jarr0 == 0)
                        k = i;
                    else k = 2;
                    if (jo is JArray)
                    {
                        recur(jo as JArray, innerkey, k);
                    }
                    else if (jo is JObject)
                    {
                        JObject newjo = new JObject();
                        newjo = recur(jo as JObject, newjo, innerkey, k);
                        job.Add(newjo);
                    }
                    else
                    {
                    }
                    jarr0++;
                
            }
        }
        public void recurse( int i, string key)
        {

            JObject newjo = new JObject();
            IDictionary<string, JToken> Jsondata = JObject.Parse(key);
            foreach (KeyValuePair<string, JToken> element in Jsondata)
            {
                string innerKey = element.Key;
                if (element.Value is JArray)
                {
                    recur(element.Value as JArray, innerKey, i);
                }
                else if (element.Value is JObject)
                {
                    recur(element.Value as JObject, newjo, innerKey, i);
                }
            }
        }
        public string jsonToCSV(string jsonContent, string delimiter)
        {
            StringWriter csvString = new StringWriter();
            using (var csv = new CsvWriter(csvString))
            {
                csv.Configuration.Delimiter = delimiter;

                using (var dt = jsonStringToTable(jsonContent))
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        csv.WriteField(column.ColumnName);
                    }
                    csv.NextRecord();

                    foreach (DataRow row in dt.Rows)
                    {
                        for (var i = 0; i < dt.Columns.Count; i++)
                        {
                            csv.WriteField(row[i]);
                        }
                        csv.NextRecord();
                    }
                }
            }
            return csvString.ToString();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

public class Currency
{
    public string CharCode { get; set; }
    public string Name { get; set; }
    public double Value { get; set; }
    public double Previous { get; set; }
}

public class CurrencyResponse
{
    public Dictionary<string, Currency> Valute { get; set; }
}


namespace CurrencyApp
{
    public partial class Form1 : Form
    {
        private const string ApiUrl = "https://www.cbr-xml-daily.ru/daily_json.js";

        public Form1()
        {
            InitializeComponent();
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "Код валюты";
            dataGridView1.Columns[1].Name = "Полное название";
            dataGridView1.Columns[2].Name = "Текущее значение";
            dataGridView1.Columns[3].Name = "Прошлое значение";
        }

        private async void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var currencies = await FetchCurrenciesAsync();
                dataGridView1.Rows.Clear();
                int count = 0;
                foreach (var currency in currencies.Values)
                {
                    if (count >= 10) break;
                    dataGridView1.Rows.Add(currency.CharCode, currency.Name, currency.Value, currency.Previous);
                    count++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private async Task<CurrencyResponse> FetchCurrenciesAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(ApiUrl);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CurrencyResponse>(content);
            }
        }
    }
}
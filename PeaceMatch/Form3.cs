using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeaceMatch
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            Database.LoadPersons();

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public void DisplayCityStatisticsByCountry(string country)
        {
            dataGridView1.ClearSelection();

            var countryPersons = Database.allPersons.Where(p => p.Country.Equals(country, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!countryPersons.Any())
            {
                MessageBox.Show($"No data found for {country}");
                return;
            }

            var countryTotals = new
            {
                TotalFood = countryPersons.Sum(p => p.Food),
                TotalWater = countryPersons.Sum(p => p.Water),
                TotalClothes = countryPersons.Sum(p => p.Clothes),
                TotalShelter = countryPersons.Sum(p => p.Shelter),
                TotalWarmth = countryPersons.Sum(p => p.Warmth),
                TotalSleep = countryPersons.Sum(p => p.SleepEssentials),
                TotalSanitary = countryPersons.Sum(p => p.SanitaryProducts),
                TotalFemaleSanitary = countryPersons.Sum(p => p.FemaleSanitaryProducts)
            };

            var cityStats = countryPersons
                .GroupBy(p => p.City)
                .Select(g => new
                {
                    City = g.Key,
                    FoodPercent = countryTotals.TotalFood > 0 ?
                        Math.Round((double)g.Sum(p => p.Food) / countryTotals.TotalFood * 100, 2) : 0,
                    WaterPercent = countryTotals.TotalWater > 0 ?
                        Math.Round((double)g.Sum(p => p.Water) / countryTotals.TotalWater * 100, 2) : 0,
                    ClothesPercent = countryTotals.TotalClothes > 0 ?
                        Math.Round((double)g.Sum(p => p.Clothes) / countryTotals.TotalClothes * 100, 2) : 0,
                    ShelterPercent = countryTotals.TotalShelter > 0 ?
                        Math.Round((double)g.Sum(p => p.Shelter) / countryTotals.TotalShelter * 100, 2) : 0,
                    WarmthPercent = countryTotals.TotalWarmth > 0 ?
                        Math.Round((double)g.Sum(p => p.Warmth) / countryTotals.TotalWarmth * 100, 2) : 0,
                    SleepPercent = countryTotals.TotalSleep > 0 ?
                        Math.Round((double)g.Sum(p => p.SleepEssentials) / countryTotals.TotalSleep * 100, 2) : 0,
                    SanitaryPercent = countryTotals.TotalSanitary > 0 ?
                        Math.Round((double)g.Sum(p => p.SanitaryProducts) / countryTotals.TotalSanitary * 100, 2) : 0,
                    FemaleSanitaryPercent = countryTotals.TotalFemaleSanitary > 0 ?
                        Math.Round((double)g.Sum(p => p.FemaleSanitaryProducts) / countryTotals.TotalFemaleSanitary * 100, 2) : 0
                })
                .OrderBy(c => c.City)
                .ToList();

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = cityStats;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                if (column.Name.EndsWith("Percent"))
                {
                    column.DefaultCellStyle.Format = "0.00'%'";
                    column.HeaderText = column.Name.Replace("Percent", "") + " (%)";
                }
            }

            dataGridView1.Columns["City"].DisplayIndex = 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            bool found = false;
            foreach (Person p in Database.allPersons)
            {
                if (textBox1.Text.ToLower() == p.Country.ToLower())
                {
                    DisplayCityStatisticsByCountry(p.Country);
                    found = true;
                }
            }
            if (!found)
            {
                MessageBox.Show("country not found");
            }
        }
    }
}


public class Person
{
    public string FullName { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public int Food { get; set; }
    public int Water { get; set; }
    public int Clothes { get; set; }
    public int Shelter { get; set; }
    public int Warmth { get; set; }
    public int SleepEssentials { get; set; }
    public int SanitaryProducts { get; set; }
    public int FemaleSanitaryProducts { get; set; }

    // Optional constructor for easy initialization
    public Person(string fullName, string country, string city,
                 int food, int water, int clothes, int shelter,
                 int warmth, int sleepEssentials, int sanitaryProducts,
                 int femaleSanitaryProducts)
    {
        FullName = fullName;
        Country = country;
        City = city;
        Food = food;
        Water = water;
        Clothes = clothes;
        Shelter = shelter;
        Warmth = warmth;
        SleepEssentials = sleepEssentials;
        SanitaryProducts = sanitaryProducts;
        FemaleSanitaryProducts = femaleSanitaryProducts;
    }

    // Default constructor
    public Person() { }
}

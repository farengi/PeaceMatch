using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PeaceMatch
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Database.LoadCountries();
            Database.LoadCities();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        public static bool IsDigit(string input)
        {
            return input.All(char.IsDigit);
        }
        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public static bool IsValidName(string name)
        {
            return Regex.IsMatch(name, @"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$");
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            bool validName = false;
            bool countryFound = false;
            bool cityFound = false;
            bool validFood = false;
            bool validWater = false;
            bool validClothes = false;
            bool validShelter = false;
            bool validWarmth = false;
            bool validSleep = false;
            bool validSanitary = false;
            bool validFemale = false;

            int count = 0;
            try
            {
                if (textBox1.Text != "" && IsValidName(textBox1.Text))
                {
                    foreach (char c in textBox1.Text)
                    {
                        if (c == ' ')
                        {
                            count++;
                        }
                    }
                    if (count == 1)
                    {
                        validName = true;
                    }
                    else
                    {
                        MessageBox.Show("Name must contain one space");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid name!");

                }


                foreach (string country in Database.countries)
                {
                    if (country.ToUpper() == textBox2.Text.ToUpper())
                    {
                        countryFound = true;
                    }
                }
                if (!countryFound)
                {
                    MessageBox.Show("Invalid country!");
                }

                foreach (var cityTuple in Database.cities)
                {

                    if (cityTuple.Item1.ToUpper() == textBox3.Text.ToUpper()
                        && cityTuple.Item2.ToUpper() == textBox2.Text.ToUpper())
                    {
                        cityFound = true;
                    }
                }
                if (!cityFound)
                {
                    MessageBox.Show("Invalid/Unmatching city!");
                }

                if (textBox4.Text != "" && IsDigit(textBox4.Text) && int.Parse(textBox4.Text) >= 0)
                {
                    validFood = true;
                }
                else
                {
                    MessageBox.Show("Invalid Food!");

                }

                if (textBox5.Text != "" && IsDigit(textBox5.Text) && int.Parse(textBox5.Text) >= 0)
                {
                    validWater = true;
                }
                else
                {
                    MessageBox.Show("Invalid Food!");

                }

                if (textBox6.Text != "" && IsDigit(textBox6.Text) && int.Parse(textBox6.Text) >= 0)
                {
                    validClothes = true;
                }
                else
                {
                    MessageBox.Show("Invalid Clothes!");
                }

                if (textBox7.Text != "" && (int.Parse(textBox7.Text) == 0 || int.Parse(textBox7.Text) == 1))
                {
                    validShelter = true;
                }
                else
                {
                    MessageBox.Show("Invalid Shelter!");
                }

                if (textBox8.Text != "" && IsDigit(textBox8.Text) && int.Parse(textBox8.Text) >= 0)
                {
                    validWarmth = true;
                }
                else
                {
                    MessageBox.Show("Invalid warmth!");
                }

                if (textBox9.Text != "" && IsDigit(textBox9.Text) && int.Parse(textBox9.Text) >= 0)
                {
                    validSleep = true;
                }
                else
                {
                    MessageBox.Show("Invalid sleep!");
                }

                if (textBox10.Text != "" && IsDigit(textBox10.Text) && int.Parse(textBox10.Text) >= 0)
                {
                    validSanitary = true;
                }
                else
                {
                    MessageBox.Show("Invalid sanitary!");

                }

                if (textBox11.Text != "" && IsDigit(textBox11.Text) && int.Parse(textBox11.Text) >= 0)
                {
                    validFemale = true;
                }
                else
                {
                    MessageBox.Show("Invalid Female!");

                }

                if (validName && countryFound && cityFound && validFood && validWater && validClothes && validShelter &&
                    validWarmth && validSleep && validSanitary && validFemale)
                {
                    Database.InsertPerson(textBox1.Text, textBox2.Text, textBox3.Text, int.Parse(textBox4.Text)
                        , int.Parse(textBox5.Text), int.Parse(textBox6.Text), int.Parse(textBox7.Text),
                        int.Parse(textBox8.Text), int.Parse(textBox9.Text), int.Parse(textBox10.Text),
                        int.Parse(textBox11.Text));
                    MessageBox.Show("Person Successfully inserted");
                }
              
            }
            catch
            {
                MessageBox.Show("Error, check what you did wrong!");
            }

        }
    }
}

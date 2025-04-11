using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
namespace PeaceMatch
{
    class Database
    {
        public static void LoadUsersFromDB()
        {
            Form1.Users.Clear();
            string connectionString = "server=127.0.0.1; database=peacematch; uid=root; pwd=root; port=3306;";
            Form1.Users.Clear(); // Clear the list before loading

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT email, password FROM user";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                try
                {
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string email = reader.GetString(0);
                        string password = reader.GetString(1);

                        Form1.Users.Add(new User(email, password));
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        public static List<string> countries = new List<string>();

        public static void LoadCountries()
        {
            countries.Clear();
            string connectionString = "server=127.0.0.1; database=peacematch; uid=root; pwd=root; port=3306;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT name FROM country";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                try
                {
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        countries.Add(reader.GetString(0)); // Get country name
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public static List<Tuple<string, string>> cities = new List<Tuple<string, string>>();

        public static void LoadCities()
        {
            cities.Clear();
            string connectionString = "server=127.0.0.1; database=peacematch; uid=root; pwd=root; port=3306;";


            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT name, country_name FROM city";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                try
                {
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string cityName = reader.GetString(0);
                        string countryName = reader.GetString(1);
                        cities.Add(new Tuple<string, string>(cityName, countryName));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public static void InsertPerson(
            string fullName, string country, string city,
            int food, int water, int clothes, int shelter,
            int warmth, int sleepEssentials, int sanitaryProducts,
            int femaleSanitaryProducts)
        {
            string connectionString = "server=127.0.0.1; database=peacematch; uid=root; pwd=root; port=3306;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = @"INSERT INTO person 
                                (full_name, country, city, Food, Water, Clothes, Shelter, Warmth, Sleep_essentials, Sanitary_products, Female_sanitary_products)
                                VALUES (@fullName, @country, @city, @food, @water, @clothes, @shelter, @warmth, @sleepEssentials, @sanitaryProducts, @femaleSanitaryProducts)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@fullName", fullName);
                cmd.Parameters.AddWithValue("@country", country);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Parameters.AddWithValue("@food", food);
                cmd.Parameters.AddWithValue("@water", water);
                cmd.Parameters.AddWithValue("@clothes", clothes);
                cmd.Parameters.AddWithValue("@shelter", shelter);
                cmd.Parameters.AddWithValue("@warmth", warmth);
                cmd.Parameters.AddWithValue("@sleepEssentials", sleepEssentials);
                cmd.Parameters.AddWithValue("@sanitaryProducts", sanitaryProducts);
                cmd.Parameters.AddWithValue("@femaleSanitaryProducts", femaleSanitaryProducts);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Person inserted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error inserting person: " + ex.Message);
                }
            }
        }

        public static List<Person> allPersons = new List<Person>();
        public static void LoadPersons()
        {
            allPersons.Clear();
            string connectionString = "server=127.0.0.1; database=peacematch; uid=root; pwd=root; port=3306;";


            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT full_name, country, city, Food, Water, Clothes, Shelter, " +
                                  "Warmth, Sleep_essentials, Sanitary_products, Female_sanitary_products " +
                                  "FROM person";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                try
                {
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Person person = new Person
                        {
                            FullName = reader["full_name"].ToString(),
                            Country = reader["country"].ToString(),
                            City = reader["city"].ToString(),
                            Food = Convert.ToInt32(reader["Food"]),
                            Water = Convert.ToInt32(reader["Water"]),
                            Clothes = Convert.ToInt32(reader["Clothes"]),
                            Shelter = Convert.ToInt32(reader["Shelter"]),
                            Warmth = Convert.ToInt32(reader["Warmth"]),
                            SleepEssentials = Convert.ToInt32(reader["Sleep_essentials"]),
                            SanitaryProducts = Convert.ToInt32(reader["Sanitary_products"]),
                            FemaleSanitaryProducts = Convert.ToInt32(reader["Female_sanitary_products"])
                        };

                        allPersons.Add(person);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }






    }
}

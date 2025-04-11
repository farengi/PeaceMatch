namespace PeaceMatch
{
    public partial class Form1 : Form
    {
        public static List<User> Users = new List<User>(); // Static list of users

        public Form1()
        {
            InitializeComponent();
            Database.LoadUsersFromDB();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool found = false;
            foreach (User u in Users)
            {
                if (u.Email == textBox1.Text && u.Password == textBox2.Text)
                {
                    Form2 form2 = new Form2();
                    form2.Show();

                    found = true;
                }
            }
            if (textBox1.Text == "admin" && textBox2.Text == "pass123")
            {
                Form3 form3 = new Form3();
                form3.Show();
                found = true;
            }
            if (!found)
            {
                MessageBox.Show("no user found");
            }


        }


    }
}

public class User
{
    public string Email { get; set; }
    public string Password { get; set; }

    // Constructor
    public User(string email, string password)
    {
        Email = email;
        Password = password;
    }

    // Method to display user info (without password for security)
    public void DisplayUserInfo()
    {
        Console.WriteLine($"Email: {Email}");
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task_manager_v2
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            if (username.Length == 0 || password.Length == 0)
            {
                MessageBox.Show("Username and password cannot be empty.");
                return;
            }
            string filePath = "task_manager_users.txt";
            if (!File.Exists(filePath))
            {
                MessageBox.Show("No users found.");
                return;
            }
            string[] lines = File.ReadAllLines(filePath);
            bool found = false;
            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts[0] == username)
                {
                    found = true;
                    if (parts[1] != password)
                    {
                        MessageBox.Show("Incorrect password.");
                        return;
                    }
                    break;
                }
            }
            if (!found)
            {
                MessageBox.Show("Username not found.");
                return;
            }
            MainForm mainForm = new MainForm(username);
            mainForm.Show();
            this.Hide();
        }
        
    

        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            if (username.Length == 0 || password.Length == 0)
            {
                MessageBox.Show("Username and password cannot be empty.");
                return;
            }
            string filePath = "task_manager_users.txt";
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts[0] == username)
                {
                    MessageBox.Show("Username already exists.");
                    return;
                }
            }
            File.AppendAllText(filePath, username + ";" + password + Environment.NewLine);
            string tasksFile = username + "_tasks.txt";
            if (!File.Exists(tasksFile))
            {
                File.Create(tasksFile);
            }
            MessageBox.Show("User created successfully.");
        } 
    }
}

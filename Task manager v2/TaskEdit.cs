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
    public partial class TaskEdit : Form
    {
        private string _username;
        private string _name;
        private string _artist;
        private DateTime _date;
        private string _description;

        public TaskEdit(string username, string name, string artist, DateTime date, string description)
        {
            InitializeComponent();
            _username = username;
            _name = name;
            _artist = artist;
            _date = date;
            _description = description;

            textBox1.Text = _name;
            textBox2.Text = _artist;
            dateTimePicker1.Value = _date;
            textBox3.Text = _description;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string taskName = textBox1.Text;
            string taskArtist = textBox2.Text;
            string taskDate = dateTimePicker1.Value.ToString("dd-MM-yyyy");
            string taskDescription = textBox3.Text;
            if (taskName.Length == 0 || taskArtist.Length == 0 || taskDate.Length == 0 || taskDescription.Length == 0)
            {
                MessageBox.Show("All fields are required.");
                return;
            }
            string filePath = _username + "_tasks.txt";
            File.AppendAllText(filePath, "to_do" + ";" + taskName + ";" + taskArtist + ";" + taskDate + ";" + taskDescription + Environment.NewLine);
            MessageBox.Show("Task added successfully.");
            this.Close();
        }
    }
}

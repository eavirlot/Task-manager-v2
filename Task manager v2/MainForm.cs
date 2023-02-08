using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Task_manager_v2
{
    public partial class MainForm : Form
    {
        private string _username;
        private string _originalTaskName;
        public MainForm(string username)
        {
            InitializeComponent();
            _username = username;
            LoadTasks();
            listBox1.DoubleClick += ListBox_DoubleClick;
            listBox2.DoubleClick += ListBox_DoubleClick;
            listBox3.DoubleClick += ListBox_DoubleClick;
        }

        private void ListBox_DoubleClick(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox.SelectedItem == null)
            {
                return;
            }
            
            string name = listBox.SelectedItem.ToString();
            _originalTaskName = listBox.SelectedItem.ToString();
            //Console.WriteLine(_originalTaskName);
            string filePath = _username + "_tasks.txt";
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts.Length != 5)
                {
                    continue;
                }

                if (parts[1] == name)
                {
                    
                    string status = parts[0];
                    string artist = parts[2];
                    string date = parts[3];
                    DateTime taskDate = DateTime.Parse(date);
                    string description = parts[4];

                    TaskEdit taskEdit = new TaskEdit(_username, status, name, artist, taskDate, description, _originalTaskName);
                    taskEdit.IsEditMode = true;
                    taskEdit.ShowDialog();
                    LoadTasks();
                    break;
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        private void add_task_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Now;
            TaskEdit taskEdit = new TaskEdit(_username,"", "", "", today,"","");
            taskEdit.IsEditMode = false;
            taskEdit.ShowDialog();
        }
        private void LoadTasks()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();

            string filePath = _username + "_tasks.txt";
            if (!File.Exists(filePath))
            {
                return;
            }

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts.Length != 5)
                {
                    continue;
                }

                string status = parts[0];
                string name = parts[1];
                string artist = parts[2];
                string date = parts[3];
                string description = parts[4];

                switch (status)
                {
                    case "to_do":
                        listBox1.Items.Add(name);
                        break;
                    case "InProgress":
                        listBox2.Items.Add(name);
                        break;
                    case "Done":
                        listBox3.Items.Add(name);
                        break;
                }
            }
        }
    }
}

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
        private Rectangle dragBoxFromMouseDown;

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
            TaskEdit taskEdit = new TaskEdit(_username, "", "", "", today, "", "");
            taskEdit.IsEditMode = false;
            taskEdit.ShowDialog();
            LoadTasks();
        }
        public void LoadTasks()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();

            string filePath = _username + "_tasks.txt";
            if (!File.Exists(filePath))
            {
                return;
            }
            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show("Это не нормально, но прошу, просто перезапустите программу, авторизуйтесь с теми же данными. Ошибка: " + ex.Message);
            }
        }




        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            string taskName = e.Data.GetData(DataFormats.StringFormat).ToString();
            string status = "to_do";
            UpdateTaskStatus(taskName, status);
            LoadTasks();
        }

        private void listBox2_DragDrop(object sender, DragEventArgs e)
        {
            string taskName = e.Data.GetData(DataFormats.StringFormat).ToString();
            string status = "InProgress";
            UpdateTaskStatus(taskName, status);
            LoadTasks();
        }

        private void listBox3_DragDrop(object sender, DragEventArgs e)
        {
            string taskName = e.Data.GetData(DataFormats.StringFormat).ToString();
            string status = "Done";
            UpdateTaskStatus(taskName, status);
            LoadTasks();
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void listBox2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void listBox3_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Get the size of the drag rectangle.
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            }
        }

        private void listBox2_MouseDown(object sender, MouseEventArgs e)
        {

            // Get the size of the drag rectangle.
            Size dragSize = SystemInformation.DragSize;

            // Create a rectangle using the DragSize, with the mouse position being
            // at the center of the rectangle.
            dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);

        }

        private void listBox3_MouseDown(object sender, MouseEventArgs e)
        {

                 // Get the size of the drag rectangle.
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);

        }

        private void UpdateTaskStatus(string taskName, string status)
        {
            string filePath = _username + "_tasks.txt";
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(';');
                if (parts.Length != 5)
                {
                    continue;
                }

                if (parts[1] == taskName)
                {
                    lines[i] = status + ";" + taskName + ";" + parts[2] + ";" + parts[3] + ";" + parts[4];
                    break;
                }
            }

            File.WriteAllLines(filePath, lines);
        }

        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    ListBox listBox = sender as ListBox;
                    if (listBox.SelectedItem == null)
                    {
                        return;
                    }

                    string taskName = listBox.SelectedItem.ToString();
                    DragDropEffects result = listBox.DoDragDrop(taskName, DragDropEffects.Copy);
                }
            }
        }

        private void listBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    ListBox listBox = sender as ListBox;
                    if (listBox.SelectedItem == null)
                    {
                        return;
                    }

                    string taskName = listBox.SelectedItem.ToString();
                    DragDropEffects result = listBox.DoDragDrop(taskName, DragDropEffects.Copy);
                }
            }
        }

        private void listBox3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    ListBox listBox = sender as ListBox;
                    if (listBox.SelectedItem == null)
                    {
                        return;
                    }

                    string taskName = listBox.SelectedItem.ToString();
                    DragDropEffects result = listBox.DoDragDrop(taskName, DragDropEffects.Copy);
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void delete_Click(object sender, EventArgs e)
        {
            ListBox listBox = listBox3;
            if (listBox.SelectedItem == null)
            {
                return;
            }
            string taskName = listBox.SelectedItem.ToString();
            string filePath = _username + "_tasks.txt";
            string[] lines = File.ReadAllLines(filePath);
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length != 5)
                    {
                        continue;
                    }
                    if (parts[1] == taskName)
                    {
                        continue;
                    }
                    writer.WriteLine(line);
                }
            }
            LoadTasks();
        }
    }
}

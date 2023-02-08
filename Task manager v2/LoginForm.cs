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
                MessageBox.Show("Имя пользователя или пароль не могут быть пустыми");
                return;
            }
            string filePath = "task_manager_users.txt";
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Пользователь не найден");
                return;
            }
            try
            {
                using (var fileStream = new StreamReader(filePath))
                {
                    bool found = false;
                    while (!fileStream.EndOfStream)
                    {
                        string line = fileStream.ReadLine();
                        string[] parts = line.Split(';');
                        if (parts[0] == username)
                        {
                            found = true;
                            if (parts[1] != password)
                            {
                                MessageBox.Show("Неправильный пароль");
                                return;
                            }
                            break;
                        }
                    }
                    if (!found)
                    {
                        MessageBox.Show("Пользователь не найден");
                        return;
                    }
                    MainForm mainForm = new MainForm(username);
                    mainForm.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            if (username.Length == 0 || password.Length == 0)
            {
                MessageBox.Show("Имя пользователя или пароль не могут быть пустыми");
                return;
            }
            string filePath = "task_manager_users.txt";
            try
            {
                using (var fileStream = File.OpenRead(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);

                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(';');
                        if (parts[0] == username)
                        {
                            MessageBox.Show("Имя пользователя уже используется");
                            return;
                        }
                    }
                }
                string tasksFile = username + "_tasks.txt";
                if (!File.Exists(tasksFile))
                {
                    File.Create(tasksFile).Close();
                }

                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(username + ";" + password);
                }

                MessageBox.Show("Пользователь успешно создан");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Это не нормально, но прошу, просто перезапустите программу и авторизуйтесь с теми же данными. Ошибка: " + ex.Message);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            string filePath = "task_manager_users.txt";
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
          
        }
    }
}

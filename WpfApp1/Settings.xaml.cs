using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private static string SettingPath = AppDomain.CurrentDomain.BaseDirectory + "Setting.Config";


        public Settings()
        {
            InitializeComponent();
            Loaded += Settings_Loaded;
        }

        private void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                StreamReader fs = new StreamReader(SettingPath);
                string setting = "";
                while (true)
                {
                    string temp = fs.ReadLine();
                    if (temp == null) break;
                    setting += temp;
                }
                Path.Text = setting;
            }
            catch
            {
            }
        }

        private void GetPath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openDialog = new FolderBrowserDialog();
            openDialog.ShowDialog();
            Path.Text = openDialog.SelectedPath;
        }

        private void SaveSetting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Path.Text != "")
                {
                    using (StreamWriter sw = new StreamWriter(SettingPath, false, Encoding.Default))
                    {
                        sw.WriteLine(Path.Text);
                    }
                    string message = "Настройки сохранены";
                    new Notifications.Notif(message).ShowDialog();
                    MainWindow window = new MainWindow();
                    window.checkApps();
                    Close();
                }
                else
                {
                    string message = "Заполните все поля!";
                    new Notifications.Notif(message).ShowDialog();
                }
            }
            catch (Exception)
            {
                string message = "Ошибка сохранения настроек";
                new Notifications.Notif(message).ShowDialog();
                Close();
            }
        }
        #region Form control 
        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
        private void BdClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
    }
}

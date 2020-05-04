using AppVersionControl.Domain.ApplicationVersion;
using AppVersionControl.Domain.Interfaces;
using AppVersionControl.Domain.Iterfaces;
using AppVersionControl.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AddAplication.xaml
    /// </summary>
    public partial class AddAplication : Window
    {
        private string Path = AppDomain.CurrentDomain.BaseDirectory;
        private static string SettingPath = AppDomain.CurrentDomain.BaseDirectory + "Setting.Config";
        public AddAplication()
        {
            InitializeComponent();
            Loaded += AppAplication_loaded;
        }

        private void AppAplication_loaded(object sender, RoutedEventArgs e)
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
                if (setting != "")
                {
                    Path = setting;
                }
            }
            catch { }
            ListApps.ItemsSource = listApp();
        }
        private List<AppVersion> listApp()
        {
            VersionInDBRepo repo_fileDB = new VersionInDBRepo(AppDomain.CurrentDomain.BaseDirectory);
            IList<AppVersion> versionsDB = repo_fileDB.FindAll();
            List<AppVersion> versionsSort = new List<AppVersion>();
            string[] VersionsApps = (from v in versionsDB select v.VersionName.ToString()).Distinct().ToArray();
            foreach (string v1 in VersionsApps)
            {
                versionsSort.Add(repo_fileDB.Find(v1));
            }
            return versionsSort;
        }
        private void ListApps_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                IVersionRepo repo = new VersionInDBRepo(Directory.GetCurrentDirectory());
                AppVersion srcV = repo.Find(((AppVersion)ListApps.SelectedItem).VersionName);
                IFileRepo repoF1 = new FileInDBRepo(AppDomain.CurrentDomain.BaseDirectory);
                IDictionary<FileOfVersion, byte[]> fileContents = repoF1.FindFileOfVersionsWithData(srcV);
                IFileRepo repoF2 = new FileInSystemRepo(Path);
                var isSucces = repoF2.Save(srcV, fileContents);
                string message = "Приложение добавлено";
                new Notifications.Notif(message).ShowDialog();
            }
            catch (Exception)
            {
                string message = "Ошибка обновления приложения";
                new Notifications.Notif(message).ShowDialog();
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
            MainWindow window = new MainWindow();
            window.checkApps();
            Close();
        }
        private void BdСollapse_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                WindowStyle = WindowStyle.None;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }
        private void Btn_min_click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Minimized;
                WindowStyle = WindowStyle.None;
            }
            else
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Minimized;
                    WindowStyle = WindowStyle.None;
                }
                else
                {
                    if (WindowState == WindowState.Minimized)
                    {
                        WindowState = WindowState.Normal;
                    }
                }
            }
        }
        #endregion
    }
}

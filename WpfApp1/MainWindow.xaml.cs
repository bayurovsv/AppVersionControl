using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using AppVersionControl.Domain.ApplicationVersion;
using AppVersionControl.Domain.Interfaces;
using AppVersionControl.Domain.Iterfaces;
using AppVersionControl.Infrastructure;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string Path = AppDomain.CurrentDomain.BaseDirectory;
        private static string SettingPath = AppDomain.CurrentDomain.BaseDirectory + "Setting.Config";

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
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
            checkApps();
        }
        public void checkApps()
        {
            Directory.CreateDirectory(Path + "\\Versions");
            VersionInFileSystemRepo repo_file = new VersionInFileSystemRepo(Path); /// путь брать из конфига
            IList<AppVersion> versions = repo_file.FindAll();
            List<AppVersion> versionsSort = new List<AppVersion>();
            string[] VersionsApps = (from v in versions select v.VersionName.ToString()).Distinct().ToArray();
            foreach (string v1 in VersionsApps)
            {
                versionsSort.Add(repo_file.Find(v1));
            }
            VersionInDBRepo repo_fileDB = new VersionInDBRepo(AppDomain.CurrentDomain.BaseDirectory);
            IList<AppVersion> versionsDB = repo_fileDB.FindAll();
            List<DisplayApplication> Applications = new List<DisplayApplication>();
            foreach (AppVersion ver in versionsSort)
            {
                foreach (AppVersion v in versionsDB)
                {
                    if (ver.VersionName.Contains(v.VersionName))
                    {
                        AppVersion version = repo_file.Find(ver);
                        AppVersion versionDB = repo_fileDB.Find(v);
                        if (version.VersionNumber == versionDB.VersionNumber)
                        {
                            DisplayApplication application = new DisplayApplication();
                            application.ApplicationName = version.VersionName;
                            application.ApplicationNumber = version.VersionNumber.ToString();
                            application.FlagUpdate = "Установленна актуальная версия";
                            Applications.Add(application);
                        }
                        else
                        {
                            DisplayApplication application = new DisplayApplication();
                            application.ApplicationName = version.VersionName;
                            application.ApplicationNumber = version.VersionNumber.ToString();
                            application.FlagUpdate = "Доступно обновление";
                            Applications.Add(application);
                        }
                    }
                }
            }
            ListApps.ItemsSource = Applications;
        }
        private void ListApps_SelectionChanged(object sender, SelectionChangedEventArgs e) // Сохранение версии
        {
            try
            {
                DisplayApplication application = (DisplayApplication)ListApps.SelectedItem;
                if (application.FlagUpdate == "Доступно обновление")
                {
                    IVersionRepo repoV1 = new VersionInDBRepo(Directory.GetCurrentDirectory());
                    AppVersion srcV = repoV1.Find(application.ApplicationName);
                    IFileRepo repoF1 = new FileInDBRepo(AppDomain.CurrentDomain.BaseDirectory);
                    IDictionary<FileOfVersion, byte[]> fileContents = repoF1.FindFileOfVersionsWithData(srcV);
                    IFileRepo repoF2 = new FileInSystemRepo(Path);
                    var isSucces = repoF2.Save(srcV, fileContents);
                    string message = "Приложение обновлено";
                    new Notifications.Notif(message).ShowDialog();
                    checkApps();
                }
            }
            catch (Exception)
            {
                string message = "Приложение не обновлено";
                new Notifications.Notif(message).ShowDialog();
            }

        }
        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
        }

        private void AddApp_Click(object sender, RoutedEventArgs e)
        {
            AddAplication AddAplication = new AddAplication();
            AddAplication.ShowDialog();
        }
        #region Form control 
        private void CloseBar(object sender, MouseButtonEventArgs e)
        {
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }
            MenuButton.IsChecked = false;
        }

        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void BdClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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

        private void Admins_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

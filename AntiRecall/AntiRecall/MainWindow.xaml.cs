using System;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using socks5;
using System.Net;
using System.Diagnostics;
using AntiRecall.deploy;
using AntiRecall.network;
using System.Threading;
using System.IO;
using AntiRecall.patch;

namespace AntiRecall
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {

        private string port;
        private static NotifyIcon ni;
        public static socks5.Socks5Server proxy;
        public static double count { get; set; }
        public static bool is_recallmodule_load { get; set; }

        private void init_minimize()
        {
            MenuItem menuItem1 = new MenuItem();
            ContextMenu contextMenu = new ContextMenu();

            menuItem1.Index = 0;
            menuItem1.Text = "退出";
            menuItem1.Click += new System.EventHandler(menuItem1_Click);
            contextMenu.MenuItems.Add(menuItem1);

            ni = new NotifyIcon();
            ni.Text = "一个万能的防撤回工具";
            ni.ContextMenu = contextMenu;
            ni.Visible = true;
            ni.Icon = MoRecall.Properties.Resources.ic_launcher;;
#if DEBUG
            //ni.Icon = new Icon("../../Resources/ic_launcher.ico");
#else
            //System.IO.Directory.GetCurrentDirectory() = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            //ni.Icon = new Icon(System.IO.Directory.GetCurrentDirectory() + "\\Resources\\ic_launcher.ico");

#endif
            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {

                    this.Show();
                    this.WindowState = WindowState.Normal;
                };
        }

        private void init_socks5()
        {
            if (port!=null)
                proxy = new Socks5Server(IPAddress.Any, Convert.ToInt32(port));
            proxy.PacketSize = 65535;
            proxy.Start();
        }

        private delegate void TextChanger();

        public void ModeCheck()
        {
             if (Xml.antiRElement["Mode"] == "proxy")
            {
                this.PortItem.Foreground = System.Windows.Media.Brushes.Black;
                this.PortText.Foreground = System.Windows.Media.Brushes.Black;
                this.PortText.IsReadOnly = false;
                this.PortItem_Copy.Foreground = System.Windows.Media.Brushes.Black;
                this.PortText_Copy.Foreground = System.Windows.Media.Brushes.Black;
                this.PortText_Copy.IsReadOnly = false;
                this.Proxy_button.IsChecked = true;
                this.Memory_patch_button.IsChecked = false;
                this.Descript_text.Content = "请手动为QQ/TIM设置代理";
                this.Explorer.Foreground = System.Windows.Media.Brushes.Black;
                this.Explorer.IsEnabled = true;
                this.Explorer_Copy.IsEnabled = true;
                this.Explorer_Copy.Foreground = System.Windows.Media.Brushes.Black;
            }

            if (Xml.antiRElement["Mode"] == "patch")
            {
                this.PortItem.Foreground = System.Windows.Media.Brushes.Gray;
                this.PortText.Foreground = System.Windows.Media.Brushes.Gray;
                this.PortText.IsReadOnly = true;
                this.PortItem_Copy.Foreground = System.Windows.Media.Brushes.Black;
                this.PortText_Copy.Foreground = System.Windows.Media.Brushes.Black;
                this.PortText_Copy.IsReadOnly = true;
                this.Proxy_button.IsChecked = false;
                this.Memory_patch_button.IsChecked = true;
                this.Descript_text.Content = "请保证在开启后30秒内登录QQ/TIM";
                this.Explorer.Foreground = System.Windows.Media.Brushes.Black;
                this.Explorer.IsEnabled = true;
                this.Explorer_Copy.IsEnabled = true;
                this.Explorer_Copy.Foreground = System.Windows.Media.Brushes.Black;
            }

            if (-1 != Xml.antiRElement["QQPath"].IndexOf("QQ.exe"))
            {
                this.Explorer.Content = "配置读取完成";
            }
            if (-1 != Xml.antiRElement["TIMPath"].IndexOf("TIM.exe"))
            {
                this.Explorer_Copy.Content = "TIM配置读取完成";
            }
        }
        /*
        private void UpdateCount()
        {
            Regex re = new Regex("\\[\\d*\\]");
#if DEBUG
            Console.WriteLine(count);
#endif
            Recall_Text.Text = re.Replace(Recall_Text.Text, "["+Convert.ToString(Math.Ceiling(count / 8))+"]");
        }

        public void ModifyRecallCount()
        {
            Recall_Text.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new TextChanger(UpdateCount));
        }*/

        
        public MainWindow()
        {
            InitializeComponent();
            //ShortCut.init_shortcut("MoRecall");
            Xml.init_xml();
            CheckUpdate.init_checkUpdate();
            init_minimize();
            if (Xml.CheckXml())
            {
                Xml.antiRElement["PortText"] = Xml.QueryXml("PortText");
                Xml.antiRElement["PortText_Copy"] = Xml.QueryXml("PortText_Copy");
                Xml.antiRElement["QQPath"] = Xml.QueryXml("QQPath");
                Xml.antiRElement["TIMPath"] = Xml.QueryXml("TIMPath");
                Xml.antiRElement["Mode"] = Xml.QueryXml("Mode");
                PortText.Text = Xml.antiRElement["PortText"];
                PortText_Copy.Text = Xml.antiRElement["PortText_Copy"];
            }
            else
            {
                Xml.CreateXml(Xml.antiRElement);
            }
            ModeCheck();
            this.Descript_text.Content = "一个不修改文件的防撤回工具";
        }


        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

            if (this.Start.IsChecked == false)
            {
                if (Xml.antiRElement["Mode"] == "proxy")
                {
                    proxy.Stop();
                }

                if (Xml.antiRElement["Mode"] == "patch")
                {

                }
            }

            if (this.Start.IsChecked == true)
            {
                port = PortText.Text;

                if (Xml.antiRElement["Mode"] == "proxy")
                {
                    init_socks5();
                    //Startup.init_startup();
                    //Modify xml
                    Xml.antiRElement["PortText"] = PortText.Text;
                    if (!Xml.CheckXml())
                        Xml.CreateXml(Xml.antiRElement);
                    else
                        Xml.ModifyXml(Xml.antiRElement);

                }
                else if (Xml.antiRElement["Mode"] == "patch")
                {
                    Xml.antiRElement["PortText"] = PortText.Text;
                    if (!Xml.CheckXml())
                        Xml.CreateXml(Xml.antiRElement);
                    else
                        Xml.ModifyXml(Xml.antiRElement);
                    var th = new Thread(() => patch_memory.StartPatch());
                    th.Start();

                }
                else
                {
                    System.Windows.MessageBox.Show("请选择一个有效的防撤回模式");
                    return;
                }

                if (-1 != Xml.antiRElement["QQPath"].IndexOf("QQ.exe"))
                {
                    try
                    {
                        Process process = new Process();
                        process.StartInfo.FileName = Xml.antiRElement["QQPath"];
                        //process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();

                    }
                    catch (Exception)
                    {
                        System.Windows.MessageBox.Show("启动QQ失败，请确认路径正确或手动启动");
                    }
                    MinimizeWindow();
                }
            }
        }

        private void StartButton_Copy_Click(object sender, RoutedEventArgs e)
        {

            if (this.Start_Copy.IsChecked == false)
            {
                if (Xml.antiRElement["Mode"] == "proxy")
                {
                    proxy.Stop();
                }

                if (Xml.antiRElement["Mode"] == "patch")
                {

                }
            }

            if (this.Start_Copy.IsChecked == true)
            {
                port = PortText_Copy.Text;

                if (Xml.antiRElement["Mode"] == "proxy")
                {
                    init_socks5();
                    //Startup.init_startup();
                    //Modify xml
                    Xml.antiRElement["PortText_Copy"] = PortText_Copy.Text;
                    if (!Xml.CheckXml())
                        Xml.CreateXml(Xml.antiRElement);
                    else
                        Xml.ModifyXml(Xml.antiRElement);

                }
                else if (Xml.antiRElement["Mode"] == "patch")
                {
                    Xml.antiRElement["PortText_Copy"] = PortText_Copy.Text;
                    if (!Xml.CheckXml())
                        Xml.CreateXml(Xml.antiRElement);
                    else
                        Xml.ModifyXml(Xml.antiRElement);
                    var th = new Thread(() => patch_memory.StartPatch());
                    th.Start();

                }
                else
                {
                    System.Windows.MessageBox.Show("请选择一个有效的防撤回模式");
                    return;
                }

                if (-1 != Xml.antiRElement["TIMPath"].IndexOf("TIM.exe"))
                {
                    try
                    {
                        Process process = new Process();
                        process.StartInfo.FileName = Xml.antiRElement["TIMPath"];
                        //process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();

                    }
                    catch (Exception)
                    {
                        System.Windows.MessageBox.Show("启动TIM失败，请确认路径正确或手动启动");
                    }
                    MinimizeWindow();
                }
            }
        }

        private void Explorer_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".exe";
            dlg.Filter = "Executable Files|*.exe|All Files|*.*";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filepath = dlg.FileName;
                Xml.antiRElement["QQPath"] = filepath;

            }
        }

             private void Explorer_Copy_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".exe";
            dlg.Filter = "Executable Files|*.exe|All Files|*.*";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filepath = dlg.FileName;
                Xml.antiRElement["TIMPath"] = filepath;

            }


        }


        private void menuItem1_Click(object Sender, EventArgs e)
        {
            ni.Visible = false;
            if (proxy != null)
                proxy.Stop();
            Close();
        }

        private void MinimizeWindow()
        {
            this.Hide();

            ni.BalloonTipTitle = "MoRecall v1.3";
            ni.BalloonTipText = "已将MoRecall最小化到托盘,程序将在后台运行";
            ni.BalloonTipIcon = ToolTipIcon.Info;
            ni.ShowBalloonTip(30000);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                MinimizeWindow();
            base.OnStateChanged(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            ni.Visible = false;
            if (proxy!=null)
                proxy.Stop();
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            //得到所有打开的进程   
            try
            {
                foreach (Process thisproc in Process.GetProcessesByName("MoRecall"))
                {
                    //找到程序进程,kill之。
                    if (!thisproc.CloseMainWindow())
                    {
                        thisproc.Kill();
                    }
                }

            }
            catch (Exception Exc)
            {
                System.Windows.MessageBox.Show(Exc.Message);
            }
            //base.OnClosed(e);
            //App.Current.Shutdown();
        }


    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;

namespace WpfMediaPlayer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        private bool mediaIsPlaying=false;

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if ((myMediaElement.Source!= null ) && (myMediaElement.NaturalDuration.HasTimeSpan))
            {
                sldrTimer.Minimum = 0;
                sldrTimer.Maximum = myMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                sldrTimer.Value = myMediaElement.Position.TotalSeconds;
            }         
        }

        private void OpenCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCmdExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Media files (*.mp3; *.mp4; *.mpeg)|*.mp3; *.mp4; *.mpeg|All files (*.*)|*.*";
            if (openDialog.ShowDialog()== true)
            {
                myMediaElement.Source = new Uri(openDialog.FileName);
                myMediaElement.LoadedBehavior = MediaState.Manual;
                myMediaElement.UnloadedBehavior = MediaState.Manual;

                myMediaElement.Volume = sldrVolume.Value;
                sldrTimer.Value = 0;
                myMediaElement.Play();
                mediaIsPlaying = true;
            }
        }
        private void menuExitApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if ((myMediaElement != null) && (myMediaElement.Source != null))
            {
                myMediaElement.Play();
                mediaIsPlaying = true;
            }            
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if ((myMediaElement != null) && (myMediaElement.Source != null))
            {
                myMediaElement.Pause();
                mediaIsPlaying = false;
            }
                
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if ((myMediaElement != null) && (myMediaElement.Source != null))
            {
                myMediaElement.Stop();
                sldrTimer.Value = 0;
            }               
        }

        private void sldrVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            myMediaElement.Volume = sldrVolume.Value;
        }

        private void sldrTimer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            myMediaElement.Position = TimeSpan.FromSeconds(sldrTimer.Value);
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            string fileName = ((DataObject)e.Data).GetFileDropList()[0];
            myMediaElement.Source = new Uri(fileName);

            myMediaElement.LoadedBehavior = MediaState.Manual;
            myMediaElement.UnloadedBehavior = MediaState.Manual;

            myMediaElement.Volume = sldrVolume.Value;
            myMediaElement.Play();
            mediaIsPlaying = true;
        }

        private void myMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan ts = myMediaElement.NaturalDuration.TimeSpan;
            sldrTimer.Maximum = ts.TotalSeconds;
        }

        private void myMediaElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mediaIsPlaying)
            {
                myMediaElement.Pause();
                mediaIsPlaying = false;
            }

            else
            {
                myMediaElement.Play();
                mediaIsPlaying = true;
            }                              
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta>0)
            {
                myMediaElement.Volume += 0.01;
                sldrVolume.Value += 0.01;
            }

            else
            {
                myMediaElement.Volume -= 0.01;
                sldrVolume.Value -= 0.01;
            }
        }
    }
}

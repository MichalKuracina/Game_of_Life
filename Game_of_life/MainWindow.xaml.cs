using Game_of_life.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Game_of_life
{
    public partial class MainWindow : Window
    {
        public static DispatcherTimer timer = new DispatcherTimer();

        public List<List<NewRect>> State1 { get; set; }
        public List<List<NewRect>> State2 { get; set; }
        public List<List<NewRect>> State3 { get; set; }
        public World World { get; set; }
        Counter myCounter;

        public MainWindow()
        {
            InitializeComponent();

            myCounter = new Counter();
            sbar.DataContext = myCounter;
                     
            int x = 30;
            int y = 30;

            World = new World(myGrid, x, y);

            timer.Tick += Go;
            timer.Interval = new TimeSpan(0,0,0,0,500);

            World.design();
            State1 = World.create();

            timer.Start();
        }

        private void Go(object sender, EventArgs e)
        {
            State2 = World.create();
            State3 = World.mutate(State1, State2);
            World.draw(State3);
            State1 = State3;

            myCounter.Count = World.CountLives(State1);

            if (myCounter.Count == 0)
            {
                timer.Stop();
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }   
}

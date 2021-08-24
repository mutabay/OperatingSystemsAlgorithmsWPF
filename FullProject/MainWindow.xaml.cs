using System;
using System.Collections.Generic;
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
using FullProject.FrameAllocation_4;
using FullProject.ProcessorSchedulingInDistributedSystems_5;
using Frame = FullProject.FrameAllocation_4.Frame;

namespace FullProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void bankers_button_Click(object sender, RoutedEventArgs e)
        {
            BankersAlgorithm bankersWindow = new BankersAlgorithm();
            Application.Current.MainWindow = bankersWindow;
            bankersWindow.Show();
        }

        private void dekkers_button_Click(object sender, RoutedEventArgs e)
        {
            DekkersAlgorithm dekkersWindow = new DekkersAlgorithm();
            Application.Current.MainWindow = dekkersWindow;
            dekkersWindow.Show();
        }

        private void processScheduling_button_Click(object sender, RoutedEventArgs e)
        {
            ProcessSchedulingAlgorithms processSchedulingWindow = new ProcessSchedulingAlgorithms();
            Application.Current.MainWindow = processSchedulingWindow;
            processSchedulingWindow.Show();
            
        }

        private void diskScheduling_button_Click(object sender, RoutedEventArgs e)
        {
            DiskSchedulingAlgorithms diskSchedulingWindow = new DiskSchedulingAlgorithms();
            Application.Current.MainWindow = diskSchedulingWindow;
            diskSchedulingWindow.Show();
        }

        private void pageReplacement_button_Click(object sender, RoutedEventArgs e)
        {
            PageReplacementAlgorithms pageReplacementWindow = new PageReplacementAlgorithms();
            Application.Current.MainWindow = pageReplacementWindow;
            pageReplacementWindow.Show();
        }

        private void processSchedulingInDistributedSystems_button_Click(object sender, RoutedEventArgs e)
        {
            ProcessSchedulingAlgorithmsInDistributedSystems processorSchedulingInDistributedWindow = new ProcessSchedulingAlgorithmsInDistributedSystems();
            Application.Current.MainWindow = processorSchedulingInDistributedWindow;
            processorSchedulingInDistributedWindow.Show();
        }

        private void frameAllocation_button_Click(object sender, RoutedEventArgs e)
        {
            FrameAllocation frameAllocationWindow = new FrameAllocation();
            Application.Current.MainWindow = frameAllocationWindow;
            frameAllocationWindow.Show();
        }
    }
}

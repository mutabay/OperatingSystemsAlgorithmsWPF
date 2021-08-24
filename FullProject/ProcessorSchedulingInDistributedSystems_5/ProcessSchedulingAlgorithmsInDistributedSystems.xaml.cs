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
using System.Windows.Shapes;
using FullProject.ProcessorSchedulingInDistributedSystems_5.Algorithms;

namespace FullProject.ProcessorSchedulingInDistributedSystems_5
{
    /// <summary>
    /// Interaction logic for ProcessSchedulingAlgorithmsInDistributedSystems.xaml
    /// </summary>

    public partial class ProcessSchedulingAlgorithmsInDistributedSystems : Window
    {
        public enum Algorithm
        {
            MaxThreshold,
            MinThreshold,
            Random
        }

        public TextBlock OutputTextBlock;
        int numberOfProcessor, maxThreshold, minThreshold, numberOfTimes;
        private Algorithm algorithm;
        public List<double> process = new List<double>();
        public int TIME_MAX = 1500;


        public ProcessSchedulingAlgorithmsInDistributedSystems()
        {
            InitializeComponent();
            selectAlgorithm_ComboBox.ItemsSource = Enum.GetValues(typeof(Algorithm)).Cast<Algorithm>();
            selectAlgorithm_ComboBox.Text = selectAlgorithm_ComboBox.Items[0].ToString();
            algorithm = Algorithm.MaxThreshold;
            minThreshold_TextBox.IsEnabled = false;
            numberOfTimes_TextBox.IsEnabled = false;

        }


        private void selectAlgorithm_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            algorithm = (Algorithm)selectAlgorithm_ComboBox.SelectedIndex;
            if (algorithm == Algorithm.MaxThreshold)
            {
                minThreshold_TextBox.IsEnabled = false;
                numberOfTimes_TextBox.IsEnabled = false;
            }
            else if (algorithm == Algorithm.MinThreshold)
            {
                minThreshold_TextBox.IsEnabled = true;
                numberOfTimes_TextBox.IsEnabled = false;
            }
            else if (algorithm == Algorithm.Random)
            {
                minThreshold_TextBox.IsEnabled = true;
                numberOfTimes_TextBox.IsEnabled = true;
            }
        }

        private void start_Button_Click(object sender, RoutedEventArgs e)
        {
            numberOfProcessor = Convert.ToInt32(this.processNumber_TextBox.Text);
            maxThreshold = Convert.ToInt32(this.maxThreshold_TextBox.Text);

            switch (algorithm)
            {
                case Algorithm.MaxThreshold:
                    OutputTextBlock = MaxThreshold.maxThreshold(process, numberOfProcessor, maxThreshold, TIME_MAX);
                    break;
                case Algorithm.MinThreshold:
                    minThreshold = Convert.ToInt32(this.minThreshold_TextBox.Text);
                    OutputTextBlock = MinThreshold.minThreshold(process, numberOfProcessor, maxThreshold, minThreshold, TIME_MAX);
                    break;
                case Algorithm.Random:
                    minThreshold = Convert.ToInt32(this.minThreshold_TextBox.Text);
                    numberOfTimes = Convert.ToInt32(this.numberOfTimes_TextBox.Text);
                    OutputTextBlock = Randoms.random(process, numberOfProcessor, maxThreshold, minThreshold, numberOfTimes, TIME_MAX);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null);
            }
            if (0 != output_StackPannel.Children.Count)
                output_StackPannel.Children.Clear();
            OutputTextBlock = EditSeeksBlock(OutputTextBlock);
            output_StackPannel.Children.Add(OutputTextBlock);

        }


        private TextBlock EditSeeksBlock(TextBlock textBlock)
        {
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Foreground = Brushes.DarkGreen;
            textBlock.Background = Brushes.LightGreen;
            textBlock.FontSize = 12;
            textBlock.FontWeight = FontWeight.FromOpenTypeWeight(500);
            textBlock.Padding = new Thickness(10);
            textBlock.TextAlignment = TextAlignment.Center;

            return textBlock;
        }



        private void reset_Button_Click(object sender, RoutedEventArgs e)
        {
            this.output_StackPannel.Children.Clear();
            ProcessSchedulingAlgorithmsInDistributedSystems newWindow = new ProcessSchedulingAlgorithmsInDistributedSystems();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            this.Close();
        }
    }

}


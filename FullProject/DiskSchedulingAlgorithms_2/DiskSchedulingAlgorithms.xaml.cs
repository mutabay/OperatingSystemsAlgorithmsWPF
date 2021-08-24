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
using FullProject.DiskSchedulingAlgorithms_2;
using FullProject.DiskSchedulingAlgorithms_2.Algorithms;

namespace FullProject
{
    /// <summary>
    /// Interaction logic for DiskSchedulingAlgorithms.xaml
    /// </summary>
   
    public partial class DiskSchedulingAlgorithms : Window
    {

        enum Algorithm
        {
            FCFS,
            SSTF,
            SCAN,
            CSCAN
        }

        private readonly DiskScheduling diskScheduling;
        private readonly List<ScheduleAlgorithm> scheduleAlgorithms;

        private Label showRequests;

        public DiskSchedulingAlgorithms()
        {
            InitializeComponent();
            this.diskScheduling = new DiskScheduling();
            this.scheduleAlgorithms = new List<ScheduleAlgorithm>()
            {
                new FCFS_Algorithm(),
                new SSTF_Algorithm(),
                new SCAN_Algorithm(),
                new CSCAN_Algorithm()
            };

            foreach (var scheduleAlgorithm in this.scheduleAlgorithms)
                this.select_ComboBox.Items.Add(scheduleAlgorithm.GetName());
            select_ComboBox.Text = this.select_ComboBox.Items[0].ToString();
            showRequests = EditSeeksLabel(showRequests, " ");

        }

        private void PrintSeekList_Label()
        {
            var seeks = "";
            seekList_StackPanel.Children.Clear();
            foreach (var seek in diskScheduling.AlreadyRead)
                seeks += seek.ToString() + " ";
            Label label = null;
            label = EditSeeksLabel(label, seeks);
            seekList_StackPanel.Children.Add(label);
        }

        private void PrintSeekOperations_Label()
        {
            var operations = "";
            totalSeekOperations_StackPanel.Children.Clear();
            totalSeekResult_StackPanel.Children.Clear();

            var seekList = diskScheduling.AlreadyRead;
            int totalSeek = 0, seekDifference = 0;
            if (seekList.Count >= 2)
            {
                for (int index = 0; index < seekList.Count - 1; index++)
                {
                    if (index == 0)
                        operations = "= ";
                    else
                        operations += "+ ";

                    operations += "| " + seekList[index] + "- " + seekList[index + 1] + " |";
                    seekDifference = Math.Abs(seekList[index] - seekList[index + 1]);
                    totalSeek += seekDifference;
                }

                Label operationsLabel = null;
                operationsLabel = EditSeeksLabel(operationsLabel, operations);
                totalSeekOperations_StackPanel.Children.Add(operationsLabel);

                Label resultLabel = null;
                resultLabel = EditSeeksLabel(resultLabel, totalSeek.ToString());
                totalSeekResult_StackPanel.Children.Add(resultLabel);
            }
        }

        private Label EditSeeksLabel(Label label, String text)
        {
            label = new Label()
            {
                Content = text.ToString(),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Brushes.DarkBlue,
                Background = Brushes.LightBlue,
                FontSize = 12,
                BorderThickness = new Thickness(2, 0, 2, 0),
                BorderBrush = Brushes.DarkGreen,
                FontWeight = FontWeight.FromOpenTypeWeight(500)
            };
            return label;
        }

        private ScheduleAlgorithm GetAlgorithm(Algorithm algorithm)
        {
            ScheduleAlgorithm schedule;
            switch (algorithm)
            {
                case Algorithm.FCFS:
                    schedule = this.scheduleAlgorithms[0];
                    break;
                case Algorithm.SSTF:
                    schedule = this.scheduleAlgorithms[1];
                    break;
                case Algorithm.SCAN:
                    schedule = this.scheduleAlgorithms[2];
                    break;
                case Algorithm.CSCAN:
                    schedule = this.scheduleAlgorithms[3];
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null);
            }
            return schedule;
        }

        private void Refresh()
        {
            DiskSchedulingAlgorithms newWindow = new DiskSchedulingAlgorithms();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            this.Close();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //this.Refresh();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (0 == this.diskScheduling.AlreadyRead.Count)
                this.start_Button.Content = @"Next";

            this.diskScheduling.AddScheduleStrategy(this.GetAlgorithm((Algorithm)this.select_ComboBox.SelectedIndex));
            this.diskScheduling.ReadNext();
            if (0 == this.diskScheduling.ReadRequests.Count)
                this.start_Button.IsEnabled = false;
            PrintSeekList_Label();
            PrintSeekOperations_Label();
            //Refresh();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int value = Convert.ToInt32(this.add_Textbox.Text);
            this.Add(value);
        }

        private void Add(int value)
        {
            this.diskScheduling.AddReadRequest(value);
            var text = (value + " | ");
            showRequests.Content += text;
            if (0 != showRequests_StackPannel.Children.Count)
                showRequests_StackPannel.Children.Clear();
            showRequests.HorizontalAlignment = HorizontalAlignment.Left;
            showRequests_StackPannel.Children.Add(showRequests);
            this.start_Button.IsEnabled = true;
        }

        private static bool IsIntegerValue(string text)
        {
            int parsedValues;
            return Int32.TryParse(text, out parsedValues);
        }

        private static void OnlyAcceptNumbers(TextBox addTextBox)
        {
            if (!IsIntegerValue(addTextBox.Text.ToString()))
                addTextBox.Text = @"0";
        }

        private void add_Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnlyAcceptNumbers(this.add_Textbox);
        }

        private void randomRequest_Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnlyAcceptNumbers(this.randomRequest_Textbox);
        }

        private void CreateRandomRequestButton_Click(object sender, RoutedEventArgs e)
        {
            int count = Convert.ToInt32(this.randomRequest_Textbox.Text);
            //this.Reset();
            List<int> randomList = new List<int>();
            Random rand = new Random();
            for (int i = 0; i < count; i++)
            {
                int element = rand.Next(DiskScheduling.RIGHT_BORDER);
                randomList.Add(element);
            }

            foreach (var value in randomList)
                this.Add(value);

        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            this.Reset();
        }

        private void Reset()
        {
            this.diskScheduling.Reset();
            this.showRequests_StackPannel.Children.Clear();
            this.start_Button.Content = @"Start";
            this.start_Button.IsEnabled = true;
            this.Refresh();
        }


    }

}

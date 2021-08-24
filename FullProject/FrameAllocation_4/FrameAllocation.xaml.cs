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
using FullProject.FrameAllocation_4.Algorithms;

namespace FullProject.FrameAllocation_4
{
    /// <summary>
    /// Interaction logic for FrameAllocation.xaml
    /// </summary>
    public partial class FrameAllocation : Window
    {
        public enum Algorithm
        {
            Proportional,
            Random,
            PageFaultFrequency,
            WorkingSet,
        }

        private Label showPageReferences_Label;
        public LinkedList<Page> pageReferenceString;
        public LinkedList<Process> processList;

        public BasicAlgorithm FrameAllocationAlgorithm = null;

        public FrameAllocation()
        {
            InitializeComponent();
            pageReferenceString = new LinkedList<Page>();
            processList = new LinkedList<Process>();
            selectAlgorithm_ComboBox.ItemsSource = Enum.GetValues(typeof(Algorithm)).Cast<Algorithm>();
            selectAlgorithm_ComboBox.Text = selectAlgorithm_ComboBox.Items[0].ToString();
            FrameAllocationAlgorithm = new Proportional("Proportional", pageReferenceString.Count);
            showPageReferences_Label = EditSeeksLabel(showPageReferences_Label, "");
        }
        private void PrintPageReferences_Label()
        {
            var text = "";

            foreach (var s in pageReferenceString)
            {
                text += s.numberOfPage + "  |  ";
            }
            showPageRef_StackPannel.Children.Clear();

            Label label = null;
            label = EditSeeksLabel(label, text);
            showPageRef_StackPannel.Children.Add(label);
        }

        private void PrintProcessCount()
        {
            string text = processList.Count.ToString();
            numberOfProcesses_StackPanel.Children.Clear();
            //foreach (var process in processList)
            //    text += processList.ToString() + " ";
            Label label = null;
            label = EditSeeksLabel(label, text);
            numberOfProcesses_StackPanel.Children.Add(label);

        }

        private void PrintFrameHeader()
        {
            frameHeader_StackPannel.Children.Clear();

            string title = FrameAllocationAlgorithm.getTitle() + Environment.NewLine;
            Label title_label = null;
            title_label = EditSeeksLabel(title_label, title);
            frameHeader_StackPannel.Children.Add(title_label);

            string frameHeader = "|     |";
            for (int i = 0; i < pageReferenceString.Count; i++)
                frameHeader += " f" + i + " |";
            Label frameHeader_label = null;
            frameHeader_label = EditSeeksLabel(frameHeader_label, frameHeader + Environment.NewLine);
            frameHeader_StackPannel.Children.Add(frameHeader_label);
        }

        private void PrintOutput()
        {
            var log = FrameAllocationAlgorithm.GetLog(0);
            string output = "";
            string[] dividedByTime = log.Split('\n');
            for (int i = 0; i < dividedByTime.Length; i++)
                output += "Time: " + i + "|" + dividedByTime[i] + Environment.NewLine;

            Label output_label = null;
            output_label = EditSeeksLabel(output_label, output);
            output_StackPannel.Children.Add(output_label);
        }

        private Label EditSeeksLabel(Label label, String text)
        {
            label = new Label()
            {
                Content = text.ToString(),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Brushes.DarkGreen,
                Background = Brushes.LightGreen,
                FontSize = 12,
                BorderThickness = new Thickness(2, 0, 2, 0),
                BorderBrush = Brushes.DarkGreen,
                FontWeight = FontWeight.FromOpenTypeWeight(500)
            };
            return label;
        }

        private void Refresh()
        {
            FrameAllocation newWindow = new FrameAllocation();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            this.Close();
        }

        private void start_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!pageReferenceString.Any() || processList.Any())
                start_Button.IsEnabled = false;

            Algorithm algorithm = (Algorithm)selectAlgorithm_ComboBox.SelectedIndex;

            switch (algorithm)
            {
                case Algorithm.Proportional:
                    FrameAllocationAlgorithm = new Proportional("Proportional", pageReferenceString.Count);
                    break;
                case Algorithm.Random:
                    FrameAllocationAlgorithm = new Rand("Rand", pageReferenceString.Count);
                    break;
                case Algorithm.PageFaultFrequency:
                    FrameAllocationAlgorithm = new PageFaultFrequency("PageFaultFrequency", pageReferenceString.Count);
                    break;
                case Algorithm.WorkingSet:
                    FrameAllocationAlgorithm = new WorkingSet("Working Set", pageReferenceString.Count);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null);
            }
            FrameAllocationAlgorithm.simulate(processList);
            PrintPageReferences_Label();
            PrintFrameHeader();
            PrintOutput();
        }

        private void addPageReference_Button_Click(object sender, RoutedEventArgs e)
        {
            int pageReference = Convert.ToInt32(this.pageReference_TextBox.Text);
            Add(pageReference);
            PrintPageReferences_Label();
        }

        private void Add(int value)
        {
            pageReferenceString.AddLast(new Page(value));
            if (0 != showPageRef_StackPannel.Children.Count)
                showPageRef_StackPannel.Children.Clear();
            if (processList.Count == 0)
                throw new Exception("Please enter process number.");
            processList.ElementAt(0).PutList(pageReferenceString);
            start_Button.IsEnabled = true;

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

        private void processNumber_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnlyAcceptNumbers(processNumber_TextBox);
            int processCount = Convert.ToInt32(processNumber_TextBox.Text);
            processList.Clear();
            for (int i = 0; i < processCount; i++)
            {
                processList.AddLast(new Process(i));
            }
            PrintProcessCount();
        }

        private void pageReference_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnlyAcceptNumbers(pageReference_TextBox);
        }

        private void randomPageRef_Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnlyAcceptNumbers(randomPageRef_Textbox);
        }

        private void createRandom_Button_Click(object sender, RoutedEventArgs e)
        {
            int pageCount = Convert.ToInt32(randomPageRef_Textbox.Text);
            List<int> randomList = new List<int>();
            Random rand = new Random();

            for (int i = 0; i < pageCount; i++)
            {
                int element = rand.Next(0, 10);
                randomList.Add(element);
            }

            foreach (var value in randomList)
                Add(value);
            PrintPageReferences_Label();

        }

        private void reset_Button_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            FrameAllocationAlgorithm.Reset();
            showPageRef_StackPannel.Children.Clear();
            this.Refresh();
        }

        private void selectAlgorithm_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FrameAllocationAlgorithm != null) FrameAllocationAlgorithm.Reset();
        }



    }

}

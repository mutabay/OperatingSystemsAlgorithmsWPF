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
using FullProject.PageReplacementAlgorithms_3;

namespace FullProject
{
    /// <summary>
    /// Interaction logic for PageReplacementAlgorithms.xaml
    /// </summary>
    public partial class PageReplacementAlgorithms : Window
    {
        public enum Algorithm
        {
            FIFO,
            LRU,
            OPT,
            ALRU,
            RAND
        }

        private Label showPageReferences_Label;

        public List<int> pageReferenceString;
        public int pageFrameCount;
        public Algorithm SelectedAlgorithm;

        public PageReplacement pageReplacementAlgorithm;

        public PageReplacementAlgorithms()
        {
            InitializeComponent();
            pageReferenceString = new List<int>();
            selectAlgorithm_ComboBox.ItemsSource = Enum.GetValues(typeof(Algorithm)).Cast<Algorithm>();
            pageReplacementAlgorithm = new PageReplacement(pageReferenceString, pageFrameCount);
            selectAlgorithm_ComboBox.Text = selectAlgorithm_ComboBox.Items[0].ToString();
            showPageReferences_Label = EditSeeksLabel(showPageReferences_Label, " ");
        }



        private void PrintPageReferences_Label()
        {
            var text = "";
            showPageRef_StackPannel.Children.Clear();
            foreach (var pageReference in pageReferenceString)
                text += pageReference.ToString() + " ";
            Label label = null;
            label = EditSeeksLabel(label, text);
            showPageRef_StackPannel.Children.Add(label);
        }

        private void PrintPageFrameCount()
        {
            numberOfFrames_StackPanel.Children.Clear();
            Label label = new Label();
            label = EditSeeksLabel(label, pageFrameCount.ToString());
            numberOfFrames_StackPanel.Children.Add(label);
        }
        private void PrintResults_Label(string pageFault, string hit)
        {
            pageFaults_StackPanel.Children.Clear();
            hit_StackPanel.Children.Clear();

            var pageFaultText = pageFault;
            Label pageFaultLabel = null;
            pageFaultLabel = EditSeeksLabel(pageFaultLabel, pageFaultText);
            pageFaults_StackPanel.Children.Add(pageFaultLabel);

            var hitText = hit;
            Label hitLabel = null;
            hitLabel = EditSeeksLabel(hitLabel, hitText);
            hit_StackPanel.Children.Add(hitLabel);
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
            PageReplacementAlgorithms newWindow = new PageReplacementAlgorithms();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            this.Close();
        }

        private void start_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!pageReferenceString.Any() || pageFrameCount == 0)
                start_Button.IsEnabled = false;
            pageReplacementAlgorithm.ResetProperties(pageReferenceString, pageFrameCount);
            Algorithm algorithm = (Algorithm)selectAlgorithm_ComboBox.SelectedIndex;

            switch (algorithm)
            {
                case Algorithm.FIFO:
                    pageReplacementAlgorithm.FIFO();
                    break;
                case Algorithm.LRU:
                    pageReplacementAlgorithm.LRU();
                    break;
                case Algorithm.OPT:
                    pageReplacementAlgorithm.OPT();
                    break;
                case Algorithm.ALRU:
                    pageReplacementAlgorithm.ALRU();
                    break;
                case Algorithm.RAND:
                    pageReplacementAlgorithm.RAND();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null);
            }
            PrintPageReferences_Label();
            PrintResults_Label(pageReplacementAlgorithm.pageFaults.ToString(), hit: pageReplacementAlgorithm.hit.ToString());
        }

        private void addPageReference_Button_Click(object sender, RoutedEventArgs e)
        {
            int pageReference = Convert.ToInt32(this.pageReference_TextBox.Text);
            Add(pageReference);

        }

        private void Add(int value)
        {
            pageReferenceString.Add(value);
            var text = value + " , ";
            showPageReferences_Label.Content += text;
            if (0 != showPageRef_StackPannel.Children.Count)
                showPageRef_StackPannel.Children.Clear();
            showPageReferences_Label.HorizontalAlignment = HorizontalAlignment.Center;
            showPageRef_StackPannel.Children.Add(showPageReferences_Label);
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

        private void frameNumber_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnlyAcceptNumbers(frameNumber_TextBox);
            pageFrameCount = Convert.ToInt32(frameNumber_TextBox.Text);
            PrintPageFrameCount();
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
            int count = Convert.ToInt32(randomPageRef_Textbox.Text);
            List<int> randomList = new List<int>();
            Random rand = new Random();
            for (int i = 0; i < count; i++)
            {
                int element = rand.Next(6);
                randomList.Add(element);
            }

            foreach (var value in randomList)
                Add(value);
        }

        private void reset_Button_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            pageReplacementAlgorithm.ResetProperties(pageReferenceString, pageFrameCount);
            showPageRef_StackPannel.Children.Clear();
            hit_StackPanel.Children.Clear();
            pageFaults_StackPanel.Children.Clear();
            this.Refresh();

        }

        private void selectAlgorithm_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pageReplacementAlgorithm.ResetProperties(pageReferenceString, pageFrameCount);
            hit_StackPanel.Children.Clear();
            pageFaults_StackPanel.Children.Clear();
        }
    }
}

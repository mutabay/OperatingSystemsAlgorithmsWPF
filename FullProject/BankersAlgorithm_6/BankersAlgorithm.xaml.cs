using System;
using System.Collections.Generic;
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
using FullProject.BankersAlgorithm_6;


namespace FullProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BankersAlgorithm : Window
    {
        private int numberOfProcesses = 0;
        private int numberOfResources = 0;
        private int[,] maxDemandMatrix;
        public int[,] allocationMatrix;
        public int[] availableResources;


        public BankersAlgorithm()
        {
            InitializeComponent();
        }

        private void PrintSafeSequence_Label(string showSafeSequence)
        {
            safeSequence_StackPanel.Children.Clear();
            Label safeSequence_label = null;
            safeSequence_label = EditSeeksLabel(safeSequence_label, showSafeSequence);
            safeSequence_StackPanel.Children.Add(safeSequence_label);
        }

        private void PrintState_Label(string state)
        {
            state_StackPanel.Children.Clear();
            Label state_label = null;
            state_label = EditSeeksLabel(state_label, state);
            state_StackPanel.Children.Add(state_label);
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

        private static bool IsIntegerValue(string text)
        {
            int parsedValues;
            return Int32.TryParse(text, result: out parsedValues);
        }

        private static void OnlyAcceptNumbers(TextBox addTextBox)
        {
            if (!IsIntegerValue(addTextBox.Text.ToString()))
                addTextBox.Text = @"0";
        }

        private void numberOfProcesses_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnlyAcceptNumbers(numberOfProcesses_TextBox);
            numberOfProcesses = Convert.ToInt32(numberOfProcesses_TextBox.Text);
        }

        private void numberOfResources_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnlyAcceptNumbers(numberOfResources_TextBox);
            numberOfResources = Convert.ToInt32(numberOfResources_TextBox.Text);
        }

        private void applyValues_Button_Click(object sender, RoutedEventArgs e)
        {
            allocationMatrix = new int[numberOfProcesses, numberOfResources];
            maxDemandMatrix = new int[numberOfProcesses, numberOfResources];
            availableResources = new int[numberOfResources];

            List<int> allocationList = allocationMatrix_TextBox.Text.Split(',').Select(int.Parse).ToList();
            allocationMatrix = FillMatrixWithList(allocationMatrix, allocationList);
            PrintListToTextBox(allocationList, allocationMatrix_TextBox);

            List<int> maxDemandList = maxDemandMatrix_TextBox.Text.Split(',').Select(int.Parse).ToList();

            maxDemandMatrix = FillMatrixWithList(maxDemandMatrix, maxDemandList);
            PrintListToTextBox(maxDemandList, maxDemandMatrix_TextBox);

            availableResources = availableResourceList_TextBox.Text.Split(',').Select(int.Parse).ToArray();
            Debug.Assert(allocationList.Count == numberOfProcesses * numberOfResources &&
                          maxDemandList.Count == numberOfProcesses * numberOfResources &&
                          availableResources.Length == numberOfResources);

        }

        private int[,] FillMatrixWithList(int[,] matrix, List<int> list)
        {
            int k = 0;
            for (int i = 0; i < numberOfProcesses; i++)
            {
                for (int j = 0; j < numberOfResources; j++)
                {
                    matrix[i, j] = list[k];
                    k++;
                }
            }

            return matrix;
        }

        private void createRandom_Button_Click(object sender, RoutedEventArgs e)
        {
            allocationMatrix = new int[numberOfProcesses, numberOfResources];
            maxDemandMatrix = new int[numberOfProcesses, numberOfResources];
            availableResources = new int[numberOfResources];
            List<int> randomMaxDemandList = new List<int>();
            List<int> randomAllocationList = new List<int>();

            Random rand = new Random();
            for (int i = 0; i < numberOfProcesses * numberOfResources; i++)
                randomMaxDemandList.Add(rand.Next(10));
            for (int i = 0; i < numberOfProcesses * numberOfResources; i++)
                randomAllocationList.Add(rand.Next(10));
            for (int i = 0; i < numberOfResources; i++)
                availableResources[i] = rand.Next(10);

            allocationMatrix = FillMatrixWithList(allocationMatrix, randomAllocationList);
            allocationMatrix = FillMatrixWithList(maxDemandMatrix, randomMaxDemandList);


            PrintListToTextBox(randomMaxDemandList, maxDemandMatrix_TextBox);
            PrintListToTextBox(randomAllocationList, allocationMatrix_TextBox);
            PrintArrayToTextBox(availableResources, availableResourceList_TextBox);

        }

        private void PrintListToTextBox(List<int> list, TextBox textBox)
        {
            string str = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (0 == i % numberOfResources && 0 != i)
                {
                    str += " | ";
                }
                str += list[i] + ",";

            }

            textBox.Text = str;
        }

        private void PrintArrayToTextBox(int[] array, TextBox textBox)
        {
            string str = "";
            for (int i = 0; i < array.Length; i++)
                str += array[i] + ",";

            textBox.Text = str;
        }

        private void PrintMatrices()
        {
            allocationMatrix_StackPannel.Children.Clear();
            maxDemand_StackPannel.Children.Clear();
            Label allocationMatrixLabel = null;
            Label maxDemandMatrixLabel = null;
            string all_str = "";
            string max_str = "";

            for (int i = 0; i < numberOfProcesses; i++)
            {
                all_str += Environment.NewLine;
                max_str += Environment.NewLine;
                for (int j = 0; j < numberOfResources; j++)
                {
                    all_str += "  " + allocationMatrix[i, j] + "  ";
                    max_str += "  " + maxDemandMatrix[i, j] + "  ";
                }


            }
            allocationMatrixLabel = EditSeeksLabel(allocationMatrixLabel, all_str);
            maxDemandMatrixLabel = EditSeeksLabel(maxDemandMatrixLabel, max_str);

            allocationMatrix_StackPannel.Children.Add(allocationMatrixLabel);
            maxDemand_StackPannel.Children.Add(maxDemandMatrixLabel);
        }

        private void start_Button_Click(object sender, RoutedEventArgs e)
        {

            string state;
            Bankers algorithm = new Bankers(numberOfProcesses
                , numberOfResources, maxDemandMatrix, allocationMatrix, availableResources);
            PrintMatrices();
            if (algorithm.IsSafeState())
            {
                state = "The processes are in safe state";
                PrintSafeSequence_Label(algorithm.showSafeSequence);
            }
            else
            {
                state = "The processes are in unsafe state";
            }

            PrintState_Label(state);

        }

        private void reset_Button_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            numberOfProcesses = 0;
            numberOfResources = 0;
            maxDemandMatrix = null;
            allocationMatrix = null;
            availableResources = null;
            this.Refresh();
        }

        private void Refresh()
        {
            BankersAlgorithm newWindow = new BankersAlgorithm();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            this.Close();
        }

    }
}

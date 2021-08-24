using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FullProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DekkersAlgorithm : Window
    {
        private static string log = "";
        private int favouredThread = 1;
        public Thread Process(int i)
        {
            int turn = 0;       // Which process turn.
            int loopCount = 4;      // Number of loops.
            bool[] flag = { false, false };     // ith process want to work.
            Random rand = new Random();
            return new Thread((() =>
            {
                int j = 1 - i;
                for (int n = 0; n < loopCount; n++)
                {
                    log += i + "th process wants to work.";      // Lock
                    flag[i] = true;     // Mark ith process.
                    while (flag[j])     // If jth process want to work too. // 2 
                    {
                        // If its ith process' turn, retry 
                        if (i == turn)
                        {
                            Thread.Yield();
                            continue;
                        }

                        flag[i] = false;    // If its jth process turn, ith process don't want to work.

                        while (j == turn)   // ith process waits for jth process turn to complete.
                            Thread.Yield();

                        flag[i] = true;     // ith now want to enter, retry 2.
                    }

                    log += Environment.NewLine + i + "th process is working at." + n;
                    Sleep(50);

                    log += Environment.NewLine + i + "th process work is done.";      // Unlock
                    turn = j;       // Its jth's turn now.
                    flag[i] = false;        // ith process don't want to work.

                }
            }));
        }

        public void Sleep(int t)
        {
            try
            {
                Thread.Sleep(t);
            }
            catch (ThreadInterruptedException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public DekkersAlgorithm()
        {
            InitializeComponent();
        }


        private void start_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Thread process_0 = null;
                Thread process_1 = null;
                log += "Starting 2 processes (threads)...";
                if (2 == favouredThread)
                {
                    process_0 = Process(1);
                    process_1 = Process(0);
                }
                else
                {
                    process_0 = Process(0);
                    process_1 = Process(1);
                }

                process_0.Start();
                process_1.Start();
                process_0.Join();
                process_1.Join();

                PrintLogLabel();
            }
            catch (ThreadInterruptedException threadInterruptedException)
            {
                Console.WriteLine(threadInterruptedException);
                throw;
            }

        }

        private void PrintLogLabel()
        {
            log_StackPannel.Children.Clear();
            Label log_label = null;
            log_label = EditSeeksLabel(log_label, log);
            if (favouredThread == 2)
                log_label.FontSize = 6;
            log_StackPannel.Children.Add(log_label);
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

        private void reset_Button_Click(object sender, RoutedEventArgs e)
        {
            DekkersAlgorithm newWindow = new DekkersAlgorithm();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            this.Close();
        }

        private void processCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnlyAcceptNumbers(processCount_textBox);
            favouredThread = Convert.ToInt32(processCount_textBox.Text);
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
    }

}

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
using FullProject.ProcessSchedulingAlgorithms_1;

namespace FullProject
{
    /// <summary>
    /// Interaction logic for ProcessScheduling.xaml
    /// </summary>
    public partial class ProcessSchedulingAlgorithms : Window
    {
        public ProcessSchedulingAlgorithms()
        {
            InitializeComponent();
            RR_radioButton.IsChecked = true;

            SRTF_radioButton.Checked += new RoutedEventHandler(RadioButtons_Checked);
            SJF_radioButton.Checked += new RoutedEventHandler(RadioButtons_Checked);
            FCFS_radioButton.Checked += new RoutedEventHandler(RadioButtons_Checked);
            RR_radioButton.Checked += new RoutedEventHandler(RadioButtons_Checked);
        }
        List<Process> processList = new List<Process>(), ganttChart = new List<Process>();
        List<int> arrivalTimeList = new List<int>();
        List<int> burstTimeList = new List<int>();
        List<int> flag = new List<int>();
        List<int> serviceTime = new List<int>();
        int processID, arrivalTime, processQuantum, pervAlgo;
        int burstTime, X = 16, t = 0, Y = 16, temp = 0, sum = 0;
        int rowIndex = 1, complete = 0, shortest = 0, completionTime, minimum;

        private float totalWaitingTime = 0f, totalTurnAroundTime = 0f;
        bool checkProcessID = false; bool insert = false; // Check for same process ID's 

        private void RadioButtons_Checked(object sender, EventArgs e)
        {
            RadioButton radioButton = stackPanel_algorithms.Children.OfType<RadioButton>().FirstOrDefault(n => (bool)n.IsChecked);
            RadioButton senderButton = sender as RadioButton;
            //quantum should be readonly when button is not RR

            if (RR_radioButton == radioButton)
            {
                quantum_textBox.Visibility = Visibility.Visible;
                quantum_label.Foreground = Brushes.Black;
            }
            else
            {
                quantum_textBox.Visibility = Visibility.Collapsed;
                quantum_textBox.IsReadOnly = true;
                quantum_label.Foreground = Brushes.DarkRed;
            }
        }

        private void process_id_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            processID = Convert.ToInt32(process_id_textBox.Text);
        }
        private void arrival_time_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            arrivalTime = Convert.ToInt32(arrival_time_textBox.Text);
        }

        private void bust_time_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            burstTime = Convert.ToInt32(bust_time_textBox.Text);
        }

        private void quantum_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            processQuantum = Convert.ToInt32(quantum_textBox.Text);
        }

        private void InitializeValues()
        {
            processList.Clear();
            ganttChart.Clear();
            arrivalTimeList.Clear();
            burstTimeList.Clear();
            flag.Clear();
            serviceTime.Clear();


            totalTurnAroundTime = totalWaitingTime = 0; X = Y = 16; temp = sum = t = burstTime = 0;
            complete = 0; shortest = 0; completionTime = 0; minimum = 0; insert = checkProcessID = false;
            ganttChart.Clear(); flag.Clear();
            burstTimeList.Clear(); arrivalTimeList.Clear(); serviceTime.Clear();
            //Panel.RowDefinitions.Clear();
            Panel.Children.Clear();
            turnAroundTime_TextBox.Text = waitingTime_TextBox.Text = "";
            //gantChart_grid.Children.Clear();

            InitializeComponent();
        }

        private void insert_button1_Click(object sender, RoutedEventArgs e)
        {

            processID = Convert.ToInt32(process_id_textBox.Text);
            arrivalTime = Convert.ToInt32(arrival_time_textBox.Text);
            burstTime = Convert.ToInt32(bust_time_textBox.Text);

            for (int i = 0; i < processList.Count; i++)
            {
                if (processList[i].processID == processID)
                    checkProcessID = true;
            }

            if (checkProcessID)
            {
                MessageBox.Show("Two process can't have same Process ID = "
                                + Convert.ToString(processID)
                                + ", at a same time \n Please Enter Process ID again", "Error");
                checkProcessID = false;
                return;
            }
            else
                processList.Add(new Process(processID, burstTime, arrivalTime));

            checkProcessID = false;
            TextBlock processId_TextBlock = new TextBlock() { Text = processID.ToString() };
            TextBlock arrivalTime_TextBlock = new TextBlock() { Text = arrivalTime.ToString() };
            TextBlock burstTime_TextBlock = new TextBlock() { Text = burstTime.ToString() };
            TextBlock turnAroundTime_TextBlock = new TextBlock() { Text = "-" };
            TextBlock waitingTimeText_TextBlock = new TextBlock() { Text = "-" };

            //add a new row to the grid
            RowDefinition newRow = new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) };
            Panel.RowDefinitions.Add(newRow);

            Grid.SetRow(processId_TextBlock, rowIndex);
            Grid.SetColumn(processId_TextBlock, 1);

            Grid.SetRow(arrivalTime_TextBlock, rowIndex);
            Grid.SetColumn(arrivalTime_TextBlock, 2);

            Grid.SetRow(burstTime_TextBlock, rowIndex);
            Grid.SetColumn(burstTime_TextBlock, 3);

            Grid.SetRow(turnAroundTime_TextBlock, rowIndex);
            Grid.SetColumn(turnAroundTime_TextBlock, 4);

            Grid.SetRow(waitingTimeText_TextBlock, rowIndex);
            Grid.SetColumn(waitingTimeText_TextBlock, 5);

            Panel.Children.Add(processId_TextBlock);
            Panel.Children.Add(arrivalTime_TextBlock);
            Panel.Children.Add(burstTime_TextBlock);
            Panel.Children.Add(turnAroundTime_TextBlock);
            Panel.Children.Add(waitingTimeText_TextBlock);

            rowIndex++;
        }

        private void run_button_Click(object sender, RoutedEventArgs e)
        {
            if (0 == processList.Count)
            {
                MessageBox.Show("Please! Insert process first", "Error");
                return;
            }


            if (0 == pervAlgo)
            {
                if ((bool)SRTF_radioButton.IsChecked)
                    SRTFFindAvgTime(processList, processList.Count);
                else if ((bool)RR_radioButton.IsChecked)
                {
                    processQuantum = Convert.ToInt32(quantum_textBox.Text);
                    if (0 == processQuantum)
                    {
                        MessageBox.Show(null, "Quantum = 0!\nEnter quantum and press RUN", "Invalid Input");
                        return;
                    }
                    else
                    {
                        RoundRobin(processList, processQuantum);
                    }
                }
                else if ((bool)FCFS_radioButton.IsChecked)
                    FCFS(processList, processList.Count);
                else if ((bool)SJF_radioButton.IsChecked)
                    SJF(processList, processList.Count);
            }
            else
            {
                if (pervAlgo != 3 && (bool)RR_radioButton.IsChecked)
                {
                    if (0 == processQuantum)
                    {
                        MessageBox.Show(null, "QUANTUM = 0 !\nEnter QUANTUM and press RUN.", "Invalid Input");
                        return;
                    }
                    else
                    {
                        InitializeValues();
                        RoundRobin(processList, processQuantum);
                    }
                }
                else
                {
                    InitializeValues();
                    if ((bool)SRTF_radioButton.IsChecked)
                        SRTFFindAvgTime(processList, processList.Count);
                    else if ((bool)FCFS_radioButton.IsChecked)
                        FCFS(processList, processList.Count);
                    else if ((bool)SJF_radioButton.IsChecked)
                        SJF(processList, processList.Count);
                }
            }

            processList = processList.OrderBy(i => i.processID).ToList();

            if ((totalTurnAroundTime / processList.Count) == 0)
                turnAroundTime_TextBox.Text = "0.00";
            else
                turnAroundTime_TextBox.Text = (totalTurnAroundTime / processList.Count).ToString("#.##");

            if ((totalWaitingTime / processList.Count) == 0)
                waitingTime_TextBox.Text = "0.00";
            else
                waitingTime_TextBox.Text = (totalWaitingTime / processList.Count).ToString("#.##");
        }

        private void remove_button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialog = MessageBox.Show("Do you really want to Remove all Process?", "Exit", MessageBoxButton.YesNo);
            if (dialog == MessageBoxResult.Yes)
            {
                InitializeValues();
            }
        }

        // Method to calculate time by SJF algorithm
        private void SJF(List<Process> processList, int n)
        {
            pervAlgo = 1;
            for (int i = 0; i < n; i++)
            {
                serviceTime.Add(i);
                flag.Add(0);
            }

            while (true)
            {
                temp = n;
                //TODO MAX YAP
                minimum = 999;
                // If all process are completed then loop will be terminated.
                if (sum == n)
                    break;

                for (int i = 0; i < n; i++)
                {
                    if ((processList[i].arrivalTime <= t) && (flag[i] == 0) && (processList[i].burstTime < minimum))
                    {
                        minimum = processList[i].burstTime;
                        temp = i;
                    }
                }

                // If temp=n means no process can be so move forward in time
                if (temp == n)
                    t++;
                else
                {
                    serviceTime[temp] = t + processList[temp].burstTime;
                    t += processList[temp].burstTime;
                    processList[temp].turnAroundTime = serviceTime[temp] - processList[temp].arrivalTime;
                    processList[temp].waitingTime = processList[temp].turnAroundTime - processList[temp].burstTime;
                    flag[temp] = 1;
                    /*
                    for (int i = 0; i < processList[temp].burstTime; i++)
                    {
                        ganttChart.Add(processList[temp]);
                        TextBox gantt_TextBox = new TextBox();
                        gantChart_grid.Children.Add(gantt_TextBox);
                        gantt_TextBox.Width = 25; gantt_TextBox.Height = 25;
                        gantt_TextBox.Text = ("P" + Convert.ToString(processList[temp].processID));
                        gantt_TextBox.Background = processList[temp].processColor;
                        gantt_TextBox.Name = "textBox1";
                        gantt_TextBox.IsReadOnly = true;
                        gantt_TextBox.Padding = new Thickness(X, 34, 0, 0);

                        X += (int)gantt_TextBox.Width;
                    }
                    */
                    sum++;
                }
            }

            for (int i = 0; i < n; i++)
            {
                totalWaitingTime += processList[i].waitingTime;
                totalTurnAroundTime += processList[i].turnAroundTime;
            }
            //PrintGanttChart(ganttChart);
        }

        // Method to calculate time by FCFS algorithm
        private void FCFS(List<Process> processList, int n)
        {
            pervAlgo = 2;
            processList = processList.OrderBy(i => i.arrivalTime).ToList();
            for (int i = 0; i < n; i++)
            {
                serviceTime.Add(i);
            }
            processList[0].waitingTime = 0;
            processList[0].turnAroundTime = processList[0].burstTime;
            // Calculating waiting time

            for (int i = 1; i < n; i++)
            {
                // Add burst time of previous processes.
                serviceTime[i] = serviceTime[i - 1] + processList[i - 1].burstTime;

                // Find waiting time of previous processes.
                // Sum - at[i]
                processList[i].waitingTime = serviceTime[i] - processList[i].arrivalTime;
                processList[i].turnAroundTime = processList[i].burstTime + processList[i].waitingTime;
                if (processList[i].waitingTime < 0)
                    processList[i].waitingTime = 0;
                /*
                // Printing Gant Chart
                for (int j = 0; j < processList[i - 1].burstTime; j++)
                {
                    ganttChart.Add(processList[i - 1]);
                    TextBox gantt_TextBox = new TextBox();
                    gantChart_grid.Children.Add(gantt_TextBox);
                    gantt_TextBox.Width = 25; gantt_TextBox.Height = 25;
                    gantt_TextBox.Text = ("P" + Convert.ToString(processList[i - 1].processID));
                    gantt_TextBox.Background = processList[i - 1].processColor;
                    gantt_TextBox.Name = "textBox1";
                    gantt_TextBox.IsReadOnly = true;
                    gantt_TextBox.Padding = new Thickness(X, 34, 0, 0);

                    X += (int)gantt_TextBox.Width;
                }
                */

            }
            /*
            for (int i = 0; i < processList[n - 1].burstTime; i++)
            {
                ganttChart.Add(processList[n - 1]);
                TextBox gantt_TextBox = new TextBox();
                gantChart_grid.Children.Add(gantt_TextBox);
                gantt_TextBox.Width = 25; gantt_TextBox.Height = 25;
                gantt_TextBox.Text = ("P" + Convert.ToString(processList[n - 1].processID));
                gantt_TextBox.Background = processList[n - 1].processColor;
                gantt_TextBox.Name = "textBox1";
                gantt_TextBox.IsReadOnly = true;
                gantt_TextBox.Padding = new Thickness(X, 34, 0, 0);

                X += (int)gantt_TextBox.Width;
            }
            */
            for (int i = 0; i < n; i++)
            {
                totalWaitingTime += processList[i].waitingTime;
                totalTurnAroundTime += processList[i].turnAroundTime;
            }
            //PrintGanttChart((ganttChart));
        }

        private void RoundRobin(List<Process> processList, int quantum)
        {
            pervAlgo = 3;
            processList = processList.OrderBy(i => i.arrivalTime).ToList();
            for (int i = 0; i < processList.Count(); i++)
            {
                arrivalTimeList.Add(processList[i].arrivalTime);
                burstTimeList.Add(processList[i].burstTime); // making new burst time and arrival time list.
            }

            while (true)
            {
                checkProcessID = true;
                for (int i = 0; i < processList.Count; i++)
                {
                    // These condition for if arrival time != 0 && check that if there come before time.
                    if (arrivalTimeList[i] <= t)
                    {
                        if (arrivalTimeList[i] <= quantum)
                        {
                            if (burstTimeList[i] > 0)
                            {
                                checkProcessID = false;
                                if (burstTimeList[i] > quantum)
                                {
                                    // Make decrease the b time
                                    t = t + quantum;
                                    burstTimeList[i] = burstTimeList[i] - quantum;
                                    arrivalTimeList[i] = arrivalTimeList[i] + quantum;
                                    /*
                                    for (int j = 0; j < quantum; j++)
                                    {
                                        ganttChart.Add(processList[i]);
                                        // Printing Gant Chart
                                        TextBox gantt_TextBox = new TextBox();
                                        gantChart_grid.Children.Add(gantt_TextBox);
                                        gantt_TextBox.Width = 25; gantt_TextBox.Height = 25;
                                        gantt_TextBox.Text = ("P" + Convert.ToString(processList[i].processID));
                                        gantt_TextBox.Background = processList[i].processColor;
                                        gantt_TextBox.Name = "textBox1";
                                        gantt_TextBox.IsReadOnly = true;
                                        gantt_TextBox.Padding = new Thickness(X, 34, 0, 0);

                                        X += (int)gantt_TextBox.Width;
                                    }
                                    */
                                }
                                else
                                {
                                    // For last time
                                    t = t + burstTimeList[i];
                                    /*
                                    for (int j = 0; j < burstTimeList[i]; j++)
                                    {
                                        ganttChart.Add(processList[i]);
                                        TextBox gantt_TextBox = new TextBox();
                                        gantChart_grid.Children.Add(gantt_TextBox);
                                        gantt_TextBox.Width = 25; gantt_TextBox.Height = 25;
                                        gantt_TextBox.Text = ("P" + Convert.ToString(processList[i].processID));
                                        gantt_TextBox.Background = processList[i].processColor;
                                        gantt_TextBox.Name = "textBox1";
                                        gantt_TextBox.IsReadOnly = true;
                                        gantt_TextBox.Padding = new Thickness(X, 34, 0, 0);
                                        X += (int)gantt_TextBox.Width;
                                    }
                                    */
                                    // Store comp. time
                                    processList[i].turnAroundTime = t - processList[i].arrivalTime;
                                    // Store wait time
                                    processList[i].waitingTime = t - processList[i].burstTime - processList[i].arrivalTime;
                                    burstTimeList[i] = 0;
                                }
                            }
                        }
                        else if (arrivalTimeList[i] > quantum)
                        {
                            // Is any have less arrival time.
                            // the coming process then execute them
                            for (int j = 0; j < processList.Count; j++)
                            {
                                // Compare
                                if (arrivalTimeList[j] < arrivalTimeList[i])
                                {
                                    if (burstTimeList[j] > 0)
                                    {
                                        checkProcessID = false;
                                        if (burstTimeList[j] > quantum)
                                        {
                                            t += quantum;
                                            burstTimeList[j] = burstTimeList[j] - quantum;
                                            arrivalTimeList[j] = arrivalTimeList[j] + quantum;
                                            /*
                                            for (int k = 0; k < quantum; k++)
                                            {
                                                ganttChart.Add(processList[j]);
                                                // Printing Gant Chart
                                                TextBox gantt_TextBox = new TextBox();
                                                gantChart_grid.Children.Add(gantt_TextBox);
                                                gantt_TextBox.Width = 25; gantt_TextBox.Height = 25;
                                                gantt_TextBox.Text = ("P" + Convert.ToString(processList[j].processID));
                                                gantt_TextBox.Background = processList[j].processColor;
                                                gantt_TextBox.Name = "textBox1";
                                                gantt_TextBox.IsReadOnly = true;
                                                gantt_TextBox.Padding = new Thickness(X, 34, 0, 0);
                                                X += (int)gantt_TextBox.Width;
                                            }
                                            */
                                            processList[i].turnAroundTime = t - processList[i].arrivalTime;
                                            processList[i].waitingTime = t - processList[i].burstTime - processList[i].arrivalTime;
                                            burstTimeList[i] = 0;
                                        }
                                        else
                                        {
                                            t = t + burstTimeList[j];
                                            /*
                                            for (int h = 0; h < burstTimeList[j]; h++)
                                            {
                                                ganttChart.Add(processList[j]);
                                                //printing Gant chart
                                                TextBox gantt_TextBox = new TextBox();
                                                gantChart_grid.Children.Add(gantt_TextBox);
                                                gantt_TextBox.Width = 25; gantt_TextBox.Height = 25;
                                                gantt_TextBox.Text = ("P" + Convert.ToString(processList[j].processID));
                                                gantt_TextBox.Background = processList[j].processColor;
                                                gantt_TextBox.Name = "textBox1";
                                                gantt_TextBox.IsReadOnly = true;
                                                gantt_TextBox.Padding = new Thickness(X, 34, 0, 0);
                                                X += (int)gantt_TextBox.Width;
                                            }
                                            */
                                            processList[j].turnAroundTime = t - processList[j].arrivalTime;
                                            processList[j].waitingTime = t - processList[j].burstTime - processList[j].arrivalTime;
                                            burstTimeList[j] = 0;
                                        }
                                    }
                                }
                            }

                            // Now the previous process according to ith is process
                            if (burstTimeList[i] > 0)
                            {
                                checkProcessID = false;

                                // Check for greaters
                                if (burstTimeList[i] > quantum)
                                {
                                    t = t + quantum;
                                    burstTimeList[i] = burstTimeList[i] - quantum;
                                    arrivalTimeList[i] = arrivalTimeList[i] + quantum;
                                    /*
                                    for (int j = 0; j < quantum; j++)
                                    {
                                        ganttChart.Add(processList[i]);
                                        //printing Gant chart
                                        TextBox gantt_TextBox = new TextBox();
                                        gantChart_grid.Children.Add(gantt_TextBox);
                                        gantt_TextBox.Width = 25; gantt_TextBox.Height = 25;
                                        gantt_TextBox.Text = ("P" + Convert.ToString(processList[i].processID));
                                        gantt_TextBox.Background = processList[i].processColor;
                                        gantt_TextBox.Name = "textBox1";
                                        gantt_TextBox.IsReadOnly = true;
                                        gantt_TextBox.Padding = new Thickness(X, 34, 0, 0);
                                        X += (int)gantt_TextBox.Width;
                                    }
                                    */
                                }
                                else
                                {
                                    t = t + burstTimeList[i];
                                    /*
                                    for (int j = 0; j < burstTimeList[i]; j++)
                                    {
                                        ganttChart.Add(processList[i]);
                                        //printing Gant chart
                                        TextBox gantt_TextBox = new TextBox();
                                        gantChart_grid.Children.Add(gantt_TextBox);
                                        gantt_TextBox.Width = 25; gantt_TextBox.Height = 25;
                                        gantt_TextBox.Text = ("P" + Convert.ToString(processList[i].processID));
                                        gantt_TextBox.Background = processList[i].processColor;
                                        gantt_TextBox.Name = "textBox1";
                                        gantt_TextBox.IsReadOnly = true;
                                        gantt_TextBox.Padding = new Thickness(X, 34, 0, 0);
                                        X += (int)gantt_TextBox.Width;
                                    }
                                    */
                                    processList[i].turnAroundTime = t - processList[i].arrivalTime;
                                    processList[i].waitingTime = t - processList[i].burstTime - processList[i].arrivalTime;
                                    burstTimeList[i] = 0;
                                }
                            }
                        }
                    }

                    // If no process is come on the critical
                    else if (arrivalTimeList[i] > t)
                    {
                        t++;
                        i--;
                    }
                }
                // For exit the while loop
                if (checkProcessID)
                    break;

            }

            for (int i = 0; i < processList.Count; i++)
            {
                totalWaitingTime += processList[i].waitingTime;
                totalTurnAroundTime += processList[i].turnAroundTime;
            }
            //PrintGanttChart(ganttChart);
        }

        // Method that calculates average time.
        private void SRTFFindAvgTime(List<Process> processList, int n)
        {
            pervAlgo = 4;
            minimum = int.MaxValue;
            // Copy the burst time into rt[]
            for (int i = 0; i < n; i++)
            {
                burstTimeList.Add(processList[i].burstTime); // making new burst time list.
            }
            // Do until all processes are done.
            while (n != complete)
            {
                // Find minimum remaining time process, from process arrived till now.
                for (int i = 0; i < n; i++)
                {
                    if ((processList[i].arrivalTime <= t) && (burstTimeList[i] < minimum) && burstTimeList[i] > 0)
                    {
                        minimum = burstTimeList[i];
                        shortest = i;
                        checkProcessID = true;
                    }
                }

                if (false == checkProcessID)
                {
                    t++;
                    continue;
                }
                // ganttChart.Add(processList[shortest]);
                // reduce remaining time by one
                burstTimeList[shortest]--;
                /*

                // Printing Gantt Chart //TODO
                TextBox gantt_TextBox = new TextBox();
                gantChart_grid.Children.Add(gantt_TextBox);
                gantt_TextBox.Width = 25; gantt_TextBox.Height = 25;
                gantt_TextBox.Text = ("P" + Convert.ToString(processList[shortest].processID));
                gantt_TextBox.Background = processList[shortest].processColor;
                gantt_TextBox.Name = "textBox1";
                gantt_TextBox.IsReadOnly = true;
                gantt_TextBox.Padding = new Thickness(X, 34, 0, 0);

                

                X += (int)gantt_TextBox.Width;
                */

                // Update minimum
                minimum = burstTimeList[shortest];

                // If process is done executing
                if (0 == burstTimeList[shortest])
                {
                    minimum = int.MaxValue;
                    // Increment complete
                    complete++;
                    checkProcessID = false;

                    // Finish time in array
                    completionTime = t + 1;
                    // Waiting time calculate
                    processList[shortest].turnAroundTime = completionTime - processList[shortest].arrivalTime;
                    processList[shortest].waitingTime = processList[shortest].turnAroundTime - processList[shortest].burstTime;

                    if (processList[shortest].waitingTime < 0)
                        processList[shortest].waitingTime = 0;
                }
                //Move next in time
                t++;
            }
            //PrintGanttChart(ganttChart);
            for (int i = 0; i < n; i++)
            {
                totalWaitingTime += processList[i].waitingTime;
                totalTurnAroundTime += processList[i].turnAroundTime;
            }
        }
        /*
        private void PrintGanttChart(List<Process> ganttChart)
        {
            int num = 0;
            TextBox gantt_TextBox_1 = new TextBox();
            gantChart_grid.Children.Add(gantt_TextBox_1);
            gantt_TextBox_1.Width = 25; gantt_TextBox_1.Height = 25;
            gantt_TextBox_1.Background = ganttChart[0].processColor;
            gantt_TextBox_1.Name = "textBox6";
            gantt_TextBox_1.IsReadOnly = true;
            gantt_TextBox_1.Text = (0.ToString());

            for (; num < ganttChart.Count; num++)
            {
                if (num < (ganttChart.Count - 1))
                {
                    if (ganttChart[num].processID != ganttChart[num + 1].processID)
                    {
                        TextBox gantt_TextBox_2 = new TextBox();
                        gantChart_grid.Children.Add(gantt_TextBox_2);
                        gantt_TextBox_2.Width = 25; gantt_TextBox_2.Height = 25;
                        gantt_TextBox_2.Background = processList[num - 1 ].processColor;
                        gantt_TextBox_2.Name = "textBox2 ";
                        gantt_TextBox_2.IsReadOnly = true;
                        gantt_TextBox_2.Text = (num.ToString());
                        gantt_TextBox_2.Padding = new Thickness(Y, 59, 0, 0);


                        TextBox gantt_TextBox_3 = new TextBox();
                        gantChart_grid.Children.Add(gantt_TextBox_3);
                        gantt_TextBox_3.Width = 25; gantt_TextBox_3.Height = 25;
                        gantt_TextBox_3.Background = processList[num + 1].processColor;
                        gantt_TextBox_3.Name = "textBox5 ";
                        gantt_TextBox_3.IsReadOnly = true;
                        gantt_TextBox_3.Text = ((num + 1).ToString());
                        gantt_TextBox_3.Padding = new Thickness(Y + gantt_TextBox_2.Width, 59, 0, 0);

                    }
                    Y += 25;
                }
                //TODO LOCATION
                if (num == (ganttChart.Count - 1))
                {
                    TextBox gantt_TextBox_4 = new TextBox();
                    gantChart_grid.Children.Add(gantt_TextBox_4);
                    gantt_TextBox_4.Width = 25; gantt_TextBox_4.Height = 25;
                    gantt_TextBox_4.Background = 0 != num ? processList[num - 1].processColor : processList[num].processColor;
                    gantt_TextBox_4.Name = "textBox7";
                    gantt_TextBox_4.Padding = new Thickness(Y, 59, 0, 0);
                    gantt_TextBox_4.IsReadOnly = true;
                    gantt_TextBox_4.Text = (num.ToString());
                }
            }
        }

        */


    }
}
    
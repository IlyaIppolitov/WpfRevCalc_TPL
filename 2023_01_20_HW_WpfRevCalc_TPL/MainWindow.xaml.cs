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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2023_01_20_HW_WpfRevCalc_TPL
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

        //decimal total = 0;
        decimal totalIncome = 0;
        decimal totalOutcome = 0;
        string incomeTaskId = string.Empty;
        string outcomeTaskId = string.Empty;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string incomeFile = @"D:\FilesToRead\income.txt";
            string outcomeFile = @"D:\FilesToRead\outcome.txt";
            Parallel.Invoke(() => calcTotalIncome(incomeFile), () => calcTotalOutcome(outcomeFile));
            MessageBox.Show($"Thread of Income calc task = {incomeTaskId}\nThread of Outcome calc task = {outcomeTaskId}");
            textBoxTotal.Text = $"Прибыль: {(totalIncome - totalOutcome).ToString()}";
        }
        void calcTotalIncome(string incomeFile)
        {
            totalIncome = 0;
            object syncObject = new object();
            string[] incomeLines = System.IO.File.ReadAllLines(incomeFile);
            incomeTaskId = Environment.CurrentManagedThreadId.ToString();
            foreach (string line in incomeLines)
            {
                // для общего случая предсматриватеся блокировка доступа
                lock (syncObject) { totalIncome += decimal.Parse(line); }
            }
        }
        void calcTotalOutcome(string outcomeFile)
        {
            totalOutcome = 0;
            object syncObject = new object();
            string[] outcomeLines = System.IO.File.ReadAllLines(outcomeFile);
            outcomeTaskId = Environment.CurrentManagedThreadId.ToString();
            foreach (string line in outcomeLines)
            {
                // для общего случая предсматриватеся блокировка доступа
                lock (syncObject) { totalOutcome += decimal.Parse(line); }
            }
        }
    }
}

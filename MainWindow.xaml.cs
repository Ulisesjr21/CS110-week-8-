using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;

namespace OperationsHub;

// MainWindow handles the tracking, timing, and reporting for Sana Vey's Greenward crew.
public partial class MainWindow : Window
{
    // Collection to hold Greenward crew tasks dynamically
    private ObservableCollection<string> crewTasks;

    // Timer variables for tracking mission work
    private DispatcherTimer missionTimer;
    private int elapsedSeconds;
    private bool isTimerRunning;

    public MainWindow()
    {
        InitializeComponent();

        // Initialize our tasks collection and add default Greenward notes
        crewTasks = new ObservableCollection<string>
        {
            "Check hydroponics pressure logs",
            "Recalibrate solar array grid",
            "Inventory rations & medical kits",
            "Clear debris from airlock 3"
        };

        // DATA HAT: Bind our collection directly to the ListBox from your XAML
        lstItems.ItemsSource = crewTasks;

        // Initialize the DispatcherTimer for 1-second intervals
        missionTimer = new DispatcherTimer();
        missionTimer.Interval = TimeSpan.FromSeconds(1);
        missionTimer.Tick += MissionTimer_Tick;

        elapsedSeconds = 0;
        isTimerRunning = false;

        // Set an initial display for our timer matching our backend tracker
        txtTimer.Text = "00:00";
    }

    // Every second, update the timer display (Format: MM:SS)
    private void MissionTimer_Tick(object? sender, EventArgs e)
    {
        elapsedSeconds++;
        TimeSpan time = TimeSpan.FromSeconds(elapsedSeconds);
        txtTimer.Text = $"{time:mm\\:ss}";
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        // Read text from the entry text box. If empty, give a generic task name.
        string inputTask = txtEntry.Text.Trim();

        if (string.IsNullOrEmpty(inputTask))
        {
            inputTask = "New Crew Assignment #" + (crewTasks.Count + 1);
        }

        crewTasks.Add(inputTask);
        txtEntry.Clear(); // Clear the field for the next entry
        txtStatus.Text = $"Pulled scattered note into hub: '{inputTask}'";
    }

    private void Remove_Click(object sender, RoutedEventArgs e)
    {
        // Check if the user has selected a specific task from the ListBox
        if (lstItems.SelectedItem != null)
        {
            string selectedTask = lstItems.SelectedItem.ToString() ?? "";
            crewTasks.Remove(selectedTask);
            txtStatus.Text = $"Completed and archived crew task: '{selectedTask}'";
        }
        // Fallback: If nothing is selected but tasks exist, remove the first one
        else if (crewTasks.Count > 0)
        {
            string removed = crewTasks[0];
            crewTasks.RemoveAt(0);
            txtStatus.Text = $"Completed and archived task: '{removed}'";
        }
        else
        {
            txtStatus.Text = "No active tasks left in Greenward's logs!";
        }
    }

    private void Timer_Click(object sender, RoutedEventArgs e)
    {
        if (!isTimerRunning)
        {
            missionTimer.Start();
            isTimerRunning = true;
            txtStatus.Text = "Mission timer STARTED. Tracking crew labor hours...";
        }
        else
        {
            missionTimer.Stop();
            isTimerRunning = false;
            txtStatus.Text = $"Mission timer PAUSED at {txtTimer.Text}.";
        }
    }

    private void Reset_Click(object sender, RoutedEventArgs e)
    {
        missionTimer.Stop();
        isTimerRunning = false;
        elapsedSeconds = 0;
        txtTimer.Text = "00:00";
        txtStatus.Text = "Mission timer reset to 00:00.";
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        // Formulate the Greenward operational status report text layout
        string reportContent = $"=== GREENWARD HUB OPERATIONS REPORT ===\n";
        reportContent += $"Generated on: {DateTime.Now}\n";
        reportContent += $"Total Mission Time Logged: {txtTimer.Text}\n\n";
        reportContent += $"--- Crew Workspace Content ---\n";
        reportContent += txtDocument.Text + "\n\n";
        reportContent += $"--- Remaining Scattered Tasks ---\n";

        foreach (var task in crewTasks)
        {
            reportContent += $"- [ ] {task}\n";
        }

        // Open up a standard Windows dialog to let the user save the report file
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
        saveFileDialog.FileName = "Greenward_Ops_Report.txt";

        if (saveFileDialog.ShowDialog() == true)
        {
            File.WriteAllText(saveFileDialog.FileName, reportContent);
            txtStatus.Text = $"Report securely compiled and saved to: {Path.GetFileName(saveFileDialog.FileName)}";
        }
    }

    private void Open_Click(object sender, RoutedEventArgs e)
    {
        // Open a file dialog to read a previously saved report back in
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

        if (openFileDialog.ShowDialog() == true)
        {
            string fileContent = File.ReadAllText(openFileDialog.FileName);
            // Push the loaded text straight into your large document textbox layout
            txtDocument.Text = fileContent;
            txtStatus.Text = $"Successfully loaded document: {Path.GetFileName(openFileDialog.FileName)}";
        }
    }

        private void Image_Click(object sender, RoutedEventArgs e)
    {
        // Quick visual toggle confirmation for your Artwork placeholder
        txtStatus.Text = "Greenward Blueprint Graphics loaded successfully into placeholder.";
    } // close Image_Click

} // close MainWindow class

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;

namespace OperationsHub;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<string> crewTasks;

    private readonly DispatcherTimer missionTimer;

    private int elapsedSeconds;
    private bool isTimerRunning;

    private int totalTasksCreated;
    private int completedTasks;

    public MainWindow()
    {
        InitializeComponent();

        crewTasks = new ObservableCollection<string>
        {
            "Check hydroponics pressure logs",
            "Recalibrate solar array grid",
            "Inventory rations & medical kits",
            "Clear debris from airlock 3"
        };

        // Connect the collection to the task list.
        lstItems.ItemsSource = crewTasks;

        // Set up the mission timer.
        missionTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        missionTimer.Tick += MissionTimer_Tick;

        elapsedSeconds = 0;
        isTimerRunning = false;

        totalTasksCreated = crewTasks.Count;
        completedTasks = 0;

        txtTimer.Text = "00:00";

        UpdateDashboard();

        txtStatus.Text =
            "Greenward hub online. Ready for crew assignments.";

        Closed += MainWindow_Closed;
    }

    private void MainWindow_Closed(object sender, EventArgs e)
    {
        missionTimer.Stop();
        isTimerRunning = false;
    }

    // Update the timer every second.
    private void MissionTimer_Tick(object sender, EventArgs e)
    {
        elapsedSeconds++;

        TimeSpan time = TimeSpan.FromSeconds(elapsedSeconds);

        // Display total minutes instead of wrapping after 59.
        txtTimer.Text =
            $"{(int)time.TotalMinutes:00}:{time.Seconds:00}";
    }

    // Refresh dashboard counts and mission progress.
    private void UpdateDashboard()
    {
        txtTaskCount.Text = crewTasks.Count.ToString("00");
        txtCompletedCount.Text = completedTasks.ToString("00");

        double progress = 0;

        if (totalTasksCreated > 0)
        {
            progress =
                (double)completedTasks / totalTasksCreated * 100;
        }

        // Clamp the percentage for compatibility with older frameworks.
        progress = Math.Max(0, Math.Min(100, progress));

        progressMission.Value = progress;
        txtProgressPercent.Text = $"{progress:0}% complete";
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        string inputTask = txtEntry.Text.Trim();

        if (string.IsNullOrWhiteSpace(inputTask))
        {
            inputTask =
                "New Crew Assignment #" + (totalTasksCreated + 1);
        }

        crewTasks.Add(inputTask);
        totalTasksCreated++;

        txtEntry.Clear();

        UpdateDashboard();

        txtStatus.Text =
            $"New crew assignment added: '{inputTask}'";
    }

    private void Remove_Click(object sender, RoutedEventArgs e)
    {
        if (lstItems.SelectedItem == null)
        {
            txtStatus.Text =
                "Select a task before removing it.";
            return;
        }

        string selectedTask = lstItems.SelectedItem.ToString();

        if (selectedTask == null)
        {
            txtStatus.Text = "Unable to read the selected task.";
            return;
        }

        crewTasks.Remove(selectedTask);
        completedTasks++;

        UpdateDashboard();

        txtStatus.Text =
            $"Crew assignment completed: '{selectedTask}'";
    }

    private void Timer_Click(object sender, RoutedEventArgs e)
    {
        if (!isTimerRunning)
        {
            missionTimer.Start();
            isTimerRunning = true;

            txtStatus.Text =
                "Mission timer started. Tracking crew work time.";
        }
        else
        {
            missionTimer.Stop();
            isTimerRunning = false;

            txtStatus.Text =
                $"Mission timer paused at {txtTimer.Text}.";
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
        string reportContent =
            "=== GREENWARD HUB OPERATIONS REPORT ===" +
            Environment.NewLine;

        reportContent +=
            $"Generated on: {DateTime.Now}" +
            Environment.NewLine;

        reportContent +=
            $"Total Mission Time Logged: {txtTimer.Text}" +
            Environment.NewLine;

        reportContent +=
            $"Active Tasks: {crewTasks.Count}" +
            Environment.NewLine;

        reportContent +=
            $"Completed Tasks This Session: {completedTasks}" +
            Environment.NewLine;

        reportContent +=
            $"Mission Progress: {txtProgressPercent.Text}" +
            Environment.NewLine +
            Environment.NewLine;

        reportContent +=
            "--- Crew Workspace Notes ---" +
            Environment.NewLine;

        reportContent +=
            txtDocument.Text +
            Environment.NewLine +
            Environment.NewLine;

        reportContent +=
            "--- Remaining Crew Tasks ---" +
            Environment.NewLine;

        foreach (string task in crewTasks)
        {
            reportContent +=
                $"- [ ] {task}" + Environment.NewLine;
        }

        SaveFileDialog dialog = new SaveFileDialog
        {
            Title = "Export Greenward Mission Report",
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
            DefaultExt = ".txt",
            AddExtension = true,
            FileName = "Greenward_Ops_Report.txt"
        };

        if (dialog.ShowDialog(this) != true)
        {
            txtStatus.Text = "Report export canceled.";
            return;
        }

        try
        {
            File.WriteAllText(dialog.FileName, reportContent);

            txtStatus.Text =
                "Mission report saved: " +
                Path.GetFileName(dialog.FileName);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "The report could not be saved.\n\n" + ex.Message,
                "Report Export Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            txtStatus.Text = "Report export failed.";
        }
    }

    private void Open_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog
        {
            Title = "Reopen Greenward Text Report",
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
            CheckFileExists = true
        };

        if (dialog.ShowDialog(this) != true)
        {
            txtStatus.Text = "Report loading canceled.";
            return;
        }

        try
        {
            string fileContent = File.ReadAllText(dialog.FileName);

            // Preserve the original behavior: reopen the file
            // in the workspace notes area.
            txtDocument.Text = fileContent;

            txtStatus.Text =
                "Document loaded: " +
                Path.GetFileName(dialog.FileName);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "The document could not be opened.\n\n" + ex.Message,
                "Document Loading Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            txtStatus.Text = "Document loading failed.";
        }
    }

    private void Image_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog
        {
            Title = "Choose Greenward Blueprint Artwork",
            Filter =
                "Image files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp",
            CheckFileExists = true
        };

        if (dialog.ShowDialog(this) != true)
        {
            txtStatus.Text = "Artwork selection canceled.";
            return;
        }

        try
        {
            BitmapImage artwork = new BitmapImage();

            artwork.BeginInit();
            artwork.CacheOption =
                BitmapCacheOption.OnLoad;
            artwork.UriSource =
                new Uri(dialog.FileName, UriKind.Absolute);
            artwork.EndInit();
            artwork.Freeze();

            imgArtwork.Source = artwork;
            imgArtwork.Visibility = Visibility.Visible;

            imgButton.Source = artwork;

            txtStatus.Text =
                "Blueprint artwork loaded: " +
                Path.GetFileName(dialog.FileName);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "The image could not be loaded.\n\n" + ex.Message,
                "Artwork Loading Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            txtStatus.Text = "Artwork loading failed.";
        }
    }
}
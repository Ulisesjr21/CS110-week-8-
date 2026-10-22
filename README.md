Greenward Operations Hub
Project Overview
For this assignment, I developed a Windows desktop application using C# and WPF in Visual Studio. I chose the Operations Hub option and customized the application around Greenward crew from Week 7.

The purpose of the Greenward Operations Hub is to provide one central location where the crew can manage assignments, track mission time, write notes, monitor progress, and save operational reports. I updated the original interface to give the application a more futuristic mission-control appearance while maintaining its core functionality.

The updated design includes:

Navy and cyan color scheme: Dark backgrounds, cyan highlights, and contrasting text create a futuristic mission-control theme.
Organized dashboard: The interface separates crew assignments, mission timing, notes, reports, and artwork into clear sections.
Improved buttons: Buttons have updated colors, spacing, and labels that better match the Greenward theme.
Greenward branding: The dashboard includes mission-control headings and labels that connect the application to the Greenward crew's story.
Blueprint artwork: A simple schematic provides a visual element that fits the science-fiction setting.
The goal of these changes was to make the application easier to navigate while giving it a more polished and consistent appearance.

Application Features
1. Crew Task Management
The task manager allows users to enter new assignments and add them to a task list. Users can select an assignment and remove it when it is completed.

The application uses an ObservableCollection in C# to manage the task list and display changes in the interface.

2. Mission Timer
The mission timer allows the crew to track the time spent working on assignments.

Users can:

Start the timer.
Pause the timer.
Reset the timer to zero.
The timer uses WPF's DispatcherTimer to update the displayed time at regular intervals.

3. Crew Notes and Workspace
The notes section provides a space for recording mission observations, maintenance details, supply needs, and information for the next crew shift.

The notes are included in the operational report when the user exports the report.

4. Mission Progress
I added a progress section to the dashboard to make task progress easier to understand at a glance. The interface includes a progress bar and labels for active and completed assignments.

These elements provide a visual overview of the crew's workload. The progress values depend on the C# logic connected to the interface.

5. Export and Reopen Reports
The report feature allows users to save operational information to a text file and reopen a previously saved report.

The export feature is designed to include:

The date and time the report is generated.
The recorded mission time.
The contents of the crew notes.
The remaining task list.
The application uses SaveFileDialog and OpenFileDialog to let users choose where to save reports and which existing text files to open.

6. Artwork and Visual Design
I included a Greenward blueprint-style graphic to reinforce the futuristic mission-control theme. The interface also retains artwork-related controls for selecting an image.

This provides a foundation for adding more visual elements and custom Greenward artwork in the future.

Technical Details
The application is built with C# and Windows Presentation Foundation (WPF).

MainWindow.xaml

Defines the dashboard layout and visual styling.
Organizes the task manager, timer, notes, progress display, report controls, and artwork.
Uses XAML controls with named elements so the C# code can interact with the interface.
MainWindow.xaml.cs

Contains the application logic and button event handlers.
Manages task assignments and the mission timer.
Handles saving and reopening text reports.
Updates the interface in response to user actions.
The XAML and C# files work together: XAML defines what the user sees, while C# controls how the application behaves.

Testing and Improvements
After updating the interface, the application should be built and tested to confirm that the XAML control names match the C# code and that each button performs its intended action.

Testing includes adding and removing tasks, starting and resetting the timer, editing notes, exporting reports, reopening text files, and selecting artwork.

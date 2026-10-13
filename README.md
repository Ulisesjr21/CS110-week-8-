
Greenward Operations Hub


Project Overview

For this assignment, I built a Windows desktop app using C# and WPF in Visual Studio. I chose the Operations Hub route and customized it for Greenward crew from Week 7. The app gives the crew one central dashboard to track tasks, time their work, and save reports instead of using messy, scattered notes.

What the App Does

• Track Tasks: Users can type a task into the text box and click Add to put it on the list. Selecting a task and clicking Remove selected deletes it when the work is done.
• Mission Timer: The Start / Pause button runs a live clock that ticks up second by second so the crew can track their labor hours. Reset clears it back to zero.
• Save & Load Reports: The Export contract button takes the live timer, the current date, any notes typed into the workspace box, and the remaining task list, and saves it all into a clean .txt file. The Reopen text button lets users load an old report back into the app.

Technical Details

• UI (MainWindow.xaml): Maintained the original layout, styles, and dark blue theme colors, but updated the subtitles to match the story context.
• Logic (MainWindow.xaml.cs): Used an ObservableCollection to link the data list to the screen, a DispatcherTimer to handle the clock, and standard file dialogs (SaveFileDialog / OpenFileDialog) for saving and reading text files.

Plans for the app 
I plan on adding a different layout chose different colors and overall make it mor futuristic and clean. maybe add more details and working buttons. 

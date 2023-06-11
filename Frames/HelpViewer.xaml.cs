using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace HCI.Frames
{
    public partial class HelpViewer : Window
    {
        private Dictionary<int, List<string>> modePages = new Dictionary<int, List<string>>
        {
            { 0, new List<string> { "login", "register" } },
            { 1, new List<string> { "clienthome", "attractions", "clienthistory", "historydetails" } },
            { 2, new List<string> { "agenthome","agenttripsform","agentrestaurants","agentrestaurantsform", "agentattractions","agentattractionsform", "agentaccommodations","agentaccommodationsform","agentlocationform" } }
        };

        private Dictionary<int, Stack<string>> navigationHistory = new Dictionary<int, Stack<string>>();
        private bool isNavigating = false;
        private int currentMode = -1;

        private static HelpViewer instance;

        public static HelpViewer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HelpViewer();
                }
                return instance;
            }
        }

        private HelpViewer()
        {
            InitializeComponent();

            CommandBinding backCommandBinding = new CommandBinding(NavigationCommands.BrowseBack, BrowseBack_Executed, BrowseBack_CanExecute);
            CommandBindings.Add(backCommandBinding);

            CommandBinding forwardCommandBinding = new CommandBinding(NavigationCommands.BrowseForward, BrowseForward_Executed, BrowseForward_CanExecute);
            CommandBindings.Add(forwardCommandBinding);
        }

        public void ShowHelpWindow(string key, MainWindow originator, int mode)
        {
            if (!modePages.ContainsKey(mode))
            {
                MessageBox.Show("Invalid mode.");
                return;
            }

            List<string> documentationPages = modePages[mode];

            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            string solutionPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(assemblyPath)));


            DirectoryInfo solutionDirectory = Directory.GetParent(solutionPath);
            string parentFolderPath = solutionDirectory?.FullName;

            string path = Path.Combine(parentFolderPath, "Help", key + ".html");

            if (!File.Exists(path))
            {
                key = "error";
            }

            Uri u = new Uri(path);

            if (!navigationHistory.ContainsKey(mode))
            {
                navigationHistory[mode] = new Stack<string>();
                navigationHistory[mode].Push(key);
            }
            else if (navigationHistory[mode].Peek() != key)
            {
                navigationHistory[mode].Push(key);
            }

            currentMode = mode; 

            wbHelp.Navigating += wbHelp_Navigating;
            wbHelp.LoadCompleted += wbHelp_LoadCompleted;
            wbHelp.Navigate(u);

            CommandManager.InvalidateRequerySuggested(); 

            Show();
        }


        private int GetCurrentPageIndex()
        {
            if (currentMode != -1 && navigationHistory.ContainsKey(currentMode))
            {
                string currentPage = navigationHistory[currentMode].Peek();
                List<string> currentPageList = modePages[currentMode];

                if (currentPageList != null)
                {
                    return currentPageList.IndexOf(currentPage);
                }
            }

            return -1; 
        }


        private void BrowseBack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = currentMode != -1 && navigationHistory.ContainsKey(currentMode) && navigationHistory[currentMode].Count > 1 && !isNavigating;
            e.Handled = true;
        }

        private void BrowseBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (navigationHistory[currentMode].Count > 1)
            {
                string currentPage = navigationHistory[currentMode].Pop(); 

                while (navigationHistory[currentMode].Count > 1 && navigationHistory[currentMode].Peek() == currentPage)
                {
                    navigationHistory[currentMode].Pop();
                }

                string previousPage = navigationHistory[currentMode].Peek();

                OpenDocumentationPage(previousPage);

                CommandManager.InvalidateRequerySuggested();
            }
        }

        private void BrowseForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (currentMode != -1 && navigationHistory.ContainsKey(currentMode))
            {
                int currentPageIndex = GetCurrentPageIndex();
                e.CanExecute = currentPageIndex < modePages[currentMode].Count - 1 && !isNavigating;
            }
            else
            {
                e.CanExecute = false;
            }

            e.Handled = true;
        }

        private void BrowseForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (navigationHistory.ContainsKey(currentMode)) 
            {
                Stack<string> currentPageStack = navigationHistory[currentMode];

                if (currentPageStack.Count > 0)
                {
                    string currentPage = currentPageStack.Peek();
                    List<string> currentPageList = modePages[currentMode];

                    if (currentPageList != null)
                    {
                        int currentPageIndex = currentPageList.IndexOf(currentPage);

                        if (currentPageIndex < currentPageList.Count - 1)
                        {
                            string nextPage = currentPageList[currentPageIndex + 1];
                            OpenDocumentationPage(nextPage);

                            CommandManager.InvalidateRequerySuggested();
                        }
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void wbHelp_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            isNavigating = true;
        }

        private void wbHelp_LoadCompleted(object sender, NavigationEventArgs e)
        {
            isNavigating = false;
        }

        private void OpenDocumentationPage(string page)
        {
            string curDir = Directory.GetCurrentDirectory();
            string projectDir = curDir;

            while (!string.IsNullOrEmpty(projectDir) && !projectDir.EndsWith("HCI"))
            {
                projectDir = Directory.GetParent(projectDir)?.FullName;
            }

            string path = Path.Combine(projectDir, "Help", page + ".html");

            if (!File.Exists(path))
            {
                page = "error";
            }

            Uri u = new Uri(path);

            // Check if the page is a new page
            if (navigationHistory[currentMode].Peek() != page)
            {
                navigationHistory[currentMode].Push(page); 
            }

            wbHelp.Navigate(u);
        }
    }
}

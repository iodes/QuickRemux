using System.Windows;
using System.Windows.Input;

namespace QuickRemux.Windows
{
    public class WindowBase : Window
    {
        public WindowBase()
        {
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CommandBinding_CloseExecuted));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, CommandBinding_MaximizeExecuted));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, CommandBinding_MinimizeExecuted));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, CommandBinding_RestoreExecuted));
        }

        private void CommandBinding_CloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void CommandBinding_MaximizeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void CommandBinding_MinimizeExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void CommandBinding_RestoreExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }
    }
}

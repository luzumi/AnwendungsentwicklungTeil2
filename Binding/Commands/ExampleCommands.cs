using System.Windows.Input;

namespace Binding
{
    public static class ExampleCommands
    {
        public static readonly RoutedUICommand RedAlert = new RoutedUICommand(
            "Alarmstufe Rot",
            "RedAlert",
            typeof(ExampleCommands),
            new InputGestureCollection() { new KeyGesture(Key.R, ModifierKeys.Control) });

        public static readonly RoutedUICommand NewWindow = new RoutedUICommand(
            "neues Fenster",
            "NewWindow",
            typeof(ExampleCommands),
            new InputGestureCollection() { new KeyGesture(Key.N, ModifierKeys.Control) });

        public static readonly RoutedUICommand CloseWindow = new RoutedUICommand(
            "Fenster schliessen",
            "CloseWindow",
            typeof(ExampleCommands),
            new InputGestureCollection() { new KeyGesture(Key.Q, ModifierKeys.Control) });

        public static readonly RoutedUICommand MySave = new RoutedUICommand(
            "speichern",
            "Save",
            typeof(ExampleCommands),
            new InputGestureCollection() { new KeyGesture(Key.S, ModifierKeys.Control) });

        
    }
}

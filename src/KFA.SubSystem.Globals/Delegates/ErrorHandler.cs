namespace KFA.SubSystem.Globals.Delegates;

public delegate void ErrorHandler(string message, string title = "Error", Exception? error = null);

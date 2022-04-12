namespace Project_Lykos;

public class State
{
    // Records current states of the form
    private readonly MainWindow window;
    // Dictionary of buttons and states
    private readonly Dictionary<Control, bool> buttonStates = new();
    // List of buttons to scan
    public List<Control> Buttons { set; get; } = new();
    public State(MainWindow form)
    {
        this.window = form;
    }


    
    /// <summary>
    /// Records the state of the buttons on the form and disables them
    /// </summary>
    public void FreezeButtons()
    {
        buttonStates.Clear();
        foreach (var button in Buttons)
        {
            buttonStates.Add(button, button.Enabled);
            button.BeginInvoke((MethodInvoker)delegate ()
            {   // Disable the button
                button.Enabled = false;
            });
        }
    }

    /// <summary>
    /// Reverts the buttons to their original state
    /// </summary>
    public void RestoreButtons()
    {
        foreach (var button in Buttons)
        {
            button.BeginInvoke((MethodInvoker)delegate ()
            {  // Re-enable the button
                button.Enabled = buttonStates[button];
            });
        }
    }
}
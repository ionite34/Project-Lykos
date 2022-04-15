namespace Project_Lykos;

public class State
{
    // Records current states of the form
    private readonly IMainWindow window;
    // Dictionary of buttons and states
    private readonly Dictionary<Control, bool> buttonStates = new();
    // List of buttons to scan
    public List<Control> Buttons { set; get; } = new();
    public State(IMainWindow form)
    {
        this.window = form;
    }


    
    /// <summary>
    /// Records the state of the buttons on the form and disables them
    /// </summary>
    public IAsyncResult FreezeButtons()
    {
        buttonStates.Clear();
        return window.BeginInvoke((MethodInvoker)delegate ()
        {   
            foreach (var button in Buttons)
            {
                buttonStates.Add(button, button.Enabled);
                // Disable the button
                button.Enabled = false;
            }
        });
    }

    /// <summary>
    /// Reverts the buttons to their original state
    /// </summary>
    public IAsyncResult RestoreButtons()
    {
        return window.BeginInvoke((MethodInvoker)delegate ()
        {
            foreach (var button in Buttons)
            {
                button.Enabled = buttonStates[button];
            }
        });
    }
}
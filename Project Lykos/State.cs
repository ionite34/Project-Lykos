namespace Project_Lykos;

public class State
{
    // Records current states of the form
    private readonly Form form;
    // Dictionary of buttons and states
    private readonly Dictionary<Control, bool> buttonStates = new();
    public State(Form form)
    {
        this.form = form;
    }

    /// <summary>
    /// Records the state of the buttons on the form and disables them
    /// </summary>
    public void FreezeButtons()
    {
        buttonStates.Clear();
        foreach (var button in form.Controls.OfType<Button>())
        {
            buttonStates.Add(button, button.Enabled);
            button.Enabled = false;
        }
    }

    /// <summary>
    /// Reverts the buttons to their original state
    /// </summary>
    public void RestoreButtons()
    {
        foreach (var button in form.Controls.OfType<Button>())
        {
            button.Enabled = buttonStates[button];
        }
    }
}
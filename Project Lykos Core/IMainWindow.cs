using System.Windows.Forms;

namespace Project_Lykos
{
    public interface IMainWindow
    {
        bool Progress1Running { get; }

        //
        // Summary:
        //     Executes the specified delegate asynchronously on the thread that the control's
        //     underlying handle was created on.
        //
        // Parameters:
        //   method:
        //     A delegate to a method that takes no parameters.
        //
        // Returns:
        //     An System.IAsyncResult that represents the result of the System.Windows.Forms.Control.BeginInvoke(System.Delegate)
        //     operation.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     No appropriate window handle can be found.
        IAsyncResult BeginInvoke(Delegate method);

        //
        // Summary:
        //     Executes the specified delegate asynchronously on the thread that the control's
        //     underlying handle was created on.
        //
        // Parameters:
        //   method:
        //     A delegate to a method that takes no parameters.
        //
        // Returns:
        //     An System.IAsyncResult that represents the result of the System.Windows.Forms.Control.BeginInvoke(System.Action)
        //     operation.
        IAsyncResult BeginInvoke(Action method);

        //
        // Summary:
        //     Executes the specified delegate asynchronously with the specified arguments,
        //     on the thread that the control's underlying handle was created on.
        //
        // Parameters:
        //   method:
        //     A delegate to a method that takes parameters of the same number and type that
        //     are contained in the args parameter.
        //
        //   args:
        //     An array of objects to pass as arguments to the given method. This can be null
        //     if no arguments are needed.
        //
        // Returns:
        //     An System.IAsyncResult that represents the result of the System.Windows.Forms.Control.BeginInvoke(System.Delegate)
        //     operation.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     No appropriate window handle can be found.
        IAsyncResult BeginInvoke(Delegate method, params object[] args);

    }
}
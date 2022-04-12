namespace Project_Lykos;

public static class Threading
{
    /// <summary>
    /// Runs a function in a new thread.
    /// </summary>
    /// <param name="function"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="initThreadAction"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static Task<TResult> RunInThread<T1, T2, TResult>(
        this Func<T1, T2, TResult> function,
        T1 param1,
        T2 param2,
        Action<Thread> initThreadAction = null)
    {
        var taskCompletionSource = new TaskCompletionSource<TResult>();

        var thread = new Thread(() =>
        {
            try
            {
                TResult result = function(param1, param2);
                taskCompletionSource.TrySetResult(result);
            }
            catch (Exception e)
            {
                taskCompletionSource.TrySetException(e);
            }
        });
        initThreadAction?.Invoke(thread);
        thread.Start();

        return taskCompletionSource.Task;
    }
}
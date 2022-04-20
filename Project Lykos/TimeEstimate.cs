namespace Project_Lykos;

public class TimeEstimate
{
    // Start time of the estimate
    private DateTime startTime;
    // Upper index of process batch (not count)
    private readonly int indexMax;
    // Flag to indicate time initialized
    public bool TimeInitialized { get; set; } = false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="maxCount">amount of items for completion</param>
    public TimeEstimate(int maxCount)
    {
        indexMax = maxCount + 1;
        startTime = DateTime.Now;
    }
    
    public void SetStartTime(DateTime time)
    {
        startTime = time;
        TimeInitialized = true;
    }

    /// <summary>
    /// Returns the estimated time of completion
    /// </summary>
    /// <param name="iterationsDone"></param>
    /// <returns>Estimated time of completion as DateTime</returns>
    public DateTime GetEtc(int iterationsDone)
    {
        var iterationsTotal = indexMax + 1;
        var msElapsed = DateTime.Now.Subtract(startTime).TotalMilliseconds;
        var unitTime = msElapsed / (double)iterationsDone;
        var etc = DateTime.Now.AddMilliseconds(unitTime * (iterationsTotal - iterationsDone));
        return etc;
    }
    
    public double GetItemsPerSecond(int iterationsDone)
    {
        var iterationsTotal = indexMax + 1;
        var msElapsed = DateTime.Now.Subtract(startTime).TotalMilliseconds;
        var unitTime = msElapsed / (double)iterationsDone; // ms per item
        var itemsPerSecond = 1000 / unitTime;
        return itemsPerSecond;
    }
}
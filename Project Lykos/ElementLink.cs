using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.UI.Core;
using ABI.Windows.System;

namespace Project_Lykos
{
    public static class ElementLink
    {
        /// <summary>
        /// Returns an IProgress object that updates provided UI elements
        /// </summary>
        /// <param name="window"></param>
        /// <param name="progressbar"></param>
        /// <param name="value"></param>
        /// <param name="status"></param>
        /// <param name="usePercent"></param>
        /// <param name="displayTotal"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static IProgress<(double current, double total)> LinkProgressBar2L(IMainWindow window, ProgressBar progressbar, Label value, Label status, string statusText, bool usePercent = false, bool displayTotal = false)
        {
            var progress = new Progress<(double current, double total)>(update =>
            {
                if (!window.Progress1Running) return;
                
                if (usePercent && displayTotal)
                {
                    throw new InvalidOperationException("Cannot use both percent and total");
                }

                var (current, total) = update;

                if (total == 0 || total < current)
                {
                    throw new IndexOutOfRangeException("Progress update provided invalid values.");
                }
                
                var newValue = (int)current;
                var totalValue = (int)total;
                var curPercent = progressbar.Value / total;
                
                // If the double values are above 99.0%, round them to 100.0%
                if (current / total > 0.99)
                {
                    totalValue = (int) total;
                    newValue = (int) total;
                    curPercent = 1;
                }

                progressbar.BeginInvoke((MethodInvoker)delegate ()
                {
                    if (progressbar.Style != ProgressBarStyle.Continuous)
                    {
                        progressbar.Style = ProgressBarStyle.Continuous;
                    }
                    
                    if (progressbar.Maximum != totalValue)
                    {
                        progressbar.Maximum = totalValue;
                    }

                    if (progressbar.Value != newValue)
                    {
                        progressbar.Value = newValue;
                    }

                    if (usePercent) // Mode '0.00%'
                    {
                        var p = curPercent.ToString("0%"); // Format percent
                        if (value.Text != p)
                        {
                            value.Text = p;
                        }
                    }
                    else if (displayTotal) // Mode '0,000/0,000'
                    {
                        var c = newValue.ToString("N0"); // Format commas
                        var t = totalValue.ToString("N0"); // Format commas
                        var str = $"{c}/{t}";
                        if (value.Text != str)
                        {
                            value.Text = str;
                        }
                    }
                    else // Mode: '0,000'
                    {
                        var c = newValue.ToString("N0"); // Format commas
                        if (value.Text != c)
                        {
                            value.Text = c;
                        }
                    }

                    // Labels
                    if (statusText != status.Text) status.Text = statusText;
                    if (!(value.Visible)) value.Visible = true;
                    if (!(status.Visible)) status.Visible = true;
                });
            });
            return progress;
        }


    /// <summary>
    /// Returns an IProgress object that updates provided UI elements
    /// </summary>
    /// <param name="window"></param>
    /// <param name="progressbar"></param>
    /// <param name="value"></param>
    /// <param name="status"></param>
    /// <param name="usePercent"></param>
    /// <param name="displayTotal"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public static IProgress<(int current, int total)> LinkProgressBarInt2L(IMainWindow window, ProgressBar progressbar, Label value, Label status, bool usePercent = false, bool displayTotal = false)
        {
            var progress = new Progress<(int current, int total)>(update =>
            {
                if (!window.Progress1Running) return;
                if (usePercent && displayTotal)
                {
                    throw new InvalidOperationException("Cannot use both percent and total");
                }

                var (current, total) = update;

                if (total == 0 || total < current)
                {
                    throw new IndexOutOfRangeException("Progress update provided invalid values.");
                }
                
                var newValue = current;
                var totalValue = total;
                var curPercent = progressbar.Value / total;

                progressbar.BeginInvoke((MethodInvoker)delegate ()
                {
                    if (progressbar.Style != ProgressBarStyle.Continuous)
                    {
                        progressbar.Style = ProgressBarStyle.Continuous;
                    }
                    
                    if (progressbar.Maximum != totalValue)
                    {
                        progressbar.Maximum = totalValue;
                    }
                    
                    if (progressbar.Value != newValue)
                    {
                        progressbar.Value = newValue;
                    }
                    
                    if (usePercent) // Mode '0.00%'
                    {
                        var p = curPercent.ToString("0%"); // Format percent
                        value.Text = p;
                    }
                    else if (displayTotal) // Mode '0,000/0,000'
                    {
                        var c = newValue.ToString("N0"); // Format commas
                        var t = totalValue.ToString("N0"); // Format commas
                        var str = $"{c}/{t}";
                        if (value.Text != str)
                        {
                            value.Text = str;
                        }
                    }
                    else // Mode: '0,000'
                    {
                        var c = newValue.ToString("N0"); // Format commas
                        if (value.Text != c)
                        {
                            value.Text = c;
                        }
                    }
                    
                    // Initialize the labels as visible.
                    if (!(value.Visible))
                    {
                        value.Visible = true;
                    }
                    if (!(status.Visible))
                    {
                        status.Visible = true;
                    }
                });
            });
            return progress;
        }

        public static IProgress<int> LinkLabel(Label label)
        {
            return new Progress<int>(update =>
            {
                // var c = update.ToString("N0"); // Format commas
                // if (label.Text != c) label.Text = c;
                //
                // // Set labels visible if not yet
                // if (!label.Visible)
                // {
                //     label.Visible = true;
                // }
            });
        }
    }
}

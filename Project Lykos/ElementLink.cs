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
    /// <summary>
    /// Static class for linking UI elements and progress reporting
    /// </summary>
    public static class ElementLink
    {

        /// <summary>
        /// Returns an IProgress object that updates provided UI elements
        /// </summary>
        /// <param name="progressbar"></param>
        /// <param name="value"></param>
        /// <param name="status"></param>
        /// <param name="usePercent"></param>
        /// <param name="displayTotal"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static IProgress<(double current, double total)> LinkProgressBar2L(ProgressBar progressbar, Label value, Label status, bool usePercent = false, bool displayTotal = false)
        {
            var progress = new Progress<(double current, double total)>(update =>
            {
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
                
                // If the double values are equal, override the int casting values.
                if (current == total)
                {
                    totalValue = (int) total;
                    newValue = (int) total;
                    curPercent = 1;
                }

                if (progressbar.Maximum != totalValue)
                {
                    progressbar.BeginInvoke((MethodInvoker)delegate ()
                    {
                        progressbar.Maximum = totalValue;
                    });
                }
                
                if (progressbar.Value != newValue)
                {
                    progressbar.BeginInvoke((MethodInvoker)delegate ()
                    {
                        progressbar.Value = newValue;
                    });
                }
                
                if (usePercent) // Mode '0.00%'
                {
                    var p = curPercent.ToString("0%"); // Format percent
                    if (value.Text != p)
                    {
                        value.BeginInvoke((MethodInvoker) delegate()
                        {
                            value.Text = p;
                        });
                    }
                }
                else if (displayTotal) // Mode '0,000/0,000'
                {
                    var c = newValue.ToString("N0"); // Format commas
                    var t = totalValue.ToString("N0"); // Format commas
                    var str = $"{c}/{t}";
                    if (value.Text != str)
                    {
                        value.BeginInvoke((MethodInvoker)delegate ()
                        {
                            value.Text = str;
                        });
                    }
                }
                else // Mode: '0,000'
                {
                    var c = newValue.ToString("N0"); // Format commas
                    if (value.Text != c)
                    {
                        value.BeginInvoke((MethodInvoker)delegate ()
                        {
                            value.Text = c;
                        });
                    }
                }
                
                // Initialize the labels as visible.
                if (!(value.Visible))
                {
                    value.BeginInvoke((MethodInvoker)delegate ()
                    {
                        value.Visible = true;

                    });
                }
                if (!(status.Visible))
                {
                    value.BeginInvoke((MethodInvoker)delegate ()
                    {
                        status.Visible = true;

                    });
                }
            });
            return progress;
        }


        /// <summary>
        /// Returns an IProgress object that updates provided UI elements
        /// </summary>
        /// <param name="progressbar"></param>
        /// <param name="value"></param>
        /// <param name="status"></param>
        /// <param name="usePercent"></param>
        /// <param name="displayTotal"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static IProgress<(int current, int total)> LinkProgressBarInt2L(ProgressBar progressbar, Label value, Label status, bool usePercent = false, bool displayTotal = false)
        {
            var progress = new Progress<(int current, int total)>(update =>
            {
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

                if (progressbar.Maximum != totalValue)
                {
                    progressbar.BeginInvoke((MethodInvoker)delegate ()
                    {
                        progressbar.Maximum = totalValue;
                    });
                }

                if (progressbar.Value != newValue)
                {
                    progressbar.BeginInvoke((MethodInvoker)delegate ()
                    {
                        progressbar.Value = newValue;
                    });
                }

                if (usePercent) // Mode '0.00%'
                {
                    var p = curPercent.ToString("0%"); // Format percent
                    if (value.Text != p)
                    {
                        value.BeginInvoke((MethodInvoker)delegate ()
                        {
                            value.Text = p;
                        });
                    }
                }
                else if (displayTotal) // Mode '0,000/0,000'
                {
                    var c = newValue.ToString("N0"); // Format commas
                    var t = totalValue.ToString("N0"); // Format commas
                    var str = $"{c}/{t}";
                    if (value.Text != str)
                    {
                        value.BeginInvoke((MethodInvoker)delegate ()
                        {
                            value.Text = str;
                        });
                    }
                }
                else // Mode: '0,000'
                {
                    var c = newValue.ToString("N0"); // Format commas
                    if (value.Text != c)
                    {
                        value.BeginInvoke((MethodInvoker)delegate ()
                        {
                            value.Text = c;
                        });
                    }
                }

                // Initialize the labels as visible.
                if (!(value.Visible))
                {
                    value.BeginInvoke((MethodInvoker)delegate ()
                    {
                        value.Visible = true;

                    });
                }
                if (!(status.Visible))
                {
                    value.BeginInvoke((MethodInvoker)delegate ()
                    {
                        status.Visible = true;

                    });
                }
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

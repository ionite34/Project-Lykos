using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static IProgress<(double current, double total)> LinkProgressBar2L(ProgressBar progressbar, Label value, Label status, bool usePercent = false, bool displayTotal = false)
        {
            var progress = new Progress<(double current, double total)>(update =>
            {
                if (usePercent && displayTotal)
                {
                    throw new InvalidOperationException("Cannot use both percent and total");
                }
                if (update.total == 0 || update.total < update.current)
                {
                    throw new IndexOutOfRangeException("Progress update provided invalid values.");
                }

                if (progressbar.Maximum != (int)update.total)
                {
                    progressbar.Maximum = (int)update.total;
                }

                var curPercent = progressbar.Value / update.total;
                var newPercent = update.current / update.total;
                var curValue = progressbar.Value;
                var newValue = (int)Math.Round(update.current);
                var totalValue = (int)Math.Round(update.total);

                if (curValue != newValue)
                {
                    progressbar.Value = newValue;
                }

                if (usePercent) // Mode '0.00%'
                {
                    var p = curPercent.ToString("0.00%"); // Format percent
                    if (value.Text != p) value.Text = p;
                }
                else if (displayTotal) // Mode '0,000/0,000'
                {
                    var c = newValue.ToString("N0"); // Format commas
                    var t = totalValue.ToString("N0"); // Format commas
                    var str = $"{c}/{t}";
                    if (value.Text != str) value.Text = str;
                }
                else // Mode: '0,000'
                {
                    var c = newValue.ToString("N0"); // Format commas
                    if (value.Text != c) value.Text = c;
                }

                // Detects if task completed
                if (update.current == update.total)
                {
                    value.Visible = false;
                    status.Visible = false;
                }
                else if (!value.Visible || !status.Visible)
                {
                    value.Visible = true;
                    status.Visible = true;
                }
            });
            return progress;
        }

        public static IProgress<int> LinkLabel(Label label)
        {
            return new Progress<int>(update =>
            {
                var c = update.ToString("N0"); // Format commas
                if (label.Text != c) label.Text = c;

                // Set labels visible if not yet
                if (!label.Visible)
                {
                    label.Visible = true;
                }
            });
        }
    }
}

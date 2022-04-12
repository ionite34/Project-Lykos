using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Lykos
{
    /// <summary>
    /// Simulates external data loading as a long operation.
    /// </summary>
    public class IndexLoader : INotifyPropertyChanged
    {
        /// <summary>
        /// Simulates data loading which takes long time to complete.
        /// This function can be interrupted by calling the Cancel method.
        /// When cancelled, method will exit at first convenient moment,
        /// which may occur after unknown period of time.
        /// </summary>
        /// <returns>Collection of integers, which represents loaded data.</returns>
        public ObservableCollection<int> Load()
        {

            Random rnd = new Random();
            ObservableCollection<int> data = new ObservableCollection<int>();
            int count = 1000;       // Number of items to fetch;
                                    // this method supposes that this number is known in advance;
                                    // if not, then it can be estimated using knowledge about particular data source,
                                    // e.g. if loading from file then file size is the count

            int pauseAfter = 7;     // Helper value, used to perform delays on coarser grain basis
            int avgPause = 30;      // Average pause length (in msec.)

            for (int i = 0; i < count && !_isCancelling; i++)
            {   // Loop iterates until complete or until cancel has been requested from the outside

                int value = 100000 + rnd.Next(900000);  // New value fetched from external source
                data.Add(value);

                if ((i + 1) % pauseAfter == 0)
                {   // Simulate delay that occurs in fetching external data
                    int pause = rnd.Next(avgPause * 2);
                    System.Threading.Thread.Sleep(pause);
                }

                SetProgress(i + 1, count);

            }

            if (_isCancelling)
            {   // Operation has been cancelled
                System.Threading.Thread.Sleep(1000 + rnd.Next(2000));   // Simulate cancellation delay
                SetCancelled();
                data.Clear();       // Method should return empty collection on cancellation
                                    // because data would anyway be incomplete
            }
            else
            {   // Operation was completed successfully
                SetComplete();
            }

            return data;

        }

        /// <summary>
        /// Sets amount of work completed up to this point.
        /// Method may change value returned by the Progress property,
        /// which in turn may raise PropertyChanged event.
        /// </summary>
        /// <param name="loaded">Number of items loaded.</param>
        /// <param name="total">Total number of items that should be loaded.</param>
        private void SetProgress(int loaded, int total)
        {
            int progress = 100 * loaded / total;
            if (_progress != progress)
            {
                _progress = progress;
                OnPropertyChanged("Progress");
            }
        }

        /// <summary>
        /// Sets IsCancelled property to true. If property value was false,
        /// then raises PropertyChanged event.
        /// </summary>
        private void SetCancelled()
        {
            if (!_isCancelled)
            {
                _isCancelled = true;
                OnPropertyChanged("IsCancelled");
            }
        }

        /// <summary>
        /// Sets IsComplete property to true. May cause IsCancelable property
        /// value to change, which leads to PropertyChanged event referring to
        /// IsCancelable property.
        /// </summary>
        private void SetComplete()
        {
            if (!_isComplete)
            {

                bool isCancelable = IsCancelable;
                _isComplete = true;

                OnPropertyChanged("IsComplete");

                if (isCancelable != IsCancelable)
                    OnPropertyChanged("IsCancelable");

            }
        }

        /// <summary>
        /// Raises PropertyChanged event.
        /// </summary>
        /// <param name="name">Name of the property which was changed.</param>
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Cancels data loading, i.e. signals that loading should be cancelled.
        /// Effective cancellation will occur at first moment convenient to stop
        /// the operation, which might not be immediate and is certainly not
        /// synchronized with call to this method. Calling this method may
        /// raise PropertyChanged event, referring to change of IsCancelable property.
        /// </summary>
        public void Cancel()
        {

            if (!_isCancelling)
            {

                bool isCancelable = IsCancelable;
                _isCancelling = true;

                if (isCancelable != IsCancelable)
                    OnPropertyChanged("IsCancelable");

            }

        }

        /// <summary>
        /// Gets percentage of work completed (in range 0..100 inclusive).
        /// </summary>
        public int Progress
        {
            get { return _progress; }
        }

        /// <summary>
        /// Gets value indicating whether data loading operation
        /// has ben cancelled.
        /// </summary>
        public bool IsCancelled
        {
            get { return _isCancelled; }
        }

        /// <summary>
        /// Gets value indicating whether data loading operation
        /// can be cancelled. Cancellation is allowed before loading
        /// completes and before cancellation has already been requested.
        /// </summary>
        public bool IsCancelable
        {
            get { return !_isCancelling && !_isCancelled && !_isComplete; }
        }

        /// <summary>
        /// Gets value indicating whether loading operation has completed without being cancelled.
        /// </summary>
        public bool IsComplete
        {
            get { return _isComplete; }
        }

        /// <summary> Event raised when public property value changes on this instance.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Current percentual progress of the loading operation.</summary>
        private int _progress;

        /// <summary>True if cancel request has been posted and loading should stop.</summary>
        private bool _isCancelling;

        /// <summary>
        /// True if load operation has been cancelled before completion.
        /// </summary>
        private bool _isCancelled;

        /// <summary>True if load operation has completed (i.e. not cancelled).</summary>
        private bool _isComplete;
    }
}

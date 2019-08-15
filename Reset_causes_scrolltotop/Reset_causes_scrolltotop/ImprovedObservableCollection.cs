using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace Reset_causes_scrolltotop
{
    /// <summary>
    /// if a major change happens at the collection all collectionchangedeventargs are yielded and only 1 reset action is triggered
    /// this insures that the ui is not updaed too often
    /// But a reset event causes a scroll to top in ios and uwp (but not in android)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ImprovedObservableCollection<T> : ObservableCollection<T>
    {
        Subject<NotifyCollectionChangedEventArgs> _collectionChangedBuffer = new Subject<NotifyCollectionChangedEventArgs>();

        public ImprovedObservableCollection()
        {
            if (SynchronizationContext.Current == null)
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            IScheduler mainThreadScheduler = new SynchronizationContextScheduler(SynchronizationContext.Current);
             
            _collectionChangedBuffer.Buffer(TimeSpan.FromMilliseconds(250))
                .ObserveOn(mainThreadScheduler)
                .Subscribe(buffer =>
                {
                    if (buffer.Count == 0)
                        return;
                    else if (buffer.Count == 1)
                        base.OnCollectionChanged(buffer[0]);
                    else
                    {
                        base.OnCollectionChanged(
                            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    }
                });
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            _collectionChangedBuffer.OnNext(e);
        }
    }
}
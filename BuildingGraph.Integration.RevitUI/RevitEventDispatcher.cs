using System;
using System.Collections.Concurrent;
using Autodesk.Revit.UI;

namespace BuildingGraph.Integrations.Revit.UIAddin
{

    public interface IRevitEventDispathcer
    {


    }


    public class RevitEventDispatcher : Autodesk.Revit.UI.IExternalEventHandler
    {
        public static RevitEventDispatcher Current { get; private set; }

        private ConcurrentQueue<Action<UIApplication>> _revitProcessingQueue = new ConcurrentQueue<Action<UIApplication>>();
        private ExternalEvent _extEvent;

        public static void Init()
        {
            if (Current != null) Current._extEvent.Dispose();
            Current = new RevitEventDispatcher();
        }

        public RevitEventDispatcher()
        {
            _extEvent = ExternalEvent.CreateJournalable(this);
        }

        public void Execute(UIApplication app)
        {
            if (Current != null) Current.DoQueuedActions(app);
        }

        public string GetName()
        {
            return "HL Revit action subscriber";
        }


        public void QueueAction(Action<UIApplication> tAct)
        {

            _revitProcessingQueue.Enqueue(tAct);
            _extEvent.Raise();

        }

        private void DoQueuedActions(UIApplication app)
        {
            if (_revitProcessingQueue.Count > 0)
            {
                Action<UIApplication> dAct;
                if (_revitProcessingQueue.TryDequeue(out dAct))
                {
                    try
                    {
                        dAct.Invoke(app);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                lock (_revitProcessingQueue)
                {
                    if (_revitProcessingQueue.Count > 0)
                    {
                        _extEvent.Raise();
                    }
                }
            }

        }
    }
}
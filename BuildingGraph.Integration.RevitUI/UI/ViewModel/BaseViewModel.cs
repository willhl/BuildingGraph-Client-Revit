using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace BuildingGraph.Integration.Revit.UIAddin.ViewModel
{
    class BaseViewModel : INotifyPropertyChanged    
    {

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public virtual void NotifyPropertyChanged(string propertyName)
        {
            //  If the event has been subscribed to, fire it.
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

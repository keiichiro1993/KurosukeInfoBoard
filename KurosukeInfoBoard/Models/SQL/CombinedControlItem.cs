using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KurosukeInfoBoard.Models.SQL
{
    public class CombinedControl
    {
        public CombinedControl() { }
        public CombinedControl(string deviceName, bool isSynchronized, string remoId, string remoName, string hueId, string hueName)
        {
            Id = null;
            Order = null;
            DeviceName = deviceName;
            IsSynchronized = isSynchronized;
            RemoId = remoId;
            RemoName = remoName;
            HueId = hueId;
            HueName = hueName;
            SynchronizedRemoItems = new ObservableCollection<SynchronizedRemoItem>();
        }

        public Int64? Id { get; set; }
        public Int64? Order { get; set; }
        public string DeviceName { get; set; }
        public bool IsSynchronized { get; set; }

        public string RemoId { get; set; }
        public string RemoName { get; set; }

        public string HueId { get; set; }
        public string HueName { get; set; }

        public ObservableCollection<SynchronizedRemoItem> SynchronizedRemoItems { get; set; }
    }

    public class SynchronizedRemoItem
    {
        public string ApplianceId { get; set; }

        public string OnSignalId { get; set; }

        public string OffSignalId { get; set; }
    }
}

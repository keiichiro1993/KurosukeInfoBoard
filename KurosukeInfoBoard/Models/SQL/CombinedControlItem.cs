using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KurosukeInfoBoard.Models.SQL
{
    public class CombinedControl
    {
        public CombinedControl() { }

        public Int64? Id { get; set; }
        public Int64? Order { get; set; }
        public string DeviceName { get; set; }
        public bool IsSynchronized { get; set; }
        public bool IsHueIndivisualControlHidden { get; set; }

        public string RemoId { get; set; }
        public string RemoName { get; set; }

        public string HueId { get; set; }
        public string HueName { get; set; }
    }

    public class SynchronizedRemoItem
    {
        public string ApplianceId { get; set; }

        public string OnSignalId { get; set; }

        public string OffSignalId { get; set; }
    }
}

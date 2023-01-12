using System.Collections.Generic;

namespace KurosukeInfoBoard.Models.SQL
{
    public class HueBridgeCacheEntity
    {
        public HueBridgeCacheEntity() { }
        public HueBridgeCacheEntity(List<HueBridgeCacheItem> bridgeCacheItems)
        {
            HueBridgeCache = bridgeCacheItems;
        }

        public List<HueBridgeCacheItem> HueBridgeCache { get; set; }
    }

    public class HueBridgeCacheItem
    {
        public HueBridgeCacheItem() { }
        public HueBridgeCacheItem(string bridgeId, string ipAddress)
        {
            Id = bridgeId;
            IpAddress = ipAddress;
        }

        public string Id { get; set; }
        public string IpAddress { get; set; }
    }
}

using System.Collections.Generic;

namespace TwitterMood.Management.Moods
{
    public class RootObject
    {
        public List<Status> statuses { get; set; }
        public SearchMetadata search_metadata { get; set; }
    }
}
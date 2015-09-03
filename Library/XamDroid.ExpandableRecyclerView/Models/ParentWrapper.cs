using System;

namespace XamDroid.ExpandableRecyclerView
{
    public class ParentWrapper
    {
        public ParentWrapper(Object parentObject, long stableId)
        {
            ParentObject = parentObject;
            StableId = stableId;
            Expanded = false;
        }

        public Object ParentObject { get; set; }

        public bool Expanded { get; set; }

        public long StableId { get; set; }
    }
}


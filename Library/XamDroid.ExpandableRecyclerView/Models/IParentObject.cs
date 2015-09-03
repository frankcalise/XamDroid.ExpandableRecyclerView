using System;
using System.Collections.Generic;

namespace XamDroid.ExpandableRecyclerView
{
    public interface IParentObject
    {
        List<Object> ChildObjectList { get; set; }
    }
}


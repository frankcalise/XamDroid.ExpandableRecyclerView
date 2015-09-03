using System;

namespace XamDroid.ExpandableRecyclerView
{
    public interface IExpandCollapseListener
    {
        void OnRecyclerViewItemExpanded(int position);

        void OnRecyclerViewItemCollapsed(int position);
    }
}


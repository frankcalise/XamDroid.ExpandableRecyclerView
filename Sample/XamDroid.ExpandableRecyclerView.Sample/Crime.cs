using System;
using System.Collections.Generic;

using Android.Views;
using Android.Content;

using XamDroid.ExpandableRecyclerView;

namespace XamDroid.ExpandableRecyclerView.Sample
{
    public class Crime : IParentObject
	{
        Guid _id;
        List<Object> _childrenList;

        public Crime()
        {
            _id = Guid.NewGuid();
            Date = DateTime.Now;
        }

        public Guid Id { get { return _id; } private set { } }

        public DateTime Date { get; set; }

        public string Title { get; set; }

        public bool Solved { get; set; }

        #region IParentObject implementation
        public List<object> ChildObjectList
        {
            get
            {
                return _childrenList;
            }
            set
            {
                _childrenList = value;
            }
        }
        #endregion
	}

}


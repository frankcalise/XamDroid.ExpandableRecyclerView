using System;
using System.Collections.Generic;

using Android.Views;
using Android.Content;

using XamDroid.ExpandableRecyclerView;

namespace XamDroid.ExpandableRecyclerView.Sample
{
    public class CrimeChild
	{
        public CrimeChild(DateTime date, bool solved)
        {
            Date = date;
            Solved = solved;
        }

        public DateTime Date { get; set; }

        public bool Solved { get; set; }
	}
}


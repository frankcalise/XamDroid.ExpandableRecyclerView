using System;

using XamDroid.ExpandableRecyclerView;
using Android.Widget;
using Android.Views;

namespace XamDroid.ExpandableRecyclerView.Sample
{
	public class CrimeChildViewHolder : ChildViewHolder
	{
        public TextView _crimeDateText;
        public CheckBox _crimeSolvedCheckBox;

        public CrimeChildViewHolder(View itemView) : base(itemView)
        {
            _crimeDateText = itemView.FindViewById<TextView>(Resource.Id.child_list_item_crime_date_text_view);
            _crimeSolvedCheckBox = itemView.FindViewById<CheckBox>(Resource.Id.child_list_item_crime_solved_check_box);
        }

	}

}


using System;

using XamDroid.ExpandableRecyclerView;
using Android.Widget;
using Android.Views;

namespace XamDroid.ExpandableRecyclerView.Sample
{
	public class CrimeParentViewHolder : ParentViewHolder
	{

        public TextView _crimeTitleTextView;
        public ImageButton _parentDropDownArrow;

        public CrimeParentViewHolder(View itemView) : base(itemView)
        {
            _crimeTitleTextView = itemView.FindViewById<TextView>(Resource.Id.parent_list_item_crime_title_text_view);
            _parentDropDownArrow = itemView.FindViewById<ImageButton>(Resource.Id.parent_list_item_expand_arrow);
        }
	}

}


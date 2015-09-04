using System;
using System.Collections.Generic;

using Android.Views;
using Android.Content;
using Android.Support.V7.Widget;

using XamDroid.ExpandableRecyclerView;
using Android.Widget;

namespace XamDroid.ExpandableRecyclerView.Sample
{
    public class CrimeExpandableAdapter : ExpandableRecyclerAdapter<CrimeParentViewHolder, CrimeChildViewHolder>
    {
        LayoutInflater _inflater;

        public CrimeExpandableAdapter(Context context, List<IParentObject> itemList) : base(context, itemList)
        {
            _inflater = LayoutInflater.From(context);
        }

        #region implemented abstract members of ExpandableRecyclerAdapter

        public override CrimeParentViewHolder OnCreateParentViewHolder(ViewGroup parentViewGroup)
        {
            var view = _inflater.Inflate(Resource.Layout.ListItemCrimeParent, parentViewGroup, false);
            return new CrimeParentViewHolder(view);
        }

        public override CrimeChildViewHolder OnCreateChildViewHolder(ViewGroup childViewGroup)
        {
            var view = _inflater.Inflate(Resource.Layout.ListItemCrimeChild, childViewGroup, false);
            return new CrimeChildViewHolder(view);
        }

        public override void OnBindParentViewHolder(CrimeParentViewHolder parentViewHolder, int position, object parentObject)
        {
            var crime = (Crime)parentObject;
            parentViewHolder._crimeTitleTextView.Text = crime.Title;
        }

        public override void OnBindChildViewHolder(CrimeChildViewHolder childViewHolder, int position, object childObject)
        {
            var crimeChild = (CrimeChild)childObject;
            childViewHolder._crimeDateText.Text = crimeChild.Date.ToString();
            childViewHolder._crimeSolvedCheckBox.Checked = crimeChild.Solved;

            childViewHolder._crimeSolvedCheckBox.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) =>
            {
                Console.WriteLine("Child CheckedChanged Position: {0}", position);
            };
        }

        #endregion
    }
}


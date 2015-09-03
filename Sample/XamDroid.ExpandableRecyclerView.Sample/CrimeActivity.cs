using System;

using Android.Support.V4.App;

namespace XamDroid.ExpandableRecyclerView.Sample
{
    public class CrimeActivity : SingleFragmentActivity
    {
        protected override Fragment CreateFragment()
        {
            return new CrimeFragment();
        }
    }
}


using System;

using Android.Support.V4.App;
using Android.App;

namespace XamDroid.ExpandableRecyclerView.Sample
{
    [Activity(Label = "XamDroid.ExpandableRecyclerView.Sample", MainLauncher = true, Icon = "@drawable/icon")]
    public class CrimeListActivity : SingleFragmentActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return new CrimeListFragment();
        }
    }
}


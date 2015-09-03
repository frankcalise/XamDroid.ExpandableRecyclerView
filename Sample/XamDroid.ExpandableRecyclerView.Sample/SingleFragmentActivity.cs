using System;

using Android.OS;
using Android.Support.V4.App;

namespace XamDroid.ExpandableRecyclerView.Sample
{
	public abstract class SingleFragmentActivity : FragmentActivity
	{
        protected abstract Fragment CreateFragment();

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ActivityFragment);

            var fm = this.SupportFragmentManager;
            var fragment = fm.FindFragmentById(Resource.Id.FragmentContainer);

            if (fragment == null)
            {
                fragment = CreateFragment();
                fm.BeginTransaction()
                    .Add(Resource.Id.FragmentContainer, fragment)
                    .Commit();
            }
        }
	}

}


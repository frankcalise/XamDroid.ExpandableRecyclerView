using System;
using System.Collections.Generic;

using Android.Support.V4.App;
using Android.Views;
using Android.OS;
using Android.Support.V7.Widget;

namespace XamDroid.ExpandableRecyclerView.Sample
{
	public class CrimeListFragment : Fragment
	{
        RecyclerView _crimeRecyclerView;

        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FragmentCrimeList, container, false);

            _crimeRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.CrimeRecyclerView);
            _crimeRecyclerView.SetLayoutManager(new LinearLayoutManager(this.Activity));

            var adapter = new CrimeExpandableAdapter(this.Activity, GenerateCrimes());
            adapter.CustomParentAnimationViewId = Resource.Id.parent_list_item_expand_arrow;
            adapter.SetParentClickableViewAnimationDefaultDuration();
            adapter.ParentAndIconExpandOnClick = true;
            adapter.OnRestoreInstanceState(savedInstanceState);

            _crimeRecyclerView.SetAdapter(adapter);

            return view;

        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            ((CrimeExpandableAdapter)_crimeRecyclerView.GetAdapter()).OnSaveInstanceState(outState);
        }

        private List<IParentObject> GenerateCrimes()
        {
            var crimeLab = CrimeLab.Get(this.Activity);
            var crimes = crimeLab.Crimes;
            var parentObjects = new List<IParentObject>();
            foreach (var crime in crimes)
            {
                var childList = new List<Object>();
                childList.Add(new CrimeChild(crime.Date, crime.Solved));
                crime.ChildObjectList = childList;
                parentObjects.Add(crime);
            }

            return parentObjects;
        }
	}

}


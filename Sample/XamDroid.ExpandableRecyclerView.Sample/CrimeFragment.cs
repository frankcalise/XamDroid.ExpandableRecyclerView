using System;

using Android.Support.V4.App;
using Android.Widget;
using Android.Views;
using Android.OS;

namespace XamDroid.ExpandableRecyclerView.Sample
{
	public class CrimeFragment : Fragment
	{

        Crime _crime;
        EditText _titleField;
        Button _dateButton;
        CheckBox _solvedCheckBox;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _crime = new Crime();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.FragmentCrime, container, false);

            _titleField = view.FindViewById<EditText>(Resource.Id.crime_title);
            _titleField.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => 
            {
                _crime.Title = e.Text.ToString();
            };

            _dateButton = view.FindViewById<Button>(Resource.Id.crime_date);
            _dateButton.Text = (_crime.Date.ToString());
            _dateButton.Enabled = false;

            _solvedCheckBox = view.FindViewById<CheckBox>(Resource.Id.crime_solved);
            _solvedCheckBox.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) => 
            {
                _crime.Solved = e.IsChecked;
            };

            return view;
        }
	}

}


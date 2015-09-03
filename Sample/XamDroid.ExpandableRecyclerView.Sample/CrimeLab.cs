using System;
using System.Collections.Generic;
using System.Linq;

using Android.Content;

namespace XamDroid.ExpandableRecyclerView.Sample
{
    public class CrimeLab
    {
        static CrimeLab _crimeLab;
        List<Crime> _crimes;

        public static CrimeLab Get(Context context)
        {
            if (_crimeLab == null)
            {
                _crimeLab = new CrimeLab(context);
            }

            return _crimeLab;
        }

        private CrimeLab(Context contrext)
        {
            _crimes = new List<Crime>();
            for (int i = 0; i < 100; i++)
            {
                var crime = new Crime()
                {
                    Title = string.Format("Crime #{0}", i),
                    Solved = (i % 2 == 0)
                };

                _crimes.Add(crime);
            }
        }

        public Crime GetCrime(Guid id)
        {
            return _crimes.Where(x => x.Id == id).SingleOrDefault();
        }

        public List<Crime> Crimes
        {
            get
            {
                return _crimes;
            }

            private set
            {
                
            }
        }
    }
}


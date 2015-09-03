using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Content;
using Android.OS;
using Newtonsoft.Json;

namespace XamDroid.ExpandableRecyclerView
{
    public abstract class ExpandableRecyclerAdapter<PVH, CVH> : RecyclerView.Adapter, IParentItemClickListener
        where PVH : ParentViewHolder
        where CVH : ChildViewHolder
    {
        const int TypeParent = 0;
        const int TypeChild = 1;
        const string StableIdMap = "ExpandableRecyclerAdapter.StableIdMap";
        const string StableIdList = "ExpandableRecyclerAdapter.StableIdList";

        public const int CustomAnimationViewNotSet = -1;
        public const long DefaultRotateDurationMs = 200;
        public const long CustomAnimationDurationNotSet = -1;

        Dictionary<long, bool> _stableIdMap;
        ExpandableRecyclerAdapterHelper _adapterHelper;
        IExpandCollapseListener _expandCollapseListener;
        bool _parentAndIconClickable = false;
        int _customParentAnimationViewId = CustomAnimationViewNotSet;
        long _animationDuration = CustomAnimationDurationNotSet;

        protected Context _context;
        protected List<Object> _itemList;
        protected List<IParentObject> _parentItemList;

        #region Constructors

        public ExpandableRecyclerAdapter(Context context, List<IParentObject> parentItemList)
            : this(context, parentItemList, CustomAnimationViewNotSet, DefaultRotateDurationMs)
        {

        }

        public ExpandableRecyclerAdapter(Context context, List<IParentObject> parentItemList,
            int customParentAnimationViewId)
            : this(context, parentItemList, customParentAnimationViewId, DefaultRotateDurationMs)
        {
            
        }

        public ExpandableRecyclerAdapter(Context context, List<IParentObject> parentItemList,
            int customParentAnimationViewId, long animationDuration)
        {
            _context = context;
            _parentItemList = parentItemList;
            _itemList = GenerateObjectList(parentItemList);
            _adapterHelper = new ExpandableRecyclerAdapterHelper(_itemList);
            _stableIdMap = GenerateStableIdMapFromList(_adapterHelper.HelperItemList);
            _customParentAnimationViewId = customParentAnimationViewId;
            _animationDuration = animationDuration;
        }

        #endregion

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup viewGroup, int viewType)
        {
            if (viewType == TypeParent)
            {
                var pvh = OnCreateParentViewHolder(viewGroup);
                pvh.ParentItemClickListener = this;

                return pvh;
            }
            else if (viewType == TypeChild)
            {
                return OnCreateChildViewHolder(viewGroup);
            }
            else
            {
                throw new ArgumentException("Invalid ViewType found");
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (_adapterHelper.GetHelperItemAtPosition(position) is ParentWrapper)
            {
                var parentViewHolder = (PVH)holder;

                if (_parentAndIconClickable)
                {
                    if (_customParentAnimationViewId != CustomAnimationViewNotSet &&
                        _animationDuration != CustomAnimationDurationNotSet)
                    {
                        parentViewHolder.SetCustomClickableViewAndItem(_customParentAnimationViewId);
                        parentViewHolder.AnimationDuration = _animationDuration;
                    }
                    else if (_customParentAnimationViewId != CustomAnimationViewNotSet)
                    {
                        parentViewHolder.SetCustomClickableViewAndItem(_customParentAnimationViewId);
                        parentViewHolder.CancelAnimation();
                    }
                    else
                    {
                        parentViewHolder.SetMainItemClickToExpand();
                    }
                }
                else
                {
                    if (_customParentAnimationViewId != CustomAnimationViewNotSet &&
                        _animationDuration != CustomAnimationDurationNotSet)
                    {
                        parentViewHolder.SetCustomClickableViewOnly(_customParentAnimationViewId);
                        parentViewHolder.AnimationDuration = _animationDuration;
                    }
                    else if (_customParentAnimationViewId != CustomAnimationViewNotSet)
                    {
                        parentViewHolder.SetCustomClickableViewOnly(_customParentAnimationViewId);
                        parentViewHolder.CancelAnimation();
                    }
                    else
                    {
                        parentViewHolder.SetMainItemClickToExpand();
                    }
                }

                parentViewHolder.Expanded = ((ParentWrapper)_adapterHelper.GetHelperItemAtPosition(position)).Expanded;
                OnBindParentViewHolder(parentViewHolder, position, _itemList[position]);
            }
            else if (_itemList[position] == null)
            {
                throw new NullReferenceException("Incorrect ViewHolder found");
            }
            else
            {
                OnBindChildViewHolder((CVH)holder, position, _itemList[position]);
            }
        }

        private Dictionary<long, bool> GenerateStableIdMapFromList(List<Object> itemList)
        {
            var parentObjectHashMap = new Dictionary<long, bool>();
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i] != null)
                {
                    var parentWrapper = (ParentWrapper)_adapterHelper.GetHelperItemAtPosition(i);
                    parentObjectHashMap.Add(parentWrapper.StableId, parentWrapper.Expanded);
                }
            }

            return parentObjectHashMap;
        }

        private List<Object> GenerateObjectList(List<IParentObject> parentObjectList)
        {
            var objectList = new List<Object>();
            foreach (var parentObject in parentObjectList)
            {
                objectList.Add(parentObject);
            }

            return objectList;
        }

        public override int ItemCount
        {
            get
            {
                return _itemList.Count;
            }
        }

        public override int GetItemViewType(int position)
        {
            if (_itemList[position] is IParentObject)
            {
                return TypeParent;
            }
            else if (_itemList[position] == null)
            {
                throw new NullReferenceException("Null object added");
            }
            else
            {
                return TypeChild;
            }
        }

        public void SetParentClickableViewAnimationDefaultDuration()
        {
            _animationDuration = DefaultRotateDurationMs;
        }

        public long AnimationDuration
        {
            get { return _animationDuration; }

            set { _animationDuration = value; }
        }

        public int CustomParentAnimationViewId 
        {
            get { return _customParentAnimationViewId; }

            set { _customParentAnimationViewId = value; }
        }

        public bool ParentAndIconExpandOnClick
        {
            get { return _parentAndIconClickable; }

            set { _parentAndIconClickable = value; }
        }

        public void RemoveAnimation()
        {
            _customParentAnimationViewId = CustomAnimationViewNotSet;
            _animationDuration = CustomAnimationDurationNotSet;
        }

        public void AddExpandCollapseListener(IExpandCollapseListener expandCollapseListener)
        {
            _expandCollapseListener = expandCollapseListener;
        }

        private void ExpandParent(IParentObject parentObject, int position)
        {
            var parentWrapper = (ParentWrapper)_adapterHelper.GetHelperItemAtPosition(position);
            if (parentWrapper == null)
            {
                return;
            }

            if (parentWrapper.Expanded)
            {
                parentWrapper.Expanded = false;

                if (_expandCollapseListener != null)
                {
                    var expandedCountBeforePosition = GetExpandedItemCount(position);
                    _expandCollapseListener.OnRecyclerViewItemCollapsed(position - expandedCountBeforePosition);
                }

                // Was Java HashMap put, need to replace the value
                _stableIdMap[parentWrapper.StableId] = false;
                //_stableIdMap.Add(parentWrapper.StableId, false);
                var childObjectList = ((IParentObject)parentWrapper.ParentObject).ChildObjectList;
                if (childObjectList != null)
                {
                    for (int i = childObjectList.Count - 1; i >= 0; i--)
                    {
                        var pos = position + i + 1;
                        _itemList.RemoveAt(pos);
                        _adapterHelper.HelperItemList.RemoveAt(pos);
                        NotifyItemRemoved(pos);
                    }
                }

            }
            else
            {
                parentWrapper.Expanded = true;

                if (_expandCollapseListener != null)
                {
                    var expandedCountBeforePosition = GetExpandedItemCount(position);
                    _expandCollapseListener.OnRecyclerViewItemExpanded(position - expandedCountBeforePosition);
                }

                // Was Java HashMap put, need to replace the value
                _stableIdMap[parentWrapper.StableId] = true;
                //_stableIdMap.Add(parentWrapper.StableId, true);
                var childObjectList = ((IParentObject)parentWrapper.ParentObject).ChildObjectList;
                if (childObjectList != null)
                {
                    for (int i = 0; i < childObjectList.Count; i++)
                    {
                        var pos = position + i + 1;
                        _itemList.Insert(pos, childObjectList[i]);
                        _adapterHelper.HelperItemList.Insert(pos, childObjectList[i]);
                        NotifyItemInserted(pos);
                    }
                }
            }
        }

        private int GetExpandedItemCount(int position)
        {
            if (position == 0)
                return 0;

            var expandedCount = 0;
            for (int i = 0; i < position; i++)
            {
                var obj = _itemList[i];
                if (!(obj is IParentObject))
                    expandedCount++;
            }

            return expandedCount;
        }

        public Bundle OnSaveInstanceState(Bundle savedInstanceStateBundle)
        {
            savedInstanceStateBundle.PutString(StableIdMap, JsonConvert.SerializeObject(_stableIdMap));

            return savedInstanceStateBundle;
        }

        public void OnRestoreInstanceState(Bundle savedInstanceStateBundle)
        {
            if (savedInstanceStateBundle == null)
                return;

            if (!savedInstanceStateBundle.ContainsKey(StableIdMap))
                return;

            _stableIdMap = JsonConvert.DeserializeObject<Dictionary<long, bool>>(savedInstanceStateBundle.GetString(StableIdMap));
            var i = 0;

            while (i < _adapterHelper.HelperItemList.Count)
            {
                if (_adapterHelper.GetHelperItemAtPosition(i) is ParentWrapper)
                {
                    var parentWrapper = (ParentWrapper)_adapterHelper.GetHelperItemAtPosition(i);

                    if (_stableIdMap.ContainsKey(parentWrapper.StableId))
                    {
                        parentWrapper.Expanded = _stableIdMap[parentWrapper.StableId];
                        if (parentWrapper.Expanded)
                        {
                            var childObjectList = ((IParentObject)parentWrapper.ParentObject).ChildObjectList;
                            if (childObjectList != null)
                            {
                                for (int j = 0; j < childObjectList.Count; j++)
                                {
                                    i++;
                                    _itemList.Insert(i, childObjectList[j]);
                                    _adapterHelper.HelperItemList.Insert(i, childObjectList[j]);
                                }
                            }
                        }
                    }
                    else
                    {
                        parentWrapper.Expanded = false;
                    }
                }
                i++;
            }

            NotifyDataSetChanged();
        }

        public abstract PVH OnCreateParentViewHolder(ViewGroup parentViewGroup);

        public abstract CVH OnCreateChildViewHolder(ViewGroup childViewGroup);

        public abstract void OnBindParentViewHolder(PVH parentViewHolder, int position, Object parentObject);

        public abstract void OnBindChildViewHolder(CVH childViewHolder, int position, Object childObject);

        #region IParentItemClickListener implementation
        public void OnParentItemClickListener(int position)
        {
            if (_itemList[position] is IParentObject)
            {
                var parentObject = (IParentObject)_itemList[position];
                ExpandParent(parentObject, position);
            }
        }
        #endregion
    }    
}
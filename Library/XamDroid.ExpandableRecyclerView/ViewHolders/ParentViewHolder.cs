using System;

using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;

namespace XamDroid.ExpandableRecyclerView
{
    public class ParentViewHolder : RecyclerView.ViewHolder, View.IOnClickListener
    {
        const float InitialPosition = 0.0f;
        const float RotatedPosition = 180f;
        const float PivotValue = 0.5f;
        const long DefaultRotateDurationMs = 200;

        View _clickableView;
        bool _isExpanded;
        bool _rotationEnabled;
        long _duration;
        float _rotation;

        public ParentViewHolder(View itemView) : base(itemView)
        {
            _isExpanded = false;
            _duration = DefaultRotateDurationMs;
            _rotation = InitialPosition;
        }

        public void SetCustomClickableViewOnly(int clickableViewId)
        {
            _clickableView = ItemView.FindViewById(clickableViewId);
            ItemView.SetOnClickListener(null);
            _clickableView.SetOnClickListener(this);

            if (_rotationEnabled)
            {
                _clickableView.Rotation = Rotation;
            }
        }

        public void SetCustomClickableViewAndItem(int clickableViewId)
        {
            _clickableView = ItemView.FindViewById(clickableViewId);
            ItemView.SetOnClickListener(this);
            _clickableView.SetOnClickListener(this);
            if (_rotationEnabled)
            {
                _clickableView.Rotation = _rotation;
            }
        }

        public long AnimationDuration
        {
            set
            {
                _rotationEnabled = true;
                _duration = value;
                if (_rotationEnabled)
                {
                    _clickableView.Rotation = _rotation;
                }
            }
        }

        public void CancelAnimation()
        {
            _rotationEnabled = false;
            if (_rotationEnabled)
            {
                _clickableView.Rotation = _rotation;
            }
        }

        public void SetMainItemClickToExpand()
        {
            if (_clickableView != null)
            {
                _clickableView.SetOnClickListener(null);
            }

            ItemView.SetOnClickListener(this);
            _rotationEnabled = false;
        }

        public IParentItemClickListener ParentItemClickListener { get; set; }

        public float Rotation
        {
            get { return _rotation; }

            set
            {
                _rotationEnabled = true;
                _rotation = value;
            }
        }

        public bool Expanded
        {
            get { return _isExpanded; }

            set
            {
                _isExpanded = value;
                if (_rotationEnabled)
                {
                    if (_isExpanded && _clickableView != null)
                    {
                        _clickableView.Rotation = RotatedPosition;
                    }
                    else if (_clickableView != null)
                    {
                        _clickableView.Rotation = InitialPosition;
                    }
                }
            }
        }

        public void OnClick(View v)
        {
            if (ParentItemClickListener != null)
            {
                if (_clickableView != null)
                {
                    if (_rotationEnabled)
                    {
                        var rotateAnimation = new RotateAnimation(RotatedPosition,
                                                  InitialPosition,
                                                  Dimension.RelativeToSelf, PivotValue,
                                                  Dimension.RelativeToSelf, PivotValue);
                                            
                        _rotation = InitialPosition;
                        rotateAnimation.Duration = _duration;
                        rotateAnimation.FillAfter = true;

                        _clickableView.StartAnimation(rotateAnimation);
                    }
                }

                Expanded = !_isExpanded;
                ParentItemClickListener.OnParentItemClickListener(this.LayoutPosition);
            }
                
        }
    }
}


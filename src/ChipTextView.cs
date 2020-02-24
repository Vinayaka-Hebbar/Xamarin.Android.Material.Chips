using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;

namespace Android.Material.Chips
{
    [Register("android/material/chips/ChipTextView")]
    public class ChipTextView : AppCompatTextView
    {
        private bool _isSelected;
        private Color _backgroundColor;
        private Color _selectedBackgroundColor;
        private Color _textColor;
        private Color _selectedTextColor;
        private int _strokeSize;
        private Color _strokeColor;
        private IOnSelectClickListener onSelectClickListener;

        public event EventHandler<SelectEventArgs> SelectClick;

        private bool _selectable;
        private int _cornerRadius;
        public ChipTextView(Context context) : this(context, null)
        {
        }

        public ChipTextView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {
        }

        public ChipTextView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Content.Res.TypedArray a = context.Theme.ObtainStyledAttributes(attrs, Resource.Styleable.ChipTextView, defStyleAttr, 0);
            _selectable = a.GetBoolean(Resource.Styleable.ChipView_chip_selectable, false);
            _backgroundColor = a.GetColor(Resource.Styleable.ChipView_chip_backgroundColor, ContextCompat.GetColor(context, Resource.Color.default_chip_background_color));
            _selectedBackgroundColor = a.GetColor(Resource.Styleable.ChipView_chip_selectedBackgroundColor, ContextCompat.GetColor(context, Resource.Color.default_chip_background_clicked_color));
            _textColor = a.GetColor(Resource.Styleable.ChipView_chip_text_color, ContextCompat.GetColor(context, Resource.Color.default_chip_text_color));
            _selectedTextColor = a.GetColor(Resource.Styleable.ChipView_chip_selected_text_color, ContextCompat.GetColor(context, Resource.Color.default_chip_text_clicked_color));
            _cornerRadius = (int)a.GetDimension(Resource.Styleable.ChipView_chip_cornerRadius, Resources.GetDimension(Resource.Dimension.chip_height) / 2);
            _strokeSize = (int)a.GetDimension(Resource.Styleable.ChipView_chip_strokeSize, 0);
            _strokeColor = a.GetColor(Resource.Styleable.ChipView_chip_strokeColor, ContextCompat.GetColor(context, Resource.Color.default_chip_close_clicked_color));
            a.Recycle();

            InitBackgroundColor();
            SetOnClickListener(new ChipClickListener((view) =>
            {
                SelectChip(view);
            }));
        }

        public void SetOnSelectClickListener(IOnSelectClickListener listener)
        {
            onSelectClickListener = listener;
        }

        private void UpdateTextView()
        {
            SetTextColor(_isSelected ? _selectedTextColor : _textColor);
        }

        private void SelectChip(View view)
        {
            if (!_selectable)
            {
                return;
            }
            _isSelected = !_isSelected;
            InitBackgroundColor();
            UpdateTextView();
            if (onSelectClickListener != null)
            {
                onSelectClickListener.OnselectClick(view, _isSelected);
            }
            SelectClick?.Invoke(this, new SelectEventArgs(view, _isSelected));
        }

        private void InitBackgroundColor()
        {
            GradientDrawable drawable = new GradientDrawable();
            drawable.SetShape(ShapeType.Rectangle);
            drawable.SetCornerRadii(new float[] { CornerRadius, CornerRadius, CornerRadius, CornerRadius, CornerRadius, CornerRadius, CornerRadius, CornerRadius });
            drawable.SetColor(_isSelected ? _selectedBackgroundColor : _backgroundColor);
            drawable.SetStroke(_strokeSize, _strokeColor);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
            {
                Background = drawable;
            }
            else
            {
#pragma warning disable CS0618 // Type or member is obsolete
                SetBackgroundDrawable(drawable);
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        public bool Selectable
        {
            get => _selectable;
            set
            {
                _selectable = value;
                _isSelected = false;
                RequestLayout();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (!_selectable)
                {
                    return;
                }
                _isSelected = value;
                RequestLayout();
            }
        }

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                RequestLayout();
            }
        }

        public Color SelectedBackgroundColor
        {
            get => _selectedBackgroundColor;
            set
            {
                _selectedBackgroundColor = value;
                RequestLayout();
            }
        }

        public Color TextColor
        {
            get => _textColor;
            set
            {
                _textColor = value;
                RequestLayout();
            }
        }

        public Color SelectedTextColor
        {
            get => _selectedTextColor;
            set
            {
                _selectedTextColor = value;
                RequestLayout();
            }
        }

        public int CornerRadius
        {
            get => _cornerRadius;
            set
            {
                _cornerRadius = value;
                RequestLayout();
            }
        }

        public int StrokeSize
        {
            get => _strokeSize;
            set
            {
                _strokeSize = value;
                RequestLayout();
            }
        }

        public Color StrokeColor
        {
            get => _strokeColor;
            set
            {
                _strokeColor = value;
                RequestLayout();
            }
        }
    }
}
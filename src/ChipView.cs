using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;

namespace Android.Material.Chips
{
    [Register("android/material/chips/ChipView")]
    public partial class ChipView : RelativeLayout
    {
        public Bitmap ChipIconBitmap { get; set; }

        private ImageView closeImageView;
        private ImageView selectImageView;
        public TextView ChipTextView { get; private set; }

        private IOnClickListener onCloseClickListener;
        private IOnSelectClickListener onSelectClickListener;
        private IOnClickListener onIconClickListener;
        public event EventHandler<ClickEventArgs> CloseClick;
        public event EventHandler<SelectEventArgs> SelectClick;
        public event EventHandler<ClickEventArgs> IconClick;
        private string _chipText = string.Empty;
        private bool _closable;
        private bool _selectable;
        private bool _isSelected;
        private Color _backgroundColor;
        private Color _selectedBackgroundColor;
        private bool _hasIcon;
        private Drawable _chipIcon;
        private int _strokeSize;
        private Color _strokeColor;
        private Color _selectedTextColor;
        private Color _textColor;
        private Color _closeColor;
        private Color _selectedCloseColor;
        private int _cornerRadius;
        private string _iconText;
        private Color _iconTextColor;
        private Color _iconTextBackgroundColor;
        private Drawable _closeIcon;
        private Drawable _selectIcon;

        public ChipView(Context context) : this(context, null, 0)
        {

        }

        public ChipView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            TypedArray a = context.Theme.ObtainStyledAttributes(attrs, Resource.Styleable.ChipView, defStyleAttr, 0);
            if (a.HasValue(Resource.Styleable.ChipView_chip_text))
            {
                _chipText = a.GetString(Resource.Styleable.ChipView_chip_text);
            }
            _hasIcon = a.GetBoolean(Resource.Styleable.ChipView_enable_icon, false);
            if (a.HasValue(Resource.Styleable.ChipView_chip_icon))
            {
                _chipIcon = a.GetDrawable(Resource.Styleable.ChipView_chip_icon);
            }
            _closable = a.GetBoolean(Resource.Styleable.ChipView_closeable, false);
            _selectable = a.GetBoolean(Resource.Styleable.ChipView_chip_selectable, false);
            _backgroundColor = a.GetColor(Resource.Styleable.ChipView_chip_backgroundColor, ContextCompat.GetColor(context, Resource.Color.default_chip_background_color));
            _selectedBackgroundColor = a.GetColor(Resource.Styleable.ChipView_chip_selectedBackgroundColor, ContextCompat.GetColor(context, Resource.Color.default_chip_background_clicked_color));
            _textColor = a.GetColor(Resource.Styleable.ChipView_chip_text_color, ContextCompat.GetColor(context, Resource.Color.default_chip_text_color));
            _selectedTextColor = a.GetColor(Resource.Styleable.ChipView_chip_selected_text_color, ContextCompat.GetColor(context, Resource.Color.default_chip_text_clicked_color));
            _closeColor = a.GetColor(Resource.Styleable.ChipView_chip_close_color, ContextCompat.GetColor(context, Resource.Color.default_chip_close_inactive_color));
            _selectedCloseColor = a.GetColor(Resource.Styleable.ChipView_chip_selected_close_color, ContextCompat.GetColor(context, Resource.Color.default_chip_close_clicked_color));
            _cornerRadius = (int)a.GetDimension(Resource.Styleable.ChipView_chip_cornerRadius, Resources.GetDimension(Resource.Dimension.chip_height) / 2);
            _strokeSize = (int)a.GetDimension(Resource.Styleable.ChipView_chip_strokeSize, 0);
            _strokeColor = a.GetColor(Resource.Styleable.ChipView_chip_strokeColor, ContextCompat.GetColor(context, Resource.Color.default_chip_close_clicked_color));
            if (a.HasValue(Resource.Styleable.ChipView_chip_iconText))
            {
                _iconText = a.GetString(Resource.Styleable.ChipView_chip_iconText);
            }
            _iconTextColor = a.GetColor(Resource.Styleable.ChipView_chip_iconTextColor, ContextCompat.GetColor(context, Resource.Color.default_chip_background_clicked_color));
            _iconTextBackgroundColor = a.GetColor(Resource.Styleable.ChipView_chip_iconTextBackgroundColor, ContextCompat.GetColor(context, Resource.Color.default_chip_close_clicked_color));
            a.Recycle();
            InitalizeViews(context);
        }

        private void InitSelectClick()
        {
            selectImageView.SetOnClickListener(new ChipClickListener((view) =>
            {
                SelectChip(view);
            }));
        }

        private void InitCloseClick()
        {
            closeImageView.SetOnTouchListener(new ChipCloseTouchListener((v, e) =>
            {
                switch (e.Action)
                {
                    case MotionEventActions.Down:
                    case MotionEventActions.PointerDown:
                        CloseChip(v, true);
                        return true;
                    case MotionEventActions.Up:
                    case MotionEventActions.PointerUp:
                        CloseChip(v, false);
                        return true;
                }
                return false;
            }));
        }

        private void InitSelectImage()
        {
            if (!_selectable)
            {
                return;
            }
            var lParams = new LayoutParams((int)Resources.GetDimension(Resource.Dimension.chip_close_icon_size_medium), (int)Resources.GetDimension(Resource.Dimension.chip_close_icon_size_medium));
            lParams.AddRule(Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean ? LayoutRules.EndOf : LayoutRules.RightOf, ChipUtils.TEXT_ID);
            lParams.AddRule(LayoutRules.CenterVertical);
            lParams.SetMargins((int)Resources.GetDimension(Resource.Dimension.chip_close_horizontal_margin),
                0,
                (int)Resources.GetDimension(Resource.Dimension.chip_close_horizontal_margin), 0);
            selectImageView.LayoutParameters = lParams;
            selectImageView.SetScaleType(ImageView.ScaleType.Center);
            selectImageView.SetImageDrawable(_selectIcon);
            ChipUtils.SetIconColor(selectImageView, _closeColor);
            InitSelectClick();
            AddView(selectImageView);
        }

        private void InitCloseImage()
        {
            if (!_closable)
            {
                return;
            }
            var lParams = new LayoutParams((int)Resources.GetDimension(Resource.Dimension.chip_close_icon_size_medium), (int)Resources.GetDimension(Resource.Dimension.chip_close_icon_size_medium));
            lParams.AddRule(Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean ? LayoutRules.EndOf : LayoutRules.RightOf, ChipUtils.TEXT_ID);
            lParams.AddRule(LayoutRules.CenterVertical);
            lParams.SetMargins((int)Resources.GetDimension(Resource.Dimension.chip_close_horizontal_margin), 0,
                (int)Resources.GetDimension(Resource.Dimension.chip_close_horizontal_margin), 0);
            closeImageView.LayoutParameters = lParams;
            closeImageView.SetScaleType(ImageView.ScaleType.Center);
            closeImageView.SetImageDrawable(_closeIcon);
            ChipUtils.SetIconColor(closeImageView, _closeColor);
            InitCloseClick();
            AddView(closeImageView);
        }

        private void InitImageIcon()
        {
            if (!_hasIcon)
            {
                return;
            }
            ImageView icon = new ImageView(Context);
            var lParams = new LayoutParams((int)Resources.GetDimension(Resource.Dimension.chip_height), (int)Resources.GetDimension(Resource.Dimension.chip_height));
            lParams.AddRule(Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr1 ? LayoutRules.AlignParentStart : LayoutRules.AlignParentLeft);
            icon.LayoutParameters = lParams;
            icon.SetScaleType(ImageView.ScaleType.FitCenter);
            icon.Id = ChipUtils.IMAGE_ID;
            if (_chipIcon != null)
            {
                if (_chipIcon is BitmapDrawable bitmapDrawable && bitmapDrawable.Bitmap != null)
                {
                    Bitmap bitmap = ((BitmapDrawable)_chipIcon).Bitmap;
                    bitmap = ChipUtils.GetSquareBitmap(bitmap);
                    bitmap = ChipUtils.GetScaledBitmap(Context, bitmap);
                    icon.SetImageBitmap(ChipUtils.GetCircleBitmap(Context, bitmap));
                }
                else
                {
                    icon.SetImageDrawable(_chipIcon);
                }
            }
            if (ChipIconBitmap != null)
            {
                ChipIconBitmap = ChipUtils.GetSquareBitmap(ChipIconBitmap);
                icon.SetImageBitmap(ChipUtils.GetCircleBitmap(Context, ChipIconBitmap));
                icon.BringToFront();
            }
            if (!string.IsNullOrEmpty(_iconText))
            {
                Bitmap textBitmap = ChipUtils.GetCircleBitmapWithText(Context, _iconText, _iconTextColor, _iconTextBackgroundColor);
                icon.SetImageBitmap(textBitmap);
            }
            icon.SetOnClickListener(new ChipClickListener((view) =>
            {
                if (onIconClickListener != null)
                {
                    onIconClickListener.OnClick(view);
                }

                IconClick?.Invoke(this, new ClickEventArgs(view));
            }));
        }

        private void CloseChip(View v, bool v2)
        {
            _isSelected = !_isSelected;
            InitBackgroundColor();
            UpdateTextView();
            closeImageView.SetImageDrawable(_closeIcon);
            ChipUtils.SetIconColor(closeImageView, _isSelected ? _selectedCloseColor : _closeColor);
            if (onCloseClickListener != null)
            {
                onCloseClickListener.OnClick(v);
            }
            CloseClick?.Invoke(this, new ClickEventArgs(v));
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
            selectImageView.SetImageDrawable(_selectIcon);
            ChipUtils.SetIconColor(selectImageView, _isSelected ? _selectedCloseColor : _closeColor);
            if (onSelectClickListener != null)
            {
                onSelectClickListener.OnselectClick(view, _isSelected);
            }
            SelectClick?.Invoke(this, new SelectEventArgs(view, _isSelected));

        }

        private void UpdateTextView()
        {
            var lParams = (RelativeLayout.LayoutParams)ChipTextView.LayoutParameters;
            if (_hasIcon || _closable || _selectable)
            {
                lParams.AddRule(Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean ? LayoutRules.EndOf : LayoutRules.RightOf, ChipUtils.IMAGE_ID);
                lParams.AddRule(LayoutRules.CenterVertical);
            }
            else
            {
                lParams.AddRule(LayoutRules.CenterInParent);
            }
            int startMargin = _hasIcon ? (int)Resources.GetDimension(Resource.Dimension.chip_icon_horizontal_margin) : (int)Resources.GetDimension(Resource.Dimension.chip_horizontal_padding);
            int endMargin = _closable || _selectable ? 0 : (int)Resources.GetDimension(Resource.Dimension.chip_horizontal_padding);

            ChipTextView.SetTextColor(_isSelected ? _selectedTextColor : _textColor);
            ChipTextView.LayoutParameters = lParams;
        }

        private void InitTextView()
        {
            if (!ViewCompat.IsAttachedToWindow(this))
            {
                return;
            }
            if (ChipTextView == null)
            {
                ChipTextView = new TextView(Context);
            }
            ChipTextView.LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            UpdateTextView();
            ChipTextView.Text = _chipText;
            ChipTextView.Id = ChipUtils.TEXT_ID;
            AddView(ChipTextView);

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

        protected void InitalizeViews(Context context)
        {
            selectImageView = new ImageView(context);
            closeImageView = new ImageView(context);
            ChipTextView = new TextView(context);
        }

        public ChipView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {

        }

        protected void BuildView()
        {
            InitCloseImage();
            InitSelectImage();
            InitBackgroundColor();
            InitTextView();
            InitImageIcon();
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            BuildView();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            ViewGroup.LayoutParams lParams = LayoutParameters;
            lParams.Width = ViewGroup.LayoutParams.WrapContent;
            lParams.Height = (int)Resources.GetDimension(Resource.Dimension.chip_height);
            LayoutParameters = lParams;
        }

        public void SetOnCloseClickListener(IOnClickListener listener)
        {
            onCloseClickListener = listener;
        }

        public void SetOnSelectClickListener(IOnSelectClickListener listener)
        {
            onSelectClickListener = listener;
        }

        public void SetOnIconClickListener(IOnClickListener listener)
        {
            onIconClickListener = listener;
        }

        public bool HasIcon
        {
            get => _hasIcon; set
            {
                _hasIcon = value;
                RequestLayout();
            }
        }

        public Drawable ChipIcon
        {
            get => _chipIcon;
            set
            {
                _chipIcon = value;
                RequestLayout();
            }
        }

        public string ChipText
        {
            get => _chipText;
            set
            {
                _chipText = value;
                RequestLayout();
            }
        }

        public bool Closable
        {
            get => _closable;
            set
            {
                _closable = value;
                _selectable = false;
                _isSelected = false;
            }
        }

        public bool Selectable
        {
            get => _selectable;
            set
            {
                _selectable = value;
                _closable = false;
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


        public Color CloseColor
        {
            get => _closeColor;
            set
            {
                _closeColor = value;
                RequestLayout();
            }
        }
        public Color SelectedCloseColor
        {
            get => _selectedCloseColor;
            set
            {
                _selectedCloseColor = value;
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

        public string IconText
        {
            get => _iconText;
            set
            {
                _iconText = value;
                RequestLayout();
            }
        }
        public Color IconTextColor
        {
            get => _iconTextColor;
            set
            {
                _iconTextColor = value;
                RequestLayout();
            }
        }
        public Color IconTextBackgroundColor
        {
            get => _iconTextBackgroundColor;
            set
            {
                _iconTextBackgroundColor = value;
                RequestLayout();
            }
        }

        public Drawable SelectIcon
        {
            get => _selectIcon;
            set
            {
                _selectIcon = value;
                if (!_isSelected)
                {
                    selectImageView.SetImageDrawable(value);
                }
            }
        }

        public Drawable CloseIcon
        {
            get => _closeIcon;
            set
            {
                _closeIcon = value;
                if (_isSelected)
                {
                    closeImageView.SetImageDrawable(value);
                }
            }
        }

        private class ChipCloseTouchListener : Java.Lang.Object, IOnTouchListener
        {
            private readonly Func<View, MotionEvent, bool> action;

            public ChipCloseTouchListener(Func<View, MotionEvent, bool> action)
            {
                this.action = action;
            }

            public bool OnTouch(View v, MotionEvent e)
            {
                return action(v, e);
            }
        }
    }
}
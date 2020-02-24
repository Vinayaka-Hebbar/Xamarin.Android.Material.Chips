using Android.Views;
using System;

namespace Android.Material.Chips
{
    public class SelectEventArgs : EventArgs
    {
        public SelectEventArgs(View view, bool isSelected)
        {
            View = view;
            IsSelected = isSelected;
        }

        public View View { get; set; }
        public bool IsSelected { get; set; }
    }
}
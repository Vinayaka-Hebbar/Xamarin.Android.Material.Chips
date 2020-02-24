using Android.Views;
using System;
using static Android.Views.View;

namespace Android.Material.Chips
{
    public class ChipClickListener : Java.Lang.Object, IOnClickListener
    {
        private readonly Action<View> action;

        public ChipClickListener(Action<View> action)
        {
            this.action = action;
        }

        public void OnClick(View v)
        {
            action(v);
        }
    }
}
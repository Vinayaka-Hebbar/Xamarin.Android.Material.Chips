using Android.Views;

namespace Android.Material.Chips
{
    public class ClickEventArgs : System.EventArgs
    {
        public ClickEventArgs(View view)
        {
            View = view;
        }

        public View View { get; }
    }
}
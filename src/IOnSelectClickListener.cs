using Android.Views;

namespace Android.Material.Chips
{
    public interface IOnSelectClickListener : Runtime.IJavaObject
    {
        void OnselectClick(View view, bool isSelected);
    }
}
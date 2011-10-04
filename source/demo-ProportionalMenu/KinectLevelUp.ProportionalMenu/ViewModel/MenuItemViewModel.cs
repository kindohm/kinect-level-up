using System.Windows.Media;
using GalaSoft.MvvmLight;

namespace KinectLevelUp.ProportionalMenu.ViewModel
{
    public class MenuItemViewModel : ViewModelBase
    {
        static readonly SolidColorBrush DefaultBrush = new SolidColorBrush(Colors.Blue);
        static readonly SolidColorBrush HighlightBrush = new SolidColorBrush(Colors.LawnGreen);

        public MenuItemViewModel()
        {
            this.ItemBrush = MenuItemViewModel.DefaultBrush;
        }

        public const string IsSelectedPropertyName = "IsSelected";
        bool isSelected = false;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (isSelected == value)
                {
                    return;
                }
                var oldValue = isSelected;
                isSelected = value;
                RaisePropertyChanged(IsSelectedPropertyName);
                this.ItemBrush = isSelected ? HighlightBrush : DefaultBrush;
            }
        }

        public const string ItemBrushPropertyName = "ItemBrush";
        SolidColorBrush itemBrush = null;
        public SolidColorBrush ItemBrush
        {
            get
            {
                return itemBrush;
            }
            set
            {
                if (itemBrush == value)
                {
                    return;
                }
                var oldValue = itemBrush;
                itemBrush = value;
                RaisePropertyChanged(ItemBrushPropertyName);
            }
        }

        public const string NamePropertyName = "Name";
        string name = string.Empty;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name == value)
                {
                    return;
                }
                var oldValue = name;
                name = value;
                RaisePropertyChanged(NamePropertyName);
            }
        }
    }
}

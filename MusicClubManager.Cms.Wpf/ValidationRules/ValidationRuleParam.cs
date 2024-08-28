using System.Windows;

namespace MusicClubManager.Cms.Wpf.ValidationRules
{
    public class ValidationRuleParam : DependencyObject
    {
        public static readonly DependencyProperty TotalPagesProperty =
        DependencyProperty.Register(
        name: nameof(TotalPages),
        propertyType: typeof(int),
        ownerType: typeof(ValidationRuleParam),
        typeMetadata: new FrameworkPropertyMetadata(defaultValue: 0));

        public int TotalPages
        {
            get => (int)GetValue(TotalPagesProperty);
            set => SetValue(TotalPagesProperty, value);
        }
    }
}

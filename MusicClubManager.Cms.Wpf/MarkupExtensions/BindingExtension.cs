using System.Windows.Markup;
using System.Windows;

using MusicClubManager.Cms.Wpf.ViewModels;

namespace MusicClubManager.Cms.Wpf.MarkupExtensions
{
    public class BindingExtension : MarkupExtension
    {
        public string Path { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provideValueTarget = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            var targetObject = provideValueTarget.TargetObject as FrameworkElement;            

            if (targetObject != null && !string.IsNullOrEmpty(Path))
            {
                var binding = new System.Windows.Data.Binding(Path)
                {
                    Source = targetObject.DataContext
                };

                return binding.ProvideValue(serviceProvider);
            }

            return null;
        }
    }
}

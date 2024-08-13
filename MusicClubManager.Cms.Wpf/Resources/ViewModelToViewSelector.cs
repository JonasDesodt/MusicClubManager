using Microsoft.Extensions.DependencyInjection;
using MusicClubManager.Cms.Wpf.ViewModels;
using MusicClubManager.Cms.Wpf.Views;
using MusicClubManager.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace MusicClubManager.Cms.Wpf.Resources
{
    public class ViewModelToViewSelector(IServiceProvider serviceProvider) : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is null)
            {
                return base.SelectTemplate(item, container);
            }

            //if (item is not Type viewModelType)
            //{
            //    return base.SelectTemplate(item, container);
            //}

            //var viewType = viewModelType.Name switch
            //{
            //    "AgendaViewModel" => typeof(AgendaView),
            //    _ => null
            //};

            var viewType = item switch
            {
                AgendaViewModel => typeof(AgendaView),
                // Add other ViewModel to View mappings here
                _ => null
            };

            if (viewType != null)
            {
                (item as AgendaViewModel).PerformanceApiService = serviceProvider.GetRequiredService<IPerformanceService>();

                var view = serviceProvider.GetRequiredService(viewType) as FrameworkElement;
                return new DataTemplate { VisualTree = new FrameworkElementFactory(view?.GetType())};
            }

            //var viewModel = serviceProvider.GetRequiredService(viewModelType);

            return base.SelectTemplate(item, container);
        }
    }
}

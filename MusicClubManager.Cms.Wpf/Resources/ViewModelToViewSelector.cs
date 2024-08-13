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

            //if (item is AgendaViewModel agendaViewModel)
            //{
            //    agendaViewModel.PerformanceApiService = serviceProvider.GetRequiredService<IPerformanceService>();
            //}

            //if(item is not Type viewModelType)
            //{
            //    return base.SelectTemplate(item, container);
            //}

            //item = serviceProvider.GetRequiredService(viewModelType);

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
                var view = serviceProvider.GetRequiredService(viewType) as FrameworkElement;
                return new DataTemplate { VisualTree = new FrameworkElementFactory(view?.GetType())};
            }

            return base.SelectTemplate(item, container);
        }
    }
}

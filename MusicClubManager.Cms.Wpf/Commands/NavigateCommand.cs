﻿using MusicClubManager.Cms.Wpf.Interfaces;
using MusicClubManager.Cms.Wpf.ViewModels;
using System.Windows.Input;

namespace MusicClubManager.Cms.Wpf.Commands
{
    public class NavigateCommand(INavigate viewModel) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if(parameter is string route)
            {
                viewModel.Navigate(route);
            }       
        }
    }
}

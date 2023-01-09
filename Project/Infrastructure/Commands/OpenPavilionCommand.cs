﻿using Project.Infrastructure.Commands.Base;
using Project.Models;
using Project.Views.Pages;

namespace Project.Infrastructure.Commands
{
    internal class OpenPavilionCommand : Command
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => Singleton.Instance.Navigate(new PavilionPage());
    }
}

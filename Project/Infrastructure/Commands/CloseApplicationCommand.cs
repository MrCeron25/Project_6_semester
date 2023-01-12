using Project.Infrastructure.Commands.Base;
using System.Windows;

namespace Project.Infrastructure.Commands
{
    internal class CloseApplicationCommand : Command
    {
        public override void Execute(object parameter) => Application.Current.Shutdown();
        public override bool CanExecute(object parameter) => true;
    }
}

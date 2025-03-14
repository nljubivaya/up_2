using Avalonia.Controls;
using ReactiveUI;
using UP2.Models;

namespace UP2.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static MainWindowViewModel Instance;

        public static PodgotovkaContext myConnection = new PodgotovkaContext();
        public MainWindowViewModel()
        {
            Instance = this;

        }
        UserControl _pageContent = new Avtorization();

        public UserControl PageContent
        {
            get => _pageContent;
            set => this.RaiseAndSetIfChanged(ref _pageContent, value);
        }
    }
}

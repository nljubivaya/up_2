using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using UP2.ViewModels;

namespace UP2;

public partial class ShowOrder : UserControl
{
    public ShowOrder()
    {
        InitializeComponent();
        DataContext = new ShowOrderVM();
    }
}
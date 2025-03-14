using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using UP2.ViewModels;

namespace UP2;

public partial class ShowProduct : UserControl
{
    public ShowProduct()
    {
        InitializeComponent();
        DataContext = new ShowProductVM();
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using UP2.ViewModels;
using WriteAndErase.ViewModels;

namespace WriteAndErase;

public partial class AddProduct : UserControl
{
    public AddProduct()
    {
        InitializeComponent();
        DataContext = new AddProductVM();
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Text;
using UP2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Microsoft.EntityFrameworkCore;
using Avalonia.Media.Imaging;
using Avalonia.Media.TextFormatting;
using Avalonia.Media;
using Avalonia;
using System.Diagnostics.Metrics;
using Tmds.DBus.Protocol;

namespace UP2;

public partial class Avtorization : UserControl
{
    public Avtorization()
    {
        InitializeComponent();
        DataContext = new AvtorizationVM();
    }
}
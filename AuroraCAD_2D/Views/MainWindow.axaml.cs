using AuroraCAD_2D.Database;
using AuroraCAD_2D.ViewModels;
using Avalonia.Controls;

namespace AuroraCAD_2D.Views;

public partial class MainWindow : Window{
    public MainWindow(){
        InitializeComponent();
        WindowState = WindowState.Maximized;
    }
}
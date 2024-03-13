using System;
using System.Reactive;
using AuroraCAD_2D.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace AuroraCAD_2D.Views;

public partial class RibbonView : UserControl{
    private RibbonViewModel _ribbonViewModel = new RibbonViewModel();
    
    public RibbonView(){
        InitializeComponent();
        // HotKeyManager.SetHotKey(DrawPointButton, new KeyGesture(Key.D, KeyModifiers.Control));
        HotKeyManager.SetHotKey(DrawLineButton, new KeyGesture(Key.F, KeyModifiers.Control));
        HotKeyManager.SetHotKey(DrawCircleButton, new KeyGesture(Key.G, KeyModifiers.Control));
        HotKeyManager.SetHotKey(EscapeKeyHolder, new KeyGesture(Key.Escape));
       
    }
    
    

    private void cancellAllFlags(object? sender, RoutedEventArgs e){
        _ribbonViewModel.ClearAllFlags();
    }
    
    private void DrawPointFlag(object? sender, RoutedEventArgs e){
        _ribbonViewModel.DrawPointFlag();
    }

    private void DrawLineFlag(object? sender, RoutedEventArgs e){
        _ribbonViewModel.DrawLineFlag();
    }

    private void DrawCircleFlag(object? sender, RoutedEventArgs e){
        _ribbonViewModel.DrawCircleFlag();
    }
}
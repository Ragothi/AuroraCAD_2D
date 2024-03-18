using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reactive;
using AuroraCAD_2D.Database;
using AuroraCAD_2D.Services;
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
    private List<Button> _buttons = new List<Button>();
    
    public RibbonView(){
        InitializeComponent();
        // HotKeyManager.SetHotKey(DrawPointButton, new KeyGesture(Key.D, KeyModifiers.Control));
        HotKeyManager.SetHotKey(DrawLineButton, new KeyGesture(Key.F, KeyModifiers.Control));
        HotKeyManager.SetHotKey(DrawCircleButton, new KeyGesture(Key.G, KeyModifiers.Control));
        HotKeyManager.SetHotKey(EscapeKeyHolder, new KeyGesture(Key.Escape));
       _buttons.Add(DrawPointButton);
       _buttons.Add(DrawLineButton);
       _buttons.Add(DrawCircleButton);
       removeButtonsColor();
       
    }

    private void removeButtonsColor(){
        foreach (Button button in _buttons){
            button.Background = Avalonia.Media.Brush.Parse(Color.Gray.Name);
        }
    }

    private void ESCButtonEvent(object? sender, RoutedEventArgs e){
        
        if (Settings.selectedPoint != null && Settings.isDrawXXXSelected[1]){
            Settings.CanvasGlobalReference.Children.Remove(Settings.selectedLine);
            if (Settings.isNewPointAdded){
                Settings.CanvasGlobalReference.Children.Remove(Settings.selectedPoint);
            }
            Settings.isNewPointAdded = true;
            Settings.selectedPoint = null;
            Settings.selectedLine = null;
        } else if (Settings.selectedPoint != null && Settings.isDrawXXXSelected[2]){
            Settings.CanvasGlobalReference.Children.Remove(Settings.selectedCircle);
            if (Settings.isNewPointAdded){
                Settings.CanvasGlobalReference.Children.Remove(Settings.selectedPoint);
            }
            Settings.isNewPointAdded = true;
            Settings.selectedPoint = null;
            Settings.selectedCircle = null;
        }
        else{
            _ribbonViewModel.ClearAllFlags();
            removeButtonsColor();
        }
    }

    private void highlight(Button button, bool flag){
        removeButtonsColor();
        if (flag){
            button.Background = Avalonia.Media.Brush.Parse(Color.YellowGreen.Name);
        }
        else{
            button.Background = Avalonia.Media.Brush.Parse(Color.Gray.Name);
        }
    }
    
    private void DrawPointFlag(object? sender, RoutedEventArgs e){
        _ribbonViewModel.DrawPointFlag();
        highlight(DrawPointButton,Settings.isDrawXXXSelected[0]);
    }

    private void DrawLineFlag(object? sender, RoutedEventArgs e){
        _ribbonViewModel.DrawLineFlag();
        highlight(DrawLineButton,Settings.isDrawXXXSelected[1]);
    }

    private void DrawCircleFlag(object? sender, RoutedEventArgs e){
        _ribbonViewModel.DrawCircleFlag();
        highlight(DrawCircleButton,Settings.isDrawXXXSelected[2]);
    }
    
   
}
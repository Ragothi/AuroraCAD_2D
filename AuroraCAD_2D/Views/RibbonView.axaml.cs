using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reactive;
using AuroraCAD_2D.Database;
using AuroraCAD_2D.Models;
using AuroraCAD_2D.Services;
using AuroraCAD_2D.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using ReactiveUI;
using Brush = Avalonia.Media.Brush;
using Point = AuroraCAD_2D.Models.Point;

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


    private async void SaveProjectButtonOnClick(object? sender, RoutedEventArgs e){
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            SuggestedStartLocation = await topLevel.StorageProvider.TryGetFolderFromPathAsync(Settings.assetsFolder),
            Title = "Save Project"
        });

        if (file is not null)
        {
            Logger.log(file.Path.AbsolutePath);
            Database.Database.save(file.Path.AbsolutePath);
        }
    }

    private async void LoadProjectButtonOnClick(object? sender, RoutedEventArgs e){
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            SuggestedStartLocation = await topLevel.StorageProvider.TryGetFolderFromPathAsync(Settings.assetsFolder),
            Title = "Load Project File",
            AllowMultiple = false
        });

        if (files.Count == 1)
        {
            //Clear current screen content
            ESCButtonEvent(null,null);
            ESCButtonEvent(null,null);
            Settings.CanvasGlobalReference.Children.Clear();
            Settings.TreeViewGlobalReference.clearLayers();
            
            //clear current data from database
            Database.Database.emptyDatabase();
            
            //Load new data into database
            Database.Database.read(files[0].Path.AbsolutePath);
            
            //load layers onto TreeView
            foreach (Layer layer in Database.Database.Layers){
                Settings.TreeViewGlobalReference.addLayer(layer);
            }
            TreeViewItem item = Settings.TreeViewGlobalReference.RootsList[0].ChildrenList[0];
            (item.NameTB.Parent as StackPanel).Background = Brush.Parse(Color.YellowGreen.Name);
            Settings.selectedLayer = Database.Database.Layers[0];
            

            //Load points and lines onto Canvas
            foreach (Point point in Database.Database.Points){
                Settings.CanvasViewGlobalReference.drawNew(point,true);
            }

            foreach (Drawable line in Database.Database.Lines){
                Settings.CanvasViewGlobalReference.drawNew(line,true);
            }
            
            Database.Database.printEntitiesAmount();
        }
    }
}
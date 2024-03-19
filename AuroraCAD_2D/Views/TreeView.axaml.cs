using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AuroraCAD_2D.Database;
using AuroraCAD_2D.Models;
using AuroraCAD_2D.Services;
using AuroraCAD_2D.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using DynamicData;
using Color = System.Drawing.Color;
using Point = AuroraCAD_2D.Models.Point;

namespace AuroraCAD_2D.Views;

public partial class TreeView : UserControl{
    private TreeViewViewmodel _treeViewViewmodel;
    private List<TreeViewItem> rootsList = new List<TreeViewItem>();
    public TreeView(){
        InitializeComponent();
        _treeViewViewmodel = new TreeViewViewmodel();
        TreeViewItem layersRoot = new TreeViewItem("Layers", Settings.assetsFolder + "layer_icon.png", true);
        rootsList.Add(layersRoot);
        
        addLayer(Layer.defaultLayer);
        (rootsList[0].Children[0].NameTB.Parent as StackPanel).Background = Brush.Parse(Color.YellowGreen.Name);
        
        Layer rLayer = new Layer(Brushes.Red, 6,4,"Red Layer");
        addLayer(rLayer);
        

        foreach (TreeViewItem treeRoot in rootsList){
            TreeViewRoot.Children.Add(treeRoot);
            foreach (TreeViewItem c in treeRoot.Children){
                TreeViewRoot.Children.Add(c);
            }
        }
        


    }

    private void addLayer(Layer layer){
        TreeViewItem newLayer = new TreeViewItem(layer.Name, false,layer.Color);
        newLayer.NameTB.PointerPressed += LayerNamePressed;
        newLayer.IconImage.PointerPressed += LayerColorPressed;
        Image image = new Image();
        image.Source = new Bitmap(Settings.assetsFolder + "visible.png");
        image.PointerPressed += changeLayerVisibility;
        image.Width = 30;
        image.Height = 30;
        (newLayer.NameTB.Parent as StackPanel).Children.Add(image);
        
        rootsList[0].addItem(newLayer);
    }

    private void LayerNamePressed(object? sender, PointerPressedEventArgs e){
        TreeViewItem currentLayerTVI = rootsList[0].Children[Database.Database.Layers.IndexOf(Settings.selectedLayer)];
        (currentLayerTVI.NameTB.Parent as StackPanel).Background = Brush.Parse(Color.Black.Name);
        TreeViewItem item = ((sender as TextBlock).Parent as StackPanel).Parent as TreeViewItem;
        Settings.selectedLayer = Database.Database.Layers[rootsList[0].Children.IndexOf(item)];
        (item.NameTB.Parent as StackPanel).Background = Brush.Parse(Color.YellowGreen.Name);
    }

    private void changeLayerVisibility(object? sender, PointerPressedEventArgs e){
        TreeViewItem item = (((sender as Image).Parent as StackPanel).Parent) as TreeViewItem;
        Layer layer = Database.Database.Layers[rootsList[0].Children.IndexOf(item)];
        bool flag = !layer.IsVisible;
        if (flag){
            (sender as Image).Source = new Bitmap(Settings.assetsFolder + "visible.png");
        }
        else{
            (sender as Image).Source = new Bitmap(Settings.assetsFolder + "hidden.png");
        }
        foreach (Point point in Database.Database.Points){
            if (point.getLayer().Equals(layer)){
                point.IsVisible =  flag;
            }
        }
        foreach (Drawable line in Database.Database.Lines){
            if (line.getLayer().Equals(layer)){
                switch (line.getType()){
                    case Drawable.DrawableType.LINE:
                        (line as Line).IsVisible =  flag;
                        break;
                    case Drawable.DrawableType.CIRCLE:
                        (line as Circle).IsVisible =  flag;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        layer.IsVisible = flag;
    }
    
    private void LayerColorPressed(object? sender, PointerPressedEventArgs e){
        
        /**
        TreeViewItem item = ((sender as Image).Parent as StackPanel).Parent as TreeViewItem;
        ColorPicker cp = new ColorPicker();
        cp.Background = Brush.Parse(Color.Cyan.Name);
        cp.Color = Avalonia.Media.Color.Parse("Cyan");
        cp.Width = 200;
        cp.Height = 200;
        
        Settings.CanvasGlobalReference.Children.Add(cp);
        
        Logger.log("LOL");
        */
    }
}
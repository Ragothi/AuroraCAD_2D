using System.IO;
using System.Reflection;
using AuroraCAD_2D.Database;
using AuroraCAD_2D.Models;
using AuroraCAD_2D.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using Color = System.Drawing.Color;

namespace AuroraCAD_2D.Views;

public partial class TreeView : UserControl{
    private TreeViewViewmodel _treeViewViewmodel;
    public TreeView(){
        InitializeComponent();
        _treeViewViewmodel = new TreeViewViewmodel();
        TreeViewItem layersRoot = new TreeViewItem("Layers", Settings.assetsFolder + "layer_icon.png", true);
        
        TreeViewItem defaultLayer = new TreeViewItem(Layer.defaultLayer.Name, false,Layer.defaultLayer.Color);
        layersRoot.addItem(defaultLayer);
        (defaultLayer.NameTB.Parent as StackPanel).Background = Brush.Parse(Color.YellowGreen.Name);

        Layer rLayer = new Layer(Brushes.Red, 6,4,"Red Layer");
        TreeViewItem redLayer = new TreeViewItem(rLayer.Name, false,rLayer.Color);
        layersRoot.addItem(redLayer);
        
        
        TreeViewRoot.Children.Add(layersRoot);
        TreeViewRoot.Children.Add(defaultLayer);
        TreeViewRoot.Children.Add(redLayer);


    }
}
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Color = System.Drawing.Color;

namespace AuroraCAD_2D.Views;

public partial class TreeViewItem : UserControl{
    private List<TreeViewItem> _children;
  

    public TreeViewItem(string name, bool isContainer){
        InitializeComponent();
        commonConstructor(name,isContainer, null,null);
    }
    
    public TreeViewItem(string name, bool isContainer,IBrush color){
        InitializeComponent();
        commonConstructor(name,isContainer, null,color);
    }
    
    
    public TreeViewItem(string name,string iconPath, bool isContainer){
        InitializeComponent();
        commonConstructor(name,isContainer,iconPath,null);
    }

    private void commonConstructor(string name, bool isContainer, string? iconPath, IBrush? color){
        NameTB.Text = name;
        ShowButton.IsVisible = isContainer;
        if (isContainer){
            _children = new List<TreeViewItem>();
            
            ShowButton.Click += (sender, args) => {
                bool flag = ShowButton.Content.ToString() == "+";
                ShowButton.Content = flag ? "-" : "+";
               
                foreach (TreeViewItem item in _children){ 
                    item.IsVisible = flag;
                }
                
            };
        } else{
            IconImage.Margin = new Thickness(40,0,0,0);
        }

        if (iconPath != null){
            IconImage.Source = new Bitmap(iconPath);
        }

        if (color != null){
            GeometryDrawing gd = new GeometryDrawing();
            gd.Geometry = new RectangleGeometry(new Rect(new Size(30, 30)));
            gd.Brush = color;
            IconImage.Source = new DrawingImage(gd);
            
        }
    }

    public List<TreeViewItem> Children => _children;

    public void addItem(TreeViewItem item){
        if (ShowButton.IsVisible){
            _children.Add(item);
        } 
    }
    

}
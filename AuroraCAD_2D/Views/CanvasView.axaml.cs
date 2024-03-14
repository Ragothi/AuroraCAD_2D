using System;
using AuroraCAD_2D.Database;
using AuroraCAD_2D.Models;
using AuroraCAD_2D.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic.CompilerServices;
using Line = AuroraCAD_2D.Models.Line;
using Point = AuroraCAD_2D.Models.Point;
using Utils = AuroraCAD_2D.Services.Utils;

namespace AuroraCAD_2D.Views;

public partial class CanvasView : UserControl{
    private CanvasViewModel _canvasViewModel = new CanvasViewModel();
    public CanvasView(){
        InitializeComponent();
        Settings.CanvasGlobalReference = Canvas;
    }

    

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e){
        if (Settings.isDrawXXXSelected[0]){
            Point p = _canvasViewModel.PointerPressedHandler(sender,e);
            drewNew(p);
        }
        else if (Settings.isDrawXXXSelected[1]){
            if (Settings.selectedPoint is null){
                Point p = _canvasViewModel.PointerPressedHandler(sender,e);
                drewNew(p);
                Settings.selectedPoint = p;
            }
            else{
                Point p2 = _canvasViewModel.PointerPressedHandler(sender,e);
                Line line = new Line(Settings.selectedPoint, p2);
                drewNew(line);
                Settings.selectedPoint = null;
            }
        }
    }

    private void redraw(){
        
    }
    
    private void drewNew(Drawable d){
        switch (d.getType()){
            case Drawable.DrawableType.POINT:
                Point p = d as Point ?? throw new InvalidOperationException();
                Utils.addToCanvas(Canvas,p,p.X,p.Y,0,0,false);
                p.PointerEntered += PointOnPointerEntered;
                p.PointerExited += PointOnPointerExited;
                break;
            case Drawable.DrawableType.LINE:
                Line l = d as Line ?? throw new InvalidOperationException();
                drewNew(l.End);
                Canvas.Children.Add(l);
                l.PointerEntered += LineOnPointerEntered;
                l.PointerExited += LineOnPointerExited;
                break;
            case Drawable.DrawableType.CIRCLE:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void PointOnPointerEntered(object? sender, PointerEventArgs e){
        (sender as Point).Fill = Brushes.Coral;
    }
    
    private void PointOnPointerExited(object? sender, PointerEventArgs e){
        Point p = (sender as Point);
        p.Fill = Brush.Parse(p.getLayer().Color.ToString());
    }
    
    private void LineOnPointerEntered(object? sender, PointerEventArgs e){
        (sender as Line).Stroke = Brushes.Coral;
    }
    
    private void LineOnPointerExited(object? sender, PointerEventArgs e){
        Line l = (sender as Line);
        l.Stroke = Brush.Parse(l.getLayer().Color.ToString());
    }
}
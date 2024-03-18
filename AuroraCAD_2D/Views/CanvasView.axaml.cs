using System;
using AuroraCAD_2D.Database;
using AuroraCAD_2D.Models;
using AuroraCAD_2D.Services;
using AuroraCAD_2D.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Line = AuroraCAD_2D.Models.Line;
using Point = AuroraCAD_2D.Models.Point;
using Utils = AuroraCAD_2D.Services.Utils;

namespace AuroraCAD_2D.Views;

public partial class CanvasView : UserControl{
    private CanvasViewModel _canvasViewModel = new CanvasViewModel();
    public CanvasView(){
        InitializeComponent();
        Settings.CanvasGlobalReference = CanvasViewRef;
        
    }

    private void pointerOverCanvas(object? sender, PointerEventArgs e){
        _canvasViewModel.pointerOverCanvasHandler(sender,e);
        if (Settings.selectedPoint != null && Settings.selectedLine != null && Settings.isDrawXXXSelected[1]){
            // Logger.log(string.Format("({0} ; {1})",Settings.selectedLine.End.X,Settings.selectedLine.End.Y));
            Settings.selectedLine.replaceEnd(Settings.mouseX,Settings.mouseY);
        } else if (Settings.selectedPoint != null && Settings.selectedCircle != null && Settings.isDrawXXXSelected[2]){
            Settings.selectedCircle.changeRadius(Settings.mouseX, Settings.mouseY);
        }
    }

    private Point validatePoint(Point p){
        bool flag = false;
        foreach (Point point in Database.Database.Points){
            if (p.distance(point) <= Settings.margin){
                p = point;
                flag = true;
                Settings.isNewPointAdded = false;
            }
        }

        if (!flag){
            drawNew(p);  
            Database.Database.addPoint(p);
        }

        return p;
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e){
        if (Settings.isDrawXXXSelected[0]){
            Point p = _canvasViewModel.PointerPressedHandler(sender,e);
            p = validatePoint(p);
            Settings.isNewPointAdded = true;
            p.PointerEntered += PointOnPointerEntered;
            p.PointerExited += PointOnPointerExited;
        }
        else if (Settings.isDrawXXXSelected[1]){
            if (Settings.selectedPoint is null){
                Point p = _canvasViewModel.PointerPressedHandler(sender,e);
                Settings.selectedPoint = validatePoint(p);
                Point p2 = new Point(Settings.mouseX, Settings.mouseY);
                Line line = new Line(Settings.selectedPoint, p2);
                Settings.selectedLine = line;
                drawNew(line);
                p.PointerEntered += PointOnPointerEntered;
                p.PointerExited += PointOnPointerExited;
            }
            else{
                Line l = Settings.selectedLine;
                Point p = l.End;
                p = validatePoint(p);
                l.replaceEnd(p.X, p.Y);
                Database.Database.addLine(l);
                l.PointerEntered += LineOnPointerEntered;
                l.PointerExited += LineOnPointerExited;
                Settings.selectedLine = null;
                Settings.selectedPoint = null;
                Settings.isNewPointAdded = true;
            }
        } else if (Settings.isDrawXXXSelected[2]){
            if (Settings.selectedPoint == null){
                Point p = _canvasViewModel.PointerPressedHandler(sender,e);
                Settings.selectedPoint =  validatePoint(p);
                Circle c = new Circle(Settings.selectedPoint.X, Settings.selectedPoint.Y, 10);
                Settings.selectedCircle = c;
                drawNew(c);
            }
            else{
                Circle c = Settings.selectedCircle;
                Point p = c.Centre;
                c.PointerEntered += CircleOnPointerEntered;
                c.PointerExited += CircleOnPointerExited;
                p.PointerEntered += PointOnPointerEntered;
                p.PointerExited += PointOnPointerExited;
                Settings.selectedCircle = null;
                Settings.selectedPoint = null;
                Database.Database.addLine(c);
                Settings.isNewPointAdded = true;
            }
        }
    }

    private void redraw(){
        
    }
    
    private void drawNew(Drawable d){
        switch (d.getType()){
            case Drawable.DrawableType.POINT:
                Point p = d as Point ?? throw new InvalidOperationException();
                Utils.addToCanvas(CanvasViewRef, p, p.X - p.getLayer().PointSize / 2, p.Y - p.getLayer().PointSize / 2,
                    0, 0, false);
                break;
            case Drawable.DrawableType.LINE:
                Line l = d as Line ?? throw new InvalidOperationException();
                // drewNew(l.End);
                CanvasViewRef.Children.Add(l);
                break;
            case Drawable.DrawableType.CIRCLE:
                Circle c = d as Circle ?? throw new InvalidOperationException();
                Utils.addToCanvas(CanvasViewRef,c,c.Centre.X-c.Rad,c.Centre.Y-c.Rad,0,0,false);
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
    
    private void CircleOnPointerEntered(object? sender, PointerEventArgs e){
        (sender as Circle).Stroke = Brushes.Coral;
    }
    
    private void CircleOnPointerExited(object? sender, PointerEventArgs e){
        Circle c = (sender as Circle);
        c.Stroke = Brush.Parse(c.getLayer().Color.ToString());
    }

    private void CanvasViewRef_OnPointerWheelChanged(object? sender, PointerWheelEventArgs e){
        if (e.Delta.Y >0){
            /*
             * Implement zoom method
             * 
             */
        }
    }
}
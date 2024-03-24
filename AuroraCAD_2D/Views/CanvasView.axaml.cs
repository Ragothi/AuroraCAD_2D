using System;
using System.Linq;
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
        Settings.CanvasViewGlobalReference = this;
        Utils.addToCanvas(CanvasViewRef,Settings.selectionBox,0,0,0,0,false);
        Settings.selectionBox.IsVisible = false;

    }

    private void pointerOverCanvas(object? sender, PointerEventArgs e){
        _canvasViewModel.pointerOverCanvasHandler(sender,e);
        if (Settings.selectedPoint != null && Settings.selectedLine != null && Settings.isDrawXXXSelected[1]){
            // Logger.log(string.Format("({0} ; {1})",Settings.selectedLine.End.X,Settings.selectedLine.End.Y));
            Settings.selectedLine.replaceEnd(Settings.mouseX,Settings.mouseY);
        } else if (Settings.selectedPoint != null && Settings.selectedCircle != null && Settings.isDrawXXXSelected[2]){
            Settings.selectedCircle.changeRadius(Settings.mouseX, Settings.mouseY);
        } else if (Settings.selectionBox.IsVisible){
            double dx = Settings.mouseX - Settings.clickedCanvasIn.X;
            double dy = Settings.mouseY - Settings.clickedCanvasIn.Y;
            if (dx <=0){
                Canvas.SetLeft(Settings.selectionBox,Settings.mouseX);
                Settings.selectionBox.Width = Math.Abs(Settings.mouseX - Settings.clickedCanvasIn.X);
            } else if (dx > 0){
                Canvas.SetLeft(Settings.selectionBox,Settings.clickedCanvasIn.X);
                Settings.selectionBox.Width = Math.Abs(Settings.mouseX - Settings.clickedCanvasIn.X);
            }

            if (dy >=0){
                Canvas.SetTop(Settings.selectionBox,Settings.clickedCanvasIn.Y);
                Settings.selectionBox.Height = Math.Abs(Settings.mouseY - Settings.clickedCanvasIn.Y);
            } else if (dy < 0){
                Canvas.SetTop(Settings.selectionBox,Settings.mouseY);
                Settings.selectionBox.Height = Math.Abs(Settings.mouseY - Settings.clickedCanvasIn.Y);
            }
            
        }
    }

    private Point validatePoint(Point p){
        bool flag = false;
        foreach (Point point in Database.Database.Points){
            if (p.distance(point) <= Settings.margin){
                p = point;
                flag = true;
                Settings.isNewPointAdded = false;
                break;
            }
        }

        if (!flag){
            drawNew(p);  
            Database.Database.addPoint(p);
        }

        return p;
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e){
        if (e.KeyModifiers != KeyModifiers.Control){
            Database.Database.unselectAll();    
        }
        
        if (Settings.isDrawXXXSelected[0]){
            Point p = new Point(Settings.mouseX,Settings.mouseY);
            p = validatePoint(p);
            Settings.isNewPointAdded = true;
            p.PointerEntered += PointOnPointerEntered;
            p.PointerExited += PointOnPointerExited;
        }
        else if (Settings.isDrawXXXSelected[1]){
            if (Settings.selectedPoint is null){
                Point p = new Point(Settings.mouseX,Settings.mouseY);
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
                Point p = new Point(Settings.mouseX,Settings.mouseY);
                Settings.selectedPoint =  validatePoint(p);
                Circle c = new Circle(Settings.selectedPoint, 10);
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
        else if (e.Pointer.IsPrimary){
            Logger.log("Selection box activated");
            if (!CanvasViewRef.Children.Contains(Settings.selectionBox)){
                CanvasViewRef.Children.Add(Settings.selectionBox);
            }
            Settings.selectionBox.IsVisible = true;
            Settings.clickedCanvasIn = new Point(Settings.mouseX, Settings.mouseY);
            // Canvas.SetLeft(Settings.selectionBox,Settings.mouseX);
            // Canvas.SetTop(Settings.selectionBox,Settings.mouseY);
            Settings.selectionBox.Width = 1;
            Settings.selectionBox.Height = 1;
            
        }
        
    }

    
    public void drawNew(Drawable d){
        switch (d.getType()){
            case Drawable.DrawableType.POINT:
                Point p = d as Point ?? throw new InvalidOperationException();
                Utils.addToCanvas(Settings.CanvasGlobalReference, p, p.X - p.getLayer().PointSize / 2, p.Y - p.getLayer().PointSize / 2,
                    0, 0, false);
                break;
            case Drawable.DrawableType.LINE:
                Line l = d as Line ?? throw new InvalidOperationException();
                // drewNew(l.End);
                Settings.CanvasGlobalReference.Children.Add(l);
                break;
            case Drawable.DrawableType.CIRCLE:
                Circle c = d as Circle ?? throw new InvalidOperationException();
                Utils.addToCanvas(Settings.CanvasGlobalReference,c,c.Centre.X-c.Rad,c.Centre.Y-c.Rad,0,0,false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void drawNew(Drawable d, bool addListeners){
        drawNew(d);
        if (addListeners){
            switch (d.getType()){
                case Drawable.DrawableType.POINT:
                    Point p = d as Point;
                    p.PointerEntered += PointOnPointerEntered;
                    p.PointerExited += PointOnPointerExited;
                    break;
                case Drawable.DrawableType.LINE:
                    Line l = d as Line;
                    l.PointerEntered += LineOnPointerEntered;
                    l.PointerExited += LineOnPointerExited;
                    break;
                case Drawable.DrawableType.CIRCLE:
                    Circle c = d as Circle;
                    c.PointerEntered += CircleOnPointerEntered;
                    c.PointerExited += CircleOnPointerExited;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
   

    private void PointOnPointerEntered(object? sender, PointerEventArgs e){
        if (!Settings.selectedDrawables.Any()){
            (sender as Point).Fill = Settings.highlightColor;
        }
    }
    
    private void PointOnPointerExited(object? sender, PointerEventArgs e){
        if (!Settings.selectedDrawables.Any()){
            Point p = (sender as Point);
            p.Fill = Brush.Parse(p.getLayer().Color.ToString());
        }
    }
    
    private void LineOnPointerEntered(object? sender, PointerEventArgs e){
        if (!Settings.selectedDrawables.Any()){
            (sender as Line).Stroke = Settings.highlightColor;
        }
    }
    
    private void LineOnPointerExited(object? sender, PointerEventArgs e){
        if (!Settings.selectedDrawables.Any()){
            Line l = (sender as Line);
            l.Stroke = Brush.Parse(l.getLayer().Color.ToString());
        }
    }
    
    private void CircleOnPointerEntered(object? sender, PointerEventArgs e){
        if (!Settings.selectedDrawables.Any()){
            (sender as Circle).Stroke = Settings.highlightColor;
        }
    }
    
    private void CircleOnPointerExited(object? sender, PointerEventArgs e){
        if (!Settings.selectedDrawables.Any()){
            Circle c = (sender as Circle);
            c.Stroke = Brush.Parse(c.getLayer().Color.ToString());
        }
    }

    private void CanvasViewRef_OnPointerWheelChanged(object? sender, PointerWheelEventArgs e){
        if (e.Delta.Y >0){
            /*
             * Implement zoom method
             * 
             */
        }
    }

    private void CanvasViewRef_OnPointerReleased(object? sender, PointerReleasedEventArgs e){
        if (Settings.selectionBox.IsVisible){
            Logger.log("Selection box deactivated");
            Settings.selectionBox.IsVisible = false;
            Database.Database.getDrawablesInSelection();
            foreach (Drawable d in Settings.selectedDrawables){
                switch (d.getType()){
                    case Drawable.DrawableType.POINT:
                        Point p = d as Point;
                        p.Fill = Settings.highlightColor;
                        break;
                    case Drawable.DrawableType.LINE:
                        Line l = d as Line;
                        l.Stroke = Settings.highlightColor;
                        l.Start.Fill = Settings.highlightColor;
                        l.End.Fill = Settings.highlightColor;
                        break;
                    case Drawable.DrawableType.CIRCLE:
                        Circle c = d as Circle;
                        c.Centre.Fill = Settings.highlightColor;
                        c.Stroke = Settings.highlightColor;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
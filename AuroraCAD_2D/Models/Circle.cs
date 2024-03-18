using System;
using System.Runtime.Intrinsics.X86;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;

namespace AuroraCAD_2D.Models;

public class Circle : Ellipse, Drawable{
    private Layer _layer;
    private Point _centre;
    private double _rad;

    public Circle(double x, double y, double rad){
        _centre = new Point(x, y);
        _layer = Layer.defaultLayer;
        _rad = rad;
        Stroke = Layer.Color;
        StrokeThickness = getLayer().LineSize;
        Width = 2 * _rad;
        Height = 2 * _rad;
    }

    public Layer Layer{
        get => _layer;
        set => _layer = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Point Centre{
        get => _centre;
        set => _centre = value ?? throw new ArgumentNullException(nameof(value));
    }

    public double Rad{
        get => _rad;
        set => _rad = value;
    }

    public Drawable.DrawableType getType(){
        return Drawable.DrawableType.CIRCLE;
    }

    public Layer getLayer(){
        return _layer;
    }

    public void changeRadius(double mouseX, double mouseY){
        _rad = _centre.distance(new Point(mouseX, mouseY));
        Width = 2*_rad;
        Height = 2*_rad;
        Canvas.SetLeft(this,Centre.X-_rad);
        Canvas.SetTop(this,Centre.Y-_rad);
    }
}
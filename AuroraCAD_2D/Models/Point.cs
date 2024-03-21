using System;
using AuroraCAD_2D.Database;
using Avalonia.Controls.Shapes;

namespace AuroraCAD_2D.Models;

public class Point : Ellipse, Drawable{
    private double _x, _y;
    private Layer _layer;

    public Point(double x, double y, Layer layer){
        _layer = layer;
        commonConstructor(x,y);
    }

    public Point(double x, double y){
        _layer = Settings.selectedLayer;
        commonConstructor(x,y);
    }

    private void commonConstructor(double x, double y){
        _x = x;
        _y = y;
        Fill = getLayer().Color;
        Width = getLayer().PointSize;
        Height = getLayer().PointSize;
    }

    public double distance(Point p){
        double x = X - p.X;
        double y = Y - p.Y;
        return Math.Sqrt(x * x + y * y);
    }

    public double X{
        get => _x;
        set => _x = value;
    }

    public double Y{
        get => _y;
        set => _y = value;
    }

    public Layer getLayer(){
        return _layer;
    }


    public Drawable.DrawableType getType(){
        return Drawable.DrawableType.POINT;
    }

    
}
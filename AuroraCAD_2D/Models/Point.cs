using System;
using Avalonia.Controls.Shapes;

namespace AuroraCAD_2D.Models;

public class Point : Ellipse, Drawable{
    private double _x, _y;
    private Layer _layer;
    

    public Point(double x, double y){
        _x = x;
        _y = y;
        _layer = Layer.defaultLayer;
        Fill = getLayer().Color;
        Width = getLayer().PointSize;
        Height = getLayer().PointSize;
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
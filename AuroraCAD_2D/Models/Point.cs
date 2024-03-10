using System;

namespace AuroraCAD_2D.Models;

public class Point{
    private double _x, _y;
    private Layer _layer;

    public Point(double x, double y){
        _x = x;
        _y = y;
        _layer = Layer.defaultLayer;
    }

    public double X{
        get => _x;
        set => _x = value;
    }

    public double Y{
        get => _y;
        set => _y = value;
    }

    public Layer Layer{
        get => _layer;
        set => _layer = value ?? throw new ArgumentNullException(nameof(value));
    }
}
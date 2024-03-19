using System;
using Avalonia.Media;

namespace AuroraCAD_2D.Models;

public class Layer{
    public static readonly Layer defaultLayer = 
        new Layer(Brush.Parse(Colors.White.ToString()),6,2,"Default");

    private IBrush _color;
    private double _pointSize;
    private double _lineSize;
    private string _name ;
    private bool _isVisible = true;

    public Layer(IBrush color, double pointSize,double lineSize, string name){
        _color = color;
        _pointSize = pointSize;
        _lineSize = lineSize;
        _name = name;
        Database.Database.addLayer(this);
    }

    public bool IsVisible{
        get => _isVisible;
        set => _isVisible = value;
    }

    public string Name{
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public IBrush Color{
        get => Brush.Parse(_color.ToString());
        set => _color = value;
    }

    public double PointSize{
        get => _pointSize;
        set => _pointSize = value;
    }
    
    public double LineSize{
        get => _lineSize;
        set => _lineSize = value;
    }
}
using Avalonia.Media;

namespace AuroraCAD_2D.Models;

public class Layer{
    public static readonly Layer defaultLayer = 
        new Layer(Brush.Parse(Colors.White.ToString()),6,2);

    private IBrush _color;
    private double _pointSize;
    private double _lineSize;

    public Layer(IBrush color, double pointSize,double lineSize){
        _color = color;
        _pointSize = pointSize;
        _lineSize = lineSize;
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
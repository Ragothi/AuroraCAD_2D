using Avalonia.Media;

namespace AuroraCAD_2D.Models;

public class Layer{
    public static readonly Layer defaultLayer = new Layer(Color.FromRgb(255,255,255),12);

    private Color _color;
    private double _size;

    public Layer(Color color, double size){
        _color = color;
        _size = size;
    }
}
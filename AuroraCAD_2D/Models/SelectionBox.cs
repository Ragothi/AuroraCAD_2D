using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace AuroraCAD_2D.Models;

public class SelectionBox : Rectangle{
    
    public SelectionBox(){
        StrokeThickness = 5;
        Stroke = Brushes.DodgerBlue;
        Fill = new SolidColorBrush(Colors.DodgerBlue,0.2);
        
    }
}
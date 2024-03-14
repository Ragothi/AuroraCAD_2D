using System;
using AuroraCAD_2D.Models;
using AuroraCAD_2D.Services;
using Avalonia.Controls;
using Avalonia.Input;

namespace AuroraCAD_2D.ViewModels;

public class CanvasViewModel : ViewModelBase{
    
    
    public Point PointerPressedHandler (object sender, PointerPressedEventArgs args)
    {
        var point = args.GetCurrentPoint(sender as Control);
        var x = point.Position.X;
        var y = point.Position.Y;
        var msg = $"Pointer press at {x}, {y} relative to sender.";
        if (point.Properties.IsLeftButtonPressed)
        {
            msg += " Left button pressed.";
        }
        if (point.Properties.IsRightButtonPressed)
        {
            msg += " Right button pressed.";
        }

        Logger.log(msg);
        Point p = new Point(x, y);
        Database.Database.addPoint(p);
        return p;
    }
}
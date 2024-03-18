using System;
using AuroraCAD_2D.Database;
using AuroraCAD_2D.Models;
using AuroraCAD_2D.Services;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Remote.Protocol.Input;

namespace AuroraCAD_2D.ViewModels;

public class CanvasViewModel : ViewModelBase{

    public void pointerOverCanvasHandler(object sender, PointerEventArgs args){
        var point = args.GetCurrentPoint(sender as Control);
        Settings.mouseX = point.Position.X;
        Settings.mouseY = point.Position.Y;
        // Logger.log(string.Format("Mouse (X;Y): ({0} ; {1})",Settings.mouseX,Settings.mouseY));
    }
    
    public Point PointerPressedHandler (object sender, PointerPressedEventArgs args)
    {
        PointerPoint point = args.GetCurrentPoint(sender as Control);
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
        return p;
    }

   
}
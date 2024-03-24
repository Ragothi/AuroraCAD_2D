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
    
    

   
}
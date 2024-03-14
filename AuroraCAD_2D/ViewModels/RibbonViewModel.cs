using System;

using System.Reactive;
using System.Windows.Input;
using AuroraCAD_2D.Database;
using AuroraCAD_2D.Models;
using AuroraCAD_2D.Services;
using Avalonia.Media;
using ReactiveUI;

namespace AuroraCAD_2D.ViewModels;

public class RibbonViewModel : ViewModelBase{
   


    public RibbonViewModel(){
        
    }

    public void ClearAllFlags(){
        Settings.clearIsDrawnFlags(-1);
        Logger.log(testLog());
    }
    
    public void DrawPointFlag(){
        Settings.clearIsDrawnFlags(0);
        Settings.isDrawXXXSelected[0] = !Settings.isDrawXXXSelected[0];
        Logger.log(testLog());
    }
    
    public void DrawLineFlag(){
        Settings.clearIsDrawnFlags(1);
        Settings.isDrawXXXSelected[1] = !Settings.isDrawXXXSelected[1];
        Logger.log(testLog());
    }
    
    public void DrawCircleFlag(){
        Settings.clearIsDrawnFlags(2);
        Settings.isDrawXXXSelected[2] = !Settings.isDrawXXXSelected[2];
        Logger.log(testLog());
    }

    private String testLog(){
        return String.Format("*************************\r\nIs drawing points enabled?\t{0}\r\nIs drawing lines enabled?\t{1}" +
                      "\r\nIs drawing circles enabled?\t{2}\r\n*************************",
            Settings.isDrawXXXSelected[0],Settings.isDrawXXXSelected[1],Settings.isDrawXXXSelected[2]);
        
    }
    
}
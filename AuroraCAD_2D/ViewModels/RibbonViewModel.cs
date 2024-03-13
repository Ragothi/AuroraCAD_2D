using System;
using System.Reactive;
using System.Windows.Input;
using AuroraCAD_2D.Database;
using AuroraCAD_2D.Models;
using ReactiveUI;

namespace AuroraCAD_2D.ViewModels;

public class RibbonViewModel : ViewModelBase{

    public RibbonViewModel(){
        
    }

    public void ClearAllFlags(){
        Settings.clearIsDrawnFlags();
        testLog();
    }
    
    public void DrawPointFlag(){
        Settings.clearIsDrawnFlags();
        Settings.isDrawXXXSelected[0] = !Settings.isDrawXXXSelected[0];
       testLog();
    }
    
    public void DrawLineFlag(){
        Settings.clearIsDrawnFlags();
        Settings.isDrawXXXSelected[1] = !Settings.isDrawXXXSelected[1];
        testLog();
    }
    
    public void DrawCircleFlag(){
        Settings.clearIsDrawnFlags();
        Settings.isDrawXXXSelected[2] = !Settings.isDrawXXXSelected[2];
        testLog();
    }

    private void testLog(){
        Console.WriteLine("*************************");
        Console.WriteLine("Is drawing points enabled?\t"+Settings.isDrawXXXSelected[0]);
        Console.WriteLine("Is drawing lines enabled?\t"+Settings.isDrawXXXSelected[1]);
        Console.WriteLine("Is drawing circles enabled?\t"+Settings.isDrawXXXSelected[2]);
        Console.WriteLine("*************************");
    }
    
}
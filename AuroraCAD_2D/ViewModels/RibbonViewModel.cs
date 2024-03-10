using System.Reactive;
using AuroraCAD_2D.Database;
using ReactiveUI;

namespace AuroraCAD_2D.ViewModels;

public class RibbonViewModel : ViewModelBase{
    public RibbonViewModel(){
        DrawPoint = ReactiveCommand.Create(DrawPointFlag);
    }

    public ReactiveCommand<Unit,Unit> DrawPoint { get; }
    
    void DrawPointFlag(){
        Settings.isDrawPointSelected = !Settings.isDrawPointSelected;
       
    }
    
}
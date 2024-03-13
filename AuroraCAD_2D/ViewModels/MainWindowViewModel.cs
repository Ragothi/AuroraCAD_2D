using System;
using System.Reactive;
using AuroraCAD_2D.Database;
using ReactiveUI;

namespace AuroraCAD_2D.ViewModels;

public class MainWindowViewModel : ViewModelBase{
#pragma warning disable CA1822 // Mark members as static
#pragma warning restore CA1822 // Mark members as static

    public MainWindowViewModel(){
        TestComment = ReactiveCommand.Create(Test);
    }
    
    public ReactiveCommand<Unit,Unit> TestComment { get; }

    public void Test(){
        Console.WriteLine("Test");
    }
    
    
}
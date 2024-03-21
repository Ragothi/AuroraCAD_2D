using System.IO;
using AuroraCAD_2D.Database;
using AuroraCAD_2D.Views;
using Avalonia;
using Avalonia.Controls;

namespace AuroraCAD_2D.Services;

public class Utils{

    public static string readFile(string fileName){
        return File.ReadAllText(fileName);
    }

    public static void writeTextToAFile(string text, string fileName, bool append){
        if (append){
            File.AppendAllText(fileName,text);
        }
        else{
            File.WriteAllText(fileName,text);
        }
    }
    
    public static void addToCanvas(Canvas parent, Control child, double left, double top, double right,
        double bottom, bool forceZero){
        parent.Children.Add(child);
        if (left!=0 || forceZero){
            Canvas.SetLeft(child,left);
        }
        if (top!=0 || forceZero){
            Canvas.SetTop(child,top);
        }
        if (right!=0 || forceZero){
            Canvas.SetRight(child,right);
        }
        if (bottom!=0 || forceZero){
            Canvas.SetBottom(child,bottom);
        }
       
    }
}
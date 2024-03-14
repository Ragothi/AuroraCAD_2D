using AuroraCAD_2D.Models;
using Avalonia.Controls;

namespace AuroraCAD_2D.Database;

public class Settings{
   
/**
 * 0 - point
 * 1 - line
 * 2 - circle
 */
public static bool[] isDrawXXXSelected = new bool[]{ false, false, false };

public static bool isLoggerOn = true;
public static Point selectedPoint = null;
public static Canvas CanvasGlobalReference = null;

public static void clearIsDrawnFlags(int skip){
    for (int i = 0; i < isDrawXXXSelected.Length; i++){
        if (skip != i){
            isDrawXXXSelected[i] = false;
        }
    }
}
}
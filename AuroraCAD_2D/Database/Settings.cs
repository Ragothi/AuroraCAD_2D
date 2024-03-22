using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AuroraCAD_2D.Models;
using AuroraCAD_2D.Views;
using Avalonia.Controls;
using Avalonia.Media;
using TreeView = AuroraCAD_2D.Views.TreeView;

namespace AuroraCAD_2D.Database;

public class Settings{
   
/**
 * 0 - point
 * 1 - line
 * 2 - circle
 */
public static bool[] isDrawXXXSelected = new bool[]{ false, false, false };
public static bool isLoggerOn = true;
public static bool isNewPointAdded = true;


public static Point selectedPoint = null;
public static Line selectedLine = null;
public static Circle selectedCircle = null;
public static Layer selectedLayer = Layer.defaultLayer;
public static Canvas CanvasGlobalReference = null;
public static CanvasView CanvasViewGlobalReference = null;
public static TreeView TreeViewGlobalReference = null;
public static RibbonView RibbonViewGlobalReference = null;
public static List<Drawable> selectedDrawables = new List<Drawable>();
public static SelectionBox selectionBox = new SelectionBox();
public static Point clickedCanvasIn = null;

public static double mouseX, mouseY;
public static double margin = 10;

public static readonly string rootFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Split("bin\\Debug\\net7.0")[0];
public static readonly string assetsFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Split("bin\\Debug\\net7.0")[0]+"/Assets/";
public static IBrush highlightColor = Brushes.Coral;


public static void clearIsDrawnFlags(int skip){
    for (int i = 0; i < isDrawXXXSelected.Length; i++){
        if (skip != i){
            isDrawXXXSelected[i] = false;
        }
    }
}
}
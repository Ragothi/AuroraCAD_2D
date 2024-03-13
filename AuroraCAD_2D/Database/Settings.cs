namespace AuroraCAD_2D.Database;

public class Settings{
   
/**
 * 0 - point
 * 1 - line
 * 2 - circle
 */
    public static bool[] isDrawXXXSelected = new bool[]{ false, false, false };

public static void clearIsDrawnFlags(){
    for (int i = 0; i < isDrawXXXSelected.Length; i++){
        isDrawXXXSelected[i] = false;
    }
}
}
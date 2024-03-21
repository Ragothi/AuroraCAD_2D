using AuroraCAD_2D.Database;
using AuroraCAD_2D.Services;
using Microsoft.VisualBasic.CompilerServices;
using Utils = AuroraCAD_2D.Services.Utils;

namespace AuroraCAD_2D.Testing;

public class Test{
    public static void Main(string[] args){
        string fileName = "save.txt";
        string content = "First line \r\n" +
                         "Second line \r\n";
        Utils.writeTextToAFile(content,Settings.assetsFolder+fileName,false);

        Logger.log(Utils.readFile(Settings.assetsFolder+fileName));
    }
}
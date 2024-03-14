using System;
using AuroraCAD_2D.Database;

namespace AuroraCAD_2D.Services;

public class Logger{
    public static void log(String msg){
        if (Settings.isLoggerOn){
            Console.WriteLine(msg);
        }
    }
}
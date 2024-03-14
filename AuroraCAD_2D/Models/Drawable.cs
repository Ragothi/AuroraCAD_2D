namespace AuroraCAD_2D.Models;

public interface Drawable{
    enum DrawableType{
        POINT,LINE,CIRCLE
    }

    public DrawableType getType();

    public Layer getLayer();
}
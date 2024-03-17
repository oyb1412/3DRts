public class Define 
{
    public enum BuildList
    {
        Barrack,
        Tower,
    }
 
    public enum MouseEventType
    {
        None,
        LeftClick,
        RightClick,
        Press,
        PressUp,
        PointDown,
        PointUp,
        Enter,
        Drag,
    }

    public enum SceneType
    {
        None,
        InGame,
    }
    
    public enum Select
    {
        None,
        Select,
        Deselect,
    }

    public enum Layer
    {
        Ground = 6,
        Monster,
        Player,
        Building,
        Mine,
    }

    public enum Cursor
    {
        Main,
        Attack,
        Patrol,
    }
    
    public struct UnitCreatePos
    {
        public int X;
        public int Z;
    }
}

public static class Directions
{
    public static Direction GetOpposite( Direction direction )
    {
        switch( direction )
        {
            case ( Direction.North ):
                return Direction.South;
            case ( Direction.South ):
                return Direction.North;
            case ( Direction.West ):
                return Direction.East;
            case ( Direction.East ):
                return Direction.West;
            default:
                return Direction.None;
        }
    }
}

public enum Direction
{
    None,
    North,
    East,
    South,
    West
}

using UnityEngine;

public static class DirecaoUtil
{
    public static Direction Oposta(Direction dir)
    {
        return (Direction)(((int)dir + 2) % 4);
    }

    public static Vector2Int ParaVetor(Direction dir)
    {
        return dir switch
        {
            Direction.Up => new Vector2Int(0, 1),
            Direction.Right => new Vector2Int(1, 0),
            Direction.Down => new Vector2Int(0, -1),
            Direction.Left => new Vector2Int(-1, 0),
            _ => Vector2Int.zero
        };
    }

    public static Direction Rotacionada(Direction dir, int graus)
    {
        int rot = ((int)dir + (graus / 90)) % 4;
        return (Direction)rot;
    }
}

using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour 
{
    public Bounds gridBounds;
    public int rows;
    public int columns;

    public bool baseBoundsOnTileSize;
    public float tileSize;
    public float tileBuffer;

    public Vector3 GetGridPosition( Vector2 coordinate )
    {
        return GetGridPosition( (int)coordinate.x, (int)coordinate.y );
    }

    public Vector3 GetGridPosition( int x, int y )
    {
        Vector3 xOffset = Vector3.right * x * _xIncrement;
        Vector3 yOffset = Vector3.up * y * _yIncrement;
        if ( baseBoundsOnTileSize )
        {
            xOffset += Vector3.right * _xIncrement * .5f + Vector3.right * tileBuffer * x;
            yOffset += Vector3.up * _yIncrement * .5f + Vector3.up * tileBuffer * y;
        }
        return _origin + xOffset + yOffset;
    }

    public void SetGridSize( int width, int height )
    {
        rows = height;
        columns = width;
    }

    public void SetTileSize( float tileSize )
    {
        this.tileSize = tileSize;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube( gridBounds.center, gridBounds.size );

        for ( int y = 0; y < rows; y++ )
        {
            for ( int x = 0; x < columns; x++ )
            {
                Gizmos.DrawWireSphere( GetGridPosition( x, y ), 0.1f );
            }
        }
    }

    Bounds GetGridBounds()
    {
        if ( baseBoundsOnTileSize )
        {
            Vector3 center = gridBounds.center;
            
            float xSize = columns * tileSize;
            float ySize = rows * tileSize;
            Vector3 size = new Vector3( xSize, ySize, 0f );

            gridBounds = new Bounds( center, size );
        } 

        return gridBounds;
    }

    private Vector3 _origin { get { return GetGridBounds().min; } }
    private float _width { get { return GetGridBounds().size.x; } }
    private float _height { get { return GetGridBounds().size.y; } }
    public float height { get { return _height; } }
    private float _xIncrement { get { return (float)_width / (float)( columns ); } }
    private float _yIncrement { get { return (float)_height / (float)( rows ); } }
    public float yIncrement { get { return _yIncrement; } }
}

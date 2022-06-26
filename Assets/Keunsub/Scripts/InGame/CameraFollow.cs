using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform Target; //always player
    public float followSpeed;
    public CameraBound Bound;
    
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        CameraSystem();
    }

    void CameraSystem()
    {
        Vector3 vec = Vector3.Lerp(transform.position, Target.position + new Vector3(0, 0, -10f), Time.deltaTime * followSpeed);
        Camera.main.orthographicSize = Bound.Size;
        if(vec.x < Bound.rect.right)
        {
            vec.x = Bound.rect.right;
        }
        if(vec.x > Bound.rect.left)
        {
            vec.x = Bound.rect.left;
        }
        if(vec.y < Bound.rect.down)
        {
            vec.y = Bound.rect.down;
        }
        if(vec.y > Bound.rect.up)
        {
            vec.y = Bound.rect.up;
        }

        transform.position = vec;
    }
}

[System.Serializable]
public class CameraBound
{
    public Vector2 Pos;
    public float Width;
    public float Height;
    public float Size;
    public BoundRect rect;

    public CameraBound(Vector2 _pos, float _width, float _height, float _size)
    {
        Pos = _pos;
        Width = _width;
        Height = _height;
        Size = _size;

        rect = new BoundRect(Pos.x - Width, Pos.x + Width, Pos.y + Height, Pos.y - Height);
    }
}

[System.Serializable]
public class BoundRect
{
    public float right, left, up, down;

    public BoundRect(float r, float l, float u, float d)
    {
        right = r;
        left = l;
        up = u;
        down = d;
    }
}
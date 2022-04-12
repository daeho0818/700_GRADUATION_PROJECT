using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    FollowVertical,
    FollowHorizontal
}

public class TitleRoom : MonoBehaviour
{
    public Vector2 clamp;
    public RoomType thisType;
}

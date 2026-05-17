using UnityEngine;
public enum WallOrientation { Top, Left, Right }

public class Wall : MonoBehaviour
{
    [SerializeField] private WallOrientation orientation;

    public WallOrientation Orientation => orientation;

}

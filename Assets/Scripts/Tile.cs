using UnityEngine;

public class Tile : MonoBehaviour
{
    [Tooltip("Tile options order:\n-x = TOP\n+z = RIGHT\n+x = BOTTOM\n-z = LEFT")]
    public string[] options;
}
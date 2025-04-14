using UnityEngine;

[CreateAssetMenu(fileName ="Checkpoint", menuName = "ScriptableObjects/Checkpoint", order = 1)]
public class Checkpoint : ScriptableObject
{
    public string name;
    public bool isComplated = false;
}

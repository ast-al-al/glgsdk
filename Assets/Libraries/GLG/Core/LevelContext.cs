using GLG;
using UnityEngine;

public class LevelContext : MonoBehaviour
{
    [SerializeField] private LevelController _levelController;
    [SerializeField] private Unit _player;
    public LevelController LevelController => _levelController;

    public Unit Player => _player;

    private void Awake()
    {
        Kernel.UpdateLevelContext(this);
    }
}

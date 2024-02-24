using GLG.UI;
using UnityEngine;

public class InGameScreen : UIController//, IManaged
{
    [SerializeField] private MoneyBlock _money;

    public MoneyBlock Money => _money;
    public Vector3 CrosshairTarget { get; set; }
}

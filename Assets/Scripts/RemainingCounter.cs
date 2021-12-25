using System.Collections.Generic;
using UnityEngine;

public class RemainingCounter : MonoBehaviour
{
    [SerializeField] private List<Tadashi> tadashis;

    public void UpdateCount(int count)
    {
        foreach (var (tadashi, index) in tadashis.WithIndex())
        {
            tadashi.SetActive(Tadashi.Direction.Top, count > index * 5);
            tadashi.SetActive(Tadashi.Direction.Center, count > index * 5 + 1);
            tadashi.SetActive(Tadashi.Direction.Right, count > index * 5 + 2);
            tadashi.SetActive(Tadashi.Direction.Left, count > index * 5 + 3);
            tadashi.SetActive(Tadashi.Direction.Bottom, count > index * 5 + 4);
        }
    }
}

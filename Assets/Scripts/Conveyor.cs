using DG.Tweening;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private DOTweenAnimation doTweenAnimation;

    public void PlayAnimation()
    {
        doTweenAnimation.DORestart();
    }
}

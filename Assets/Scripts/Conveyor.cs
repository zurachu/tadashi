using DG.Tweening;
using KanKikuchi.AudioManager;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private DOTweenAnimation doTweenAnimation;

    public void PlayAnimation()
    {
        doTweenAnimation.DORestart();
        SEManager.Instance.Play(SEPath.DRIL3);
    }
}

using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TadashiParts : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async UniTask PlayAttachedAnimation()
    {
        var image = GetComponent<Image>();
        image.color = Color.red;

        var sequence = DOTween.Sequence();
        await sequence.Append(transform.DOScale(2f, 0.25f).From(1f))
                      .Join(image.DOFade(0f, 0.25f).From(1f))
                      .SetLink(gameObject);
        ;
    }
}

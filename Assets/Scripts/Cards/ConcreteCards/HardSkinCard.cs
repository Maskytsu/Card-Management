using DG.Tweening;
using UnityEngine;

public class HardSkinCard : Card
{
    public override void CardPlayAnimation()
    {
        Vector3 cardBaseScale = transform.localScale;

        Sequence animationSeq = DOTween.Sequence();
        animationSeq.Append(transform.DOScaleX(cardBaseScale.x * 1.5f, 0.5f));
        animationSeq.Append(transform.DOScale(cardBaseScale.x, 0.5f));

        animationSeq.onComplete += () =>
        {
            OnPlayAnimationEnd?.Invoke();
        };
    }
}
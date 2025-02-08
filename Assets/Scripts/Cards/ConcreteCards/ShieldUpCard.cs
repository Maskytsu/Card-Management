using DG.Tweening;
using UnityEngine;

public class ShieldUpCard : Card
{
    public override void CardPlayAnimation()
    {
        Vector3 cardBaseScale = transform.localScale;

        Sequence animationSeq = DOTween.Sequence();
        animationSeq.Append(transform.DOScale(cardBaseScale * 1.5f, 0.5f));
        animationSeq.Append(transform.DOScale(cardBaseScale, 0.5f));

        animationSeq.onComplete += () =>
        {
            OnPlayAnimationEnd?.Invoke();
        };
    }
}

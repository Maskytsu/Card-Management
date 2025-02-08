using DG.Tweening;
using UnityEngine;

public class ShieldUpCard : Card
{
    public override void CardPlayAnimation()
    {
        Vector3 cardBaseScale = CardTransform.localScale;

        Sequence animationSeq = DOTween.Sequence();
        animationSeq.Append(CardTransform.DOScale(cardBaseScale * 1.5f, 0.5f));
        animationSeq.Append(CardTransform.DOScale(cardBaseScale, 0.5f));

        animationSeq.onComplete += () =>
        {
            OnPlayAnimationEnd?.Invoke();
        };
    }
}

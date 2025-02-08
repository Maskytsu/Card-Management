using DG.Tweening;
using UnityEngine;

public class HardSkinCard : Card
{
    public override void CardPlayAnimation()
    {
        Vector3 cardBaseScale = CardTransform.localScale;

        Sequence animationSeq = DOTween.Sequence();
        animationSeq.Append(CardTransform.DOScaleX(cardBaseScale.x * 1.5f, 0.5f));
        animationSeq.Append(CardTransform.DOScale(cardBaseScale.x, 0.5f));

        animationSeq.onComplete += () =>
        {
            OnPlayAnimationEnd?.Invoke();
        };
    }
}

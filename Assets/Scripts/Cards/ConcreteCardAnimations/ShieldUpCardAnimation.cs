using DG.Tweening;
using UnityEngine;

public class ShieldUpCardAnimation : CardAnimation
{
    public override void CardPlayAnimation(Sequence animationSeq)
    {
        Vector3 cardBaseScale = transform.localScale;

        animationSeq.Append(transform.DOScale(cardBaseScale * 1.5f, 0.5f));
        animationSeq.Append(transform.DOScale(cardBaseScale, 0.5f));
    }
}
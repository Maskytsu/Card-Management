using DG.Tweening;
using UnityEngine;

public class ShieldUpCardAnimation : CardAnimation
{
    public override void CardPlayAnimation(Sequence animationSeq, Transform transformToAnimate)
    {
        Vector3 cardBaseScale = transformToAnimate.localScale;

        animationSeq.Append(transformToAnimate.DOScale(cardBaseScale * 1.5f, 0.5f));
        animationSeq.Append(transformToAnimate.DOScale(cardBaseScale, 0.5f));
    }
}
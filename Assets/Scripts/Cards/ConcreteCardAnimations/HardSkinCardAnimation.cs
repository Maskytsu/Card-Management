using DG.Tweening;
using UnityEngine;

public class HardSkinCardAnimation : CardAnimation
{
    public override void CardPlayAnimation(Sequence animationSeq, Transform transformToAnimate)
    {
        Vector3 cardBaseScale = transformToAnimate.localScale;

        animationSeq.Append(transformToAnimate.DOScaleX(cardBaseScale.x * 1.5f, 0.5f));
        animationSeq.Append(transformToAnimate.DOScale(cardBaseScale.x, 0.5f));
    }
}
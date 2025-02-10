using DG.Tweening;
using UnityEngine;

public class ActivateCardAnimation : CardAnimation
{
    public override void CardPlayAnimation(Sequence animationSeq, Transform transformToAnimate)
    {
        animationSeq.Append(transformToAnimate.DORotate(new Vector3(0, 0, 360f), 0.75f, RotateMode.FastBeyond360));
    }
}
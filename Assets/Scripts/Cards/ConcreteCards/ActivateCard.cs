using DG.Tweening;
using UnityEngine;

public class ActivateCard : Card
{
    protected override void CardPlayAnimation(Sequence animationSeq)
    {
        animationSeq.Append(transform.DORotate(new Vector3(0, 0, 360f), 0.75f, RotateMode.FastBeyond360));
    }
}
using DG.Tweening;
using UnityEngine;

public class SubZeroCardAnimation : CardAnimation
{
    public override void CardPlayAnimation(Sequence animationSeq)
    {
        Vector3 cardBaseScale = transform.localScale;

        animationSeq.Append(transform.DOScale(cardBaseScale / 2, 0.5f));
        animationSeq.Append(transform.DOScale(cardBaseScale, 0.5f));
    }
}
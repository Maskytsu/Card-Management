using DG.Tweening;
using UnityEngine;

public class HardSkinCard : Card
{
    protected override void CardPlayAnimation(Sequence animationSeq)
    {
        Vector3 cardBaseScale = transform.localScale;

        animationSeq.Append(transform.DOScaleX(cardBaseScale.x * 1.5f, 0.5f));
        animationSeq.Append(transform.DOScale(cardBaseScale.x, 0.5f));
    }
}
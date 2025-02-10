using DG.Tweening;
using UnityEngine;

public abstract class CardAnimation : MonoBehaviour
{
    /// <summary>
    /// Animation should be created as part of given <paramref name="animationSeq"/> on the <paramref name="transformToAnimate"/>.
    /// </summary>
    public abstract void CardPlayAnimation(Sequence animationSeq, Transform transformToAnimate);
}
using DG.Tweening;
using UnityEngine;

public abstract class CardAnimation : MonoBehaviour
{
    /// <summary>
    /// Animation should be created as part of given Sequence <paramref name="animationSeq"/>.
    /// </summary>
    public abstract void CardPlayAnimation(Sequence animationSeq, Transform transformToAnimate);
}
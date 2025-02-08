using DG.Tweening;
using UnityEngine;

public class ActivateCard : Card
{
    public override void CardPlayAnimation()
    {
        Tween rotateTween = transform.DORotate(new Vector3(0, 0, 360f), 0.75f, RotateMode.FastBeyond360);

        rotateTween.onComplete += () =>
        {
            OnPlayAnimationEnd?.Invoke();
        };
    }
}

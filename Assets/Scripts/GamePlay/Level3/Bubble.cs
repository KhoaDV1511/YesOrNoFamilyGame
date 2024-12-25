using System;
using System.Linq;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
    [SerializeField] private Image imgBubble, imgItem;
    [SerializeField] private SkeletonGraphic effectBubble;
    [SerializeField] private Button btnBubble;
    private Tween _twShowItem, _twShowEffect, _twTimeShowItem;
    private Action<int> _poke;
    private Action<TypePlayThreeChoose> _animPoke;
    public TypePlayThreeChoose typePlayThreeChoose;
    public bool isChoose;

    private void Start()
    {
        btnBubble.onClick.AddListener(Poke);
    }

    public void Init( Action<int> poke, Action<TypePlayThreeChoose> animPoke)
    {
        isChoose = true;
        _poke = poke;
        _animPoke = animPoke;
        imgBubble.Show();
        imgBubble.Show();
        effectBubble.Hide();
        imgItem.Hide();
    }

    private void Poke()
    {
        if(!isChoose) return;
        _animPoke?.Invoke(typePlayThreeChoose);
        _twShowEffect?.Kill();
        _twShowEffect = DOVirtual.DelayedCall(0.6f, Effect);
    }
    public void Effect()
    {
        imgBubble.Hide();
        effectBubble.Show();
        SetAnimBubble(false);
    }

    private void SetAnimBubble(bool loop)
    {
        effectBubble.startingAnimation = effectBubble.SkeletonData.Animations.Items.First().Name;
        effectBubble.startingLoop = loop;
        effectBubble.Initialize(true);
        _twShowItem?.Kill();
        _twShowItem = DOVirtual.DelayedCall(effectBubble.SkeletonData.Animations.Items.First().Duration,
            () =>
            {
                _twTimeShowItem?.Kill();
                _twTimeShowItem = DOVirtual.DelayedCall(2.667f, imgItem.Hide);
                imgItem.Show();
                imgItem.SetNativeSize();
                _poke?.Invoke((int)typePlayThreeChoose);
            });
    }
}
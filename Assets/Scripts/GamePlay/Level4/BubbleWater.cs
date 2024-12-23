using System;
using System.Linq;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BubbleWater : MonoBehaviour
{
    [SerializeField] private SkeletonGraphic effectBubble, bubble;
    [SerializeField] private Button btnBubble;
    private Tween _twShowEffect;
    private Action<int> _poke;
    private Action<ItemAnimFour> _animPoke;
    public TypePlayFourChoose typePlayFourChoose;
    public ItemAnimFour typePoke;
    public bool isChoose;
    
    private void Start()
    {
        btnBubble.onClick.AddListener(Poke);
    }

    public void Init(Action<int> poke, Action<ItemAnimFour> animPoke)
    {
        isChoose = true;
        _poke = poke;
        _animPoke = animPoke;
        bubble.Show();
        bubble.Show();
        effectBubble.Hide();
    }

    private void Poke()
    {
        if(!isChoose) return;
        Debug.Log("poke");
        _animPoke?.Invoke(typePoke);
        _twShowEffect?.Kill();
        _twShowEffect = DOVirtual.DelayedCall(0.6f, Effect);
    }
    public void Effect()
    {
        bubble.Hide();
        effectBubble.Show();
        SetAnimBubble(false);
    }

    private void SetAnimBubble(bool loop)
    {
        effectBubble.startingAnimation = effectBubble.SkeletonData.Animations.Items.First().Name;
        effectBubble.startingLoop = loop;
        effectBubble.Initialize(true);
        _poke?.Invoke((int)typePlayFourChoose);
    }
}
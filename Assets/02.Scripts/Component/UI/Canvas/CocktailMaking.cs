using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocktailMaking : UIBase_Popup
{
    void Start() => Init();

    [SerializeField] ShakeCocktail shake;
    public override void Init()
    {
        base.Init();

        if(shake == null) shake = gameObject.FindChild<ShakeCocktail>("MakingImage");

        shake.OnMakingEnd -= OnEnd;
        shake.OnMakingEnd += OnEnd;

        GameManager.Sound.Play(Define.SoundType.LoopFX, "cocktail_making");
    }

    public Action OnEndMaking = () => { };

    void OnEnd()
    {
        shake.OnMakingEnd -= OnEnd;
        OnEndMaking();
        OnEndMaking = null;
        GameManager.Sound.Stop(Define.SoundType.LoopFX);
        GameManager.UI.ClosePopupUI<CocktailMaking>();
    }
}

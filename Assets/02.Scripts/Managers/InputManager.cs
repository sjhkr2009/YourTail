using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager
{
    public event Action<string> InputMaterialSelect;
    public event Action<CocktailMaterials> InputMaterialInfo;
    public event Action<Customers> InputBirdInfo;
    public event Action<GameState> InputStateChange;
    public event Action InputRetryCocktail;
    public event Action InputEscape;

    public void InMaterialSelect(string id) => InputMaterialSelect?.Invoke(id);
    public void InMaterialInfo(CocktailMaterials material) => InputMaterialInfo?.Invoke(material);
    public void InBirdInfo(Customers customer) => InputBirdInfo?.Invoke(customer);
    public void InStateChange(GameState state) => InputStateChange?.Invoke(state);
    public void InRetryCocktail() => InputRetryCocktail?.Invoke();
    public void InEscape() => InputEscape?.Invoke();
    public void OnUpdate()
    {
        if (Input.anyKeyDown)
            GameManager.Sound.Play(Define.SoundType.FX, "click");

        if (Input.GetKeyDown(KeyCode.Escape))
            InputEscape?.Invoke();

        if (Input.GetKeyDown(KeyCode.T)) // Test
            GameManager.UI.OpenPopupUI<TutorialUI>();
    }
    public void Clear()
    {
        InputMaterialSelect = null;
        InputMaterialInfo = null;
        InputBirdInfo = null;
        InputStateChange = null;
        InputRetryCocktail = null;
        InputEscape = null;
    }
}

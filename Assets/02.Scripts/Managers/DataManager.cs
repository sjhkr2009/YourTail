using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public List<string> RecipeCollection { get; private set; } = new List<string>();
    
    public Dictionary<string, int> CustomerLevel { get; private set; } = new Dictionary<string, int>() { };
    public Dictionary<string, int> CustomerExp { get; private set; } = new Dictionary<string, int>() { };
    int CustomerCount => CustomerLevel.Count;

    public int Birdcoin {
        get => PlayerPrefs.GetInt("BIRD_COIN_COUNT", 0);
        set => PlayerPrefs.SetInt("BIRD_COIN_COUNT", value);
    }
    public List<string> CollectedRecipe { get; private set; } = new List<string>();
    int RecipeCount => CollectedRecipe.Count;
    public List<string> CollectedSubmaterial { get; private set; } = new List<string>();
    int SubmaterialCount => CollectedSubmaterial.Count;

    #region Set Data
    public void SetLevel(string name, int level)
    {
        if (CustomerLevel.ContainsKey(name)) CustomerLevel[name] = level;
        else CustomerLevel.Add(name, level);
    }
    public void SetExp(string name, int exp)
    {
        if (CustomerLevel.ContainsKey(name)) CustomerLevel[name] = exp;
        else CustomerLevel.Add(name, exp);
    }
    public void AddBirdcoin(int value) => Birdcoin += value;

    public void AddRecipe(string id) {
        if (CollectedRecipe.Contains(id) == false) 
            CollectedRecipe.Add(id);
    }

    public void AddMaterial(string id) {
        if(CollectedSubmaterial.Contains(id) == false)
            CollectedSubmaterial.Add(id);
    }
    #endregion

    #region Save Data
    public void Save()
    {
        SaveLevel();
        SaveExp();
        SaveRecipe();
        SaveMaterials();
    }
    void SaveLevel()
    {
        int count = 0;
        foreach (var item in CustomerLevel)
        {
            PlayerPrefs.SetInt(item.Key, item.Value);
            PlayerPrefs.SetString($"Customer{count}Level", item.Key);
            count++;
        }
        PlayerPrefs.SetInt(nameof(CustomerCount), CustomerCount);
    }
    void SaveExp()
    {
        int count = 0;
        foreach (var item in CustomerExp)
        {
            PlayerPrefs.SetInt(item.Key, item.Value);
            PlayerPrefs.SetString($"Customer{count}Exp", item.Key);
            count++;
        }
        PlayerPrefs.SetInt(nameof(CustomerCount), CustomerCount);
    }
    void SaveRecipe()
    {
        for (int i = 0; i < RecipeCount; i++)
        {
            PlayerPrefs.SetString($"{nameof(CollectedRecipe)}{i}", CollectedRecipe[i]);
        }
        PlayerPrefs.SetInt(nameof(RecipeCount), RecipeCount);
    }
    void SaveMaterials()
    {
        for (int i = 0; i < SubmaterialCount; i++)
        {
            PlayerPrefs.SetString($"{nameof(CollectedSubmaterial)}{i}", CollectedSubmaterial[i]);
        }
        PlayerPrefs.SetInt(nameof(SubmaterialCount), SubmaterialCount);
    }

    #endregion

    #region Load Data
    public void Load()
    {
        LoadLevel();
        LoadExp();
        LoadRecipe();
        LoadMaterials();
    }

    void LoadLevel()
    {
        int count = PlayerPrefs.GetInt(nameof(CustomerCount));
        CustomerLevel.Clear();
        for (int i = 0; i < count; i++)
        {
            string key = PlayerPrefs.GetString($"Customer{i}Level");
            if (string.IsNullOrEmpty(key)) continue;

            int value = PlayerPrefs.GetInt(key);
            CustomerLevel.Add(key, value);
        }
    }
    void LoadExp()
    {
        int count = PlayerPrefs.GetInt(nameof(CustomerCount));
        CustomerExp.Clear();
        for (int i = 0; i < count; i++)
        {
            string key = PlayerPrefs.GetString($"Customer{i}Exp");
            if (string.IsNullOrEmpty(key)) continue;
            int value = PlayerPrefs.GetInt(key);
            CustomerExp.Add(key, value);
        }
    }
    void LoadRecipe()
    {
        int count = PlayerPrefs.GetInt(nameof(RecipeCount));
        CollectedRecipe.Clear();
        for (int i = 0; i < count; i++)
        {
            string id = PlayerPrefs.GetString($"{nameof(CollectedRecipe)}{i}");
            CollectedRecipe.Add(id);
        }
    }
    void LoadMaterials()
    {
        int count = PlayerPrefs.GetInt(nameof(SubmaterialCount));
        CollectedSubmaterial.Clear();
        for (int i = 0; i < count; i++)
        {
            string id = PlayerPrefs.GetString($"{nameof(CollectedSubmaterial)}{i}");
            CollectedSubmaterial.Add(id);
        }
    }
    #endregion

    public bool AddRecipe(Cocktail cocktail)
	{
		string id = cocktail.Id;

		if (cocktail.IsDefault || RecipeCollection.Contains(id))
			return false;

		RecipeCollection.Add(id);
		return true;
	}
}

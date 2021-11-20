using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InventoryData : MonoBehaviour
{
    public static InventoryData INSTANCE;
    public Dictionary<string, int> equippables; // Haven't equipped (in storage)
    public Dictionary<string, int> equipments;  // Already equipped
    public Dictionary<string, int> consumeables;

    const string saveName = "/inventory-data.json";

    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            INSTANCE = this;
            DontDestroyOnLoad(gameObject);
            LoadInventoryData();
        }
    }

    public void BuyConsumeable(string consumable, int amount)
    {
        if (consumeables.ContainsKey(consumable))
            consumeables[consumable] += amount;
        else
            consumeables.Add(consumable, amount);
    }

    public void SellConsumeable(string consumable, int amount)
    {
        if (consumeables.ContainsKey(consumable))
        {
            consumeables[consumable] -= amount;
            if (consumeables[consumable] == 0)
                consumeables.Remove(consumable);
        }
    }

    public void Consume(string consumable)
    {
        if (consumeables.ContainsKey(consumable))
        {
            consumeables[consumable]--;
            if (consumeables[consumable] == 0)
                consumeables.Remove(consumable);
        }
    }

    public void BuyEquippable(string equippable, int amount)
    {
        if (equippables.ContainsKey(equippable))
            equippables[equippable] += amount;
        else
            equippables.Add(equippable, amount);
    }

    public void SellEquippable(string equippable, int amount)
    {
        if (equippables.ContainsKey(equippable))
        {
            equippables[equippable] -= amount;
            if (equippables[equippable] == 0)
                equippables.Remove(equippable);
        }
    }

    public void EquipEquippable(string equippable)
    {
        if (equippables.ContainsKey(equippable))
        {
            equippables[equippable]--;
            if (equippables[equippable] == 0)
                equippables.Remove(equippable);

            if (equipments.ContainsKey(equippable))
                equipments[equippable]++;
            else
                equipments.Add(equippable, 1);
        }
    }

    public void UnEquipEquipment(string equipment)
    {
        if (equipments.ContainsKey(equipment))
        {
            equipments[equipment]--;
            if (equipments[equipment] == 0)
                equipments.Remove(equipment);

            if (equippables.ContainsKey(equipment))
                equippables[equipment]++;
            else
                equippables.Add(equipment, 1);
        }
    }

    public void SaveInventoryData()
    {
        string jsonEquippables = "";
        foreach (KeyValuePair<string, int> dict in equippables)
        {
            EquippableData data = new EquippableData();
            data.equippableName = dict.Key;
            data.amount = dict.Value;
            data.isEquipped = false;
            string json = JsonUtility.ToJson(data);
            jsonEquippables += json + ",";
        }

        string jsonEquipments = "";
        foreach (KeyValuePair<string, int> dict in equipments)
        {
            EquippableData data = new EquippableData();
            data.equippableName = dict.Key;
            data.amount = dict.Value;
            data.isEquipped = true;
            string json = JsonUtility.ToJson(data);
            jsonEquipments += json + ",";
        }

        string jsonConsumeables = "";
        foreach (KeyValuePair<string, int> dict in consumeables)
        {
            ConsumableData data = new ConsumableData();
            data.consumableName = dict.Key;
            data.amount = dict.Value;
            string json = JsonUtility.ToJson(data);
            jsonConsumeables += json + ",";
        }

        string allData = jsonEquippables + "~" + jsonEquipments + "~" + jsonConsumeables;
        File.WriteAllText(Application.persistentDataPath + saveName, allData);
    }

    void LoadInventoryData()
    {
        equippables = new Dictionary<string, int>();
        equipments = new Dictionary<string, int>();
        consumeables = new Dictionary<string, int>();

        string path = Application.persistentDataPath + saveName;
        if (File.Exists(path))
        {
            string allData = File.ReadAllText(path);

            string[] json = allData.Split('~');

            // Index 0 for equippables
            string[] equippablesData = json[0].Split(',');
            for (int i = 0; i < equippablesData.Length - 1; ++i)
            {
                EquippableData data = JsonUtility.FromJson<EquippableData>(equippablesData[i]);
                equippables.Add(data.equippableName, data.amount);
            }

            // Index 1 for equipments
            string[] equipmentsData = json[1].Split(',');
            for (int i = 0; i < equipmentsData.Length - 1; ++i)
            {
                EquippableData data = JsonUtility.FromJson<EquippableData>(equipmentsData[i]);
                equipments.Add(data.equippableName, data.amount);
            }

            // Index 2 for consumeables
            string[] consumeablesData = json[2].Split(',');
            for (int i = 0; i < consumeablesData.Length - 1; ++i)
            {
                ConsumableData data = JsonUtility.FromJson<ConsumableData>(consumeablesData[i]);
                consumeables.Add(data.consumableName, data.amount);
            }
        }
    }

    [System.Serializable]
    class ConsumableData
    {
        public string consumableName;
        public int amount;
    }

    [System.Serializable]
    class EquippableData
    {
        public string equippableName;
        public int amount;
        public bool isEquipped;
    }
}

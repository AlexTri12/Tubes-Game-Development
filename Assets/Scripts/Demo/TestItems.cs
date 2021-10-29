using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItems : MonoBehaviour
{
    List<GameObject> inventory = new List<GameObject>();
    List<GameObject> combatants = new List<GameObject>();

    private void Start()
    {
        CreateItems();
        CreateCombatants();
        StartCoroutine(SimulateBattle());
    }

    private void OnEnable()
    {
        this.AddObserver(OnEquippedItem, Equipment.EquippedNotification);
        this.AddObserver(OnUnEquippedItem, Equipment.UnEquippedNotification);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnEquippedItem, Equipment.EquippedNotification);
        this.RemoveObserver(OnUnEquippedItem, Equipment.UnEquippedNotification);
    }

    void OnEquippedItem(object sender, object args)
    {
        Equipment eq = sender as Equipment;
        Equippable item = args as Equippable;
        inventory.Remove(item.gameObject);
        Debug.Log(string.Format("{0} equipped {1}", eq.name, item.name));
    }

    void OnUnEquippedItem(object sender, object args)
    {
        Equipment eq = sender as Equipment;
        Equippable item = args as Equippable;
        inventory.Add(item.gameObject);
        Debug.Log(string.Format("{0} un-equipped {1}", eq.name, item.name));
    }

    GameObject CreateItem(string title, StatsTypes type, int amount)
    {
        GameObject item = new GameObject(title);
        StatModifierFeature smf = item.AddComponent<StatModifierFeature>();
        smf.type = type;
        smf.amount = amount;
        return item;
    }

    GameObject CreateConsumableItem(string title, StatsTypes type, int amount)
    {
        GameObject item = CreateItem(title, type, amount);
        item.AddComponent<Consumeable>();
        return item;
    }

    GameObject CreateEquippableItem(string title, StatsTypes type, int amount, EquipSlots slot)
    {
        GameObject item = CreateItem(title, type, amount);
        Equippable equip = item.AddComponent<Equippable>();
        equip.defaultSlots = slot;
        return item;
    }

    GameObject CreateActor(string title)
    {
        GameObject actor = new GameObject(title);
        Stats s = actor.AddComponent<Stats>();
        s[StatsTypes.HP] = s[StatsTypes.MHP] = UnityEngine.Random.Range(500, 1000);
        s[StatsTypes.ATK] = UnityEngine.Random.Range(30, 50);
        s[StatsTypes.DEF] = UnityEngine.Random.Range(30, 50);
        return actor;
    }

    GameObject CreateHero()
    {
        GameObject actor = CreateActor("Hero");
        actor.AddComponent<Equipment>();
        return actor;
    }

    void CreateItems()
    {
        inventory.Add(CreateConsumableItem("Health Potion", StatsTypes.HP, 300));
        inventory.Add(CreateConsumableItem("Bomb", StatsTypes.HP, -150));
        inventory.Add(CreateEquippableItem("Sword", StatsTypes.ATK, 10, EquipSlots.Primary));
        inventory.Add(CreateEquippableItem("Board Sword", StatsTypes.ATK, 15, EquipSlots.Primary | EquipSlots.Secondary));
        inventory.Add(CreateEquippableItem("Shield", StatsTypes.DEF, 10, EquipSlots.Secondary));
    }

    void CreateCombatants()
    {
        combatants.Add(CreateHero());
        combatants.Add(CreateActor("Monster"));
    }

    IEnumerator SimulateBattle()
    {
        while (VictoryCheck() == false)
        {
            LogCombatants();
            HeroTurn();
            EnemyTurn();
            yield return new WaitForSeconds(1);
        }
        LogCombatants();
        Debug.Log("Battle Completed");
    }

    void HeroTurn()
    {
        int rnd = UnityEngine.Random.Range(0, 2);
        switch (rnd)
        {
            case 0:
                Attack(combatants[0], combatants[1]);
                break;
            default:
                UseInventory();
                break;
        }
    }

    void EnemyTurn()
    {
        Attack(combatants[1], combatants[0]);
    }

    void Attack(GameObject attacker, GameObject defender)
    {
        Stats s1 = attacker.GetComponent<Stats>();
        Stats s2 = defender.GetComponent<Stats>();

        int damage = Mathf.FloorToInt(((s1[StatsTypes.ATK] * 4) - (s1[StatsTypes.DEF] * 2)) * UnityEngine.Random.Range(0.9f, 1.0f));
        s2[StatsTypes.HP] -= damage;

        Debug.Log(string.Format("{0} hits {1} for {2} damage!", attacker.name, defender.name, damage));
    }

    void UseInventory()
    {
        int rnd = UnityEngine.Random.Range(0, inventory.Count);

        GameObject item = inventory[rnd];
        if (item.GetComponent<Consumeable>() != null)
            ConsumeItem(item);
        else
            EquipItem(item);
    }

    void ConsumeItem(GameObject item)
    {
        inventory.Remove(item);

        StatModifierFeature smf = item.GetComponent<StatModifierFeature>();
        if (smf.amount > 0)
        {
            item.GetComponent<Consumeable>().Consume(combatants[0]);
            Debug.Log("Ah... a potion!");
        }
        else
        {
            item.GetComponent<Consumeable>().Consume(combatants[1]);
            Debug.Log("Take this you stupid monster!");
        }
    }

    void EquipItem(GameObject item)
    {
        Debug.Log("Perhaps this will help...");
        Equippable toEquip = item.GetComponent<Equippable>();
        Equipment equipment = combatants[0].GetComponent<Equipment>();
        equipment.Equip(toEquip, toEquip.defaultSlots);
    }

    bool VictoryCheck()
    {
        for (int i = 0; i < combatants.Count; ++i)
        {
            Stats s = combatants[i].GetComponent<Stats>();
            if (s[StatsTypes.HP] <= 0)
                return true;
        }
        return false;
    }

    void LogCombatants()
    {
        Debug.Log("============");
        for (int i = 0; i < combatants.Count; ++i)
            LogToConsole(combatants[i]);
        Debug.Log("============");
    }

    void LogToConsole(GameObject actor)
    {
        Stats s = actor.GetComponent<Stats>();
        Debug.Log(string.Format("Name: {0} HP: {1}/{2} ATK: {3} DEF: {4}", actor.name, s[StatsTypes.HP], s[StatsTypes.MHP], s[StatsTypes.ATK], s[StatsTypes.DEF]));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {
    
    public string name {
        get;
        private set;
    }

    public int attack {
        get;
        private set;
    }

    public int defense {
        get;
        private set;
    }

    public int agility {
        get;
        private set;
    }

    public int luck {
        get;
        private set;
    }

    public Inventory inv {
        get;
        private set;
    }

    public EquipmentInv equipment {
        get;
        private set;
    }

    public Character() {
        name = "Charlie";
        attack = 1;
        defense = 1;
        agility = 1;
        luck = 1;
        inv = new Inventory();
        Debug.Log("Character Created");
    }
}

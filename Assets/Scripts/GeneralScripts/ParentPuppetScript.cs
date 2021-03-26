using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public interface ParentPuppetScript {

    void setCurrentActionPointCount(int newCount);

    int getCurrentActionPointCount();

    string getPuppetName();

    int getModifiedStrength();

    int getModifiedMeleeAttackCount();

}

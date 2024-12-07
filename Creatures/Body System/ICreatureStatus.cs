using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Creatures have needs they must satisfy in order to live and maintain their abilities
 */
public interface ICreatureStatus 
{
    void UpdateNeeds();
    bool CheckCanExist();
    bool CheckCanBeAlive();
    bool CheckCanBeConcious();
    bool CheckCanMoveLimbs();
    bool CheckCanMove();
}

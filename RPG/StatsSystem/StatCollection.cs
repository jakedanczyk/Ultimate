using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Urth
{

    /// <summary>
    /// The base class used to define a collection of Stats.
    /// Also used to apply and remove StatModifiers from individual
    /// Stats.
    /// </summary>
    public class StatCollection
    {
        private Dictionary<StatType, Stat> _statDict;

        /// <summary>
        /// Dictionary containing all stats held within the collection
        /// </summary>
        public Dictionary<StatType, Stat> StatDict
        {
            get
            {
                if (_statDict == null)
                {
                    _statDict = new Dictionary<StatType, Stat>();
                }
                return _statDict;
            }
        }

        /// <summary>
        /// Initializes the Stats class
        /// </summary>
        private void Awake()
        {
            //if (LevelSerializer.IsDeserializing) return;

            ConfigureStats();
        }

        /// <summary>
        /// Overridable method used to create and setup the stats contained within
        /// the Stats class: StatLinkers, Stat Default Values, Etc.
        /// </summary>
        public virtual void ConfigureStats() { }

        /// <summary>
        /// Checks if there is a Stat with the given type and id
        /// </summary>
        public bool ContainStat(StatType statType)
        {
            return StatDict.ContainsKey(statType);
        }

        /// <summary>
        /// Gets the Stat with the given StatTyp and ID
        /// </summary>
        public Stat GetStat(StatType statType)
        {
            if (ContainStat(statType))
            {
                return StatDict[statType];
            }
            return null;
        }

        /// <summary>
        /// Gets the Stat with the given StatType and ID as type T
        /// </summary>
        public T GetStat<T>(StatType type) where T : Stat
        {
            return GetStat(type) as T;
        }

        /// <summary>
        /// Creates a new instance of the stat ands adds it to the StatDict
        /// </summary>
        protected T CreateStat<T>(StatType statType) where T : Stat
        {
            T stat = System.Activator.CreateInstance<T>();
            StatDict.Add(statType, stat);
            return stat;
        }

        /// <summary>
        /// Creates or Gets a Stat of type T. Used within the setup method during initialization.
        /// </summary>
        protected T CreateOrGetStat<T>(StatType statType) where T : Stat
        {
            T stat = GetStat<T>(statType);
            if (stat == null)
            {
                stat = CreateStat<T>(statType);
            }
            return stat;
        }


        /// <summary>
        /// Adds a Stat Modifier to the Target stat.
        /// </summary>
        public void AddStatModifier(StatType target, StatModifier mod)
        {
            AddStatModifier(target, mod, false);
        }

        /// <summary>
        /// Adds a Stat Modifier to the Target stat and then updates the stat's value.
        /// </summary>
        public void AddStatModifier(StatType target, StatModifier mod, bool update)
        {
            if (ContainStat(target))
            {
                var modStat = GetStat(target) as IStatModifiable;
                if (modStat != null)
                {
                    modStat.AddModifier(mod);
                    if (update == true)
                    {
                        modStat.UpdateModifiers();
                    }
                }
                else
                {
                    Debug.Log("[Stats] Trying to add Stat Modifier to non modifiable stat \"" + target.ToString() + "\"");
                }
            }
            else
            {
                Debug.Log("[Stats] Trying to add Stat Modifier to \"" + target.ToString() + "\", but Stats does not contain that stat");
            }
        }

        /// <summary>
        /// Removes a Stat Modifier to the Target stat.
        /// </summary>
        public void RemoveStatModifier(StatType target, StatModifier mod)
        {
            RemoveStatModifier(target, mod, false);
        }

        /// <summary>
        /// Removes a Stat Modifier to the Target stat and then updates the stat's value.
        /// </summary>
        public void RemoveStatModifier(StatType target, StatModifier mod, bool update)
        {
            if (ContainStat(target))
            {
                var modStat = GetStat(target) as IStatModifiable;
                if (modStat != null)
                {
                    modStat.RemoveModifier(mod);
                    if (update == true)
                    {
                        modStat.UpdateModifiers();
                    }
                }
                else
                {
                    Debug.Log("[Stats] Trying to remove Stat Modifier from non modifiable stat \"" + target.ToString() + "\"");
                }
            }
            else
            {
                Debug.Log("[Stats] Trying to remove Stat Modifier from \"" + target.ToString() + "\", but StatCollection does not contain that stat");
            }
        }

        /// <summary>
        /// Clears all stat modifiers from all stats in the collection.
        /// </summary>
        public void ClearStatModifiers()
        {
            ClearStatModifiers(false);
        }

        /// <summary>
        /// Clears all stat modifiers from all stats in the collection then updates all the stat's values.
        /// </summary>
        /// <param name="update"></param>
        public void ClearStatModifiers(bool update)
        {
            foreach (var key in StatDict.Keys)
            {
                ClearStatModifier(key, update);
            }
        }

        /// <summary>
        /// Clears all stat modifiers from the target stat.
        /// </summary>
        public void ClearStatModifier(StatType target)
        {
            ClearStatModifier(target, false);
        }

        /// <summary>
        /// Clears all stat modifiers from the target stat then updates the stat's value.
        /// </summary>
        public void ClearStatModifier(StatType target, bool update)
        {
            if (ContainStat(target))
            {
                var modStat = GetStat(target) as IStatModifiable;
                if (modStat != null)
                {
                    modStat.ClearModifiers();
                    if (update == true)
                    {
                        modStat.UpdateModifiers();
                    }
                }
                else
                {
                    Debug.Log("[Stats] Trying to clear Stat Modifiers from non modifiable stat \"" + target.ToString() + "\"");
                }
            }
            else
            {
                Debug.Log("[Stats] Trying to clear Stat Modifiers from \"" + target.ToString() + "\", but StatCollection does not contain that stat");
            }
        }

        /// <summary>
        /// Updates all stat modifier's values
        /// </summary>
        public void UpdateStatModifiers()
        {
            foreach (var key in StatDict.Keys)
            {
                UpdateStatModifer(key);
            }
        }

        /// <summary>
        /// Updates the target stat's modifier value
        /// </summary>
        public void UpdateStatModifer(StatType target)
        {
            if (ContainStat(target))
            {
                var modStat = GetStat(target) as IStatModifiable;
                if (modStat != null)
                {
                    modStat.UpdateModifiers();
                }
                else
                {
                    Debug.Log("[Stats] Trying to Update Stat Modifiers for a non modifiable stat \"" + target.ToString() + "\"");
                }
            }
            else
            {
                Debug.Log("[Stats] Trying to Update Stat Modifiers for \"" + target.ToString() + "\", but StatCollection does not contain that stat");
            }
        }

        /// <summary>
        /// Scales all stats in the collection to the same target level
        /// </summary>
        public void ScaleStatCollection(int level)
        {
            foreach (var key in StatDict.Keys)
            {
                ScaleStat(key, level);
            }
        }

        /// <summary>
        /// Scales the target stat in the collection to the target level
        /// </summary>
        public void ScaleStat(StatType target, int level)
        {
            if (ContainStat(target))
            {
                var stat = GetStat(target) as IStatScalable;
                if (stat != null)
                {
                    stat.ScaleStat(level);
                }
                else
                {
                    Debug.Log("[Stats] Trying to Scale Stat with a non scalable stat \"" + target.ToString() + "\"");
                }
            }
            else
            {
                Debug.Log("[Stats] Trying to Scale Stat for \"" + target.ToString() + "\", but StatCollection does not contain that stat");
            }
        }
    }
}
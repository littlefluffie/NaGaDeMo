using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaGaDeMo
{
    public enum TargetType
    {
        None,
        Self,
        Single,
        Multiple
    }

    public delegate void ActionDelegate(Character Character = null, List<XNAObject> Targets = null);

    // Maybe for Spells? Actions? Dunno 
    public abstract class Action
    {
        public TargetType TargetType { get; set; }

        public int APCost;

        public ActionDelegate Method;

        public virtual void Resolve(Character character = null, List<XNAObject> targets = null)
        {
            Method(character, targets);
        }
    }

    public class Attack : Action
    {
        
    }

    public class Ability : Action
    {

    }

    public class Spell : Action
    {
        public int BaseManaCost { get; set; }

        public string SpellName { get; set; }

        public int Range { get; set; }

    }
}

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

    public delegate void SpellDelegate(Character Caster = null, List<XNAObject> Targets = null);

    // Maybe for Spells? Actions? Dunno 
    public abstract class Action
    {

    }

    public class Spell
    {
        public TargetType TargetType { get; set; }

        public int BaseManaCost { get; set; }

        public string SpellName { get; set; }

        public int Range { get; set; }

        public SpellDelegate Method { get; set; }

        public void Resolve(Character caster = null, List<XNAObject> targets = null)
        {
            Method(caster, targets);
        }
    }
}

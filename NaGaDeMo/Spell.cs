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

    public interface Castable
    {
        void Resolve(Character Caster, List<XNAObject> Targets);
    }

    public delegate void SpellDelegate(Character Caster = null, List<XNAObject> Targets = null);

    public class Spell : Castable
    {
        public TargetType TargetType { get; set; }

        public int BaseManaCost { get; set; }
        
        public string SpellName { get; set; }

        public SpellDelegate Method { get; set; }

        public void Resolve(Character caster = null, List<XNAObject> targets = null)
        {
            Method(caster, targets);
        }
    }
}

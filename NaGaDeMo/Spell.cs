using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaGaDeMo
{
    public interface Castable
    {
        void Resolve(Targetable target);
    }

    public delegate void SpellDelegate(Character Caster = null, List<Targetable> Targets = null);

    public class Spell : Castable
    {
        private SpellDelegate SpellName;

        public void Resolve(Targetable target)
        {
            throw new NotImplementedException();
        }
    }
}

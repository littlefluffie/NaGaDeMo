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

  
    public class Spell : Castable
    {
        public void Resolve(Targetable target)
        {
            throw new NotImplementedException();
        }
    }
}

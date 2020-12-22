using System;
using System.Collections.Generic;
using System.Text;

namespace OPP.Sound
{
    public abstract class SoundSlot
    {
        protected SoundSlot next;

        public abstract void playSound(string sound);

        public abstract void setNextChain(SoundSlot next);
    }
}

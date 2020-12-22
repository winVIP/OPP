using System;
using System.Collections.Generic;
using System.Text;

namespace OPP.Sound
{
	public class SoundMachine
	{
		SoundSlot Normal = new NormalSlot();
		SoundSlot SizeDown = new SizeDownSlot();
		SoundSlot SizeUp = new SizeUpSlot();
		SoundSlot Confused = new ConfusedSlot();
		SoundSlot PassThrough = new PassThroughSlot();

		public SoundMachine()
		{
			Normal.setNextChain(SizeDown);
			SizeDown.setNextChain(SizeUp);
			SizeUp.setNextChain(Confused);
			Confused.setNextChain(PassThrough);
		}

		public void PlaySound(string sound)
		{
			Normal.playSound(sound);

		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace OPP.Sound
{
	public class PassThroughSlot : SoundSlot
	{
		public override void playSound(string sound)
		{
			Debug.WriteLine("Sorry no sound found");
		}

		public override void setNextChain(SoundSlot next)
		{
			this.next = next;
		}
	}
}

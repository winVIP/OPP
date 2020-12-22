using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Text;

namespace OPP.Sound
{
	public class NormalSlot : SoundSlot
	{
		private string slotSound = "Normal.wav";
		public override void playSound(string sound)
		{
			if (sound == slotSound)
			{
				Debug.WriteLine("Playing Normal.wav");
				using (var soundPlayer = new SoundPlayer(@"..\..\..\Sound\Normal.wav"))
				{
					soundPlayer.Play(); // can also use soundPlayer.PlaySync()
				}
			}
			else
			{
				Debug.WriteLine("Passing request");
				next.playSound(sound);
			}
		}

		public override void setNextChain(SoundSlot next)
		{
			this.next = next;
		}
	}
}

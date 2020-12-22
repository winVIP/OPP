using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Text;

namespace OPP.Sound
{
	public class SizeUpSlot : SoundSlot
	{
		private string slotSound = "SizeUp.wav";
		public override void playSound(string sound)
		{
			if (sound == slotSound)
			{
				Debug.WriteLine("Playing SizeUp.wav");
				using (var soundPlayer = new SoundPlayer(@"..\..\..\Sound\SizeUp.wav"))
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

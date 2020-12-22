using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Text;

namespace OPP.Sound
{
	public class SizeDownSlot : SoundSlot
	{
		private string slotSound = "SizeDown.wav";
		public override void playSound(string sound)
		{
			if (sound == slotSound)
			{
				Debug.WriteLine("Playing SizeDown.wav");
				using (var soundPlayer = new SoundPlayer(@"..\..\..\Sound\SizeDown.wav"))
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

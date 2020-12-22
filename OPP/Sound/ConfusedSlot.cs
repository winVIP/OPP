using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Text;

namespace OPP.Sound
{
	public class ConfusedSlot : SoundSlot
	{
		private string slotSound = "Confused.wav";
		public override void playSound(string sound)
		{
			if (sound == slotSound)
			{
				Debug.WriteLine("Playing Confused.wav");
				using (var soundPlayer = new SoundPlayer(@"..\..\..\Sound\Confused.wav"))
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

using System;

namespace NCC.ClearView.Application.Core
{
	public class ColorLevels
	{
		public ColorLevels()
		{
		}
		public string Name(int _color)
		{
            if (_color == 1)
                return "Green";
            if (_color == 2)
                return "Bronze";
            if (_color == 3)
                return "Silver";
            if (_color == 4)
                return "Gold";
            return "";
		}
	}
}

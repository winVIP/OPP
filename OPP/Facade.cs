using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace OPP
{
    public class Facade
    {
        public Game game;
        public Map map;
        public Facade()
        {
            game = new Game();
            map = new Map();
        }

        public void FullReset()
        {
            map.ClearFood();
            map.ClearPlayers();
            game.getPlayers();
            game.Update();
        }

        public void FoodUpdate()
        {
            map.ClearFood();
            map.getFood();
            game.Update();
        }

        public void PlayersUpdate()
        {
            map.ClearPlayers();
            map.getPlayers();
            game.Update();
        }

    }
}

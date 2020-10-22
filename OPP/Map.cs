using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OPP
{
    public class Map
    {
        List<Unit> players;

        List<Unit> food;

        public Map()
        {
            players = new List<Unit>();
            food = new List<Unit>();
        }

        public void addUnit(Unit unit)
        {
            if(unit.getType() == 0)
            {
                players.Add(unit);
            }
            else
            {
                food.Add(unit);
            }
        }

        public List<Unit> getFood()
        {
            return food;
        }
        public List<Unit> getPlayers()
        {
            return players;
        }

        public Unit getFood(int i)
        {
            return food.ElementAt(i);
        }
        public Unit getPlayers(int i)
        {
            return players.ElementAt(i);
        }

        public void addFood(Unit food)
        {
            this.food.Add(food);
        }
        public void addPlayer(Unit player)
        {
            this.players.Add(player);
        }

        public void setFood(int i, Unit food)
        {
            this.food.Insert(i, food);
        }
        public void setPlayer(int i, Unit player)
        {
            this.players.Insert(i, player);
        }

        public void removeFood(int i)
        {
            food.RemoveAt(i);
        }
        public void removePlayers(int i)
        {
            players.RemoveAt(i);
        }

        public void ClearFood()
        {
            //TODO clear players also
            food.Clear();
        }

        public List<Unit> GetAllUnitsExceptMe(Color color)
        {
            List<Unit> all = new List<Unit>();
            all.Concat(food);
            foreach(Unit player in players)
            {
                if(player.getColor() != color)
                {
                    all.Add(player);
                }
            }

            return all;
        }
    }
}

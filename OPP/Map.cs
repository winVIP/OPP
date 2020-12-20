using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPP
{
    public class Map
    {
        public int i = 0;

        List<Unit> players;

        List<Unit> food;

        public List<Unit> newFood = new List<Unit>();


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
            food.index = i;
            i++;
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

        public void ClearPlayers()
        {
            players.Clear();
        }

        public void ClearFood()
        {
            food.Clear();
        }
    }
}

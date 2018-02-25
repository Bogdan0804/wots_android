using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;

namespace Wots.GamePlay.Tiles
{
    public class Door : Tile
    {
        public string id;
        public string d2;
        public string world;

        public override void Initialize(Vector2 position, string state)
        {
            id = state.Split(':')[0];
            d2 = state.Split(':')[1];
            world = state.Split(':')[2];

            this.Position = position;
            this.Collidable = false;
            this.Prefs.usePrefLeft = true;
            this.Prefs.usePrefRight = true;
            this.Prefs.OnLeft += new Func<Player, bool>(e => {
                e.useGravity = false;
                World.LoadWorld(world);
                foreach (var tile in World.Worlds[World.WorldName].Tiles)
                {
                    if (tile.Type == "door")
                    {
                        var door = (Door)tile;
                        if (door.d2 == id)
                            GameScreen.Player.PlayerSprite.Position = door.Position + new Vector2(192, -30);
                    }
                }
                e.useGravity = true;
                return true;
            });
            this.Prefs.OnRight += new Func<Player, bool>(e => {
                e.useGravity = false;
                World.LoadWorld(world);
                foreach (var tile in World.Worlds[World.WorldName].Tiles)
                {
                    if (tile.Type == "door")
                    {
                        var door = (Door)tile;
                        if (door.d2 == id)
                            GameScreen.Player.PlayerSprite.Position = door.Position + new Vector2(-192, -30);
                    }
                }
                e.useGravity = true;
                return true;
            });
        }
    }
}
﻿using PokeU.Model;
using PokeU.Model.GroundObject;
using PokeU.View.GroundObject;
using PokeU.View.ResourcesManager;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View
{
    public class LandWorld2D
    {
        public static readonly Dictionary<Type, IObject2DFactory> MappingObjectModelView;

        public static readonly TextureManager TextureManager;

        private Dictionary<ILandChunk, LandChunk2D> landChunksDictionary;

        private ChunkResourcesLoader chunkResourcesLoader;

        static LandWorld2D()
        {
            TextureManager = new TextureManager();

            MappingObjectModelView = new Dictionary<Type, IObject2DFactory>();

            MappingObjectModelView.Add(typeof(GroundLandObject), new GroundObject2DFactory());
            MappingObjectModelView.Add(typeof(LandChunk), new LandChunk2DFactory());

            foreach(IObject2DFactory factory in MappingObjectModelView.Values)
            {
                TextureManager.TextureLoaded += factory.OnTextureLoaded;
                TextureManager.TextureUnloaded += factory.OnTextureUnloaded;
            }
        }

        public LandWorld2D(LandWorld landWorld)
        {
            this.landChunksDictionary = new Dictionary<ILandChunk, LandChunk2D>();

            this.chunkResourcesLoader = new ChunkResourcesLoader();

            landWorld.ChunkAdded += OnChunkAdded;
            landWorld.ChunkRemoved += OnChunkRemoved;
        }

        public void DrawIn(RenderWindow window)
        {
            foreach (LandChunk2D landChunk2D in this.landChunksDictionary.Values)
            {
                SFML.Graphics.View view = window.GetView();

                FloatRect bounds = new FloatRect(landChunk2D.Position, new SFML.System.Vector2f(landChunk2D.Width, landChunk2D.Height));

                FloatRect boundsView = new FloatRect(view.Center.X - view.Size.X / 2, view.Center.Y - view.Size.Y / 2, view.Size.X, view.Size.Y);

                if (bounds.Intersects(boundsView))
                {
                    landChunk2D.DrawIn(window);
                    landChunk2D.DrawIn(window);
                    landChunk2D.DrawIn(window);
                    landChunk2D.DrawIn(window);
                    landChunk2D.DrawIn(window);
                }
            }
        }

        private void OnChunkAdded(ILandChunk obj)
        {
            this.chunkResourcesLoader.LoadChunkResources(this, obj);

            IObject2DFactory landChunk2DFactory = LandWorld2D.MappingObjectModelView[obj.GetType()];

            this.landChunksDictionary.Add(obj, landChunk2DFactory.CreateObject2D(obj) as LandChunk2D);
        }


        private void OnChunkRemoved(ILandChunk obj)
        {
            this.chunkResourcesLoader.UnloadChunkResources(this, obj);

            this.landChunksDictionary[obj].Dispose();

            this.landChunksDictionary.Remove(obj);
        }

    }
}
using PokeU.Model;
using PokeU.View;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeU
{
    public class MainWindow
    {
        public static readonly int MODEL_TO_VIEW = 16;

        private Vector2f resolutionScreen;

        private FloatRect boundsView;

        private LandWorld landWorld;

        private LandWorld2D landWorld2D;

        public MainWindow()
        {
            this.landWorld = new LandWorld();

            this.landWorld2D = new LandWorld2D(this.landWorld);
        }

        public void Run()
        {
            var mode = new SFML.Window.VideoMode(800, 600);
            //var window = new SFML.Graphics.RenderWindow(SFML.Window.VideoMode.FullscreenModes[0], "Pokemon Union", SFML.Window.Styles.Fullscreen);
            var window = new SFML.Graphics.RenderWindow(mode, "Pokemon Union");

            window.KeyPressed += Window_KeyPressed;

            window.MouseButtonPressed += OnMouseButtonPressed;
            window.MouseButtonReleased += OnMouseButtonReleased;
            window.MouseMoved += OnMouseMoved;

            //this.object2DManager.SizeScreen = window.GetView().Size;


            SFML.Graphics.View view = window.GetView();

            view.Size = new Vector2f(800, 600);

            this.resolutionScreen = new Vector2f(view.Size.X, view.Size.Y);
            view.Center = new Vector2f(9492, -12595);
            this.SetView(window, view);

            window.SetVerticalSyncEnabled(true);

            Clock clock = new Clock();

            this.landWorld.OnFocusAreaChanged(view.Center / MODEL_TO_VIEW, this.resolutionScreen / MODEL_TO_VIEW, 0);

            // Start the game loop
            while (window.IsOpen)
            {
                Time deltaTime = clock.Restart();

                // Game logic update
                this.landWorld.Update(deltaTime);

                // Draw window
                AObject2D.UpdateZoomAnimationManager(deltaTime);

                window.Clear();

                this.landWorld2D.DrawIn(window, ref this.boundsView);

                // Process events
                window.DispatchEvents();


                // To remove after.
                if (Keyboard.IsKeyPressed(Keyboard.Key.Z))
                {
                    view.Center += new Vector2f(0, -2f);
                }
                else if(Keyboard.IsKeyPressed(Keyboard.Key.S))
                {
                    view.Center += new Vector2f(0, 2f);
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                {
                    view.Center += new Vector2f(2f, 0);
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Q))
                {
                    view.Center += new Vector2f(-2f, 0);
                }
                // Console.WriteLine(view.Center.X + " : " + view.Center.Y);

                this.landWorld.OnFocusAreaChanged(view.Center / MODEL_TO_VIEW, this.resolutionScreen / MODEL_TO_VIEW, 0);

                this.SetView(window, view);

                // Finally, display the rendered frame on screen
                window.Display();
            }

            this.landWorld.Dispose();

            AObject2D.StopAnimationManager();
        }

        private void SetView(SFML.Graphics.RenderWindow window, SFML.Graphics.View view)
        {
            this.boundsView = new FloatRect(view.Center.X - view.Size.X / 2, view.Center.Y - view.Size.Y / 2, view.Size.X, view.Size.Y);

            window.SetView(view);
        }

        private void OnMouseMoved(object sender, SFML.Window.MouseMoveEventArgs e)
        {

        }

        private void OnMouseButtonReleased(object sender, SFML.Window.MouseButtonEventArgs e)
        {

        }

        private void OnMouseButtonPressed(object sender, SFML.Window.MouseButtonEventArgs e)
        {
            
        }

        /// <summary>
        /// Function called when a key is pressed
        /// </summary>
        private void Window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            var window = (SFML.Window.Window)sender;
            if (e.Code == SFML.Window.Keyboard.Key.Escape)
            {
                window.Close();
            }
            else if(e.Code == SFML.Window.Keyboard.Key.Up)
            {
                this.landWorld2D.CurrentAltitude++;
            }
            else if(e.Code == SFML.Window.Keyboard.Key.Down)
            {
                this.landWorld2D.CurrentAltitude--;
            }
        }
    }
}

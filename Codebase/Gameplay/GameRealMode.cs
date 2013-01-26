using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GGJ_DisasterMode.Codebase.Screens;

namespace GGJ_DisasterMode.Codebase.Gameplay
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    public partial class GameplayManager
    {
        private int realVariable;
        private GameGrid grid;

        private const int actionCount = 2;
        private Actions.Action[] actions;

        private void ConstructReal()
        {
            int actionCount =Enum.GetNames(typeof(Actions.ActionType)).Length;
            actions = new Actions.Action[actionCount];

            int i = 0;
            foreach (Actions.ActionType type in Enum.GetValues(typeof(Actions.ActionType)))
            {
                actions[i] = new Actions.Action(type, new Rectangle(585 + 60 + (215 * i), 155, 150, 150));
                ++i;
            }

            currentState = DragState.Idle;
            currentlyDragging = null;
        }

        private void LoadContentReal(ContentManager content)
        {
            Texture2D pixelTexture = content.Load<Texture2D>("graphics//pixel");
            this.grid = new GameGrid(pixelTexture, 9, 9, 32, 31, 18, 18, 1, Color.Black);

            foreach (Actions.Action action in actions)
            {
                action.LoadContent(content);
            }

            
        }
        
        public void UpdateReal(GameTime gameTime, out bool missionRunning)
        {
            foreach (Actions.Action action in actions)
            {
                action.Update(gameTime);
            }



            missionRunning = this.missionRunning;
        }
        
        private void DrawReal(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            grid.Draw(spriteBatch);
            foreach (Actions.Action action in actions)
            {
                action.Draw(spriteBatch);
            }
            spriteBatch.End();

        }

        private IEnumerable<Draggable> GetRealDraggables()
        {
            return actions;
        }

        private void HandleInputReal(InputState input)
        {
            
        }

    }
}

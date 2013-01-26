using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using GGJ_DisasterMode.Codebase.Characters;

namespace GGJ_DisasterMode.Codebase.Screens
{
    class AudioResultsScreen : GameScreen
    {
        enum BarCategory
        {
            category_tooHot = 0,
            category_tooCold,
            category_thirsty,
            category_hungry, 
            category_ill, 
        }

        struct BarEntry
        {
            public float value;
            public Texture2D graphIcon;

            public BarEntry(float entryVal, Texture2D entryTexture)
            {
                value = entryVal;
                graphIcon = entryTexture;
            }
        }

        List<BarEntry>[] data;

        Texture2D background;
        Texture2D closeButton;
        Vector2 uiposition;

        Rectangle closeButtonPosition;

        public AudioResultsScreen(List<Civilian> civiliansToReportOn, bool top)
        {
            uiposition = new Vector2(36.0f, top ? 36.0f : 9.0f + 270.0f);
            closeButtonPosition = new Rectangle((int)uiposition.X + 8, (int)uiposition.Y + 8, 29, 29);
            IsPopup = true;

            data = new List<BarEntry>[Enum.GetValues(typeof(BarCategory)).Length];

            data[(int)BarCategory.category_hungry] = new List<BarEntry>();
            data[(int)BarCategory.category_tooCold] = new List<BarEntry>();
            data[(int)BarCategory.category_thirsty] = new List<BarEntry>();
            data[(int)BarCategory.category_tooHot] = new List<BarEntry>();
            data[(int)BarCategory.category_ill] = new List<BarEntry>();

            CrunchData(civiliansToReportOn);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            ContentManager content = new ContentManager(ScreenManager.Game.Services, "Content");
            background = content.Load<Texture2D>("Graphics//UI//background");
            closeButton = content.Load<Texture2D>("Graphics//UI//close");
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            if (input.GetMouseDown())
            {
                if(closeButtonPosition.Contains(input.GetMousePosition()))
                {
                    this.ExitScreen();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            
            spriteBatch.Begin();
            spriteBatch.Draw(background, uiposition, Color.White);
            spriteBatch.Draw(closeButton, closeButtonPosition, Color.White);
            foreach (BarCategory b in Enum.GetValues(typeof(BarCategory)))
            {
                List<BarEntry> entries = data[(int)b];
                float xOffset = 0.0f;
                foreach (BarEntry barEntry in entries)
                {
                    Vector2 position = GetStartPoint(b);
                    position.Y += barEntry.value * 32.0f;
                    position.X += xOffset;
                    spriteBatch.Draw(barEntry.graphIcon, position, Color.White);

                    //Loop the offset
                    xOffset += 32.0f;
                    if (xOffset >= 64.0f)
                        xOffset = -64.0f;
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void CrunchData(List<Civilian> civilians)
        {
            foreach (Civilian popMemeber in civilians)
            {
                AddDataPoint(BarCategory.category_tooCold, popMemeber.CurrentColdTemp, null);
                AddDataPoint(BarCategory.category_tooHot, popMemeber.CurrentHotTemp, null);
                AddDataPoint(BarCategory.category_thirsty, popMemeber.CurrentThirst, null);
                AddDataPoint(BarCategory.category_ill, popMemeber.CurrentHealth, null);
                AddDataPoint(BarCategory.category_hungry, popMemeber.CurrentHunger, null);
            }
        }

        private void AddDataPoint(BarCategory category, float value, Texture2D graphTexture)
        {
            data[(int)category].Add(new BarEntry(value, graphTexture));
        }

        private Vector2 GetStartPoint(BarCategory category)
        {
            return new Vector2((int)category * 50, 100);
        }

    }
}

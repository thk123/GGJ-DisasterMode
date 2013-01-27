using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

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
        Texture2D face;
        Vector2 uiposition;

        Rectangle closeButtonPosition;

        List<Civilian> civilians;

        SoundEffectInstance foodLow;
        SoundEffectInstance heatLow;
        SoundEffectInstance healthLow;
        SoundEffectInstance thirstlow;

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

            civilians = civiliansToReportOn;
        }

        

        public void UpdateData()
        {
            foreach (List<BarEntry> dataEntryList in data)
            {
                dataEntryList.Clear();
            }
            CrunchData(civilians);

            if (tooHungry >= data[0].Count / 2)
            {
                if (foodLow.State != SoundState.Playing)
                {
                    foodLow.Play();
                }
            }
            else
            {
                if (foodLow.State != SoundState.Playing)
                {
                    foodLow.Stop();
                }
            }

            if (tooThirsty >= data[0].Count / 2)
            {
                if (thirstlow.State != SoundState.Playing)
                {
                    thirstlow.Play();
                }
            }
            else
            {
                if (thirstlow.State != SoundState.Playing)
                {
                    thirstlow.Stop();
                }
            }


            if (tooIll >= data[0].Count / 2)
            {
                if (healthLow.State != SoundState.Playing)
                {
                    healthLow.Play();
                }
            }
            else
            {
                if (healthLow.State != SoundState.Playing)
                {
                    healthLow.Stop();
                }
            }

            if (tooHot >= data[0].Count / 2 || tooCold >= data[0].Count / 2)
            {
                if (heatLow.State != SoundState.Playing)
                {
                    heatLow.Play();
                }
            }
            else
            {
                if (heatLow.State != SoundState.Playing)
                {
                    heatLow.Stop();
                }
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            ContentManager content = new ContentManager(ScreenManager.Game.Services, "Content");
            background = content.Load<Texture2D>("Graphics//UI//background");
            closeButton = content.Load<Texture2D>("Graphics//UI//close");
            face = content.Load<Texture2D>("Graphics//ListenPeople//adult_0");

            foodLow = content.Load<SoundEffect>("Audio//people//baby_cry").CreateInstance();
            healthLow = content.Load<SoundEffect>("Audio//people//cough").CreateInstance();
            heatLow = content.Load<SoundEffect>("Audio//people//groan").CreateInstance();
            thirstlow = content.Load<SoundEffect>("Audio//people//womanCry").CreateInstance();

            /*foodLow.IsLooped = true;
            heatLow.IsLooped = true;
            healthLow.IsLooped = true;
            thirstlow.IsLooped = true;*/

            foodLow.Volume = 0.6f;
            healthLow.Volume = 0.6f;
            heatLow.Volume = 0.6f;
            thirstlow.Volume = 0.6f;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            heatLow.Stop();
            healthLow.Stop();
            thirstlow.Stop();
            foodLow.Stop();
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
                
                //if (b == BarCategory.category_tooHot)
                {
                    List<BarEntry> entries = data[(int)b];
                    float xOffsetSize = 88.0f / (float)entries.Count;
                    float xOffset = -44.0f;
                    foreach (BarEntry barEntry in entries)
                    {
                        Vector2 position = GetStartPoint(b);
                        position.Y += (1 - (barEntry.value / 100.0f)) * 150.0f;
                        position.X += xOffset;
                        spriteBatch.Draw(barEntry.graphIcon, position, Color.White);

                        //Loop the offset
                        xOffset += xOffsetSize;
                        if (xOffset >= 44.0f)
                            xOffset = -44.0f;
                    }
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        int tooCold;
        int tooHot;
        int tooThirsty;
        int tooIll;
        int tooHungry;

        private void CrunchData(List<Civilian> civilians)
        {
            tooCold = 0;
            tooHot = 0;
            tooThirsty = 0;
            tooIll = 0;
            tooHungry = 0;
            foreach (Civilian popMemeber in civilians)
            {
                AddDataPoint(BarCategory.category_tooCold, popMemeber.CurrentColdTemp, popMemeber.GraphTexture);
                if(popMemeber.CurrentColdTemp < 50.0f)
                {
                    ++tooCold;
                }
                AddDataPoint(BarCategory.category_tooHot, popMemeber.CurrentHotTemp, popMemeber.GraphTexture);
                if (popMemeber.CurrentHotTemp < 50.0f)
                {
                    ++tooHot;
                }
                AddDataPoint(BarCategory.category_thirsty, popMemeber.CurrentThirst, popMemeber.GraphTexture);
                if (popMemeber.CurrentThirst< 50.0f)
                {
                    ++tooThirsty;
                }
                AddDataPoint(BarCategory.category_ill, popMemeber.CurrentHealth, popMemeber.GraphTexture);
                if (popMemeber.CurrentHealth< 50.0f)
                {
                    ++tooIll;
                }
                AddDataPoint(BarCategory.category_hungry, popMemeber.CurrentHunger, popMemeber.GraphTexture);
                if (popMemeber.CurrentHunger< 50.0f)
                {
                    ++tooHungry;
                }
            }
        }

        private void AddDataPoint(BarCategory category, float value, Texture2D graphTexture)
        {
            data[(int)category].Add(new BarEntry(value, graphTexture));
        }

        private Vector2 GetStartPoint(BarCategory category)
        {
            return uiposition + new Vector2(44 + (int)category * 105, 32.0f);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using GGJ_DisasterMode.Codebase.Characters;

namespace GGJ_DisasterMode.Codebase.Dropoffs
{
    enum DropoffType
    {
        Dropoff_Health,
        Dropoff_Food,
        Dropoff_Water,
        Dropoff_Temperature,
    }

    enum DropoffState
    {
        Unordered,
        Ordered,
        Placed,
        Delivered,
        Used,
        Decayed, 
    }

    struct DropoffProperties
    {
        public DropoffType type;
        public int duration;
        public int delay;
        public int useCount;

        public string shopTextureLoc;
        public string gridTextureLoc;
    }

    abstract class Dropoff : Draggable
    {
        public DropoffState CurrentState
        {
            get;
            private set;
        }

        public int DaysToDelivery
        {
            get;
            private set;
        }

        public int DaysToDecay
        {
            get;
            private set;
        }

        public int RemainingUses
        {
            get;
            private set;
        }


        DropoffProperties dropoffProperties;

        Texture2D shopTexture;
        Texture2D gridTexture;

        public Dropoff(DropoffProperties properties, Rectangle storeSlot)
            :base(storeSlot)
        {
            CurrentState = DropoffState.Unordered;
            dropoffProperties = properties;
            ResetToDefaultProperties();
        }

        public virtual void LoadContent(ContentManager content)
        {
            shopTexture = content.Load<Texture2D>(dropoffProperties.shopTextureLoc);
            gridTexture = content.Load<Texture2D>(dropoffProperties.gridTextureLoc);

            base.SetContent(shopTexture, null);
        }

       /* public bool CheckCollisionAgainstShopRectangle(Point p)
        {
            return storeSlot.Contains(p);
        }       

        public void BeginDrag(Point mousePoint)
        {
            currentPosition = new Vector2(mousePoint.X, mousePoint.Y);
            CurrentState = DropoffState.Ordered;
        }

        public void UpdateDrag(Point mouseDelta)
        {
            currentPosition.X += mouseDelta.X;
            currentPosition.Y += mouseDelta.Y;
        }

        public void PlaceDropoff(Point gridPoint)
        {
            CurrentState = DropoffState.Placed;
            DaysToDecay = dropoffProperties.duration;
        }

        public void PutDropoffBackToStore()
        {
            CurrentState = DropoffState.Unordered;
        }*/

        public virtual void ProcessDay()
        {
            switch (CurrentState)
            {
                case DropoffState.Unordered:
                    //Nothing changes - we are in the shop
                    break;
                case DropoffState.Ordered:
                    // Probably something has gone wrong?
                    break;
                case DropoffState.Placed:
                    // 
                    --DaysToDelivery;
                    if (DaysToDelivery == 0)
                    {
                        CurrentState = DropoffState.Delivered;
                    }
                    break;
                case DropoffState.Delivered:
                    --DaysToDecay;
                    if (DaysToDecay == 0)
                    {
                        CurrentState = DropoffState.Decayed;
                    }
                    break;
                case DropoffState.Used:
                    break;

                case DropoffState.Decayed:
                    break;
                default:
                    break;
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);/*
            switch (CurrentState)
            {
                case DropoffState.Unordered:
                    // Draw the shop image
                    spriteBatch.Draw(shopTexture, storeSlot, Color.White);
                    break;
                case DropoffState.Ordered:
                    // Draw the shop image as being dragged across the screen
                    spriteBatch.Draw(shopTexture, currentPosition, Color.White);
                    break;
                case DropoffState.Placed:
                    // Draw the object with countdown on the map
                    break;
                case DropoffState.Delivered:
                    break;
                case DropoffState.Used:
                    break;
                case DropoffState.Decayed:
                    break;
                default:
                    break;
            }*/
        }

        public virtual void UseDropoff(Civilian character)
        {
            --RemainingUses;
            if (RemainingUses == 0)
            {
                CurrentState = DropoffState.Used;
            }
        }

        private void ResetToDefaultProperties()
        {
            DaysToDelivery = dropoffProperties.delay;
            DaysToDecay = dropoffProperties.duration;
            RemainingUses = dropoffProperties.useCount;
        }
    }
}

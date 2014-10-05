using FiascoRL.Display.UI.Controls.Coordinates;
using FiascoRL.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Display.UI.Controls
{
    public class InventoryControl : WindowControl
    {
        /// <summary>
        /// Creates a new inventory screen.
        /// </summary>
        public InventoryControl()
            : base()
        {
            this.Coords = new RelativeRectangle(
                new RelativeVector(0.25f, 0.0f),
                new RelativeVector(0.10f, 0.0f),
                0.5f,
                0.8f);

            TitleBarControl titleBar = new TitleBarControl("INVENTORY");
            this.AddChild(titleBar);

            AddFilters();
            AddCloseButton();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Draw close button.
            Rectangle rect = GetActualCoords();
            spriteBatch.Draw(UITexture, new Rectangle((int)(rect.X + rect.Width * 0.925), rect.Y - 10, 24, 24), new Rectangle(762, 86, 24, 24), Color.White);


            UIGraphic.DrawBorderText(spriteBatch, UIGraphic.FiascoFontSmall, "FILTER", 330, 92, 2f, new Color(50, 50, 50));

            spriteBatch.Draw(UITexture, new Rectangle(330, 150, 500, 1), new Rectangle(310, 153, 1, 1), Color.DarkGray);

            Items.Select((x, i) => new { Item = x, Index = i }).ToList().ForEach(x =>
            {
                spriteBatch.DrawString(UIGraphic.FiascoFontSmall, x.Item.Name, new Vector2(330, 160 + 18 * x.Index), Color.White,
                    0, Vector2.Zero, 2, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                if (x.Item.Quantity > 1)
                {
                    spriteBatch.DrawString(UIGraphic.FiascoFontSmall, x.Item.Quantity.ToString(), new Vector2(520, 160 + 18 * x.Index), Color.White,
                        0, Vector2.Zero, 2, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                }
            });
        }

        #region Inventory Buttons
        class InventoryButton : Control
        {
            public Item.ItemCategory Category;
            private bool _mouseover;

            public InventoryButton(Item.ItemCategory type)
                : base()
            {
                Category = type;
                this.CenterBitmap = ButtonTypeToRectangle(type);
            }

            public override void Update(GameTime gameTime)
            {
                Rectangle rect = GetActualCoords();
                MouseState ms = Mouse.GetState();
                Point p = new Point(ms.X, ms.Y);
                InventoryButton selected = ((InventoryControl)Parent).InventoryButtonSelected;

                // If presently being clicked
                if (rect.Contains(p) && ms.LeftButton == ButtonState.Pressed)
                {
                    ((InventoryControl)Parent).InventoryButtonSelected = this;
                }

                // If mouseover or if selected
                if (rect.Contains(p) || (selected != null && selected == this))
                {
                    _mouseover = true;
                }
                else
                {
                    _mouseover = false;
                }
            }

            public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
            {
                Rectangle rect = GetActualCoords();

                if (_mouseover)
                {
                    UIGraphic.DrawBorder(spriteBatch, UITexture, rect, CenterBitmap, Color.Gray, Color.White);
                }
                else
                {
                    spriteBatch.Draw(UITexture, rect, CenterBitmap, Color.SlateGray);
                }
            }

            private Rectangle ButtonTypeToRectangle(Item.ItemCategory type)
            {
                switch (type)
                {
                    case Item.ItemCategory.All:
                        return new Rectangle(336, 456, 32, 32);
                    case Item.ItemCategory.Armor:
                        return new Rectangle(240, 424, 32, 32);
                    case Item.ItemCategory.Belt:
                        return new Rectangle(240, 456, 32, 32);
                    case Item.ItemCategory.Boots:
                        return new Rectangle(272, 456, 32, 32);
                    case Item.ItemCategory.Helmet:
                        return new Rectangle(208, 424, 32, 32);
                    case Item.ItemCategory.Necklace:
                        return new Rectangle(272, 424, 32, 32);
                    case Item.ItemCategory.Potion:
                        return new Rectangle(336, 424, 32, 32);
                    case Item.ItemCategory.Ring:
                        return new Rectangle(304, 456, 32, 32);
                    case Item.ItemCategory.Shield:
                        return new Rectangle(304, 424, 32, 32);
                    case Item.ItemCategory.Weapon:
                        return new Rectangle(208, 456, 32, 32);
                    default:
                        return default(Rectangle);
                }
            }
        }
        #endregion

        #region Helper methods
        private void AddCloseButton()
        {
            Rectangle rect = GetActualCoords();
            WindowButtonControl closeButton = new WindowButtonControl(WindowButtonControl.ButtonColor.Red, WindowButtonControl.OverlayType.Close);
            closeButton.Coords = new RelativeRectangle(
                new RelativeVector(0.925f, 4.0f),
                new RelativeVector(0.0f, -6.0f),
                (float)16 / rect.Width,
                (float)16 / rect.Height);
            AddChild(closeButton);
        }

        private void AddFilters()
        {
            Rectangle rect = GetActualCoords();

            int count = 0;
            foreach (Item.ItemCategory t in (Item.ItemCategory[])Enum.GetValues(typeof(Item.ItemCategory)))
            {
                InventoryButton button = new InventoryButton(t);
                button.Coords = new RelativeRectangle(
                    new RelativeVector(0.05f, count * 36),
                    new RelativeVector(0.08f, 0),
                    (float)32 / rect.Width,
                    (float)32 / rect.Height);
                count++;
                this.AddChild(button);
            }

            // Set 'ALL' as initial filter.
            InventoryButtonSelected = Children
                .Where(x => x.GetType() == typeof(InventoryButton))
                .Cast<InventoryButton>()
                .First();
        }
        #endregion

        #region Properties

        private List<Item> _currentlyDisplayedItems;
        private List<Item> Items
        {
            get
            {
                if (_currentlyDisplayedItems == null)
                {
                    return Session.Player.GetItems();
                }
                else
                {
                    return _currentlyDisplayedItems;
                }
            }
        }

        private InventoryButton _inventoryButtonSelected;
        InventoryButton InventoryButtonSelected
        {
            get
            {
                return _inventoryButtonSelected;
            }
            set
            {
                _inventoryButtonSelected = value;
                if (value.Category == Item.ItemCategory.All)
                {
                    _currentlyDisplayedItems = Session.Player.GetItems()
                        .OrderBy(x => x.Name)
                        .ToList();
                }
                else
                {
                    _currentlyDisplayedItems = Session.Player.GetItems()
                        .Where(x => x.Category == value.Category)
                        .OrderBy(x => x.Name)
                        .ToList();
                }
            }
        }
        #endregion
    }
}

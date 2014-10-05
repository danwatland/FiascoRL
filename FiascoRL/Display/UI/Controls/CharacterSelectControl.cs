using FiascoRL.Display.UI.Controls.Coordinates;
using FiascoRL.Display.UI.Controls.Interfaces;
using FiascoRL.Etc.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FiascoRL.Display.UI.Controls
{
    public class CharacterSelectControl : WindowControl, IHoverableIconHandler
    {
        public CharacterSelectControl()
        {
            this.Coords = new RelativeRectangle(
                new RelativeVector(0.1f, 0.0f),
                new RelativeVector(0.1f, 0.0f),
                0.8f,
                0.8f);

            TitleBarControl titleBar = new TitleBarControl("CHARACTER SELECT") { Enabled = true };
            AddChild(titleBar);

            this.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.PropertyType == typeof(HoverableIconControl))
                .ToList()
                .ForEach(x =>
            {
                AddChild((HoverableIconControl)x.GetValue(this, null));
            });

            AddChild(CancelButton);
            CancelButton.SetPositionAndAutoSize(new RelativeVector(0.8f, 0f), new RelativeVector(0.9f, 0f));

            AddChild(ConfirmButton);
            ConfirmButton.SetPositionAndAutoSize(new RelativeVector(0.9f, 0f), new RelativeVector(0.9f, 0f));
        }

        #region Buttons
        private RaisedButtonControl _confirm;
        RaisedButtonControl ConfirmButton
        {
            get
            {
                if (_confirm == null)
                {
                    _confirm = new RaisedButtonControl("Confirm")
                    {
                        Enabled = true,
                        OnClick = new Action(() => 
                        {
                            Session.Player.ChangeGraphicIndex(GetSelected().IconIndex);
                            Enabled = false; 
                        }),
                    };
                }
                return _confirm;
            }
        }

        private RaisedButtonControl _cancel;
        RaisedButtonControl CancelButton
        {
            get
            {
                if (_cancel == null)
                {
                    _cancel = new RaisedButtonControl("Cancel")
                    {
                        Enabled = true,
                        OnClick = new Action(() => { this.Enabled = false; }),
                    };
                }
                return _cancel;
            }
        }
        #endregion

        #region Class icons
        private HoverableIconControl _fighter;
        HoverableIconControl Fighter
        {
            get
            {
                if (_fighter == null)
                {
                    _fighter = new HoverableIconControl(
                    SpriteGraphic.Creatures,
                    SpriteGraphic.GetSprite(SpriteGraphic.Creatures, 0),
                    "FIGHTER")
                        {
                            Coords = new RelativeRectangle(
                                new RelativeVector(0.0f, 0f),
                                new RelativeVector(0.0f, 0f),
                                24f / GetActualCoords().Width,
                                24f / GetActualCoords().Height),
                            Enabled = true,
                            IconIndex = 0,
                        };
                }
                return _fighter;
            }
        }

        private HoverableIconControl _thief;
        HoverableIconControl Thief
        {
            get
            {
                if (_thief == null)
                {
                    _thief = new HoverableIconControl(
                    SpriteGraphic.Creatures,
                    SpriteGraphic.GetSprite(SpriteGraphic.Creatures, 1),
                    "THIEF")
                    {
                        Coords = new RelativeRectangle(
                            new RelativeVector(0.115f, 0f),
                            new RelativeVector(0.0f, 0f),
                            24f / GetActualCoords().Width,
                            24f / GetActualCoords().Height),
                        Enabled = true,
                        IconIndex = 1,
                    };
                }
                return _thief;
            }
        }

        private HoverableIconControl _archer;
        HoverableIconControl Archer
        {
            get
            {
                if (_archer == null)
                {
                    _archer = new HoverableIconControl(
                    SpriteGraphic.Creatures,
                    SpriteGraphic.GetSprite(SpriteGraphic.Creatures, 2),
                    "ARCHER")
                    {
                        Coords = new RelativeRectangle(
                            new RelativeVector(0.23f, 0f),
                            new RelativeVector(0.0f, 0f),
                            24f / GetActualCoords().Width,
                            24f / GetActualCoords().Height),
                        Enabled = true,
                        IconIndex = 2,
                    };
                }
                return _archer;
            }
        }

        private HoverableIconControl _wizard;
        HoverableIconControl Wizard
        {
            get
            {
                if (_wizard == null)
                {
                    _wizard = new HoverableIconControl(
                    SpriteGraphic.Creatures,
                    SpriteGraphic.GetSprite(SpriteGraphic.Creatures, 3),
                    "WIZARD")
                    {
                        Coords = new RelativeRectangle(
                            new RelativeVector(0.345f, 0f),
                            new RelativeVector(0.0f, 0f),
                            24f / GetActualCoords().Width,
                            24f / GetActualCoords().Height),
                        Enabled = true,
                        IconIndex = 3,
                    };
                }
                return _wizard;
            }
        }

        private HoverableIconControl _priest;
        HoverableIconControl Priest
        {
            get
            {
                if (_priest == null)
                {
                    _priest = new HoverableIconControl(
                    SpriteGraphic.Creatures,
                    SpriteGraphic.GetSprite(SpriteGraphic.Creatures, 4),
                    "PRIEST")
                    {
                        Coords = new RelativeRectangle(
                            new RelativeVector(0.0f, 0f),
                            new RelativeVector(0.2f, 0f),
                            24f / GetActualCoords().Width,
                            24f / GetActualCoords().Height),
                        Enabled = true,
                        IconIndex = 4,
                    };
                }
                return _priest;
            }
        }

        private HoverableIconControl _shaman;
        HoverableIconControl Shaman
        {
            get
            {
                if (_shaman == null)
                {
                    _shaman = new HoverableIconControl(
                    SpriteGraphic.Creatures,
                    SpriteGraphic.GetSprite(SpriteGraphic.Creatures, 5),
                    "SHAMAN")
                    {
                        Coords = new RelativeRectangle(
                            new RelativeVector(0.115f, 0f),
                            new RelativeVector(0.2f, 0f),
                            24f / GetActualCoords().Width,
                            24f / GetActualCoords().Height),
                        Enabled = true,
                        IconIndex = 5,
                    };
                }
                return _shaman;
            }
        }

        private HoverableIconControl _berserker;
        HoverableIconControl Berserker
        {
            get
            {
                if (_berserker == null)
                {
                    _berserker = new HoverableIconControl(
                    SpriteGraphic.Creatures,
                    SpriteGraphic.GetSprite(SpriteGraphic.Creatures, 6),
                    "BERSERKER")
                    {
                        Coords = new RelativeRectangle(
                            new RelativeVector(0.23f, 0f),
                            new RelativeVector(0.2f, 0f),
                            24f / GetActualCoords().Width,
                            24f / GetActualCoords().Height),
                        Enabled = true,
                        IconIndex = 6,
                    };
                }
                return _berserker;
            }
        }

        private HoverableIconControl _paladin;
        HoverableIconControl Paladin
        {
            get
            {
                if (_paladin == null)
                {
                    _paladin = new HoverableIconControl(
                    SpriteGraphic.Creatures,
                    SpriteGraphic.GetSprite(SpriteGraphic.Creatures, 8),
                    "PALADIN")
                    {
                        Coords = new RelativeRectangle(
                            new RelativeVector(0.345f, 0f),
                            new RelativeVector(0.2f, 0f),
                            24f / GetActualCoords().Width,
                            24f / GetActualCoords().Height),
                        Enabled = true,
                        IconIndex = 8,
                    };
                }
                return _paladin;
            }
        }
        #endregion

        public void DeselectAll()
        {
            Children.Where(x => x.GetType() == typeof(HoverableIconControl))
                .ToList()
                .ForEach(x =>
                {
                    ((HoverableIconControl)x).Selected = false;
                });
        }

        public HoverableIconControl GetSelected()
        {
            return Children.Where(x => x.GetType() == typeof(HoverableIconControl))
                .Cast<HoverableIconControl>()
                .Where(x => x.Selected)
                .First();
        }

    }
}

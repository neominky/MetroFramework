﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Controls
{
    [ToolboxBitmap(typeof(RadioButton))]
    public class MetroRadioButton : RadioButton, IMetroControl
    {
        #region Interface

        private MetroColorStyle metroStyle = MetroColorStyle.Blue;
        [Category("Metro Appearance")]
        public MetroColorStyle Style
        {
            get
            {
                if (StyleManager != null)
                    return StyleManager.Style;

                return metroStyle;
            }
            set { metroStyle = value; }
        }

        private MetroThemeStyle metroTheme = MetroThemeStyle.Light;
        [Category("Metro Appearance")]
        public MetroThemeStyle Theme
        {
            get
            {
                if (StyleManager != null)
                    return StyleManager.Theme;

                return metroTheme;
            }
            set { metroTheme = value; }
        }

        private MetroStyleManager metroStyleManager = null;
        [Browsable(false)]
        public MetroStyleManager StyleManager
        {
            get { return metroStyleManager; }
            set { metroStyleManager = value; }
        }

        #endregion

        #region Fields

        private bool useStyleColors = false;
        [Category("Metro Appearance")]
        public bool UseStyleColors
        {
            get { return useStyleColors; }
            set { useStyleColors = value; }
        }

        private MetroLinkSize metroLinkSize = MetroLinkSize.Small;
        [Category("Metro Appearance")]
        public MetroLinkSize FontSize
        {
            get { return metroLinkSize; }
            set { metroLinkSize = value; }
        }

        private MetroLinkWeight metroLinkWeight = MetroLinkWeight.Regular;
        [Category("Metro Appearance")]
        public MetroLinkWeight FontWeight
        {
            get { return metroLinkWeight; }
            set { metroLinkWeight = value; }
        }

        [Browsable(false)]
        public override Color BackColor
        {
            get
            {
                return MetroPaint.BackColor.Form(Theme);
            }
        }

        [Browsable(false)]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }

        [Browsable(false)]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

        private bool isHovered = false;
        private bool isPressed = false;
        private bool isFocused = false;

        #endregion

        #region Constructor

        public MetroRadioButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
        }

        #endregion

        #region Paint Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            Color backColor, borderColor, foreColor;

            if (Parent != null)
                backColor = Parent.BackColor;
            else
                backColor = MetroPaint.BackColor.Form(Theme);

            if (isHovered && !isPressed && Enabled)
            {
                foreColor = MetroPaint.ForeColor.CheckBox.Hover(Theme);
                borderColor = MetroPaint.BorderColor.CheckBox.Hover(Theme);
            }
            else if (isHovered && isPressed && Enabled)
            {
                foreColor = MetroPaint.ForeColor.CheckBox.Press(Theme);
                borderColor = MetroPaint.BorderColor.CheckBox.Press(Theme);
            }
            else if (!Enabled)
            {
                foreColor = MetroPaint.ForeColor.CheckBox.Disabled(Theme);
                borderColor = MetroPaint.BorderColor.CheckBox.Disabled(Theme);
            }
            else
            {
                foreColor = !useStyleColors ? MetroPaint.ForeColor.CheckBox.Normal(Theme) : MetroPaint.GetStyleColor(Style);
                borderColor = MetroPaint.BorderColor.CheckBox.Normal(Theme);
            }

            e.Graphics.Clear(backColor);

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            using (Pen p = new Pen(borderColor))
            {
                Rectangle boxRect = new Rectangle(0, Height / 2 - 6, 12, 12);
                e.Graphics.DrawEllipse(p, boxRect);
            }

            if (Checked)
            {
                using (SolidBrush b = new SolidBrush(borderColor))
                {
                    Rectangle boxRect = new Rectangle(3, Height / 2 - 3, 6, 6);
                    e.Graphics.FillEllipse(b, boxRect);
                }
            }

            e.Graphics.SmoothingMode = SmoothingMode.Default;

            Rectangle textRect = new Rectangle(16, 0, Width - 16, Height);
            TextRenderer.DrawText(e.Graphics, Text, MetroFonts.Link(metroLinkSize, metroLinkWeight), textRect, foreColor, backColor, MetroPaint.GetTextFormatFlags(TextAlign));
        }

        #endregion

        #region Focus Methods

        protected override void OnGotFocus(EventArgs e)
        {
            isFocused = true;
            Invalidate();

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            isFocused = false;
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLostFocus(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            isFocused = true;
            Invalidate();

            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            isFocused = false;
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLeave(e);
        }

        #endregion

        #region Keyboard Methods

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                isHovered = true;
                isPressed = true;
                Invalidate();
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnKeyUp(e);
        }

        #endregion

        #region Mouse Methods

        protected override void OnMouseEnter(EventArgs e)
        {
            isHovered = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPressed = true;
                Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isPressed = false;
            Invalidate();

            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            isHovered = false;
            Invalidate();

            base.OnMouseLeave(e);
        }

        #endregion

        #region Overridden Methods

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            Invalidate();
        }

        #endregion
    }
}
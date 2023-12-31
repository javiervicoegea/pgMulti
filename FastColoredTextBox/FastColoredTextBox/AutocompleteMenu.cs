﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using System.Text;
using System.Runtime.InteropServices;

namespace FastColoredTextBoxNS
{
    /// <summary>
    /// Popup menu for autocomplete
    /// </summary>
    [Browsable(false)]
    public class AutocompleteMenu : ToolStripDropDown, IDisposable
    {
        AutocompleteListView listView;
        public ToolStripControlHost host;
        public Range Fragment { get; internal set; }

        /// <summary>
        /// Regex pattern for serach fragment around caret
        /// </summary>
        public string SearchPattern { get; set; }
        /// <summary>
        /// Minimum fragment length for popup
        /// </summary>
        public int MinFragmentLength { get; set; }
        /// <summary>
        /// User selects item
        /// </summary>
        public event EventHandler<SelectingEventArgs> Selecting;
        /// <summary>
        /// It fires after item inserting
        /// </summary>
        public event EventHandler<SelectedEventArgs> Selected;
        /// <summary>
        /// Occurs when popup menu is opening
        /// </summary>
        public new event EventHandler<CancelEventArgs> Opening;

        // ## Added:
        public new event EventHandler<ProcessKeyDownEventArgs> ProcessKeyDown;

        public new event EventHandler<ProcessKeyPressingEventArgs> ProcessKeyPressing;
        // ## End added

        public bool Forcing = false;

        /// <summary>
        /// Allow TAB for select menu item
        /// </summary>
        public bool AllowTabKey { get { return listView.AllowTabKey; } set { listView.AllowTabKey = value; } }
        /// <summary>
        /// Interval of menu appear (ms)
        /// </summary>
        public int AppearInterval { get { return listView.AppearInterval; } set { listView.AppearInterval = value; } }
        /// <summary>
        /// Sets the max tooltip window size
        /// </summary>
        public int MaxTooltipWidth { get { return listView._ToolTipForm.MaxToolTipWidth; } set { listView._ToolTipForm.MaxToolTipWidth = value; } }
        /// <summary>
        /// Tooltip will perm show and duration will be ignored
        /// </summary>

        /// <summary>
        /// Back color of selected item
        /// </summary>
        [DefaultValue(typeof(Color), "Orange")]
        public Color SelectedColor
        {
            get { return listView.SelectedColor; }
            set { listView.SelectedColor = value; }
        }

        /// <summary>
        /// Border color of hovered item
        /// </summary>
        [DefaultValue(typeof(Color), "Red")]
        public Color HoveredColor
        {
            get { return listView.HoveredColor; }
            set { listView.HoveredColor = value; }
        }

        public FastColoredTextBox FastColoredTextBox { get => listView.tb; }

        public AutocompleteMenu(FastColoredTextBox tb)
        {
            // create a new popup and add the list view to it 
            AutoClose = false;
            AutoSize = false;
            Margin = Padding.Empty;
            Padding = Padding.Empty;
            BackColor = Color.White;
            listView = new AutocompleteListView(tb);
            host = new ToolStripControlHost(listView);
            host.Margin = new Padding(2, 2, 2, 2);
            host.Padding = Padding.Empty;
            host.AutoSize = false;
            host.AutoToolTip = false;
            CalcSize();
            base.Items.Add(host);
            listView.Parent = this;
            SearchPattern = @"[\w\.]";
            MinFragmentLength = 2;
        }

        public new Font Font
        {
            get { return listView.Font; }
            set { listView.Font = value; }
        }

        //## Added:
        private Font _TipFont;
        public new Font TipFont
        {
            get { return _TipFont; }
            set { _TipFont = value; }
        }

        private Font _PreselectedFont;
        public new Font PreselectedFont
        {
            get { return _PreselectedFont; }
            set { _PreselectedFont = value; }
        }
        //## End added

        new internal void OnOpening(CancelEventArgs args)
        {
            if (Opening != null)
                Opening(this, args);
        }

        public new void Close()
        {
            listView._ToolTipForm.Hide();
            base.Close();
        }

        internal void CalcSize()
        {
            host.Size = listView.Size;
            Size = new System.Drawing.Size(listView.Size.Width + 4, listView.Size.Height + 4);
        }

        public virtual void OnSelecting()
        {
            listView.OnSelecting();
        }

        public void SelectNext(int shift)
        {
            listView.SelectNext(shift);
        }

        internal void OnSelecting(SelectingEventArgs args)
        {
            if (Selecting != null)
                Selecting(this, args);
        }

        public void OnSelected(SelectedEventArgs args)
        {
            if (Selected != null)
                Selected(this, args);
        }

        public new AutocompleteListView Items
        {
            get { return listView; }
        }

        /// <summary>
        /// Shows popup menu immediately
        /// </summary>
        /// <param name="forced">If True - MinFragmentLength will be ignored</param>
        public void Show(bool forced)
        {
            Items.DoAutocomplete(forced);
        }

        /// <summary>
        /// Minimal size of menu
        /// </summary>
        public new Size MinimumSize
        {
            get { return Items.MinimumSize; }
            set { Items.MinimumSize = value; }
        }

        // ## Added:
        internal void OnProcessKeyDown(ProcessKeyDownEventArgs e)
        {
            if (ProcessKeyDown != null)
            {
                ProcessKeyDown(this, e);
            }
        }
        internal void OnProcessKeyPressing(ProcessKeyPressingEventArgs e)
        {
            if (ProcessKeyPressing != null)
            {
                ProcessKeyPressing(this, e);
            }
        }
        // ## End added

        /// <summary>
        /// Image list of menu
        /// </summary>
        public new ImageList ImageList
        {
            get { return Items.ImageList; }
            set { Items.ImageList = value; }
        }

        /// <summary>
        /// Tooltip duration (ms)
        /// </summary>
        public int ToolTipDuration
        {
            get { return Items.ToolTipDuration; }
            set { Items.ToolTipDuration = value; }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (listView != null && !listView.IsDisposed)
                listView.Dispose();
        }
    }

    [System.ComponentModel.ToolboxItem(false)]
    public class AutocompleteListView : UserControl, IDisposable
    {
        public event EventHandler FocussedItemIndexChanged;

        internal List<AutocompleteItem> visibleItems;
        IEnumerable<AutocompleteItem> sourceItems = new List<AutocompleteItem>();
        int focussedItemIndex = 0;
        int hoveredItemIndex = -1;

        // Added:
        private bool _ExplicitlySelected = false;
        // End added

        private int ItemHeight
        {
            get { return Font.Height + 2; }
        }

        AutocompleteMenu Menu { get { return Parent as AutocompleteMenu; } }
        int oldItemCount = 0;
        internal FastColoredTextBox tb;
        internal ToolTipForm _ToolTipForm = new ToolTipForm();
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        internal bool AllowTabKey { get; set; }
        public ImageList ImageList { get; set; }
        internal int AppearInterval { get { return timer.Interval; } set { timer.Interval = value; } }
        internal int ToolTipDuration { get; set; }

        public Color SelectedColor { get; set; }
        public Color HoveredColor { get; set; }
        public int FocussedItemIndex
        {
            get { return focussedItemIndex; }
            set
            {
                System.Diagnostics.Debug.WriteLine(value);
                if (focussedItemIndex > 0 && value == 0)
                {
                    System.Diagnostics.Debug.WriteLine("MAL");
                }
                if (focussedItemIndex != value)
                {
                    focussedItemIndex = value;
                    if (FocussedItemIndexChanged != null)
                        FocussedItemIndexChanged(this, EventArgs.Empty);
                }
            }
        }

        public AutocompleteItem FocussedItem
        {
            get
            {
                if (FocussedItemIndex >= 0 && focussedItemIndex < visibleItems.Count)
                    return visibleItems[focussedItemIndex];
                return null;
            }
            set
            {
                FocussedItemIndex = visibleItems.IndexOf(value);
            }
        }

        internal AutocompleteListView(FastColoredTextBox tb)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            base.Font = new Font(FontFamily.GenericSansSerif, 9);
            visibleItems = new List<AutocompleteItem>();
            VerticalScroll.SmallChange = ItemHeight;
            MaximumSize = new Size(Size.Width, 180);
            AppearInterval = 500;
            timer.Tick += new EventHandler(timer_Tick);
            SelectedColor = Color.Orange;
            HoveredColor = Color.Red;
            ToolTipDuration = 3000;

            this.tb = tb;

            tb.KeyDown += new KeyEventHandler(tb_KeyDown);
            tb.SelectionChanged += new EventHandler(tb_SelectionChanged);
            tb.KeyPressed += new KeyPressEventHandler(tb_KeyPressed);
            // ## Added:
            tb.KeyPressing += new KeyPressEventHandler(tb_KeyPressing);
            // ## End added

            Form form = tb.FindForm();
            if (form != null)
            {
                form.LocationChanged += delegate { SafetyClose(); };
                form.ResizeBegin += delegate { SafetyClose(); };
                form.FormClosing += delegate { SafetyClose(); };
                form.LostFocus += delegate { SafetyClose(); };
            }

            tb.LostFocus += (o, e) =>
            {
                if (Menu != null && !Menu.IsDisposed)
                    if (!Menu.Focused)
                        SafetyClose();
            };

            tb.Scroll += delegate { SafetyClose(); };

            this.VisibleChanged += (o, e) =>
            {
                if (this.Visible)
                    DoSelectedVisible();
            };
        }

        private SizeF MeasureToolTipText(Control control, string text, Font f, int maxWidth, int fn)
        {
            Graphics g = Graphics.FromHwnd(control.Handle);
            StringBuilder sbCurrentLine = new StringBuilder();
            //StringBuilder sbLines = new StringBuilder();

            float w = 0;
            float h = 0;
            float currentLineWidth = 0;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                sbCurrentLine.Append(c);

                SizeF currentSize = g.MeasureString(sbCurrentLine.ToString(), f);
                if (currentSize.Width > maxWidth)
                {
                    w = Math.Max(w, currentLineWidth);
                    h += currentSize.Height;
                    sbCurrentLine.Clear();
                    i--;
                    //sbLines.Append("\r\n");
                }
                else if (i == text.Length - 1)
                {
                    w = Math.Max(w, currentLineWidth);
                    h += currentSize.Height;
                    //sbLines.Append(c);
                }
                else
                {
                    currentLineWidth = currentSize.Width;
                    //sbLines.Append(c);
                }
            }

            return new SizeF(w, h);
        }


        protected override void Dispose(bool disposing)
        {
            if (_ToolTipForm != null) { _ToolTipForm.Dispose(); }
            if (tb != null)
            {
                tb.KeyDown -= tb_KeyDown;
                tb.KeyPressed -= tb_KeyPressed;
                tb.SelectionChanged -= tb_SelectionChanged;
            }

            if (timer != null)
            {
                timer.Stop();
                timer.Tick -= timer_Tick;
                timer.Dispose();
            }

            base.Dispose(disposing);
        }

        void SafetyClose()
        {
            if (Menu != null && !Menu.IsDisposed)
                Menu.Close();
        }

        // ## Added:
        private void tb_KeyPressing(object sender, KeyPressEventArgs e)
        {
            bool backspaceORdel = e.KeyChar == '\b' || e.KeyChar == 0xff;

            if (!backspaceORdel && tb.Selection.Start == tb.Selection.End && tb.Selection.Start.iChar > 0)
            {
                if (Menu.Visible)
                {
                    ProcessKeyPressingEventArgs ev = new ProcessKeyPressingEventArgs(e.KeyChar, FocussedItem, FocussedItemIndex, _ExplicitlySelected);
                    Menu.OnProcessKeyPressing(ev);

                    if (ev.Select)
                    {
                        OnSelecting();
                    }

                    DoAutocomplete(false);
                }
            }
        }
        // ## End added

        void tb_KeyPressed(object sender, KeyPressEventArgs e)
        {
            bool backspaceORdel = e.KeyChar == '\b' || e.KeyChar == 0xff;

            /*
            if (backspaceORdel)
                prevSelection = tb.Selection.Start;*/
            if (Menu.Visible && !backspaceORdel)
            {
                DoAutocomplete(false);
            }
            else
                ResetTimer(timer);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            DoAutocomplete(false);
        }

        void ResetTimer(System.Windows.Forms.Timer timer)
        {
            timer.Stop();
            timer.Start();
        }

        internal void DoAutocomplete()
        {
            DoAutocomplete(false);
        }

        internal void DoAutocomplete(bool forced)
        {
            Menu.Forcing = forced;

            try
            {
                if (!Menu.Enabled)
                {
                    Menu.Close();
                    return;
                }

                visibleItems.Clear();
                FocussedItemIndex = 0;
                VerticalScroll.Value = 0;
                //some magic for update scrolls
                AutoScrollMinSize -= new Size(1, 0);
                AutoScrollMinSize += new Size(1, 0);
                //get fragment around caret
                Range fragment = tb.Selection.GetFragment(Menu.SearchPattern);
                string text = fragment.Text;
                //calc screen point for popup menu
                Point point = tb.PlaceToPoint(fragment.End);
                point.Offset(2, tb.CharHeight);
                //
                if (forced || (text.Length >= Menu.MinFragmentLength
                    && tb.Selection.IsEmpty /*pops up only if selected range is empty*/
                    && (tb.Selection.Start > fragment.Start || text.Length == 0/*pops up only if caret is after first letter*/)))
                {
                    Menu.Fragment = fragment;
                    bool foundSelected = false;
                    //build popup menu
                    foreach (var item in sourceItems)
                    {
                        item.Parent = Menu;
                        CompareResult res = item.Compare(text);
                        if (res != CompareResult.Hidden)
                            visibleItems.Add(item);
                        if (res == CompareResult.VisibleAndSelected && !foundSelected)
                        {
                            foundSelected = true;
                            FocussedItemIndex = visibleItems.Count - 1;
                        }
                    }

                    if (foundSelected)
                    {
                        AdjustScroll();
                        DoSelectedVisible();
                    }
                }

                //show popup menu
                if (Count > 0)
                {
                    if (!Menu.Visible)
                    {
                        CancelEventArgs args = new CancelEventArgs();
                        Menu.OnOpening(args);
                        if (!args.Cancel)
                        {
                            _ExplicitlySelected = false;
                            Menu.Show(tb, point);
                        }
                    }

                    DoSelectedVisible();
                    Invalidate();
                }
                else
                    Menu.Close();
            }
            finally
            {
                Menu.Forcing = false;
            }
        }

        void tb_SelectionChanged(object sender, EventArgs e)
        {
            /*
            FastColoredTextBox tb = sender as FastColoredTextBox;
            
            if (Math.Abs(prevSelection.iChar - tb.Selection.Start.iChar) > 1 ||
                        prevSelection.iLine != tb.Selection.Start.iLine)
                Menu.Close();
            prevSelection = tb.Selection.Start;*/
            if (Menu.Visible)
            {
                bool needClose = false;

                if (!tb.Selection.IsEmpty)
                    needClose = true;
                else
                    if (!Menu.Fragment.Contains(tb.Selection.Start))
                {
                    if (tb.Selection.Start.iLine == Menu.Fragment.End.iLine && tb.Selection.Start.iChar == Menu.Fragment.End.iChar + 1)
                    {
                        //user press key at end of fragment
                        char c = tb.Selection.CharBeforeStart;
                        if (!Regex.IsMatch(c.ToString(), Menu.SearchPattern))//check char
                            needClose = true;
                    }
                    else
                        needClose = true;
                }

                if (needClose)
                    Menu.Close();
            }

        }

        void tb_KeyDown(object sender, KeyEventArgs e)
        {
            var tb = sender as FastColoredTextBox;

            if (Menu.Visible)
                if (ProcessKey(e.KeyCode, e.Modifiers))
                    e.Handled = true;

            if (!Menu.Visible)
            {
                if (tb.HotkeysMapping.ContainsKey(e.KeyData) && tb.HotkeysMapping[e.KeyData] == FCTBAction.AutocompleteMenu)
                {
                    DoAutocomplete(true);
                    e.Handled = true;
                }
                else
                {
                    if (e.KeyCode == Keys.Escape && timer.Enabled)
                        timer.Stop();
                }
            }
        }

        void AdjustScroll()
        {
            if (oldItemCount == visibleItems.Count)
                return;

            int needHeight = ItemHeight * visibleItems.Count + 1;
            Height = Math.Min(needHeight, MaximumSize.Height);
            Menu.CalcSize();

            AutoScrollMinSize = new Size(0, needHeight);
            oldItemCount = visibleItems.Count;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            AdjustScroll();

            var itemHeight = ItemHeight;
            int startI = VerticalScroll.Value / itemHeight - 1;
            int finishI = (VerticalScroll.Value + ClientSize.Height) / itemHeight + 1;
            startI = Math.Max(startI, 0);
            finishI = Math.Min(finishI, visibleItems.Count);
            int y = 0;
            // ## Modified:
            int leftPadding = 26;
            // ## End modified
            for (int i = startI; i < finishI; i++)
            {
                y = i * itemHeight - VerticalScroll.Value;

                var item = visibleItems[i];

                if (item.BackColor != Color.Transparent)
                    using (var brush = new SolidBrush(item.BackColor))
                        e.Graphics.FillRectangle(brush, 1, y, ClientSize.Width - 1 - 1, itemHeight - 1);

                if (ImageList != null && visibleItems[i].ImageIndex >= 0)
                    e.Graphics.DrawImage(ImageList.Images[item.ImageIndex], 1, y);

                if (i == FocussedItemIndex)
                    using (var selectedBrush = new LinearGradientBrush(new Point(0, y - 3), new Point(0, y + itemHeight), Color.Transparent, SelectedColor))
                    using (var pen = new Pen(SelectedColor))
                    {
                        e.Graphics.FillRectangle(selectedBrush, leftPadding, y, ClientSize.Width - 1 - leftPadding, itemHeight - 1);
                        e.Graphics.DrawRectangle(pen, leftPadding, y, ClientSize.Width - 1 - leftPadding, itemHeight - 1);
                    }

                if (i == hoveredItemIndex)
                    using (var pen = new Pen(HoveredColor))
                        e.Graphics.DrawRectangle(pen, leftPadding, y, ClientSize.Width - 1 - leftPadding, itemHeight - 1);

                using (var brush = new SolidBrush(item.ForeColor != Color.Transparent ? item.ForeColor : ForeColor))
                    e.Graphics.DrawString(item.ToString(), item.Font != null ? item.Font : Font, brush, leftPadding, y);
            }
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                FocussedItemIndex = PointToItemIndex(e.Location);
                DoSelectedVisible();
                _ExplicitlySelected = true;
                Invalidate();
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            FocussedItemIndex = PointToItemIndex(e.Location);
            Invalidate();
            OnSelecting();
        }

        internal virtual void OnSelecting()
        {
            if (FocussedItemIndex < 0 || FocussedItemIndex >= visibleItems.Count)
                return;
            tb.TextSource.Manager.BeginAutoUndoCommands();
            try
            {
                AutocompleteItem item = FocussedItem;
                SelectingEventArgs args = new SelectingEventArgs()
                {
                    Item = item,
                    SelectedIndex = FocussedItemIndex
                };

                Menu.OnSelecting(args);

                if (args.Cancel)
                {
                    FocussedItemIndex = args.SelectedIndex;
                    Invalidate();
                    return;
                }

                if (!args.Handled)
                {
                    var fragment = Menu.Fragment;
                    DoAutocomplete(item, fragment);
                }

                Menu.Close();
                //
                SelectedEventArgs args2 = new SelectedEventArgs()
                {
                    Item = item,
                    Tb = Menu.Fragment.tb
                };
                item.OnSelected(Menu, args2);
                Menu.OnSelected(args2);
            }
            finally
            {
                tb.TextSource.Manager.EndAutoUndoCommands();
            }
        }

        // ## Modified:
        private void DoAutocomplete(AutocompleteItem item, Range fragment)
        {
            item.DoAutocomplete(fragment);
        }
        // ## End modified

        int PointToItemIndex(Point p)
        {
            return (p.Y + VerticalScroll.Value) / ItemHeight;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            ProcessKey(keyData, Keys.None);

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool ProcessKey(Keys keyData, Keys keyModifiers)
        {
            if (keyModifiers == Keys.None)
                switch (keyData)
                {
                    case Keys.Down:
                        SelectNext(+1);
                        return true;
                    case Keys.PageDown:
                        SelectNext(+10);
                        return true;
                    case Keys.Up:
                        SelectNext(-1);
                        return true;
                    case Keys.PageUp:
                        SelectNext(-10);
                        return true;
                    //case Keys.Enter:
                    //    OnSelecting();
                    //    return true;
                    case Keys.Tab:
                        if (!AllowTabKey)
                            break;
                        OnSelecting();
                        return true;
                    case Keys.Escape:
                        Menu.Close();
                        return true;
                }

            // ## Added:
            ProcessKeyDownEventArgs e = new ProcessKeyDownEventArgs(keyData, keyModifiers, FocussedItem, FocussedItemIndex, _ExplicitlySelected);
            Menu.OnProcessKeyDown(e);

            if (e.Select)
            {
                OnSelecting();
            }
            // ## End added

            return e.Handled;
        }

        public void SelectNext(int shift)
        {
            FocussedItemIndex = Math.Max(0, Math.Min(FocussedItemIndex + shift, visibleItems.Count - 1));
            DoSelectedVisible();
            _ExplicitlySelected = true;
            //
            Invalidate();
        }

        private void DoSelectedVisible()
        {
            if (FocussedItemIndex == 1)
            {
                System.Diagnostics.Debug.WriteLine(FocussedItemIndex + " - " + FocussedItem.Text + " - " + FocussedItem.ToolTipText);
            }
            if (FocussedItem != null)
                SetToolTip(FocussedItem);

            var y = FocussedItemIndex * ItemHeight - VerticalScroll.Value;
            if (y < 0)
                VerticalScroll.Value = FocussedItemIndex * ItemHeight;
            if (y > ClientSize.Height - ItemHeight)
                VerticalScroll.Value = Math.Min(VerticalScroll.Maximum, FocussedItemIndex * ItemHeight - ClientSize.Height + ItemHeight);
            //some magic for update scrolls
            AutoScrollMinSize -= new Size(1, 0);
            AutoScrollMinSize += new Size(1, 0);
        }

        private void SetToolTip(AutocompleteItem autocompleteItem)
        {
            var title = autocompleteItem.ToolTipTitle;
            var text = autocompleteItem.ToolTipText;

            _ToolTipForm.Hide();

            if (string.IsNullOrEmpty(title))
            {
                return;
            }

            if (this.Parent != null)
            {
                IWin32Window window = this.Parent ?? this;
                Point location;
                int y = Math.Max(0, focussedItemIndex * ItemHeight - VerticalScroll.Value);

                bool isLeftAnchor;

                if ((this.PointToScreen(this.Location).X + Width + 5 + _ToolTipForm.MaxToolTipWidth) < Screen.FromControl(this.Parent).WorkingArea.Right)
                {
                    isLeftAnchor = true;
                    location = new Point(Right + 5, y);
                }
                else
                {
                    isLeftAnchor = false;
                    location = new Point(Left - 5, y);
                }

                location = PointToScreen(location);

                if (string.IsNullOrEmpty(text))
                {
                    _ToolTipForm.Show(window, title, null, location, isLeftAnchor);
                }
                else
                {
                    _ToolTipForm.Show(window, title, text, location, isLeftAnchor);
                }
            }
        }

        public int Count
        {
            get { return visibleItems.Count; }
        }

        public void SetAutocompleteItems(ICollection<string> items)
        {
            List<AutocompleteItem> list = new List<AutocompleteItem>(items.Count);
            foreach (var item in items)
                list.Add(new AutocompleteItem(item));
            SetAutocompleteItems(list);
        }

        public void SetAutocompleteItems(IEnumerable<AutocompleteItem> items)
        {
            sourceItems = items;
        }
    }

    public class SelectingEventArgs : EventArgs
    {
        public AutocompleteItem Item { get; internal set; }
        public bool Cancel { get; set; }
        public int SelectedIndex { get; set; }
        public bool Handled { get; set; }
    }

    public class SelectedEventArgs : EventArgs
    {
        public AutocompleteItem Item { get; internal set; }
        public FastColoredTextBox Tb { get; set; }
    }

    // ## Added:
    public class ProcessKeyDownEventArgs : EventArgs
    {
        public readonly Keys KeyData;
        public readonly Keys KeyModifiers;
        public readonly AutocompleteItem Item;
        public readonly int ItemIndex;
        public bool Select = false;
        public bool Handled = false;
        public bool ExplicitlySelected = false;

        public ProcessKeyDownEventArgs(Keys keyData, Keys keyModifiers, AutocompleteItem item, int itemIndex, bool explicitlySelected)
        {
            KeyData = keyData;
            KeyModifiers = keyModifiers;
            Item = item;
            ItemIndex = itemIndex;
            ExplicitlySelected = explicitlySelected;
        }
    }
    public class ProcessKeyPressingEventArgs : EventArgs
    {
        public readonly char KeyChar;
        public readonly AutocompleteItem Item;
        public readonly int ItemIndex;
        public bool Select = false;
        public bool Handled = false;
        public bool ExplicitlySelected = false;

        public ProcessKeyPressingEventArgs(char keyChar, AutocompleteItem item, int itemIndex, bool explicitlySelected)
        {
            KeyChar = keyChar;
            Item = item;
            ItemIndex = itemIndex;
            ExplicitlySelected = explicitlySelected;
        }
    }
    // ## End added
}

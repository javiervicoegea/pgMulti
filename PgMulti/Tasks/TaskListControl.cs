namespace PgMulti.Tasks
{
    public class TaskListControl : ScrollableControl
    {
        private const int InternalItemHeight = 20;
        private const int HorizontalItemMargin = 10;
        private const int VerticalItemMargin = 2;
        private const int ItemPadding = 5;
        private const int MiddleItemHeight = InternalItemHeight + 2 * ItemPadding;
        private const int ExternalItemHeight = MiddleItemHeight + 2 * VerticalItemMargin;
        private const int ReservedGliphSize = 12;
        private const int SelectedTabGliphSize = 12;
        private const int SelectedTaskGliphSize = 6;

        public event EventHandler<EventArgs>? SelectedIndexChanged;
        public event EventHandler<IsSelectedTabTaskNeededEventArgs>? IsSelectedTabTaskNeeded;

        private List<int> _SelectedIndices = new List<int>();
        private List<PgTask> _Tasks = new List<PgTask>();
        private Mutex _Mutex = new Mutex(false);


        private Brush InitForeBrush;
        private Brush RunningForeBrush;
        private Brush FinishedForeBrush;
        private Brush ErrorForeBrush;
        private Brush ProgressForeBrush;

        private Brush BackgroundBrush;
        private Brush InitBackBrush;
        private Brush RunningBackBrush;
        private Brush FinishedBackBrush;
        private Brush ErrorBackBrush;
        private Brush ProgressBackBrush;

        private Brush SelectedInitBackBrush;
        private Brush SelectedRunningBackBrush;
        private Brush SelectedFinishedBackBrush;
        private Brush SelectedErrorBackBrush;
        private Brush SelectedProgressForeBrush;
        private Brush SelectedProgressBackBrush;

        private Pen InitForePen;
        private Pen RunningForePen;
        private Pen FinishedForePen;
        private Pen ErrorForePen;
        private Pen ProgressForePen;
        private Pen SelectedProgressForePen;

        public TaskListControl()
        {
            TabStop = true;
            VerticalScroll.LargeChange = ExternalItemHeight;
            VerticalScroll.SmallChange = ExternalItemHeight / 3;
            DoubleBuffered = true;


            InitForeBrush = new SolidBrush(Color.DarkGray);
            RunningForeBrush = new SolidBrush(Color.DarkBlue);
            FinishedForeBrush = new SolidBrush(Color.DarkGreen);
            ErrorForeBrush = new SolidBrush(Color.DarkRed);
            ProgressForeBrush = new SolidBrush(Color.DarkGreen);

            BackgroundBrush = new SolidBrush(Color.White);
            InitBackBrush = new SolidBrush(Color.White);
            RunningBackBrush = new SolidBrush(Color.FromArgb(230, 230, 255));
            FinishedBackBrush = new SolidBrush(Color.FromArgb(230, 255, 230));
            ErrorBackBrush = new SolidBrush(Color.FromArgb(255, 230, 230));
            ProgressBackBrush = new SolidBrush(Color.FromArgb(230, 255, 230));

            SelectedInitBackBrush = new SolidBrush(Color.LightGray);
            SelectedRunningBackBrush = new SolidBrush(Color.LightBlue);
            SelectedFinishedBackBrush = new SolidBrush(Color.LightGreen);
            SelectedErrorBackBrush = new SolidBrush(Color.LightPink);
            SelectedProgressForeBrush = new SolidBrush(Color.FromArgb(12, 64, 61));
            SelectedProgressBackBrush = new SolidBrush(Color.LightSeaGreen);

            InitForePen = new Pen(InitForeBrush);
            RunningForePen = new Pen(RunningForeBrush);
            FinishedForePen = new Pen(FinishedForeBrush);
            ErrorForePen = new Pen(ErrorForeBrush);
            ProgressForePen = new Pen(ProgressForeBrush);
            SelectedProgressForePen = new Pen(SelectedProgressForeBrush);
        }

        public List<int> SelectedIndices
        {
            get => _SelectedIndices;
        }

        public List<PgTask> SelectedTasks
        {
            get
            {
                _Mutex.WaitOne();
                try
                {
                    List<PgTask> tasks = new List<PgTask>();
                    foreach (int index in _SelectedIndices)
                    {
                        tasks.Add(_Tasks[index]);
                    }

                    return tasks;
                }
                finally { _Mutex.ReleaseMutex(); }
            }
        }

        public IReadOnlyList<PgTask> Tasks { get => _Tasks; }
        public Mutex Mutex { get => _Mutex; }

        public void ClearTasks()
        {
            _Mutex.WaitOne();
            try
            {
                _Tasks.Clear();
                _SelectedIndices.Clear();
                UpdateScrollBars();
                Invalidate();
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        public void PrependTask(PgTask t)
        {
            _Mutex.WaitOne();
            try
            {
                _Tasks.Insert(0, t);
                UpdateScrollBars();
                for (int i = 0; i < _SelectedIndices.Count; i++) _SelectedIndices[i]++;
                Invalidate();
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        public void AppendTask(PgTask t)
        {
            _Mutex.WaitOne();
            try
            {
                _Tasks.Add(t);
                UpdateScrollBars();
                Invalidate(t);
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        public void RemoveTask(PgTask t)
        {
            _Mutex.WaitOne();
            try
            {
                int index = IndexOfTask(t);
                if (index == -1) return;

                if (_SelectedIndices.Contains(index)) _SelectedIndices.Remove(index);
                for (int i = 0; i < _SelectedIndices.Count; i++) if (_SelectedIndices[i]>index) _SelectedIndices[i]--;

                _Tasks.Remove(t);
                UpdateScrollBars();
                Invalidate();
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        public int IndexOfTask(PgTask t)
        {
            _Mutex.WaitOne();
            try
            {
                return _Tasks.IndexOf(t);
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        public void ClearSelection()
        {
            _Mutex.WaitOne();
            try
            {
                _SelectedIndices = new List<int>();
                Invalidate();
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        public void SelectIndex(int index, bool v)
        {
            _Mutex.WaitOne();
            try
            {
                if (index < 0 || index > _Tasks.Count) throw new ArgumentException();
                if (_SelectedIndices.Contains(index) == v) return;

                if (v)
                {
                    _SelectedIndices.Add(index);
                }
                else
                {
                    _SelectedIndices.Remove(index);
                }
                Invalidate(_Tasks[index]);
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        public void SelectTask(PgTask task, bool v)
        {
            if (task == null) throw new ArgumentNullException();
            _Mutex.WaitOne();
            try
            {
                int index = _Tasks.IndexOf(task);
                if (index == -1) throw new ArgumentException();
                SelectIndex(index, v);
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        public void Invalidate(PgTask t)
        {
            _Mutex.WaitOne();
            try
            {
                int index = _Tasks.IndexOf(t);
                if (index == -1) throw new ArgumentException();
                int y = ExternalItemHeight * index;
                Invalidate(new Rectangle(0, y + AutoScrollPosition.Y, Width, ExternalItemHeight));
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int externalItemWidth = Width;
            int middleItemWidth = externalItemWidth - 2 * HorizontalItemMargin;
            int internalItemWidth = middleItemWidth - 2 * HorizontalItemMargin;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillRectangle(BackgroundBrush, e.ClipRectangle);

            _Mutex.WaitOne();
            try
            {
                int index = (e.ClipRectangle.Y - AutoScrollPosition.Y) / ExternalItemHeight;
                int count = e.ClipRectangle.Height / ExternalItemHeight + 2;

                for (int i = Math.Max(0, index); i < Math.Min(_Tasks.Count, index + count); i++)
                {
                    PgTask t = _Tasks[i];

                    bool isSelectedTask = SelectedIndices.Contains(i);

                    Brush foreBrush;
                    Brush backBrush;
                    Brush progressBackBrush;
                    Brush progressForeBrush;

                    Pen forePen;
                    Pen progressForePen;

                    switch (t.State)
                    {
                        case PgTask.StateEnum.Init:
                            foreBrush = InitForeBrush;
                            forePen = InitForePen;
                            break;
                        case PgTask.StateEnum.Running:
                            foreBrush = RunningForeBrush;
                            forePen = RunningForePen;
                            break;
                        case PgTask.StateEnum.Finished:
                            if (t.Exception == null)
                            {
                                foreBrush = FinishedForeBrush;
                                forePen = FinishedForePen;
                            }
                            else
                            {
                                foreBrush = ErrorForeBrush;
                                forePen = ErrorForePen;
                            }
                            break;
                        default:
                            throw new NotSupportedException();
                    }

                    if (isSelectedTask)
                    {
                        progressBackBrush = SelectedProgressBackBrush;
                        progressForeBrush = SelectedProgressForeBrush;
                        progressForePen = SelectedProgressForePen;

                        switch (t.State)
                        {
                            case PgTask.StateEnum.Init:
                                backBrush = SelectedInitBackBrush;
                                break;
                            case PgTask.StateEnum.Running:
                                backBrush = SelectedRunningBackBrush;
                                break;
                            case PgTask.StateEnum.Finished:
                                if (t.Exception == null)
                                {
                                    backBrush = SelectedFinishedBackBrush;
                                }
                                else
                                {
                                    backBrush = SelectedErrorBackBrush;
                                }
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                    }
                    else
                    {
                        progressBackBrush = ProgressBackBrush;
                        progressForeBrush = ProgressForeBrush;
                        progressForePen = ProgressForePen;

                        switch (t.State)
                        {
                            case PgTask.StateEnum.Init:
                                backBrush = InitBackBrush;
                                break;
                            case PgTask.StateEnum.Running:
                                backBrush = RunningBackBrush;
                                break;
                            case PgTask.StateEnum.Finished:
                                if (t.Exception == null)
                                {
                                    backBrush = FinishedBackBrush;
                                }
                                else
                                {
                                    backBrush = ErrorBackBrush;
                                }
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                    }

                    int taskMiddleBaseY = VerticalItemMargin + i * ExternalItemHeight + AutoScrollPosition.Y;
                    int taskMiddleBaseX = HorizontalItemMargin;

                    e.Graphics.FillRectangle(backBrush, taskMiddleBaseX, taskMiddleBaseY, middleItemWidth, MiddleItemHeight);

                    if (t.State == PgTask.StateEnum.Running)
                    {
                        int w;
                        if (t is PgTaskExecutorSqlCsv)
                        {
                            PgTaskExecutorSqlCsv tcsv = (PgTaskExecutorSqlCsv)t;
                            if (tcsv.DBs.Count > 1)
                            {
                                w = (tcsv.CurrentDBIndex * middleItemWidth) / tcsv.DBs.Count;
                            }
                            else
                            {
                                w = (t.CurrentStatementIndex * middleItemWidth) / t.StatementCount;
                            }
                        }
                        else
                        {
                            w = (t.CurrentStatementIndex * middleItemWidth) / t.StatementCount;
                        }

                        e.Graphics.FillRectangle(progressBackBrush, taskMiddleBaseX, taskMiddleBaseY, w, MiddleItemHeight);
                        e.Graphics.DrawRectangle(progressForePen, taskMiddleBaseX, taskMiddleBaseY, w - 2, MiddleItemHeight - 2);
                    }

                    if (isSelectedTask) e.Graphics.FillEllipse(foreBrush, taskMiddleBaseX + ItemPadding + (ReservedGliphSize - SelectedTaskGliphSize) / 2, taskMiddleBaseY + (MiddleItemHeight - SelectedTaskGliphSize) / 2, SelectedTaskGliphSize, SelectedTaskGliphSize);
                    IsSelectedTabTaskNeededEventArgs isttn = new IsSelectedTabTaskNeededEventArgs(t);

                    if (IsSelectedTabTaskNeeded != null) IsSelectedTabTaskNeeded(this, isttn);

                    if (isttn.IsSelectedTabTask) e.Graphics.DrawEllipse(forePen, taskMiddleBaseX + ItemPadding + (ReservedGliphSize-SelectedTabGliphSize)/2, taskMiddleBaseY + (MiddleItemHeight - SelectedTabGliphSize) / 2, SelectedTabGliphSize, SelectedTabGliphSize);

                    e.Graphics.DrawString(t.ToString(), Font, foreBrush, new RectangleF(taskMiddleBaseX + 3 * ItemPadding + ReservedGliphSize, taskMiddleBaseY + ItemPadding, internalItemWidth - ReservedGliphSize - 2 * ItemPadding, InternalItemHeight), StringFormat.GenericTypographic);
                }
            }
            finally { _Mutex.ReleaseMutex(); }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _Mutex.WaitOne();
                try
                {
                    Select();

                    int index = (e.Y - AutoScrollPosition.Y) / ExternalItemHeight;

                    if (index < 0 || index >= _Tasks.Count) return;

                    List<int> prevSelectedIndices = _SelectedIndices.ToList();

                    if ((ModifierKeys & Keys.Control) == Keys.Control)
                    {
                        SelectIndex(index, !prevSelectedIndices.Contains(index));
                    }
                    else if ((ModifierKeys & Keys.Shift) == Keys.Shift && prevSelectedIndices.Count > 0)
                    {
                        foreach (int i in prevSelectedIndices.Skip(1))
                        {
                            SelectIndex(i, false);
                        }

                        int fromIndex = prevSelectedIndices[0];
                        int toIndex;
                        if (fromIndex < index)
                        {
                            fromIndex++;
                            toIndex = index;
                        }
                        else
                        {
                            toIndex = fromIndex - 1;
                            fromIndex = index;
                        }

                        for (int i = fromIndex; i <= toIndex; i++)
                        {
                            SelectIndex(i, true);
                        }
                    }
                    else
                    {
                        foreach (int i in prevSelectedIndices)
                        {
                            if (i != index)
                            {
                                SelectIndex(i, false);
                            }
                        }

                        SelectIndex(index, true);
                    }

                    if (prevSelectedIndices.Count != _SelectedIndices.Count || prevSelectedIndices.Any(i => !_SelectedIndices.Contains(i)) || _SelectedIndices.Any(i => !prevSelectedIndices.Contains(i)))
                    {
                        if (SelectedIndexChanged != null)
                        {
                            SelectedIndexChanged(this, EventArgs.Empty);
                        }
                        Update();
                    }

                }
                finally { _Mutex.ReleaseMutex(); }

            }
        }

        private void UpdateScrollBars()
        {
            AutoScrollMinSize = new Size(0, _Tasks.Count * ExternalItemHeight);
        }

        public class IsSelectedTabTaskNeededEventArgs : EventArgs
        {
            public PgTask Task { get; private set; }
            public bool IsSelectedTabTask { get; set; }

            public IsSelectedTabTaskNeededEventArgs(PgTask t)
            {
                Task = t;
                IsSelectedTabTask = false;
            }
        }
    }
}

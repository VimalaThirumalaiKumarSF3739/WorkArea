
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Maui.Core
{

    /// <summary>
    /// 
    /// </summary>
    public abstract class SfView : View, IDrawableLayout, IVisualTreeElement
    {
        #region fields

        /// <summary>
        /// 
        /// </summary>
        private readonly ILayoutManager layoutManager;

        /// <summary>
        /// 
        /// </summary>
        private DrawingOrder drawingOrder = DrawingOrder.NoDraw;

        /// <summary>
        /// 
        /// </summary>
        private bool clipToBounds = true;

        /// <summary>
        /// 
        /// </summary>
        private Thickness padding = new Thickness(0);

        /// <summary>
        /// 
        /// </summary>
        readonly List<IView> children = new();

        #endregion

        #region properties

        /// <summary>
        /// 
        /// </summary>
        private SfViewHandler? LayoutHandler => Handler as SfViewHandler;


        /// <summary>
        /// 
        /// </summary>
        internal DrawingOrder DrawingOrder
        {
            get
            {
                return drawingOrder;
            }
            set
            {
                drawingOrder = value;
                this.LayoutHandler?.SetDrawingOrder(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<IView> Children => this;

        /// <summary>
        /// 
        /// </summary>
        public bool ClipToBounds
        {
            get
            {
                return this.clipToBounds;
            }
            set
            {
                this.clipToBounds = value;
                this.LayoutHandler?.UpdateClipToBounds(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Thickness Padding
        {
            get
            {
                return this.padding;
            }
            set
            {
                this.padding = value;
                this.MeasureContent(this.Width,this.Height);
                this.ArrangeContent(this.Bounds);
            }
        }

        IReadOnlyList<IVisualTreeElement> IVisualTreeElement.GetVisualChildren() => Children.Cast<IVisualTreeElement>().ToList().AsReadOnly();

        bool Microsoft.Maui.ILayout.ClipsToBounds => this.ClipToBounds;

        int ICollection<IView>.Count => this.children.Count;

        bool ICollection<IView>.IsReadOnly => ((ICollection<IView>)children).IsReadOnly;

        bool ISafeAreaView.IgnoreSafeArea => false;

        Thickness IPadding.Padding => this.Padding;

        DrawingOrder IDrawableLayout.DrawingOrder { get => this.DrawingOrder; set => this.DrawingOrder = value; }

        IView IList<IView>.this[int index] { get => this.children[index] ; set => this.children[index] = value; }

        #endregion

        #region ctor

        /// <summary>
        /// 
        /// </summary>
        public SfView()
        {
            layoutManager = new DrawableLayoutManager(this);
        }

        #endregion

        #region virtual methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="dirtyRect"></param>
        protected virtual void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="widthConstraint"></param>
        /// <param name="heightConstraint"></param>
        /// <returns></returns>
        protected virtual Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            return this.layoutManager.Measure(widthConstraint, heightConstraint);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        protected virtual Size ArrangeContent(Rect bounds)
        {
            return this.layoutManager.ArrangeChildren(bounds);
        }

        #endregion

        #region protected methods

        /// <summary>
        /// 
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            foreach (var item in children)
            {
                if (item is BindableObject child)
                {
                    this.UpdateBindingContextToChild(child);
                }
            }
        }

        #endregion

        #region override methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="widthConstraint"></param>
        /// <param name="heightConstraint"></param>
        /// <returns></returns>
        protected sealed override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            return base.MeasureOverride(widthConstraint, heightConstraint);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        protected sealed override Size ArrangeOverride(Rect bounds)
        {
            return base.ArrangeOverride(bounds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="widthConstraint"></param>
        /// <param name="heightConstraint"></param>
        /// <returns></returns>
        protected sealed override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            if (Handler != null)
            {
                this.LayoutHandler?.SetDrawingOrder(this.DrawingOrder);
                this.LayoutHandler?.UpdateClipToBounds(this.ClipToBounds);
                this.LayoutHandler?.Invalidate();
            }
        }

        #endregion

        #region internal methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        internal void Add(View view)
        {
            ((Microsoft.Maui.ILayout)this).Add(view);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        private void UpdateBindingContextToChild(BindableObject view)
        {
            SetInheritedBindingContext(view, BindingContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        internal void Remove(View view)
        {
            ((Microsoft.Maui.ILayout)this).Remove(view);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="view"></param>
        internal void Insert(int index, View view)
        {
            ((Microsoft.Maui.ILayout)this).Insert(index,view);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Clear()
        {
            ((Microsoft.Maui.ILayout)this).Clear();
        }

        internal void InvalidateDrawable()
        {
            this.LayoutHandler?.Invalidate();
        }

        #endregion

        #region private methods

        void IDrawableLayout.InvalidateDrawable()
        {
            this.InvalidateDrawable();
        }

        Size Microsoft.Maui.ILayout.CrossPlatformMeasure(double widthConstraint, double heightConstraint)
        {
            return MeasureContent(widthConstraint, heightConstraint);
        }

        Size Microsoft.Maui.ILayout.CrossPlatformArrange(Rect bounds)
        {
            return ArrangeContent(bounds);
        }

        int IList<IView>.IndexOf(IView child)
        {
            return children.IndexOf(child);
        }

        void IList<IView>.Insert(int index, IView child)
        {
            this.children.Insert(index, child);
            this.LayoutHandler?.Insert(index, child);
            if (child is Element element)
            {
                OnChildAdded(element);
            }
        }

        void IList<IView>.RemoveAt(int index)
        {
            var child = children[index];
            this.LayoutHandler?.Remove(this.children[index]);
            this.children.RemoveAt(index);
            
            if (child is Element element)
            {
                OnChildRemoved(element,index);
            }
        }

        void ICollection<IView>.Add(IView child)
        {
            this.children.Add(child);
            this.LayoutHandler?.Add(child);
            if (child is Element element)
            {
                OnChildAdded(element);
            }
        }

        void ICollection<IView>.Clear()
        {
            this.children.Clear();
            this.LayoutHandler?.Clear();
        }

        bool ICollection<IView>.Contains(IView child)
        {
            return this.children.Contains(child);
        }

        void ICollection<IView>.CopyTo(IView[] array, int arrayIndex)
        {
            this.children.CopyTo(array, arrayIndex);
        }

        bool ICollection<IView>.Remove(IView child)
        {
            var index = children.IndexOf(child);
            this.LayoutHandler?.Remove(child);
            var childRemoved = this.children.Remove(child);
            if (child is Element element)
            {
                OnChildRemoved(element, index);
            }
            return childRemoved;
        }

        IEnumerator<IView> IEnumerable<IView>.GetEnumerator()
        {
            return children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return children.GetEnumerator();
        }

        void IDrawable.Draw(ICanvas canvas, RectF dirtyRect)
        {
            this.OnDraw(canvas, dirtyRect);
        }

        Rect IAbsoluteLayout.GetLayoutBounds(IView view)
        {
            BindableObject bindable = (BindableObject)view;
            return (Rect)bindable.GetValue(AbsoluteLayout.LayoutBoundsProperty);
        }

        AbsoluteLayoutFlags IAbsoluteLayout.GetLayoutFlags(IView view)
        {
            BindableObject bindable = (BindableObject)view;
            return (AbsoluteLayoutFlags)bindable.GetValue(AbsoluteLayout.LayoutFlagsProperty);
        }

        #endregion
    }
}
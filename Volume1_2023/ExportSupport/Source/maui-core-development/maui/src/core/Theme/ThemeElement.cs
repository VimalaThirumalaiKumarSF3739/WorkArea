using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Syncfusion.Maui.Themes
{
    /// <summary>
    /// ThemeElement from which SubViews/Styles of element should be inherited.
    /// </summary>
    internal static class ThemeElement
    {
        /// <summary>
        /// The common theme property to get syncfusion theme.
        /// </summary>
        public static BindableProperty CommonThemeProperty = BindableProperty.Create(
            "CommonTheme",
            returnType: typeof(string),
            declaringType: typeof(IThemeElement),
            defaultValue: "Default",
            propertyChanged: OnCommonThemePropertyChanged);

        /// <summary>
        /// The control theme property to get theme of the respective control.
        /// </summary>
        public static BindableProperty ControlThemeProperty = BindableProperty.Create(
            "ControlTheme",
            typeof(string),
            typeof(IThemeElement),
            defaultValue: "Default",
            propertyChanged: OnControlThemeChanged);

        /// <summary>
        /// The property used to set primary color for theme.
        /// </summary>
        public static BindableProperty PrimaryColorProperty = BindableProperty.Create(
            "PrimaryColor",
            typeof(Color),
            typeof(IThemeElement),
            defaultValue: Color.FromRgb(0,0,0),
            propertyChanged: null);

        /// <summary>
        /// The property used to set primary light background color for theme.
        /// </summary>
        public static BindableProperty PrimaryLightColorProperty = BindableProperty.Create(
            "PrimaryLightColor",
            typeof(Color),
            typeof(IThemeElement),
            defaultValue: Color.FromRgb(0,0,0),
            propertyChanged: null);

        /// <summary>
        /// The property used to set primary dark background color for theme.
        /// </summary>
        public static BindableProperty PrimaryDarkColorProperty = BindableProperty.Create(
            "PrimaryDarkColor",
            typeof(Color),
            typeof(IThemeElement),
            defaultValue: Color.FromRgb(0,0,0),
            propertyChanged: null);

        /// <summary>
        /// The property used to set primary foreground color for theme.
        /// </summary>
        public static BindableProperty PrimaryForegroundColorProperty = BindableProperty.Create(
            "PrimaryForegroundColor",
            typeof(Color),
            typeof(IThemeElement),
            defaultValue: Color.FromRgb(0,0,0),
            propertyChanged: null);

        /// <summary>
        /// The property used to set primary light foreground color for theme.
        /// </summary>
        public static BindableProperty PrimaryLightForegroundColorProperty = BindableProperty.Create(
            "PrimaryLightForegroundColor",
            typeof(Color),
            typeof(IThemeElement),
            defaultValue: Color.FromRgb(0,0,0),
            propertyChanged: null);

        /// <summary>
        /// The property used to set primary dark foreground color for theme.
        /// </summary>
        public static BindableProperty PrimaryDarkForegroundColorProperty = BindableProperty.Create(
            "PrimaryDarkForegroundColor",
            typeof(Color),
            typeof(IThemeElement),
            defaultValue: Color.FromRgb(0,0,0),
            propertyChanged: null);

        /// <summary>
        /// The property used to set success color for theme.
        /// </summary>
        public static BindableProperty SuccessColorProperty = BindableProperty.Create(
            "SuccessColor",
            typeof(Color),
            typeof(IThemeElement),
            defaultValue: Color.FromRgb(0,0,0),
            propertyChanged: null);

        /// <summary>
        /// The property used to set error color for theme.
        /// </summary>
        public static BindableProperty ErrorColorProperty = BindableProperty.Create(
            "ErrorColor",
            typeof(Color),
            typeof(IThemeElement),
            defaultValue: Color.FromRgb(0,0,0),
            propertyChanged: null);

        /// <summary>
        /// The property used to set warning color for theme.
        /// </summary>
        public static BindableProperty WarningColorProperty = BindableProperty.Create(
            "WarningColor",
            typeof(Color),
            typeof(IThemeElement),
            defaultValue: Color.FromRgb(0,0,0),
            propertyChanged: null);

        /// <summary>
        /// The property used to set info(such as more details) color for theme.
        /// </summary>
        public static BindableProperty InfoColorProperty = BindableProperty.Create(
            "InfoColor",
            typeof(Color),
            typeof(IThemeElement),
            defaultValue: Color.FromRgb(0,0,0),
            propertyChanged: null);

        /// <summary>
        /// The property used to cache resource dictionaries which are belonged to control.
        /// </summary>
        private static readonly Dictionary<string, WeakReference<ResourceDictionary>> ControlThemeCache = new Dictionary<string, WeakReference<ResourceDictionary>>();

        /// <summary>
        /// The style target dictionaries.
        /// </summary>
        private static readonly List<WeakReference<ResourceDictionary>> StyleTargetDictionaries = new List<WeakReference<ResourceDictionary>>();

        /// <summary>
        /// The control key property.
        /// </summary>
        private static BindableProperty controlKeyProperty = BindableProperty.Create(
            "ControlKey",
            typeof(string),
            typeof(IThemeElement),
            defaultValue: string.Empty);

        /// <summary>
        /// The implicit style property.
        /// </summary>
        private static BindableProperty implicitStyleProperty = BindableProperty.Create(
            "ImplicitStyle",
            typeof(Style),
            typeof(IThemeElement),
            default(Style),
            propertyChanged: OnImplicitStyleChanged);

        /// <summary>
        /// The Dictionary contains pending dictionaries to which are to be merged.
        /// </summary>
        private static Dictionary<ResourceDictionary, List<ResourceDictionary>> pendingDictionariesToMerge = new Dictionary<ResourceDictionary, List<ResourceDictionary>>();

        /// <summary>
        /// Boolean property to check whether pending dictionaries are merged.
        /// </summary>
        private static bool isScheduled = false;

        /// <summary>
        /// Holds element of setter object, to call apply method
        /// </summary>
        private static Object[] elements = new Object[1];

        /// <summary>
        ///  Call this <see langword="static"/> method in the constructor for which implement IParentThemeElement and IThemeElement interfaces.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="controlKey">Control key.</param>
        internal static void InitializeThemeResources(Element element, string controlKey)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            Type type = element.GetType();

            element.SetValue(controlKeyProperty, controlKey);

            ////It is mandatory to set following two keys in the constructor
            element.SetDynamicResource(ThemeElement.CommonThemeProperty, "SyncfusionTheme");

            ////Here, you should name the key with following pattern: ControlName + Theme. For example, if it is SfChart control, it should be SfChartTheme
            element.SetDynamicResource(ThemeElement.ControlThemeProperty, controlKey);

            ////Setting dynaimic resource for the common keys. Grid like controls which has setting with virtual methods, can use these properties to return the value.
            element.SetDynamicResource(PrimaryColorProperty, "SyncPrimaryColor");
            element.SetDynamicResource(PrimaryDarkColorProperty, "SyncPrimaryDarkColor");
            element.SetDynamicResource(PrimaryLightColorProperty, "SyncPrimaryLightColor");
            element.SetDynamicResource(PrimaryForegroundColorProperty, "SyncPrimaryForegroundColor");
            element.SetDynamicResource(PrimaryDarkForegroundColorProperty, "SyncPrimaryDarkForegroundColor");
            element.SetDynamicResource(PrimaryLightForegroundColorProperty, "SyncPrimaryLightForegroundColor");
            element.SetDynamicResource(SuccessColorProperty, "SyncSuccessColor");
            element.SetDynamicResource(ErrorColorProperty, "SyncErrorColor");
            element.SetDynamicResource(WarningColorProperty, "SyncWarningColor");
            element.SetDynamicResource(InfoColorProperty, "SyncInfoColor");

            if (element as VisualElement == null)
            {
                string? key = type.FullName;
                element.SetDynamicResource(implicitStyleProperty, key);
            }
        }

        /// <summary>
        /// Adds the style dictionary.
        /// </summary>
        /// <param name="resourceDictionary">Resource dictionary.</param>
        internal static void AddStyleDictionary(ResourceDictionary resourceDictionary)
        {
            if (resourceDictionary == null)
            {
                throw new ArgumentNullException(nameof(resourceDictionary));
            }

            StyleTargetDictionaries.Add(new WeakReference<ResourceDictionary>(resourceDictionary));
        }

        /// <summary>
        /// Merges the pending dictionaries to existing resource dictionaries.
        /// </summary>
        private static void MergePendingDictionaries()
        {
            if (pendingDictionariesToMerge != null)
            {
                foreach (var pendingDictionaries in pendingDictionariesToMerge)
                {
                    var targetDictionary = pendingDictionaries.Key;

                    foreach (var themeDictionary in pendingDictionaries.Value)
                    {
                        targetDictionary.MergedDictionaries.Add(themeDictionary);
                    }

                    pendingDictionaries.Value.Clear();
                }

                pendingDictionariesToMerge.Clear();
            }

            isScheduled = false;
        }

        /// <summary>
        /// Called when control theme was changed implicitily
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnImplicitStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            Style? style = newValue as Style;
            Element? element = bindable as Element;

            if (element != null && style != null && !ApplyStyle(element, style))
            {
                foreach (Setter setter in (IEnumerable<Setter>)style.Setters)
                {
                    DynamicResource? dynamicResource = setter.Value as DynamicResource;
                    if (dynamicResource != null)
                    {
                        element.SetDynamicResource(setter.Property, dynamicResource.Key);
                    }
                    else
                    {
                        element.SetValue(setter.Property, setter.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Returns a boolean value indicating whether the Setter's Apply Method is invoked using Reflection or not.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        private static bool ApplyStyle(Element element, Style style)
        {
            var applyMethodInfo = typeof(Style).GetInterface("IStyle")?.GetMethod("Apply");

            if (applyMethodInfo != null)
            {
                elements![0] = element;
                applyMethodInfo.Invoke(style, elements);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Implementation of common theme property changed.
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnCommonThemePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var themeElement = bindable as IThemeElement;

            if (themeElement != null)
            {
                themeElement.OnCommonThemeChanged((string)oldValue, (string)newValue);
            }
        }

        /// <summary>
        /// Merges the theme dictionary to pending dictionaries.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="themeDictionary">Theme dictionary.</param>
        private static void MergeThemeDictionary(string key, ResourceDictionary themeDictionary)
        {
            if (themeDictionary == null)
            {
                return;
            }

            int count = StyleTargetDictionaries.Count;

            for (int i = count - 1; i >= 0; i--)
            {
                var weakReference = StyleTargetDictionaries[i];
                ResourceDictionary? resourceDictionary;
                weakReference.TryGetTarget(out resourceDictionary);
                if (resourceDictionary != null)
                {
                    object value;
                    if (resourceDictionary.TryGetValue(key, out value))
                    {
                        if (!resourceDictionary.MergedDictionaries.Contains(themeDictionary))
                        {
                            List<ResourceDictionary>? pendingDictionaries = null;

                            if (!pendingDictionariesToMerge.ContainsKey(resourceDictionary))
                            {
                                pendingDictionaries = new List<ResourceDictionary>();
                                pendingDictionariesToMerge.Add(resourceDictionary, pendingDictionaries);
                            }
                            else
                            {
                                pendingDictionaries = pendingDictionariesToMerge[resourceDictionary];
                            }

                            pendingDictionaries.Add(themeDictionary);

                            if (!isScheduled && Application.Current != null)
                            {
                                Application.Current.Dispatcher.Dispatch(MergePendingDictionaries);
                                isScheduled = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tries the get theme dictionary from Control.
        /// </summary>
        /// <returns><c>true</c>, if get theme dictionary was tryed, <c>false</c> otherwise.</returns>
        /// <param name="element">Element.</param>
        /// <param name="resourceDictionary">Resource dictionary.</param>
        private static bool TryGetThemeDictionary(VisualElement element, out ResourceDictionary? resourceDictionary)
        {
            resourceDictionary = null;
            if (element != null)
            {
                string key = (string)element.GetValue(controlKeyProperty);
                WeakReference<ResourceDictionary>? weakReference = null;

                if (ControlThemeCache.TryGetValue(key, out weakReference)
                    && weakReference.TryGetTarget(out resourceDictionary))
                {
                    return true;
                }
                else if (ControlThemeCache.ContainsKey(key))
                {
                    ControlThemeCache.Remove(key);
                }

                var parentElement = element as IParentThemeElement;
                if (parentElement != null)
                {
                    resourceDictionary = parentElement.GetThemeDictionary();

                    if (resourceDictionary != null)
                    {
                        weakReference = new WeakReference<ResourceDictionary>(resourceDictionary);
                        ControlThemeCache.Add(key, weakReference);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Implementation of control theme changed.
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnControlThemeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            IThemeElement? themeElement = bindable as IThemeElement;
            if (bindable != null)
            {
                var controlKey = (string)bindable.GetValue(controlKeyProperty);

                VisualElement? visualElement = bindable as VisualElement;

                if (visualElement != null && StyleTargetDictionaries?.Count > 0)
                {
                    ResourceDictionary? themeDictionary;
                    if (TryGetThemeDictionary(visualElement, out themeDictionary) && themeDictionary != null)
                    {
                        MergeThemeDictionary(controlKey, themeDictionary);
                    }
                }

                if (themeElement != null)
                {
                    themeElement.OnControlThemeChanged((string)oldValue, (string)newValue);
                }
            }
        }
    }
}

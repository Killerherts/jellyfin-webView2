﻿using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace jellyfin_cWeb2
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application.
                }

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                bool enableDebug = true;
                if (enableDebug)
                {
                    rootFrame.Navigate(typeof(UrlInputPage));
                }
                if (rootFrame.Content == null)
                {
                    if (localSettings.Values.TryGetValue("savedUrl", out var savedUrl) && savedUrl is string url)
                    {
                        rootFrame.Navigate(typeof(MainPage), url);
                    }
                    else
                    {
                        rootFrame.Navigate(typeof(UrlInputPage));
                    }
                }

                Window.Current.Activate();
            }
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        /// <summary>
        /// Clears the saved URL from the application settings.
        /// </summary>
        public static void ClearSavedUrl()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values.Remove("savedUrl");
        }
    }
}
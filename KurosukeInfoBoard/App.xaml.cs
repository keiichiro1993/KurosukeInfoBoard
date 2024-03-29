﻿using KurosukeInfoBoard.Utils;
using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Windows.ApplicationModel.Resources;

namespace KurosukeInfoBoard
{
    /// <summary>
    /// 既定の Application クラスを補完するアプリケーション固有の動作を提供します。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        ///単一アプリケーション オブジェクトを初期化します。これは、実行される作成したコードの
        ///最初の行であるため、論理的には main() または WinMain() と等価です。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            var resource = ResourceLoader.GetForViewIndependentUse("Keys");
            AppCenter.Start(resource.GetString("AppCenterKey"), typeof(Analytics), typeof(Crashes));
        }

        /// <summary>
        /// アプリケーションがエンド ユーザーによって正常に起動されたときに呼び出されます。他のエントリ ポイントは、
        /// アプリケーションが特定のファイルを開くために起動されたときなどに使用されます。
        /// </summary>
        /// <param name="e">起動の要求とプロセスの詳細を表示します。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // ウィンドウに既にコンテンツが表示されている場合は、アプリケーションの初期化を繰り返さずに、
            // ウィンドウがアクティブであることだけを確認してください
            if (rootFrame == null)
            {
                // ナビゲーション コンテキストとして動作するフレームを作成し、最初のページに移動します
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 以前中断したアプリケーションから状態を読み込みます
                }

                // フレームを現在のウィンドウに配置します
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // ナビゲーションの履歴スタックが復元されていない場合、最初のページに移動します。
                    // このとき、必要な情報をナビゲーション パラメーターとして渡して、新しいページを
                    // 作成します
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // 現在のウィンドウがアクティブであることを確認します
                Window.Current.Activate();
            }

            // Activate Screen Saver Timer
            AppGlobalVariables.Frame = rootFrame;
            AppGlobalVariables.Dispatcher = Window.Current.Dispatcher;
            Window.Current.CoreWindow.TouchHitTesting += CoreWindow_TouchHitTesting; //detect user activity
            if (SettingsHelper.Settings.IsScreenSaverEnabled.GetValue<bool>()) { ScreenSaverTimer.StartTimer(); }
        }

        private void CoreWindow_TouchHitTesting(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.TouchHitTestingEventArgs args)
        {
            Utils.AppGlobalVariables.LastTouchActivity = DateTime.Now;
        }

        /// <summary>
        /// 特定のページへの移動が失敗したときに呼び出されます
        /// </summary>
        /// <param name="sender">移動に失敗したフレーム</param>
        /// <param name="e">ナビゲーション エラーの詳細</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            DebugHelper.Debugger.WriteErrorLog("Failed to load Page " + e.SourcePageType.FullName, e.Exception);
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// アプリケーションの実行が中断されたときに呼び出されます。
        /// アプリケーションが終了されるか、メモリの内容がそのままで再開されるかに
        /// かかわらず、アプリケーションの状態が保存されます。
        /// </summary>
        /// <param name="sender">中断要求の送信元。</param>
        /// <param name="e">中断要求の詳細。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: アプリケーションの状態を保存してバックグラウンドの動作があれば停止します
            deferral.Complete();
        }


        /// <summary>
        /// Invoked when the application is launched through a custom URI scheme, such as
        /// is the case in an OAuth 2.0 authorization flow.
        /// </summary>
        /// <param name="args">Details about the URI that activated the app.</param>
        protected override void OnActivated(IActivatedEventArgs args)
        {
            // When the app was activated by a Protocol (custom URI scheme), forwards
            // the URI to the MainPage through a Navigate event.
            if (args.Kind == ActivationKind.Protocol)
            {
                // Extracts the authorization response URI from the arguments.
                ProtocolActivatedEventArgs protocolArgs = (ProtocolActivatedEventArgs)args;
                Uri uri = protocolArgs.Uri;
                Debug.WriteLine("Authorization Response: " + uri.AbsoluteUri);

                Utils.AppGlobalVariables.GoogleAuthResultUri = uri;

                // Gets the current frame, making one if needed.
                //var frame = Window.Current.Content as Frame;
                //if (frame == null)
                //   frame = new Frame();

                // Opens the URI for "navigation" (handling) on the MainPage.
                //frame.Navigate(typeof(MainPage), uri);
                //Window.Current.Content = frame;
                //Window.Current.Activate();
            }
        }

        public static bool TryGoBack()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                try
                {
                    rootFrame.GoBack();
                }
                catch (OperationCanceledException ex)
                {
                    DebugHelper.Debugger.WriteErrorLog("Operation cancelled in App.TryGoBack.", ex);
                    return false;
                }
                catch (Exception ex)
                {
                    DebugHelper.Debugger.WriteErrorLog("Error occurred in App.TryGoBack. Loading Main Page instead.", ex);
                    rootFrame.Navigate(typeof(MainPage));
                }
                return true;
            }
            return false;
        }
    }
}

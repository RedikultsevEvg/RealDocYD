using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;

using Disk.SDK;
using Disk.SDK.Provider;

namespace SdkSample.WPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        /// TODO: Register your application https://oauth.yandex.ru/client/new and set here your application ID and return URL.

//#error define your client_id and return_url
        private const string CLIENT_ID = "f75503b1cc734370a0a0e1f6e1acabe5";
        //private const string RETURN_URL = "https://oauth.yandex.ru/authorize?response_type=token&client_id=f75503b1cc734370a0a0e1f6e1acabe5";
        //private const string RETURN_URL = "https://oauth.yandex.ru/verification_code";
        private const string RETURN_URL = "https://oauth.yandex.ru/verification_code#access_token=AgAAAAACuFN0AAW808LC4Pxn90nevCsZgQSS4Oo&token_type=bearer&expires_in=31524949";

        private readonly IDiskSdkClient sdkClient;

        public LoginWindow()
        {
            this.InitializeComponent();
        }

        public LoginWindow(IDiskSdkClient sdkClient)
            : this()
        {
            this.sdkClient = sdkClient;
            //Добавляем токен
            this.sdkClient.AccessToken= "AgAAAAACuFN0AAW808LC4Pxn90nevCsZgQSS4Oo";
            this.sdkClient.AuthorizeAsync(new WebBrowserWrapper(browser), CLIENT_ID, RETURN_URL, this.CompleteCallback);
        }

        private void CompleteCallback(object sender, GenericSdkEventArgs<string> e)
        {
            if (this.AuthCompleted != null)
            {
                this.AuthCompleted(this, new GenericSdkEventArgs<string>(e.Result));
            }

            this.Close();
        }

        public event EventHandler<GenericSdkEventArgs<string>> AuthCompleted;
    }
}
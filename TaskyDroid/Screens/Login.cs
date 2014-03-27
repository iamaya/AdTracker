using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TaskyAndroid.Screens
{
    [Activity(Label = "AdTracker", MainLauncher = true)]
    public class Login : Activity
    {
        protected Button _btnLogin = null;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.SetContentView(Resource.Layout.Login);

            _btnLogin = FindViewById<Button>(Resource.Id.btnLogin);

            // Create your application here
            if (_btnLogin != null)
            {
                this._btnLogin.Click += (sender, e) =>
                       {
                           this.StartActivity(typeof(HomeScreen));
                       };
            }
        }
    }
}
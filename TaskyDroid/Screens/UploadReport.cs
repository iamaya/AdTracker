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
using Android.Graphics;
using Java.IO;
using System.Collections;

namespace TaskyAndroid.Screens
{
    [Activity(Label = "Upload Report")]
    public class UploadReport : Activity
    {
        protected ImageView _capturedImage;
        protected TextView _txtComments;
        protected File _file;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.UploadReport);

            string[] reportData = Intent.GetStringArrayExtra("UploadReport");

            _capturedImage = FindViewById<ImageView>(Resource.Id.CapturedImage);
            _txtComments = FindViewById<TextView>(Resource.Id.txtComments);

            _file = new Java.IO.File(reportData[2]);

            // Create your application here

            // display in ImageView. We will resize the bitmap to fit the display
            // Loading the full sized image will consume to much memory 
            // and cause the application to crash.
            int height = 80;
            int width = 720;
            using (Bitmap bitmap = _file.Path.LoadAndResizeBitmap(width, height))
            {
                _capturedImage.SetImageBitmap(bitmap);
            }

            _txtComments.Text = reportData[0] + System.Environment.NewLine + System.Environment.NewLine;
            _txtComments.Text += reportData[1] + System.Environment.NewLine;

        }
    }
}
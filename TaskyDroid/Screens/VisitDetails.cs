using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Provider;
using Java.IO;
using Android.OS;

using Tasky.BL;

using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using Android.Content.PM;
using System.Collections;


namespace TaskyAndroid.Screens
{
    [Activity(Label = "Visit Details")]
    public class VisitDetails : Activity
    {

        protected Task _task = new Task();
        protected Button _btnYesAdOnScreen = null;
        protected Button _btnNoAdOnScreen = null;
        protected TextView _txtAdDetails = null;
        protected TextView _txtDrOfficeTitle = null;
        protected File _dir;
        protected static File _file;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Intent intent = new Intent(MediaStore.ActionImageCapture);

            int taskID = Intent.GetIntExtra("VisitID", 0);
            if (taskID > 0)
            {
                this._task = Tasky.BL.Managers.TaskManager.GetTask(taskID);
            }

            this.SetContentView(Resource.Layout.VisitDetails);

            this._txtDrOfficeTitle = this.FindViewById<TextView>(Resource.Id.txtDrOfficeTitle);
            this._txtAdDetails = this.FindViewById<TextView>(Resource.Id.txtAdDetail);
            this._btnYesAdOnScreen = this.FindViewById<Button>(Resource.Id.btnAdOnScreen);
            this._btnNoAdOnScreen = this.FindViewById<Button>(Resource.Id.btnNoAdOnScreen);

            // name
            if (this._txtDrOfficeTitle != null) { this._txtDrOfficeTitle.Text = this._task.Name; }

            // notes
            if (this._txtAdDetails != null) { this._txtAdDetails.Text = this._task.Notes; }

            // button clicks 
            //			this._btnNoAdOnScreen.Click += (sender, e) => { this.CancelDelete(); };
            this._btnYesAdOnScreen.Click += (sender, e) =>
            {
               if (IsThereAnAppToTakePictures()) 
                   InitiatePhoto();
            };
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // make it available in the gallery
           // Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            //Uri contentUri = Uri.FromFile(_file);
            //mediaScanIntent.SetData(contentUri);
            //SendBroadcast(mediaScanIntent);


            IList<string> reportData = new List<string>();

            reportData.Add(this._task.Name);
            reportData.Add(this._task.Notes);
            reportData.Add(_file.AbsolutePath);

            var uploadReport = new Intent(this, typeof(UploadReport));
            uploadReport.PutExtra("UploadReport", reportData.ToArray<string>());
            this.StartActivity(uploadReport);

        }

        private void InitiatePhoto()
        {
            CreateDirectoryForPictures();
            Intent intent = new Intent(MediaStore.ActionImageCapture);

            _file = new File(_dir, String.Format("myPic_{0}.jpg", Guid.NewGuid()));
            
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(_file));

            StartActivityForResult(intent, 0);
        }

        private void CreateDirectoryForPictures()
        {
            _dir = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "AdTracker");
            if (!_dir.Exists())
            {
                _dir.Mkdirs();
            }
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        protected void Save()
        {
            //			this._task.Name = this._nameTextEdit.Text;
            //		this._task.Notes = this._notesTextEdit.Text;
            Tasky.BL.Managers.TaskManager.SaveTask(this._task);
            this.Finish();
        }

        protected void CancelDelete()
        {
            if (this._task.ID != 0)
            {
                Tasky.BL.Managers.TaskManager.DeleteTask(this._task.ID);
            }
            this.Finish();
        }
    }
}
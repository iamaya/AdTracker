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

using Tasky.BL;

namespace TaskyAndroid.Screens
{
    [Activity(Label = "Today's Visits")]
    public class TodaysVisits : Activity
    {
        protected Adapters.TaskListAdapter _taskList;
        protected IList<Task> _tasks;
        protected ListView _visitListView = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.TodaysVisits);

            //Find our controls
            this._visitListView = FindViewById<ListView>(Resource.Id.lstVisits);

            // wire up task click handler
            if (this._visitListView != null)
            {
                this._visitListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
                {
                    var visitDetails = new Intent(this, typeof(VisitDetails));
                    visitDetails.PutExtra("VisitID", this._tasks[e.Position].ID);
                    this.StartActivity(visitDetails);
                };
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            this._tasks = Tasky.BL.Managers.TaskManager.GetTasks();

            // create our adapter
            this._taskList = new Adapters.TaskListAdapter(this, this._tasks);

            //Hook up our adapter to our ListView
            this._visitListView.Adapter = this._taskList;
        }

    }
}
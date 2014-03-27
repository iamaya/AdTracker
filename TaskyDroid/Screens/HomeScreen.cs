using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Tasky.BL;

namespace TaskyAndroid.Screens
{
    [Activity(Label = "Home", Icon = "@drawable/launcher")]			
	public class HomeScreen : Activity
	{
		protected Adapters.TaskListAdapter _taskList;
		protected IList<Task> _tasks;
		protected Button _addTaskButton = null;
        protected Button _todaysVisitsButton = null;
		protected ListView _taskListView = null;
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			
			// set our layout to be the home screen
			this.SetContentView(Resource.Layout.HomeScreen);

			//Find our controls
			this._taskListView = FindViewById<ListView> (Resource.Id.lstTasks);
			this._addTaskButton = FindViewById<Button> (Resource.Id.btnAddTask);
            this._todaysVisitsButton = FindViewById<Button>(Resource.Id.btnTodaysVisits);

			// wire up add task button handler
			if(this._addTaskButton != null)
			{
				this._addTaskButton.Click += (sender, e) => {
					this.StartActivity(typeof(TaskDetailsScreen));
				};
			}

            // wire up today's visits button handler

            if (this._todaysVisitsButton != null)
            {
                this._todaysVisitsButton.Click += (sender, e) =>
                {
                    this.StartActivity(typeof(TodaysVisits));
                };
            }
			
			// wire up task click handler
			if(this._taskListView != null)
			{
				this._taskListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
					var taskDetails = new Intent (this, typeof (TaskDetailsScreen));
					taskDetails.PutExtra ("VisitID", this._tasks[e.Position].ID);
					this.StartActivity (taskDetails);
				};
			}
		}
		
		protected override void OnResume ()
		{
			base.OnResume ();

			this._tasks = Tasky.BL.Managers.TaskManager.GetTasks();
			
			// create our adapter
			this._taskList = new Adapters.TaskListAdapter(this, this._tasks);

			//Hook up our adapter to our ListView
			this._taskListView.Adapter = this._taskList;
		}
		
	}
}


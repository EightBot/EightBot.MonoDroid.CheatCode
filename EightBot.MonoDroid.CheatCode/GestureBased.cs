using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Environment = System.Environment;

namespace EightBot.MonoDroid.CheatCode
{
    [Activity(Label = "Code Tester", MainLauncher = true, Icon = "@drawable/icon",
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class GestureBased : Activity, GestureDetector.IOnGestureListener
    {
        private const Int32 MINIMUM_MOVEMENT_DISTANCE = 15;
        private readonly CommandBuffer<String> _commandBuffer = new CommandBuffer<String>(15);
        private GestureDetector _gestureDetector;

        private ImageView contraImage;
        private Animation moveLefttoRight;
        private ImageView toastyImage;
        private Animation toastyIn;
        private Animation toastyOut;
        private TextView tvCommandList;
        private TextView tvLastCommand;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _gestureDetector = new GestureDetector(this);

            SetupControls();
        }

        private void SetupControls()
        {
            tvLastCommand = FindViewById<TextView>(Resource.Id.tvMain_LastCommand);

            tvCommandList = FindViewById<TextView>(Resource.Id.tvMain_CommandList);


            contraImage = FindViewById<ImageView>(Resource.Id.ivMain_contra);

            var screenSize = new Point();
            WindowManager.DefaultDisplay.GetSize(screenSize);

            moveLefttoRight = new TranslateAnimation(0 - contraImage.Width, screenSize.X + contraImage.Width, 0, 0);
            moveLefttoRight.Duration = 3000;
            moveLefttoRight.Interpolator = new LinearInterpolator();
            moveLefttoRight.FillAfter = false;

            toastyImage = FindViewById<ImageView>(Resource.Id.ivMain_toasty);

            toastyIn = AnimationUtils.LoadAnimation(this, Resource.Animation.toasty_popup);
            toastyOut = AnimationUtils.LoadAnimation(this, Resource.Animation.toasty_popout);

            toastyIn.AnimationEnd +=
                delegate
                {
                    toastyImage.Visibility = ViewStates.Visible;
                    Thread.Sleep(1000);
                    toastyImage.StartAnimation(toastyOut);
                    toastyImage.Visibility = ViewStates.Invisible;
                };
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            _gestureDetector.OnTouchEvent(e);
            return false;
        }

        #region "On Gesture Listener"

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            Task<bool> task = Task.Factory.StartNew(() =>
                {
                    float xChange = e2.GetX() - e1.GetX();
                    float yChange = e2.GetY() - e1.GetY();

                    if (Math.Abs((int)xChange) <= MINIMUM_MOVEMENT_DISTANCE &&
                        Math.Abs((int)yChange) <= MINIMUM_MOVEMENT_DISTANCE)
                        return false;

                    if (Math.Abs(xChange) > Math.Abs(yChange))
                        ProcessCommand(xChange > 0 ? CodeHelper.KEY_RIGHT : CodeHelper.KEY_LEFT);
                    else
                        ProcessCommand(yChange > 0 ? CodeHelper.KEY_DOWN : CodeHelper.KEY_UP);

                    return true;
                });


            return task.Result;
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            Task<bool> task = Task.Factory.StartNew(() =>
                {
                    ProcessCommand(CodeHelper.KEY_TAP);
                    return true;
                });

            return task.Result;
        }

        public bool OnDown(MotionEvent e)
        {
            return true;
        }


        public void OnLongPress(MotionEvent e)
        {
        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            return false;
        }

        public void OnShowPress(MotionEvent e)
        {
        }

        #endregion

        #region " Methods "

        private void ProcessCommand(String command)
        {
            _commandBuffer.Add(command);

            string[] commandArray = _commandBuffer.Commands;

            if (_commandBuffer.ProcessCommandList(CodeHelper.MortalKombatCode))
            {
                MediaPlayer.Create(this, Resource.Raw.toasty).Start();
                RunOnUiThread(() => toastyImage.StartAnimation(toastyIn));

                command = "Mortal Kombat Kode";
            }

            if (_commandBuffer.ProcessCommandList(CodeHelper.KonamiCode))
            {
                MediaPlayer.Create(this, Resource.Raw.contra).Start();

                RunOnUiThread(() => contraImage.StartAnimation(moveLefttoRight));
                command = "Konami Code";
            }

            RunOnUiThread(() =>
                {
                    tvLastCommand.Text = command;
                    tvCommandList.Text = String.Join(Environment.NewLine, commandArray.Reverse());
                });
        }

        #endregion
    }
}
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Windows.Media.Control;

namespace LyricPlayer
{
    public partial class Form1 : Form
    {

        Label entireName, positionLabel;
        PictureBox thumbnail;
        GlobalSystemMediaTransportControlsSessionManager sessionManager;

        public Form1()
        {
            InitializeComponent();

            entireName = this.Controls["label1"] as Label;
            positionLabel = this.Controls["label2"] as Label;
            thumbnail = Controls["pictureBox1"] as PictureBox;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            GlobalSystemMediaTransportControlsSessionManager transportControl = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            transportControl.SessionsChanged += smtcChanged;

            sessionManager = transportControl;
            sessionManager.GetCurrentSession().MediaPropertiesChanged += musicPropertyUpdated;
        }

        private async void label1_Click(object sender, EventArgs e)
        {
            updateCurrentProperty();
        }

        private void smtcChanged(GlobalSystemMediaTransportControlsSessionManager sender, SessionsChangedEventArgs args)
        {
            Debug.WriteLine("Information changed");
            updateCurrentProperty();
        }

        private void musicPropertyUpdated(GlobalSystemMediaTransportControlsSession session, MediaPropertiesChangedEventArgs args)
        {
            updateCurrentProperty();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void updateCurrentProperty()
        {
            GlobalSystemMediaTransportControlsSession s = sessionManager.GetCurrentSession();
            GlobalSystemMediaTransportControlsSessionMediaProperties m = await s.TryGetMediaPropertiesAsync();
            GlobalSystemMediaTransportControlsSessionTimelineProperties p = s.GetTimelineProperties();
            //m.Thumbnail
            //thumbnail.Image = SetBitmap(m);
            this.Invoke(new Action(delegate () {
                entireName.Text = m.Artist + " - " + m.Title;
                positionLabel.Text = p.Position.TotalSeconds.ToString();
            }));
        }
    }
}

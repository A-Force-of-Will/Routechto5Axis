// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainView.cs" company="">
//   Copyright (c) 2022 
//   Author zstockton
// </copyright>
// <summary>
//  If this project is helpful please take a short survey at ->
//  http://ux.mastercam.com/Surveys/APISDKSupport 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Forms;

namespace RoutechToFiveAxis
{
    /// <summary> The main view. </summary>
    public partial class MainView : Form
    {
        private Main main = new Main();

        /// <summary> Initializes a new instance of the <see cref="MainView"/> class. </summary>
        public MainView() => this.InitializeComponent();

        /// <summary> The on ok click. </summary>
        ///
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e">      The event arg. </param>
        private void OnOkClick(object sender, System.EventArgs e) =>
            MessageBox.Show(Properties.Resources.MessageTile, Properties.Resources.Message);

        /// <summary> The on close click. </summary>
        ///
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e">      The event arg. </param>
        private void OnCloseClick(object sender, System.EventArgs e) => this.Close();

        private void ToLarryBtn_Click(object sender, System.EventArgs e)
        {
            main.RunToLarry();

            Close();
        }

        private void ToGrantBtn_Click(object sender, System.EventArgs e)
        {
            main.RunToGrant();

            Close();
        }

        private void ToMikeBtn_Click(object sender, System.EventArgs e)
        {
            main.RunToMike();

            Close();
        }
    }
}

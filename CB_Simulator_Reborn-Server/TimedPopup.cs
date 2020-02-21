/* Timed popups supplied by Tony Zackin (https://www.codeproject.com/script/Membership/View.aspx?mid=3971920)
 * Written February 2018
 * Acquired from https://www.codeproject.com/Tips/1228138/A-Very-Simple-Csharp-Asynchronous-Timed-Message-Bo under The Code Project Open License (CPOL) (http://www.codeproject.com/info/cpol10.aspx)
 * Line 53 as well as lines 75 to 83 has been modified by Oskar Stenberg (oskar@stenit.eu) in February 2020
 */

using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace PopUp
{
    /// <summary>
    /// A timed message box.
    /// </summary>
    /// <remarks>You can change some of the attributes of the underlying text box via the Set method.<para/>
    /// However, you can access the public TextBox object (as TextBox or tb) directly and modify it in your code.</remarks>
    /// <example>
    /// Definition: static TimedPopUp popUp = new TimedPopUp(); <para />
    /// Usage: popUp.Set("Hello"[, delay][, width][, height][, fontName][, fontSize][, fontStyle]); <para />
    /// popUp.Show();
    /// </example>
    partial class TimedPopUp : Form
    {
        public TimedPopUp()
        {
            InitializeComponent();
        }

        public TextBox TextBox { get => tb; }
        static int _waitTime;

        /// <summary>
        /// Initialize the values used to display the message box for a specific number of milliseconds.
        /// </summary>
        /// <param name="msg">The message to display in the text box.</param>
        /// <param name="caption">Optional: The title string of the text box. 
        /// Default = "".</param>
        /// <param name="waitTime">Optional: The time to display the message in milliseconds. 
        /// Default = 1000.</param>
        /// <param name="width">Optional: The width in pixels of the form. 
        /// Default = 600.</param>
        /// <param name="height">Optional: The height in pixels of the form. 
        /// Default = 100.</param>
        /// <param name="familyName">Optional: The font family name. 
        /// Default = "Courier New".</param>
        /// <param name="emSize">Optional: The size of the font of the text box. 
        /// Default = 12.</param>
        /// <param name="style">Optional: The sytyle of the font, viz., Regular, Bold, Italic, Underline, or Strikeout. 
        /// Default = FontStyle.Bold.</param>
        /// <remarks>Note that the Show method is used to actually display the message box.</remarks>
        public void Set(string msg, string caption = "", int waitTime = 1000, int width = 600, int height = 150,
                        string familyName = "Sans Serif", float emSize = 12, FontStyle style = FontStyle.Regular)
        {
            tb.Text = msg;
            tb.Font = new Font(familyName, emSize, style);
            this.Size = new Size(width, height);
            this.Text = caption;
            _waitTime = waitTime;
        }

        /// <summary>
        /// This is the method which is used to display the message box. The Set method must be called prior to using this.
        /// </summary>
        /// <remarks>Note that this method effectively hides the normal Show method for the form.</remarks>
        async new public void Show()
        {
            // Invoke the normal form Show method to display the message box.
            base.Show();
            // The await operator makes this asynchronous so that the caller can continue to work while the message is displayed.
            await Task.Delay(_waitTime);
            // Once the wait time has run out the form is hidden.
            this.Hide();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            this.Hide();
        }
    }
}

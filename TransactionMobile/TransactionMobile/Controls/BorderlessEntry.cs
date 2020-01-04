namespace TransactionMobile.Controls
{
    using Xamarin.Forms;
    using Xamarin.Forms.Internals;

    /// <summary>
    /// This class is inherited from Xamarin.Forms.Entry to remove the border for Entry control in the Android platform.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Entry" />
    [Preserve(AllMembers = true)]
    public class BorderlessEntry : Entry
    {
    }
}
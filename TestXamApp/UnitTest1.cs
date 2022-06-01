using ShapesBalanceXamFormsApp;
using Xamarin.Forms;

namespace TestXamApp
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestWalletReturnsSameNumberOfElements()
        {


            var wallets = new List<MainPage.Wallet>()
            {
                new MainPage.Wallet(99, Brush.Blue),
                new MainPage.Wallet(1, Brush.Brown)

            };
            var normalizedWallets = Normalization.Normalize(wallets);

            Assert.AreEqual(normalizedWallets.Count(), wallets.Count());

        }

    }
}
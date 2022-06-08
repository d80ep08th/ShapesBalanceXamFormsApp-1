using ShapesBalanceXamFormsApp;
using Xamarin.Forms;

namespace TestXamApp
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestReturnsEqualElements()
        {
            var wallets = new List<MainPage.Wallet>()
            {
                new MainPage.Wallet(99, Brush.Blue),
                new MainPage.Wallet(1, Brush.Brown)

            };
            var normalizedWallets = Normalization.Normalize(wallets);

            Assert.AreEqual(normalizedWallets.Count(), wallets.Count());
        }

        [TestMethod]
        public void TestBasic()
        {
            var wallets = new List<MainPage.Wallet>()
            {
                new MainPage.Wallet(98, Brush.Blue),
                new MainPage.Wallet(2, Brush.Brown)

            };
            var normalizedWallets = Normalization.Normalize(wallets);

            Assert.AreEqual(normalizedWallets.ElementAt(0).Percent, 98.00);
            Assert.AreEqual(normalizedWallets.ElementAt(1).Percent, 2.00);
        }

        [TestMethod]
        public void TestNormalize()
        {
            var wallets = new List<MainPage.Wallet>()
            {
                new MainPage.Wallet(99, Brush.Blue),
                new MainPage.Wallet(1, Brush.Brown)

            };
            var normalizedWallets = Normalization.Normalize(wallets);

            Assert.AreEqual(normalizedWallets.ElementAt(0).Percent, 98.00);
            Assert.AreEqual(normalizedWallets.ElementAt(1).Percent, 2.00);
        }


        [TestMethod]
        public void TestNormalize2()
        {


            var wallets = new List<MainPage.Wallet>()
            {
                new MainPage.Wallet(95, Brush.Blue),
                new MainPage.Wallet(4.5, Brush.Brown),
                new MainPage.Wallet(0.5, Brush.Yellow)

            };
            var normalizedWallets = Normalization.Normalize(wallets);

            Assert.AreEqual(normalizedWallets.ElementAt(0).Percent, 95.00);
            Assert.AreEqual(normalizedWallets.ElementAt(1).Percent, 5.00);
        }

        [TestMethod]
        public void TestNormalize3()
        {
            var wallets = new List<MainPage.Wallet>()
            {
                new MainPage.Wallet(95, Brush.Blue),
                new MainPage.Wallet(2.5, Brush.Brown),
                new MainPage.Wallet(1.5, Brush.Yellow),
                new MainPage.Wallet(1.0, Brush.Red)

            };
            var normalizedWallets = Normalization.Normalize(wallets);

            Assert.AreEqual(normalizedWallets.ElementAt(0).Percent, 93.50);
            Assert.AreEqual(normalizedWallets.ElementAt(1).Percent, 2.50);
            Assert.AreEqual(normalizedWallets.ElementAt(2).Percent, 2.00);
            Assert.AreEqual(normalizedWallets.ElementAt(3).Percent, 2.00);
        }

        [TestMethod]
        public void TestFullCircle()
        {
            var wallets = new List<MainPage.Wallet>()
            {
                new MainPage.Wallet(99.5, Brush.Blue),
                new MainPage.Wallet(0.5, Brush.Brown)

            };
            var normalizedWallets = Normalization.Normalize(wallets);

            Assert.AreEqual(normalizedWallets.ElementAt(0).Percent, 100);
        }

        [TestMethod]
        public void TestBeautifyAmount()
        {

            var wallets = new List<MainPage.Wallet>()
            {
                new MainPage.Wallet(90000, Brush.Blue),
                new MainPage.Wallet(10000, Brush.Brown)

            };

            var balance = wallets.Sum(x => x.CryptoValue);
            
            var beautifiedAmount = Beautification.BeautifyAmount(balance.ToString());

            Assert.AreEqual("100,000", beautifiedAmount);
        }
    }


    
}
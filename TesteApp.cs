using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.MultiTouch;

namespace TesteMobile;

public class TestsApp
{
    public static string sauce_username = Environment.GetEnvironmentVariable("sauce_username");
    public static string sauce_acess_key = Environment.GetEnvironmentVariable("sauce_acess_key");
    public Uri uri = new Uri($"https://{sauce_username}:{sauce_acess_key}@ondemand.us-west-1.saucelabs.com:443/wd/hub");
    public AndroidDriver<AndroidElement> driver {get;set;}

    [SetUp]
    public void Setup()
    {
        var options = new AppiumOptions();
        options.AddAdditionalCapability(MobileCapabilityType.PlatformName,"Android");
        options.AddAdditionalCapability(MobileCapabilityType.PlatformVersion,"9.0");
        options.AddAdditionalCapability(MobileCapabilityType.DeviceName,"Samsung Galaxy S9 FHD GoogleAPI Emulator");
        options.AddAdditionalCapability(MobileCapabilityType.App,"storage:filename=mda-2.0.0-21.apk");
        options.AddAdditionalCapability("appPackage", "com.saucelabs.mydemoapp.android");
        options.AddAdditionalCapability("appActivity", "com.saucelabs.mydemoapp.android.view.activities.SplashActivity");
        options.AddAdditionalCapability("newCommandTimeout", 90);

        driver = new AndroidDriver<AndroidElement>(remoteAddress: uri, driverOptions: options, commandTimeout: TimeSpan.FromSeconds(180));
    }

    [TearDown]
    public void Finalizar()
    {
        if(driver == null) 
        return;

        driver.Quit();
    }

    [Test]
    public void SelecionarProdutos()
    {
        //Aguardando carregamento
        Assert.That(driver.FindElement(MobileBy.AccessibilityId("App logo and name")).Displayed, Is.True);

        driver.FindElement(MobileBy.AccessibilityId("Sauce Labs Backpack")).Click();
        
        //verificar o nome do produto na tela do produto
        string tituloProduto = driver.FindElement(MobileBy.Id("com.saucelabs.mydemoapp.android:id/productTV")).Text;
        Assert.That(tituloProduto, Is.EqualTo("Sauce Labs Backpack"));

        //verificar o preço do produto na tela do produto
        string precoProduto = driver.FindElement(MobileBy.Id("com.saucelabs.mydemoapp.android:id/priceTV")).Text;
        Assert.That(precoProduto, Is.EqualTo("$ 29.99"));

        // arrasta pra cima
        TouchAction touchAction = new TouchAction(driver); // instancia objeto para reduzir gestos
        touchAction.Press(600, 1700);
        touchAction.MoveTo(600,700);
        touchAction.Release();
        touchAction.Perform();

        //adicionar mais de um
        driver.FindElement(MobileBy.Id("com.saucelabs.mydemoapp.android:id/plusIV")).Click();

        //adicionar no carrinho
        driver.FindElement(MobileBy.AccessibilityId("Tap to add product to cart")).Click();

        //clicar no carrinho
        driver.FindElement(MobileBy.Id("com.saucelabs.mydemoapp.android:id/cartTV")).Click();
        Thread.Sleep(3000);

        // validar nome do produto no carrinho
        tituloProduto = driver.FindElement(MobileBy.Id("com.saucelabs.mydemoapp.android:id/titleTV")).Text;
        Assert.That(tituloProduto, Is.EqualTo("Sauce Labs Backpack"));
        Thread.Sleep(3000);

        // validar qtde do produto no carrinho
        string qtdeProduto = driver.FindElement(MobileBy.Id("com.saucelabs.mydemoapp.android:id/itemsTV")).Text;
        Assert.That(qtdeProduto, Is.EqualTo("2 Items"));

        // validar o preço do produto no carrinho
        precoProduto = driver.FindElement(MobileBy.Id("com.saucelabs.mydemoapp.android:id/totalPriceTV")).Text;
        Assert.That(precoProduto, Is.EqualTo("$ 59.98"));


    }
}
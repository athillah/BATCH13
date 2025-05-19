using System;
using System.Runtime.CompilerServices;
using static System.Console;

public enum Status { Idle, Mixing, Done }

public delegate void MixingIsDoneHandler();
public delegate void MixerIsIdleHandler();
public delegate void SellProductsHandler(int toSell, ProductSeller seller);
public delegate void BuyStocksHandler(int toBuy, StockBuyer buyer);
public delegate void DisplaySummaryHandler();

public class Mixer {
    public event MixingIsDoneHandler MixingIsDone;
    public event MixerIsIdleHandler MixerIsIdle;
    private Status _status { get; set; }
    public void DoneMixing() {
        MixingIsDone.Invoke();
        _status = Status.Idle;
    }
    public void IdleMixer() {
        MixerIsIdle.Invoke();
        _status = Status.Mixing;
    }
    public void Feed(Feeder feeder) {
        if (_status == Status.Idle && feeder.Stocks != 0) {
            IdleMixer();
        } else {
            WriteLine("There is a process needs to be done.");
        }
    }
    public void Discharge() {
        if (_status == Status.Done) {
            DoneMixing();
        } else {
            WriteLine("There is no product to discharge.");
        }
    }

    public void Process() {
        if (_status == Status.Mixing) {
            WriteLine("Done Mixing");
            _status = Status.Done;
        } else {
            WriteLine("There is no process to do.");
        }
    }
    public void OnDisplayingSummary() {
        WriteLine($"Mixer Status:\t{_status}");
    }
}

public class Feeder {
    public event BuyStocksHandler BuyingStocks;
    public int Stocks { get; set; }
    public Feeder(int stock)
    {
        Stocks = stock;
    }
    public void OnMixerIsIdle() {
        //Stock--;
        WriteLine("Mixer feeded.");
        Stocks--;
    }
    public void BuyStocks (int toBuy, StockBuyer buyer) {
        WriteLine("Stock bought.");
        BuyingStocks.Invoke(toBuy, buyer);
        Stocks += toBuy;
    }
    public void OnDisplayingSummary() {
        WriteLine($"Remaining stock(s):\t{Stocks}");
    }
}

public class Discharger {
    public event SellProductsHandler SellingProducts;
    public int Products = 0;
    public void OnMixingIsDone() {
        //Product++;
        WriteLine("Discharged");
        Products++;
    }
    public void SellProducts(int toSell, ProductSeller seller) {
        if (Products != 0 && toSell > 0) {
            WriteLine("Product sold.");
            SellingProducts.Invoke(toSell, seller);
            Products -= toSell;
        }
    }
    public void OnDisplayingSummary() {
        WriteLine($"Product(s) ready:\t{Products}");
    }
}

public class ProductSeller {
    private double _totalRevenue;
    public double LastRevenue = 0;
    private int _soldProducts = 0;
    public void OnSellingProducts(int products, ProductSeller seller) {
        _soldProducts += products;
        LastRevenue = ((products + products) * products / (products - (products / 2) + 1)) * 15;
        _totalRevenue += LastRevenue;
        WriteLine($"Sold {products} products for {LastRevenue}.");
    }
    public void OnDisplayingSummary() {
        WriteLine($"Product(s) sold:\t{_soldProducts}");
        WriteLine($"Total revenue:\t\t{_totalRevenue}");
    }
}

public class StockBuyer {
    private double _totalSpending;
    public double LastSpending = 0;
    private int _boughtStocks = 0;
    public void OnBuyingStocks(int stocks, StockBuyer buyer) {
        _boughtStocks += stocks;
        Random rnd = new Random();
        double mod = 0.8 + rnd.NextDouble() * (1.2 - 0.8);
        LastSpending = stocks * 10 * mod;
        _totalSpending += LastSpending;
        WriteLine($"Bought {stocks} stocks for {LastSpending}.");
    }
    public void OnDisplayingSummary() {
        WriteLine($"Stock(s) bought:\t{_boughtStocks}");
        WriteLine($"Total spending:\t\t{_totalSpending}");
    }
}

public class Accountant {
    private double _debit;
    private double _credit;
    private double GetBalance() {
        return _debit - _credit;
    }
    private void GainDebit(double debit) {
        _debit += debit;
    }
    private void GainCredit(double credit) {
        _credit += credit;
    }
    public void OnSellingProducts(int products, ProductSeller seller) {
        GainDebit(seller.LastRevenue);
    }
    public void OnBuyingStocks(int stocks, StockBuyer buyer) {
        GainCredit(buyer.LastSpending);
    }
    public void OnDisplayingSummary() {
        WriteLine($"Total balance:\t\t{GetBalance()}");
    }
}

public class Menu () {
    public event DisplaySummaryHandler DisplayingSummary;
    public void DisplayMenu() {
        WriteLine("\n==MENU===\n 1. Feed Mixer\n 2. Do Process\n 3. Discharge Product");
        WriteLine(" 4. Sell Product\n 5. Buy Stock\n 6. Summary\n 7. Exit");
    }
    public void DisplaySummary()
    {
        DisplayingSummary.Invoke();
    }
    public int GetCom(int com) {
        return int.Parse(ReadLine());
    }
}

class Program {
    static void Main() {
        Mixer mixer = new Mixer();
        Feeder feeder = new Feeder(10);
        Discharger discharger = new Discharger();
        Accountant accountant = new Accountant();
        ProductSeller seller = new ProductSeller();
        StockBuyer buyer = new StockBuyer();
        Menu menu = new Menu();

        mixer.MixerIsIdle += feeder.OnMixerIsIdle;
        mixer.MixingIsDone += discharger.OnMixingIsDone;

        discharger.SellingProducts += seller.OnSellingProducts;
        feeder.BuyingStocks += buyer.OnBuyingStocks;

        feeder.BuyingStocks += accountant.OnBuyingStocks;
        discharger.SellingProducts += accountant.OnSellingProducts;

        menu.DisplayingSummary += mixer.OnDisplayingSummary;
        menu.DisplayingSummary += feeder.OnDisplayingSummary;
        menu.DisplayingSummary += discharger.OnDisplayingSummary;
        menu.DisplayingSummary += seller.OnDisplayingSummary;
        menu.DisplayingSummary += buyer.OnDisplayingSummary;
        menu.DisplayingSummary += accountant.OnDisplayingSummary;

        int Com = 0;
        bool Exit = false;
        Clear();
        
        while (!Exit) {
            menu.DisplayMenu();
            Com = int.Parse(ReadLine());
            Clear();
            switch (Com) {
                case 1:
                    mixer.Feed(feeder);
                    break;
                case 2:
                    mixer.Process();
                    break;
                case 3:
                    mixer.Discharge();
                    break;
                case 4:
                    WriteLine($"How many you want to sell? (Ready product: {discharger.Products})");
                    int toSell = int.Parse(ReadLine());
                    if (toSell > 0) {
                        discharger.SellProducts(toSell, seller);
                        break;
                    }
                    WriteLine("Please input a valid number");
                    break;
                case 5:
                    WriteLine($"How many you want to buy? (Remaining stock: {feeder.Stocks})");
                    int toBuy = int.Parse(ReadLine());
                    if (toBuy > 0) {
                        feeder.BuyStocks(toBuy, buyer);
                        break;
                    }
                    WriteLine("Please input a valid number");
                    break;
                case 6:
                    menu.DisplaySummary();
                    break;
                case 7:
                    Exit = true;
                    break;
                default:
                    break;
            }
            WriteLine("\n");
        }
    }
}
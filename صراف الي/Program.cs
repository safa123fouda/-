using System;
namespace صراف_الي
{
    using System;

    namespace ATMSystem
    {
        // الصنف الأساسي Account
        public class Account
        {
            public int AccountNumber { get; set; }
            public double Balance { get; protected set; }

            public Account(int accountNumber, double initialBalance)
            {
                AccountNumber = accountNumber;
                Balance = initialBalance;
            }

            public virtual void Deposit(double amount)
            {
                if (amount > 0)
                {
                    Balance += amount;
                    Console.WriteLine("Deposit successful.");
                }
                else
                {
                    Console.WriteLine("Invalid amount. Please enter a positive value.");
                }
            }

            public virtual void Withdraw(double amount)
            {
                if (amount > 0 && amount <= Balance)
                {
                    Balance -= amount;
                    Console.WriteLine("Withdrawal successful.");
                }
                else
                {
                    Console.WriteLine("Insufficient balance or invalid amount.");
                }
            }

            public void DisplayBalance()
            {
                Console.WriteLine($"Current Balance: {Balance:C}");
            }
        }

        // صنف التوفير
        public class SavingAccount : Account
        {
            private const double InterestRate = 0.05;

            public SavingAccount(int accountNumber, double initialBalance) : base(accountNumber, initialBalance)
            {
                ApplyInterest();
            }

            private void ApplyInterest()
            {
                Balance += Balance * InterestRate;
            }
        }

        // صنف الحساب الجاري
        public class CheckingAccount : Account
        {
            private const double WithdrawalFee = 1.0;

            public CheckingAccount(int accountNumber, double initialBalance) : base(accountNumber, initialBalance) { }

            public override void Withdraw(double amount)
            {
                double totalAmount = amount + WithdrawalFee;
                if (amount > 0 && totalAmount <= Balance)
                {
                    Balance -= totalAmount;
                    Console.WriteLine($"Withdrawal successful. A fee of {WithdrawalFee:C} was applied.");
                }
                else
                {
                    Console.WriteLine("Insufficient balance or invalid amount.");
                }
            }
        }

        // صنف الصراف الآلي
        public class ATM
        {
            private Account[] accounts;

            public ATM(Account[] accounts)
            {
                this.accounts = accounts;
            }

            public void Start()
            {
                while (true)
                {
                    Console.WriteLine("Enter your account number:");
                    if (!int.TryParse(Console.ReadLine(), out int accountNumber))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid account number.");
                        continue;
                    }

                    Account account = FindAccount(accountNumber);

                    if (account == null)
                    {
                        Console.WriteLine("Account not found.");
                        continue;
                    }

                    while (true)
                    {
                        Console.WriteLine("1. Deposit");
                        Console.WriteLine("2. Withdraw");
                        Console.WriteLine("3. Display Balance");
                        Console.WriteLine("4. Log Out");
                        Console.Write("Choose an option: ");
                        string option = Console.ReadLine();

                        switch (option)
                        {
                            case "1":
                                Console.WriteLine("Enter deposit amount:");
                                if (double.TryParse(Console.ReadLine(), out double depositAmount))
                                {
                                    account.Deposit(depositAmount);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid amount.");
                                }
                                break;
                            case "2":
                                Console.WriteLine("Enter withdrawal amount:");
                                if (double.TryParse(Console.ReadLine(), out double withdrawalAmount))
                                {
                                    account.Withdraw(withdrawalAmount);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid amount.");
                                }
                                break;
                            case "3":
                                account.DisplayBalance();
                                break;
                            case "4":
                                Console.WriteLine("Logging out...");
                                break; // خروج من القائمة الداخلية للعودة لإدخال رقم حساب جديد
                            default:
                                Console.WriteLine("Invalid option. Please try again.");
                                break;
                        }

                        if (option == "4") break; // إنهاء الحلقة الداخلية والعودة لإدخال رقم حساب جديد
                    }
                }
            }

            private Account FindAccount(int accountNumber)
            {
                foreach (Account account in accounts)
                {
                    if (account.AccountNumber == accountNumber)
                    {
                        return account;
                    }
                }
                return null;
            }
        }

        // البرنامج الرئيسي
        class Program
        {
            static void Main(string[] args)
            {
                Account[] accounts = new Account[]
                {
                new SavingAccount(101, 500),
                new CheckingAccount(102, 1000),
                new SavingAccount(103, 200),
                new CheckingAccount(104, 750)
                };

                ATM atm = new ATM(accounts);
                atm.Start();
            }
        }
    }
}

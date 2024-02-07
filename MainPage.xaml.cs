using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Calculator
{
    public sealed partial class MainPage : Page
    {
        private string previousResult = "0";
        private string selectedOperation = "";

        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_Result.Text == "0" || selectedOperation == "=" || TextBox_Result.Text == "Cannot divide by zero")
            {
                TextBox_Result.Text = "";
                selectedOperation = "";
            }

            // Lägger till klickade knappens innehåll till resultattexten
            TextBox_Result.Text += (string)((Button)sender).Content;
        }

        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            string operation = (string)((Button)sender).Content;

            if (operation == "+" || operation == "-" || operation == "x" || operation == "/")
            {
                // Försöker konvertera det tidigare resultatet till en double
                if (double.TryParse(previousResult, out double num1))
                {
                    // Om det redan finns en pågående operation
                    if (!string.IsNullOrEmpty(selectedOperation))
                    {
                        // Utför den pågående operationen med det nya numret
                        double.TryParse(TextBox_Result.Text, out double num2);
                        double result = CalculateResult(num1, num2, selectedOperation);
                        TextBox_Result.Text = result.ToString();
                    }

                    // Uppdaterar det föregående resultatet och den valda operationen
                    previousResult = TextBox_Result.Text;
                    selectedOperation = operation;

                    textBlock_RecentInput.Text = previousResult + selectedOperation;

                    TextBox_Result.Text = "";
                }
                else
                {
                    textBlock_RecentInput.Text = "Invalid input";
                }
            }
            else if (operation == "=")
            {
                // Sparar det nuvarande resultatet
                if (double.TryParse(previousResult, out double num1))
                {
                    // Om det finns en pågående operation
                    if (!string.IsNullOrEmpty(selectedOperation))
                    {
                        double.TryParse(TextBox_Result.Text, out double num2);

                        // Kontrollerar för division med noll innan beräkning
                        if (selectedOperation == "/" && num2 == 0)
                        {
                            TextBox_Result.Text = "Cannot divide by zero";
                        }
                        else
                        {
                            TextBox_Result.Text = CalculateResult(num1, num2, selectedOperation).ToString();
                        }
                    }
                    string oper = selectedOperation;
         
                    // Uppdaterar textBlock_RecentInput med den senaste inputen och resultatet
                    textBlock_RecentInput.Text = TextBox_Result.Text + oper;

                    // Återställer den valda operationen och nollställer textBlock_RecentInput
                    selectedOperation = "";
                    textBlock_RecentInput.Text = "";
                }
                else
                {
                    textBlock_RecentInput.Text = "Invalid input";
                }
            }
            else if (operation == "C")
            {
                // Återställer resultattexten, den valda operationen och den senaste inputen
                TextBox_Result.Text = "0";
                selectedOperation = "";
                textBlock_RecentInput.Text = "";
            }
        }

        // Metod för att utföra de olika matematiska operationerna
        private double CalculateResult(double num1, double num2, string operation)
        {
            // Använder en switch-sats för att utföra den specifika operationen
            switch (operation)
            {
                case "+":
                    return num1 + num2;
                case "-":
                    return num1 - num2;
                case "x":
                    return num1 * num2;
                case "/":
                    // Kontrollerar för att undvika division med noll
                    return num2 != 0 ? num1 / num2 : 0;
                // Om ingen giltig operation matchas, returnera 0
                default:
                    return 0;
            }
        }
    }
}

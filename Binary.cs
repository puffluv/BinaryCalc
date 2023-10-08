using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private string currentInput = "";
        private char currentOperator = ' ';
        private bool isOperatorClicked = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Number_Click(object sender, RoutedEventArgs e)
        {
            string clickedNumber = ((Button)sender).Tag.ToString();

            if (string.IsNullOrEmpty(currentInput) || isOperatorClicked)
                currentInput = clickedNumber;
            else
                currentInput += clickedNumber;

            isOperatorClicked = false;
            UpdateTextLabel();
        }

        private void Button_Operation_Click(object sender, RoutedEventArgs e)
        {
            if (!isOperatorClicked)
            {
                currentOperator = ((Button)sender).Content.ToString()[0];
                isOperatorClicked = true;
            }
        }

        private void Button_Equals_Click(object sender, RoutedEventArgs e)
        {
            int[] n1Array = BinaryStringToArray(currentInput);
            int[] n2Array = BinaryStringToArray(currentInput);

            switch (currentOperator)
            {
                case '+':
                    int[] resultArray = binaryAdd(n1Array, n2Array);
                    currentInput = BinaryArrayToString(resultArray);
                    UpdateTextLabel();
                    break;
                case '-':
                    resultArray = binarySub(n1Array, n2Array);
                    currentInput = BinaryArrayToString(resultArray);
                    UpdateTextLabel();
                    break;
                case '*':
                    resultArray = binaryMul(n1Array, n2Array);
                    currentInput = BinaryArrayToString(resultArray);
                    UpdateTextLabel();
                    break;
                case '/':
                    if (!n2Array.All(bit => bit == 0))
                    {
                        resultArray = binaryDiv(n1Array, n2Array);
                        currentInput = BinaryArrayToString(resultArray);
                        UpdateTextLabel();
                    }
                    else
                    {
                        textLabel.Text = "Делить на ноль нельзя!";
                    }
                    break;
            }
        }

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            currentInput = "";
            currentOperator = ' ';
            isOperatorClicked = false;
            UpdateTextLabel();
        }

        private void UpdateTextLabel()
        {
            textLabel.Text = currentInput;
        }

        public static int[] binaryAdd(int[] n1, int[] n2)
        {
            int maxLength = Math.Max(n1.Length, n2.Length);
            Array.Resize(ref n1, maxLength);
            Array.Resize(ref n2, maxLength);

            int[] result = new int[maxLength];
            int carry = 0;

            for (int i = maxLength - 1; i >= 0; i--)
            {
                int sum = n1[i] + n2[i] + carry;
                result[i] = sum % 2;
                carry = sum / 2;
            }

            if (carry > 0)
            {
                Array.Resize(ref result, maxLength + 1);
                result[0] = carry;
            }

            return result;
        }

        public static int[] binarySub(int[] n1, int[] n2)
        {
            int maxLength = Math.Max(n1.Length, n2.Length);
            Array.Resize(ref n1, maxLength);
            Array.Resize(ref n2, maxLength);

            int[] result = new int[maxLength];
            int borrow = 0;

            for (int i = maxLength - 1; i >= 0; i--)
            {
                int diff = n1[i] - n2[i] - borrow;
                if (diff < 0)
                {
                    diff += 2;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }
                result[i] = diff;
            }

            return result;
        }

        public static int[] binaryMul(int[] n1, int[] n2)
        {

            string binStr1 = string.Join("", n1);
            string binStr2 = string.Join("", n2);
            int int1 = Convert.ToInt32(binStr1, 2);
            int int2 = Convert.ToInt32(binStr2, 2);
            int product = int1 * int2;
            string resultBinStr = Convert.ToString(product, 2);
            int[] result = new int[resultBinStr.Length];

            for (int i = 0; i < resultBinStr.Length; i++)
            {
                result[i] = int.Parse(resultBinStr[i].ToString());
            }

            return result;
        }

        public static int[] binaryDiv(int[] n1, int[] n2)
        {
            string binStr1 = string.Join("", n1);
            string binStr2 = string.Join("", n2);
            int int1 = Convert.ToInt32(binStr1, 2);
            int int2 = Convert.ToInt32(binStr2, 2);

            if (int2 == 0)
            {
                return new int[8]; // Возвратить специальный флаг, чтобы позже определить деление на ноль
            }

            int quotient = int1 / int2;
            string resultBinStr = Convert.ToString(quotient, 2);
            int[] result = new int[resultBinStr.Length];

            for (int i = 0; i < resultBinStr.Length; i++)
            {
                result[i] = int.Parse(resultBinStr[i].ToString());
            }

            return result;
        }
        private int[] BinaryStringToArray(string binaryString)
        {
            int[] binaryArray = new int[8];

            for (int i = 0; i < binaryString.Length; i++)
            {
                binaryArray[i] = binaryString[i] == '1' ? 1 : 0;
            }

            return binaryArray;
        }

        // Функция для преобразования массива в строку
        private string BinaryArrayToString(int[] binaryArray)
        {
            string binaryString = string.Join("", binaryArray);
            return binaryString.PadLeft(8, '0'); // Добавляем нули слева, чтобы получить 8 бит
        }
    }
}

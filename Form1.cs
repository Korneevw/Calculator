using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        private TextBox _results;
        private string _expression;
        private List<string> _lastExpression;
        private bool _calculateAgain;
        private string[] _buttonSymbols;
        public Form1()
        {
            InitializeComponent();
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoSize = false;
            Width = 355;
            Height = 765;
            this.Text = "Calculator";
            this.Icon = Properties.Resources.calc;
            _results = new TextBox()
            {
                Location = new Point(Left + 10, Top + 10),
                Size = new Size(320, 50),
                Font = new Font(DefaultFont.FontFamily, 24, FontStyle.Regular),
                ReadOnly = true
            };
            Controls.Add(_results);
            Button clear = new Button()
            {
                Location = new Point(120, 615),
                Size = new Size(100, 100),
                Text = "C",
                Font = new Font(DefaultFont.FontFamily, 50, FontStyle.Regular)
            };
            clear.Click += (object sender, EventArgs e) => ChangeExpression("");
            Controls.Add(clear);
            _expression = "";
            _buttonSymbols = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "+", "-", "*", "/", "=" };
            ButtonCreator(3, 5, 10, 10, Left + 10, _results.Bottom + 10, 100, 100);
        }
        private void ButtonCreator(int arrayWidth, int arrayHeight, int horizontalSpace, int verticalSpace, int x, int y, int width, int height)
        {
            int posX = x;
            int posY = y;
            int index = 0;
            for (int i = 0; i < arrayHeight; i++)
            {
                for (int j = 0; j < arrayWidth; j++)
                {
                    Button b = new Button()
                    {
                        Location = new Point(posX, posY),
                        Size = new Size(width, height),
                        Text = _buttonSymbols[index],
                        Font = new Font(DefaultFont.FontFamily, 50, FontStyle.Regular)
                    };
                    b.Click += Button_Click;
                    Controls.Add(b);
                    index++;
                    posX += width + horizontalSpace;
                }
                posX = x;
                posY += height + verticalSpace;
            }
        }
        private void Button_Click(object sender, EventArgs e)
        {
            if (_expression.Length < 18)
            {
                if ((sender as Button).Text == "=")
                {
                    if (_calculateAgain == true)
                    {
                        Calculate(_lastExpression);
                    }
                    else if (_expression.Length != 0 && IsNumber(_expression[_expression.Length - 1]) == true)
                    {
                        Calculate(_expression);
                        _calculateAgain = true;
                    }
                }
                else
                {
                    _calculateAgain = false;
                    if (_expression == "" && IsNumber((sender as Button).Text) == true)
                    {
                        ChangeExpression(_expression + (sender as Button).Text);
                    }
                    else if (_expression.Length >= 1)
                    {
                        if (IsNumber((sender as Button).Text) == false && IsNumber(_expression[_expression.Length - 1]) == true)
                        {
                            ChangeExpression(_expression + (sender as Button).Text);
                        }
                        else if (IsNumber((sender as Button).Text) == true)
                        {
                            ChangeExpression(_expression + (sender as Button).Text);
                        }
                    }
                }
            }
        }
        private void ChangeExpression(string value)
        {
            _expression = value;
            _results.Text = _expression;
        }
        private void Calculate(string expression)
        {
            List<string> splitExpression = new List<string>();
            int index = 0;
            string expressionElement = "";
            while (true)
            {
                if (IsNumber(expression[index]) == true)
                {
                    expressionElement += expression[index].ToString();
                }
                else if (IsNumber(expression[index]) == false)
                {
                    splitExpression.Add(expressionElement);
                    splitExpression.Add(expression[index].ToString());
                    expressionElement = "";
                }
                if (index == expression.Length - 1)
                {
                    splitExpression.Add(expressionElement);
                    break;
                }
                else
                {
                    index++;
                }
            }
            _lastExpression = new List<string>(splitExpression);
            while (splitExpression.Count != 1)
            {
                index = 0;
                if (splitExpression.Contains("*"))
                {
                    index = splitExpression.IndexOf("*");
                    splitExpression[index - 1] = (Convert.ToInt64(splitExpression[index - 1]) * Convert.ToInt64(splitExpression[index + 1])).ToString();
                }
                else if (splitExpression.Contains("/"))
                {
                    index = splitExpression.IndexOf("/");
                    splitExpression[index - 1] = (Convert.ToInt64(splitExpression[index - 1]) / Convert.ToInt64(splitExpression[index + 1])).ToString();
                }
                else
                {
                    index = 1;
                    switch (splitExpression[1])
                    {
                        case "+":
                            splitExpression[index - 1] = (Convert.ToInt64(splitExpression[index - 1]) + Convert.ToInt64(splitExpression[index + 1])).ToString();
                            break;
                        case "-":
                            splitExpression[index - 1] = (Convert.ToInt64(splitExpression[index - 1]) - Convert.ToInt64(splitExpression[index + 1])).ToString();
                            break;
                    }
                    if (Convert.ToInt64(splitExpression[index - 1]) < 0)
                    {
                        splitExpression[index - 1] = "0";
                    }
                }
                splitExpression.RemoveAt(index + 1);
                splitExpression.RemoveAt(index);
            }
            ChangeExpression("");
            ChangeExpression(splitExpression[0]);
            _lastExpression[0] = splitExpression[0];
        }
        private void Calculate(List<string> splitExpression)
        {
            _lastExpression = new List<string>(splitExpression);
            int index = 0;
            while (splitExpression.Count != 1)
            {
                if (splitExpression.Contains("*"))
                {
                    index = splitExpression.IndexOf("*");
                    splitExpression[index - 1] = (Convert.ToInt64(splitExpression[index - 1]) * Convert.ToInt64(splitExpression[index + 1])).ToString();
                }
                else if (splitExpression.Contains("/"))
                {
                    index = splitExpression.IndexOf("/");
                    splitExpression[index - 1] = (Convert.ToInt64(splitExpression[index - 1]) / Convert.ToInt64(splitExpression[index + 1])).ToString();
                }
                else
                {
                    index = 1;
                    switch (splitExpression[1])
                    {
                        case "+":
                            splitExpression[index - 1] = (Convert.ToInt64(splitExpression[index - 1]) + Convert.ToInt64(splitExpression[index + 1])).ToString();
                            break;
                        case "-":
                            splitExpression[index - 1] = (Convert.ToInt64(splitExpression[index - 1]) - Convert.ToInt64(splitExpression[index + 1])).ToString();
                            break;
                    }
                    if (Convert.ToInt64(splitExpression[index - 1]) < 0)
                    {
                        splitExpression[index - 1] = "0";
                    }
                }
                splitExpression.RemoveAt(index + 1);
                splitExpression.RemoveAt(index);
            }
            ChangeExpression("");
            ChangeExpression(splitExpression[0]);
            _lastExpression[0] = splitExpression[0];
        }
        private bool IsNumber(string s)
        {
            return int.TryParse(s, out _);
        }
        private bool IsNumber(char c)
        {
            return int.TryParse(c.ToString(), out _);
        }
    }
}

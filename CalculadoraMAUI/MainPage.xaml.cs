using Microsoft.Maui.Controls;
using System;

namespace CalculadoraMAUI
{
	public partial class MainPage : ContentPage
	{
		private double _currentNumber;
		private string _operator;
		private bool _isOperatorPending;

		public MainPage()
		{
			InitializeComponent();
		}

		private void OnNumberClicked(object sender, EventArgs e)
		{
			var button = sender as Button;
			var digit = button.Text;

			if (ResultEntry.Text == "0" || _isOperatorPending)
			{
				ResultEntry.Text = digit;
				_isOperatorPending = false;
			}
			else
			{
				ResultEntry.Text += digit;
			}
		}

		private void OnOperatorClicked(object sender, EventArgs e)
		{
			var button = sender as Button;
			_operator = button.Text;
			_currentNumber = Double.Parse(ResultEntry.Text);
			_isOperatorPending = true;
		}

		private void OnEqualClicked(object sender, EventArgs e)
		{
			if (_isOperatorPending) return;

			double newNumber = double.Parse(ResultEntry.Text);
			double result = 0;

			switch (_operator)
			{
				case "+":
					result = _currentNumber + newNumber;
					break;
				case "−":
					result = _currentNumber - newNumber;
					break;
				case "×":
					result = _currentNumber * newNumber;
					break;
				case "÷":
					result = _currentNumber / newNumber;
					break;
			}

			ResultEntry.Text = result.ToString();
			_isOperatorPending = true;
		}

		private void OnClearClicked(object sender, EventArgs e)
		{
			ResultEntry.Text = "0";
			_currentNumber = 0;
			_operator = null;
			_isOperatorPending = false;
		}

		private void OnToggleSignClicked(object sender, EventArgs e)
		{
			double number = double.Parse(ResultEntry.Text);
			ResultEntry.Text = (-number).ToString();
		}

		private void OnPercentageClicked(object sender, EventArgs e)
		{
			double number = double.Parse(ResultEntry.Text);
			ResultEntry.Text = (number / 100).ToString();
		}

		private void OnDecimalClicked(object sender, EventArgs e)
		{
			if (!ResultEntry.Text.Contains("."))
			{
				ResultEntry.Text += ".";
			}
		}
		
	}
}
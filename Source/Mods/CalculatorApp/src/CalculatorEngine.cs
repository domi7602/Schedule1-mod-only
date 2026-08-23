using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CalculatorApp;

/// <summary>
/// Core math and state engine for CalculatorApp.
/// Uses exact decimal arithmetic to avoid floating point imprecision artifacts,
/// supports advanced operators (√, x², 1/x), repeated equals operations,
/// and calculation history management.
/// </summary>
public sealed class CalculatorEngine
{
    private const int MaxDisplayDigits = 16;
    private readonly CalculatorState _state;

    public event Action? OnStateChanged;

    public CalculatorEngine(CalculatorState state)
    {
        _state = state ?? throw new ArgumentNullException(nameof(state));
    }

    public string DisplayText => _state.DisplayText;
    public string ExpressionText => _state.ExpressionText;
    public string? PendingOperator => _state.PendingOperator;
    public bool HasPendingOperation => _state.FirstOperand.HasValue && !string.IsNullOrEmpty(_state.PendingOperator);
    public bool IsDirtyEntry => _state.DisplayText != "0" && !_state.IsNewEntry;
    public bool HasError => _state.DisplayText == "Error" || _state.DisplayText.StartsWith("Cannot") || _state.DisplayText.StartsWith("Invalid");
    public IReadOnlyList<CalculationRecord> History => _state.History;

    /// <summary>
    /// Enters a numerical digit (0 - 9).
    /// </summary>
    public void InputDigit(int digit)
    {
        if (digit < 0 || digit > 9) return;

        if (HasError || _state.IsNewEntry)
        {
            _state.DisplayText = digit.ToString(CultureInfo.InvariantCulture);
            _state.IsNewEntry = false;
        }
        else
        {
            if (_state.DisplayText == "0")
            {
                _state.DisplayText = digit.ToString(CultureInfo.InvariantCulture);
            }
            else if (_state.DisplayText.Replace("-", "").Replace(".", "").Length < MaxDisplayDigits)
            {
                _state.DisplayText += digit.ToString(CultureInfo.InvariantCulture);
            }
        }

        SaveAndNotify();
    }

    /// <summary>
    /// Enters a decimal point.
    /// </summary>
    public void InputDecimal()
    {
        if (HasError || _state.IsNewEntry)
        {
            _state.DisplayText = "0.";
            _state.IsNewEntry = false;
        }
        else if (!_state.DisplayText.Contains('.'))
        {
            _state.DisplayText += ".";
        }

        SaveAndNotify();
    }

    /// <summary>
    /// Sets an arithmetic operator (+, -, ×, ÷).
    /// If an operator was already active and a second number entered, evaluates intermediate result first.
    /// </summary>
    public void InputOperator(string op)
    {
        if (HasError) return;

        string normalizedOp = NormalizeOperator(op);

        if (_state.FirstOperand.HasValue && !_state.IsNewEntry && !string.IsNullOrEmpty(_state.PendingOperator))
        {
            // Evaluate intermediate result
            if (TryEvaluateCurrent(out decimal intermediateResult, out string? intermediateError))
            {
                _state.DisplayText = FormatNumber(intermediateResult);
                _state.FirstOperand = intermediateResult;
            }
            else
            {
                SetError(intermediateError ?? "Error");
                return;
            }
        }
        else
        {
            if (decimal.TryParse(_state.DisplayText, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal currentVal))
            {
                _state.FirstOperand = currentVal;
            }
            else
            {
                _state.FirstOperand = 0m;
            }
        }

        _state.PendingOperator = normalizedOp;
        _state.ExpressionText = $"{FormatNumber(_state.FirstOperand.Value)} {normalizedOp}";
        _state.IsNewEntry = true;

        SaveAndNotify();
    }

    /// <summary>
    /// Calculates the final result for the current expression and stores it into history.
    /// Supports repeated equals pressing to repeat the last calculation.
    /// </summary>
    public void InputEquals()
    {
        if (HasError) return;

        if (_state.FirstOperand.HasValue && !string.IsNullOrEmpty(_state.PendingOperator))
        {
            if (decimal.TryParse(_state.DisplayText, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal secondOperand))
            {
                if (TryCompute(_state.FirstOperand.Value, secondOperand, _state.PendingOperator, out decimal result, out string? errorMessage))
                {
                    string formattedResult = FormatNumber(result);
                    string fullExpression = $"{FormatNumber(_state.FirstOperand.Value)} {_state.PendingOperator} {FormatNumber(secondOperand)}";

                    // Record in history
                    AddHistoryRecord(fullExpression, formattedResult);

                    _state.LastSecondOperand = secondOperand;
                    _state.LastOperator = _state.PendingOperator;
                    _state.ExpressionText = $"{fullExpression} =";
                    _state.DisplayText = formattedResult;
                    _state.FirstOperand = null;
                    _state.PendingOperator = null;
                    _state.IsNewEntry = true;
                }
                else
                {
                    SetError(errorMessage ?? "Error", $"{FormatNumber(_state.FirstOperand.Value)} {_state.PendingOperator} {FormatNumber(secondOperand)} =");
                    return;
                }
            }
        }
        else if (_state.LastSecondOperand.HasValue && !string.IsNullOrEmpty(_state.LastOperator))
        {
            // Repeated equals operation: repeat last operator with last second operand
            if (decimal.TryParse(_state.DisplayText, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal currentVal))
            {
                if (TryCompute(currentVal, _state.LastSecondOperand.Value, _state.LastOperator, out decimal result, out string? errorMessage))
                {
                    string formattedResult = FormatNumber(result);
                    string fullExpression = $"{FormatNumber(currentVal)} {_state.LastOperator} {FormatNumber(_state.LastSecondOperand.Value)}";

                    AddHistoryRecord(fullExpression, formattedResult);

                    _state.ExpressionText = $"{fullExpression} =";
                    _state.DisplayText = formattedResult;
                    _state.IsNewEntry = true;
                }
                else
                {
                    SetError(errorMessage ?? "Error");
                    return;
                }
            }
        }

        SaveAndNotify();
    }

    /// <summary>
    /// Percentage calculation.
    /// If an operator is pending (+ / -), computes percentage of the first operand.
    /// Otherwise, divides the current number by 100.
    /// </summary>
    public void InputPercent()
    {
        if (HasError) return;

        if (decimal.TryParse(_state.DisplayText, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal currentVal))
        {
            decimal resultVal;
            if (_state.FirstOperand.HasValue && (_state.PendingOperator == "+" || _state.PendingOperator == "-"))
            {
                resultVal = _state.FirstOperand.Value * (currentVal / 100m);
            }
            else
            {
                resultVal = currentVal / 100m;
            }

            _state.DisplayText = FormatNumber(resultVal);
            _state.IsNewEntry = false;
            SaveAndNotify();
        }
    }

    /// <summary>
    /// Inverts the sign of the currently displayed number (±).
    /// </summary>
    public void InputNegate()
    {
        if (HasError || _state.DisplayText == "0") return;

        if (_state.DisplayText.StartsWith('-'))
        {
            _state.DisplayText = _state.DisplayText.Substring(1);
        }
        else
        {
            _state.DisplayText = "-" + _state.DisplayText;
        }

        SaveAndNotify();
    }

    /// <summary>
    /// Computes the square root (√) of the currently displayed number.
    /// </summary>
    public void InputSquareRoot()
    {
        if (HasError) return;

        if (decimal.TryParse(_state.DisplayText, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal val))
        {
            if (val < 0m)
            {
                SetError("Invalid input");
                return;
            }

            double dVal = (double)val;
            double sqrt = Math.Sqrt(dVal);
            decimal result = (decimal)sqrt;

            string formatted = FormatNumber(result);
            _state.ExpressionText = $"√({FormatNumber(val)}) =";
            _state.DisplayText = formatted;
            _state.IsNewEntry = true;
            AddHistoryRecord($"√({FormatNumber(val)})", formatted);
            SaveAndNotify();
        }
    }

    /// <summary>
    /// Computes the square (x²) of the currently displayed number.
    /// </summary>
    public void InputSquare()
    {
        if (HasError) return;

        if (decimal.TryParse(_state.DisplayText, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal val))
        {
            decimal result = val * val;
            string formatted = FormatNumber(result);
            _state.ExpressionText = $"sqr({FormatNumber(val)}) =";
            _state.DisplayText = formatted;
            _state.IsNewEntry = true;
            AddHistoryRecord($"sqr({FormatNumber(val)})", formatted);
            SaveAndNotify();
        }
    }

    /// <summary>
    /// Computes the reciprocal (1/x) of the currently displayed number.
    /// </summary>
    public void InputReciprocal()
    {
        if (HasError) return;

        if (decimal.TryParse(_state.DisplayText, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal val))
        {
            if (val == 0m)
            {
                SetError("Cannot divide by 0");
                return;
            }

            decimal result = 1m / val;
            string formatted = FormatNumber(result);
            _state.ExpressionText = $"1/({FormatNumber(val)}) =";
            _state.DisplayText = formatted;
            _state.IsNewEntry = true;
            AddHistoryRecord($"1/({FormatNumber(val)})", formatted);
            SaveAndNotify();
        }
    }

    /// <summary>
    /// Deletes the last character of the current input (Backspace).
    /// </summary>
    public void InputBackspace()
    {
        if (HasError || _state.IsNewEntry)
        {
            _state.DisplayText = "0";
            _state.IsNewEntry = true;
        }
        else if (_state.DisplayText.Length > 1)
        {
            _state.DisplayText = _state.DisplayText.Substring(0, _state.DisplayText.Length - 1);
            if (_state.DisplayText == "-" || _state.DisplayText == "-0")
            {
                _state.DisplayText = "0";
            }
        }
        else
        {
            _state.DisplayText = "0";
        }

        SaveAndNotify();
    }

    /// <summary>
    /// Clear action:
    /// If current entry is dirty (non-zero and actively typing), clears current entry to 0 (Clear Entry / C).
    /// Otherwise performs an All Clear (AC).
    /// </summary>
    public void InputClear()
    {
        if (IsDirtyEntry)
        {
            _state.DisplayText = "0";
            _state.IsNewEntry = true;
        }
        else
        {
            ClearAll();
        }

        SaveAndNotify();
    }

    /// <summary>
    /// Resets all calculator state (All Clear / AC).
    /// </summary>
    public void ClearAll()
    {
        _state.DisplayText = "0";
        _state.ExpressionText = string.Empty;
        _state.FirstOperand = null;
        _state.PendingOperator = null;
        _state.LastSecondOperand = null;
        _state.LastOperator = null;
        _state.IsNewEntry = true;
        SaveAndNotify();
    }

    /// <summary>
    /// Directly sets a numerical value into the display (e.g. from Cash balance, Bank balance, or Clipboard paste).
    /// </summary>
    public void SetDisplayValue(decimal value, string? customExpression = null)
    {
        _state.DisplayText = FormatNumber(value);
        if (!string.IsNullOrEmpty(customExpression))
        {
            _state.ExpressionText = customExpression;
        }
        _state.IsNewEntry = true;
        SaveAndNotify();
    }

    /// <summary>
    /// Attempts to parse and paste arbitrary clipboard text into the calculator.
    /// Supports both standard InvariantCulture ("1234.56") and European formats ("1.234,56" / "1234,56").
    /// </summary>
    public bool TryPaste(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return false;

        string trimmed = text.Trim();
        // Remove currency symbols & spaces
        string cleaned = trimmed.Replace("$", "").Replace("€", "").Replace("£", "").Replace("¥", "").Replace(" ", "");

        // 1. Try InvariantCulture standard
        if (decimal.TryParse(cleaned, NumberStyles.Number | NumberStyles.Float, CultureInfo.InvariantCulture, out decimal val))
        {
            SetDisplayValue(val, $"Pasted ({FormatNumber(val)})");
            return true;
        }

        // 2. Try European comma decimal (e.g. "1234,56" or "1.234,56")
        var deCulture = CultureInfo.GetCultureInfo("de-DE");
        if (decimal.TryParse(cleaned, NumberStyles.Number | NumberStyles.Float, deCulture, out val))
        {
            SetDisplayValue(val, $"Pasted ({FormatNumber(val)})");
            return true;
        }

        // 3. Try US comma thousand separator (e.g. "1,234.56")
        if (decimal.TryParse(cleaned.Replace(",", ""), NumberStyles.Number | NumberStyles.Float, CultureInfo.InvariantCulture, out val))
        {
            SetDisplayValue(val, $"Pasted ({FormatNumber(val)})");
            return true;
        }

        return false;
    }

    /// <summary>
    /// Restores a past calculation record's result into the current active input.
    /// </summary>
    public void RestoreHistoryResult(CalculationRecord record)
    {
        if (record == null || string.IsNullOrEmpty(record.Result)) return;

        _state.DisplayText = record.Result;
        _state.ExpressionText = $"Ans ({record.Expression}) =";
        _state.FirstOperand = null;
        _state.PendingOperator = null;
        _state.LastSecondOperand = null;
        _state.LastOperator = null;
        _state.IsNewEntry = true;
        SaveAndNotify();
    }

    /// <summary>
    /// Deletes a specific history record.
    /// </summary>
    public void DeleteHistoryRecord(string id)
    {
        if (string.IsNullOrEmpty(id)) return;
        _state.History.RemoveAll(r => r.Id == id);
        SaveAndNotify();
    }

    /// <summary>
    /// Clears all calculation history records.
    /// </summary>
    public void ClearHistory()
    {
        _state.History.Clear();
        SaveAndNotify();
    }

    /// <summary>
    /// Returns history records filtered by a search query (matches expression, result, or date).
    /// Uses StringComparison.OrdinalIgnoreCase to avoid per-character allocation.
    /// </summary>
    public IEnumerable<CalculationRecord> GetFilteredHistory(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return _state.History;
        }

        string q = query.Trim();
        return _state.History.Where(r =>
            (!string.IsNullOrEmpty(r.Expression) && r.Expression.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0) ||
            (!string.IsNullOrEmpty(r.Result) && r.Result.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0));
    }

    private void AddHistoryRecord(string expression, string result)
    {
        var record = new CalculationRecord
        {
            Expression = expression,
            Result = result,
            Timestamp = DateTime.UtcNow
        };
        _state.History.Insert(0, record);

        // Cap history at 100 entries
        if (_state.History.Count > 100)
        {
            _state.History.RemoveRange(100, _state.History.Count - 100);
        }
    }

    private void SetError(string message, string? expression = null)
    {
        _state.DisplayText = message;
        _state.ExpressionText = expression ?? string.Empty;
        _state.FirstOperand = null;
        _state.PendingOperator = null;
        _state.LastSecondOperand = null;
        _state.LastOperator = null;
        _state.IsNewEntry = true;
        SaveAndNotify();
    }

    private bool TryEvaluateCurrent(out decimal result, out string? errorMessage)
    {
        result = 0m;
        errorMessage = null;

        if (!_state.FirstOperand.HasValue || string.IsNullOrEmpty(_state.PendingOperator))
        {
            return false;
        }

        if (!decimal.TryParse(_state.DisplayText, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal secondOperand))
        {
            errorMessage = "Error";
            return false;
        }

        return TryCompute(_state.FirstOperand.Value, secondOperand, _state.PendingOperator, out result, out errorMessage);
    }

    private static bool TryCompute(decimal a, decimal b, string op, out decimal result, out string? errorMessage)
    {
        result = 0m;
        errorMessage = null;

        try
        {
            switch (op)
            {
                case "+":
                    result = a + b;
                    return true;
                case "-":
                    result = a - b;
                    return true;
                case "×":
                case "*":
                    result = a * b;
                    return true;
                case "÷":
                case "/":
                    if (b == 0m)
                    {
                        errorMessage = "Cannot divide by 0";
                        return false;
                    }
                    result = a / b;
                    return true;
                default:
                    errorMessage = "Error";
                    return false;
            }
        }
        catch (OverflowException)
        {
            errorMessage = "Overflow";
            return false;
        }
        catch (Exception)
        {
            errorMessage = "Error";
            return false;
        }
    }

    private static string NormalizeOperator(string op)
    {
        return op switch
        {
            "*" => "×",
            "/" => "÷",
            _ => op
        };
    }

    public static string FormatNumber(decimal val)
    {
        // Normalize decimal to strip trailing zeroes after decimal point
        decimal normalized = val / 1.000000000000000000000000000000000m;
        string formatted = normalized.ToString("0.################", CultureInfo.InvariantCulture);

        if (formatted.Length > MaxDisplayDigits)
        {
            // Truncate to MaxDisplayDigits without exponential mess for large numbers where possible
            if (formatted.Contains('.'))
            {
                int dotIdx = formatted.IndexOf('.');
                if (dotIdx < MaxDisplayDigits)
                {
                    formatted = formatted.Substring(0, Math.Min(formatted.Length, MaxDisplayDigits)).TrimEnd('.');
                }
            }
        }

        return formatted;
    }

    private void SaveAndNotify()
    {
        _state.Save();
        OnStateChanged?.Invoke();
    }
}

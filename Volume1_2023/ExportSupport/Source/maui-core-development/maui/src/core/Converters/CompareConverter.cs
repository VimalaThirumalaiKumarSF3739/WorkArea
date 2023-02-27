using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Converters
{
    /// <summary>
	/// This class converts true object or false object or boolean based on the comparison result
    /// </summary>
    public class CompareConverter : IMarkupExtension<IValueConverter>, IValueConverter
    {
        /// <summary>
        /// Comparison operator
        /// </summary>
        public enum OperatorType
        {
            /// <summary>
            /// Not Equal Operator
            /// </summary>
            NotEqual,

            /// <summary>
            /// Smaller Operator
            /// </summary>
            Smaller,

            /// <summary>
            /// Smaller or Equal Operator
            /// </summary>
            SmallerOrEqual,

            /// <summary>
            /// Equal Operator
            /// </summary>
            Equal,

            /// <summary>
            /// Greater Operator
            /// </summary>
            Greater,

            /// <summary>
            /// Greater or Equal Operator
            /// </summary>
            GreaterOrEqual,
        }

        /// <summary>
        /// Gets or sets the value of comparing value.
        /// </summary>
        public IComparable? ValueForComparing { get; set; }

        /// <summary>
        /// Gets or sets the value of comparison operator.
        /// </summary>
        public OperatorType ComparisonOperator { get; set; }

        /// <summary>
        /// Gets or sets the value to be returned when the comparison is true
        /// </summary>
        public object? TrueValueObject { get; set; }

        /// <summary>
        /// Gets or sets the value to be returned when the comparison is false
        /// </summary>
        public object? FalseValueObject { get; set; }

        /// <summary>
        /// The Mode Option either Boolean or Object
        /// </summary>
        enum ModeOptions
        {
            Boolean,
            Object
        }

        ModeOptions modeOption;

        /// <summary>
        /// Converts an object that implements IComparable to a specified object or a boolean based on a comparison result.
        /// </summary>
        /// <param name="value">The value must be the type of color</param>
        /// <param name="targetType">The type of target property</param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used</param>
        /// <param name="culture">The culture to use in the converter, not used</param>
        /// <returns>Returns the true object or false object or boolean value based on the comparison result</returns>
        /// <exception cref="ArgumentException">Exception is thrown when the value type is null or not a type of comparable interface</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ValueForComparing == null)
            {
                throw new ArgumentNullException("Comparing Value should not be null", nameof(ValueForComparing));
            }
            if (value is not IComparable)
            {
                throw new ArgumentException("is expected to implement IComparable interface.", nameof(value));
            }
            if (TrueValueObject != null)
            {
                modeOption = ModeOptions.Object;
            }

            var valueIComparable = (IComparable)value;
            var result = valueIComparable.CompareTo(ValueForComparing);
            object resultMode;
            switch (ComparisonOperator)
            {
                case OperatorType.Smaller:
                    resultMode = CheckCondition(result < 0);
                    break;
                case OperatorType.SmallerOrEqual:
                    resultMode = CheckCondition(result <= 0);
                    break;
                case OperatorType.Equal:
                    resultMode = CheckCondition(result == 0);
                    break;
                case OperatorType.NotEqual:
                    resultMode = CheckCondition(result != 0);
                    break;
                case OperatorType.GreaterOrEqual:
                    resultMode = CheckCondition(result >= 0);
                    break;
                case OperatorType.Greater:
                    resultMode = CheckCondition(result > 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Comparison Operator is not suppoerted",nameof(ComparisonOperator));
            }
            return resultMode;
        }

        object CheckCondition(bool comparisonResult)
        {
            if (modeOption == ModeOptions.Object)
                return comparisonResult ? TrueValueObject! : FalseValueObject!;

            return comparisonResult;
        }

        /// <summary>
        /// Converts back are impossible to revert to the original value
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay.");
        }

        /// <inheritdoc/>
        public IValueConverter ProvideValue(IServiceProvider serviceProvider)
        {
            return (IValueConverter)this;
        }
        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ((IMarkupExtension<IValueConverter>)this).ProvideValue(serviceProvider);
        }

    }
}

namespace MtgApiManager.Lib.Utility
{
    using MtgApiManager.Lib.Types;
    using System;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Contains extension methods.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Extracts the description attribute from an <see cref="Enum"/> type.
        /// </summary>
        /// <param name="value">A <see cref="Enum"/> value to get the description for.</param>
        /// <returns>A <see cref="string"/> for the description of the value.</returns>
        public static string GetDescription(this Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var attr = value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (!attr.Any())
            {
                return value.ToString();
            }

            return (attr.First() as DescriptionAttribute).Description;
        }

        /// <summary>
        /// Gets the logical operator required as part of the API query parameter.
        /// </summary>
        /// <param name="logicalOperator">The chosen Operator.</param>
        /// <returns>The logical operator string value.</returns>
        public static string GetOperator(this Operator logicalOperator)
        {
            switch (logicalOperator)
            {
                case Operator.AND: 
                    return ",";

                case Operator.OR: 
                    return "|";

                default: 
                    throw new ArgumentOutOfRangeException($"Operator {logicalOperator} does not exist.");
            }
        }
    }
}
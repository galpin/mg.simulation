// Basics.NET
//
// Copyright (c) Martin Galpin 2012.
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library. If not, see <http://www.gnu.org/licenses/>. 

using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Basics.NET
{
    /// <summary>
    /// Provides utility methods for guarding parameters. This class is <see langword="static"/>.
    /// </summary>
    [DebuggerStepThrough]
    public static class Guard
    {
        #region Public Methods

        /// <summary>
        /// Throws an <see cref="System.ArgumentNullException"/> if <paramref name="obj"/> is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="obj"/>.</typeparam>
        /// <param name="obj">The object which should not be <see langword="null"/>.</param>
        /// <param name="parameterFunc">
        ///  A function that gets the name of the parameter to check (referenced in the exception message).
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="obj"/> is <see langword="null"/>.
        /// </exception>
        public static void IsNotNull<T>(T obj, Expression<Func<T>> parameterFunc)
        {
            IsNotNull(obj, GetParameterName(parameterFunc));
        }

        /// <summary>
        /// Throws an <see cref="System.ArgumentNullException"/> if <paramref name="obj"/> is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="obj"/>.</typeparam>
        /// <param name="obj">The object which should not be <see langword="null"/>.</param>
        /// <param name="parameter">
        /// The name of the parameter to check (referenced in the exception message).
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="obj"/> is <see langword="null"/>.
        /// </exception>
        public static void IsNotNull<T>(T obj, string parameter)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(parameter);
            }
        }

        /// <summary>
        /// Throws an <see cref="System.ArgumentOutOfRangeException"/> if a specified condition is not <see langword="true"/>.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="parameterFunc">
        /// A function that gets the name of the parameter to check (referenced in the exception message).
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when <paramref name="condition"/> is not <see langword="true"/>.
        /// </exception>
        public static void IsInRange<T>(bool condition, Expression<Func<T>> parameterFunc)
        {
            IsInRange(condition, GetParameterName(parameterFunc));
        }

        /// <summary>
        /// Throws an <see cref="System.ArgumentOutOfRangeException"/> if a specified condition is not <see langword="true"/>.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="parameter">
        /// The name of the parameter to check (referenced in the exception message).
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when <paramref name="condition"/> is not <see langword="true"/>.
        /// </exception>
        public static void IsInRange(bool condition, string parameter)
        {
            if (!condition)
            {
                throw new ArgumentOutOfRangeException(parameter);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the name of the first member in the expression body.
        /// </summary>
        /// <param name="reference">The expression.</param>
        private static string GetParameterName(LambdaExpression reference)
        {
            return ((MemberExpression) reference.Body).Member.Name;
        }

        #endregion
    }
}
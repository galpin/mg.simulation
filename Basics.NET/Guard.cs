// Copyright (c) Sahara Force India Formula One Team 2012.

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
        /// A function that gets the name of the parameter to check (referenced in the exception message).
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
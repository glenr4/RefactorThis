using FluentAssertions;
using FluentAssertions.Equivalency;
using System;

namespace RefactorThis.Application.Tests
{
    public static class FluentAssertionsExtensions
    {
        /// <summary>
        /// Returns a bool instead of thowing an exception.
        /// This is useful in Mock Verify(It.Is) assertions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static bool ShouldBeEquivalentTrue<T>(this T actual, T expected)
        {
            try
            {
                actual.Should().BeEquivalentTo(expected);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a bool instead of thowing an exception.
        /// This is useful in Mock Verify(It.Is) assertions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static bool ShouldBeEquivalentTrue<T>(this T actual, T expected, Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config)
        {
            try
            {
                actual.Should().BeEquivalentTo(expected, config);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}